using Exiled.API.Features;
using Server = Exiled.Events.Handlers.Server;
using Player = Exiled.Events.Handlers.Player;
using System;

namespace Pro079Core
{
	public class Pro079 : Plugin<Configs>
	{
		internal static Pro079 Instance;
		public EventHandlers EventHandlers;
		private static Pro079Manager _manager;
		/// <summary>
		/// <para>Manager that contains all commands and useful functions</para>
		/// </summary>
		public static Pro079Manager Manager
		{
			get
			{
				if (_manager == null) _manager = new Pro079Manager();
				return _manager;
			}
		}

		public override void OnEnabled()
		{
			base.OnEnabled();
			EventHandlers = new EventHandlers(this);
            Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Server.SendingConsoleCommand += EventHandlers.OnCallCommand;
            Player.ChangingRole += EventHandlers.OnSetRole;
            Player.Died += EventHandlers.OnPlayerDie;
			Instance = this;
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
            Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Server.SendingConsoleCommand -= EventHandlers.OnCallCommand;
            Player.ChangingRole -= EventHandlers.OnSetRole;
            Player.Died -= EventHandlers.OnPlayerDie;
			EventHandlers = null;
		}

        public override string Name => "Pro079";
        public override string Author => "Build";
        public override Version RequiredExiledVersion => new Version(2, 0, 9);
    }
}
