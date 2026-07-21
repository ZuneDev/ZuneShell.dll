namespace MicrosoftZuneLibrary;

public class FirmwareUpdateBeginArgs
{
    private UpdateStep m_Step;

    public UpdateStep Step => m_Step;

    public FirmwareUpdateBeginArgs(UpdateStep step)
    {
        m_Step = step;
    }
}
