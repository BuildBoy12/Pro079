namespace Pro079Blackout.Configs
{
    using Pro079.API.Interfaces;

    public class Translations : ITranslations
    {
        public string Command { get; set; } = "blackout";
        public string Description { get; set; } = "Shuts the facility down for {min} minute$";
        public string Usage { get; set; } = ".079 ultimate blackout";
    }
}