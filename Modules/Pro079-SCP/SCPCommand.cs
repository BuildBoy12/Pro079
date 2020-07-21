using Exiled.API.Features;
using Pro079Core.API;

namespace SCPCommand
{
	internal class SCPCommand : ICommand079
	{
		public string Command => SCPPlugin.ConfigRef.Config.Translations.ScpCmd;

		public string ExtraArguments => SCPPlugin.ConfigRef.Config.Translations.ScpExtraInfo;

		public string HelpInfo => SCPPlugin.ConfigRef.Config.Translations.ScpUsage;

		public bool Cassie => true;

		public int Cooldown => SCPPlugin.ConfigRef.Config.CommandCooldown;

		public int MinLevel => SCPPlugin.ConfigRef.Config.CommandLevel;

		public int APCost => SCPPlugin.ConfigRef.Config.CommandCost;

		public string CommandReady => SCPPlugin.ConfigRef.Config.Translations.ScpReady;

		public int CurrentCooldown { get; set; }

		public string CallCommand(string[] args, Player player, CommandOutput output)
		{
			if (args.Length < 2)
			{
				output.Success = false;
				return SCPPlugin.ConfigRef.Config.Translations.ScpUse.Replace("$min", SCPPlugin.ConfigRef.Config.CommandCost.ToString());
			}

			if (!SCPPlugin.ConfigRef.Config.ScpList.Contains(args[0]))
			{
				output.Success = false;
				return SCPPlugin.ConfigRef.Config.Translations.ScpExist + " - " + SCPPlugin.ConfigRef.Config.Translations.ScpUse.Replace("$min", SCPPlugin.ConfigRef.Config.CommandCost.ToString());
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
						player.SendConsoleMessage(SCPPlugin.ConfigRef.Config.Translations.ScpNoMtfLeft, "red");
					}
					
					//PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement(args[0], dummy);
					break;
				case "classd":
					broadcast += " terminated by ClassD personnel";
					break;
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
					return SCPPlugin.ConfigRef.Config.Translations.ScpWay + " .079 scp " + args[0] + " (classd/scientist/chaos/tesla/mtf/decont)";
			}
			Respawning.RespawnEffectsController.PlayCassieAnnouncement(broadcast, false, true);
			Pro079Core.Pro079.Manager.GiveExp(player, 5 * (player.Level + 1));
			return Pro079Core.Pro079.Manager.CommandSuccess;
		}
	}
}
