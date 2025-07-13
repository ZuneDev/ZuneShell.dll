namespace Microsoft.Zune.Util;

public class DownloadEventArguments(float percent, EDownloadTaskState state, uint hrError)
{
	private float m_percent = percent;

	private EDownloadTaskState m_state = state;

	private uint m_hrError = hrError;

	public uint Error => hrError;

	public float Progress => percent;

	public EDownloadTaskState State => state;
}
