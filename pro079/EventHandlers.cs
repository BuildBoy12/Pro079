using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EXILED;
using EXILED.Extensions;
using Pro079Core.API;

namespace Pro079Core
{
	public class EventHandlers
	{
		public Pro079 plugin;
		public EventHandlers(Pro079 plugin) => this.plugin = plugin;
		internal static Door[] DoorArray { get; private set; }

		public void OnCallCommand(ConsoleCommandEvent ev)
		{
			if (ev.Command.StartsWith("079"))
			{
				if (!plugin.enable)
				{
					return;
				}
				if (ev.Player.GetRole() != RoleType.Scp079)
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
					ev.Color = "white";
					ev.ReturnMessage = Pro079Logic.GetHelp();
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
						ev.ReturnMessage = "<Made by RogerFK#3679, Ported by BuildBoy12#6125>";
						return;
					}
					else if (args[0] == plugin.suicidecmd)
					{
						if (!plugin.suicide)
						{
							ev.ReturnMessage = plugin.disabled;
							return;
						}
						IEnumerable<ReferenceHub> PCplayers = Player.GetHubs(RoleType.Scp079);
						int pcs = PCplayers.Count();
						if (Team.SCP.GetHubs().Count() - pcs != 0)
						{
							ev.ReturnMessage = plugin.cantsuicide;
							return;
						}
						MEC.Timing.RunCoroutine(Pro079Logic.SixthGen(ev.Player), MEC.Segment.FixedUpdate);
						return;
					}
					if (args[0] == plugin.ultcmd)
					{
						if (args.Length == 1)
						{
							ev.Color = "white";
							ev.ReturnMessage = Pro079Logic.GetUltimates();
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
						if (!ev.Player.GetBypassMode())
						{
							if (ev.Player.GetLevel() + 1 < plugin.ultLevel)
							{
								ev.ReturnMessage = Pro079.Configs.LowLevel(plugin.ultLevel);
								return;
							}
							if (ev.Player.GetEnergy() < ultimate.Cost)
							{
								ev.ReturnMessage = Pro079.Configs.LowAP(ultimate.Cost);
								return;
							}
							Pro079.Manager.DrainAP(ev.Player, ultimate.Cost);

                            Pro079.Manager.UltimateCooldown += ultimate.Cooldown;
						}
						ev.ReturnMessage = ultimate.TriggerUltimate(args.Skip(1).ToArray(), ev.Player);
						return;
					}

					// When everything else wasn't caught, search for external commands //
					if (!Pro079.Manager.Commands.TryGetValue(args[0], out ICommand079 CommandHandler))
					{
						ev.ReturnMessage = plugin.unknowncmd;
						return;
					}
					if (!ev.Player.GetBypassMode())
					{
						if (ev.Player.GetLevel() + 1 < CommandHandler.MinLevel)
						{
							ev.ReturnMessage = Pro079.Configs.LowLevel(CommandHandler.MinLevel);
							return;
						}
						else if (ev.Player.GetEnergy() < CommandHandler.APCost)
						{
							ev.ReturnMessage = Pro079.Configs.LowAP(CommandHandler.APCost);
							return;
						}
						int cooldown = CommandHandler.CurrentCooldown - RoundSummary.roundTime;
						if (cooldown > 0)
						{
							ev.ReturnMessage = Pro079.Configs.CmdOnCooldown(cooldown);
							return;
						}
						if (CommandHandler.Cassie)
						{
							if (Pro079.Manager.CassieCooldown > 0)
							{
								ev.Color = "white";
								ev.ReturnMessage = plugin.cassieOnCooldown.Replace("$cd", Pro079.Manager.CassieCooldown.ToString()).Replace("$(cd)", Pro079.Manager.CassieCooldown.ToString());
								return;
							}
						}
					}
					// A try-catch statement in case any command malfunctions.
					try
					{
						CommandOutput output = new CommandOutput(true, true, true, false);
						ev.ReturnMessage = CommandHandler.CallCommand(args.Skip(1).ToArray(), ev.Player, output);
						// Drains the AP and sets it on cooldown if the command wasn't set on cooldown before (a.k.a. if you didn't do it manually)
						// You should only change the value of Success if your command needs more argument the user didn't insert. If there's any bug, it's your fault.
						if (!ev.Player.GetBypassMode() && output.Success)
						{
							if(output.DrainAp) Pro079.Manager.DrainAP(ev.Player, CommandHandler.APCost);

							if (CommandHandler.CurrentCooldown < RoundSummary.roundTime) Pro079.Manager.SetOnCooldown(CommandHandler);

							if (CommandHandler.Cassie && output.CassieCooldown)
							{
								Pro079.Manager.CassieCooldown = plugin.cassieCooldown;
								if (!string.IsNullOrEmpty(plugin.cassieready))
								{
									int p = (int)System.Environment.OSVersion.Platform;
									if ((p == 4) || (p == 6) || (p == 128)) MEC.Timing.RunCoroutine(Pro079Logic.CooldownCassie(plugin.cassieCooldown), MEC.Segment.Update);
									else MEC.Timing.RunCoroutine(Pro079Logic.CooldownCassie(plugin.cassieCooldown), 1);
								} 
							}
						}
						if (!output.CustomReturnColor)
						{
							ev.ReturnMessage = $"<color=\"{(output.Success ? "#30e330" : "red")}\">" + ev.ReturnMessage + "</color>";
						}
					}
					catch (Exception e)
					{
						Log.Error($"Error with command \"{args[0]}\" and literally not my problem:\n" + e.ToString());
						ev.ReturnMessage = plugin.error + ": " + e.Message;
					}
				}
			}
		}

		public void OnSetRole(SetClassEvent ev)
		{
			try
			{
				if (!plugin.spawnBroadcast || !plugin.enable)
				{
					return;
				}

				if (ev.Role == RoleType.Scp079)
				{
					MEC.Timing.RunCoroutine(Pro079Logic.DelaySpawnMsg(ev.Player));
				}
			}
			catch(Exception e)
			{
				Log.Error($"Pro079 SetClassEvent error: {e}");
			}		
		}

		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			try
			{
				if (ev.Player.GetTeam() == Team.SCP && ev.Player.GetRole() != RoleType.Scp079)
				{
					IEnumerable<ReferenceHub> PCplayers = Player.GetHubs(RoleType.Scp079);
					int pcs = PCplayers.Count();
					if (pcs < 0) return;
					if (Team.SCP.GetHubs().Count() - pcs <= 1)
					{
						MEC.Timing.RunCoroutine(Pro079Logic.DelayKysMessage(PCplayers), 1);
					}
				}
			}
			catch(Exception e)
			{
				Log.Error($"Pro079 PlayerDeathEvent error: {e}");
			}		
		}

		public void OnWaitingForPlayers()
		{
			try
			{
				DoorArray = UnityEngine.Object.FindObjectsOfType<Door>();
				foreach (KeyValuePair<string, ICommand079> Command in Pro079.Manager.Commands)
				{
					Command.Value.CurrentCooldown = 0;
				}
				Pro079.Manager.UltimateCooldown = 0;
				Pro079.Manager.CassieCooldown = 0;
			}
			catch (Exception e)
			{
				Log.Error($"Pro079 WaitingForPlayers error: {e}");
			}
		}
	}
}
