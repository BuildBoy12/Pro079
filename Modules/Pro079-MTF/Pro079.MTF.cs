using Exiled.API.Features;

namespace Pro079MTF
{
    public class MTFPlugin : Plugin<Config>
	{	
		public override void OnEnabled()
		{
			base.OnEnabled();
			Pro079Core.Pro079.Manager.RegisterCommand(new MTFCommand(this));
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
		}

		public override string Name => "Pro079.MTF";
		public override string Author => "Build";
	}
}