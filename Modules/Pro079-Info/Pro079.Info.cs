using System;
using Exiled.API.Features;
using Pro079Core;

namespace InfoCommand
{
	public class InfoPlugin : Plugin<Config>
	{
		private InfoCommand InfoCommand;

		private static readonly Lazy<InfoPlugin> LazyInstance = new Lazy<InfoPlugin>(() => new InfoPlugin());
		private InfoPlugin() { }
		public static InfoPlugin ConfigRef => LazyInstance.Value;

		public override void OnEnabled()
		{
			base.OnEnabled();
			InfoCommand = new InfoCommand();
			Exiled.Events.Handlers.Server.RespawningTeam += InfoCommand.OnTeamRespawn;
			Exiled.Events.Handlers.Server.WaitingForPlayers += InfoCommand.OnWaitingForPlayers;
			Pro079.Manager.RegisterCommand(InfoCommand);
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
			Exiled.Events.Handlers.Server.RespawningTeam -= InfoCommand.OnTeamRespawn;
			Exiled.Events.Handlers.Server.WaitingForPlayers -= InfoCommand.OnWaitingForPlayers;
			InfoCommand = null;
		}
		
		public override string Name => "Pro079.Info";
		public override string Author => "Build";
	}
}