namespace Pro079.API.Interfaces
{
    using CommandSystem;

    public interface IUltimate079 : ICommand
    {
        /// <summary>
        /// How much cooldown the ultimate sets all other ultimates for
        /// </summary>
        int Cooldown { get; }

        /// <summary>
        /// If set to 0, won't be shown. AP cost for the ultimate.
        /// </summary>
        int Cost { get; }
    }
}