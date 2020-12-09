namespace Pro079.Logic
{
    using API.Interfaces;
    using System;
    using System.Linq;
    using System.Text;
    using NorthwoodLib.Pools;

    public static class Methods
    {
        private static string _helpMessage;

        public static string GetHelp()
        {
            if (!string.IsNullOrEmpty(_helpMessage))
                return _helpMessage;

            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            stringBuilder.Append(FormatCommands());
            stringBuilder.Append(FormatUltimates());
            _helpMessage = stringBuilder.ToString();
            StringBuilderPool.Shared.Return(stringBuilder);
            return _helpMessage;
        }

        internal static string FormatCommands()
        {
            if (!Pro079.Singleton.Config.EnableModules)
                return string.Empty;

            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            stringBuilder.AppendLine("-- Commands --");
            foreach (var command in Manager.Commands)
            {
                stringBuilder.AppendLine("<b>.079");
                if (!string.IsNullOrEmpty(command.ExtraArguments))
                {
                    stringBuilder.Append(" ");
                    stringBuilder.Append(command.ExtraArguments);
                }

                stringBuilder.Append("</b> - ");
                stringBuilder.Append(command.Description);
                stringBuilder.Append(FormatEnergyLevel(command.Cost, command.MinLevel));
            }

            string str = stringBuilder.ToString();
            StringBuilderPool.Shared.Return(stringBuilder);
            return str;
        }

        internal static string FormatUltimates()
        {
            if (!Pro079.Singleton.Config.EnableUltimates)
                return string.Empty;

            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            stringBuilder.AppendLine("-- Ultimates --");
            foreach (var ultimate in Manager.Ultimates)
            {
                stringBuilder.AppendLine("<b>.079");
                stringBuilder.Append(Pro079.Singleton.Translations.UltCmd);
                stringBuilder.Append(" ");
                stringBuilder.Append(ultimate.Command);
                stringBuilder.Append("</b>");
                stringBuilder.Append(" - ");
                stringBuilder.Append(ultimate.Description);
                stringBuilder.Append(Pro079.Singleton.Translations.UltData);
                stringBuilder.Append(stringBuilder.ToString()
                    .ReplaceAfterToken('$', new[]
                    {
                        new Tuple<string, object>("cost", ultimate.Cost),
                        new Tuple<string, object>("cd", ultimate.Cooldown)
                    }));
            }

            string str = stringBuilder.ToString();
            StringBuilderPool.Shared.Return(stringBuilder);
            return str;
        }

        public static string FormatEnergyLevel(int energy, int level)
        {
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            if (energy > 0)
                stringBuilder.Append(Pro079.Singleton.Translations.Energy);
            if (level > 1)
                stringBuilder.Append(Pro079.Singleton.Translations.Level);

            string str = stringBuilder.ToString();
            StringBuilderPool.Shared.Return(stringBuilder);
            str.ReplaceAfterToken('$', new[]
            {
                new Tuple<string, object>("ap", energy),
                new Tuple<string, object>("lvl", level),
            });

            if (str.Length == 0)
                return string.Empty;

            stringBuilder = StringBuilderPool.Shared.Rent();
            stringBuilder.Append("(");
            stringBuilder.Append(str);
            stringBuilder.Append(")");
            StringBuilderPool.Shared.Return(stringBuilder);
            return stringBuilder.ToString();
        }

        public static ICommand079 GetCommand(string command)
        {
            return Manager.Commands.FirstOrDefault(command079 =>
                command079.Command == command || command079.Aliases.Any(alias => alias == command));
        }

        public static IUltimate079 GetUltimate(string command)
        {
            return Manager.Ultimates.FirstOrDefault(ultimate079 =>
                ultimate079.Command == command || ultimate079.Aliases.Any(alias => alias == command));
        }

        public static string LevelString(int level, bool uppercase = true)
        {
            if (uppercase || char.IsDigit(Pro079.Singleton.Translations.Level[0]))
            {
                return char.ToUpper(Pro079.Singleton.Translations.Level[0])
                       + Pro079.Singleton.Translations.Level.Substring(1).Replace("$lvl", level.ToString());
            }

            return Pro079.Singleton.Translations.Level.Replace("$lvl", level.ToString());
        }
    }
}