using EXILED;
using Pro079Core;

namespace InfoCommand
{
	public class InfoPlugin : Plugin
	{
		public InfoCommand InfoCommand;

		public bool enable = true;
		public int alive = 1;
		public int decont = 2;
		public int escaped = 2;
		public int plebs = 2;
		public int mtfci = 3;
		public int gens = 1;
		public int mtfest = 3;
		public bool mtfop = true;
		public bool longTime = true;
		public string color = "red";

		public override void OnDisable()
		{
			Events.TeamRespawnEvent -= InfoCommand.OnTeamRespawn;
			Events.WaitingForPlayersEvent -= InfoCommand.OnWaitingForPlayers;
			Log.Info("Pro079 Info disabled.");
		}
		public override void OnEnable()
		{
			ReloadConfigs();
			if (!enable)
				return;

			InfoCommand = new InfoCommand(this);
			Events.TeamRespawnEvent += InfoCommand.OnTeamRespawn;
			Events.WaitingForPlayersEvent += InfoCommand.OnWaitingForPlayers;
			Log.Info("evWaitin");
			Pro079.Manager.RegisterCommand(InfoCommand);
			Log.Info("register");
			Log.Info("Pro079 Info enabled.");
		}

		public void ReloadConfigs()
		{
			enable = Config.GetBool("p079_info_enable", true);
			alive = Config.GetInt("p079_info_alive", 1);
			decont = Config.GetInt("p079_info_decont", 2);
			escaped = Config.GetInt("p079_info_escaped", 2);
			plebs = Config.GetInt("p079_info_plebs", 2);
			mtfci = Config.GetInt("p079_info_mtfci", 3);
			gens = Config.GetInt("p079_info_gens", 1);
			mtfest = Config.GetInt("p079_info_mtfest", 3);
			mtfop = Config.GetBool("p079_info_mtfop", true);
			longTime = Config.GetBool("p079_info_time", true);
			color = Config.GetString("p079_info_color", "red");
		}

		//Language Options
		public readonly string infocmd = "info";
		public readonly string decontdisabled = "Decontamination is disabled";
		public readonly string deconthappened = "LCZ is decontaminated";
		public readonly string decontbug = "should have happened";
		public readonly string mtfRespawn = "in $time";
		public readonly string mtfest0 = "between $(min)s and $(max)s";
		public readonly string mtfest1 = "less than $(max)";
		public readonly string mtfest2 = "are respawning / should have already respawned";
		public readonly string infomsg = "SCP alive: $scpalive\\nHumans alive: $humans | Next MTF/Chaos: $estMTF\\nTime until decontamination: $decont\\nEscaped Class Ds:  $cdesc | Escaped scientists:    $sciesc\\nAlive Class-Ds:    $cdalive | Alive chaos:           $cialive\\nAlive scientists:  $scialive | Alive MTFs:            $mtfalive";
		public readonly string lockeduntil = "Locked until level $lvl";
		public readonly string generators = "Generators:";
		public readonly string generatorin = "$room's generator";
		public readonly string activated = "is activated.";
		public readonly string hastablet = "has a tablet";
		public readonly string notablet = "doesn't have a tablet";
		public readonly string timeleft = "and has $secs remaining";
		public readonly string infoextrahelp = "Shows stuff about the facility";
		public readonly string iminutes = "minute$";
		public readonly string iseconds = "second$";
		public readonly string iand = "and";
		// I think this thing fucks germans and others.
		public readonly string pluralSuffix = "s";

		public override void OnReload()
		{

		}

		public override string getName => "Pro079.Info";
	}
}