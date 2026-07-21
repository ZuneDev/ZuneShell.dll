namespace MicrosoftZuneLibrary;

public class FirmwareProcessCompleteArgs
{
    private FirmwareUpdateErrorInfo m_ErrorInfo;
    private UpdateStep m_Step;
    private CompletionAction m_Action;
    private bool m_DisconnectDeviceOnComplete;

    public bool DisconnectDeviceOnComplete => m_DisconnectDeviceOnComplete;

    public CompletionAction Action => m_Action;

    public UpdateStep Step => m_Step;

    public FirmwareUpdateErrorInfo ErrorInfo => m_ErrorInfo;

    public FirmwareProcessCompleteArgs(FirmwareUpdateErrorInfo errorInfo, UpdateStep step, CompletionAction action, bool disconnectDeviceOnComplete)
    {
        m_ErrorInfo = errorInfo;
        m_Step = step;
        m_Action = action;
        m_DisconnectDeviceOnComplete = disconnectDeviceOnComplete;
    }
}
