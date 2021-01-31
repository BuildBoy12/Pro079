namespace Pro079
{
    using Commands;
    using Configs;
    using Exiled.API.Features;
    using Handlers;
    using Logic;
    using MEC;
    using System;
    using System.IO;
    using PlayerEvents = Exiled.Events.Handlers.Player;
    using ServerEvents = Exiled.Events.Handlers.Server;

    public class Pro079 : Plugin<Config>
    {
        public static Pro079 Singleton;
        private PlayerHandlers _playerHandlers;
        private ServerHandlers _serverHandlers;
        public Translations Translations;

        public override void OnEnabled()
        {
            if (!File.Exists(Config.TranslationsDirectory))
                File.Create(Config.TranslationsDirectory).Close();

            Singleton = this;
            Manager.LoadTranslations();
            Translations = new Translations();
            _playerHandlers = new PlayerHandlers();
            _serverHandlers = new ServerHandlers();
            PlayerEvents.ChangingRole += _playerHandlers.OnChangingRole;
            PlayerEvents.Died += _playerHandlers.OnDied;
            ServerEvents.WaitingForPlayers += _serverHandlers.OnWaitingForPlayers;

            if (Config.SuicideCommand)
                Manager.RegisterCommand(new SuicideCommand());
            if (Config.EnableTips)
                Manager.RegisterCommand(new TipsCommand());
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            PlayerEvents.ChangingRole -= _playerHandlers.OnChangingRole;
            PlayerEvents.Died -= _playerHandlers.OnDied;
            ServerEvents.WaitingForPlayers -= _serverHandlers.OnWaitingForPlayers;
            _playerHandlers = null;
            _serverHandlers = null;
            foreach (var coroutineHandle in Manager.CoroutineHandles)
                Timing.KillCoroutines(coroutineHandle);

            Translations = null;
            Singleton = null;
            base.OnDisabled();
        }

        public override string Author { get; } = "Build";
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 29);
        public override Version Version { get; } = new Version(4, 0, 0);
    }
}