using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exiled.API.Features;
using Pro079Core.API;

namespace Pro079Core
{
    public class Pro079Manager
	{
		public static Pro079Manager Manager;
		public int CassieCd = 0;
		/// <summary>
		/// The remaining seconds for CASSIE to be active.
		/// </summary>
		public int CassieCooldown
		{
			// The logic demands a high IQ or a lot of knowledge in C# to be understood.
			set => CassieCd = Round.ElapsedTime.Seconds + value;
			get
			{
				int cd = CassieCd - Round.ElapsedTime.Seconds;
				return cd <= 0 ? 0 : cd;
			}
		}

		/// <summary>
		/// Dictionary with all the Commands and their respective handlers
		/// </summary>
		public Dictionary<string, ICommand079> Commands = new Dictionary<string, ICommand079>();
		/// <summary>
		/// Function used to register the current command. Doesn't register EventHandlers, so be aware of that.
		/// </summary>
		/// <param name="CommandHandler">The class that implements ICommand079</param>
		/// <returns></returns>
		public string RegisterCommand(ICommand079 CommandHandler)
		{
			if (CommandHandler == null || string.IsNullOrEmpty(CommandHandler.Command))
			{
				return "Trying to register a \"null\" or an empty name is not allowed!";
			}
			if (Commands.ContainsKey(CommandHandler.Command))
			{
				return "You can't register the same command twice!";
			}
			Commands.Add(CommandHandler.Command, CommandHandler);
			return "Command succesfully added.";
		}
		/// <summary>
		/// Dictionary with all the Ultimates and their respective handlers
		/// </summary>
		public Dictionary<string, IUltimate079> Ultimates = new Dictionary<string, IUltimate079>();
		/// <summary>
		/// Function used to register the current command. Doesn't register EventHandlers, so be aware of that.
		/// </summary>
		/// <param name="UltimateHandler">The class that implements ICommand079</param>
		/// <returns></returns>
		public string RegisterUltimate(IUltimate079 UltimateHandler)
		{
			if (UltimateHandler == null || string.IsNullOrEmpty(UltimateHandler.Name))
			{
				return "Trying to register a \"null\" or an empty name is not allowed!";
			}
			string name = UltimateHandler.Name.ToLower();
			if (Ultimates.ContainsKey(name))
			{
				return "An ultimate called " + UltimateHandler.Name + " was already added!";
			}
			Ultimates.Add(name, UltimateHandler);
			return "Ultimate succesfully added.";	
		}

