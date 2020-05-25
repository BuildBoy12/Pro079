using System;
using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using EXILED.Patches;
using MEC;
using Pro079Core.API;

namespace Pro079Core
{
	public static class Pro079Logic
	{
		////////////////////////////////
		//			  HELP			  //
		////////////////////////////////
		private static List<string> Help;
		private static string FormatEnergyLevel(int energy, int level, string energStr, string lvlStr)
		{
			string str;
			if (energy > 0)
			{
				str = "(" + energStr.Replace("$ap", energy.ToString())
					+ (level > 1 ? ", " + lvlStr.Replace("$lvl", level.ToString()) : "") + ')';
				return str;
			}
			if (energy <= 0 && level > 1)
			{
				str = "(" + FirstCharToUpper(lvlStr.Replace("$lvl", level.ToString())) + ')';
				return str;
			}
			return string.Empty;
		}
		private static void FetchExternalHelp()
		{
			Help = new List<string>(Pro079.Manager.Commands.Keys.Count);
			foreach (KeyValuePair<string, ICommand079> kvp in Pro079.Manager.Commands)
			{
				if (!kvp.Value.Disabled) Help.Add($"<b>.079 {kvp.Key + (!string.IsNullOrEmpty(kvp.Value.ExtraArguments) ? " " + kvp.Value.ExtraArguments : string.Empty)}</b> - {kvp.Value.HelpInfo} {FormatEnergyLevel(kvp.Value.APCost, kvp.Value.MinLevel, Pro079.Instance.energy, Pro079.Instance.level)}");
			}
		}
		internal static string GetHelp()
		{
			string help = Pro079.basicHelp;
			if (Help == null || Help.Count != Pro079.Manager.Commands.Keys.Count) FetchExternalHelp();
			foreach (string line in Help)
			{
				help += Environment.NewLine + line;
			}
			if (Pro079.Instance.suicide) help += Environment.NewLine + $"<b>.079 {Pro079.Instance.suicidecmd}</b> - " + Pro079.Instance.suicidehelp;
			if (Pro079.Instance.ult) help += Environment.NewLine + $"<b>.079 {Pro079.Instance.ultcmd}</b> - " + Pro079.Instance.ulthelp;
			if (Pro079.Instance.tips) help += Environment.NewLine + $"<b>.079 {Pro079.Instance.tipscmd}</b> - " + Pro079.Instance.tipshelp;
			return help;
		}
		private static List<string> UltimateHelp;
		private static void FetchUltimates()
		{
			UltimateHelp = new List<string>(Pro079.Manager.Ultimates.Keys.Count);
			foreach (KeyValuePair<string, IUltimate079> kvp in Pro079.Manager.Ultimates)
			{
				UltimateHelp.Add($"<b>.079 {Pro079.Instance.ultcmd} {kvp.Key}</b> - {kvp.Value.Info} {Pro079.Instance.ultdata.Replace("$cd", kvp.Value.Cooldown.ToString()).Replace("$cost", kvp.Value.Cost.ToString())}");
			}
		}
		internal static string GetUltimates()
		{
			string help = Pro079.Instance.ultusageFirstline;
			if (UltimateHelp == null || UltimateHelp.Count != Pro079.Manager.Ultimates.Keys.Count) FetchUltimates();
			foreach (string line in UltimateHelp)
			{
				help += Environment.NewLine + line;
			}
			return help;
		}

		// This thing below was pasted from here: https://www.c-sharpcorner.com/blogs/first-letter-in-uppercase-in-c-sharp1
		internal static string FirstCharToUpper(string s)
		{
			// Check for empty string.
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			// Return char and concat substring.
			return char.ToUpper(s[0]) + s.Substring(1);
		}

