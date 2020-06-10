using System;
using Pro079Core;
using Pro079Core.API;
using EXILED;
using EXILED.Extensions;
using System.Collections.Generic;
using System.Text;
using LightContainmentZoneDecontamination;

namespace InfoCommand
{
	public class InfoCommand : ICommand079
	{
		private int LastMtfSpawn;
		private readonly InfoPlugin plugin;
		public InfoCommand(InfoPlugin plugin)
		{
			this.plugin = plugin;
		}

		public bool OverrideDisable = false;
		private bool DeconBool;
		private float DeconTime;
		private int MinMTF;
		private int MaxMTF;
		private Dictionary<string, string> replaceDict;

		public bool Disabled
		{
			get => OverrideDisable ? OverrideDisable : !plugin.enable;
			set => OverrideDisable = value;
		}

		public string Command => plugin.infocmd;

		public string ExtraArguments => string.Empty;

		public string HelpInfo => plugin.infoextrahelp;

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

		public string CallCommand(string[] args, ReferenceHub player, CommandOutput output)
		{
			output.CustomReturnColor = true;
			int level = player.GetBypassMode() ? 5 : player.GetLevel() + 1;
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

			if (level < plugin.decont)
			{
				decontTime = '[' + Pro079.Configs.LevelString(plugin.decont, true) + ']';
			}
			else
			{
				if (DeconBool)
				{
					decontTime = plugin.decontdisabled;
				}
				else if (Map.IsLCZDecontaminated)
				{
					decontTime = plugin.deconthappened;
				}
				else
				{
					//Bs timespan logic because I got tired of it not cooperating :b
					TimeSpan auxTime = TimeSpan.FromMinutes(DeconTime) - TimeSpan.FromSeconds(DecontaminationController.GetServerTime);
                    decontTime = auxTime > TimeSpan.FromSeconds(0) ? Stylize(auxTime.ToString()) : plugin.decontbug;
				}
			}
			if (level < plugin.escaped)
			{
				ScientistsEscaped = '[' + Pro079.Configs.LevelString(plugin.escaped, true) + ']';
				ClassDEscaped = '[' + Pro079.Configs.LevelString(plugin.escaped, true) + ']';
			}
			else
			{
				ClassDEscaped = Stylize(RoundSummary.escaped_ds.ToString("00"));
				ScientistsEscaped = Stylize(RoundSummary.escaped_scientists.ToString("00"));
			}

			if (level < plugin.plebs)
			{
				ClassDAlive = '[' + Pro079.Configs.LevelString(plugin.plebs, true) + ']';
				ScientistsAlive = '[' + Pro079.Configs.LevelString(plugin.plebs, true) + ']';
			}
			else
			{
				ClassDAlive = Stylize(RoundSummary.singleton.CountTeam(Team.CDP).ToString("00"));
				ScientistsAlive = Stylize(RoundSummary.singleton.CountTeam(Team.RSC).ToString("00"));
			}
			if (level < plugin.mtfci)
			{
				MTFAlive = '[' + Pro079.Configs.LevelString(plugin.mtfci, true) + ']';
				CiAlive = '[' + Pro079.Configs.LevelString(plugin.mtfci, true) + ']';
			}
			else
			{
				MTFAlive = Stylize(RoundSummary.singleton.CountTeam(Team.MTF).ToString("00"));
				CiAlive = Stylize(RoundSummary.singleton.CountTeam(Team.CHI).ToString("00"));
			}
			if (level > plugin.mtfest)
			{
				if (plugin.mtfop)
				{
					var cmp = PlayerManager.localPlayer.GetComponent<MTFRespawn>();
					if (cmp.timeToNextRespawn > 0f)
					{
						if (plugin.longTime) estMTFtime = plugin.mtfRespawn.Replace("$time", SecondsToTime(cmp.timeToNextRespawn));
						else estMTFtime = plugin.mtfRespawn.Replace("$time", Stylize(cmp.timeToNextRespawn.ToString("0")));
					}
					else
					{
						estMTFtime = plugin.mtfest2;
					}
				}
				else
				{
					if (RoundSummary.roundTime - LastMtfSpawn < MinMTF)
					{
						if (plugin.longTime) estMTFtime = plugin.mtfest0.Replace("$(min)", SecondsToTime(MinMTF - RoundSummary.roundTime + LastMtfSpawn)).Replace("$(max)", SecondsToTime(MaxMTF - RoundSummary.roundTime + LastMtfSpawn));
						else estMTFtime = plugin.mtfest0.Replace("$(min)", Stylize(MinMTF - RoundSummary.roundTime + LastMtfSpawn.ToString("0"))).Replace("$(max)", (MaxMTF - RoundSummary.roundTime + LastMtfSpawn).ToString("0"));
					}
					else if (RoundSummary.roundTime - LastMtfSpawn < MaxMTF)
					{
						if (plugin.longTime) estMTFtime = plugin.mtfest1.Replace("$(max)", SecondsToTime(MaxMTF - RoundSummary.roundTime + LastMtfSpawn));
						else estMTFtime = plugin.mtfest1.Replace("$(max)", Stylize(MaxMTF - RoundSummary.roundTime + LastMtfSpawn.ToString("0")));
					}
					else
					{
						estMTFtime = plugin.mtfest2;
					} 
				}
			}
			else
			{
				estMTFtime = '[' + Pro079.Configs.LevelString(plugin.mtfest, true) + ']';
			}
			replaceDict = new Dictionary<string, string>()
			{
				{ "$scpalive", RoundSummary.singleton.CountTeam(Team.SCP).ToString() },
				{ "$humans", humansAlive },
				{ "$estMTF", estMTFtime },
				{ "$decont", decontTime },
				{ "$cdesc", ClassDEscaped },
				{ "$sciesc", ScientistsEscaped },
				{ "$cdalive", ClassDAlive },
				{ "$cialive", CiAlive },
				{ "$scialive", ScientistsAlive },
				{ "$mtfalive", MTFAlive },
				{ "\\n", Environment.NewLine }
			};
			player.SendConsoleMessage(ReplaceString(new StringBuilder(plugin.infomsg, plugin.infomsg.Length * 2)), "white");
			if (level >= plugin.gens)
			{
				string ReturnMessage = plugin.generators;
				foreach (var generator in Generator079.generators)
				{
					ReturnMessage += '\n' + plugin.generatorin.Replace("$room", generator.curRoom) + ' ';
					if (generator.NetworkremainingPowerup == 0)
					{
						ReturnMessage += plugin.activated + '\n';
					}
					else
					{
						ReturnMessage += (generator.isTabletConnected ? plugin.hastablet : plugin.notablet) + ' ' + plugin.timeleft.Replace("$sec", Stylize((int) generator.remainingPowerup));
					}
				}
				return "<color=white>" + ReturnMessage + "</color>";
			}
			else
			{
				return "<color=red>[" + plugin.lockeduntil.Replace("$lvl", Stylize(plugin.gens)) + "]</color>";
			}
		}
		private string ReplaceString(StringBuilder str)
        {
			foreach(string s in replaceDict.Keys)
            {
				str = str.Replace(s, replaceDict[s]);
            }
			return str.ToString();
        }
		private string SecondsToTime(float sec)
		{
			int seconds = (int)sec % 60;
			int mins = ((int)sec - seconds) / 60;
			return (mins > 0 ? Stylize(mins.ToString()) + $" {plugin.iminutes.Replace("$", (mins != 1 ? plugin.pluralSuffix : string.Empty))}" : string.Empty)
				+ ((seconds > 0 && mins > 0) ? $" {plugin.iand} ": string.Empty) +
				(seconds != 0 ? $"{Stylize(seconds)} <color={plugin.color}>{plugin.iseconds.Replace("$", (seconds != 1 ? plugin.pluralSuffix : string.Empty))}</color>" : string.Empty);
		}
		private string Stylize(object obj)
		{
			return $"<b><color={plugin.color}>{obj}</color></b>";
		}

		public void OnWaitingForPlayers()
		{
            DeconBool = GameCore.ConfigFile.ServerConfig.GetBool("disable_decontamination");
			DeconTime = GameCore.ConfigFile.ServerConfig.GetFloat("decontamination_time", 11.47f);
			MinMTF = GameCore.ConfigFile.ServerConfig.GetInt("minimum_MTF_time_to_spawn");
			MaxMTF = GameCore.ConfigFile.ServerConfig.GetInt("maximum_MTF_time_to_spawn");
		}
		public void OnTeamRespawn(ref TeamRespawnEvent ev)
		{
			LastMtfSpawn = RoundSummary.roundTime;
		}
	}
}
