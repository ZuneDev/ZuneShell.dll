namespace MicrosoftZuneLibrary;

public class FirmwareUpdateProgressArgs
{
	private UpdateStep m_Step;

	public UpdateStep Step => m_Step;

	public FirmwareUpdateProgressArgs(UpdateStep Step)
	{
		m_Step = Step;
	}
}
