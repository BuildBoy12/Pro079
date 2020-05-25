using EXILED;

namespace BlackoutUltimate
{
	public class BlackoutUltimate : Plugin
	{
		BlackoutLogic BlackoutLogic;

		public bool enable;
		public int cooldown;
		public int minutes;
		public int cost;

		public override void OnDisable()
		{
			BlackoutLogic = null;
			Log.Info("Pro079 Blackout disabled.");
		}
		public override void OnEnable()
		{
			ReloadConfigs();
			if (!enable)
				return;

			BlackoutLogic = new BlackoutLogic(this);
			Pro079Core.Pro079.Manager.RegisterUltimate(BlackoutLogic);
			Log.Info("Pro079 Blackout enabled");
		}
		
		public void ReloadConfigs()
		{
			enable = Config.GetBool("p079_blackout_enable", true);
			cooldown = Config.GetInt("p079_blackout_cooldown", 180);
			minutes = Config.GetInt("p079_blackout_minutes", 1);
			cost = Config.GetInt("p079_blackout_cost", 50);
		}

		//LangOptions
		public readonly string p0blackoutinfo = "Shuts the facility down for {min} minute$";

		public override void OnReload()
		{
			
		}

		public override string getName => "Pro079.Ultimates.Blackout";
	}
}