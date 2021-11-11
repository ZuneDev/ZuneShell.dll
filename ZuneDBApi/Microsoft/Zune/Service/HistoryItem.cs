namespace Microsoft.Zune.Service
{
	public class HistoryItem
	{
		private string m_compId;

		private string m_date;

		private int m_remainingLicenseCount;

		public int RemainingLicenseCount => m_remainingLicenseCount;

		public string Date => m_date;

		public string CompId => m_compId;

		public HistoryItem(string compId, string date, int remainingLicenseCount)
		{
			m_compId = compId;
			m_date = date;
			m_remainingLicenseCount = remainingLicenseCount;
		}
	}
}
