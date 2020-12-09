namespace Pro079.Configs
{
    using Exiled.API.Features;
    using Exiled.API.Interfaces;
    using System.ComponentModel;
    using System.IO;

    public sealed class Config : IConfig
    {
        public Config() =>
            TranslationsDirectory = Path.Combine(Paths.Configs, "Pro079Translations.yml");

        public bool IsEnabled { get; set; } = true;

        public string TranslationsDirectory { get; private set; }

        [Description("Enables use of the suicide command.")]
        public bool SuicideCommand { get; set; } = true;

        [Description("Enables use of the tips command.")]
        public bool EnableTips { get; set; } = true;

        [Description("Allows or disallows the loading of modules.")]
        public bool EnableModules { get; set; } = true;

        [Description("Allows or disallows the loading of ultimates.")]
        public bool EnableUltimates { get; set; } = true;

        [Description("Minimum level to use ultimates when enabled.")]
        public int UltimateLevel { get; set; } = 4;

        [Description("Toggles the use of a cooldown on cassie commands.")]
        public bool EnableCassieCooldown { get; set; } = true;

        [Description("The cooldown in seconds for commands that use cassie.")]
        public int CassieCooldown { get; set; } = 30;

        [Description("Enables the broadcast used when a 079 spawns.")]
        public bool EnableSpawnBroadcast { get; set; } = true;
    }
}