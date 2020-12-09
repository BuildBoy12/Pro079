namespace Pro079Lockdown
{
    using Configs;
    using Exiled.API.Features;
    using Pro079.Logic;
    using System;
    using PlayerEvents = Exiled.Events.Handlers.Player;
    using ServerEvents = Exiled.Events.Handlers.Server;

    public class Pro079Lockdown : Plugin<Config>
    {
        private EventHandlers _eventHandlers;
        internal static Pro079Lockdown Singleton;
        internal Translations Translations;
        internal bool IsActive;

        public override void OnEnabled()
        {
            Singleton = this;
            _eventHandlers = new EventHandlers();
            PlayerEvents.InteractingDoor += _eventHandlers.OnInteractingDoor;
            ServerEvents.WaitingForPlayers += _eventHandlers.OnWaitingForPlayers;
            Translations = new Translations();
            if (!Manager.RegisterUltimate(new LockdownCommand()))
                OnDisabled();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            PlayerEvents.InteractingDoor -= _eventHandlers.OnInteractingDoor;
            ServerEvents.WaitingForPlayers -= _eventHandlers.OnWaitingForPlayers;
            _eventHandlers = null;
            Translations = null;
            Singleton = null;
            base.OnDisabled();
        }

        public override string Author => "Build";
        public override Version Version => new Version(4, 0, 0);
    }
}