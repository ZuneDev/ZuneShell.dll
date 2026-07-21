namespace Microsoft.Zune.Util;

public class UpdateCheckEventArguments
{
    private bool m_fUpdateFound;
    private bool m_fCriticalUpdateFound;
    private int m_hr;

    public int HR => m_hr;

    public bool CriticalUpdateFound => m_fCriticalUpdateFound;

    public bool UpdateFound => m_fUpdateFound;

    public UpdateCheckEventArguments(bool updateFound, bool criticalUpdateFound, int hr)
    {
        m_fUpdateFound = updateFound;
        m_fCriticalUpdateFound = criticalUpdateFound;
        m_hr = hr;
    }
}
