using System.Collections.Generic;
using Pro079Core.API;
using Exiled.Events.EventArgs;
using Exiled.API.Features;

namespace LockdownUltimate
{
	public class LockdownUltimate : IUltimate079
	{
		private readonly LockdownPlugin plugin;
		public LockdownUltimate(LockdownPlugin plugin) => this.plugin = plugin;

		private bool CurrentlyRunning = false;
		public string Info => plugin.Config.Translations.LockdownInfo.Replace("{min}", plugin.Config.Time.ToString());
		public int Cooldown => plugin.Config.Cooldown;
		public int Cost => plugin.Config.Cost;
		public string Name => "Lockdown";

		public void OnWaitingForPlayers()
		{
			CurrentlyRunning = false;
		}
		public void OnDoorAccess(InteractingDoorEventArgs ev)
		{
			if (CurrentlyRunning == false || string.IsNullOrWhiteSpace(ev.Door.DoorName))
			{
				return;
			}
			else
			{
				if (ev.Player.Team == Team.SCP)
				{
					ev.IsAllowed = true;
				}
				else
				{
					ev.IsAllowed = false;
				}
			}
		}
		private IEnumerator<float> Ult2Toggle()
		{
			NineTailedFoxAnnouncer.singleton.ServerOnlyAddGlitchyPhrase("Security Lockdown pitch_2 scp 0 7 9 . Override pitch_1 Detected .  . Automatic Emergency Zone  lockdown Initializing . 3 . 2 . 1 ", 10, 20);
			yield return MEC.Timing.WaitForSeconds(10f);
			CurrentlyRunning = true;
			yield return MEC.Timing.WaitForSeconds(plugin.Config.Time);
			CurrentlyRunning = false;
			Respawning.RespawnEffectsController.PlayCassieAnnouncement("Automatic Emergency zone lockdown . Disabled", false, true);
		}

		public string TriggerUltimate(string[] args, Player Player)
		{
			int p = (int)System.Environment.OSVersion.Platform;
			if ((p == 4) || (p == 6) || (p == 128)) MEC.Timing.RunCoroutine(Ult2Toggle(), MEC.Segment.Update);
			else MEC.Timing.RunCoroutine(Ult2Toggle(), 1);
			return Pro079Core.Pro079.Manager.UltimateLaunched;
		}
	}
}
