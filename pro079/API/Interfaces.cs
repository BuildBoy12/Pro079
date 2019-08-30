﻿using Smod2;
using Smod2.API;

namespace Pro079.API
{
	public interface ICommand079
	{
		//////////////////////////////////////////////////////
		//					  BASIC DATA					//
		//////////////////////////////////////////////////////
		/// <summary>
		/// If the command is currently disabled, useful for untested modules and alike.
		/// </summary>
		bool Disabled { set; get; }
		/// <summary>
		/// The name of the command
		/// </summary>
		string Command { get; }
		/// <summary>
		/// The string of info that will be displayed when ".079" is typed
		/// </summary>
		string HelpInfo { get; }

		//////////////////////////////////////////////////////
		//			LOGIC AND PER-COMMAND CONFIGS			//
		//////////////////////////////////////////////////////

		/// <summary>
		/// Function executed when the command is sent. This is where the logic goes.
		/// </summary>
		/// <param name="args">Array of strings with the extra arguments. Sometimes, it's not needed.</param>
		/// <param name="Player">Player who sent the command. This player contains the <see cref="Scp079Data"/> in case you want to modify anything about it.</param>
		/// <returns>The string to be returned to the Player's console.</returns>
		string CallCommand(string[] args, Smod2.API.Player Player);
		/// <summary>
		/// If it uses C.A.S.S.I.E. cooldowns, and sets it on cooldown or not. Optional.
		/// </summary>
		bool Cassie { get; }
		/// <summary>
		/// Cooldown before the command can be used again
		/// </summary>
		int Cooldown { get; }
		/// <summary>
		/// The minimum level the command needs for it to be launched
		/// </summary>
		int MinLevel { get; }
		/// <summary>
		/// Cost for the command to be launched
		/// </summary>
		int APCost { get; }
		/// <summary>
		/// Message to be sent to the player when the command is ready.
		/// Can be <see cref="string.Empty"/> or <see cref="null"/> to avoid broadcasting the player when the command is ready.
		/// </summary>
		string CommandReady { get; }
		/// <summary>
		/// Time when the command will be ready. 
		/// After each round, it gets resetted. This shouldn't get modified or initialized outside of Pro-079 Core in most cases. Remove both throw operations and you're good to go.
		/// Uses <see cref="PluginManager.Manager.Server.Round.Duration"/> as it's base.
		/// </summary>
		int CurrentCooldown { set; get; }
	}
	public interface IUltimate079
	{
		/// <summary>
		/// The name of the command. Can have spaces.
		/// </summary>
		string Name { get; }
		/// <summary>
		/// The info that will be shown about what the ultimate is about
		/// </summary>
		string Info { get; }
		/// <summary>
		/// How much cooldown the ultimate sets all other ultimates for
		/// </summary>
		int Cooldown { get; }
		/// <summary>
		/// If set to 0, won't be shown. AP cost for the ultimate.
		/// </summary>
		int Cost { get; }
		/// <summary>
		/// The logic of the function goes here
		/// </summary>
		/// <param name="args"></param>
		/// <param name="Player"></param>
		/// <returns></returns>
		string TriggerUltimate(string[] args, Smod2.API.Player Player);
	}
}
