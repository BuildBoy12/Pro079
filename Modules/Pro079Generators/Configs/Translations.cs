namespace Pro079Generators.Configs
{
    using Pro079.API.Interfaces;

    public class Translations : ITranslations
    {
        public string Command { get; set; } = "gen";
        public string CommandReady { get; set; } = "<b>Generator command ready</b>";
        public string Description { get; set; } = "Fakes the specified generator activation sequence.";
        public string Usage { get; set; }
    }
}