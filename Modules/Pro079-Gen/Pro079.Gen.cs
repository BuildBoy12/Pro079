using Exiled.API.Features;
using System;

namespace GeneratorCommand
{
	public class GeneratorPlugin : Plugin<Config>
	{
		//Setup Config Instance
		private static readonly Lazy<GeneratorPlugin> LazyInstance = new Lazy<GeneratorPlugin>(() => new GeneratorPlugin());
		private GeneratorPlugin() { }
		public static GeneratorPlugin ConfigRef => LazyInstance.Value;

		public override void OnEnabled()
		{
			base.OnEnabled();
			Pro079Core.Pro079.Manager.RegisterCommand(new GenCommand());
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
		}

		public override string Name => "Pro079.Gen";
		public override string Author => "Build";
	}
}