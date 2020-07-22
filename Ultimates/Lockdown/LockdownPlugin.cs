using Exiled.API.Features;

namespace LockdownUltimate
{
    public class LockdownPlugin : Plugin<Config>
	{
		private LockdownUltimate LockdownUltimate;

		public override void OnEnabled()
		{
			base.OnEnabled();
			LockdownUltimate = new LockdownUltimate(this);
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