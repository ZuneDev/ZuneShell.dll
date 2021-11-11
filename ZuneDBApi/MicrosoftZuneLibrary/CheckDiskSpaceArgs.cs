namespace MicrosoftZuneLibrary
{
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

		public CheckDiskSpaceArgs(int HrStatus, ulong AdditionalSpaceRequiredInBytes, ulong TotalSpaceRequiredInBytes, string DriveLetter)
		{
			m_HrStatus = HrStatus;
			m_AdditionalSpaceRequiredInBytes = AdditionalSpaceRequiredInBytes;
			m_TotalSpaceRequiredInBytes = TotalSpaceRequiredInBytes;
			m_DriveLetter = DriveLetter;
		}
	}
}
