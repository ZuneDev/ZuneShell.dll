using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Util
{
	public class DownloadTask : IDisposable
	{
		private unsafe DownloadTaskProxy* m_pDownloadTaskProxy;

		private DownloadProgressHandler m_defaultProgressHandler;

		[SpecialName]
		public unsafe event DownloadProgressHandler OnProgressChanged
		{
			add
			{
				int num = _003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002ESetDelegate(m_pDownloadTaskProxy, value);
				if (num < 0)
				{
					throw new ApplicationException(_003CModule_003E.GetErrorDescription(num));
				}
			}
			remove
			{
				_003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002EResetDelegate(m_pDownloadTaskProxy);
			}
		}

		public unsafe DownloadTask(IDownloadTask* pDownloadTask)
		{
			//IL_001f: Expected I, but got I8
			DownloadTaskProxy* ptr = (DownloadTaskProxy*)_003CModule_003E.@new(32uL);
			DownloadTaskProxy* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : _003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002E_007Bctor_007D(ptr, pDownloadTask));
			}
			catch
			{
				//try-fault
				_003CModule_003E.delete(ptr);
				throw;
			}
			m_pDownloadTaskProxy = ptr2;
			if (ptr2 == null)
			{
				throw new ApplicationException(_003CModule_003E.GetErrorDescription(-2147024882));
			}
		}

		private void _007EDownloadTask()
		{
			_0021DownloadTask();
		}

		private unsafe void _0021DownloadTask()
		{
			//IL_0019: Expected I, but got I8
			//IL_0022: Expected I, but got I8
			DownloadTaskProxy* pDownloadTaskProxy = m_pDownloadTaskProxy;
			if (0L != (nint)pDownloadTaskProxy)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pDownloadTaskProxy + 16)))((nint)pDownloadTaskProxy);
				m_pDownloadTaskProxy = null;
			}
		}

		public unsafe string GetTaskId()
		{
			return _003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002EGetTaskId(m_pDownloadTaskProxy);
		}

		public unsafe EDownloadTaskState GetState()
		{
			return _003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002EGetState(m_pDownloadTaskProxy);
		}

		public unsafe float GetProgress()
		{
			return _003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002EGetProgress(m_pDownloadTaskProxy);
		}

		public unsafe ulong GetBytesDownloaded()
		{
			return _003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002EGetBytesDownloaded(m_pDownloadTaskProxy);
		}

		public unsafe int GetDownloadSecondsRemaining()
		{
			return _003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002EGetDownloadSecondsRemaining(m_pDownloadTaskProxy);
		}

		public unsafe int GetDownloadFileSecondsRemaining(int iFile)
		{
			return _003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002EGetDownloadFileSecondsRemaining(m_pDownloadTaskProxy, iFile);
		}

		public unsafe string GetTempFileName(int iFile)
		{
			return _003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002EGetTempFileName(m_pDownloadTaskProxy, iFile);
		}

		public unsafe ulong GetFinalFileSize(int iFile)
		{
			return _003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002EGetFinalFileSize(m_pDownloadTaskProxy, iFile);
		}

		public unsafe int GetDownloadBytesPerSecond()
		{
			return _003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002EGetDownloadBytesPerSecond(m_pDownloadTaskProxy);
		}

		public unsafe string GetProperty(string propertyName)
		{
			return _003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002EGetProperty(m_pDownloadTaskProxy, propertyName);
		}

		public unsafe int GetPropertyInt(string propertyName)
		{
			return _003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002EGetPropertyInt(m_pDownloadTaskProxy, propertyName);
		}

		public unsafe void SetProperty(string propertyName, string propertyValue)
		{
			_003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002ESetProperty(m_pDownloadTaskProxy, propertyName, propertyValue);
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool CanCancel()
		{
			EDownloadTaskState eDownloadTaskState = _003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002EGetState(m_pDownloadTaskProxy);
			int num = ((eDownloadTaskState == EDownloadTaskState.DLTaskPending || eDownloadTaskState == EDownloadTaskState.DLTaskPendingAttach || eDownloadTaskState == EDownloadTaskState.DLTaskDownloading || eDownloadTaskState == EDownloadTaskState.DLTaskPaused) ? 1 : 0);
			return (byte)num != 0;
		}

		public unsafe void Cancel()
		{
			_003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002ECancel(m_pDownloadTaskProxy);
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool CanReorder()
		{
			EDownloadTaskState eDownloadTaskState = _003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002EGetState(m_pDownloadTaskProxy);
			int num = ((eDownloadTaskState == EDownloadTaskState.DLTaskPending || eDownloadTaskState == EDownloadTaskState.DLTaskPendingAttach || eDownloadTaskState == EDownloadTaskState.DLTaskDownloading || eDownloadTaskState == EDownloadTaskState.DLTaskPaused) ? 1 : 0);
			return (byte)num != 0;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe override bool Equals(object obj)
		{
			bool result = false;
			DownloadTask downloadTask = obj as DownloadTask;
			if (downloadTask != null)
			{
				result = _003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002EEquals(m_pDownloadTaskProxy, downloadTask.m_pDownloadTaskProxy);
			}
			return result;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		internal unsafe void DownloadNext()
		{
			_003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002EDownloadNext(m_pDownloadTaskProxy);
		}

		[return: MarshalAs(UnmanagedType.U1)]
		internal unsafe bool SetPosition(int position)
		{
			return (byte)((_003CModule_003E.Microsoft_002EZune_002EUtil_002EDownloadTaskProxy_002ESetPosition(m_pDownloadTaskProxy, (uint)position) >= 0) ? 1u : 0u) != 0;
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_0021DownloadTask();
				return;
			}
			try
			{
				_0021DownloadTask();
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

		~DownloadTask()
		{
			Dispose(false);
		}
	}
}
