namespace Pro079Blackout.Configs
{
    using Exiled.API.Interfaces;

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public int Cooldown { get; set; } = 180;
        public int BlackoutDuration { get; set; } = 60;
        public int Cost { get; set; } = 50;
    }
}