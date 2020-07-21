using Exiled.API.Interfaces;
using System.ComponentModel;

namespace GeneratorCommand
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Cooldown (in seconds")]
        public int CommandCooldown { get; set; } = 60;

        public int CommandCost { get; set; } = 40;

        public int CommandCostBlackout { get; set; } = 40;

        public int CommandLevel { get; set; } = 2;

        public int CommandLevelBlackout { get; set; } = 3;

        public int CommandBlackoutPenalty { get; set; } = 60;

        public Translations Translations { get; set; } = new Translations();
    }

    public sealed class Translations
    {
        public string GenCmd { get; set; } = "gen";
        public string GenUse { get; set; } = "Usage: .079 gen (1-6) - Will announce there are X generator activated, or will fake your death if you type 6. 5 generators will fake your whole recontainment process.";
        public string Gen5Msg { get; set; } = "Success. Your recontainment procedure, including when lights are turned off and a message telling you died, will be played.";
        public string Gen6Msg { get; set; } = "Fake death command launched.";
        public string GenUsage { get; set; } = "Announces that X generators are enabled, if it's 6 it will fake your suicide";
        public string GenReady { get; set; } = "<b>Generator command ready</b>";
    }
}
