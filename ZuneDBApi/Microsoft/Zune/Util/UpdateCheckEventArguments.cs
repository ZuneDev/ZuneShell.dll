using System.Runtime.InteropServices;

namespace Microsoft.Zune.Util
{
	public class UpdateCheckEventArguments
	{
		private bool m_fUpdateFound;

		private bool m_fCriticalUpdateFound;

		private int m_hr;

		public int HR => m_hr;

		public bool CriticalUpdateFound
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_fCriticalUpdateFound;
			}
		}

		public bool UpdateFound
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_fUpdateFound;
			}
		}

		internal UpdateCheckEventArguments([MarshalAs(UnmanagedType.U1)] bool updateFound, [MarshalAs(UnmanagedType.U1)] bool criticalUpdateFound, int hr)
		{
			m_fUpdateFound = updateFound;
			m_fCriticalUpdateFound = criticalUpdateFound;
			m_hr = hr;
		}
	}
}
