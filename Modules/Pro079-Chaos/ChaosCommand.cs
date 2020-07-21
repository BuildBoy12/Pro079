using Exiled.API.Features;
using Pro079Core.API;

namespace ChaosCommand
{
	internal class ChaosCommand : ICommand079
	{
		public bool Cassie => true;

		public string ExtraArguments => string.Empty;

		public string HelpInfo => ChaosPlugin.ConfigRef.Config.Translations.ChaosHelp;

		public string Command => ChaosPlugin.ConfigRef.Config.Translations.ChaosCommand;

		public string CommandReady => ChaosPlugin.ConfigRef.Config.Translations.CommandReady;

		public int CurrentCooldown { get; set; }

		public int Cooldown => ChaosPlugin.ConfigRef.Config.CommandCooldown;

		public int MinLevel => ChaosPlugin.ConfigRef.Config.CommandLevel;

		public int APCost => ChaosPlugin.ConfigRef.Config.CommandCost;	

		public string CallCommand(string[] args, Player player, CommandOutput output)
		{
			Respawning.RespawnEffectsController.PlayCassieAnnouncement(ChaosPlugin.ConfigRef.Config.BroadcastMessage, false, true);
			return Pro079Core.Pro079.Manager.CommandSuccess;
		}
	}
}
