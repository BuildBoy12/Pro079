using Exiled.API.Interfaces;
using System.ComponentModel;

namespace LockdownUltimate
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public int Time { get; set; } = 60;

        public int Cooldown { get; set; } = 180;

        public int Cost { get; set; } = 50;

        public Translations Translations { get; set; } = new Translations();
    }

    public sealed class Translations
    {
        public string LockdownInfo { get; set; } = "makes humans unable to open doors that require a keycard, but SCPs can open any";
    }
}
