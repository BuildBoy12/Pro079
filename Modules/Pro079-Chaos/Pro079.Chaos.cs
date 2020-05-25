using EXILED;
using Pro079Core;

namespace ChaosCommand
{
	public class ChaosPlugin : Plugin
	{
		public bool enable;
		public int cooldown;
		public int cost;
		public int level;
		public string msg;

		public override void OnDisable()
		{
			Log.Info("Pro079 Chaos disabled.");
		}
		public override void OnEnable()
		{
			ReloadConfigs();
			if (!enable)
				return;

			Pro079.Manager.RegisterCommand(new ChaosCommand(this));
			Log.Info("Pro079 Chaos enabled");		
		}

		public void ReloadConfigs()
		{
			enable = Config.GetBool("p079_chaos_enable", true);
			cooldown = Config.GetInt("p079_chaos_cooldown", 50);
			cost = Config.GetInt("p079_chaos_cost", 50);
			level = Config.GetInt("p079_chaos_level", 2);
			msg = Config.GetString("p079_chaos_msg", "warning . chaosinsurgency detected in the surface");
		}

		//LangOptions
		public readonly string chaoscmd = "chaos";
		public readonly string chaoshelp = "Fakes the chaos coming";
		public readonly string ready = "<b><color=\"green\">Chaos command ready</color></b>";

		public override void OnReload()
		{

		}

		public override string getName => "Pro079.Chaos";
	}
}