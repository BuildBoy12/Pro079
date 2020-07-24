using Exiled.API.Interfaces;
using System.ComponentModel;

namespace Pro079MTF
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
		public int Cooldown { get; set; } = 60;
		public int Level { get; set; } = 2;
		public int Cost { get; set; } = 70;
		public int MaxScp { get; set; } = 5;

		public Translations Translations { get; set; } = new Translations();
	}

    public sealed class Translations
    {
		public string MtfCmd { get; set; } = "mtf";
		public string MtfUse { get; set; } = "Usage: .079 mtf (p) (5) (4), will say Papa-5 is coming and there are 4 SCP remaining - $min ap";
		public string MtfMaxScp { get; set; } = "Maximum SCPs: $max";
		public string MtfUsage { get; set; } = "<character> <number> <alive-scps>";
		public string MtfExtendedHelp { get; set; } = "Announces that a new MTF squad arrived, with your own custom number of SCPs";
		public string MtfReady { get; set; } = "<b><color=\"blue\">MTF command ready!</color></b>";
	}
}
