namespace MicrosoftZuneLibrary;

public class CheckDiskSpaceArgs
{
    private int m_HrStatus;
    private ulong m_AdditionalSpaceRequiredInBytes;
    private ulong m_TotalSpaceRequiredInBytes;
    private string m_DriveLetter;

    public string DriveLetter => m_DriveLetter;

    public ulong TotalSpaceRequiredInBytes => m_TotalSpaceRequiredInBytes;

    public ulong AdditionalSpaceRequiredInBytes => m_AdditionalSpaceRequiredInBytes;

    public int HrStatus => m_HrStatus;

    public CheckDiskSpaceArgs(int hrStatus, ulong additionalSpaceRequiredInBytes, ulong totalSpaceRequiredInBytes, string driveLetter)
    {
        m_HrStatus = hrStatus;
        m_AdditionalSpaceRequiredInBytes = additionalSpaceRequiredInBytes;
        m_TotalSpaceRequiredInBytes = totalSpaceRequiredInBytes;
        m_DriveLetter = driveLetter;
    }
}
