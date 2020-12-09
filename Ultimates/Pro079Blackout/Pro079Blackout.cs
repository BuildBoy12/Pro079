namespace Pro079Blackout
{
    using Configs;
    using Exiled.API.Features;
    using Pro079.Logic;
    using System;

    public class Pro079Blackout : Plugin<Config>
    {
        internal static Pro079Blackout Singleton;
        internal Translations Translations;

        public override void OnEnabled()
        {
            Singleton = this;
            Translations = new Translations();
            if (!Manager.RegisterUltimate(new BlackoutCommand()))
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