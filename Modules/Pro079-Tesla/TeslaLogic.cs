using Exiled.API.Features;
using System;
using System.Collections.Generic;

namespace TeslaCommand
{
	internal static class TeslaLogic
	{
		internal static IEnumerator<float> DisableTeslas(float time, TeslaPlugin plugin)
		{
			TeslaGate[] teslas = UnityEngine.Object.FindObjectsOfType<TeslaGate>();
			int length = teslas.Length;
			float[] distances = new float[teslas.Length];
			int i;

			for (i = 0; i < length; i++)
			{
				distances[i] = teslas[i].sizeOfTrigger;
				teslas[i].sizeOfTrigger = -1f;
			}
			int remTime = plugin.Config.TeslaRemaining;
			yield return MEC.Timing.WaitForSeconds(time - remTime);
			foreach (Player player in Player.List)
			{
				string remainingStr = plugin.Config.Translations.TeslaRem;
				if (player.Role == RoleType.Scp079)
				{
					for (i = remTime; i > 0; i--)
					{
						player.Broadcast(1, Environment.NewLine + remainingStr.Replace("$sec", "<b>" + i + "</b>"), Broadcast.BroadcastFlags.Normal);
					}
					player.Broadcast(5, Environment.NewLine + plugin.Config.Translations.TeslaReenabled, Broadcast.BroadcastFlags.Normal);
				}
			}
			yield return MEC.Timing.WaitForSeconds(remTime);

			for (i = 0; i < length; i++)
			{
				teslas[i].sizeOfTrigger = distances[i];
			}
		}

		public static float time = 0f;
		internal static IEnumerator<float> TeslaTimer(float ltime)
        {
			time = ltime;
			for (int i = 0; i < ltime; i++)
            {
				time--;
				yield return MEC.Timing.WaitForSeconds(1f);
            }
        }
	}
}
