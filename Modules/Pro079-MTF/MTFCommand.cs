using Pro079Core;
using Pro079Core.API;

namespace Pro079MTF
{
	internal class MTFCommand : ICommand079
	{
		private readonly MTFPlugin plugin;
		public MTFCommand(MTFPlugin plugin)
		{
			this.plugin = plugin;
		}

		public bool OverrideDisable = false;
		public bool Disabled
		{
			get => OverrideDisable ? OverrideDisable : !plugin.enable;
			set => OverrideDisable = value;
		}

		public string Command => plugin.mtfcmd;

		public string HelpInfo => plugin.mtfextendedHelp;

		public bool Cassie => true;

		public int Cooldown => plugin.cooldown;

		public int MinLevel => plugin.level;

		public int APCost => plugin.cost;

		public string CommandReady => plugin.mtfready;

		public int CurrentCooldown { get; set; }

		public string ExtraArguments => plugin.mtfusage;

		public string CallCommand(string[] args, ReferenceHub player, CommandOutput output)
		{
			if (args.Length >= 3)
			{
				if (!int.TryParse(args[2], out int scpLeft) || !int.TryParse(args[1], out int mtfNum) || !char.IsLetter(args[0][0]))
				{
					output.Success = false;
					return plugin.mtfuse.Replace("$min", plugin.cost.ToString());
				}
				if (scpLeft > plugin.maxscp)
				{
					output.Success = false;
					return plugin.mtfuse.Replace("$min", plugin.cost.ToString()) +
						plugin.mtfmaxscp.Replace("$max", plugin.maxscp.ToString());
				}
				PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement("MtfUnit epsilon 11 designated nato_" + args[0] + " " + mtfNum + " " + "HasEntered AllRemaining AwaitingRecontainment" + " " + scpLeft + " " + "scpsubjects", false, true);
				Pro079.Manager.GiveExp(player, 5f);
				return Pro079.Configs.CommandSuccess;
			}
			else
			{
				output.Success = false;
				return plugin.mtfuse.Replace("$min", plugin.cost.ToString());
			}
		}
	}
}
