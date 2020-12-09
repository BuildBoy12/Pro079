namespace Pro079Scp
{
    using CommandSystem;
    using Exiled.API.Features;
    using Pro079.API.Interfaces;
    using System;
    using System.Linq;

    public class ScpCommand : ICommand079
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = Pro079Scp.Singleton.Translations.Usage;
            if (arguments.Count < 3)
                return false;


            if (!Pro079Scp.Singleton.Config.ScpList.Contains(arguments.At(1)))
                return false;

            Player ply = Player.Get((sender as CommandSender)?.SenderId);
            string scpNum = string.Join(" ", arguments.At(1).ToCharArray());
            string broadcast = "scp " + scpNum;

            switch (arguments.At(2))
            {
                case "mtf":
                    Player dummy = Player.List.FirstOrDefault(player => player.Team == Team.MTF);
                    if (dummy == null)
                    {
                        response = Pro079Scp.Singleton.Translations.NoMtfLeft;
                        return false;
                    }

                    if (!Respawning.NamingRules.UnitNamingRules.TryGetNamingRule(
                        Respawning.SpawnableTeamType.NineTailedFox,
                        out Respawning.NamingRules.UnitNamingRule unitNamingRule))
                        broadcast += " Unknown";
                    else
                        broadcast += " CONTAINEDSUCCESSFULLY CONTAINMENTUNIT " +
                                     unitNamingRule.GetCassieUnitName(dummy.ReferenceHub.characterClassManager
                                         .CurUnitName);

                    break;
                case "classd":
                    broadcast += " terminated by ClassD personnel";
                    break;
                case "sci":
                case "scientist":
                    broadcast += " terminated by science personnel";
                    break;
                case "chaos":
                    broadcast += " terminated by ChaosInsurgency";
                    break;
                case "tesla":
                    broadcast += " Successfully Terminated by automatic security system";
                    break;
                case "decont":
                    broadcast += " Lost in Decontamination Sequence";
                    break;
                default:
                    response = Pro079Scp.Singleton.Translations.IncorrectMethodName + " .079 scp " + arguments.At(1) +
                               " (classd/sci/chaos/tesla/mtf/decont)";
                    return false;
            }

            Exiled.API.Features.Cassie.Message(broadcast);
            response = Pro079.Pro079.Singleton.Translations.Success;
            return true;
        }

        public string Command => Pro079Scp.Singleton.Translations.Command;
        public string[] Aliases => Array.Empty<string>();
        public string Description => Pro079Scp.Singleton.Translations.Description;

        public string ExtraArguments => string.Empty;
        public bool Cassie => true;
        public int Cooldown => Pro079Scp.Singleton.Config.Cooldown;
        public int MinLevel => Pro079Scp.Singleton.Config.Level;
        public int Cost => Pro079Scp.Singleton.Config.Cost;
        public string CommandReady => Pro079Scp.Singleton.Translations.CommandReady;
    }
}