namespace Pro079.Logic
{
    using API.Interfaces;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.API.Interfaces;
    using Exiled.Loader;
    using Exiled.Loader.Features.Configs;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using YamlDotNet.Core;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;
    using YamlDotNet.Serialization.NodeDeserializers;

    /// The majority of the TranslationManager partial is nabbed from <see cref="Exiled.Loader.ConfigManager"/> and <see cref="Exiled.Loader.Loader"/> because I'm still bad at this type of thing.
    public static partial class Manager
    {
        public static void LoadTranslations() => Save(Load(Read()));

        /// <summary>
        /// Gets the config serializer.
        /// </summary>
        private static ISerializer Serializer => new SerializerBuilder()
            .WithTypeInspector(inner => new CommentGatheringTypeInspector(inner))
            .WithEmissionPhaseObjectGraphVisitor(args => new CommentsObjectGraphVisitor(args.InnerVisitor))
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreFields()
            .Build();

        /// <summary>
        /// Gets the config serializer.
        /// </summary>
        private static IDeserializer Deserializer => new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .WithNodeDeserializer(inner => new ValidatingNodeDeserializer(inner),
                deserializer => deserializer.InsteadOf<ObjectNodeDeserializer>())
            .IgnoreFields()
            .IgnoreUnmatchedProperties()
            .Build();

        private static Dictionary<string, ITranslations> Load(string rawConfigs)
        {
            Log.Info("Loading Pro079 translation configs.");
            rawConfigs = Regex.Replace(rawConfigs, @"\ !.*", string.Empty)
                .Replace("!Dictionary[string,IConfig]", string.Empty);
            Dictionary<string, object> rawDeserializedConfigs =
                Deserializer.Deserialize<Dictionary<string, object>>(rawConfigs) ?? new Dictionary<string, object>();
            Dictionary<string, ITranslations> deserializedConfigs = new Dictionary<string, ITranslations>();

            if (!rawDeserializedConfigs.TryGetValue("Pro079", out object rawDeserializedConfig))
            {
                deserializedConfigs.Add("Pro079", new Translations());
            }
            else
            {
                deserializedConfigs.Add("Pro079",
                    Deserializer.Deserialize<Translations>(Serializer.Serialize(rawDeserializedConfig)));
                Pro079.Singleton.Translations.CopyProperties(deserializedConfigs["Pro079"]);
            }

            var plugins = Loader.Plugins.ToList();
            plugins.Sort(delegate(IPlugin<IConfig> plugin, IPlugin<IConfig> plugin1)
            {
                int name = string.Compare(plugin.Name, plugin1.Name, StringComparison.Ordinal);
                return name;
            });

            foreach (var plugin in plugins)
            {
                if (plugin.Prefix == Pro079.Singleton.Prefix)
                    continue;

                Tuple<Type, ITranslations> translations = CreateTranslations(plugin.Assembly);
                if (translations == null)
                    continue;

                if (!rawDeserializedConfigs.TryGetValue(plugin.Prefix, out rawDeserializedConfig))
                {
                    deserializedConfigs.Add(plugin.Prefix, translations.Item2);
                }
                else
                {
                    try
                    {
                        deserializedConfigs.Add(plugin.Prefix,
                            (ITranslations) Deserializer.Deserialize(Serializer.Serialize(rawDeserializedConfig),
                                translations.Item1));
                    }
                    catch (YamlException yamlException)
                    {
                        Log.Error(
                            $"{plugin.Name} configs could not be loaded, some of them are in a wrong format, default configs will be loaded instead! {yamlException}");
                        deserializedConfigs.Add(plugin.Prefix, translations.Item2);
                    }
                    catch (NullReferenceException)
                    {
                        Log.Error("null");
                    }
                }
            }

            return deserializedConfigs;
        }

        private static Tuple<Type, ITranslations> CreateTranslations(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes().Where(type => !type.IsAbstract))
            {
                if (type.GetInterfaces().All(x => x != typeof(ITranslations)))
                    continue;

                ITranslations translations = null;
                var constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor != null)
                {
                    translations = constructor.Invoke(null) as ITranslations;
                }
                else
                {
                    var value = Array
                        .Find(type.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public),
                            property => property.PropertyType == type)?.GetValue(null);
                    if (value != null)
                        translations = value as ITranslations;
                }

                if (translations == null)
                {
                    Log.Error(
                        $"{type.FullName} is a valid Translation class, but it cannot be loaded. It either doesn't have a public default constructor without any arguments or a static property of the {type.FullName} type!");
                    continue;
                }

                return new Tuple<Type, ITranslations>(type, translations);
            }

            return null;
        }

        private static bool Save(string configs)
        {
            try
            {
                File.WriteAllText(Pro079.Singleton.Config.TranslationsDirectory, configs ?? string.Empty);

                return true;
            }
            catch (Exception exception)
            {
                Log.Error(
                    $"An error has occurred while saving configs to {Pro079.Singleton.Config.TranslationsDirectory} path: {exception}");

                return false;
            }
        }

        private static bool Save(Dictionary<string, ITranslations> configs)
        {
            try
            {
                if (configs == null || configs.Count == 0)
                    return false;

                return Save(Serializer.Serialize(configs));
            }
            catch (YamlException yamlException)
            {
                Log.Error($"An error has occurred while serializing configs: {yamlException}");

                return false;
            }
        }

        private static string Read()
        {
            try
            {
                if (File.Exists(Pro079.Singleton.Config.TranslationsDirectory))
                    return File.ReadAllText(Pro079.Singleton.Config.TranslationsDirectory);
            }
            catch (Exception exception)
            {
                Log.Error(
                    $"An error has occurred while reading configs from {Pro079.Singleton.Config.TranslationsDirectory} path: {exception}");
            }

            return string.Empty;
        }
    }
}