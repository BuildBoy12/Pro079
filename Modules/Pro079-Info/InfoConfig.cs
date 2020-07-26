using Exiled.API.Interfaces;
using System.ComponentModel;

namespace InfoCommand
{
	public sealed class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		public int AliveLevel { get; set; } = 1;

		public int DecontLevel { get; set; } = 2;

		public int EscapedLevel { get; set; } = 2;

		public int RscCdpLevel { get; set; } = 2;

		public int MtfChiLevel { get; set; } = 3;

		public int GenLevel { get; set; } = 1;

		public int MtfEstLevel { get; set; } = 3;

		public bool MtfOp { get; set; } = true;

		public bool LongTime { get; set; } = true;

		public string Color { get; set; } = "red";

		public Translations Translations { get; set; } = new Translations();
	}

	public sealed class Translations
    {
		public string InfoCmd { get; set; } = "info";
		public string DecontDisabled { get; set; } = "Decontamination is disabled";
		public string DecontHappened { get; set; } = "LCZ is decontaminated";
		public string DecontBug { get; set; } = "should have happened";
		public string MtfRespawn { get; set; } = "in $time";
		public string MtfEst0 { get; set; } = "between $(min)s and $(max)s";
		public string MtfEst1 { get; set; } = "less than $(max)";
		public string MtfEst2 { get; set; } = "are respawning / should have already respawned";
		public string InfoMsg { get; set; } = "SCP alive: $scpalive\\nHumans alive: $humans | Next MTF/Chaos: $estMTF\\nTime until decontamination: $decont\\nEscaped Class Ds:  $cdesc | Escaped Scientists: $sciesc\\nAlive Class-Ds:    $cdalive | Alive Chaos:        $cialive\\nAlive Scientists:  $scialive | Alive MTFs:         $mtfalive";
		public string LockedUntil { get; set; } = "Locked until level $lvl";
		public string Generators { get; set; } = "Generators:";
		public string GeneratorRoom { get; set; } = "$room's generator";
		public string GeneratorActivated { get; set; } = "is activated.";
		public string HasTablet { get; set; } = "has a tablet";
		public string NoTablet { get; set; } = "doesn't have a tablet";
		public string TimeLeft { get; set; } = "and has $secs remaining";
		public string InfoExtraHelp { get; set; } = "Shows stuff about the facility";
		public string Minutes { get; set; } = "minute$";
		public string Seconds { get; set; } = "second$";
		public string And { get; set; } = "and";
		[Description("Used for variables such as \"Minute\" and \"Second\" for translation barriers.")]
		public string PluralSuffix { get; set; } = "s";
	}
}