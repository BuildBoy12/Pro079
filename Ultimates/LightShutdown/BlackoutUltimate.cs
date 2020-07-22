﻿using System.Collections.Generic;
using Exiled.API.Features;
using Pro079Core.API;

namespace BlackoutUltimate
{
    internal class BlackoutLogic : IUltimate079
	{
		private readonly BlackoutUltimate plugin;
		public BlackoutLogic(BlackoutUltimate plugin) => this.plugin = plugin;

		public string Name => "Blackout";

		public string Info => plugin.Config.Translations.BlackoutInfo.Replace("$", plugin.Config.BlackoutMinutes != 1 ? "s" : string.Empty).Replace("{min}", plugin.Config.BlackoutMinutes.ToString());

		public int Cooldown => plugin.Config.BlackoutCooldown;

		public int Cost => plugin.Config.BlackoutCost;

		public string TriggerUltimate(string[] args, Player Player)
		{
			int p = (int)System.Environment.OSVersion.Platform;
			if ((p == 4) || (p == 6) || (p == 128)) MEC.Timing.RunCoroutine(ShutDownLights(), MEC.Segment.Update);
			else MEC.Timing.RunCoroutine(ShutDownLights(), 1);
			return Pro079Core.Pro079.Manager.UltimateLaunched;
		}

		private IEnumerator<float> ShutDownLights()
		{
			Respawning.RespawnEffectsController.PlayCassieAnnouncement("warning . malfunction detected on heavy containment zone . Scp079Recon6 . . . light systems Disengaged", false, true);
			yield return MEC.Timing.WaitForSeconds(12.1f);
			Map.TurnOffAllLights(plugin.Config.BlackoutMinutes * 60f, false);
		}
	}
}