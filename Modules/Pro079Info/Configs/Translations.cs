namespace Pro079Info.Configs
{
    using Pro079.API.Interfaces;
    using System.ComponentModel;

    public class Translations : ITranslations
    {
        public string Command { get; set; } = "info";
        public string CommandReady { get; set; } = "<b>Info command ready</b>";
        public string Description { get; set; } = "";
        public string Usage { get; set; } = ".079 info";
        public string DecontDisabled { get; set; } = "Decontamination is disabled";
        public string DecontHappened { get; set; } = "LCZ is decontaminated";
        public string DecontBug { get; set; } = "should have happened";
        public string MtfRespawn { get; set; } = "in $time";
        public string MtfEst0 { get; set; } = "between $(min)s and $(max)s";
        public string MtfEst1 { get; set; } = "less than $(max)";
        public string MtfEst2 { get; set; } = "are respawning / should have already respawned";

        public string InfoMsg { get; set; } =
            "SCP alive: $scpalive\\nHumans alive: $humans | Next MTF/Chaos: $estMTF\\nTime until decontamination: $decont\\nEscaped Class Ds:  $cdesc | Escaped Scientists: $sciesc\\nAlive Class-Ds:    $cdalive | Alive Chaos:        $cialive\\nAlive Scientists:  $scialive | Alive MTFs:         $mtfalive";

        public string LockedUntil { get; set; } = "Locked until level $lvl";
        public string Generators { get; set; } = "Generators:";
        public string GeneratorActivated { get; set; } = "is activated.";
        public string HasTablet { get; set; } = "has a tablet";
        public string NoTablet { get; set; } = "doesn't have a tablet";
        public string TimeLeft { get; set; } = "and has $secs remaining";
        public string Minutes { get; set; } = "minute$";
        public string Seconds { get; set; } = "second$";
        public string And { get; set; } = "and";

        [Description("Used for variables such as \"Minute\" and \"Second\" for translation barriers.")]
        public string PluralSuffix { get; set; } = "s";
    }
}