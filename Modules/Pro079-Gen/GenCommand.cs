using Exiled.API.Features;
using Pro079Core;
using Pro079Core.API;

namespace GeneratorCommand
{
	internal class GenCommand : ICommand079
	{
		private readonly GeneratorPlugin plugin;
		public GenCommand(GeneratorPlugin plugin) => this.plugin = plugin;

		public bool OverrideDisable = false;
		public bool Disabled
		{
			get => OverrideDisable || !plugin.Config.IsEnabled;
			set => OverrideDisable = value;
		}

		public string Command => plugin.Config.Translations.GenCmd;

		public string ExtraArguments => "[1-6]";

		public string HelpInfo => plugin.Config.Translations.GenUse;

		public bool Cassie => true;

		public int Cooldown => plugin.Config.Cooldown;

		public int MinLevel => plugin.Config.Level;

		public int APCost => plugin.Config.Cost;

		public string CommandReady => plugin.Config.Translations.GenReady;

		public int CurrentCooldown { get; set; }

		public string CallCommand(string[] args, Player player, CommandOutput output)
		{
			int blackcost = APCost + plugin.Config.CostBlackout;
			if(args.Length == 0)
			{
				output.Success = false;
				return plugin.Config.Translations.GenUse;
			}
			switch (args[0])
			{
				case "1":
				case "2":
				case "3":
				case "4":
					Respawning.RespawnEffectsController.PlayCassieAnnouncement("Scp079Recon" + args[0], false, true);
					Pro079.Manager.GiveExp(player, 20f);
					return Pro079.Manager.CommandSuccess;
				case "5":
					if (!player.IsBypassModeEnabled)
					{
						if (player.Level < plugin.Config.LevelBlackout - 1)
						{
							output.Success = false;
							return Pro079.Manager.LowLevel(MinLevel);
						}
						if (player.Energy < blackcost)
						{
							output.Success = false;
							return Pro079.Manager.LowAP(blackcost);
						}
						Pro079.Manager.DrainAP(player, blackcost);
					}
					MEC.Timing.RunCoroutine(Pro079Logic.Fake5Gens(), MEC.Segment.FixedUpdate);
					Pro079.Manager.GiveExp(player, 80f);
					Pro079.Manager.DrainAP(player, blackcost);
					Pro079.Manager.SetOnCooldown(this, 70 + plugin.Config.BlackoutPenalty + Cooldown);
					return plugin.Config.Translations.Gen5Msg;
				case "6":
					if (!player.IsBypassModeEnabled)
					{
						if (player.Level < plugin.Config.LevelBlackout - 1)
						{
							output.Success = false;
							return Pro079.Manager.LowLevel(plugin.Config.LevelBlackout);
						}
						if (player.Energy < blackcost)
						{
							output.Success = false;
							return Pro079.Manager.LowAP(blackcost);
						}
						Pro079.Manager.DrainAP(player, blackcost);
					}
					MEC.Timing.RunCoroutine(Pro079Logic.SixthGen(), MEC.Segment.FixedUpdate);
					Pro079.Manager.GiveExp(player, 50f);
					Pro079.Manager.DrainAP(player, blackcost);
					Pro079.Manager.SetOnCooldown(this, plugin.Config.BlackoutPenalty + Cooldown);
					return plugin.Config.Translations.Gen5Msg;
				default:
					output.Success = false;
					return plugin.Config.Translations.GenUse;
			}
		}
	}
}