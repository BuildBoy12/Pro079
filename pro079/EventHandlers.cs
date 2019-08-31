using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Pro079Core.API;
using Smod2;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;
using Smod2.EventSystem.Events;

namespace Pro079Core
{
	internal class EventHandlers : IEventHandlerCallCommand, IEventHandlerSetRole,
		IEventHandlerPlayerDie, IEventHandlerWaitingForPlayers
	{
		private readonly Pro079 plugin;
		public EventHandlers(Pro079 plugin)
		{
			this.plugin = plugin;
		}

		internal static FlickerableLight[] FlickerableLightsArray { get; private set; }
		internal static Door[] DoorArray { get; private set; }

		public void OnCallCommand(PlayerCallCommandEvent ev)
		{
			if (ev.Command.StartsWith("079"))
			{
				if (!plugin.enable)
				{
					return;
				}
				if (ev.Player.TeamRole.Role != Role.SCP_079)
				{
					ev.ReturnMessage = plugin.notscp079;
					return;
				}

				ev.ReturnMessage = plugin.error;
				// this block is pasted from PlayerPrefs https://github.com/probe4aiur/PlayerPreferences/
				MatchCollection collection = new Regex("[^\\s\"\']+|\"([^\"]*)\"|\'([^\']*)\'").Matches(ev.Command);
				string[] args = new string[collection.Count - 1];

				for (int i = 1; i < collection.Count; i++)
				{
					if (collection[i].Value[0] == '\"' && collection[i].Value[collection[i].Value.Length - 1] == '\"')
					{
						args[i - 1] = collection[i].Value.Substring(1, collection[i].Value.Length - 2);
					}
					else
					{
						args[i - 1] = collection[i].Value;
					}
				}
				// end of the paste thx

				if (args.Length == 0)
				{
					ev.ReturnMessage = "<color=\"white\">" + Logic.GetHelp() + "</color>";
				}
				else if (args.Length >= 1)
				{
					if (args[0] == plugin.tipscmd)
					{
						if (!plugin.tips)
						{
							ev.ReturnMessage = plugin.disabled;
							return;
						}
						ev.Player.SendConsoleMessage(plugin.tipsMsg.Replace("\\n", "\n"), "white");
						ev.ReturnMessage = "<Made by RogerFK#3679>";
						return;
					}
					else if (args[0] == plugin.suicidecmd)
					{
						if (!plugin.suicide)
						{
							ev.ReturnMessage = plugin.disabled;
							return;
						}
						List<Player> PCplayers = PluginManager.Manager.Server.GetPlayers(Role.SCP_079);
						int pcs = PCplayers.Count;
						if (PluginManager.Manager.Server.Round.Stats.SCPAlive + PluginManager.Manager.Server.Round.Stats.Zombies - pcs != 0)
						{
							ev.ReturnMessage = plugin.cantsuicide;
							return;
						}
						PluginManager.Manager.Server.Map.AnnounceCustomMessage("Scp079Recon6");
						int p = (int)System.Environment.OSVersion.Platform;
						if ((p == 4) || (p == 6) || (p == 128)) MEC.Timing.RunCoroutine(Logic.FakeKillPC(ev.Player), MEC.Segment.Update);
						else MEC.Timing.RunCoroutine(Logic.FakeKillPC(ev.Player), 1);
						return;
					}
					if (args[0] == plugin.ultcmd)
					{
						if (args.Length == 1)
						{
							ev.ReturnMessage = Logic.GetUltimates();
							return;
						}
						if (Pro079.Manager.UltimateCooldown > 0)
						{
							plugin.ultdown.Replace("$cd", Pro079.Manager.UltimateCooldown.ToString());
							return;
						}
						IUltimate079 ultimate = Pro079.Manager.GetUltimate(string.Join(" ", args.Skip(1).ToArray()));
						if (ultimate == null)
						{
							ev.ReturnMessage = plugin.ulterror;
							return;
						}
						ultimate.TriggerUltimate(args.Skip(1).ToArray(), ev.Player);
					}

					// When everything else wasn't caught, search for external commands //
					if (!Pro079.Manager.Commands.TryGetValue(args[0], out ICommand079 CommandHandler))
					{
						ev.ReturnMessage = plugin.unknowncmd;
						return;
					}
					if (!ev.Player.GetBypassMode())
					{
						if (ev.Player.Scp079Data.Level + 1 < CommandHandler.MinLevel)
						{
							ev.ReturnMessage = Pro079.Configs.LowLevel(CommandHandler.MinLevel);
							return;
						}
						else if (ev.Player.Scp079Data.AP < CommandHandler.APCost)
						{
							ev.ReturnMessage = Pro079.Configs.LowAP(CommandHandler.APCost);
							return;
						}
						int cooldown = CommandHandler.CurrentCooldown - PluginManager.Manager.Server.Round.Duration;
						if (cooldown > 0)
						{
							ev.ReturnMessage = Pro079.Configs.CmdOnCooldown(cooldown);
							return;
						}
						if (CommandHandler.Cassie)
						{
							if (Pro079.Manager.CassieCooldown > 0)
							{
								ev.ReturnMessage = plugin.cassieOnCooldown.Replace("$cd", Pro079.Manager.CassieCooldown.ToString()).Replace("$(cd)", Pro079.Manager.CassieCooldown.ToString());
								return;
							}
							Pro079.Manager.CassieCooldown = plugin.cassieCooldown;
							if (!string.IsNullOrEmpty(plugin.cassieready))
							{
								int p = (int)System.Environment.OSVersion.Platform;
								if ((p == 4) || (p == 6) || (p == 128)) MEC.Timing.RunCoroutine(Logic.CooldownCassie(plugin.cassieCooldown), MEC.Segment.Update);
								else MEC.Timing.RunCoroutine(Logic.CooldownCassie(plugin.cassieCooldown), 1);
							}
						}
					}
					// A try-catch statement since some plugins will throw an exception, I'm sure of it.
					try
					{
						ev.ReturnMessage = CommandHandler.CallCommand(args.Skip(1).ToArray(), ev.Player);
						Pro079.Manager.SetOnCooldown(CommandHandler);
						Pro079.Manager.DrainAP(ev.Player, CommandHandler.APCost);
					}
					catch (Exception e)
					{
						plugin.Error($"Error with command {args[0]} and literally not my problem:\n" + e.ToString());
						ev.ReturnMessage = plugin.error + ": " + e.Message;
					}
				}

			}
		}

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			if (!plugin.spawnBroadcast || !plugin.enable)
			{
				return;
			}

			if (ev.Role == Role.SCP_079)
			{
				MEC.Timing.RunCoroutine(Logic.DelaySpawnMsg(ev.Player), 1);
			}
		}

		public void OnPlayerDie(PlayerDeathEvent ev)
		{
			if (ev.Player.TeamRole.Team == Smod2.API.Team.SCP && ev.Player.TeamRole.Role != Role.SCP_079)
			{
				List<Player> PCplayers = PluginManager.Manager.Server.GetPlayers(Role.SCP_079);
				int pcs = PCplayers.Count;
				if (pcs < 0) return;
				if (PluginManager.Manager.Server.Round.Stats.SCPAlive + PluginManager.Manager.Server.Round.Stats.Zombies - pcs <= 1)
				{
					MEC.Timing.RunCoroutine(Logic.DelayKysMessage(PCplayers), 1);
				}
			}
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			FlickerableLightsArray = UnityEngine.Object.FindObjectsOfType<FlickerableLight>();
			DoorArray = UnityEngine.Object.FindObjectsOfType<Door>();
		}
	}
}
