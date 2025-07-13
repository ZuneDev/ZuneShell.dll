using System.Collections;

namespace Microsoft.Zune.Util;

public class DownloadManagerMoveArguments
{
	private IList m_tasks;

	private int m_position;

	public int Position => m_position;

	public IList Tasks => m_tasks;

	internal DownloadManagerMoveArguments(IList Tasks, int position)
	{
		m_position = position;
		m_tasks = Tasks;
	}
}