		///////////////////////////////////////
		//	 		LOGIC FUNCTIONS			 //
		///////////////////////////////////////
		internal static IEnumerator<float> DelaySpawnMsg(ReferenceHub player)
		{
			yield return Timing.WaitForSeconds(0.1f);
			if (player.GetRole() == RoleType.Scp079)
			{
				player.ClearBroadcasts();
				player.Broadcast(20, Pro079.broadcastMsg, false);
				player.SendConsoleMessage(GetHelp(), "white");
			}
		}
		/// <summary>
		/// Fakes a suicide/suicides the given player (6th generator)
		/// </summary>
		public static IEnumerator<float> SixthGen(ReferenceHub player = null)
		{
			PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement("SCP079RECON6", false, false);
			PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement("SCP 0 7 9 CONTAINEDSUCCESSFULLY", false, false);
			for (int j = 0; j < 350; j++)
			{
				yield return Timing.WaitForSeconds(0f);
			}
			Generator079.mainGenerator.CallRpcOvercharge();
			foreach (Door door in EventHandlers.DoorArray)
			{
				Scp079Interactable component = door.GetComponent<Scp079Interactable>();
				if (component.currentZonesAndRooms[0].currentZone == "HeavyRooms" && door.isOpen && !door.locked && !door.destroyed)
				{
					door.ChangeState(true);
				}
			}
			if (player != null) player.SetRole(RoleType.Spectator);
			Recontainer079.isLocked = true;
			for (int k = 0; k < 500; k++)
			{
				yield return Timing.WaitForSeconds(0f);
			}
			Recontainer079.isLocked = false;
		}
		/// <summary>
		/// Does the whole recontainment process the same way as main game does.
		/// </summary>
		public static IEnumerator<float> Fake5Gens()
		{
			// People complained about it being "easy to be told apart". Not anymore.
			MTFRespawn mtf = PlayerManager.localPlayer.GetComponent<MTFRespawn>();
			 NineTailedFoxAnnouncer annc = NineTailedFoxAnnouncer.singleton;
			while (annc.queue.Count > 0 || AlphaWarheadController.Host.inProgress)
			{
				yield return Timing.WaitForSeconds(0f);
			}
			mtf.CallRpcPlayCustomAnnouncement("SCP079RECON5", false, true);
			// This massive for loop jank is what the main game does. Go complain to them.
			for (int i = 0; i < 2750; i++)
			{
				yield return Timing.WaitForSeconds(0f);
			}
			while (annc.queue.Count > 0 || AlphaWarheadController.Host.inProgress)
			{
				yield return Timing.WaitForSeconds(0f);
			}
			mtf.CallRpcPlayCustomAnnouncement("SCP079RECON6", false, false);
			mtf.CallRpcPlayCustomAnnouncement("SCP 0 7 9 CONTAINEDSUCCESSFULLY", false, false);
			for (int j = 0; j < 350; j++)
			{
				yield return Timing.WaitForSeconds(0f);
			}
			Generator079.generators[0].CallRpcOvercharge();
			foreach (Door door in UnityEngine.Object.FindObjectsOfType<Door>())
			{
				Scp079Interactable component = door.GetComponent<Scp079Interactable>();
				if (component.currentZonesAndRooms[0].currentZone == "HeavyRooms" && door.isOpen && !door.locked)
				{
					door.ChangeState(true);
				}
			}
			Recontainer079.isLocked = true;
			for (int k = 0; k < 500; k++)
			{
				yield return Timing.WaitForSeconds(0f);
			}
			Recontainer079.isLocked = false;
		}

		internal static IEnumerator<float> CooldownCassie(float time)
		{
			if (time > 5)
			{
				yield return MEC.Timing.WaitForSeconds(time);

				IEnumerable<ReferenceHub> PCplayers = Player.GetHubs(RoleType.Scp079);
				foreach (ReferenceHub player in PCplayers)
				{
					player.Broadcast(3, Pro079.Instance.cassieready, false);
				}
			}
		}
		internal static IEnumerator<float> DelayKysMessage(IEnumerable<ReferenceHub> PCplayers)
		{
			if (string.IsNullOrEmpty(Pro079.Instance.kys)) yield break;
			yield return Timing.WaitForSeconds(0.3f);
			if (Team.SCP.GetHubs().Count() - PCplayers.Count() == 0)
			{
				foreach (ReferenceHub player in PCplayers)
				{
					player.Broadcast(20, Pro079.Instance.kys, false);
				}
			}
		}
	}
}
