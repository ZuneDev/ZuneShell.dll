using System;

namespace Microsoft.Zune.Configuration
{
	public class TunerInfo
	{
		private string m_name;

		private string m_tunerVersion;

		private DateTime m_dtCreated;

		private DateTime m_dtLastUsed;

		private TunerType m_tunerType;

		private TunerRegisterType m_tunerRegisterType;

		internal string m_tunerId;

		public TunerRegisterType TunerRegisterType => m_tunerRegisterType;

		public DateTime DateLastUsed => m_dtLastUsed;

		public DateTime DateCreated => m_dtCreated;

		public TunerType TunerType => m_tunerType;

		public string TunerVersion => m_tunerVersion;

		public string TunerName
		{
			get
			{
				if (!(m_name == (string)null) && m_name.Length != 0)
				{
					return m_name;
				}
				return m_tunerId;
			}
		}

		internal TunerInfo(string name, string tunerId, string tunerVersion, TunerType tunerType, TunerRegisterType tunerRegisterType, string dateCreated, string dateLastUsed)
		{
			m_name = name;
			m_tunerId = tunerId;
			m_tunerType = tunerType;
			m_tunerRegisterType = tunerRegisterType;
			m_tunerVersion = tunerVersion;
			if (dateCreated == (string)null || !DateTime.TryParse(dateCreated, out m_dtCreated))
			{
				m_dtCreated = DateTime.MinValue;
			}
			if (dateLastUsed == (string)null || !DateTime.TryParse(dateLastUsed, out m_dtLastUsed))
			{
				m_dtLastUsed = DateTime.MinValue;
			}
		}
	}
}
