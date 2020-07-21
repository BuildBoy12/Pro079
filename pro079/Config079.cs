using Exiled.API.Interfaces;
using System.ComponentModel;

namespace Pro079Core
{
	public sealed class Configs : IConfig
	{
		//////////////////////////////////////////////////////
		//					CONFIG OPTIONS					//
		//////////////////////////////////////////////////////

		/// <summary>
		/// Bool to check if Pro-079 has been disabled through the config
		/// </summary>
		public bool IsEnabled { get; set; } = true;
		/// <summary>
		/// Checks if the suicide command is enabled or not
		/// </summary>
		[Description("Enables use of the suicide command.")]
		public bool SuicideCommand { get; set; } = true;

		[Description("Enables use of the tips command.")]
		public bool EnableTips { get; set; } = true;
		/// <summary>
		/// Checks if ultimates are enabled.
		/// Useful if you're developing a ultimate that registers events to avoid causing unintentional lag.
		/// </summary>
		[Description("Allows or disallows the loading of ultimates.")]
		public bool EnableUltimates { get; set; } = true;

		[Description("Minimum level to use ultimates when enabled.")]
		public int UltimateLevel { get; set; } = 4;

		[Description("Toggles the use of a cooldown on cassie commands.")]
		public bool EnableCassieCooldown { get; set; } = true;

		[Description("The cooldown in seconds for commands that use cassie.")]
		public int CassieCooldown { get; set; } = 30;

		[Description("Enables the broadcast used when a 079 spawns.")]
		public bool EnableSpawnBroadcast { get; set; } = true;

		//Nabbed method from Roger. Looked too good to not do so. Will probably give the translations their own handler in due time though, it's a bit cluttered for now but it's a simple temporary implementation.
		[Description("Core plugin translation options.")]
		public Translations Translations { get; set; } = new Translations();
	}

	public sealed class Translations
    {
		public string Disabled { get; set; } = "This command is disabled.";
		public string Level { get; set; } = "level $lvl";
		public string Energy { get; set; } = "$ap AP";
		public string LowLevel { get; set; } = "Your level is too low (you need $min)";
		public string LowMana { get; set; } = "Not enough AP (you need $min)";
		public string Success { get; set; } = "Command successfully launched";
		public string UltLaunched { get; set; } = "Ultimate successfully used.";
		public string BasicHelp { get; set; } = "<b>.079</b> - Displays this info message.";
		public string SuicideHelp { get; set; } = "Overcharges the generators to die when you're alone";
		public string UltHelp { get; set; } = "Displays info about ultimates";
		public string UltData { get; set; } = "(Cost: $cost | Cooldown: $cd)";
		public string TipsHelp { get; set; } = "Tips about SCP-079 and stuff to take into account";
		public string UltDown { get; set; } = "You must wait $cds before using ultimates again.";
		public string Cooldown { get; set; } = "You have to wait $cds before using this command again";
		public string CassieOnCooldown { get; set; } = "Wait $cds before using a command that requires CASSIE (the announcer)";
		public string NotScp079 { get; set; } = "You aren't SCP-079!";
		public string BroadcastMsg { get; set; } = "<color=#85ff4c>Press ` to open up the console and use additional commands.</color>";
		public string TipsMsg { get; set; } = @"TAB (above Caps Lock): opens up the map.\nSpacebar: switches the camera view from the normal mode to the FPS one (with the white dot).\nWASD: move to the camera the plugin says\nTo get out of the Heavy Containment Zone, go to the elevator (with TAB) and click the floor's white rectangle, or to the checkpoint and press WASD to get out\nAdditionally, this plugin provides extra commands by typing '.079' in the console";
		public string UnknownCmd { get; set; } = "Unknown command. Type \".079\" for help.";
		public string CassieReady { get; set; } = "<color=#85ff4c>Announcer (CASSIE) commands ready</color>";
		public string UltReady { get; set; } = "<color=#85ff4c>Ultimates ready</color>";
		public string CantSuicide { get; set; } = "You can't suicide when there's other SCP's remaining";
		public string UltLocked { get; set; } = "To use an ultimate, you need level $lvl";
		public string kys { get; set; } = "<color=#AA1515>Press ` and write \".079 suicide\" to kill yourself.</color>";
		public string UltUsageFirstLine { get; set; } = "Usage: .079 ultimate <name>";
		public string MiniDisabled { get; set; } = "disabled";
		public string TipsCmd { get; set; } = "tips";
		public string SuicideCmd { get; set; } = "suicide";
		public string UltCmd { get; set; } = "ultimate";
		public string Error { get; set; } = "There was an error with that command. Use another command, check the usage, or else contact the guy who made that command.";
		public string UltError { get; set; } = "Ultimate not found. Use <b>.079 ultimate</b> to check the names.";
	}
}
