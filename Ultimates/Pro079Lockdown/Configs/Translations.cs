namespace Pro079Lockdown.Configs
{
    using Pro079.API.Interfaces;

    public class Translations : ITranslations
    {
        public string Command { get; set; } = "lockdown";

        public string Description { get; set; } =
            "Makes humans unable to open doors that require a keycard, but SCPs can open any door.";

        public string Usage { get; set; } = ".079 ultimate lockdown";
    }
}