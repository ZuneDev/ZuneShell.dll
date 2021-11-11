namespace MicrosoftZuneLibrary
{
	internal class ContinueFirmwareUpdateArgs
	{
		private FirmwareCompleteHandler m_OnCompleteHandler;

		public FirmwareCompleteHandler OnCompleteHandler => m_OnCompleteHandler;

		public ContinueFirmwareUpdateArgs(FirmwareCompleteHandler OnCompleteHandler)
		{
			m_OnCompleteHandler = OnCompleteHandler;
		}
	}
}
