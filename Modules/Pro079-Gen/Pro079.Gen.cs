using Exiled.API.Features;
using System;

namespace GeneratorCommand
{
    public class GeneratorPlugin : Plugin<Config>
	{
		public override void OnEnabled()
		{
			base.OnEnabled();
			Pro079Core.Pro079.Manager.RegisterCommand(new GenCommand(this));
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
		}

		public override string Name => "Pro079.Gen";
		public override string Author => "Build";
		public override Version RequiredExiledVersion => new Version(2, 0, 9);
	}
}