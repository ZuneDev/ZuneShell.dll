using System;
using System.Runtime.InteropServices;
using ZuneDBApi.Interop;
using ZuneUI;

namespace Microsoft.Zune.Util;

public class ContentRefreshTask : IDisposable
{
	private unsafe IContentRefreshTask* m_pContentRefreshTask = null;

	private static ContentRefreshTask sm_ContentRefreshTask = null;

	public static bool HasInstance
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return sm_ContentRefreshTask != null;
		}
	}

	public static ContentRefreshTask Instance
	{
		get
		{
			if (sm_ContentRefreshTask == null)
			{
				sm_ContentRefreshTask = new ContentRefreshTask();
			}
			return sm_ContentRefreshTask;
		}
	}

	private void _007EContentRefreshTask()
	{
		_0021ContentRefreshTask();
	}

	private unsafe void _0021ContentRefreshTask()
	{
		//IL_0017: Expected I, but got I8
		//IL_0031: Expected I, but got I8
		//IL_003a: Expected I, but got I8
		IContentRefreshTask* pContentRefreshTask = m_pContentRefreshTask;
		if (pContentRefreshTask != null)
		{
			IContentRefreshTask* intPtr = pContentRefreshTask;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)intPtr + 32)))((nint)intPtr);
			pContentRefreshTask = m_pContentRefreshTask;
			if (0L != (nint)pContentRefreshTask)
			{
				IContentRefreshTask* intPtr2 = pContentRefreshTask;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
				m_pContentRefreshTask = null;
			}
		}
	}

	public unsafe HRESULT StartContentRefresh(AsyncCompleteHandler completeHandler)
	{
		//IL_001b: Expected I, but got I8
		//IL_0035: Expected I, but got I8
		//IL_007c: Expected I, but got I8
		//IL_0066: Expected I, but got I8
		//IL_006a: Expected I, but got I8
		int num = 0;
		AsyncCallbackWrapper* ptr = (AsyncCallbackWrapper*)global::_003CModule_003E.@new(24uL);
		AsyncCallbackWrapper* ptr2;
		try
		{
			ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EUtil_002EAsyncCallbackWrapper_002E_007Bctor_007D(ptr, completeHandler));
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.delete(ptr);
			throw;
		}
		num = (((long)(nint)ptr2 == 0) ? (-2147418113) : num);
		IContentRefreshTask* ptr3 = null;
		if (num >= 0)
		{
			num = ZuneLibraryExports.CreateContentRefreshTask((IAsyncCallback*)ptr2, &ptr3);
			if (num >= 0)
			{
				if (0L != (nint)ptr3)
				{
					m_pContentRefreshTask = ptr3;
				}
			}
			else if (0L != (nint)ptr3)
			{
				IContentRefreshTask* intPtr = ptr3;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
				ptr3 = null;
			}
		}
		if (0L != (nint)ptr2)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
		}
		return new HRESULT(num);
	}

	private unsafe ContentRefreshTask()
	{
	}//IL_0008: Expected I, but got I8


	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
	{
		if (P_0)
		{
			_0021ContentRefreshTask();
			return;
		}
		try
		{
			_0021ContentRefreshTask();
		}
		finally
		{
			base.Finalize();
		}
	}

	public virtual sealed void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	~ContentRefreshTask()
	{
		Dispose(false);
	}
}
