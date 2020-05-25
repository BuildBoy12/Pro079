using EXILED;
using EXILED.Patches;

namespace LockdownUltimate
{
	public class LockdownPlugin : Plugin
	{
		public LockdownUltimate LockdownUltimate;
		public override void OnDisable()
		{
			Events.DoorInteractEvent -= LockdownUltimate.OnDoorAccess;
			Events.WaitingForPlayersEvent -= LockdownUltimate.OnWaitingForPlayers;
			LockdownUltimate = null;
			Log.Info("Pro079 Lockdown disabled.");
		}
		public override void OnEnable()
		{
			LoadConfigs();
			if (!enable)
				return;

			LockdownUltimate = new LockdownUltimate(this);
			Events.DoorInteractEvent += LockdownUltimate.OnDoorAccess;
			Events.WaitingForPlayersEvent += LockdownUltimate.OnWaitingForPlayers;	
			Pro079Core.Pro079Manager.Manager.RegisterUltimate(LockdownUltimate);
			Log.Info("Pro079 Lockdown enabled");
		}

		public bool enable;
		public int time;
		public int cooldown;
		public int cost;
		public void LoadConfigs()
		{
			enable = Config.GetBool("p079_lockdown_enable", true);
			time = Config.GetInt("p079_lockdown_time", 60);
			cooldown = Config.GetInt("p079_lockdown_cooldown", 180);
			cost = Config.GetInt("p079_lockdown_cost", 50);
		}

        public readonly string lockdownInfo = "makes humans unable to open doors that require a keycard, but SCPs can open any";
        public override void OnReload()
		{

		}

		public override string getName => "Pro079.Ultimates.Lockdown";
	}
}