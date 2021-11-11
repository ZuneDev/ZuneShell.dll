using System.Runtime.InteropServices;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary
{
	internal class StartCheckForUpdatesArgs : HrStatusBase
	{
		private bool m_ForceServerRequest;

		private DeferredInvokeHandler m_CheckForUpdatesComplete;

		public DeferredInvokeHandler CheckForUpdatesComplete => m_CheckForUpdatesComplete;

		public bool ForceServerRequest
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_ForceServerRequest;
			}
		}

		public StartCheckForUpdatesArgs([MarshalAs(UnmanagedType.U1)] bool ForceServerRequest, DeferredInvokeHandler CheckForUpdatesComplete)
		{
			m_ForceServerRequest = ForceServerRequest;
			m_CheckForUpdatesComplete = CheckForUpdatesComplete;
		}
	}
}
