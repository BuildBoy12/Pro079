using Exiled.API.Features;
using Pro079Core;
using Pro079Core.API;

namespace GeneratorCommand
{
	internal class GenCommand : ICommand079
	{
		public bool OverrideDisable = false;
		public bool Disabled
		{
			get => OverrideDisable || !GeneratorPlugin.ConfigRef.Config.IsEnabled;
			set => OverrideDisable = value;
		}

		public string Command => GeneratorPlugin.ConfigRef.Config.Translations.GenCmd;

		public string ExtraArguments => "[1-6]";

		public string HelpInfo => GeneratorPlugin.ConfigRef.Config.Translations.GenUse;

		public bool Cassie => true;

		public int Cooldown => GeneratorPlugin.ConfigRef.Config.CommandCooldown;

		public int MinLevel => GeneratorPlugin.ConfigRef.Config.CommandLevel;

		public int APCost => GeneratorPlugin.ConfigRef.Config.CommandCost;

		public string CommandReady => GeneratorPlugin.ConfigRef.Config.Translations.GenReady;

		public int CurrentCooldown { get; set; }

		public string CallCommand(string[] args, Player player, CommandOutput output)
		{
			int blackcost = APCost + GeneratorPlugin.ConfigRef.Config.CommandCostBlackout;
			if(args.Length == 0)
			{
				output.Success = false;
				return GeneratorPlugin.ConfigRef.Config.Translations.GenUse;
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
						if (player.Level < GeneratorPlugin.ConfigRef.Config.CommandLevelBlackout - 1)
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
					Pro079.Manager.SetOnCooldown(this, 70 + GeneratorPlugin.ConfigRef.Config.CommandBlackoutPenalty + Cooldown);
					return GeneratorPlugin.ConfigRef.Config.Translations.Gen5Msg;
				case "6":
					if (!player.IsBypassModeEnabled)
					{
						if (player.Level < GeneratorPlugin.ConfigRef.Config.CommandLevelBlackout - 1)
						{
							output.Success = false;
							return Pro079.Manager.LowLevel(GeneratorPlugin.ConfigRef.Config.CommandLevelBlackout);
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
					Pro079.Manager.SetOnCooldown(this, GeneratorPlugin.ConfigRef.Config.CommandBlackoutPenalty + Cooldown);
					return GeneratorPlugin.ConfigRef.Config.Translations.Gen5Msg;
				default:
					output.Success = false;
					return GeneratorPlugin.ConfigRef.Config.Translations.GenUse;
			}
		}
	}
}
