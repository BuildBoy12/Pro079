using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace SCPCommand
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
		public int CommandCooldown { get; set; } = 30;
		public int CommandCost { get; set; } = 40;
		public int CommandLevel { get; set; } = 1;
		public List<string> ScpList { get; set; } = 
		   new List<string> { "173", "096", "106", "049", "939" };

		[Description("Translatables")]
        public Translations Translations { get; set; } = new Translations();
    }

    public sealed class Translations
    {
		public string ScpExtraInfo { get; set; } = "<###> <reason>";
		public string ScpUsage { get; set; } = "Fakes an SCP(173, 096...) death, the reason can be: classd, scientists, chaos, tesla, decont";
		public string ScpUse { get; set; } = "Usage: .079 scp (173/096/106/049/939) (classd/scientist/tesla/chaos/decont) - $min AP";
		public string ScpExist { get; set; } = "Type a SCP that exists";
		public string ScpWay { get; set; } = "Type a method that exists";
		public string ScpNoMtfLeft { get; set; } = "No MTF's alive. Sending as \"unknown\"";
		public string ScpCmd { get; set; } = "scp";
		public string ScpReady { get; set; } = "<b><color=\"red\">SCP command ready</color></b>";
	}
}
