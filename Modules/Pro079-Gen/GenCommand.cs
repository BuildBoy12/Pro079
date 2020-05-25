using EXILED.Extensions;
using Pro079Core;
using Pro079Core.API;

namespace GeneratorCommand
{
	internal class GenCommand : ICommand079
	{
		private readonly GeneratorPlugin plugin;
		public GenCommand(GeneratorPlugin plugin)
		{
			this.plugin = plugin;
		}

		public bool OverrideDisable = false;
		public bool Disabled
		{
			get => OverrideDisable || !plugin.enable;
			set => OverrideDisable = value;
		}

		public string Command => plugin.gencmd;

		public string ExtraArguments => "[1-6]";

		public string HelpInfo => plugin.genusage;

		public bool Cassie => true;

		public int Cooldown => plugin.cooldown;

		public int MinLevel => plugin.level;

		public int APCost => plugin.cost;

		public string CommandReady => plugin.genready;

		public int CurrentCooldown { get; set; }

		public string CallCommand(string[] args, ReferenceHub player, CommandOutput output)
		{
			int blackcost = plugin.cost + plugin.costBlackout;
			if(args.Length == 0)
			{
				output.Success = false;
				return plugin.genuse;
			}
			switch (args[0])
			{
				case "1":
				case "2":
				case "3":
				case "4":
					PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement("Scp079Recon" + args[0], false, true);
					Pro079.Manager.GiveExp(player, 20f);
					return Pro079.Configs.CommandSuccess;
				case "5":
					if (!player.GetBypassMode())
					{
						if (player.GetLevel() < plugin.levelBlackout - 1)
						{
							output.Success = false;
							return Pro079.Configs.LowLevel(plugin.levelBlackout);
						}
						if (player.GetEnergy() < blackcost)
						{
							output.Success = false;
							return Pro079.Configs.LowAP(blackcost);
						}
						Pro079.Manager.DrainAP(player, plugin.costBlackout);
					}
					MEC.Timing.RunCoroutine(Pro079Logic.Fake5Gens(), MEC.Segment.FixedUpdate);
					Pro079.Manager.GiveExp(player, 80f);
					Pro079.Manager.DrainAP(player, blackcost);
					Pro079.Manager.SetOnCooldown(this, 70 + plugin.penalty + plugin.cooldown);
					return plugin.gen5msg;
				case "6":
					if (!player.GetBypassMode())
					{
						if (player.GetLevel() < plugin.levelBlackout - 1)
						{
							output.Success = false;
							return Pro079.Configs.LowLevel(plugin.levelBlackout);
						}
						if (player.GetEnergy() < blackcost)
						{
							output.Success = false;
							return Pro079.Configs.LowAP(blackcost);
						}
						Pro079.Manager.DrainAP(player, plugin.costBlackout);
					}
					MEC.Timing.RunCoroutine(Pro079Logic.SixthGen(), MEC.Segment.FixedUpdate);
					Pro079.Manager.GiveExp(player, 50f);
					Pro079.Manager.DrainAP(player, blackcost);
					Pro079.Manager.SetOnCooldown(this, plugin.penalty + plugin.cooldown);
					return plugin.gen5msg;
				default:
					output.Success = false;
					return plugin.genuse;
			}
		}
	}
}