		/// <summary>
		/// Properly sets the cooldown for the given command, and delays a broadcast to tell the user when it's ready if the property <see cref="CommandReady"/> has been set.
		/// </summary>
		/// <param name="Command">Handler</param>
		/// <param name="CustomValue">Change it customized, doesn't need to be set</param>
		public void SetOnCooldown(ICommand079 Command, int CustomValue = -1)
		{
			if (CustomValue > -1)
			{
				Command.CurrentCooldown = Round.ElapsedTime.Seconds + CustomValue;
				if (!string.IsNullOrEmpty(Command.CommandReady))
				{
					int p = (int)Environment.OSVersion.Platform;
					if ((p == 4) || (p == 6) || (p == 128)) MEC.Timing.RunCoroutine(DelayMessage(Command.CommandReady, CustomValue), MEC.Segment.Update);
					else MEC.Timing.RunCoroutine(DelayMessage(Command.CommandReady, CustomValue), 1);
				}
			}
			else
			{
				Command.CurrentCooldown = Round.ElapsedTime.Seconds + Command.Cooldown;
				if (!string.IsNullOrEmpty(Command.CommandReady))
				{
					int p = (int)Environment.OSVersion.Platform;
					if ((p == 4) || (p == 6) || (p == 128)) MEC.Timing.RunCoroutine(DelayMessage(Command.CommandReady, Command.Cooldown), MEC.Segment.Update);
					else MEC.Timing.RunCoroutine(DelayMessage(Command.CommandReady, Command.Cooldown), 1);
				}
			}

		}
		public int UltCooldown = 0;
		/// <summary>
		/// Remaining seconds for the ultimates to be ready, in seconds. 0 means it has no cooldown
		/// </summary>
		public int UltimateCooldown
		{
			// The logic demands a high IQ or a lot of knowledge in C# to be understood.
			set => UltCooldown = Round.ElapsedTime.Seconds + value;
			get
			{
				int cd = UltCooldown - Round.ElapsedTime.Seconds;
				return cd <= 0 ? 0 : cd;
			}
		}
		public void SetOnCooldown(IUltimate079 Ultimate)
		{
			UltimateCooldown = Ultimate.Cooldown + Round.ElapsedTime.Seconds;

			if (!string.IsNullOrEmpty(Pro079.Instance.Config.Translations.UltReady) || Pro079.Instance.Config.Translations.UltReady == "disable" || Pro079.Instance.Config.Translations.UltReady == "disabled" || Pro079.Instance.Config.Translations.UltReady == "none" || Pro079.Instance.Config.Translations.UltReady == "null")
			{
				int p = (int)Environment.OSVersion.Platform;
				if ((p == 4) || (p == 6) || (p == 128)) MEC.Timing.RunCoroutine(DelayMessage(Pro079.Instance.Config.Translations.UltReady, Ultimate.Cooldown), MEC.Segment.Update);
				else MEC.Timing.RunCoroutine(DelayMessage(Pro079.Instance.Config.Translations.UltReady, Ultimate.Cooldown), 1);

			}
		}
		private IEnumerator<float> DelayMessage(string message, int delay)
		{
			yield return MEC.Timing.WaitForSeconds(delay);
			IEnumerable<Player> pcs = Player.List.Where(x => x.Role == RoleType.Scp079);
			foreach (Player pc in pcs) pc.Broadcast(6, message, Broadcast.BroadcastFlags.Normal);
		}
		/// <summary>
		/// Properly gives the player XP. Must be done per-command.
		/// </summary>
		/// <param name="player">The player to give XP to</param>
		/// <param name="XP">The amount of XP</param>
		public void GiveExp(Player player, float XP)
		{
			player.Experience += XP;
		}
		/// <summary>
		/// Drains the AP from a player. Will never go below 0. Negative amounts probably add AP instead of draining it.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="amount"></param>
		public void DrainAP(Player player, float amount)
		{
			if (player.Energy < amount) player.Energy = 0;
			else player.Energy -= amount;
		}
		/// <summary>
		/// Gets the ultimate based on the name or based on how it starts.
		/// </summary>
		/// <param name="Name"></param>
		/// <returns></returns>
		public IUltimate079 GetUltimate(string Name)
		{
			if (Pro079.Manager.Ultimates.TryGetValue(Name, out IUltimate079 ultimate))
			{
				return ultimate;
			}
			string name = Pro079.Manager.Ultimates.Keys.OrderBy(x => x.Length).FirstOrDefault(x => x.StartsWith(Name));
			if (name == null)
			{
				return null;
			}
			return Pro079.Manager.Ultimates[name];
		}

		//Nabbed from https://gist.github.com/RogerFK/b155626b3d987f9c81510e17bcd72c00
		/// <summary>
		/// Optimized method that replaces a <see cref="string"/> based on an <see cref="Tuple[]"/>
		/// </summary>
		/// <param name="source">The string to use as source</param>
		/// <param name="token">The starting token</param>
		/// <param name="valuePairs">The value pairs (tuples) to use as "key -> value"</param>
		/// <returns>The string after replacement</returns>
		public string ReplaceAfterToken(string source, char token, Tuple<string, object>[] valuePairs)
		{
			if (valuePairs == null)
			{
				throw new ArgumentNullException("valuePairs");
			}
			StringBuilder builder = new StringBuilder(Convert.ToInt32(Math.Ceiling(source.Length * 1.5f)));

			int i = 0;
			int sourceLength = source.Length;

			do
			{
				// Append characters until you find the token
				char auxChar = token == char.MaxValue ? (char)(char.MaxValue - 1) : char.MaxValue;
				for (; i < sourceLength && (auxChar = source[i]) != token; i++) builder.Append(auxChar);

				// Ensures no weird stuff regarding token being null
				if (auxChar == token)
				{
					int movePos = 0;

					// Try to find a tuple
					foreach (Tuple<string, object> kvp in valuePairs)
					{
						int j, k;
						for (j = 0, k = i + 1; j < kvp.Item1.Length && k < source.Length && source[k] == kvp.Item1[j]; j++, k++) ;
						// General condition for "key found"
						if (j == kvp.Item1.Length)
						{
							movePos = j;
							builder.Append(kvp.Item2); // append what we're replacing the key with
							break;
						}
					}
					// Don't skip the token if you didn't find the key, append it
					if (movePos == 0) builder.Append(token);
					else i += movePos;
				}
				i++;
			} while (i < sourceLength);

			return builder.ToString();
		}
		//End of nab

