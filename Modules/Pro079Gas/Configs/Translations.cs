namespace Pro079Gas.Configs
{
    using Pro079.API.Interfaces;

    public class Translations : ITranslations
    {
        public string Command { get; set; } = "gas";
        public string CommandReady { get; set; } = "<b>Gas command ready</b>";
        public string Description { get; set; } = "Gasses all players in the room Scp079 is looking in";
        public string Usage { get; set; } = ".079 gas";
        public string NoPlayersFound { get; set; } = "There are no humans in the current room to gas.";
    }
}