namespace Pro079Tesla
{
    using CommandSystem;
    using Exiled.API.Features;
    using MEC;
    using Pro079.API.Interfaces;
    using Pro079.Logic;
    using System;
    using System.Collections.Generic;

    public class TeslaCommand : ICommand079
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (Pro079Tesla.Singleton.IsActive)
            {
                response = Pro079Tesla.Singleton.Translations.OnCooldown.ReplaceAfterToken('$', new[]
                {
                    new Tuple<string, object>("time", _secondsRemaining)
                });
                return false;
            }

            if (arguments.Count < 2 || !int.TryParse(arguments.At(1), out int seconds))
            {
                response = Pro079Tesla.Singleton.Translations.Usage;
                return false;
            }

            _plySender = Player.Get((sender as CommandSender)?.SenderId);

            Pro079Tesla.Singleton.CoroutineHandle = Timing.RunCoroutine(TeslaCountdown(seconds));
            response = Pro079.Pro079.Singleton.Translations.Success;
            return true;
        }

        public string Command => Pro079Tesla.Singleton.Translations.Command;
        public string[] Aliases => Array.Empty<string>();
        public string Description => Pro079Tesla.Singleton.Translations.Description;

        public string ExtraArguments => string.Empty;
        public bool Cassie => false;
        public int Cooldown => Pro079Tesla.Singleton.Config.Cooldown;
        public int MinLevel => Pro079Tesla.Singleton.Config.Level;
        public int Cost => Pro079Tesla.Singleton.Config.Cost;
        public string CommandReady => Pro079Tesla.Singleton.Translations.CommandReady;

        private Player _plySender;
        private int _secondsRemaining;

        private IEnumerator<float> TeslaCountdown(int seconds)
        {
            Pro079Tesla.Singleton.IsActive = true;
            _secondsRemaining = seconds;
            for (int i = 0; i < seconds; i++)
            {
                if (Pro079Tesla.Singleton.Config.GrantExperience)
                    _plySender.Experience++;

                _secondsRemaining--;
                yield return 0f;
            }

            Pro079Tesla.Singleton.IsActive = false;
        }
    }
}