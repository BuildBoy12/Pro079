using Exiled.API.Features;
using System;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace LockdownUltimate
{
    public class LockdownPlugin : Plugin<Config>
	{
		private LockdownUltimate LockdownUltimate;

		public override void OnEnabled()
		{
			base.OnEnabled();
			LockdownUltimate = new LockdownUltimate(this);
			Player.InteractingDoor += LockdownUltimate.OnDoorAccess;
            Server.WaitingForPlayers += LockdownUltimate.OnWaitingForPlayers;
			Pro079Core.Pro079.Manager.RegisterUltimate(LockdownUltimate);	
		}

		public override void OnDisabled()
		{
            base.OnDisabled();
            Player.InteractingDoor -= LockdownUltimate.OnDoorAccess;
            Server.WaitingForPlayers -= LockdownUltimate.OnWaitingForPlayers;
			LockdownUltimate = null;
		}

		public override string Name => "Pro079.Ultimates.Lockdown";
        public override string Author => "Build";
		public override Version RequiredExiledVersion => new Version(2, 0, 9);
	}
}