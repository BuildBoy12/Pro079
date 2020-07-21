using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
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
				Help.Add($"<b>.079 {kvp.Key + (!string.IsNullOrEmpty(kvp.Value.ExtraArguments) ? " " + kvp.Value.ExtraArguments : string.Empty)}</b> - {kvp.Value.HelpInfo} {FormatEnergyLevel(kvp.Value.APCost, kvp.Value.MinLevel, Pro079.ConfigRef.Config.Translations.Energy, Pro079.ConfigRef.Config.Translations.Level)}");
			}
		}
		internal static string GetHelp()
		{
			string help = Pro079.ConfigRef.Config.Translations.BasicHelp;
			if (Help == null || Help.Count != Pro079.Manager.Commands.Keys.Count) FetchExternalHelp();
			foreach (string line in Help)
			{
				help += Environment.NewLine + line;
			}
			if (Pro079.Instance.Config.SuicideCommand) help += Environment.NewLine + $"<b>.079 {Pro079.ConfigRef.Config.Translations.SuicideCmd}</b> - " + Pro079.ConfigRef.Config.Translations.SuicideHelp;
			if (Pro079.Instance.Config.EnableUltimates) help += Environment.NewLine + $"<b>.079 {Pro079.ConfigRef.Config.Translations.UltCmd}</b> - " + Pro079.ConfigRef.Config.Translations.UltHelp;
			if (Pro079.Instance.Config.EnableTips) help += Environment.NewLine + $"<b>.079 {Pro079.ConfigRef.Config.Translations.TipsCmd}</b> - " + Pro079.ConfigRef.Config.Translations.TipsHelp;
			return help;
		}
		private static List<string> UltimateHelp;
		private static void FetchUltimates()
		{
			UltimateHelp = new List<string>(Pro079.Manager.Ultimates.Keys.Count);
			foreach (KeyValuePair<string, IUltimate079> kvp in Pro079.Manager.Ultimates)
			{
				string HelpMsg = $"<b>.079 {Pro079.ConfigRef.Config.Translations.UltCmd} {kvp.Key}</b> - {kvp.Value.Info} {Pro079.ConfigRef.Config.Translations.UltData}";
				UltimateHelp.Add(Pro079.Manager.ReplaceAfterToken(HelpMsg, '$', new Tuple<string, object>[]
				{
					new Tuple<string, object>("cd", kvp.Value.Cooldown),
					new Tuple<string, object>("cost", kvp.Value.Cost),
				}));
			}
		}
		internal static string GetUltimates()
		{
			string help = Pro079.ConfigRef.Config.Translations.UltUsageFirstLine;
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
		internal static IEnumerator<float> DelaySpawnMsg(Player player)
		{
			yield return Timing.WaitForSeconds(0.1f);
			if (player.Role == RoleType.Scp079)
			{
				player.ClearBroadcasts();
				player.Broadcast(20, Pro079.ConfigRef.Config.Translations.BroadcastMsg, Broadcast.BroadcastFlags.Normal);
				player.SendConsoleMessage(GetHelp(), "white");
			}
		}
		/// <summary>
		/// Fakes a suicide/suicides the given player (6th generator)
		/// </summary>
		public static IEnumerator<float> SixthGen(Player player = null)
		{
			Respawning.RespawnEffectsController.PlayCassieAnnouncement("SCP079RECON6", false, true);
			Respawning.RespawnEffectsController.PlayCassieAnnouncement("SCP 0 7 9 CONTAINEDSUCCESSFULLY", false, false);

			for (int j = 0; j < 350; j++)
			{
				yield return Timing.WaitForSeconds(0f);
			}
			Generator079.mainGenerator.CallRpcOvercharge();
			foreach (Door door in Map.Doors)
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
			NineTailedFoxAnnouncer annc = NineTailedFoxAnnouncer.singleton;
			while (annc.queue.Count > 0 || AlphaWarheadController.Host.inProgress)
			{
				yield return Timing.WaitForSeconds(0f);
			}
			Respawning.RespawnEffectsController.PlayCassieAnnouncement("SCP079RECON5", false, true);
			// This massive for loop jank is what the main game does. Go complain to them.
			for (int i = 0; i < 2750; i++)
			{
				yield return Timing.WaitForSeconds(0f);
			}
			while (annc.queue.Count > 0 || AlphaWarheadController.Host.inProgress)
			{
				yield return Timing.WaitForSeconds(0f);
			}
			Respawning.RespawnEffectsController.PlayCassieAnnouncement("SCP079RECON6", false, true);
			Respawning.RespawnEffectsController.PlayCassieAnnouncement("SCP 0 7 9 CONTAINEDSUCCESSFULLY", false, false);
			for (int j = 0; j < 350; j++)
			{
				yield return Timing.WaitForSeconds(0f);
			}
			Generator079.mainGenerator.CallRpcOvercharge();
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

				IEnumerable<Player> PCplayers = Player.List.Where(x => x.Role == RoleType.Scp079);
				foreach (Player player in PCplayers)
				{
					player.Broadcast(3, Pro079.ConfigRef.Config.Translations.CassieReady, Broadcast.BroadcastFlags.Normal);
				}
				
			}
		}
		internal static IEnumerator<float> DelayKysMessage(IEnumerable<Player> PCplayers)
		{
			if (string.IsNullOrEmpty(Pro079.ConfigRef.Config.Translations.kys)) yield break;
			yield return Timing.WaitForSeconds(0.3f);
			if (Player.List.Where(x => x.Team == Team.SCP).Count() - PCplayers.Count() == 0)
			{
				foreach (Player player in PCplayers)
				{
					player.Broadcast(20, Pro079.ConfigRef.Config.Translations.kys, Broadcast.BroadcastFlags.Normal);
				}
			}
		}
	}
}
