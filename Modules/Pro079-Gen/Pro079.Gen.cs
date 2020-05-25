using EXILED;

namespace GeneratorCommand
{
	public class GeneratorPlugin : Plugin
	{
		public bool enable;
		public int cooldown;
		public int cost;
		public int costBlackout;
		public int level;
		public int levelBlackout;
		public int penalty;

		public override void OnDisable()
		{
			Log.Info("Pro079 Generator disabled.");
		}
		public override void OnEnable()
		{
			ReloadConfigs();
			if (!enable)
				return;

			Pro079Core.Pro079.Manager.RegisterCommand(new GenCommand(this));
			Log.Info("Pro079 Generator enabled");
		}
		
		public void ReloadConfigs()
		{
			enable = Config.GetBool("p079_gen_enable", true);
			cooldown = Config.GetInt("p079_gen_cooldown", 60);
			cost = Config.GetInt("p079_gen_cost", 40);
			costBlackout = Config.GetInt("p079_gen_cost_blackout", 40);
			level = Config.GetInt("p079_gen_level", 2);
			levelBlackout = Config.GetInt("p079_gen_level_blackout", 3);
			penalty = Config.GetInt("p079_gen_penalty", 60);
		}

		//LangOptions
		public readonly string gencmd = "gen";
		public readonly string genuse = "Usage: .079 gen (1-6) - Will announce there are X generator activated, or will fake your death if you type 6. 5 generators will fake your whole recontainment process. - $min AP";
		public readonly string gen5msg = "Success. Your recontainment procedure, including when lights are turned off and a message telling you died, will be played.";
		public readonly string gen6msg = "Fake death command launched.";
		public readonly string genusage = "Announces that X generators are enabled, if it's 6 it will fake your suicide";
		public readonly string genready = "<b>Generator command ready</b>";

		public override void OnReload()
		{
					
		}

		public override string getName => "Pro079.Gen";
	}
}