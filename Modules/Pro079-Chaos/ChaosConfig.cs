using Exiled.API.Interfaces;
using System.ComponentModel;

namespace ChaosCommand
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Cooldown between uses of the chaos command.")]
        public int CommandCooldown { get; set; } = 50;

        [Description("Amount of AP for the command to consume.")]
        public int CommandCost { get; set; } = 50;

        [Description("Minimum level a 079 must be to use this command.")]
        public int CommandLevel { get; set; } = 2;

        [Description("C.A.S.S.I.E. announcement that will play when the command is used.")]
        public string BroadcastMessage { get; set; } = "warning . chaosinsurgency detected in the surface";

        public Translations Translations { get; set; } = new Translations();
    }

    public sealed class Translations
    {
        public string ChaosCommand { get; set; } = "chaos";

        public string ChaosHelp { get; set; } = "Fakes the chaos coming";

        public string CommandReady { get; set; } = "<b><color=\"green\">Chaos command ready</color></b>";
    }
}
