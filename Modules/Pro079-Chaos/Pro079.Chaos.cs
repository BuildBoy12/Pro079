using Exiled.API.Features;
using Pro079Core;
using System;

namespace ChaosCommand
{
	public class ChaosPlugin : Plugin<Config>
	{
		private static readonly Lazy<ChaosPlugin> LazyInstance = new Lazy<ChaosPlugin>(() => new ChaosPlugin());
		private ChaosPlugin() { }
		public static ChaosPlugin ConfigRef => LazyInstance.Value;

		public override void OnEnabled()
		{
			base.OnEnabled();
			Pro079.Manager.RegisterCommand(new ChaosCommand());
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
		}
		
		public override string Name => "Pro079.Chaos";
        public override string Author => "Build";
    }
}