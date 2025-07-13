using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary;

public class CheckForUpdatesArgs
{
	private FirmwareUpdateErrorInfo m_ErrorInfo;

	private UpdatePackageCollection m_UpdatePackages;

	private bool m_RequiresSyncBeforeUpdate;

	public bool RequiresSyncBeforeUpdate
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return m_RequiresSyncBeforeUpdate;
		}
	}

	public UpdatePackageCollection UpdatePackages => m_UpdatePackages;

	public FirmwareUpdateErrorInfo ErrorInfo => m_ErrorInfo;

	public CheckForUpdatesArgs(FirmwareUpdateErrorInfo ErrorInfo, UpdatePackageCollection UpdatePackages, [MarshalAs(UnmanagedType.U1)] bool RequiresSyncBeforeUpdate)
	{
		m_ErrorInfo = ErrorInfo;
		m_UpdatePackages = UpdatePackages;
		m_RequiresSyncBeforeUpdate = RequiresSyncBeforeUpdate;
	}
}
