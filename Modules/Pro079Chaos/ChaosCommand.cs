namespace Pro079Chaos
{
    using CommandSystem;
    using Pro079.API.Interfaces;
    using System;
    using static Pro079Chaos;

    public class ChaosCommand : ICommand079
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Exiled.API.Features.Cassie.Message(Singleton.Config.BroadcastMessage);
            response = Pro079.Pro079.Singleton.Translations.Success;
            return true;
        }

        public string Command => Singleton.Translations.Command;
        public string[] Aliases => Array.Empty<string>();
        public string Description => Singleton.Translations.Description;

        public string ExtraArguments => string.Empty;
        public bool Cassie => true;
        public int Cooldown => Singleton.Config.Cooldown;
        public int MinLevel => Singleton.Config.Level;
        public int Cost => Singleton.Config.Cost;
        public string CommandReady => Singleton.Translations.CommandReady;
    }
}