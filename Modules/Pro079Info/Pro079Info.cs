namespace Pro079Info
{
    using Configs;
    using Exiled.API.Features;
    using Pro079.Logic;
    using System;

    public class Pro079Info : Plugin<Config>
    {
        private InfoCommand _infoCommand;
        internal static Pro079Info Singleton;
        internal Translations Translations;

        public override void OnEnabled()
        {
            Singleton = this;
            Translations = new Translations();
            _infoCommand = new InfoCommand();
            Exiled.Events.Handlers.Server.RespawningTeam += _infoCommand.OnRespawningTeam;
            if (!Manager.RegisterCommand(_infoCommand))
                OnDisabled();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RespawningTeam -= _infoCommand.OnRespawningTeam;
            _infoCommand = null;
            Singleton = null;
            Translations = null;
            base.OnDisabled();
        }

        public override string Author => "Build";
        public override Version Version => new Version(4, 0, 0);
    }
}