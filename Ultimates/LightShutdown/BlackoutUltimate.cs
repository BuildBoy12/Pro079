using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using Pro079Core.API;

namespace BlackoutUltimate
{
	internal class BlackoutLogic : IUltimate079
	{
		private readonly BlackoutUltimate plugin;
		public BlackoutLogic(BlackoutUltimate plugin)
		{
			this.plugin = plugin;
		}
		public string Name => "Blackout";

		public string Info => plugin.p0blackoutinfo.Replace("$", plugin.minutes != 1 ? "s" : string.Empty).Replace("{min}", plugin.minutes.ToString());

		public int Cooldown => plugin.cooldown;

		public int Cost => plugin.cost;

		//public Room[] Rooms { get; private set; }

		/*public void OnWaitingForPlayers()
		{
			Rooms = PluginManager.Manager.Server.Map.Get079InteractionRooms(Scp079InteractionType.CAMERA).Where(r => r.ZoneType != ZoneType.ENTRANCE).ToArray();
		}*/

		public string TriggerUltimate(string[] args, ReferenceHub Player)
		{
			int p = (int)System.Environment.OSVersion.Platform;
			if ((p == 4) || (p == 6) || (p == 128)) MEC.Timing.RunCoroutine(ShutDownLights(), MEC.Segment.Update);
			else MEC.Timing.RunCoroutine(ShutDownLights(), 1);
			return Pro079Core.Configs.Instance.UltimateLaunched;
		}

		private IEnumerator<float> ShutDownLights()
		{
			PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement("warning . malfunction detected on heavy containment zone . Scp079Recon6 . . . light systems Disengaged", false, true);
			yield return MEC.Timing.WaitForSeconds(12.1f);
			/*float start = RoundSummary.roundTime;
			while (start + plugin.minutes * 60f > RoundSummary.roundTime)
			{
				foreach (var room in Rooms)
				{
					room.FlickerLights();
				}
				yield return MEC.Timing.WaitForSeconds(8f);
			}*/
			Map.TurnOffAllLights(plugin.minutes * 60f, false);
		}
	}
}
