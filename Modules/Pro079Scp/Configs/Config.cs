namespace Pro079Scp.Configs
{
    using Exiled.API.Interfaces;
    using System.Collections.Generic;

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public int Cooldown { get; set; } = 50;
        public int Cost { get; set; } = 50;
        public int Level { get; set; } = 2;

        public List<string> ScpList { get; set; } =
            new List<string> {"173", "096", "106", "049", "939"};
    }
}