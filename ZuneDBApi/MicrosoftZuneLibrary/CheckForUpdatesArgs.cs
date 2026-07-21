namespace MicrosoftZuneLibrary;

public class CheckForUpdatesArgs
{
    private FirmwareUpdateErrorInfo m_ErrorInfo;
    private UpdatePackageCollection m_UpdatePackages;
    private bool m_RequiresSyncBeforeUpdate;

    public bool RequiresSyncBeforeUpdate => m_RequiresSyncBeforeUpdate;

    public UpdatePackageCollection UpdatePackages => m_UpdatePackages;

    public FirmwareUpdateErrorInfo ErrorInfo => m_ErrorInfo;

    public CheckForUpdatesArgs(FirmwareUpdateErrorInfo errorInfo, UpdatePackageCollection updatePackages, bool requiresSyncBeforeUpdate)
    {
        m_ErrorInfo = errorInfo;
        m_UpdatePackages = updatePackages;
        m_RequiresSyncBeforeUpdate = requiresSyncBeforeUpdate;
    }
}
