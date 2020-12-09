namespace Pro079.API.Interfaces
{
    using CommandSystem;

    public interface ICommand079 : ICommand
    {
        //////////////////////////////////////////////////////
        //					  BASIC DATA					//
        //////////////////////////////////////////////////////

        /// <summary>
        /// Can be <see cref="string.Empty"/> if not needed. Usage for the command that goes after the command itself, aka .079 [command] [usage]
        /// </summary>
        string ExtraArguments { get; }

        //////////////////////////////////////////////////////
        //			LOGIC AND PER-COMMAND CONFIGS			//
        //////////////////////////////////////////////////////

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
        int Cost { get; }

        /// <summary>
        /// Message to be sent to the player when the command is ready.
        /// Can be <see cref="string.Empty"/> or <see cref="null"/> to avoid broadcasting the player when the command is ready.
        /// </summary>
        string CommandReady { get; }
    }
}