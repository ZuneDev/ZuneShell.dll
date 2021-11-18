namespace MicrosoftZuneLibrary
{
	public class MediatorCancelTimerCallback
	{
		private unsafe FirmwareUpdateMediator* m_mediator;

		public unsafe MediatorCancelTimerCallback(FirmwareUpdateMediator* mediator)
		{
			m_mediator = mediator;
		}

		public unsafe void TimerHit(object P_0)
		{
			FirmwareUpdateMediator* mediator = m_mediator;
			if (mediator != null)
			{
				Module.MicrosoftZuneLibrary_002EFirmwareUpdateMediator_002EForceCancel(mediator);
			}
		}
	}
}
