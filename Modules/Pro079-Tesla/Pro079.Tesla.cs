using Exiled.API.Features;
using Pro079Core;
using Pro079Core.API;
using System;
using System.Collections.Generic;

namespace TeslaCommand
{
	public class TeslaPlugin : Plugin<Config>
	{
		private TeslaCommand TeslaCommand;
		private static readonly Lazy<TeslaPlugin> LazyInstance = new Lazy<TeslaPlugin>(() => new TeslaPlugin());
		private TeslaPlugin() { }
		public static TeslaPlugin ConfigRef => LazyInstance.Value;

		public override void OnEnabled()
		{
			base.OnEnabled();
			TeslaCommand = new TeslaCommand();
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
		public string Command => TeslaPlugin.ConfigRef.Config.Translations.TeslaCmd;

		public string ExtraArguments => TeslaPlugin.ConfigRef.Config.Translations.TeslaExtra;

		public string HelpInfo => TeslaPlugin.ConfigRef.Config.Translations.TeslaUse;

		public bool Cassie => false;

		public int Cooldown => 0;

		public int MinLevel => TeslaPlugin.ConfigRef.Config.CommandLevel;

		public int APCost => TeslaPlugin.ConfigRef.Config.CommandCost;

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
				return TeslaPlugin.ConfigRef.Config.Translations.OnCooldown.Replace("$time", TeslaLogic.time.ToString());
            }
			if (args.Length < 1 || !float.TryParse(args[0], out float time))
			{
				output.Success = false;
				return TeslaPlugin.ConfigRef.Config.Translations.TeslaUsage.Replace("$cmd", TeslaPlugin.ConfigRef.Config.Translations.TeslaCmd);
			}
			CoroutineHandles.Add(MEC.Timing.RunCoroutine(TeslaLogic.DisableTeslas(time), MEC.Segment.Update));
			CoroutineHandles.Add(MEC.Timing.RunCoroutine(TeslaLogic.TeslaTimer(time), MEC.Segment.Update));
			Pro079.Manager.GiveExp(player, time);
			return TeslaPlugin.ConfigRef.Config.Translations.GlobalTesla;
		}
	}
}