using Exiled.API.Features;
using System;

namespace SCPCommand
{
	public class SCPPlugin : Plugin<Config>
	{
		private static readonly Lazy<SCPPlugin> LazyInstance = new Lazy<SCPPlugin>(() => new SCPPlugin());
		private SCPPlugin() { }
		public static SCPPlugin ConfigRef => LazyInstance.Value;

		public override void OnEnabled()
		{
			base.OnEnabled();
			Pro079Core.Pro079.Manager.RegisterCommand(new SCPCommand());
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
		}
		
		public override string Name => "Pro079.SCP";
		public override string Author => "Build";
	}
}