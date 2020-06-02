using EXILED;

namespace Pro079Core
{
	public class Pro079 : Plugin
	{
		public EventHandlers EventHandlers;
		private static Pro079Manager _manager;
		/// <summary>
		/// <para>Manager that contains all commands and useful functions</para>
		/// </summary>
		public static Pro079Manager Manager
		{
			get
			{
				if (_manager == null) _manager = new Pro079Manager(Instance);
				return _manager;
			}
		}
		private static Configs _configs;
		/// <summary>
		/// User defined configurations and language options
		/// </summary>
		public static Configs Configs
		{
			get
			{
				if (_configs == null) _configs = new Configs(Instance);
				return _configs;
			}
		}
		// Config options //

		// Public options
		public bool enable;
		public bool suicide;
		public bool ult;
		public bool tips;
		public void PublicConfigs()
		{
			enable = Config.GetBool("p079_enable", true);
			suicide = Config.GetBool("p079_suicide", true);
			ult = Config.GetBool("p079_ult", true);
			tips = Config.GetBool("p079_tips", true);
		}

		// Core-only options
		public int cassieCooldown;
		public bool spawnBroadcast;
		public int ultLevel;

		public void CoreConfigs()
		{
			cassieCooldown = Config.GetInt("p079_cassie_cooldown", 30);
			spawnBroadcast = Config.GetBool("p079_spawn_broadcast", true);
			ultLevel = Config.GetInt("p079_ult_level", 4);
		}

		// Language options //

		// Public translations
		public readonly string disabled = "This command is disabled.";
		public readonly string level = "level $lvl";
		public readonly string energy = "$ap AP";
		public readonly string lowlevel = "Your level is too low (you need $min)";
		public readonly string lowmana = "Not enough AP (you need $min)";
		public readonly string success = "Command successfully launched";
		public readonly string ultlaunched = "Ultimate successfully used.";

		// Core-only
		public static string basicHelp = "<b>.079</b> - Displays this info message.";
		public readonly string suicidehelp = "Overcharges the generators to die when you're alone";
		public readonly string ulthelp = "Displays info about ultimates";
		public readonly string ultdata = "(Cost: $cost | Cooldown: $cd)";
		public readonly string tipshelp = "Tips about SCP-079 and stuff to take into account";
		public readonly string ultdown = "You must wait $cds before using ultimates again.";
		public readonly string cooldown = "You have to wait $cds before using this command again";
		public readonly string cassieOnCooldown = "Wait $cds before using a command that requires CASSIE (the announcer)";
		public readonly string notscp079 = "You aren't SCP-079!";
		public static string broadcastMsg = "<color=#85ff4c>Press ` to open up the console and use additional commands.</color>";
		//tips
		public readonly string tipsMsg = @"TAB (above Caps Lock): opens up the map.\nSpacebar: switches the camera view from the normal mode to the FPS one (with the white dot).\nWASD: move to the camera the plugin says\nTo get out of the Heavy Containment Zone, go to the elevetor (with TAB) and click the floor's white rectangle, or to the checkpoint and press WASD to get out\nAdditionally, this plugins provide extra commands by typing .079 in the console";
		//
		public readonly string unknowncmd = "Unknown command. Type \".079\" for help.";
		public readonly string cassieready = "<color=#85ff4c>Announcer (CASSIE) commands ready</color>";
		public readonly string ultready = "<color=#85ff4c>Ultimates ready</color>";
		public readonly string cantsuicide = "You can't suicide when there's other SCP's remaining";
		public readonly string ultlocked = "To use an ultimate, you need level $lvl";
		public readonly string kys = "<color=#AA1515>Press ` and write \".079 suicide\" to kill yourself.</color>";
		public readonly string ultusageFirstline = "Usage: .079 ultimate <name>";
		public readonly string minidisabled = "disabled";
		// Core cmds translations
		public readonly string tipscmd = "tips";
		public readonly string suicidecmd = "suicide";
		public readonly string ultcmd = "ultimate";
		public readonly string error = "There was an error with that command. Use another command, check the usage, or else contact the guy who made that command.";
		public readonly string ulterror = "Ultimate not found. Use <b>.079 ultimate</b> to check the names.";

		public override void OnDisable()
		{
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			Events.ConsoleCommandEvent -= EventHandlers.OnCallCommand;
			Events.SetClassEvent -= EventHandlers.OnSetRole;
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDie;
			EventHandlers = null;
			Log.Info("Pro079 Core disabled.");
		}

		public override void OnEnable()
		{
			PublicConfigs();
			if (!enable)
				return;

			CoreConfigs();
			EventHandlers = new EventHandlers(this);
			Events.WaitingForPlayersEvent += EventHandlers.OnWaitingForPlayers;
			Events.ConsoleCommandEvent += EventHandlers.OnCallCommand;
			Events.SetClassEvent += EventHandlers.OnSetRole;
			Events.PlayerDeathEvent += EventHandlers.OnPlayerDie;
			// Info string to ASSERT DOMINANCE. (let's just delete it since it only works in windows XD)
			Log.Info("Pro079 Core enabled.");//\n      ╔═══╗╔═══╗╔═══╗╔═══╗╔═══╗╔═══╗\n      ║╔═╗║║╔═╗║║╔═╗║║╔═╗║║╔═╗║║╔═╗║\n      ║╚═╝║║╚═╝║║║─║║║║║║║╚╝╔╝║║╚═╝║\n      ║╔══╝║╔╗╔╝║║─║║║║║║║──║╔╝╚══╗║\n      ║║───║║║╚╗║╚═╝║║╚═╝║──║║─╔══╝║\n      ╚╝───╚╝╚═╝╚═══╝╚═══╝──╚╝─╚═══╝");
			Instance = this;
		}

		public override void OnReload()
		{

		}

		public override string getName => "Pro079";

		internal static Pro079 Instance;
	}
}
