using Exiled.API.Features;
using System;

namespace LockdownUltimate
{
    public class LockdownPlugin : Plugin<Config>
	{
		private static readonly Lazy<LockdownPlugin> LazyInstance = new Lazy<LockdownPlugin>(() => new LockdownPlugin());
		private LockdownPlugin() { }
		public static LockdownPlugin ConfigRef => LazyInstance.Value;

		private LockdownUltimate LockdownUltimate;

		public override void OnEnabled()
		{
			base.OnEnabled();
			LockdownUltimate = new LockdownUltimate();
			Exiled.Events.Handlers.Player.InteractingDoor += LockdownUltimate.OnDoorAccess;
			Exiled.Events.Handlers.Server.WaitingForPlayers += LockdownUltimate.OnWaitingForPlayers;
			Pro079Core.Pro079Manager.Manager.RegisterUltimate(LockdownUltimate);	
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
			Exiled.Events.Handlers.Player.InteractingDoor -= LockdownUltimate.OnDoorAccess;
			Exiled.Events.Handlers.Server.WaitingForPlayers -= LockdownUltimate.OnWaitingForPlayers;
			LockdownUltimate = null;
		}

		public override string Name => "Pro079.Ultimates.Lockdown";
        public override string Author => "Build";
    }
}