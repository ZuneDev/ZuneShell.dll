namespace MicrosoftZuneLibrary
{
	public class FirmwareUpdateBeginArgs
	{
		private UpdateStep m_Step;

		public UpdateStep Step => m_Step;

		public FirmwareUpdateBeginArgs(UpdateStep Step)
		{
			m_Step = Step;
		}
	}
}
