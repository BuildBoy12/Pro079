namespace Pro079Scp
{
    using Configs;
    using Exiled.API.Features;
    using Pro079.Logic;
    using System;

    public class Pro079Scp : Plugin<Config>
    {
        internal static Pro079Scp Singleton;
        internal Translations Translations;

        public override void OnEnabled()
        {
            Singleton = this;
            Translations = new Translations();
            if (!Manager.RegisterCommand(new ScpCommand()))
                OnDisabled();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Singleton = null;
            Translations = null;
            base.OnDisabled();
        }

        public override string Author => "Build";
        public override Version Version => new Version(4, 0, 0);
    }
}