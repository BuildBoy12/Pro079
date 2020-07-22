using Exiled.API.Features;
using Pro079Core;
using Pro079Core.API;
using System.Collections.Generic;

namespace TeslaCommand
{
    public class TeslaPlugin : Plugin<Config>
	{
		private TeslaCommand TeslaCommand;

		public override void OnEnabled()
		{
			base.OnEnabled();
			TeslaCommand = new TeslaCommand(this);
			Exiled.Events.Handlers.Server.WaitingForPlayers += TeslaCommand.OnWaitingForPlayers;
			Pro079.Manager.RegisterCommand(TeslaCommand);
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
			Exiled.Events.Handlers.Server.WaitingForPlayers -= TeslaCommand.OnWaitingForPlayers;
			TeslaCommand = null;
		}

		public override string Name => "Pro079.Tesla";
		public override string Author => "Build";
	}

	public class TeslaCommand : ICommand079
	{
		private readonly TeslaPlugin plugin;
		public TeslaCommand(TeslaPlugin plugin) => this.plugin = plugin;

		public string Command => plugin.Config.Translations.TeslaCmd;

		public string ExtraArguments => plugin.Config.Translations.TeslaExtra;

		public string HelpInfo => plugin.Config.Translations.TeslaUse;

		public bool Cassie => false;

		public int Cooldown => 0;

		public int MinLevel => plugin.Config.CommandLevel;

		public int APCost => plugin.Config.CommandCost;

		public string CommandReady => string.Empty;

		public int CurrentCooldown { get => 0; set => _ = value; }

		public List<MEC.CoroutineHandle> CoroutineHandles = new List<MEC.CoroutineHandle>();

		public void OnWaitingForPlayers()
        {
			MEC.Timing.KillCoroutines(CoroutineHandles);
			TeslaLogic.time = 0;
        }
		public string CallCommand(string[] args, Player player, CommandOutput output)
		{
			if(TeslaLogic.time > 0)
            {
				output.Success = false;
				return plugin.Config.Translations.OnCooldown.Replace("$time", TeslaLogic.time.ToString());
            }
			if (args.Length < 1 || !float.TryParse(args[0], out float time))
			{
				output.Success = false;
				return plugin.Config.Translations.TeslaUsage.Replace("$cmd", plugin.Config.Translations.TeslaCmd);
			}
			CoroutineHandles.Add(MEC.Timing.RunCoroutine(TeslaLogic.DisableTeslas(time, plugin), MEC.Segment.Update));
			CoroutineHandles.Add(MEC.Timing.RunCoroutine(TeslaLogic.TeslaTimer(time), MEC.Segment.Update));
			Pro079.Manager.GiveExp(player, time);
			return plugin.Config.Translations.GlobalTesla;
		}
	}
}