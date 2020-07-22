using Exiled.API.Features;

namespace BlackoutUltimate
{
    public class BlackoutUltimate : Plugin<Config>
	{
		public override void OnEnabled()
		{
			base.OnEnabled();
			Pro079Core.Pro079.Manager.RegisterUltimate(new BlackoutLogic(this));
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
		}

		public override string Name => "Pro079.Ultimates.Blackout";
		public override string Author => "Build";
    }
}