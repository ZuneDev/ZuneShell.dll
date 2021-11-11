using System;
using System.Collections;
using System.Reflection;
using Microsoft.Iris;

namespace Microsoft.Zune.Util
{
	[DefaultMember("Item")]
	public class DownloadTaskList : AggregateList
	{
		private ArrayListDataSet m_activeDownloadTasks;

		private ArrayListDataSet m_completedDownloadTasks;

		private ArrayListDataSet m_failedDownloadTasks;

		private ArrayListDataSet m_cancelledDownloadTasks;

		private static DownloadTaskList m_instance;

		public ArrayListDataSet FailedDownloads => m_failedDownloadTasks;

		public ArrayListDataSet CancelledDownloads => m_cancelledDownloadTasks;

		public ArrayListDataSet CompletedDownloads => m_completedDownloadTasks;

		public ArrayListDataSet ActiveDownloads => m_activeDownloadTasks;

		public static DownloadTaskList Instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = new DownloadTaskList();
				}
				return m_instance;
			}
		}

		private DownloadTaskList()
			: base(DownloadManager.Instance.CompletedDownloads, DownloadManager.Instance.CancelledDownloads, DownloadManager.Instance.FailedDownloads, DownloadManager.Instance.ActiveDownloads)
		{
			try
			{
				m_completedDownloadTasks = DownloadManager.Instance.CompletedDownloads;
				m_cancelledDownloadTasks = DownloadManager.Instance.CancelledDownloads;
				m_failedDownloadTasks = DownloadManager.Instance.FailedDownloads;
				m_activeDownloadTasks = DownloadManager.Instance.ActiveDownloads;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		public int TopActivePosition()
		{
			int num = m_failedDownloadTasks.Count + m_cancelledDownloadTasks.Count;
			return m_completedDownloadTasks.Count + num;
		}

		public int SetPosition(IList list, int insertAt)
		{
			int num = TopActivePosition();
			insertAt = ((insertAt >= num) ? (insertAt - num) : 0);
			return DownloadManager.Instance.SetPosition(list, insertAt);
		}
	}
}
