using Exiled.API.Features;
using System;

namespace BlackoutUltimate
{
	public class BlackoutUltimate : Plugin<Config>
	{
		private static readonly Lazy<BlackoutUltimate> LazyInstance = new Lazy<BlackoutUltimate>(() => new BlackoutUltimate());
		private BlackoutUltimate() { }
		public static BlackoutUltimate ConfigRef => LazyInstance.Value;

		private BlackoutLogic BlackoutLogic;

		public override void OnEnabled()
		{
			base.OnEnabled();
			BlackoutLogic = new BlackoutLogic();
			Pro079Core.Pro079.Manager.RegisterUltimate(BlackoutLogic);
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
			BlackoutLogic = null;
		}

		public override string Name => "Pro079.Ultimates.Blackout";
		public override string Author => "Build";
    }
}