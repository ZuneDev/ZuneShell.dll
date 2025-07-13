namespace MicrosoftZuneLibrary;

internal class HrStatusBase
{
	protected int m_HrStatus;

	public int HrStatus
	{
		get
		{
			return m_HrStatus;
		}
		set
		{
			m_HrStatus = value;
		}
	}

	public HrStatusBase()
	{
		m_HrStatus = 0;
	}
}
