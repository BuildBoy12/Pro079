namespace Pro079Blackout
{
    using CommandSystem;
    using Exiled.API.Features;
    using MEC;
    using Pro079.API.Interfaces;
    using System;
    using System.Collections.Generic;

    public class BlackoutCommand : IUltimate079
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Timing.RunCoroutine(RunUltimate());
            response = Pro079.Pro079.Singleton.Translations.Success;
            return true;
        }

        public string Command => Pro079Blackout.Singleton.Translations.Command;
        public string[] Aliases => Array.Empty<string>();
        public string Description => Pro079Blackout.Singleton.Translations.Description;
        public int Cooldown => Pro079Blackout.Singleton.Config.Cooldown;
        public int Cost => Pro079Blackout.Singleton.Config.Cost;

        private IEnumerator<float> RunUltimate()
        {
            Cassie.Message(
                "warning . malfunction detected on heavy containment zone . Scp079Recon6 . . . light systems Disengaged");
            yield return Timing.WaitForSeconds(12.1f);
            Map.TurnOffAllLights(Pro079Blackout.Singleton.Config.BlackoutDuration);
        }
    }
}