		//////////////////////////////////////////////////////
		//				   LANGUAGE OPTIONS					//
		//////////////////////////////////////////////////////

		/// <summary>
		/// Gets the translated string for level. Something like: "level {number}"
		/// </summary>
		/// <param name="Level">The number of the level</param>
		/// <param name="Uppercase">If the first character should be uppercase</param>
		/// <returns>The translated level string</returns>
		public string LevelString(int Level, bool Uppercase)
		{
			if (Uppercase || char.IsDigit(Pro079.Instance.Config.Translations.Level[0]))
			{
				return char.ToUpper(Pro079.Instance.Config.Translations.Level[0])
					+ Pro079.Instance.Config.Translations.Level.Substring(1).Replace("$lvl", Level.ToString());
			}

			return Pro079.Instance.Config.Translations.Level.Replace("$lvl", Level.ToString());
		}
		/// <summary>
		/// Gets the translated string for AP. Something like: "{number} AP"
		/// </summary>
		/// <param name="AP">The number of the AP</param>
		/// <param name="Uppercase">If the first character should be uppercase</param>
		/// <returns>The translated AP string</returns>
		public string APString(int AP, bool Uppercase)
		{
			if (Uppercase || char.IsDigit(Pro079.Instance.Config.Translations.Level[0]))
			{
				return char.ToUpper(Pro079.Instance.Config.Translations.Level[0])
					 + Pro079.Instance.Config.Translations.Level.Substring(1).Replace("$ap", AP.ToString());
			}

			return Pro079.Instance.Config.Translations.Level.Replace("$ap", AP.ToString());
		}
		/// <summary>
		/// Gets the "Not enough AP (you need $min)" but translated
		/// </summary>
		/// <param name="MinAP">The minimum AP</param>
		/// <returns>The translated string</returns>
		public string LowAP(int MinAP)
		{
			return Min(Pro079.Instance.Config.Translations.LowMana, MinAP);
		}
		/// <summary>
		/// Gets the "Your level is too low (you need $min)" but translated
		/// </summary>
		/// <param name="MinLevel">The required level</param>
		/// <returns>The translated string</returns>
		public string LowLevel(int MinLevel)
		{
			return Min(Pro079.Instance.Config.Translations.LowLevel, MinLevel);
		}
		private string Min(string str, int number)
		{
			return str.Replace("$min", number.ToString());
		}
		/// <summary>
		/// Returns the cooldown translated
		/// </summary>
		public string CmdOnCooldown(int Cooldown)
		{
			return Pro079.Instance.Config.Translations.Cooldown.Replace("$cd", Cooldown.ToString());
		}

		/// <summary>
		/// Translated "This command is disabled."
		/// </summary>
		public string CommandDisabled => Pro079.Instance.Config.Translations.Disabled;
		/// <summary>
		/// Translated "Command succesfully launched".
		/// </summary>
		public string CommandSuccess => Pro079.Instance.Config.Translations.Success;
		/// <summary>
		/// Translated "Ultimate succesfully used."
		/// </summary>
		public string UltimateLaunched => Pro079.Instance.Config.Translations.UltLaunched;
	}
}