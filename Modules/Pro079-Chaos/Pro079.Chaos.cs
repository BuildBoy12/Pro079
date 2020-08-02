using Exiled.API.Features;
using Pro079Core;
using System;

namespace ChaosCommand
{
    public class ChaosPlugin : Plugin<Config>
	{
		public override void OnEnabled()
		{
			base.OnEnabled();
			Pro079.Manager.RegisterCommand(new ChaosCommand(this));
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
		}
		
		public override string Name => "Pro079.Chaos";
        public override string Author => "Build";
		public override Version RequiredExiledVersion => new Version(2, 0, 9);
	}
}