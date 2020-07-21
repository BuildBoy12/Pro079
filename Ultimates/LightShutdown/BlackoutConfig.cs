using Exiled.API.Interfaces;
using System.ComponentModel;

namespace BlackoutUltimate
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public int BlackoutCooldown { get; set; } = 180;
        public int BlackoutMinutes { get; set; } = 1;
        public int BlackoutCost { get; set; } = 50;

        [Description("Translatables")]
        public Translations Translations { get; set; } = new Translations();
    }

    public sealed class Translations
    {
        public string BlackoutInfo { get; set; } = "Shuts the facility down for {min} minute$";
    }
}
