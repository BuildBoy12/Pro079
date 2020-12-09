namespace Pro079Lockdown
{
    using CommandSystem;
    using Exiled.API.Features;
    using MEC;
    using Pro079.API.Interfaces;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class LockdownCommand : IUltimate079
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Timing.RunCoroutine(RunUltimate());
            response = Pro079.Pro079.Singleton.Translations.Success;
            return true;
        }

        public string Command => Pro079Lockdown.Singleton.Translations.Command;
        public string[] Aliases => Array.Empty<string>();
        public string Description => Pro079Lockdown.Singleton.Translations.Description;
        public int Cooldown => Pro079Lockdown.Singleton.Config.Cooldown;
        public int Cost => Pro079Lockdown.Singleton.Config.Cost;

        private IEnumerator<float> RunUltimate()
        {
            NineTailedFoxAnnouncer.singleton.ServerOnlyAddGlitchyPhrase(
                "Security Lockdown pitch_2 scp 0 7 9 . Override pitch_1 Detected . . Automatic Emergency Zone lockdown Initializing . 3 . 2 . 1",
                10, 20);
            yield return Timing.WaitForSeconds(10f);
            Pro079Lockdown.Singleton.IsActive = true;
            for (int i = 0; i < Application.targetFrameRate * Pro079Lockdown.Singleton.Config.LockdownDuration; i++)
                yield return 0f;

            Pro079Lockdown.Singleton.IsActive = false;
            Cassie.Message("Automatic Emergency zone lockdown . Disabled");
        }
    }
}