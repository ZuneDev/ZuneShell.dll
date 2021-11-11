namespace Microsoft.Zune.Util
{
	public class DownloadEventArguments
	{
		private float m_percent;

		private EDownloadTaskState m_state;

		private uint m_hrError;

		public uint Error => m_hrError;

		public float Progress => m_percent;

		public EDownloadTaskState State => m_state;

		internal DownloadEventArguments(float percent, EDownloadTaskState state, uint hrError)
		{
			m_percent = percent;
			m_state = state;
			m_hrError = hrError;
			base._002Ector();
		}
	}
}
