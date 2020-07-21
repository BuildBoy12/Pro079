using Exiled.API.Features;
using System;

namespace Pro079MTF
{
	public class MTFPlugin : Plugin<Config>
	{
		private static readonly Lazy<MTFPlugin> LazyInstance = new Lazy<MTFPlugin>(() => new MTFPlugin());
		private MTFPlugin() { }
		public static MTFPlugin ConfigRef => LazyInstance.Value;
	
		public override void OnEnabled()
		{
			base.OnEnabled();
			Pro079Core.Pro079.Manager.RegisterCommand(new MTFCommand());
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
		}

		public override string Name => "Pro079.MTF";
		public override string Author => "Build";
	}
}