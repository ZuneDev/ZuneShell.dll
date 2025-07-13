using Microsoft.Iris;

namespace MicrosoftZuneLibrary;

internal class StartFirmwareRestoreArgs : HrStatusBase
{
	private FirmwareRestorePoint m_RestorePoint;

	private DeferredInvokeHandler m_FirmwareUpdateBegin;

	private DeferredInvokeHandler m_FirmwareUpdateProgress;

	private DeferredInvokeHandler m_FirmwareUpdateComplete;

	public DeferredInvokeHandler FirmwareUpdateComplete => m_FirmwareUpdateComplete;

	public DeferredInvokeHandler FirmwareUpdateProgress => m_FirmwareUpdateProgress;

	public DeferredInvokeHandler FirmwareUpdateBegin => m_FirmwareUpdateBegin;

	public FirmwareRestorePoint RestorePoint => m_RestorePoint;

	public StartFirmwareRestoreArgs(FirmwareRestorePoint RestorePoint, DeferredInvokeHandler FirmwareUpdateBegin, DeferredInvokeHandler FirmwareUpdateProgress, DeferredInvokeHandler FirmwareUpdateComplete)
	{
		m_RestorePoint = RestorePoint;
		m_FirmwareUpdateBegin = FirmwareUpdateBegin;
		m_FirmwareUpdateProgress = FirmwareUpdateProgress;
		m_FirmwareUpdateComplete = FirmwareUpdateComplete;
	}
}
