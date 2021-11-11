using ZuneUI;

namespace Microsoft.Zune.Service
{
	public class PropertyError
	{
		private HRESULT m_hr;

		private string m_name;

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				m_name = value;
			}
		}

		public HRESULT Hr
		{
			get
			{
				return m_hr;
			}
			set
			{
				m_hr = value;
			}
		}
	}
}
