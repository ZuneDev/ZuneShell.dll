using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.Zune.Util
{
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
					lock (sm_lock)
                    {
						if (sm_win7ShellManager == null)
						{
							Win7ShellManager win7ShellManager = new Win7ShellManager();
							Thread.MemoryBarrier();
							sm_win7ShellManager = win7ShellManager;
						}
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
			CComPtrNtv<IWin7ShellManager> cComPtrNtv_003CIWin7ShellManager_003E = new();
			int num;
			try
			{
				num = Module.GetSingleton(Module.GUID_IWin7ShellManager, (void**)(cComPtrNtv_003CIWin7ShellManager_003E.p));
				if (num >= 0)
				{
					fixed (char* pathPtr = path.ToCharArray())
					{
						ushort* ptr = (ushort*)pathPtr;
						try
						{
							long num2 = *(long*)(*(ulong*)(cComPtrNtv_003CIWin7ShellManager_003E.p)) + 96;
							long num3 = *(long*)(cComPtrNtv_003CIWin7ShellManager_003E.p);
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EWin7LibraryKind, int, ushort*, int>)(*(ulong*)num2))((nint)num3, libraryKind, defaultSaveFolder ? 1 : 0, ptr);
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
				cComPtrNtv_003CIWin7ShellManager_003E.Dispose();
				throw;
			}
			cComPtrNtv_003CIWin7ShellManager_003E.Dispose();
			return num;
		}

		public unsafe int RemoveLocationFromLibrary(EWin7LibraryKind libraryKind, out bool defaultSaveFolder, string path)
		{
			//IL_0037: Expected I, but got I8
			//IL_0037: Expected I, but got I8
			CComPtrNtv<IWin7ShellManager> cComPtrNtv_003CIWin7ShellManager_003E = new();
			int num;
			try
			{
				num = Module.GetSingleton(Module.GUID_IWin7ShellManager, (void**)(cComPtrNtv_003CIWin7ShellManager_003E.p));
				int num2 = 0;
				int num5;
				if (num >= 0)
				{
					fixed (char* pathPtr = path.ToCharArray())
					{
						ushort* ptr = (ushort*)pathPtr;
						try
						{
							long num3 = *(long*)(*(ulong*)(cComPtrNtv_003CIWin7ShellManager_003E.p)) + 104;
							long num4 = *(long*)(cComPtrNtv_003CIWin7ShellManager_003E.p);
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EWin7LibraryKind, int*, ushort*, int>)(*(ulong*)num3))((nint)num4, libraryKind, &num2, ptr);
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
						num5 = 1;
						goto IL_004c;
					}
				}
				num5 = 0;
				goto IL_004c;
				IL_004c:
				defaultSaveFolder = (byte)num5 != 0;
			}
			catch
			{
				//try-fault
				cComPtrNtv_003CIWin7ShellManager_003E.Dispose();
				throw;
			}
			cComPtrNtv_003CIWin7ShellManager_003E.Dispose();
			return num;
		}

		public unsafe int BeginJumpListSession(out JumpListSession session)
		{
			session = null;
			//IL_0003: Expected I, but got I8
			//IL_0006: Expected I, but got I8
			//IL_0026: Expected I, but got I8
			//IL_0045: Expected I, but got I8
			//IL_0049: Expected I, but got I8
			//IL_005b: Expected I, but got I8
			IWin7ShellManager* ptr = null;
			IJumpList* ptr2 = null;
			int num = Module.GetSingleton(Module.GUID_IWin7ShellManager, (void**)(&ptr));
			if (num >= 0)
			{
				IWin7ShellManager* intPtr = ptr;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IJumpList**, int>)(*(ulong*)(*(long*)ptr + 24)))((nint)intPtr, &ptr2);
				if (num >= 0)
				{
					session = new JumpListSession(ptr2);
				}
			}
			if (0L != (nint)ptr2)
			{
				IJumpList* intPtr2 = ptr2;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
				ptr2 = null;
			}
			if (0L != (nint)ptr)
			{
				IWin7ShellManager* intPtr3 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
			}
			return num;
		}

		public unsafe int BeginThumbBarSession(IntPtr hWnd, out ThumbBar thumbBar)
		{
			thumbBar = null;
			//IL_0003: Expected I, but got I8
			//IL_0006: Expected I, but got I8
			//IL_002f: Expected I, but got I8
			//IL_004e: Expected I, but got I8
			//IL_0052: Expected I, but got I8
			//IL_0064: Expected I, but got I8
			IWin7ShellManager* ptr = null;
			IThumbBar* ptr2 = null;
			int num = Module.GetSingleton(Module.GUID_IWin7ShellManager, (void**)(&ptr));
			if (num >= 0)
			{
				long num2 = *(long*)ptr + 32;
				IWin7ShellManager* intPtr = ptr;
				void* intPtr2 = hWnd.ToPointer();
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HWND*, IThumbBar**, int>)(*(ulong*)num2))((nint)intPtr, (HWND*)intPtr2, &ptr2);
				if (num >= 0)
				{
					thumbBar = new ThumbBar(ptr2);
				}
			}
			if (0L != (nint)ptr2)
			{
				IThumbBar* intPtr3 = ptr2;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
				ptr2 = null;
			}
			if (0L != (nint)ptr)
			{
				IWin7ShellManager* intPtr4 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr4 + 16)))((nint)intPtr4);
			}
			return num;
		}

		public unsafe int SubprocWindow(IntPtr hWnd)
		{
			//IL_0003: Expected I, but got I8
			//IL_002a: Expected I, but got I8
			//IL_003d: Expected I, but got I8
			IWin7ShellManager* ptr = null;
			int num = Module.GetSingleton(Module.GUID_IWin7ShellManager, (void**)(&ptr));
			if (num >= 0)
			{
				long num2 = *(long*)ptr + 40;
				IWin7ShellManager* intPtr = ptr;
				void* intPtr2 = hWnd.ToPointer();
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, HWND*, int>)(*(ulong*)num2))((nint)intPtr, (HWND*)intPtr2);
			}
			if (0L != (nint)ptr)
			{
				IWin7ShellManager* intPtr3 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
			}
			return num;
		}

		public unsafe int ShowLibraryDialog(EWin7LibraryKind libraryKind, IntPtr hWnd, string title, string helpText)
		{
			//IL_0013: Expected I, but got I8
			//IL_003e: Expected I, but got I8
			//IL_0051: Expected I, but got I8
			fixed (char* titlePtr = title.ToCharArray())
			{
				ushort* ptr2 = (ushort*)titlePtr;
				fixed (char* helpTextPtr = helpText.ToCharArray())
				{
					ushort* ptr3 = (ushort*)helpTextPtr;
					IWin7ShellManager* ptr = null;
					int num = Module.GetSingleton(Module.GUID_IWin7ShellManager, (void**)(&ptr));
					if (num >= 0)
					{
						long num2 = *(long*)ptr + 80;
						IWin7ShellManager* intPtr = ptr;
						void* intPtr2 = hWnd.ToPointer();
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EWin7LibraryKind, HWND*, ushort*, ushort*, int>)(*(ulong*)num2))((nint)intPtr, libraryKind, (HWND*)intPtr2, ptr2, ptr3);
					}
					if (0L != (nint)ptr)
					{
						IWin7ShellManager* intPtr3 = ptr;
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
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
			int num = Module.GetSingleton(Module.GUID_IWin7ShellManager, (void**)(&ptr));
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
			int num = Module.GetSingleton(Module.GUID_IWin7Libraries, (void**)(&ptr));
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
			//IL_0031: Expected I, but got I8
			//IL_0048: Expected I, but got I8
			//IL_005b: Expected I, but got I8
			IWin7ShellManager* ptr = null;
			if (Module.GetSingleton(Module.GUID_IWin7ShellManager, (void**)(&ptr)) >= 0)
			{
				Win7ShellManagerMediator* ptr2 = (Win7ShellManagerMediator*)Module.@new(24uL);
				Win7ShellManagerMediator* ptr3;
				try
				{
					ptr3 = ((ptr2 == null) ? null : Module.Microsoft_002EZune_002EUtil_002EWin7ShellManagerMediator_002E_007Bctor_007D(ptr2, this));
				}
				catch
				{
					//try-fault
					Module.delete(ptr2);
					throw;
				}
				IWin7ShellManager* intPtr = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IWin7ShellManagerMediator*, int>)(*(ulong*)(*(long*)ptr + 48)))((nint)intPtr, (IWin7ShellManagerMediator*)ptr3);
			}
			if (0L != (nint)ptr)
			{
				IWin7ShellManager* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
		}
	}
}
