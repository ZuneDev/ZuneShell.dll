using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using _003CCppImplementationDetails_003E;

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
				if (_003CModule_003E.WPP_GLOBAL_Control != System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && (uint)(*(byte*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 25uL)) >= 5u)
				{
					_003CModule_003E.WPP_SF_(*(ulong*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 16uL), 10, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
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
				if (_003CModule_003E.WPP_GLOBAL_Control != System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && (uint)(*(byte*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 25uL)) >= 5u)
				{
					_003CModule_003E.WPP_SF_(*(ulong*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 16uL), 11, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
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
			if (_003CModule_003E.WPP_GLOBAL_Control != System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && (uint)(*(byte*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 25uL)) >= 5u)
			{
				_003CModule_003E.WPP_SF_(*(ulong*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 16uL), 12, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
			}
			HWND__* ptr = null;
			CComPtrNtv_003CIZuneWebHost_003E cComPtrNtv_003CIZuneWebHost_003E;
			*(long*)(&cComPtrNtv_003CIZuneWebHost_003E) = 0L;
			long result;
			try
			{
				_003CModule_003E.GetSingleton((_GUID)_003CModule_003E._GUID_51005f8f_675e_45e1_ae94_8edef996a02e, (void**)(&cComPtrNtv_003CIZuneWebHost_003E));
				fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref _003CModule_003E.PtrToStringChars(navUrl)))
				{
					ZuneWebHostEventSink* ptr2 = (ZuneWebHostEventSink*)_003CModule_003E.@new(24uL);
					ZuneWebHostEventSink* zuneWebHostEventSink;
					try
					{
						zuneWebHostEventSink = ((ptr2 == null) ? null : _003CModule_003E.Microsoft_002EZune_002EUtil_002EZuneWebHostEventSink_002E_007Bctor_007D(ptr2, this, (IZuneWebHost*)(*(ulong*)(&cComPtrNtv_003CIZuneWebHost_003E))));
					}
					catch
					{
						//try-fault
						_003CModule_003E.delete(ptr2);
						throw;
					}
					m_zuneWebHostEventSink = zuneWebHostEventSink;
					long num = *(long*)(*(ulong*)(&cComPtrNtv_003CIZuneWebHost_003E)) + 24;
					long num2 = *(long*)(&cComPtrNtv_003CIZuneWebHost_003E);
					int num3 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, HWND__*, int, int, HWND__**, int>)(*(ulong*)num))((nint)num2, ptr3, (HWND__*)hWndHost, width, height, &ptr);
					if (_003CModule_003E.WPP_GLOBAL_Control != System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && (uint)(*(byte*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 25uL)) >= 5u)
					{
						sbyte* a = (sbyte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref num3 < 0 ? ref _003CModule_003E._003F_003F_C_0040_07PPOLEBIF_0040_003F5ERROR_003F3_003F_0024AA_0040 : ref System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00_0024_0024CBD, _0024ArrayType_0024_0024_0024BY07_0024_0024CBD>(ref _003CModule_003E._003F_003F_C_0040_00CNPNBAHC_0040_003F_0024AA_0040));
						_003CModule_003E.WPP_SF_sD(*(ulong*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 16uL), 13, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids), a, (uint)num3);
					}
					result = (nint)ptr;
				}
			}
			catch
			{
				//try-fault
				_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIZuneWebHost_003E*, void>)(&_003CModule_003E.CComPtrNtv_003CIZuneWebHost_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIZuneWebHost_003E);
				throw;
			}
			_003CModule_003E.CComPtrNtv_003CIZuneWebHost_003E_002ERelease(&cComPtrNtv_003CIZuneWebHost_003E);
			return result;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool SetSize(long hWndHost, int width, int height)
		{
			//IL_004d: Expected I, but got I8
			bool result = false;
			if (_003CModule_003E.WPP_GLOBAL_Control != System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && (uint)(*(byte*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 25uL)) >= 5u)
			{
				_003CModule_003E.WPP_SF_(*(ulong*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 16uL), 14, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
			}
			if (hWndHost != 0L)
			{
				HWND__* window = _003CModule_003E.GetWindow((HWND__*)hWndHost, 5u);
				if (window != null)
				{
					_003CModule_003E.MoveWindow(window, 0, 0, width, height, 0);
					result = true;
				}
			}
			if (_003CModule_003E.WPP_GLOBAL_Control != System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && (uint)(*(byte*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 25uL)) >= 5u)
			{
				_003CModule_003E.WPP_SF_(*(ulong*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 16uL), 15, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
			}
			return result;
		}

		public unsafe void SetNavigationCompleteHandler(NavigationCompleteHandler navigationCompleteHandler)
		{
			if (_003CModule_003E.WPP_GLOBAL_Control != System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && (uint)(*(byte*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 25uL)) >= 5u)
			{
				_003CModule_003E.WPP_SF_(*(ulong*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 16uL), 16, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
			}
			m_navCompleteHandler = navigationCompleteHandler;
		}

		public unsafe void SetNavigationErrorHandler(NavigationErrorHandler navigationErrorHandler)
		{
			if (_003CModule_003E.WPP_GLOBAL_Control != System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && (uint)(*(byte*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 25uL)) >= 5u)
			{
				_003CModule_003E.WPP_SF_(*(ulong*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 16uL), 17, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
			}
			m_navErrorHandler = navigationErrorHandler;
		}

		public unsafe void OnNavigationComplete(string data)
		{
			if (_003CModule_003E.WPP_GLOBAL_Control != System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && (uint)(*(byte*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 25uL)) >= 5u)
			{
				_003CModule_003E.WPP_SF_(*(ulong*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 16uL), 18, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
			}
			if (m_navCompleteHandler != null)
			{
				m_navCompleteHandler(data);
			}
			if (_003CModule_003E.WPP_GLOBAL_Control != System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && (uint)(*(byte*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 25uL)) >= 5u)
			{
				_003CModule_003E.WPP_SF_(*(ulong*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 16uL), 19, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
			}
		}

		public unsafe void OnNavigationError(string navUrl, int errorCode)
		{
			if (_003CModule_003E.WPP_GLOBAL_Control != System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && (uint)(*(byte*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 25uL)) >= 5u)
			{
				_003CModule_003E.WPP_SF_(*(ulong*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 16uL), 20, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
			}
			if (m_navErrorHandler != null)
			{
				m_navErrorHandler(navUrl, errorCode);
			}
			if (_003CModule_003E.WPP_GLOBAL_Control != System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.WPP_GLOBAL_Control) && ((uint)(*(int*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 28uL)) & 0x8000u) != 0 && (uint)(*(byte*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 25uL)) >= 5u)
			{
				_003CModule_003E.WPP_SF_(*(ulong*)((ulong)(nint)_003CModule_003E.WPP_GLOBAL_Control + 16uL), 21, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003FA0xd3863cf3_002EWPP_ZuneWebHostInterop_cpp_Traceguids));
			}
		}
	}
}
