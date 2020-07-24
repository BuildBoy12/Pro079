using Exiled.API.Interfaces;

namespace BlackoutUltimate
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public int Cooldown { get; set; } = 180;
        public int Minutes { get; set; } = 1;
        public int Cost { get; set; } = 50;

        public Translations Translations { get; set; } = new Translations();
    }

    public sealed class Translations
    {
        public string BlackoutInfo { get; set; } = "Shuts the facility down for {min} minute$";
    }
}
