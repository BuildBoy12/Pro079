namespace Pro079Lockdown
{
    using Exiled.Events.EventArgs;

    public class EventHandlers
    {
        public void OnWaitingForPlayers() => Pro079Lockdown.Singleton.IsActive = false;

        public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (!Pro079Lockdown.Singleton.IsActive || ev.Door.PermissionLevels == 0)
                return;

            ev.IsAllowed = ev.Player.Team == Team.SCP;
        }
    }
}