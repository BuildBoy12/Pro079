using Exiled.API.Interfaces;

namespace TeslaCommand
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public int Cost { get; set; } = 40;

        public int Level { get; set; } = 1;

        public int TeslaRemaining { get; set; } = 5;

        public Translations Translations { get; set; } = new Translations();
    }

    public sealed class Translations
    {
        public string TeslaCmd { get; set; } = "te";
        public string TeslaExtra { get; set; } = "<time>";
        public string TeslaUse { get; set; } = "Disables all teslas for the amount of seconds you want";
        public string GlobalTesla { get; set; } = "All teslas disabled.";
        public string TeslaUsage { get; set; } = "Usage: .079 $cmd <time>";
        public string TeslaRem { get; set; } = "Teslas re-enabled in $sec seconds";
        public string TeslaReenabled { get; set; } = "<color=#66F>Teslas re-enabled</color>";
        public string OnCooldown { get; set; } = "Wait $time seconds to use this command.";
    }
}
