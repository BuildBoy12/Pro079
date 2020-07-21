using System;
using Pro079Core;
using Pro079Core.API;
using LightContainmentZoneDecontamination;
using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace InfoCommand
{
    public class InfoCommand : ICommand079
	{
		private int LastMtfSpawn;
		private bool DeconBool;
		private float DeconTime;
		private int MinMTF;
		private int MaxMTF;

		public string Command => InfoPlugin.ConfigRef.Config.Translations.InfoCmd;

		public string ExtraArguments => string.Empty;

		public string HelpInfo => InfoPlugin.ConfigRef.Config.Translations.InfoExtraHelp;

		public bool Cassie => false;

		public int Cooldown => 0;

		public int MinLevel => 1;

		public int APCost => 5;

		public string CommandReady => string.Empty;

		public int CurrentCooldown
		{
			get => 0;
			set => _ = value;
		}

		public string CallCommand(string[] args, Player player, CommandOutput output)
		{
			output.CustomReturnColor = true;
			int level = player.IsBypassModeEnabled ? 5 : player.Level + 1;
			string humansAlive;
			string decontTime;
			string ScientistsEscaped;
			string ClassDEscaped;
			string ClassDAlive;
			string ScientistsAlive;
			string MTFAlive;
			string CiAlive;
			string estMTFtime;

			humansAlive = (RoundSummary.singleton.CountTeam(Team.CDP) + RoundSummary.singleton.CountTeam(Team.RSC) + RoundSummary.singleton.CountTeam(Team.CHI) + RoundSummary.singleton.CountTeam(Team.MTF).ToString());

			if (level < InfoPlugin.ConfigRef.Config.DecontLevel)
			{
				decontTime = '[' + Pro079.Manager.LevelString(InfoPlugin.ConfigRef.Config.DecontLevel, true) + ']';
			}
			else
			{
				if (DeconBool)
				{
					decontTime = InfoPlugin.ConfigRef.Config.Translations.DecontDisabled;
				}
				else if (Map.IsLCZDecontaminated)
				{
					decontTime = InfoPlugin.ConfigRef.Config.Translations.DecontHappened;
				}
				else
				{
					//Bs timespan logic because I got tired of it not cooperating :b
					TimeSpan auxTime = TimeSpan.FromMinutes(DeconTime) - TimeSpan.FromSeconds(DecontaminationController.GetServerTime);
                    decontTime = auxTime > TimeSpan.FromSeconds(0) ? Stylize(auxTime.ToString()) : InfoPlugin.ConfigRef.Config.Translations.DecontBug;
				}
			}
			if (level < InfoPlugin.ConfigRef.Config.EscapedLevel)
			{
				ScientistsEscaped = '[' + Pro079.Manager.LevelString(InfoPlugin.ConfigRef.Config.EscapedLevel, true) + ']';
				ClassDEscaped = '[' + Pro079.Manager.LevelString(InfoPlugin.ConfigRef.Config.EscapedLevel, true) + ']';
			}
			else
			{
				ClassDEscaped = Stylize(RoundSummary.escaped_ds.ToString("00"));
				ScientistsEscaped = Stylize(RoundSummary.escaped_scientists.ToString("00"));
			}

			if (level < InfoPlugin.ConfigRef.Config.RscCdpLevel)
			{
				ClassDAlive = '[' + Pro079.Manager.LevelString(InfoPlugin.ConfigRef.Config.RscCdpLevel, true) + ']';
				ScientistsAlive = '[' + Pro079.Manager.LevelString(InfoPlugin.ConfigRef.Config.RscCdpLevel, true) + ']';
			}
			else
			{
				ClassDAlive = Stylize(RoundSummary.singleton.CountTeam(Team.CDP).ToString("00"));
				ScientistsAlive = Stylize(RoundSummary.singleton.CountTeam(Team.RSC).ToString("00"));
			}
			if (level < InfoPlugin.ConfigRef.Config.MtfChiLevel)
			{
				MTFAlive = '[' + Pro079.Manager.LevelString(InfoPlugin.ConfigRef.Config.MtfChiLevel, true) + ']';
				CiAlive = '[' + Pro079.Manager.LevelString(InfoPlugin.ConfigRef.Config.MtfChiLevel, true) + ']';
			}
			else
			{
				MTFAlive = Stylize(RoundSummary.singleton.CountTeam(Team.MTF).ToString("00"));
				CiAlive = Stylize(RoundSummary.singleton.CountTeam(Team.CHI).ToString("00"));
			}
			if (level > InfoPlugin.ConfigRef.Config.MtfEstLevel)
			{
				if (InfoPlugin.ConfigRef.Config.MtfOp)
				{
					var cmp = PlayerManager.localPlayer.GetComponent<Respawning.RespawnManager>();
					if (cmp._timeForNextSequence > 0f)
					{
						if (InfoPlugin.ConfigRef.Config.LongTime) estMTFtime = InfoPlugin.ConfigRef.Config.Translations.MtfRespawn.Replace("$time", SecondsToTime(cmp._timeForNextSequence));
						else estMTFtime = InfoPlugin.ConfigRef.Config.Translations.MtfRespawn.Replace("$time", Stylize(cmp._timeForNextSequence.ToString("0")));
					}
					else
					{
						estMTFtime = InfoPlugin.ConfigRef.Config.Translations.MtfEst2;
					}
				}
				else
				{
					if (Round.ElapsedTime.Seconds - LastMtfSpawn < MinMTF)
					{
						if (InfoPlugin.ConfigRef.Config.LongTime) estMTFtime = InfoPlugin.ConfigRef.Config.Translations.MtfEst0.Replace("$(min)", SecondsToTime(MinMTF - Round.ElapsedTime.Seconds + LastMtfSpawn)).Replace("$(max)", SecondsToTime(MaxMTF - Round.ElapsedTime.Seconds + LastMtfSpawn));
						else estMTFtime = InfoPlugin.ConfigRef.Config.Translations.MtfEst0.Replace("$(min)", Stylize(MinMTF - Round.ElapsedTime.Seconds + LastMtfSpawn.ToString("0"))).Replace("$(max)", (MaxMTF - Round.ElapsedTime.Seconds + LastMtfSpawn).ToString("0"));
					}
					else if (Round.ElapsedTime.Seconds - LastMtfSpawn < MaxMTF)
					{
						if (InfoPlugin.ConfigRef.Config.LongTime) estMTFtime = InfoPlugin.ConfigRef.Config.Translations.MtfEst1.Replace("$(max)", SecondsToTime(MaxMTF - Round.ElapsedTime.Seconds + LastMtfSpawn));
						else estMTFtime = InfoPlugin.ConfigRef.Config.Translations.MtfEst1.Replace("$(max)", Stylize(MaxMTF - Round.ElapsedTime.Seconds + LastMtfSpawn.ToString("0")));
					}
					else
					{
						estMTFtime = InfoPlugin.ConfigRef.Config.Translations.MtfEst2;
					} 
				}
			}
			else
			{
				estMTFtime = '[' + Pro079.Manager.LevelString(InfoPlugin.ConfigRef.Config.MtfEstLevel, true) + ']';
			}
			string InfoMsg = Pro079.Manager.ReplaceAfterToken(InfoPlugin.ConfigRef.Config.Translations.InfoMsg, '$', new Tuple<string, object>[] 
			{
				new Tuple<string, object>("scpalive", humansAlive),
				new Tuple<string, object>("humans", estMTFtime),
				new Tuple<string, object>("estMTF", decontTime),
				new Tuple<string, object>("decont", ClassDEscaped),
				new Tuple<string, object>("cdesc", ScientistsEscaped),
				new Tuple<string, object>("sciesc", ScientistsEscaped),
				new Tuple<string, object>("cdalive", ClassDAlive),
				new Tuple<string, object>("cialive", CiAlive),
				new Tuple<string, object>("scialive", ScientistsAlive),
				new Tuple<string, object>("mtfalive", MTFAlive),
				new Tuple<string, object>("\\n", Environment.NewLine),
			});
			player.SendConsoleMessage(InfoMsg, "white");
			if (level >= InfoPlugin.ConfigRef.Config.GenLevel)
			{
				string ReturnMessage = InfoPlugin.ConfigRef.Config.Translations.Generators;
				foreach (var generator in Generator079.Generators)
				{
					ReturnMessage += '\n' + InfoPlugin.ConfigRef.Config.Translations.HasTablet.Replace("$room", generator.CurRoom) + ' ';
					if (generator.NetworkremainingPowerup == 0)
					{
						ReturnMessage += InfoPlugin.ConfigRef.Config.Translations.GeneratorActivated + '\n';
					}
					else
					{
						ReturnMessage += (generator.isTabletConnected ? InfoPlugin.ConfigRef.Config.Translations.HasTablet : InfoPlugin.ConfigRef.Config.Translations.NoTablet) + ' ' + InfoPlugin.ConfigRef.Config.Translations.TimeLeft.Replace("$sec", Stylize((int) generator.remainingPowerup));
					}
				}
				return "<color=white>" + ReturnMessage + "</color>";
			}
			else
			{
				return "<color=red>[" + InfoPlugin.ConfigRef.Config.Translations.LockedUntil.Replace("$lvl", Stylize(InfoPlugin.ConfigRef.Config.Translations.Generators)) + "]</color>";
			}
		}
		private string SecondsToTime(float sec)
		{
			int seconds = (int)sec % 60;
			int mins = ((int)sec - seconds) / 60;
			return (mins > 0 ? Stylize(mins.ToString()) + $" {InfoPlugin.ConfigRef.Config.Translations.Minutes.Replace("$", (mins != 1 ? InfoPlugin.ConfigRef.Config.Translations.PluralSuffix : string.Empty))}" : string.Empty)
				+ ((seconds > 0 && mins > 0) ? $" {InfoPlugin.ConfigRef.Config.Translations.And} ": string.Empty) +
				(seconds != 0 ? $"{Stylize(seconds)} <color={InfoPlugin.ConfigRef.Config.Color}>{InfoPlugin.ConfigRef.Config.Translations.Seconds.Replace("$", (seconds != 1 ? InfoPlugin.ConfigRef.Config.Translations.PluralSuffix : string.Empty))}</color>" : string.Empty);
		}
		private string Stylize(object obj)
		{
			return $"<b><color={InfoPlugin.ConfigRef.Config.Color}>{obj}</color></b>";
		}

		public void OnWaitingForPlayers()
		{
            DeconBool = GameCore.ConfigFile.ServerConfig.GetBool("disable_decontamination");
			DeconTime = GameCore.ConfigFile.ServerConfig.GetFloat("decontamination_time", 11.47f);
			MinMTF = GameCore.ConfigFile.ServerConfig.GetInt("minimum_MTF_time_to_spawn");
			MaxMTF = GameCore.ConfigFile.ServerConfig.GetInt("maximum_MTF_time_to_spawn");
		}
		public void OnTeamRespawn(RespawningTeamEventArgs ev)
		{
			LastMtfSpawn = Round.ElapsedTime.Seconds;
		}
	}
}
