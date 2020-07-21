using Exiled.API.Features;
using Pro079Core;
using Pro079Core.API;

namespace Pro079MTF
{
	internal class MTFCommand : ICommand079
	{
		public string Command => MTFPlugin.ConfigRef.Config.Translations.MtfCmd;

		public string HelpInfo => MTFPlugin.ConfigRef.Config.Translations.MtfExtendedHelp;

		public bool Cassie => true;

		public int Cooldown => MTFPlugin.ConfigRef.Config.CommandCooldown;

		public int MinLevel => MTFPlugin.ConfigRef.Config.CommandLevel;

		public int APCost => MTFPlugin.ConfigRef.Config.CommandCost;

		public string CommandReady => MTFPlugin.ConfigRef.Config.Translations.MtfReady;

		public int CurrentCooldown { get; set; }

		public string ExtraArguments => MTFPlugin.ConfigRef.Config.Translations.MtfUsage;

		public string CallCommand(string[] args, Player player, CommandOutput output)
		{
			if (args.Length >= 3)
			{
				if (!int.TryParse(args[2], out int scpLeft) || !int.TryParse(args[1], out int mtfNum) || !char.IsLetter(args[0][0]))
				{
					output.Success = false;
					return Command.Replace("$min", APCost.ToString());
				}
				if (scpLeft > MTFPlugin.ConfigRef.Config.MaxScp)
				{
					output.Success = false;
					return MTFPlugin.ConfigRef.Config.Translations.MtfUse.Replace("$min", APCost.ToString()) +
						MTFPlugin.ConfigRef.Config.Translations.MtfMaxScp.Replace("$max", MTFPlugin.ConfigRef.Config.Translations.MtfMaxScp.ToString());
				}
				Respawning.RespawnEffectsController.PlayCassieAnnouncement("MtfUnit epsilon 11 designated nato_" + args[0] + " " + mtfNum + " " + "HasEntered AllRemaining AwaitingRecontainment" + " " + scpLeft + " " + "scpsubjects", false, true);
				Pro079.Manager.GiveExp(player, 5f);
				return Pro079.Manager.CommandSuccess;
			}
			else
			{
				output.Success = false;
				return MTFPlugin.ConfigRef.Config.Translations.MtfUse.Replace("$min", APCost.ToString());
			}
		}
	}
}
