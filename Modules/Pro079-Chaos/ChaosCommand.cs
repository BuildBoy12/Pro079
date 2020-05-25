using Pro079Core.API;

namespace ChaosCommand
{
	internal class ChaosCommand : ICommand079
	{
		private readonly ChaosPlugin plugin;

		public ChaosCommand(ChaosPlugin plugin)
		{
			this.plugin = plugin;
		}

		public bool OverrideDisable = false;
		public bool Disabled
		{
			get => OverrideDisable || !plugin.enable;
			set => OverrideDisable = value;
		}

		public string Command => plugin.chaoscmd;

		public string ExtraArguments => string.Empty;

		public string HelpInfo => plugin.chaoshelp;

		public bool Cassie => true;

		public int Cooldown => plugin.cooldown;

		public int MinLevel => plugin.level;

		public int APCost => plugin.cost;

		public string CommandReady => plugin.ready;

		public int CurrentCooldown { get; set; }

		public string CallCommand(string[] args, ReferenceHub player, CommandOutput output)
		{
			PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement(plugin.msg, false, true);
			return Pro079Core.Pro079.Configs.CommandSuccess;
		}
	}
}
