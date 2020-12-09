namespace Pro079Info.Configs
{
    using Exiled.API.Interfaces;

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public int Cooldown { get; set; } = 50;
        public int Cost { get; set; } = 10;

        public int AliveLevel { get; set; } = 1;

        public int DecontLevel { get; set; } = 2;

        public int EscapedLevel { get; set; } = 2;

        public int RscCdpLevel { get; set; } = 2;

        public int MtfChiLevel { get; set; } = 3;

        public int GenLevel { get; set; } = 1;

        public int MtfEstLevel { get; set; } = 3;

        public bool MtfOp { get; set; } = true;

        public bool LongTime { get; set; } = true;

        public string Color { get; set; } = "red";
    }
}