using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
namespace Microsoft.Zune.Util
{
	public class ZuneWebHost
	{
		private static object s_lock = new object();

		private static ZuneWebHost s_zuneWebHost = null;

		private unsafe ZuneWebHostEventSink* m_zuneWebHostEventSink;

		private NavigationCompleteHandler m_navCompleteHandler;

		private NavigationErrorHandler m_navErrorHandler;

		public unsafe static ZuneWebHost Instance
		{
			get
			{
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 25uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 16uL), 10, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
				}
				if (s_zuneWebHost == null)
				{
					try
					{
						Monitor.Enter(s_lock);
						if (s_zuneWebHost == null)
						{
							ZuneWebHost zuneWebHost = new ZuneWebHost();
							Thread.MemoryBarrier();
							s_zuneWebHost = zuneWebHost;
						}
					}
					finally
					{
						Monitor.Exit(s_lock);
					}
				}
				if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 25uL) >= 5u)
				{
					Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 16uL), 11, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
				}
				return s_zuneWebHost;
			}
		}

		public unsafe long Initialize(string navUrl, long hWndHost, int width, int height)
		{
			//IL_0045: Expected I, but got I8
			//IL_0075: Expected I, but got I8
			//IL_007b: Expected I, but got I8
			//IL_00a8: Expected I, but got I8
			//IL_00a8: Expected I, but got I8
			//IL_00a8: Expected I, but got I8
			//IL_0102: Expected I8, but got I
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 25uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 16uL), 12, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
			}
			HWND* ptr = null;
			CComPtrNtv<IZuneWebHost> cComPtrNtv_003CIZuneWebHost_003E = new();
			long result;
			try
			{
				Module.GetSingleton((_GUID)Module._GUID_51005f8f_675e_45e1_ae94_8edef996a02e, (void**)(cComPtrNtv_003CIZuneWebHost_003E.p));
				fixed (char* navUrlPtr = navUrl.ToCharArray())
				{
					ushort* ptr3 = (ushort*)navUrlPtr;
					ZuneWebHostEventSink* ptr2 = (ZuneWebHostEventSink*)Module.@new(24uL);
					ZuneWebHostEventSink* zuneWebHostEventSink;
					try
					{
						zuneWebHostEventSink = ((ptr2 == null) ? null : Module.Microsoft_002EZune_002EUtil_002EZuneWebHostEventSink_002E_007Bctor_007D(ptr2, this, (IZuneWebHost*)(*(ulong*)(cComPtrNtv_003CIZuneWebHost_003E.p))));
					}
					catch
					{
						//try-fault
						Module.delete(ptr2);
						throw;
					}
					m_zuneWebHostEventSink = zuneWebHostEventSink;
					long num = *(long*)(*(ulong*)(cComPtrNtv_003CIZuneWebHost_003E.p)) + 24;
					long num2 = *(long*)(cComPtrNtv_003CIZuneWebHost_003E.p);
					int num3 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, HWND*, int, int, HWND**, int>)(*(ulong*)num))((nint)num2, ptr3, (HWND*)hWndHost, width, height, &ptr);
					if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 25uL) >= 5u)
					{
						sbyte* a = (sbyte*)Unsafe.AsPointer(ref num3 < 0 ? ref Module._003F_003F_C_0040_07PPOLEBIF_0040_003F5ERROR_003F3_003F_0024AA_0040 : ref Unsafe.As<_0024ArrayType_0024_0024_0024BY00_0024_0024CBD, _0024ArrayType_0024_0024_0024BY07_0024_0024CBD>(ref Module._003F_003F_C_0040_00CNPNBAHC_0040_003F_0024AA_0040));
						Module.WPP_SF_sD(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 16uL), 13, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids), a, (uint)num3);
					}
					result = (nint)ptr;
				}
			}
			catch
			{
				//try-fault
				cComPtrNtv_003CIZuneWebHost_003E.Dispose();
				throw;
			}
			cComPtrNtv_003CIZuneWebHost_003E.Dispose();
			return result;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool SetSize(long hWndHost, int width, int height)
		{
			//IL_004d: Expected I, but got I8
			bool result = false;
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 25uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 16uL), 14, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
			}
			if (hWndHost != 0L)
			{
				HWND* window = Module.GetWindow((HWND*)hWndHost, 5u);
				if (window != null)
				{
					Module.MoveWindow(window, 0, 0, width, height, 0);
					result = true;
				}
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 25uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 16uL), 15, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
			}
			return result;
		}

		public unsafe void SetNavigationCompleteHandler(NavigationCompleteHandler navigationCompleteHandler)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 25uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 16uL), 16, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
			}
			m_navCompleteHandler = navigationCompleteHandler;
		}

		public unsafe void SetNavigationErrorHandler(NavigationErrorHandler navigationErrorHandler)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 25uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 16uL), 17, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
			}
			m_navErrorHandler = navigationErrorHandler;
		}

		public unsafe void OnNavigationComplete(string data)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 25uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 16uL), 18, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
			}
			if (m_navCompleteHandler != null)
			{
				m_navCompleteHandler(data);
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 25uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 16uL), 19, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
			}
		}

		public unsafe void OnNavigationError(string navUrl, int errorCode)
		{
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 25uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 16uL), 20, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
			}
			if (m_navErrorHandler != null)
			{
				m_navErrorHandler(navUrl, errorCode);
			}
			if (Module.WPP_GLOBAL_Control != Unsafe.AsPointer(ref Module.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)Module.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && *(byte*)((ulong)(nint)Module.WPP_GLOBAL_Control + 25uL) >= 5u)
			{
				Module.WPP_SF_(*(ulong*)((ulong)(nint)Module.WPP_GLOBAL_Control + 16uL), 21, (_GUID*)Unsafe.AsPointer(ref Module._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
			}
		}
	}
}
