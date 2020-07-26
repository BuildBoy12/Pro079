namespace Pro079Core.API
{
	public class CommandOutput
	{
		public CommandOutput(bool success, bool drainAp, bool cassieCooldown, bool customReturnColor)
		{
			Success = success;
			DrainAp = drainAp;
			CassieCooldown = cassieCooldown;
			CustomReturnColor = customReturnColor;
		}

		public bool Success { set; get; }
		public bool DrainAp { set; get; }
		public bool CassieCooldown { set; get; }
		public bool CustomReturnColor { set; get; }
	}
}