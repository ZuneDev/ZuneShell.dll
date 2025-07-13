using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.Zune.Util;

public class Win7ShellManager
{
	private WindowPositionKeyPressHandler _003Cbacking_store_003EOnWindowPositionKeyPress;

	private ThumbBarButtonPressHandler _003Cbacking_store_003EOnThumbBarButtonPress;

	private MonitorChangeHandler _003Cbacking_store_003EOnMonitorChange;

	private static Win7ShellManager sm_win7ShellManager = null;

	private static object sm_lock = new object();

	public static Win7ShellManager Instance
	{
		get
		{
			if (sm_win7ShellManager == null)
			{
				try
				{
					Monitor.Enter(sm_lock);
					if (sm_win7ShellManager == null)
					{
						Win7ShellManager win7ShellManager = new Win7ShellManager();
						Thread.MemoryBarrier();
						sm_win7ShellManager = win7ShellManager;
					}
				}
				finally
				{
					Monitor.Exit(sm_lock);
				}
			}
			return sm_win7ShellManager;
		}
	}

	[SpecialName]
	public event MonitorChangeHandler OnMonitorChange
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			_003Cbacking_store_003EOnMonitorChange = (MonitorChangeHandler)Delegate.Combine(_003Cbacking_store_003EOnMonitorChange, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			_003Cbacking_store_003EOnMonitorChange = (MonitorChangeHandler)Delegate.Remove(_003Cbacking_store_003EOnMonitorChange, value);
		}
	}

	[SpecialName]
	public event ThumbBarButtonPressHandler OnThumbBarButtonPress
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			_003Cbacking_store_003EOnThumbBarButtonPress = (ThumbBarButtonPressHandler)Delegate.Combine(_003Cbacking_store_003EOnThumbBarButtonPress, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			_003Cbacking_store_003EOnThumbBarButtonPress = (ThumbBarButtonPressHandler)Delegate.Remove(_003Cbacking_store_003EOnThumbBarButtonPress, value);
		}
	}

	[SpecialName]
	public event WindowPositionKeyPressHandler OnWindowPositionKeyPress
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			_003Cbacking_store_003EOnWindowPositionKeyPress = (WindowPositionKeyPressHandler)Delegate.Combine(_003Cbacking_store_003EOnWindowPositionKeyPress, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			_003Cbacking_store_003EOnWindowPositionKeyPress = (WindowPositionKeyPressHandler)Delegate.Remove(_003Cbacking_store_003EOnWindowPositionKeyPress, value);
		}
	}

	public unsafe int AddLocationToLibrary(EWin7LibraryKind libraryKind, [MarshalAs(UnmanagedType.U1)] bool defaultSaveFolder, string path)
	{
		//IL_0035: Expected I, but got I8
		//IL_0035: Expected I, but got I8
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIWin7ShellManager_003E cComPtrNtv_003CIWin7ShellManager_003E);
		*(long*)(&cComPtrNtv_003CIWin7ShellManager_003E) = 0L;
		int num;
		try
		{
			num = global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_a89c52eb_97a9_417b_9872_46c040f1b76f, (void**)(&cComPtrNtv_003CIWin7ShellManager_003E));
			if (num >= 0)
			{
				fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(path)))
				{
					try
					{
						long num2 = *(long*)(*(ulong*)(&cComPtrNtv_003CIWin7ShellManager_003E)) + 96;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EWin7LibraryKind, int, ushort*, int>)(*(ulong*)num2))((nint)(*(long*)(&cComPtrNtv_003CIWin7ShellManager_003E)), libraryKind, defaultSaveFolder ? 1 : 0, ptr);
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
				}
			}
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIWin7ShellManager_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIWin7ShellManager_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIWin7ShellManager_003E);
			throw;
		}
		global::_003CModule_003E.CComPtrNtv_003CIWin7ShellManager_003E_002ERelease(&cComPtrNtv_003CIWin7ShellManager_003E);
		return num;
	}

	public unsafe int RemoveLocationFromLibrary(EWin7LibraryKind libraryKind, out bool defaultSaveFolder, string path)
	{
		//IL_0037: Expected I, but got I8
		//IL_0037: Expected I, but got I8
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIWin7ShellManager_003E cComPtrNtv_003CIWin7ShellManager_003E);
		*(long*)(&cComPtrNtv_003CIWin7ShellManager_003E) = 0L;
		int num;
		try
		{
			num = global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_a89c52eb_97a9_417b_9872_46c040f1b76f, (void**)(&cComPtrNtv_003CIWin7ShellManager_003E));
			int num2 = 0;
			int num4;
			if (num >= 0)
			{
				fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(path)))
				{
					try
					{
						long num3 = *(long*)(*(ulong*)(&cComPtrNtv_003CIWin7ShellManager_003E)) + 104;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EWin7LibraryKind, int*, ushort*, int>)(*(ulong*)num3))((nint)(*(long*)(&cComPtrNtv_003CIWin7ShellManager_003E)), libraryKind, &num2, ptr);
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
				}
				if (num2 == 1)
				{
					num4 = 1;
					goto IL_004c;
				}
			}
			num4 = 0;
			goto IL_004c;
			IL_004c:
			defaultSaveFolder = (byte)num4 != 0;
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIWin7ShellManager_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIWin7ShellManager_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIWin7ShellManager_003E);
			throw;
		}
		global::_003CModule_003E.CComPtrNtv_003CIWin7ShellManager_003E_002ERelease(&cComPtrNtv_003CIWin7ShellManager_003E);
		return num;
	}

	public unsafe int BeginJumpListSession(out JumpListSession session)
	{
		//IL_0003: Expected I, but got I8
		//IL_0006: Expected I, but got I8
		//IL_0026: Expected I, but got I8
		//IL_0045: Expected I, but got I8
		//IL_0049: Expected I, but got I8
		//IL_005b: Expected I, but got I8
		IWin7ShellManager* ptr = null;
		IJumpList* ptr2 = null;
		int num = global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_a89c52eb_97a9_417b_9872_46c040f1b76f, (void**)(&ptr));
		if (num >= 0)
		{
			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IJumpList**, int>)(*(ulong*)(*(long*)ptr + 24)))((nint)ptr, &ptr2);
			if (num >= 0)
			{
				session = new JumpListSession(ptr2);
			}
		}
		if (0L != (nint)ptr2)
		{
			IJumpList* intPtr = ptr2;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
			ptr2 = null;
		}
		if (0L != (nint)ptr)
		{
			IWin7ShellManager* intPtr2 = ptr;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
		}
		return num;
	}

	public unsafe int BeginThumbBarSession(IntPtr hWnd, out ThumbBar thumbBar)
	{
		//IL_0003: Expected I, but got I8
		//IL_0006: Expected I, but got I8
		//IL_002f: Expected I, but got I8
		//IL_004e: Expected I, but got I8
		//IL_0052: Expected I, but got I8
		//IL_0064: Expected I, but got I8
		IWin7ShellManager* ptr = null;
		IThumbBar* ptr2 = null;
		int num = global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_a89c52eb_97a9_417b_9872_46c040f1b76f, (void**)(&ptr));
		if (num >= 0)
		{
			long num2 = *(long*)ptr + 32;
			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HWND__*, IThumbBar**, int>)(*(ulong*)num2))((nint)ptr, (HWND__*)hWnd.ToPointer(), &ptr2);
			if (num >= 0)
			{
				thumbBar = new ThumbBar(ptr2);
			}
		}
		if (0L != (nint)ptr2)
		{
			IThumbBar* intPtr = ptr2;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
			ptr2 = null;
		}
		if (0L != (nint)ptr)
		{
			IWin7ShellManager* intPtr2 = ptr;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
		}
		return num;
	}

	public unsafe int SubprocWindow(IntPtr hWnd)
	{
		//IL_0003: Expected I, but got I8
		//IL_002a: Expected I, but got I8
		//IL_003d: Expected I, but got I8
		IWin7ShellManager* ptr = null;
		int num = global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_a89c52eb_97a9_417b_9872_46c040f1b76f, (void**)(&ptr));
		if (num >= 0)
		{
			long num2 = *(long*)ptr + 40;
			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HWND__*, int>)(*(ulong*)num2))((nint)ptr, (HWND__*)hWnd.ToPointer());
		}
		if (0L != (nint)ptr)
		{
			IWin7ShellManager* intPtr = ptr;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
		}
		return num;
	}

	public unsafe int ShowLibraryDialog(EWin7LibraryKind libraryKind, IntPtr hWnd, string title, string helpText)
	{
		//IL_0013: Expected I, but got I8
		//IL_003e: Expected I, but got I8
		//IL_0051: Expected I, but got I8
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(title)))
		{
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(helpText)))
			{
				IWin7ShellManager* ptr3 = null;
				int num = global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_a89c52eb_97a9_417b_9872_46c040f1b76f, (void**)(&ptr3));
				if (num >= 0)
				{
					long num2 = *(long*)ptr3 + 80;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EWin7LibraryKind, HWND__*, ushort*, ushort*, int>)(*(ulong*)num2))((nint)ptr3, libraryKind, (HWND__*)hWnd.ToPointer(), ptr, ptr2);
				}
				if (0L != (nint)ptr3)
				{
					IWin7ShellManager* intPtr = ptr3;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
				}
				return num;
			}
		}
	}

	public unsafe int SyncLibraryFolders()
	{
		//IL_0003: Expected I, but got I8
		//IL_0021: Expected I, but got I8
		//IL_0034: Expected I, but got I8
		IWin7ShellManager* ptr = null;
		int num = global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_a89c52eb_97a9_417b_9872_46c040f1b76f, (void**)(&ptr));
		if (num >= 0)
		{
			IWin7ShellManager* intPtr = ptr;
			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)intPtr + 88)))((nint)intPtr);
		}
		if (0L != (nint)ptr)
		{
			IWin7ShellManager* intPtr2 = ptr;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
		}
		return num;
	}

	public unsafe int CreatePodcastLibraryTemplate()
	{
		//IL_0003: Expected I, but got I8
		//IL_0021: Expected I, but got I8
		//IL_0034: Expected I, but got I8
		IWin7Libraries* ptr = null;
		int num = global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_e24c5c6a_85a5_440e_93e1_bb51e32033ac, (void**)(&ptr));
		if (num >= 0)
		{
			IWin7Libraries* intPtr = ptr;
			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)intPtr + 56)))((nint)intPtr);
		}
		if (0L != (nint)ptr)
		{
			IWin7Libraries* intPtr2 = ptr;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
		}
		return num;
	}

	[SpecialName]
	protected void raise_OnWindowPositionKeyPress(WindowPositionKeys value0)
	{
		_003Cbacking_store_003EOnWindowPositionKeyPress?.Invoke(value0);
	}

	[SpecialName]
	protected void raise_OnThumbBarButtonPress(uint value0)
	{
		_003Cbacking_store_003EOnThumbBarButtonPress?.Invoke(value0);
	}

	[SpecialName]
	protected void raise_OnMonitorChange()
	{
		_003Cbacking_store_003EOnMonitorChange?.Invoke();
	}

	internal int WindowPositionKeyPressDetected(WindowPositionKeys key)
	{
		raise_OnWindowPositionKeyPress(key);
		return 0;
	}

	internal int ThumbBarButtonPressed(uint iUniqueID)
	{
		raise_OnThumbBarButtonPress(iUniqueID);
		return 0;
	}

	internal int MonitorChanged()
	{
		raise_OnMonitorChange();
		return 0;
	}

	private unsafe Win7ShellManager()
	{
		//IL_0009: Expected I, but got I8
		//IL_005b: Expected I, but got I8
		//IL_0031: Expected I, but got I8
		//IL_0048: Expected I, but got I8
		IWin7ShellManager* ptr = null;
		if (global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_a89c52eb_97a9_417b_9872_46c040f1b76f, (void**)(&ptr)) >= 0)
		{
			Win7ShellManagerMediator* ptr2 = (Win7ShellManagerMediator*)global::_003CModule_003E.@new(24uL);
			Win7ShellManagerMediator* ptr3;
			try
			{
				ptr3 = ((ptr2 == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EUtil_002EWin7ShellManagerMediator_002E_007Bctor_007D(ptr2, this));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr2);
				throw;
			}
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IWin7ShellManagerMediator*, int>)(*(ulong*)(*(long*)ptr + 48)))((nint)ptr, (IWin7ShellManagerMediator*)ptr3);
		}
		if (0L != (nint)ptr)
		{
			IWin7ShellManager* intPtr = ptr;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
		}
	}
}
