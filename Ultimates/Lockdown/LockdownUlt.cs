﻿using System.Collections.Generic;
using Pro079Core.API;
using EXILED;
using EXILED.Extensions;

namespace LockdownUltimate
{
	public class LockdownUltimate : IUltimate079
	{
		private readonly LockdownPlugin plugin;
		public bool CurrentlyRunning { get; private set; }
		public string Info => plugin.lockdownInfo.Replace("{min}", plugin.time.ToString());
		public int Cooldown => plugin.cooldown;
		public int Cost => plugin.cost;
		public string Name => "Lockdown";
		public LockdownUltimate(LockdownPlugin plugin)
		{
			this.plugin = plugin;
		}
		public void OnWaitingForPlayers()
		{
			CurrentlyRunning = false;
		}
		public void OnDoorAccess(ref DoorInteractionEvent ev)
		{
			if (CurrentlyRunning == false || string.IsNullOrWhiteSpace(ev.Door.permissionLevel))
			{
				return;
			}
			else
			{
				if (ev.Player.GetTeam() == Team.SCP)
				{
					ev.Allow = true;
				}
				else
				{
					ev.Allow = false;
				}
			}
		}
		private IEnumerator<float> Ult2Toggle()
		{
			PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement("Security Lockdown pitch_2 scp 0 7 9 . Override .g6  pitch_1 Detected .  . jam_010_5 Automatic Emergency Zone  lockdown Initializing . 3 . jam_010_8 2 . 1 ", false, true);
			yield return MEC.Timing.WaitForSeconds(10f);
			CurrentlyRunning = true;
			yield return MEC.Timing.WaitForSeconds(plugin.time);
			CurrentlyRunning = false;
			PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement("Automatic Emergency zone lockdown . Disabled", false, true);
		}

		public string TriggerUltimate(string[] args, ReferenceHub Player)
		{
			int p = (int)System.Environment.OSVersion.Platform;
			if ((p == 4) || (p == 6) || (p == 128)) MEC.Timing.RunCoroutine(Ult2Toggle(), MEC.Segment.Update);
			else MEC.Timing.RunCoroutine(Ult2Toggle(), 1);
			return Pro079Core.Configs.Instance.UltimateLaunched;
		}
	}
}
