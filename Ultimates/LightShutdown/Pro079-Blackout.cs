using Exiled.API.Features;

namespace BlackoutUltimate
{
    public class BlackoutUltimate : Plugin<Config>
	{
		private BlackoutLogic BlackoutLogic;

		public override void OnEnabled()
		{
			base.OnEnabled();
			BlackoutLogic = new BlackoutLogic(this);
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