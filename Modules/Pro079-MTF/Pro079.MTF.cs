using EXILED;

namespace Pro079MTF
{
	public class MTFPlugin : Plugin
	{
		public bool enable = true;
		public int cooldown = 60;
		public int level = 2;
		public int cost = 70;
		public int maxscp = 5;

		public override void OnDisable()
		{
			Log.Info("Pro079 MTF disabled.");
		}
		public override void OnEnable()
		{
			ReloadConfigs();
			if (!enable)
				return;

			Pro079Core.Pro079.Manager.RegisterCommand(new MTFCommand(this));
			Log.Info("Pro079 MTF enabled");
		}
		
		public void ReloadConfigs()
		{
			enable = Config.GetBool("p079_mtf_enable", true);
			cooldown = Config.GetInt("p079_mtf_cooldown", 60);
			level = Config.GetInt("p079_mtf_level", 2);
			cost = Config.GetInt("p079_mtf_cost", 70);
			maxscp = Config.GetInt("p079_mtf_maxscp", 5);
		}

		//Language Options
		public readonly string mtfcmd = "mtf";
		public readonly string mtfuse = "Usage: .079 mtf (p) (5) (4), will say Papa-5 is coming and there are 4 SCP remaining - $min ap";
		public readonly string mtfmaxscp = "Maximum SCPs: $max";
		public readonly string mtfusage = "<character> <number> <alive-scps>";
		public readonly string mtfextendedHelp = "Announces that a new MTF squad arrived, with your own custom number of SCPs";
		public readonly string mtfready = "<b><color=\"blue\">MTF command ready!</color></b>";

		public override void OnReload()
		{
			
		}

		public override string getName => "Pro079.MTF";
	}
}