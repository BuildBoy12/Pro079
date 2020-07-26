using Exiled.API.Features;
using Pro079Core.API;

namespace SCPCommand
{
	internal class SCPCommand : ICommand079
	{
		private readonly SCPPlugin plugin;
		public SCPCommand(SCPPlugin plugin) => this.plugin = plugin;

		public string Command => plugin.Config.Translations.ScpCmd;

		public string ExtraArguments => plugin.Config.Translations.ScpExtraInfo;

		public string HelpInfo => plugin.Config.Translations.ScpUsage;

		public bool Cassie => true;

		public int Cooldown => plugin.Config.Cooldown;

		public int MinLevel => plugin.Config.Level;

		public int APCost => plugin.Config.Cost;

		public string CommandReady => plugin.Config.Translations.ScpReady;

		public int CurrentCooldown { get; set; }

		public string CallCommand(string[] args, Player player, CommandOutput output)
		{
			if (args.Length < 2)
			{
				output.Success = false;
				return plugin.Config.Translations.ScpUse.Replace("$min", plugin.Config.Cost.ToString());
			}

			if (!plugin.Config.ScpList.Contains(args[0]))
			{
				output.Success = false;
				return plugin.Config.Translations.ScpExist + " - " + plugin.Config.Translations.ScpUse.Replace("$min", plugin.Config.Cost.ToString());
			}
			string scpNum = string.Join(" ", args[0].ToCharArray());
			string broadcast = "scp " + scpNum;
			switch (args[1])
			{
				case "mtf":
					Player dummy = null;
					foreach (Player ply in Player.List)
					{
						if (ply.Team == Team.MTF)
						{
							dummy = ply;
							break;
						}
					}
					if (dummy == null)
					{
						player.SendConsoleMessage(plugin.Config.Translations.ScpNoMtfLeft, "red");
						broadcast += " Unknown";
					}
					else
                    {
						if (!Respawning.NamingRules.UnitNamingRules.TryGetNamingRule(Respawning.SpawnableTeamType.NineTailedFox, out Respawning.NamingRules.UnitNamingRule unitNamingRule))
							broadcast += " Unknown";
						else
							broadcast += " CONTAINEDSUCCESSFULLY CONTAINMENTUNIT " + unitNamingRule.GetCassieUnitName(dummy.ReferenceHub.characterClassManager.CurUnitName);
					}				
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
					return plugin.Config.Translations.ScpWay + " .079 scp " + args[0] + " (classd/sci/chaos/tesla/mtf/decont)";
			}
			Respawning.RespawnEffectsController.PlayCassieAnnouncement(broadcast, false, true);
			Pro079Core.Pro079.Manager.GiveExp(player, 5 * (player.Level + 1));
			return Pro079Core.Pro079.Manager.CommandSuccess;
		}
	}
}