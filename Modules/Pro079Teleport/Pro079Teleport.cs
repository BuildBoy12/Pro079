namespace Pro079Teleport
{
    using Configs;
    using Exiled.API.Features;
    using Pro079.Logic;
    using System;

    public class Pro079Teleport : Plugin<Config>
    {
        internal static Pro079Teleport Singleton;
        internal Translations Translations;

        public override void OnEnabled()
        {
            Singleton = this;
            Translations = new Translations();
            if (!Manager.RegisterCommand(new TeleportCommand()))
                OnDisabled();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Translations = null;
            Singleton = null;
            base.OnDisabled();
        }

        public override string Author => "Build";
        public override Version Version => new Version(4, 0, 0);
    }
}