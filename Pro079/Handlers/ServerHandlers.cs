namespace Pro079.Handlers
{
    using Logic;

    public class ServerHandlers
    {
        public void OnWaitingForPlayers()
        {
            Manager.CassieCooldowns.Clear();
            Manager.CommandCooldowns.Clear();
            Manager.UltimateCooldowns.Clear();
            Manager.CanSuicide = false;
        }
    }
}