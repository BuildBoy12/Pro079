namespace Pro079.Handlers
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using Logic;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PlayerHandlers
    {
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.NewRole != RoleType.Scp079)
            {
                Manager.CassieCooldowns.Remove(ev.Player);
                return;
            }

            Manager.CassieCooldowns[ev.Player] = DateTime.Now;
            Manager.UltimateCooldowns[ev.Player] = DateTime.Now;

            if (Pro079.Singleton.Config.EnableSpawnBroadcast)
            {
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(10, "");
            }

            ev.Player.SendConsoleMessage("", "white");
        }

        public void OnDied(DiedEventArgs ev)
        {
            if (!Pro079.Singleton.Config.SuicideCommand)
                return;

            List<Player> pcPlayers = Player.Get(RoleType.Scp079).ToList();
            int pcCount = pcPlayers.Count;
            if (ev.Target.Team != Team.SCP ||
                ev.Target.Role == RoleType.Scp079 ||
                pcCount < 0 ||
                Player.Get(Team.SCP).Count() - pcCount > 0)
                return;

            Manager.CanSuicide = true;
            if (string.IsNullOrEmpty(Pro079.Singleton.Translations.Kys))
                return;

            for (int i = 0; i < pcCount; i++)
                pcPlayers[i].Broadcast(5, "");
        }
    }
}