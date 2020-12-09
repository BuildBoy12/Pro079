namespace Pro079Teleport.Configs
{
    using Pro079.API.Interfaces;

    public class Translations : ITranslations
    {
        public string Command { get; set; } = "teleport";
        public string CommandReady { get; set; } = "<b>Teleport command ready</b>";
        public string Description { get; set; } = "Moves the Scp079's camera to near the closest Scp";
        public string Usage { get; set; } = ".079 teleport";
    }
}