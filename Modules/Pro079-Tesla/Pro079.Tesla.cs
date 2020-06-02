using Pro079Core;
using Pro079Core.API;
using EXILED;

namespace TeslaCommand
{
	public class TeslaPlugin : Plugin
	{
		public bool enable;
		public int cost;
		public int level;
		public int remaining;

		public override void OnDisable()
		{
			Log.Info("Pro079 Tesla disabled.");
		}

		public override void OnEnable()
		{
			ReloadConfigs();
			if (!enable)
				return;

			Pro079.Manager.RegisterCommand(new TeslaCommand(this));
			Log.Info("Pro079 Tesla enabled");
		}

		public void ReloadConfigs()
		{
			enable = Config.GetBool("p079_tesla_enable", true);
			cost = Config.GetInt("p079_tesla_cost", 40);
			level = Config.GetInt("p079_tesla_level", 1);
			remaining = Config.GetInt("p079_tesla_remaining", 5);
		}
		
		//Lang Options
		public readonly string teslacmd = "te";
		public readonly string teslaExtra = "<time>";
		public readonly string teslause = "Disables all teslas for the amount of seconds you want";
		public readonly string globaltesla = "All teslas disabled.";
		public readonly string teslausage = "Usage: .079 $cmd <time>";
		public readonly string teslarem = "Teslas re-enabled in $sec seconds";
		public readonly string teslarenabled = "<color=#66F>Teslas re-enabled</color>";

		public override void OnReload()
		{
			
		}

		public override string getName => "Pro079.Tesla";
	}
	public class TeslaCommand : ICommand079
	{
		private readonly TeslaPlugin plugin;
		public TeslaCommand(TeslaPlugin teslaPlugin)
		{
			plugin = teslaPlugin;
		}

		public bool OverrideDisable = false;
		public bool Disabled
		{
			get => OverrideDisable || !plugin.enable;
			set => OverrideDisable = value;
		}

		public string Command => plugin.teslacmd;

		public string ExtraArguments => plugin.teslaExtra;

		public string HelpInfo => plugin.teslause;

		public bool Cassie => false;

		public int Cooldown => 0;

		public int MinLevel => plugin.level;

		public int APCost => plugin.cost;

		public string CommandReady => string.Empty;

		public int CurrentCooldown { get => 0; set => _ = value; }

		public string CallCommand(string[] args, ReferenceHub player, CommandOutput output)
		{
			float time;
			if (args.Length < 1 || !float.TryParse(args[1], out time))
			{
				output.Success = false;
				return plugin.teslausage.Replace("$cmd", plugin.teslacmd);
			}
			MEC.Timing.RunCoroutine(TeslaLogic.DisableTeslas(time, plugin), MEC.Segment.Update);
			Pro079.Manager.GiveExp(player, time);
			return plugin.globaltesla;
		}
	}
}