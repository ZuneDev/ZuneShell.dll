namespace Microsoft.Zune.Util;

public class DownloadManagerUpdateArguments
{
	private EDownloadManagerUpdateType m_type;

	private DownloadTask m_task;

	private int m_queuePosition;

	public int QueuePosition => m_queuePosition;

	public DownloadTask Task => m_task;

	public EDownloadManagerUpdateType Type => m_type;

	internal DownloadManagerUpdateArguments(EDownloadManagerUpdateType type, DownloadTask task, int queuePosition)
	{
		m_type = type;
		m_task = task;
		m_queuePosition = queuePosition;
	}
}
