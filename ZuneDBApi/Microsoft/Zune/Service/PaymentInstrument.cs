namespace Microsoft.Zune.Service
{
	public class PaymentInstrument
	{
		private string m_id;

		private PaymentType m_type;

		public PaymentType Type => m_type;

		public string Id
		{
			get
			{
				return m_id;
			}
			set
			{
				m_id = value;
			}
		}

		public PaymentInstrument(string id, PaymentType type)
		{
			m_id = id;
			m_type = type;
		}
	}
}
