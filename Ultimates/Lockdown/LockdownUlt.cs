using System.Collections.Generic;
using Pro079Core.API;
using Exiled.Events.EventArgs;
using Exiled.API.Features;

namespace LockdownUltimate
{
	public class LockdownUltimate : IUltimate079
	{
		public bool CurrentlyRunning { get; private set; }
		public string Info => LockdownPlugin.ConfigRef.Config.Translations.LockdownInfo.Replace("{min}", LockdownPlugin.ConfigRef.Config.LockdownTime.ToString());
		public int Cooldown => LockdownPlugin.ConfigRef.Config.LockdownCooldown;
		public int Cost => LockdownPlugin.ConfigRef.Config.LockdownCost;
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
			Respawning.RespawnEffectsController.PlayCassieAnnouncement("Security Lockdown pitch_2 scp 0 7 9 . Override .g6  pitch_1 Detected .  . jam_010_5 Automatic Emergency Zone  lockdown Initializing . 3 . jam_010_8 2 . 1 ", false, true);
			yield return MEC.Timing.WaitForSeconds(10f);
			CurrentlyRunning = true;
			yield return MEC.Timing.WaitForSeconds(LockdownPlugin.ConfigRef.Config.LockdownTime);
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
