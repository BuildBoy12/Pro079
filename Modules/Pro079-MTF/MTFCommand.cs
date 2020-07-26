using Exiled.API.Features;
using Pro079Core;
using Pro079Core.API;

namespace Pro079MTF
{
	internal class MTFCommand : ICommand079
	{
		private readonly MTFPlugin plugin;
		public MTFCommand(MTFPlugin plugin) => this.plugin = plugin;

		public string Command => plugin.Config.Translations.MtfCmd;

		public string HelpInfo => plugin.Config.Translations.MtfExtendedHelp;

		public bool Cassie => true;

		public int Cooldown => plugin.Config.Cooldown;

		public int MinLevel => plugin.Config.Level;

		public int APCost => plugin.Config.Cost;

		public string CommandReady => plugin.Config.Translations.MtfReady;

		public int CurrentCooldown { get; set; }

		public string ExtraArguments => plugin.Config.Translations.MtfUsage;

		public string CallCommand(string[] args, Player player, CommandOutput output)
		{
			if (args.Length >= 3)
			{
				if (!int.TryParse(args[2], out int scpLeft) || !int.TryParse(args[1], out int mtfNum) || !char.IsLetter(args[0][0]))
				{
					output.Success = false;
					return Command.Replace("$min", APCost.ToString());
				}
				if (scpLeft > plugin.Config.MaxScp)
				{
					output.Success = false;
					return plugin.Config.Translations.MtfUse.Replace("$min", APCost.ToString()) +
						plugin.Config.Translations.MtfMaxScp.Replace("$max", plugin.Config.Translations.MtfMaxScp.ToString());
				}
				Respawning.RespawnEffectsController.PlayCassieAnnouncement("MtfUnit epsilon 11 designated nato_" + args[0][0] + " " + mtfNum + " " + "HasEntered AllRemaining AwaitingRecontainment" + " " + scpLeft + " " + "scpsubjects", false, true);
				Pro079.Manager.GiveExp(player, 5f);
				return Pro079.Manager.CommandSuccess;
			}
			else
			{
				output.Success = false;
				return plugin.Config.Translations.MtfUse.Replace("$min", APCost.ToString());
			}
		}
	}
}