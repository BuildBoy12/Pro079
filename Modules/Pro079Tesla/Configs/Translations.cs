namespace Pro079Tesla.Configs
{
    using Pro079.API.Interfaces;

    public class Translations : ITranslations
    {
        public string Command { get; set; } = "tesla";
        public string CommandReady { get; set; } = "<b>Tesla command ready</b>";
        public string Description { get; set; } = "Disables all tesla gates for the set amount of seconds";
        public string OnCooldown { get; set; } = "Tesla gates are still disabled for $time seconds.";
        public string Usage { get; set; } = "Usage: .079 tesla <time>";
    }
}