namespace Pro079.Logic
{
    using API.Interfaces;
    using Exiled.API.Features;
    using MEC;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static partial class Manager
    {
        public static readonly Dictionary<Player, DateTime> CassieCooldowns = new Dictionary<Player, DateTime>();

        public static readonly Dictionary<Player, List<ICommand079>> CommandCooldowns =
            new Dictionary<Player, List<ICommand079>>();

        public static readonly Dictionary<Player, DateTime> UltimateCooldowns = new Dictionary<Player, DateTime>();
        internal static readonly List<ICommand079> Commands = new List<ICommand079>();
        internal static readonly List<IUltimate079> Ultimates = new List<IUltimate079>();
        internal static readonly List<CoroutineHandle> CoroutineHandles = new List<CoroutineHandle>();
        internal static bool CanSuicide;

        public static bool RegisterCommand(ICommand079 command079)
        {
            if (!Pro079.Singleton.Config.EnableModules || Commands.Contains(command079))
                return false;

            Commands.Add(command079);
            return true;
        }

        public static bool RegisterUltimate(IUltimate079 ultimate079)
        {
            if (!Pro079.Singleton.Config.EnableUltimates || Ultimates.Contains(ultimate079))
                return false;

            Ultimates.Add(ultimate079);
            return true;
        }

        public static IEnumerator<float> SetOnCooldown(Player ply, ICommand079 command079)
        {
            if (command079.Cooldown == 0)
                yield break;

            if (!CommandCooldowns.ContainsKey(ply))
                CommandCooldowns.Add(ply, new List<ICommand079> {command079});
            else
                CommandCooldowns[ply].Add(command079);

            for (int i = 0; i < Application.targetFrameRate * command079.Cooldown; i++)
                yield return 0f;

            CommandCooldowns[ply].Remove(command079);
            if (!string.IsNullOrEmpty(command079.CommandReady))
                ply.Broadcast(10, command079.CommandReady);
        }
    }
}