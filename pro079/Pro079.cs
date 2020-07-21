using Exiled.API.Features;
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

		private static readonly Lazy<Pro079> LazyInstance = new Lazy<Pro079>(() => new Pro079());
		private Pro079() { }
		public static Pro079 ConfigRef => LazyInstance.Value;

		public override void OnEnabled()
		{
			base.OnEnabled();
			EventHandlers = new EventHandlers(this);
			Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
			Exiled.Events.Handlers.Server.SendingConsoleCommand += EventHandlers.OnCallCommand;
			Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.OnSetRole;
			Exiled.Events.Handlers.Player.Died += EventHandlers.OnPlayerDie;
			Instance = this;
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
			Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
			Exiled.Events.Handlers.Server.SendingConsoleCommand -= EventHandlers.OnCallCommand;
			Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.OnSetRole;
			Exiled.Events.Handlers.Player.Died -= EventHandlers.OnPlayerDie;
			EventHandlers = null;
		}

        public override string Name => "Pro079";
        public override string Author => "Build"; 
    }
}
