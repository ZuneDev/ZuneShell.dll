using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.Zune.Util;

public class RadioStationManager : IDisposable
{
	private unsafe IRadioStationManager* m_pRadioStationManager = null;

	private static RadioStationManager sm_radioStationManager = null;

	private static object sm_lock = new object();

	public static RadioStationManager Instance
	{
		get
		{
			if (sm_radioStationManager == null)
			{
				try
				{
					Monitor.Enter(sm_lock);
					if (sm_radioStationManager == null)
					{
						RadioStationManager radioStationManager = new RadioStationManager();
						Thread.MemoryBarrier();
						sm_radioStationManager = radioStationManager;
					}
				}
				finally
				{
					Monitor.Exit(sm_lock);
				}
			}
			return sm_radioStationManager;
		}
	}

	private void _007ERadioStationManager()
	{
		_0021RadioStationManager();
	}

	private unsafe void _0021RadioStationManager()
	{
		//IL_0019: Expected I, but got I8
		//IL_0022: Expected I, but got I8
		IRadioStationManager* pRadioStationManager = m_pRadioStationManager;
		if (0L != (nint)pRadioStationManager)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pRadioStationManager + 16)))((nint)pRadioStationManager);
			m_pRadioStationManager = null;
		}
	}

	public unsafe RadioPlaylist GetRadioPlaylist(string uri)
	{
		//IL_0003: Expected I, but got I8
		//IL_0006: Expected I, but got I8
		//IL_0036: Expected I, but got I8
		//IL_0054: Expected I, but got I8
		//IL_0058: Expected I, but got I8
		//IL_006a: Expected I, but got I8
		IRadioStationManager* ptr = null;
		IRadioPlaylist* ptr2 = null;
		RadioPlaylist result = null;
		fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(uri)))
		{
			int singleton = global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_e1c20902_172d_4c40_bc82_5164f64ab783, (void**)(&ptr));
			if (singleton >= 0)
			{
				long num = *(long*)ptr + 24;
				singleton = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, IRadioPlaylist**, int>)(*(ulong*)num))((nint)ptr, ptr3, &ptr2);
				if (singleton >= 0)
				{
					result = new RadioPlaylist(ptr2);
				}
			}
			if (0L != (nint)ptr)
			{
				IRadioStationManager* intPtr = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
				ptr = null;
			}
			if (0L != (nint)ptr2)
			{
				IRadioPlaylist* intPtr2 = ptr2;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
			return result;
		}
	}

	public unsafe void AddStation(string title, string sourceUrl, string imageUrl, RadioStationProgressHandler radioStationProgressHandler)
	{
		//IL_001a: Expected I, but got I8
		//IL_0060: Expected I, but got I8
		//IL_0073: Expected I, but got I8
		//IL_0086: Expected I, but got I8
		RadioStationProxy* ptr = (RadioStationProxy*)global::_003CModule_003E.@new(24uL);
		RadioStationProxy* ptr2;
		try
		{
			ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EUtil_002ERadioStationProxy_002E_007Bctor_007D(ptr, radioStationProgressHandler));
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.delete(ptr);
			throw;
		}
		fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(title)))
		{
			fixed (ushort* ptr4 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(sourceUrl)))
			{
				fixed (ushort* ptr5 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(imageUrl)))
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out IRadioStationManager* ptr6);
					if (global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_e1c20902_172d_4c40_bc82_5164f64ab783, (void**)(&ptr6)) >= 0)
					{
						long num = *(long*)ptr6 + 32;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, ushort*, IAsyncCallback*, int>)(*(ulong*)num))((nint)ptr6, ptr3, ptr4, ptr5, (IAsyncCallback*)ptr2);
					}
					if (0L != (nint)ptr2)
					{
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
					}
					if (0L != (nint)ptr6)
					{
						IRadioStationManager* intPtr = ptr6;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
					}
				}
			}
		}
	}

	public unsafe void DeleteStation(string title, RadioStationProgressHandler radioStationProgressHandler)
	{
		//IL_0019: Expected I, but got I8
		//IL_004b: Expected I, but got I8
		//IL_005e: Expected I, but got I8
		//IL_0071: Expected I, but got I8
		RadioStationProxy* ptr = (RadioStationProxy*)global::_003CModule_003E.@new(24uL);
		RadioStationProxy* ptr2;
		try
		{
			ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EUtil_002ERadioStationProxy_002E_007Bctor_007D(ptr, radioStationProgressHandler));
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.delete(ptr);
			throw;
		}
		fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(title)))
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out IRadioStationManager* ptr4);
			if (global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_e1c20902_172d_4c40_bc82_5164f64ab783, (void**)(&ptr4)) >= 0)
			{
				long num = *(long*)ptr4 + 40;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, IAsyncCallback*, int>)(*(ulong*)num))((nint)ptr4, ptr3, (IAsyncCallback*)ptr2);
			}
			if (0L != (nint)ptr2)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
			}
			if (0L != (nint)ptr4)
			{
				IRadioStationManager* intPtr = ptr4;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
			}
		}
	}

	private unsafe RadioStationManager()
	{
	}//IL_0008: Expected I, but got I8


	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
	{
		if (P_0)
		{
			_0021RadioStationManager();
			return;
		}
		try
		{
			_0021RadioStationManager();
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

	~RadioStationManager()
	{
		Dispose(false);
	}
}
