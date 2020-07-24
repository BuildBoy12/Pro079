using Exiled.API.Interfaces;

namespace ChaosCommand
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public int Cooldown { get; set; } = 50;

        public int Cost { get; set; } = 50;

        public int Level { get; set; } = 2;

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
