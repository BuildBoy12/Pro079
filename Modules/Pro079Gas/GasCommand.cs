namespace Pro079Gas
{
    using CommandSystem;
    using Exiled.API.Features;
    using Pro079.API.Interfaces;
    using System;
    using System.Linq;
    using static Pro079Gas;

    public class GasCommand : ICommand079
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get((sender as CommandSender)?.SenderId);
            Room room = Map.FindParentRoom(ply.GameObject);
            if (!room.Players.Any() || room.Players.All(player => player.Team == Team.SCP))
            {
                response = Pro079Gas.Singleton.Translations.NoPlayersFound;
                return false;
            }

            foreach (Player player in room.Players.Where(player => player.Team != Team.SCP))
            {
                player.EnableEffect<CustomPlayerEffects.Decontaminating>(3f);
            }

            response = Pro079.Pro079.Singleton.Translations.Success;
            return true;
        }

        public string Command => Singleton.Translations.Command;
        public string[] Aliases => Array.Empty<string>();
        public string Description => Singleton.Translations.Description;

        public string ExtraArguments => string.Empty;
        public bool Cassie => false;
        public int Cooldown => Singleton.Config.Cooldown;
        public int MinLevel => Singleton.Config.Level;
        public int Cost => Singleton.Config.Cost;
        public string CommandReady => Singleton.Translations.CommandReady;
    }
}