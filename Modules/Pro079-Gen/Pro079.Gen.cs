using Exiled.API.Features;

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
	}
}