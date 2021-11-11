using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary
{
	public class FirmwareProcessCompleteArgs
	{
		private FirmwareUpdateErrorInfo m_ErrorInfo;

		private UpdateStep m_Step;

		private CompletionAction m_Action;

		private bool m_DisconnectDeviceOnComplete;

		public bool DisconnectDeviceOnComplete
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_DisconnectDeviceOnComplete;
			}
		}

		public CompletionAction Action => m_Action;

		public UpdateStep Step => m_Step;

		public FirmwareUpdateErrorInfo ErrorInfo => m_ErrorInfo;

		public FirmwareProcessCompleteArgs(FirmwareUpdateErrorInfo ErrorInfo, UpdateStep Step, CompletionAction Action, [MarshalAs(UnmanagedType.U1)] bool DisconnectDeviceOnComplete)
		{
			m_ErrorInfo = ErrorInfo;
			m_Step = Step;
			m_Action = Action;
			m_DisconnectDeviceOnComplete = DisconnectDeviceOnComplete;
		}
	}
}
