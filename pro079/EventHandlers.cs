namespace Pro079Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Exiled.API.Features;
	using Exiled.Events.EventArgs;
	using Pro079Core.API;

	public class EventHandlers
	{
		public Pro079 plugin;
		public EventHandlers(Pro079 plugin) => this.plugin = plugin;

		public void OnCallCommand(SendingConsoleCommandEventArgs ev)
		{
			if (ev.Name == "079")
			{
				ev.Allow = false;
				if (ev.Player.Role != RoleType.Scp079)
				{
					ev.ReturnMessage = Pro079.ConfigRef.Config.Translations.NotScp079;
					return;
				}
				ev.ReturnMessage = Pro079.ConfigRef.Config.Translations.Error;
				if (ev.Arguments.Count == 0)
				{
					ev.Color = "white";
					ev.ReturnMessage = Pro079Logic.GetHelp();
				}
				else if (ev.Arguments.Count >= 1)
				{
					if (ev.Arguments[0] == Pro079.ConfigRef.Config.Translations.TipsCmd)
					{
						if (!Pro079.ConfigRef.Config.EnableTips)
						{
							ev.ReturnMessage = Pro079.ConfigRef.Config.Translations.Disabled;
							return;
						}
						ev.Player.SendConsoleMessage(Pro079.ConfigRef.Config.Translations.TipsMsg.Replace("\\n", "\n"), "white");
						ev.ReturnMessage = "<Made by RogerFK#3679, Ported by BuildBoy12#6125>";
						return;
					}
					else if (ev.Arguments[0] == Pro079.ConfigRef.Config.Translations.SuicideCmd)
					{
						if (!Pro079.ConfigRef.Config.SuicideCommand)
						{
							ev.ReturnMessage = Pro079.ConfigRef.Config.Translations.Disabled;
							return;
						}
						int pcs = Scp079PlayerScript.instances.Count();
						if (Player.List.Where(x => x.Team == Team.SCP).Count() - pcs != 0)
						{
							ev.ReturnMessage = Pro079.ConfigRef.Config.Translations.CantSuicide;
							return;
						}
						MEC.Timing.RunCoroutine(Pro079Logic.SixthGen(ev.Player), MEC.Segment.FixedUpdate);
						return;
					}
					if (ev.Arguments[0] == Pro079.ConfigRef.Config.Translations.UltCmd)
					{
						if (ev.Arguments.Count == 1)
						{
							ev.Color = "white";
							ev.ReturnMessage = Pro079Logic.GetUltimates();
							return;
						}
						if (Pro079.Manager.UltimateCooldown > 0)
						{
							Pro079.ConfigRef.Config.Translations.UltDown.Replace("$cd", Pro079.Manager.UltimateCooldown.ToString());
							return;
						}
						IUltimate079 ultimate = Pro079.Manager.GetUltimate(string.Join(" ", ev.Arguments.Skip(1).ToArray()));
						if (ultimate == null)
						{
							ev.ReturnMessage = Pro079.ConfigRef.Config.Translations.UltError;
							return;
						}
						if (!ev.Player.IsBypassModeEnabled)
						{
							if (ev.Player.Level + 1 < Pro079.ConfigRef.Config.UltimateLevel)
							{
								ev.ReturnMessage = Pro079.Manager.LowLevel(Pro079.ConfigRef.Config.UltimateLevel);
								return;
							}
							if (ev.Player.Energy < ultimate.Cost)
							{
								ev.ReturnMessage = Pro079.Manager.LowAP(ultimate.Cost);
								return;
							}
							Pro079.Manager.DrainAP(ev.Player, ultimate.Cost);

                            Pro079.Manager.UltimateCooldown += ultimate.Cooldown;
						}
						ev.ReturnMessage = ultimate.TriggerUltimate(ev.Arguments.Skip(1).ToArray(), ev.Player);
						return;
					}

					// When everything else wasn't caught, search for external commands //
					if (!Pro079.Manager.Commands.TryGetValue(ev.Arguments[0], out ICommand079 CommandHandler))
					{
						ev.ReturnMessage = Pro079.ConfigRef.Config.Translations.UnknownCmd;
						return;
					}
					if (!ev.Player.IsBypassModeEnabled)
					{
						if (ev.Player.Level + 1 < CommandHandler.MinLevel)
						{
							ev.ReturnMessage = Pro079.Manager.LowLevel(CommandHandler.MinLevel);
							return;
						}
						else if (ev.Player.Energy < CommandHandler.APCost)
						{
							ev.ReturnMessage = Pro079.Manager.LowAP(CommandHandler.APCost);
							return;
						}
						int cooldown = CommandHandler.CurrentCooldown - Round.ElapsedTime.Seconds;
						if (cooldown > 0)
						{
							ev.ReturnMessage = Pro079.Manager.CmdOnCooldown(cooldown);
							return;
						}
						if (CommandHandler.Cassie)
						{
							if (Pro079.Manager.CassieCooldown > 0)
							{
								ev.Color = "white";
								ev.ReturnMessage = Pro079.ConfigRef.Config.Translations.CassieOnCooldown.Replace("$cd", Pro079.Manager.CassieCooldown.ToString()).Replace("$(cd)", Pro079.Manager.CassieCooldown.ToString());
								return;
							}
						}
					}
					// A try-catch statement in case any command malfunctions.
					try
					{
						CommandOutput output = new CommandOutput(true, true, true, false);
						ev.ReturnMessage = CommandHandler.CallCommand(ev.Arguments.Skip(1).ToArray(), ev.Player, output);
						// Drains the AP and sets it on cooldown if the command wasn't set on cooldown before (a.k.a. if you didn't do it manually)
						// You should only change the value of Success if your command needs more argument the user didn't insert. If there's any bug, it's your fault.
						if (!ev.Player.IsBypassModeEnabled && output.Success)
						{
							if(output.DrainAp) Pro079.Manager.DrainAP(ev.Player, CommandHandler.APCost);

							if (CommandHandler.CurrentCooldown < Round.ElapsedTime.Seconds) Pro079.Manager.SetOnCooldown(CommandHandler);

							if (CommandHandler.Cassie && output.CassieCooldown)
							{
								Pro079.Manager.CassieCooldown = Pro079.ConfigRef.Config.CassieCooldown;
								if (!string.IsNullOrEmpty(Pro079.ConfigRef.Config.Translations.CassieReady))
								{
									int p = (int)System.Environment.OSVersion.Platform;
									if ((p == 4) || (p == 6) || (p == 128)) MEC.Timing.RunCoroutine(Pro079Logic.CooldownCassie(Pro079.ConfigRef.Config.CassieCooldown), MEC.Segment.Update);
									else MEC.Timing.RunCoroutine(Pro079Logic.CooldownCassie(Pro079.ConfigRef.Config.CassieCooldown), 1);
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
						Log.Error($"Error with command \"{ev.Arguments[0]}\" and literally not my problem:\n" + e);
						ev.ReturnMessage = Pro079.ConfigRef.Config.Translations.Error + ": " + e.Message;
					}
				}
			}
		}

		public void OnSetRole(ChangingRoleEventArgs ev)
		{
			try
			{
				if (!Pro079.ConfigRef.Config.EnableSpawnBroadcast)
				{
					return;
				}

				if (ev.NewRole == RoleType.Scp079)
				{
					MEC.Timing.RunCoroutine(Pro079Logic.DelaySpawnMsg(ev.Player));
				}
			}
			catch(Exception e)
			{
				Log.Error($"Pro079 SetClassEvent error: {e}");
			}		
		}

		public void OnPlayerDie(DiedEventArgs ev)
		{
			try
			{
				if (ev.Target.Team == Team.SCP && ev.Target.Role != RoleType.Scp079)
				{
					IEnumerable<Player> PCplayers = Player.List.Where(x => x.Role == RoleType.Scp079);
					int pcs = PCplayers.Count();
					if (pcs < 0) return;
					if (Player.List.Where(x => x.Team == Team.SCP).Count() - pcs <= 1)
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