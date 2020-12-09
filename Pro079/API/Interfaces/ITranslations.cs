namespace Pro079.API.Interfaces
{
    public interface ITranslations
    {
        /// <summary>
        /// The command that a <see cref="ICommand079"/> or <see cref="IUltimate079"/> will use.
        /// </summary>
        string Command { get; set; }

        /// <summary>
        /// The description to be implemented in a <see cref="ICommand079"/> or <see cref="IUltimate079"/>.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Syntax and/or example for the command.
        /// </summary>
        string Usage { get; set; }
    }
}