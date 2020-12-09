namespace Pro079Generators.Configs
{
    using Exiled.API.Interfaces;
    using System.ComponentModel;

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public int Cooldown { get; set; } = 50;
        public int Cost { get; set; } = 50;

        [Description("Energy used on top of the Cost variable if a blackout occurs due to the command.")]
        public int CostBlackout { get; set; } = 40;

        public int Level { get; set; } = 2;
        public int LevelBlackout { get; set; } = 3;

        [Description("Additional cooldown for the plugin.")]
        public int BlackoutPenalty { get; set; } = 60;
    }
}