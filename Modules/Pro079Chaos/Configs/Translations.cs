namespace Pro079Chaos.Configs
{
    using Pro079.API.Interfaces;

    public class Translations : ITranslations
    {
        public string Command { get; set; } = "chaos";
        public string Description { get; set; } = "Fakes the chaos announcement";
        public string Usage { get; set; }
        public string CommandReady { get; set; } = "<b><color=\"green\">Chaos command ready</color></b>";
    }
}