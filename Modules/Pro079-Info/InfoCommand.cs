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
		private readonly InfoPlugin plugin;
		public InfoCommand(InfoPlugin plugin) => this.plugin = plugin;

		private int LastMtfSpawn;
		private bool DeconBool;
		private float DeconTime;
		private int MinMTF;
		private int MaxMTF;

		public string Command => plugin.Config.Translations.InfoCmd;

		public string ExtraArguments => string.Empty;

		public string HelpInfo => plugin.Config.Translations.InfoExtraHelp;

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

			if (level < plugin.Config.DecontLevel)
			{
				decontTime = '[' + Pro079.Manager.LevelString(plugin.Config.DecontLevel, true) + ']';
			}
			else
			{
				if (DeconBool)
				{
					decontTime = plugin.Config.Translations.DecontDisabled;
				}
				else if (Map.IsLCZDecontaminated)
				{
					decontTime = plugin.Config.Translations.DecontHappened;
				}
				else
				{
					//Bs timespan logic because I got tired of it not cooperating :b
					TimeSpan auxTime = TimeSpan.FromMinutes(DeconTime) - TimeSpan.FromSeconds(DecontaminationController.GetServerTime);
                    decontTime = auxTime > TimeSpan.FromSeconds(0) ? Stylize(auxTime.ToString()) : plugin.Config.Translations.DecontBug;
				}
			}
			if (level < plugin.Config.EscapedLevel)
			{
				ScientistsEscaped = '[' + Pro079.Manager.LevelString(plugin.Config.EscapedLevel, true) + ']';
				ClassDEscaped = '[' + Pro079.Manager.LevelString(plugin.Config.EscapedLevel, true) + ']';
			}
			else
			{
				ClassDEscaped = Stylize(RoundSummary.escaped_ds.ToString("00"));
				ScientistsEscaped = Stylize(RoundSummary.escaped_scientists.ToString("00"));
			}

			if (level < plugin.Config.RscCdpLevel)
			{
				ClassDAlive = '[' + Pro079.Manager.LevelString(plugin.Config.RscCdpLevel, true) + ']';
				ScientistsAlive = '[' + Pro079.Manager.LevelString(plugin.Config.RscCdpLevel, true) + ']';
			}
			else
			{
				ClassDAlive = Stylize(RoundSummary.singleton.CountTeam(Team.CDP).ToString("00"));
				ScientistsAlive = Stylize(RoundSummary.singleton.CountTeam(Team.RSC).ToString("00"));
			}
			if (level < plugin.Config.MtfChiLevel)
			{
				MTFAlive = '[' + Pro079.Manager.LevelString(plugin.Config.MtfChiLevel, true) + ']';
				CiAlive = '[' + Pro079.Manager.LevelString(plugin.Config.MtfChiLevel, true) + ']';
			}
			else
			{
				MTFAlive = Stylize(RoundSummary.singleton.CountTeam(Team.MTF).ToString("00"));
				CiAlive = Stylize(RoundSummary.singleton.CountTeam(Team.CHI).ToString("00"));
			}
			if (level > plugin.Config.MtfEstLevel)
			{
				if (plugin.Config.MtfOp)
				{
					var cmp = PlayerManager.localPlayer.GetComponent<Respawning.RespawnManager>();
					if (cmp._timeForNextSequence > 0f)
					{
						if (plugin.Config.LongTime) estMTFtime = plugin.Config.Translations.MtfRespawn.Replace("$time", SecondsToTime(cmp._timeForNextSequence));
						else estMTFtime = plugin.Config.Translations.MtfRespawn.Replace("$time", Stylize(cmp._timeForNextSequence.ToString("0")));
					}
					else
					{
						estMTFtime = plugin.Config.Translations.MtfEst2;
					}
				}
				else
				{
					if (Round.ElapsedTime.Seconds - LastMtfSpawn < MinMTF)
					{
						if (plugin.Config.LongTime) estMTFtime = plugin.Config.Translations.MtfEst0.Replace("$(min)", SecondsToTime(MinMTF - Round.ElapsedTime.Seconds + LastMtfSpawn)).Replace("$(max)", SecondsToTime(MaxMTF - Round.ElapsedTime.Seconds + LastMtfSpawn));
						else estMTFtime = plugin.Config.Translations.MtfEst0.Replace("$(min)", Stylize(MinMTF - Round.ElapsedTime.Seconds + LastMtfSpawn.ToString("0"))).Replace("$(max)", (MaxMTF - Round.ElapsedTime.Seconds + LastMtfSpawn).ToString("0"));
					}
					else if (Round.ElapsedTime.Seconds - LastMtfSpawn < MaxMTF)
					{
						if (plugin.Config.LongTime) estMTFtime = plugin.Config.Translations.MtfEst1.Replace("$(max)", SecondsToTime(MaxMTF - Round.ElapsedTime.Seconds + LastMtfSpawn));
						else estMTFtime = plugin.Config.Translations.MtfEst1.Replace("$(max)", Stylize(MaxMTF - Round.ElapsedTime.Seconds + LastMtfSpawn.ToString("0")));
					}
					else
					{
						estMTFtime = plugin.Config.Translations.MtfEst2;
					} 
				}
			}
			else
			{
				estMTFtime = '[' + Pro079.Manager.LevelString(plugin.Config.MtfEstLevel, true) + ']';
			}
			string InfoMsg = Pro079.Manager.ReplaceAfterToken(plugin.Config.Translations.InfoMsg, '$', new Tuple<string, object>[] 
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
			});
			player.SendConsoleMessage(InfoMsg.Replace("\\n", Environment.NewLine), "white");
			if (level >= plugin.Config.GenLevel)
			{
				string ReturnMessage = plugin.Config.Translations.Generators;
				foreach (var generator in Generator079.Generators)
				{
					ReturnMessage += '\n' + plugin.Config.Translations.HasTablet.Replace("$room", generator.CurRoom) + ' ';
					if (generator.NetworkremainingPowerup == 0)
					{
						ReturnMessage += plugin.Config.Translations.GeneratorActivated + '\n';
					}
					else
					{
						ReturnMessage += (generator.isTabletConnected ? plugin.Config.Translations.HasTablet : plugin.Config.Translations.NoTablet) + ' ' + plugin.Config.Translations.TimeLeft.Replace("$sec", Stylize((int) generator.remainingPowerup));
					}
				}
				return "<color=white>" + ReturnMessage + "</color>";
			}
			else
			{
				return "<color=red>[" + plugin.Config.Translations.LockedUntil.Replace("$lvl", Stylize(plugin.Config.Translations.Generators)) + "]</color>";
			}
		}
		private string SecondsToTime(float sec)
		{
			int seconds = (int)sec % 60;
			int mins = ((int)sec - seconds) / 60;
			return (mins > 0 ? Stylize(mins.ToString()) + $" {plugin.Config.Translations.Minutes.Replace("$", (mins != 1 ? plugin.Config.Translations.PluralSuffix : string.Empty))}" : string.Empty)
				+ ((seconds > 0 && mins > 0) ? $" {plugin.Config.Translations.And} ": string.Empty) +
				(seconds != 0 ? $"{Stylize(seconds)} <color={plugin.Config.Color}>{plugin.Config.Translations.Seconds.Replace("$", (seconds != 1 ? plugin.Config.Translations.PluralSuffix : string.Empty))}</color>" : string.Empty);
		}
		private string Stylize(object obj)
		{
			return $"<b><color={plugin.Config.Color}>{obj}</color></b>";
		}

		public void OnWaitingForPlayers()
		{
            DeconBool = GameCore.ConfigFile.ServerConfig.GetBool("disable_decontamination");
			DeconTime = GameCore.ConfigFile.ServerConfig.GetFloat("decontamination_time", 11.47f);
			MinMTF = GameCore.ConfigFile.ServerConfig.GetInt("minimum_MTF_time_to_spawn");
			MaxMTF = GameCore.ConfigFile.ServerConfig.GetInt("maximum_MTF_time_to_spawn");
		}
		public void OnTeamRespawn(RespawningTeamEventArgs _ev)
		{
			LastMtfSpawn = Round.ElapsedTime.Seconds;
		}
	}
}
