using Microsoft.Iris;

namespace MicrosoftZuneLibrary;

internal class StartFirmwareUpdateArgs : HrStatusBase
{
	private UpdatePackageCollection m_Updates;

	private DeferredInvokeHandler m_FirmwareUpdateBegin;

	private DeferredInvokeHandler m_FirmwareUpdateProgress;

	private DeferredInvokeHandler m_FirmwareUpdateComplete;

	private FirmwareUpdateOption m_UpdateOption;

	public FirmwareUpdateOption UpdateOption => m_UpdateOption;

	public DeferredInvokeHandler FirmwareUpdateComplete => m_FirmwareUpdateComplete;

	public DeferredInvokeHandler FirmwareUpdateProgress => m_FirmwareUpdateProgress;

	public DeferredInvokeHandler FirmwareUpdateBegin => m_FirmwareUpdateBegin;

	public UpdatePackageCollection Updates => m_Updates;

	public StartFirmwareUpdateArgs(UpdatePackageCollection Updates, DeferredInvokeHandler FirmwareUpdateBegin, DeferredInvokeHandler FirmwareUpdateProgress, DeferredInvokeHandler FirmwareUpdateComplete, FirmwareUpdateOption UpdateOption)
	{
		m_Updates = Updates;
		m_FirmwareUpdateBegin = FirmwareUpdateBegin;
		m_FirmwareUpdateProgress = FirmwareUpdateProgress;
		m_FirmwareUpdateComplete = FirmwareUpdateComplete;
		m_UpdateOption = UpdateOption;
	}
}
