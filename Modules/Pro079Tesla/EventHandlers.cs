namespace Pro079Tesla
{
    using Exiled.Events.EventArgs;

    public class EventHandlers
    {
        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            ev.IsTriggerable = !Pro079Tesla.Singleton.IsActive;
        }
    }
}