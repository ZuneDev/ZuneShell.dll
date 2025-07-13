using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace MicrosoftZuneLibrary;

public class WorkerQueue
{
	private class WorkItem
	{
		public WaitCallback Callback;

		public object Context;
	}

	private static object _queuesLock = new object();

	private static List<WorkerQueue> _queues = new List<WorkerQueue>(1);

	private volatile bool _fShutdown;

	private volatile bool _fAbort;

	private ManualResetEvent _hCompletionEvent;

	private List<WorkItem> _workItems;

	public static WorkerQueue CreateInstance()
	{
		ManagedLock managedLock = null;
		ManagedLock managedLock2 = new ManagedLock(_queuesLock);
		WorkerQueue result;
		try
		{
			managedLock = managedLock2;
			if (_queues == null)
			{
				result = null;
				goto IL_0023;
			}
		}
		catch
		{
			//try-fault
			((IDisposable)managedLock).Dispose();
			throw;
		}
		WorkerQueue workerQueue;
		try
		{
			workerQueue = new WorkerQueue();
			_queues.Add(workerQueue);
		}
		catch
		{
			//try-fault
			((IDisposable)managedLock).Dispose();
			throw;
		}
		((IDisposable)managedLock).Dispose();
		return workerQueue;
		IL_0023:
		((IDisposable)managedLock).Dispose();
		return result;
	}

	public static void ShutdownAll()
	{
		ManagedLock managedLock = null;
		ManagedLock managedLock2 = new ManagedLock(_queuesLock);
		try
		{
			managedLock = managedLock2;
			if (_queues != null)
			{
				int num = 0;
				if (0 < _queues.Count)
				{
					do
					{
						_queues[num].Shutdown();
						num++;
					}
					while (num < _queues.Count);
				}
				_queues.Clear();
				_queues = null;
			}
		}
		catch
		{
			//try-fault
			((IDisposable)managedLock).Dispose();
			throw;
		}
		((IDisposable)managedLock).Dispose();
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public bool QueueSequentialWorkItem(WaitCallback callback, object context)
	{
		ManagedLock managedLock = null;
		if (callback == null)
		{
			return false;
		}
		bool result = true;
		ManagedLock managedLock2 = new ManagedLock(this);
		try
		{
			managedLock = managedLock2;
			if (!_fShutdown)
			{
				WorkItem workItem = new WorkItem();
				if (_hCompletionEvent == null)
				{
					_hCompletionEvent = new ManualResetEvent(initialState: true);
				}
				workItem.Callback = callback;
				workItem.Context = context;
				if (_workItems == null)
				{
					_workItems = new List<WorkItem>();
				}
				_workItems.Add(workItem);
				if (_workItems.Count == 1 && (!_hCompletionEvent.Reset() || !ThreadPool.QueueUserWorkItem(ProcessQueue, null)))
				{
					result = false;
					_workItems.Clear();
					_hCompletionEvent.Set();
				}
			}
			else
			{
				result = false;
			}
		}
		catch
		{
			//try-fault
			((IDisposable)managedLock).Dispose();
			throw;
		}
		((IDisposable)managedLock).Dispose();
		return result;
	}

	private WorkerQueue()
	{
	}

	private void ProcessQueue(object context)
	{
		ManagedLock managedLock = null;
		ManagedLock managedLock2 = new ManagedLock(this);
		try
		{
			managedLock = managedLock2;
			if (_fAbort)
			{
				goto IL_0088;
			}
		}
		catch
		{
			//try-fault
			((IDisposable)managedLock).Dispose();
			throw;
		}
		while (true)
		{
			WorkItem workItem;
			try
			{
				if (_workItems.Count == 0)
				{
					break;
				}
				workItem = _workItems[0];
				_workItems.RemoveAt(0);
				goto IL_0052;
			}
			catch
			{
				//try-fault
				((IDisposable)managedLock).Dispose();
				throw;
			}
			IL_0052:
			((IDisposable)managedLock).Dispose();
			workItem.Callback(workItem.Context);
			managedLock2 = new ManagedLock(this);
			try
			{
				managedLock = managedLock2;
				if (!_fAbort)
				{
					continue;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)managedLock).Dispose();
				throw;
			}
			break;
		}
		goto IL_0088;
		IL_0088:
		try
		{
			_hCompletionEvent.Set();
		}
		catch
		{
			//try-fault
			((IDisposable)managedLock).Dispose();
			throw;
		}
		((IDisposable)managedLock).Dispose();
	}

	private void Shutdown()
	{
		ManagedLock managedLock = null;
		ManagedLock managedLock2 = new ManagedLock(this);
		try
		{
			managedLock = managedLock2;
			if (!_fShutdown)
			{
				goto IL_002b;
			}
		}
		catch
		{
			//try-fault
			((IDisposable)managedLock).Dispose();
			throw;
		}
		((IDisposable)managedLock).Dispose();
		return;
		IL_002b:
		try
		{
			_fAbort = true;
			_fShutdown = true;
		}
		catch
		{
			//try-fault
			((IDisposable)managedLock).Dispose();
			throw;
		}
		((IDisposable)managedLock).Dispose();
		ManualResetEvent hCompletionEvent = _hCompletionEvent;
		if (hCompletionEvent != null)
		{
			hCompletionEvent.WaitOne();
			((IDisposable)_hCompletionEvent)?.Dispose();
			_hCompletionEvent = null;
		}
		_workItems?.Clear();
	}
}
