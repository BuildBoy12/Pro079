using Exiled.API.Interfaces;
using System.ComponentModel;

namespace LockdownUltimate
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public int LockdownTime { get; set; } = 60;

        public int LockdownCooldown { get; set; } = 180;

        public int LockdownCost { get; set; } = 50;

        [Description("Translatables")]
        public Translations Translations { get; set; } = new Translations();
    }

    public sealed class Translations
    {
        public string LockdownInfo { get; set; } = "makes humans unable to open doors that require a keycard, but SCPs can open any";
    }
}
