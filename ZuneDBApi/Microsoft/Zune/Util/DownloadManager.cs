using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Iris;

namespace Microsoft.Zune.Util
{
	public class DownloadManager : IDisposable
	{
		private unsafe DownloadManagerProxy* m_pDownloadManagerProxy;

		private static DownloadManager sm_downloadManager = null;

		private static object sm_lock = new object();

		private ArrayListDataSet m_activeDownloadTasks;

		private ArrayListDataSet m_completedDownloadTasks;

		private ArrayListDataSet m_cancelledDownloadTasks;

		private ArrayListDataSet m_failedDownloadTasks;

		private DownloadManagerUpdateHandler m_defaultUpdateHandler;

		public ArrayListDataSet CancelledDownloads => m_cancelledDownloadTasks;

		public ArrayListDataSet FailedDownloads => m_failedDownloadTasks;

		public ArrayListDataSet CompletedDownloads => m_completedDownloadTasks;

		public ArrayListDataSet ActiveDownloads => m_activeDownloadTasks;

		public unsafe float Percentage => *(float*)((ulong)(nint)m_pDownloadManagerProxy + 24uL);

		public unsafe bool HadFailures
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return *(int*)((ulong)(nint)m_pDownloadManagerProxy + 20uL) > 0;
			}
		}

		public unsafe bool HadCancellations
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return *(int*)((ulong)(nint)m_pDownloadManagerProxy + 16uL) > 0;
			}
		}

		public unsafe bool IsQueuePaused
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_0005: Expected I, but got I8
				//IL_0024: Expected I, but got I8
				//IL_0037: Expected I, but got I8
				bool result = false;
				IDownloadManager* ptr = null;
				if (Module.GetSingleton((_GUID)Module._GUID_399f851b_a600_4e88_90c3_03b8f2770076, (void**)(&ptr)) >= 0)
				{
					IDownloadManager* intPtr = ptr;
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte>)(*(ulong*)(*(long*)intPtr + 144)))((nint)intPtr) != 0;
				}
				if (0L != (nint)ptr)
				{
					IDownloadManager* intPtr2 = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
				}
				return result;
			}
		}

		public unsafe bool Finished
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((*(int*)((ulong)(nint)m_pDownloadManagerProxy + 8uL) <= 0) ? 1u : 0u) != 0;
			}
		}

		public unsafe int ActiveItem
		{
			get
			{
				DownloadManagerProxy* pDownloadManagerProxy = m_pDownloadManagerProxy;
				return *(int*)((ulong)(nint)pDownloadManagerProxy + 20uL) + *(int*)((ulong)(nint)pDownloadManagerProxy + 16uL) + *(int*)((ulong)(nint)pDownloadManagerProxy + 12uL) + 1;
			}
		}

		public unsafe int TotalInProgressItems => *(int*)((ulong)(nint)m_pDownloadManagerProxy + 8uL);

		public unsafe int TotalItems
		{
			get
			{
				DownloadManagerProxy* pDownloadManagerProxy = m_pDownloadManagerProxy;
				return *(int*)((ulong)(nint)pDownloadManagerProxy + 20uL) + *(int*)((ulong)(nint)pDownloadManagerProxy + 16uL) + *(int*)((ulong)(nint)pDownloadManagerProxy + 12uL) + *(int*)((ulong)(nint)pDownloadManagerProxy + 8uL);
			}
		}

		public static DownloadManager Instance
		{
			get
			{
				if (sm_downloadManager == null)
				{
					try
					{
						Monitor.Enter(sm_lock);
						if (sm_downloadManager == null)
						{
							DownloadManager downloadManager = new DownloadManager();
							Thread.MemoryBarrier();
							sm_downloadManager = downloadManager;
						}
					}
					finally
					{
						Monitor.Exit(sm_lock);
					}
				}
				return sm_downloadManager;
			}
		}

		[SpecialName]
		public unsafe event DownloadManagerUpdateHandler OnProgressChanged
		{
			add
			{
				Module.Microsoft_002EZune_002EUtil_002EDownloadManagerProxy_002EAddDelegate(m_pDownloadManagerProxy, value);
			}
			remove
			{
				Module.Microsoft_002EZune_002EUtil_002EDownloadManagerProxy_002ERemoveDelegate(m_pDownloadManagerProxy, value);
			}
		}

		private void _007EDownloadManager()
		{
			if (m_defaultUpdateHandler != null)
			{
				OnProgressChanged -= m_defaultUpdateHandler;
				m_defaultUpdateHandler = null;
			}
			_0021DownloadManager();
		}

		private unsafe void _0021DownloadManager()
		{
			//IL_0019: Expected I, but got I8
			//IL_0022: Expected I, but got I8
			DownloadManagerProxy* pDownloadManagerProxy = m_pDownloadManagerProxy;
			if (0L != (nint)pDownloadManagerProxy)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pDownloadManagerProxy + 16)))((nint)pDownloadManagerProxy);
				m_pDownloadManagerProxy = null;
			}
		}

		public static DownloadManager CreateInstance()
		{
			return Instance;
		}

		public unsafe DownloadTask GetTask(string taskId)
		{
			//IL_0003: Expected I, but got I8
			//IL_0006: Expected I, but got I8
			//IL_0036: Expected I, but got I8
			//IL_0054: Expected I, but got I8
			//IL_0058: Expected I, but got I8
			//IL_006a: Expected I, but got I8
			IDownloadManager* ptr = null;
			IDownloadTask* ptr2 = null;
			DownloadTask result = null;
			fixed (char* taskIdPtr = taskId.ToCharArray())
			{
				ushort* ptr3 = (ushort*)taskIdPtr;
				int singleton = Module.GetSingleton((_GUID)Module._GUID_399f851b_a600_4e88_90c3_03b8f2770076, (void**)(&ptr));
				if (singleton >= 0)
				{
					long num = *(long*)ptr + 72;
					IDownloadManager* intPtr = ptr;
					singleton = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, IDownloadTask**, int>)(*(ulong*)num))((nint)intPtr, ptr3, &ptr2);
					if (singleton >= 0)
					{
						result = new DownloadTask(ptr2);
					}
				}
				if (0L != (nint)ptr)
				{
					IDownloadManager* intPtr2 = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
					ptr = null;
				}
				if (0L != (nint)ptr2)
				{
					IDownloadTask* intPtr3 = ptr2;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
				}
				return result;
			}
		}

		public int SetPosition(IList list, int position)
		{
			int num = 0;
			if (list != null && position >= 0)
			{
				ArrayList arrayList = new ArrayList(list.Count);
				int num2 = list.Count - 1;
				if (num2 >= 0)
				{
					do
					{
						DownloadTask downloadTask = list[num2] as DownloadTask;
						if (downloadTask != null && downloadTask.CanReorder() && downloadTask.SetPosition(position))
						{
							arrayList.Add(downloadTask);
							num++;
						}
						num2 += -1;
					}
					while (num2 >= 0);
					if (num > 0)
					{
						DownloadManagerMoveArguments args = new DownloadManagerMoveArguments(arrayList, position);
						UpdatePositions(args);
					}
				}
			}
			return num;
		}

		public void DownloadNext(DownloadTask task)
		{
			if (task != null && task.CanReorder())
			{
				task.DownloadNext();
				UpdateActiveList();
			}
		}

		public void RemoveFailed(DownloadTask task)
		{
			if (Application.IsApplicationThread)
			{
				DeferredRemoveFailed(task);
			}
			else
			{
				Application.DeferredInvoke(DeferredRemoveFailed, task);
			}
		}

		public unsafe void PauseQueue()
		{
			Module.Microsoft_002EZune_002EUtil_002EDownloadManagerProxy_002EPauseQueue(m_pDownloadManagerProxy);
		}

		public unsafe void ResumeQueue()
		{
			Module.Microsoft_002EZune_002EUtil_002EDownloadManagerProxy_002EResumeQueue(m_pDownloadManagerProxy);
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool SignInRequired()
		{
			//IL_0005: Expected I, but got I8
			//IL_0024: Expected I, but got I8
			//IL_0033: Expected I, but got I8
			//IL_0043: Expected I, but got I8
			//IL_0056: Expected I, but got I8
			//IL_006a: Expected I, but got I8
			//IL_006a: Expected I, but got I8
			//IL_0084: Expected I, but got I8
			//IL_00a0: Expected I, but got I8
			bool flag = false;
			IDownloadManager* ptr = null;
			if (Module.GetSingleton((_GUID)Module._GUID_399f851b_a600_4e88_90c3_03b8f2770076, (void**)(&ptr)) >= 0)
			{
				IDownloadManager* intPtr = ptr;
				int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)intPtr + 88)))((nint)intPtr);
				int num2 = 0;
				if (0 < num)
				{
					while (!flag)
					{
						IDownloadTask* ptr2 = null;
						IDownloadManager* intPtr2 = ptr;
						if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, IDownloadTask**, int>)(*(ulong*)(*(long*)ptr + 104)))((nint)intPtr2, num2, &ptr2) >= 0)
						{
							IDownloadTask* intPtr3 = ptr2;
							if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EDownloadType>)(*(ulong*)(*(long*)intPtr3 + 144)))((nint)intPtr3) == 0)
							{
								IDownloadTask* intPtr4 = ptr2;
								flag = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, EDownloadTaskState>)(*(ulong*)(*(long*)ptr2 + 200)))((nint)intPtr4, null) == EDownloadTaskState.DLTaskPendingAttach || flag;
							}
						}
						if (0L != (nint)ptr2)
						{
							IDownloadTask* intPtr5 = ptr2;
							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr5 + 16)))((nint)intPtr5);
						}
						num2++;
						if (num2 >= num)
						{
							break;
						}
					}
				}
			}
			if (0L != (nint)ptr)
			{
				IDownloadManager* intPtr6 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr6 + 16)))((nint)intPtr6);
			}
			return flag;
		}

		private unsafe DownloadManager()
		{
			//IL_004a: Expected I, but got I8
			m_activeDownloadTasks = new ArrayListDataSet();
			m_completedDownloadTasks = new ArrayListDataSet();
			m_failedDownloadTasks = new ArrayListDataSet();
			m_cancelledDownloadTasks = new ArrayListDataSet();
			DownloadManagerProxy* ptr = (DownloadManagerProxy*)Module.@new(112uL);
			DownloadManagerProxy* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : Module.Microsoft_002EZune_002EUtil_002EDownloadManagerProxy_002E_007Bctor_007D(ptr));
			}
			catch
			{
				//try-fault
				Module.delete(ptr);
				throw;
			}
			m_pDownloadManagerProxy = ptr2;
			if (ptr2 != null && Module.Microsoft_002EZune_002EUtil_002EDownloadManagerProxy_002EInitialize(ptr2) >= 0)
			{
				OnProgressChanged += (m_defaultUpdateHandler = DefaultUpdateHandler);
			}
		}

		private void DefaultUpdateHandler(DownloadManagerUpdateArguments args)
		{
			if (args.Type == EDownloadManagerUpdateType.TaskAdded)
			{
				AddActiveTask(args);
			}
			else if (args.Type != EDownloadManagerUpdateType.TaskProgressChanged)
			{
				Application.DeferredInvoke(DeferredUpdateInactiveLists, args);
			}
		}

		private void UpdatePositions(DownloadManagerMoveArguments args)
		{
			if (Application.IsApplicationThread)
			{
				DeferredUpdatePositions(args);
			}
			else
			{
				Application.DeferredInvoke(DeferredUpdatePositions, args);
			}
		}

		private void AddActiveTask(DownloadManagerUpdateArguments args)
		{
			if (Application.IsApplicationThread)
			{
				DeferredAddActiveTask(args);
			}
			else
			{
				Application.DeferredInvoke(DeferredAddActiveTask, args);
			}
		}

		private void UpdateActiveList()
		{
			ThreadPool.QueueUserWorkItem(UpdateActiveListOnWorkerThread, this);
		}

		private unsafe static void UpdateActiveListOnWorkerThread(object args)
		{
			//IL_0029: Expected I, but got I8
			//IL_0029: Expected I, but got I8
			//IL_005c: Expected I, but got I8
			//IL_005c: Expected I, but got I8
			//IL_0077: Expected I, but got I8
			//IL_0077: Expected I, but got I8
			//IL_0091: Expected I, but got I8
			//IL_0091: Expected I, but got I8
			//IL_0091: Expected I, but got I8
			//IL_009f: Expected I, but got I8
			CComPtrNtv_003CIDownloadManager_003E cComPtrNtv_003CIDownloadManager_003E;
			*(long*)(&cComPtrNtv_003CIDownloadManager_003E) = 0L;
			try
			{
				int singleton = Module.GetSingleton((_GUID)Module._GUID_399f851b_a600_4e88_90c3_03b8f2770076, (void**)(&cComPtrNtv_003CIDownloadManager_003E));
				int num = 0;
				if (singleton >= 0)
				{
					long num2 = *(long*)(&cComPtrNtv_003CIDownloadManager_003E);
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIDownloadManager_003E)) + 88)))((nint)num2);
					num = ((num >= 0) ? num : 0);
				}
				IList list = new ArrayList(num);
				int num3 = 0;
				if (0 < num)
				{
					do
					{
						CComPtrNtv_003CIDownloadTask_003E cComPtrNtv_003CIDownloadTask_003E;
						*(long*)(&cComPtrNtv_003CIDownloadTask_003E) = 0L;
						try
						{
							long num4 = *(long*)(&cComPtrNtv_003CIDownloadManager_003E);
							singleton = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, IDownloadTask**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIDownloadManager_003E)) + 104)))((nint)num4, num3, (IDownloadTask**)(&cComPtrNtv_003CIDownloadTask_003E));
							if (singleton >= 0)
							{
								long num5 = *(long*)(&cComPtrNtv_003CIDownloadTask_003E);
								if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EDownloadType>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIDownloadTask_003E)) + 144)))((nint)num5) == 0)
								{
									long num6 = *(long*)(&cComPtrNtv_003CIDownloadTask_003E);
									if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int*, EDownloadTaskState>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIDownloadTask_003E)) + 200)))((nint)num6, null) != EDownloadTaskState.DLTaskComplete)
									{
										list.Add(new DownloadTask((IDownloadTask*)(*(ulong*)(&cComPtrNtv_003CIDownloadTask_003E))));
									}
								}
							}
						}
						catch
						{
							//try-fault
							Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIDownloadTask_003E*, void>)(&Module.CComPtrNtv_003CIDownloadTask_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIDownloadTask_003E);
							throw;
						}
						Module.CComPtrNtv_003CIDownloadTask_003E_002ERelease(&cComPtrNtv_003CIDownloadTask_003E);
						num3++;
					}
					while (num3 < num);
				}
				Application.DeferredInvoke(DeferredUpdateActiveList, list);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIDownloadManager_003E*, void>)(&Module.CComPtrNtv_003CIDownloadManager_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIDownloadManager_003E);
				throw;
			}
			Module.CComPtrNtv_003CIDownloadManager_003E_002ERelease(&cComPtrNtv_003CIDownloadManager_003E);
		}

		private void DeferredUpdateActiveList(object args)
		{
			m_activeDownloadTasks.Source = args as ArrayList;
		}

		private void DeferredUpdateInactiveLists(object args)
		{
			DownloadManagerUpdateArguments downloadManagerUpdateArguments = args as DownloadManagerUpdateArguments;
			if (downloadManagerUpdateArguments != null)
			{
				if (downloadManagerUpdateArguments.Type == EDownloadManagerUpdateType.TaskCompleted)
				{
					m_completedDownloadTasks.Add(downloadManagerUpdateArguments.Task);
					m_activeDownloadTasks.Remove(downloadManagerUpdateArguments.Task);
				}
				else if (downloadManagerUpdateArguments.Type == EDownloadManagerUpdateType.TaskFailed)
				{
					m_failedDownloadTasks.Add(downloadManagerUpdateArguments.Task);
					m_activeDownloadTasks.Remove(downloadManagerUpdateArguments.Task);
				}
				else if (downloadManagerUpdateArguments.Type == EDownloadManagerUpdateType.TaskCancelled)
				{
					m_cancelledDownloadTasks.Add(downloadManagerUpdateArguments.Task);
					m_activeDownloadTasks.Remove(downloadManagerUpdateArguments.Task);
				}
				if (m_activeDownloadTasks.Count == 0)
				{
					DeferredClearLists(null);
				}
			}
		}

		private void DeferredClearLists(object args)
		{
			m_activeDownloadTasks.Clear();
			m_completedDownloadTasks.Clear();
			m_cancelledDownloadTasks.Clear();
		}

		private unsafe void DeferredRemoveFailed(object args)
		{
			DownloadTask downloadTask = args as DownloadTask;
			if (downloadTask == null)
			{
				return;
			}
			switch (downloadTask.GetState())
			{
			case EDownloadTaskState.DLTaskCancelled:
			{
				int num2 = m_cancelledDownloadTasks.IndexOf(downloadTask);
				if (num2 >= 0)
				{
					m_cancelledDownloadTasks.RemoveAt(num2);
					Module.Microsoft_002EZune_002EUtil_002EDownloadManagerProxy_002ERemoveCancelledCount(m_pDownloadManagerProxy);
				}
				break;
			}
			case EDownloadTaskState.DLTaskFailed:
			{
				int num = m_failedDownloadTasks.IndexOf(downloadTask);
				if (num >= 0)
				{
					m_failedDownloadTasks.RemoveAt(num);
					Module.Microsoft_002EZune_002EUtil_002EDownloadManagerProxy_002ERemoveFailedCount(m_pDownloadManagerProxy);
				}
				break;
			}
			}
		}

		private void DeferredUpdatePositions(object args)
		{
			DownloadManagerMoveArguments downloadManagerMoveArguments = args as DownloadManagerMoveArguments;
			if (downloadManagerMoveArguments == null)
			{
				return;
			}
			int position = downloadManagerMoveArguments.Position;
			if (position <= m_activeDownloadTasks.Count && position >= 0)
			{
				int num = 0;
				if (0 >= downloadManagerMoveArguments.Tasks.Count)
				{
					return;
				}
				do
				{
					DownloadTask downloadTask = downloadManagerMoveArguments.Tasks[num] as DownloadTask;
					if (downloadTask != null)
					{
						int num2 = m_activeDownloadTasks.IndexOf(downloadTask);
						if (num2 >= 0)
						{
							m_activeDownloadTasks.Move(num2, position);
						}
					}
					num++;
				}
				while (num < downloadManagerMoveArguments.Tasks.Count);
			}
			else
			{
				UpdateActiveList();
			}
		}

		private unsafe void DeferredAddActiveTask(object args)
		{
			DownloadManagerUpdateArguments downloadManagerUpdateArguments = args as DownloadManagerUpdateArguments;
			if (downloadManagerUpdateArguments == null)
			{
				return;
			}
			int num = *(int*)((ulong)(nint)m_pDownloadManagerProxy + 8uL);
			if (num <= m_activeDownloadTasks.Count + 1 && downloadManagerUpdateArguments.QueuePosition <= m_activeDownloadTasks.Count && downloadManagerUpdateArguments.QueuePosition >= 0)
			{
				if (num != m_activeDownloadTasks.Count || !m_activeDownloadTasks.Contains(downloadManagerUpdateArguments.Task))
				{
					m_activeDownloadTasks.Insert(downloadManagerUpdateArguments.QueuePosition, downloadManagerUpdateArguments.Task);
				}
			}
			else
			{
				UpdateActiveList();
			}
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_007EDownloadManager();
				return;
			}
			try
			{
				_0021DownloadManager();
			}
			finally
			{
				base.Finalize();
			}
		}

		public sealed override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~DownloadManager()
		{
			Dispose(false);
		}
	}
}
