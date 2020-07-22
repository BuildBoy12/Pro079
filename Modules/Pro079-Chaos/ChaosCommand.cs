using Exiled.API.Features;
using Pro079Core.API;

namespace ChaosCommand
{
	internal class ChaosCommand : ICommand079
	{
		private readonly ChaosPlugin plugin;
		public ChaosCommand(ChaosPlugin plugin) => this.plugin = plugin;

		public bool Cassie => true;

		public string ExtraArguments => string.Empty;

		public string HelpInfo => plugin.Config.Translations.ChaosHelp;

		public string Command => plugin.Config.Translations.ChaosCommand;

		public string CommandReady => plugin.Config.Translations.CommandReady;

		public int CurrentCooldown { get; set; }

		public int Cooldown => plugin.Config.CommandCooldown;

		public int MinLevel => plugin.Config.CommandLevel;

		public int APCost => plugin.Config.CommandCost;	

		public string CallCommand(string[] args, Player player, CommandOutput output)
		{
			Respawning.RespawnEffectsController.PlayCassieAnnouncement(plugin.Config.BroadcastMessage, false, true);
			return Pro079Core.Pro079.Manager.CommandSuccess;
		}
	}
}
