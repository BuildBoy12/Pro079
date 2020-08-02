using Exiled.API.Features;
using Pro079Core;
using Server = Exiled.Events.Handlers.Server;

namespace InfoCommand
{
    public class InfoPlugin : Plugin<Config>
	{
		private InfoCommand InfoCommand;

		public override void OnEnabled()
		{
			base.OnEnabled();
			InfoCommand = new InfoCommand(this);
            Server.RespawningTeam += InfoCommand.OnTeamRespawn;
            Server.WaitingForPlayers += InfoCommand.OnWaitingForPlayers;
			Pro079.Manager.RegisterCommand(InfoCommand);
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
            Server.RespawningTeam -= InfoCommand.OnTeamRespawn;
            Server.WaitingForPlayers -= InfoCommand.OnWaitingForPlayers;
			InfoCommand = null;
		}
		
		public override string Name => "Pro079.Info";
		public override string Author => "Build";
	}
}