using EXILED;

namespace SCPCommand
{
	public class SCPPlugin : Plugin
	{
		public bool enable;
		public int cooldown;
		public int cost;
		public int level;
		// this should get changed at some point
		public string[] list = new string[] { "173", "096", "106", "049", "939" };

		public override void OnDisable()
		{
			Log.Info("Pro079 SCP command disabled.");
		}
		public override void OnEnable()
		{
			ReloadConfigs();
			if (!enable)
				return;

			Pro079Core.Pro079.Manager.RegisterCommand(new SCPCommand(this));
			Log.Info("Pro079 SCP command enabled");
		}

		public void ReloadConfigs()
		{
			enable = Config.GetBool("p079_scp_enable", true);
			cooldown = Config.GetInt("p079_scp_cooldown", 30);
			cost = Config.GetInt("p079_scp_cost", 40);
			level = Config.GetInt("p079_scp_level", 1);
			//list = Config.GetString("");
		}

		//Language Options
		public readonly string scpextrainfo = "<###> <reason>";
		public readonly string scpusage = "Fakes an SCP(173, 096...) death, the reason can be: unknown, tesla, mtf, decon";
		public readonly string scpuse = "Usage: .079 scp (173/096/106/049/939) (unknown/tesla/mtf/decont) - $min AP";
		public readonly string scpexist = "Type a SCP that exists";
		public readonly string scpway = "Type a method that exists";
		public readonly string scpnomtfleft = "No MTF's alive. Sending as \"unknown\"";
		public readonly string scpcmd = "scp";
		public readonly string scpready = "<b><color=\"red\">SCP command ready</color></b>";

		public override void OnReload()
		{
			
		}

		public override string getName => "Pro079.SCP";
	}
}