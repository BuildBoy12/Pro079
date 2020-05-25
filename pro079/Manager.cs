using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using Pro079Core.API;

namespace Pro079Core
{
	public class Pro079Manager
	{
		public static Pro079Manager Manager;
		private readonly Pro079 plugin;
		public Pro079Manager(Pro079 plugin)
		{
			this.plugin = plugin;
			Manager = this;
		}
		public int CassieCd = 0;
		/// <summary>
		/// The remaining seconds for CASSIE to be active.
		/// </summary>
		public int CassieCooldown
		{
			// The logic demands a high IQ or a lot of knowledge in C# to be understood.
			set => CassieCd = RoundSummary.roundTime + value;
			get
			{
				int cd = CassieCd - RoundSummary.roundTime;
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
				return "Trying to register a \"null\" or an empty Command is not allowed!";
			}
			if (Commands.ContainsKey(CommandHandler.Command))
			{
				return "You can't register the same command twice!";
			}
			Commands.Add(CommandHandler.Command, CommandHandler);
			return "Command succesfully added";
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
			return "Ultimate succesfully added";
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
				Command.CurrentCooldown = RoundSummary.roundTime + CustomValue;
				if (!string.IsNullOrEmpty(Command.CommandReady))
				{
					int p = (int)System.Environment.OSVersion.Platform;
					if ((p == 4) || (p == 6) || (p == 128)) MEC.Timing.RunCoroutine(DelayMessage(Command.CommandReady, CustomValue), MEC.Segment.Update);
					else MEC.Timing.RunCoroutine(DelayMessage(Command.CommandReady, CustomValue), 1);
				}
			}
			else
			{
				Command.CurrentCooldown = RoundSummary.roundTime + Command.Cooldown;
				if (!string.IsNullOrEmpty(Command.CommandReady))
				{
					int p = (int)System.Environment.OSVersion.Platform;
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
			set => UltCooldown = RoundSummary.roundTime + value;
			get
			{
				int cd = UltCooldown - RoundSummary.roundTime;
				return cd <= 0 ? 0 : cd;
			}
		}
		public void SetOnCooldown(IUltimate079 Ultimate)
		{
			UltimateCooldown = Ultimate.Cooldown + RoundSummary.roundTime;

			if (!string.IsNullOrEmpty(plugin.ultready) || plugin.ultready == "disable" || plugin.ultready == "disabled" || plugin.ultready == "none" || plugin.ultready == "null")
			{
				int p = (int)System.Environment.OSVersion.Platform;
				if ((p == 4) || (p == 6) || (p == 128)) MEC.Timing.RunCoroutine(DelayMessage(plugin.ultready, Ultimate.Cooldown), MEC.Segment.Update);
				else MEC.Timing.RunCoroutine(DelayMessage(plugin.ultready, Ultimate.Cooldown), 1);
			}
		}
		private IEnumerator<float> DelayMessage(string message, int delay)
		{
			yield return MEC.Timing.WaitForSeconds(delay);
			IEnumerable<ReferenceHub> pcs = Player.GetHubs(RoleType.Scp079);
			foreach (ReferenceHub pc in pcs) pc.Broadcast(6, message, false);
		}
		/// <summary>
		/// Properly gives the player XP. Must be done per-command.
		/// </summary>
		/// <param name="player">The player to give XP to</param>
		/// <param name="XP">The amount of XP</param>
		public void GiveExp(ReferenceHub player, float XP)
		{
			player.AddExperience(XP);
		}
		/// <summary>
		/// Drains the AP from a player. Will never go below 0. Negative amounts probably add AP instead of draining it.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="amount"></param>
		public void DrainAP(ReferenceHub player, float amount)
		{
			if (player.GetEnergy() < amount) player.SetEnergy(0);
			else player.SetEnergy(player.GetEnergy() - amount);
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
	}
}
