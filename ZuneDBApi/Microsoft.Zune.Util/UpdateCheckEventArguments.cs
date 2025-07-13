using System.Runtime.InteropServices;

namespace Microsoft.Zune.Util;

public class UpdateCheckEventArguments([MarshalAs(UnmanagedType.U1)] bool updateFound, [MarshalAs(UnmanagedType.U1)] bool criticalUpdateFound, int hr)
{
	private bool m_fUpdateFound = updateFound;

	private bool m_fCriticalUpdateFound = criticalUpdateFound;

	private int m_hr = hr;

	public int HR => hr;

	public bool CriticalUpdateFound
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return criticalUpdateFound;
		}
	}

	public bool UpdateFound
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return updateFound;
		}
	}
}
