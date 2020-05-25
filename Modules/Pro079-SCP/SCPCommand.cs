using System.Collections.Generic;
using System.Linq;
using EXILED.Extensions;
using Pro079Core.API;

namespace SCPCommand
{
	internal class SCPCommand : ICommand079
	{
		private readonly SCPPlugin plugin;
		public SCPCommand(SCPPlugin plugin)
		{
			this.plugin = plugin;
		}

		public bool OverrideDisable = false;
		public bool Disabled
		{
			get => OverrideDisable ? OverrideDisable : !plugin.enable;
			set => OverrideDisable = value;
		}

		public string Command => plugin.scpcmd;

		public string ExtraArguments => plugin.scpextrainfo;

		public string HelpInfo => plugin.scpusage;

		public bool Cassie => true;

		public int Cooldown => plugin.cooldown;

		public int MinLevel => plugin.level;

		public int APCost => plugin.cost;

		public string CommandReady => plugin.scpready;

		public int CurrentCooldown { get; set; }

		public string CallCommand(string[] args, ReferenceHub player, CommandOutput output)
		{
			if (args.Length < 2)
			{
				output.Success = false;
				return plugin.scpuse.Replace("$min", plugin.cost.ToString());
			}

			/*if (!plugin.GetConfigList("p079_scp_list").Contains(args[1]))
			{
				output.Success = false;
				return plugin.scpexist + " - " + plugin.scpuse.Replace("$min", plugin.cost.ToString());
			}*/
			string scpNum = string.Join(" ", args[1].ToCharArray());
			switch (args[1])
			{
				case "mtf":
					ReferenceHub dummy = null;
					foreach (ReferenceHub ply in Player.GetHubs())
					{
						if (ply.GetTeam() == Team.MTF)
						{
							dummy = ply;
							break;
						}
					}
					if (dummy == null)
					{
						player.SendConsoleMessage(plugin.scpnomtfleft, "red");
					}
					
					//PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement(args[0], dummy);
					break;
				case "classd":
					PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement("scp " + scpNum + " terminated by ClassD personnel", false, true);
					break;
				case "scientist":
					PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement("scp " + scpNum + " terminated by science personnel", false, true);
					break;
				case "chaos":
					PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement("scp " + scpNum + " terminated by ChaosInsurgency", false, true);
					break;
				case "tesla":
					PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement("scp " + scpNum + " Successfully Terminated by automatic security system", false, true);
					break;
				case "decont":
					PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement("scp " + scpNum + " Lost in Decontamination Sequence", false, true);
					break;
				default:
					return plugin.scpway + " .079 scp " + args[0] + " (classd/scientist/chaos/tesla/mtf/decont)";
			}
			Pro079Core.Pro079.Manager.GiveExp(player, 5 * (player.GetLevel() + 1));
			return Pro079Core.Pro079.Configs.CommandSuccess;
		}
	}
}
