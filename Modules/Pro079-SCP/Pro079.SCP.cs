using Exiled.API.Features;
using System;

namespace SCPCommand
{
	public class SCPPlugin : Plugin<Config>
	{
		public override void OnEnabled()
		{
			base.OnEnabled();
			Pro079Core.Pro079.Manager.RegisterCommand(new SCPCommand(this));
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
		}
		
		public override string Name => "Pro079.SCP";
		public override string Author => "Build";
	}
}