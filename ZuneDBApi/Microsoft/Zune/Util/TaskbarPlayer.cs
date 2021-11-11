using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Iris;

namespace Microsoft.Zune.Util
{
	public class TaskbarPlayer : ModelItem, IDisposable
	{
		internal uint m_uTaskbarPlayerStateMsg = 0u;

		internal uint m_uTaskbarPlayerCommandMsg = 0u;

		private TaskbarPlayerCommandHandler m_commandHandler;

		private bool m_fInitialized = false;

		private bool m_fPopupVisible = false;

		private bool m_fToolbarVisible = false;

		private bool m_fInteractivePopupTimer = false;

		private bool m_fHidePopupTimer = false;

		private bool m_fShowToolbar = false;

		private Command m_restoreCommand;

		private unsafe HWND__* m_hWndCommandDispatcher = null;

		private unsafe HWND__* m_hWndTaskbarPlayer = null;

		private unsafe HWND__* m_hWndFrame = null;

		private Point m_popupPosition;

		private Size m_popupSize;

		private WindowState m_restoreState;

		private Point m_restorePosition;

		private Size m_restoreSize;

		private static object sm_lock = new object();

		private static TaskbarPlayer sm_taskbarPlayer = null;

		public Size RestoreSize => m_restoreSize;

		public Point RestorePosition => m_restorePosition;

		public WindowState RestoreState
		{
			get
			{
				return m_restoreState;
			}
			set
			{
				if (m_restoreState != value)
				{
					m_restoreState = value;
					FirePropertyChanged("RestoreState");
				}
			}
		}

		public Size PopupSize => m_popupSize;

		public Point PopupPosition => m_popupPosition;

		public bool PopupVisible
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_fPopupVisible;
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				if (m_fPopupVisible != value)
				{
					m_fPopupVisible = value;
					FirePropertyChanged("PopupVisible");
				}
			}
		}

		public bool ToolbarVisible
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_fToolbarVisible;
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				if (m_fToolbarVisible != value)
				{
					m_fToolbarVisible = value;
					FirePropertyChanged("ToolbarVisible");
				}
			}
		}

		public unsafe bool EnableToolbar
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_0026: Expected I, but got I8
				//IL_004b: Expected I, but got I8
				//IL_004b: Expected I, but got I8
				bool result = false;
				if (System.Runtime.CompilerServices.Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, long>(ref _003CModule_003E.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand) == 0)
				{
					_003CModule_003E.CoCreateInstance((_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.CLSID_TrayDeskBand), null, 4u, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.IID_ITrayDeskBand), (void**)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand));
				}
				if (System.Runtime.CompilerServices.Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, long>(ref _003CModule_003E.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand) != 0L)
				{
					long microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand = System.Runtime.CompilerServices.Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, long>(ref _003CModule_003E.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand);
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int>)(*(ulong*)(*(long*)System.Runtime.CompilerServices.Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, ulong>(ref _003CModule_003E.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand) + 40)))((nint)microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._GUID_f2d3efa4_12f4_466b_a41c_d9ec613ad509)) == 0;
				}
				return result;
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				//IL_0024: Expected I, but got I8
				//IL_0044: Expected I, but got I8
				//IL_0044: Expected I, but got I8
				//IL_0064: Expected I, but got I8
				//IL_0064: Expected I, but got I8
				//IL_0083: Expected I, but got I8
				//IL_0083: Expected I, but got I8
				//IL_009b: Expected I, but got I8
				//IL_009b: Expected I, but got I8
				if (System.Runtime.CompilerServices.Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, long>(ref _003CModule_003E.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand) == 0)
				{
					_003CModule_003E.CoCreateInstance((_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.CLSID_TrayDeskBand), null, 4u, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.IID_ITrayDeskBand), (void**)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand));
				}
				if (System.Runtime.CompilerServices.Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, long>(ref _003CModule_003E.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand) != 0L)
				{
					long microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand = System.Runtime.CompilerServices.Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, long>(ref _003CModule_003E.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand);
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)System.Runtime.CompilerServices.Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, ulong>(ref _003CModule_003E.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand) + 48)))((nint)microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand);
					if (value)
					{
						long microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand2 = System.Runtime.CompilerServices.Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, long>(ref _003CModule_003E.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand);
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int>)(*(ulong*)(*(long*)System.Runtime.CompilerServices.Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, ulong>(ref _003CModule_003E.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand) + 24)))((nint)microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand2, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._GUID_f2d3efa4_12f4_466b_a41c_d9ec613ad509));
					}
					else
					{
						long microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand3 = System.Runtime.CompilerServices.Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, long>(ref _003CModule_003E.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand);
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int>)(*(ulong*)(*(long*)System.Runtime.CompilerServices.Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, ulong>(ref _003CModule_003E.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand) + 32)))((nint)microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand3, (_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._GUID_f2d3efa4_12f4_466b_a41c_d9ec613ad509));
					}
					long microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand4 = System.Runtime.CompilerServices.Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, long>(ref _003CModule_003E.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand);
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)System.Runtime.CompilerServices.Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, ulong>(ref _003CModule_003E.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand) + 48)))((nint)microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand4);
				}
			}
		}

		public unsafe bool ShowToolbar
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_fShowToolbar;
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				//IL_0044: Expected I4, but got I8
				//IL_0056: Expected I4, but got I8
				if (m_fShowToolbar != value)
				{
					m_fShowToolbar = value;
					FirePropertyChanged("ShowToolbar");
					if (value)
					{
						HMONITOR__* ptr = _003CModule_003E.MonitorFromWindow(m_hWndFrame, 2u);
						if (ptr != null)
						{
							tagMONITORINFO tagMONITORINFO;
							// IL initblk instruction
							System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagMONITORINFO, 4), 0, 36);
							*(int*)(&tagMONITORINFO) = 40;
							tagWINDOWPLACEMENT tagWINDOWPLACEMENT;
							// IL initblk instruction
							System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagWINDOWPLACEMENT, 4), 0, 40);
							*(int*)(&tagWINDOWPLACEMENT) = 44;
							if (_003CModule_003E.GetMonitorInfoW(ptr, &tagMONITORINFO) != 0 && _003CModule_003E.GetWindowPlacement(m_hWndFrame, &tagWINDOWPLACEMENT) != 0)
							{
								WindowState windowState2 = (RestoreState = System.Runtime.CompilerServices.Unsafe.As<tagWINDOWPLACEMENT, WindowState>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagWINDOWPLACEMENT, 4)) & WindowState.Maximized);
								_003CModule_003E.OffsetRect((tagRECT*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagWINDOWPLACEMENT, 28)), System.Runtime.CompilerServices.Unsafe.As<tagMONITORINFO, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagMONITORINFO, 20)), System.Runtime.CompilerServices.Unsafe.As<tagMONITORINFO, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagMONITORINFO, 24)));
								tagRECT tagRECT;
								if (_003CModule_003E.IntersectRect(&tagRECT, (tagRECT*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagWINDOWPLACEMENT, 28)), (tagRECT*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagMONITORINFO, 20))) == 0)
								{
									_003CModule_003E.CopyRect((tagRECT*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagWINDOWPLACEMENT, 28)), (tagRECT*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagMONITORINFO, 20)));
									_003CModule_003E.InflateRect((tagRECT*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagWINDOWPLACEMENT, 28)), -100, -100);
								}
								RestorePosition.X = System.Runtime.CompilerServices.Unsafe.As<tagWINDOWPLACEMENT, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagWINDOWPLACEMENT, 28));
								RestorePosition.Y = System.Runtime.CompilerServices.Unsafe.As<tagWINDOWPLACEMENT, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagWINDOWPLACEMENT, 32));
								RestoreSize.Width = System.Runtime.CompilerServices.Unsafe.As<tagWINDOWPLACEMENT, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagWINDOWPLACEMENT, 36)) - System.Runtime.CompilerServices.Unsafe.As<tagWINDOWPLACEMENT, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagWINDOWPLACEMENT, 28));
								RestoreSize.Height = System.Runtime.CompilerServices.Unsafe.As<tagWINDOWPLACEMENT, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagWINDOWPLACEMENT, 40)) - System.Runtime.CompilerServices.Unsafe.As<tagWINDOWPLACEMENT, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagWINDOWPLACEMENT, 32));
							}
						}
					}
				}
				DispatchCommand(ETaskbarPlayerCommand.PC_Connect, 0);
				if (value)
				{
					HWND__* hWndTaskbarPlayer = m_hWndTaskbarPlayer;
					if (hWndTaskbarPlayer == null || _003CModule_003E.IsWindow(hWndTaskbarPlayer) == 0)
					{
						return;
					}
				}
				ToolbarVisible = value;
			}
		}

		public Command Restore => m_restoreCommand;

		public static TaskbarPlayer Instance
		{
			get
			{
				if (sm_taskbarPlayer == null)
				{
					sm_taskbarPlayer = new TaskbarPlayer();
				}
				return sm_taskbarPlayer;
			}
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool Initialize(IntPtr hWndFrame, TaskbarPlayerCommandHandler commandHandler)
		{
			//IL_0059: Expected I, but got I8
			//IL_0067: Expected I4, but got I8
			//IL_0088: Expected I8, but got I
			//IL_008f: Expected I8, but got I
			//IL_00a1: Expected I, but got I8
			//IL_00a1: Expected I, but got I8
			//IL_00a2: Expected I8, but got I
			//IL_00e1: Expected I, but got I8
			//IL_00e1: Expected I, but got I8
			//IL_00e1: Expected I, but got I8
			if (!m_fInitialized)
			{
				m_hWndFrame = (HWND__*)hWndFrame.ToPointer();
				m_commandHandler = commandHandler;
				_003CModule_003E.gcroot_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003ATaskbarPlayer_0020_005E_003E_002E_003D((gcroot_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003ATaskbarPlayer_0020_005E_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.Microsoft_002EZune_002EUtil_002Esm_gcTaskbarPlayer), this);
				m_uTaskbarPlayerStateMsg = _003CModule_003E.RegisterWindowMessageW((ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1DE_0040LJLIMGOK_0040_003F_0024AAZ_003F_0024AAu_003F_0024AAn_003F_0024AAe_003F_0024AAT_003F_0024AAa_003F_0024AAs_003F_0024AAk_003F_0024AAb_003F_0024AAa_003F_0024AAr_003F_0024AAP_003F_0024AAl_003F_0024AAa_003F_0024AAy_003F_0024AAe_003F_0024AAr_003F_0024AAS_003F_0024AAt_003F_0024AAa_003F_0024AAt_003F_0024AAe_003F_0024AAM_003F_0024AAs_003F_0024AAg_003F_0024AA_003F_0024AA_0040));
				m_uTaskbarPlayerCommandMsg = _003CModule_003E.RegisterWindowMessageW((ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1DI_0040BOCHIFKJ_0040_003F_0024AAZ_003F_0024AAu_003F_0024AAn_003F_0024AAe_003F_0024AAT_003F_0024AAa_003F_0024AAs_003F_0024AAk_003F_0024AAb_003F_0024AAa_003F_0024AAr_003F_0024AAP_003F_0024AAl_003F_0024AAa_003F_0024AAy_003F_0024AAe_003F_0024AAr_003F_0024AAC_003F_0024AAo_003F_0024AAm_003F_0024AAm_003F_0024AAa_003F_0024AAn_003F_0024AAd_003F_0024AAM_003F_0024AAs_003F_0024AAg_003F_0024AA_003F_0024AA_0040));
				HINSTANCE__* ptr = (HINSTANCE__*)_003CModule_003E.GetWindowLongPtrW(m_hWndFrame, -6);
				tagWNDCLASSW tagWNDCLASSW;
				*(int*)(&tagWNDCLASSW) = 0;
				// IL initblk instruction
				System.Runtime.CompilerServices.Unsafe.InitBlock(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagWNDCLASSW, 8), 0, 64);
				if (_003CModule_003E.GetClassInfoW(ptr, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1EG_0040LILHHEEP_0040_003F_0024AAZ_003F_0024AAu_003F_0024AAn_003F_0024AAe_003F_0024AAT_003F_0024AAa_003F_0024AAs_003F_0024AAk_003F_0024AAb_003F_0024AAa_003F_0024AAr_003F_0024AAP_003F_0024AAl_003F_0024AAa_003F_0024AAy_003F_0024AAe_003F_0024AAr_003F_0024AAC_003F_0024AAo_003F_0024AAm_003F_0024AAm_003F_0024AAa_003F_0024AAn_003F_0024AAd_003F_0024AAD_003F_0024AAi_003F_0024AAs_003F_0024AAp_003F_0024AAa_003F_0024AAt_003F_0024AAc_003F_0024AAh_0040), &tagWNDCLASSW) == 0)
				{
					*(int*)(&tagWNDCLASSW) = 512;
					System.Runtime.CompilerServices.Unsafe.As<tagWNDCLASSW, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagWNDCLASSW, 8)) = (nint)_003CModule_003E.__unep_0040_003FCommandDispatcherWindowProc_0040Util_0040Zune_0040Microsoft_0040_0040_0024_0024FYA_JPEAUHWND___0040_0040I_K_J_0040Z;
					System.Runtime.CompilerServices.Unsafe.As<tagWNDCLASSW, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagWNDCLASSW, 24)) = (nint)ptr;
					System.Runtime.CompilerServices.Unsafe.As<tagWNDCLASSW, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagWNDCLASSW, 40)) = (nint)_003CModule_003E.LoadCursorW(null, (ushort*)32514);
					System.Runtime.CompilerServices.Unsafe.As<tagWNDCLASSW, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagWNDCLASSW, 48)) = 1L;
					System.Runtime.CompilerServices.Unsafe.As<tagWNDCLASSW, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagWNDCLASSW, 64)) = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1EG_0040LILHHEEP_0040_003F_0024AAZ_003F_0024AAu_003F_0024AAn_003F_0024AAe_003F_0024AAT_003F_0024AAa_003F_0024AAs_003F_0024AAk_003F_0024AAb_003F_0024AAa_003F_0024AAr_003F_0024AAP_003F_0024AAl_003F_0024AAa_003F_0024AAy_003F_0024AAe_003F_0024AAr_003F_0024AAC_003F_0024AAo_003F_0024AAm_003F_0024AAm_003F_0024AAa_003F_0024AAn_003F_0024AAd_003F_0024AAD_003F_0024AAi_003F_0024AAs_003F_0024AAp_003F_0024AAa_003F_0024AAt_003F_0024AAc_003F_0024AAh_0040);
					_003CModule_003E.RegisterClassW(&tagWNDCLASSW);
				}
				if (_003CModule_003E.CreateWindowExW(128u, (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1EG_0040LILHHEEP_0040_003F_0024AAZ_003F_0024AAu_003F_0024AAn_003F_0024AAe_003F_0024AAT_003F_0024AAa_003F_0024AAs_003F_0024AAk_003F_0024AAb_003F_0024AAa_003F_0024AAr_003F_0024AAP_003F_0024AAl_003F_0024AAa_003F_0024AAy_003F_0024AAe_003F_0024AAr_003F_0024AAC_003F_0024AAo_003F_0024AAm_003F_0024AAm_003F_0024AAa_003F_0024AAn_003F_0024AAd_003F_0024AAD_003F_0024AAi_003F_0024AAs_003F_0024AAp_003F_0024AAa_003F_0024AAt_003F_0024AAc_003F_0024AAh_0040), (ushort*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E._003F_003F_C_0040_1EG_0040LILHHEEP_0040_003F_0024AAZ_003F_0024AAu_003F_0024AAn_003F_0024AAe_003F_0024AAT_003F_0024AAa_003F_0024AAs_003F_0024AAk_003F_0024AAb_003F_0024AAa_003F_0024AAr_003F_0024AAP_003F_0024AAl_003F_0024AAa_003F_0024AAy_003F_0024AAe_003F_0024AAr_003F_0024AAC_003F_0024AAo_003F_0024AAm_003F_0024AAm_003F_0024AAa_003F_0024AAn_003F_0024AAd_003F_0024AAD_003F_0024AAi_003F_0024AAs_003F_0024AAp_003F_0024AAa_003F_0024AAt_003F_0024AAc_003F_0024AAh_0040), 2147483648u, 0, 0, 0, 0, null, null, ptr, null) != null)
				{
					m_fInitialized = true;
				}
			}
			return m_fInitialized;
		}

		public unsafe void UpdateToolbar(ETaskbarPlayerState state)
		{
			HWND__* hWndTaskbarPlayer = m_hWndTaskbarPlayer;
			if (hWndTaskbarPlayer != null && _003CModule_003E.IsWindow(hWndTaskbarPlayer) != 0)
			{
				if (ShowToolbar)
				{
					state |= ETaskbarPlayerState.PS_Minimized;
				}
				_003CModule_003E.PostMessageW(m_hWndTaskbarPlayer, m_uTaskbarPlayerStateMsg, (ulong)state, 0L);
			}
		}

		internal unsafe void OnCreate(HWND__* hWnd)
		{
			m_hWndCommandDispatcher = hWnd;
		}

		internal unsafe void OnDestory(HWND__* hWnd)
		{
			//IL_0024: Expected I, but got I8
			StopTimer(hWnd, 1uL, ref m_fInteractivePopupTimer);
			StopTimer(hWnd, 2uL, ref m_fHidePopupTimer);
			m_hWndCommandDispatcher = null;
		}

		internal unsafe void OnTimer(HWND__* hWnd, uint dwTimerId)
		{
			switch (dwTimerId)
			{
			case 2u:
				if (MouseInWindow(m_hWndFrame) == 0 && MouseInWindow(m_hWndTaskbarPlayer) == 0)
				{
					DispatchCommand(ETaskbarPlayerCommand.PC_Popup, 0);
					StopTimer(hWnd, 1uL, ref m_fInteractivePopupTimer);
				}
				StopTimer(hWnd, 2uL, ref m_fHidePopupTimer);
				break;
			case 1u:
				if (MouseInWindow(m_hWndFrame) == 0 && MouseInWindow(m_hWndTaskbarPlayer) == 0)
				{
					StartTimer(hWnd, 2uL, 500u, ref m_fHidePopupTimer);
				}
				else
				{
					StopTimer(hWnd, 2uL, ref m_fHidePopupTimer);
				}
				break;
			}
		}

		internal unsafe void OnCommandMsg(ETaskbarPlayerCommand command, long param)
		{
			//IL_000d: Expected I, but got I8
			if (command == ETaskbarPlayerCommand.PC_Connect && param != 0L)
			{
				m_hWndTaskbarPlayer = (HWND__*)param;
				if (ShowToolbar)
				{
					ToolbarVisible = true;
				}
			}
			DispatchCommand(command, (int)param);
		}

		private unsafe TaskbarPlayer()
		{
			//IL_0040: Expected I, but got I8
			//IL_0048: Expected I, but got I8
			//IL_0050: Expected I, but got I8
			try
			{
				m_restoreCommand = new Command();
				m_popupPosition = new Point();
				m_popupSize = new Size();
				m_restoreState = WindowState.Maximized;
				m_restorePosition = new Point();
				m_restoreSize = new Size();
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		private void _007ETaskbarPlayer()
		{
			_0021TaskbarPlayer();
		}

		private unsafe void _0021TaskbarPlayer()
		{
			HWND__* hWndCommandDispatcher = m_hWndCommandDispatcher;
			if (hWndCommandDispatcher != null && _003CModule_003E.IsWindow(hWndCommandDispatcher) != 0)
			{
				_003CModule_003E.PostMessageW(m_hWndCommandDispatcher, 16u, 0uL, 0L);
			}
			if (System.Runtime.CompilerServices.Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, long>(ref _003CModule_003E.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand) != 0L)
			{
				_003CModule_003E.CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E_002ERelease((CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref _003CModule_003E.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand));
			}
		}

		private unsafe void DisplayPopup(int x, int y)
		{
			//IL_0030: Expected I4, but got I8
			if (PopupVisible)
			{
				return;
			}
			tagPOINT tagPOINT;
			*(int*)(&tagPOINT) = x;
			System.Runtime.CompilerServices.Unsafe.As<tagPOINT, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagPOINT, 4)) = y;
			HMONITOR__* ptr = _003CModule_003E.MonitorFromPoint(tagPOINT, 2u);
			if (ptr != null)
			{
				tagMONITORINFO tagMONITORINFO;
				// IL initblk instruction
				System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagMONITORINFO, 4), 0, 36);
				*(int*)(&tagMONITORINFO) = 40;
				if (_003CModule_003E.GetMonitorInfoW(ptr, &tagMONITORINFO) != 0)
				{
					if (x < System.Runtime.CompilerServices.Unsafe.As<tagMONITORINFO, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagMONITORINFO, 20)))
					{
						PopupPosition.X = System.Runtime.CompilerServices.Unsafe.As<tagMONITORINFO, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagMONITORINFO, 20));
					}
					else if (x > System.Runtime.CompilerServices.Unsafe.As<tagMONITORINFO, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagMONITORINFO, 28)) - PopupSize.Width)
					{
						PopupPosition.X = System.Runtime.CompilerServices.Unsafe.As<tagMONITORINFO, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagMONITORINFO, 28)) - PopupSize.Width;
					}
					else
					{
						PopupPosition.X = x;
					}
					if (y < System.Runtime.CompilerServices.Unsafe.As<tagMONITORINFO, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagMONITORINFO, 24)))
					{
						PopupPosition.Y = System.Runtime.CompilerServices.Unsafe.As<tagMONITORINFO, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagMONITORINFO, 24));
					}
					else if (y > System.Runtime.CompilerServices.Unsafe.As<tagMONITORINFO, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagMONITORINFO, 32)) - PopupSize.Height)
					{
						PopupPosition.Y = System.Runtime.CompilerServices.Unsafe.As<tagMONITORINFO, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagMONITORINFO, 32)) - PopupSize.Height;
					}
					else
					{
						PopupPosition.Y = y;
					}
				}
			}
			PopupVisible = true;
			StartTimer(m_hWndCommandDispatcher, 1uL, 250u, ref m_fInteractivePopupTimer);
		}

		private void DispatchCommand(ETaskbarPlayerCommand command, int value)
		{
			if (m_commandHandler == null)
			{
				return;
			}
			switch (command)
			{
			case ETaskbarPlayerCommand.PC_Restore:
				if (ToolbarVisible)
				{
					ShowToolbar = false;
					PopupVisible = false;
					GetKeyboardFocus();
					Restore.Invoke();
				}
				break;
			case ETaskbarPlayerCommand.PC_Popup:
				if (value != 0)
				{
					DisplayPopup((ushort)((ulong)value >> 16), (ushort)value);
				}
				else
				{
					PopupVisible = false;
				}
				break;
			}
			m_commandHandler(command, value);
		}

		private unsafe void GetKeyboardFocus()
		{
			//IL_0029: Expected I4, but got I8
			HWND__* hWndFrame = m_hWndFrame;
			if (hWndFrame != null && _003CModule_003E.IsWindow(hWndFrame) != 0 && _003CModule_003E.GetForegroundWindow() != m_hWndFrame)
			{
				tagINPUT tagINPUT;
				// IL initblk instruction
				System.Runtime.CompilerServices.Unsafe.InitBlock(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagINPUT, 8), 0, 32);
				*(int*)(&tagINPUT) = 1;
				System.Runtime.CompilerServices.Unsafe.As<tagINPUT, short>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagINPUT, 8)) = 0;
				_003CModule_003E.SendInput(1u, &tagINPUT, 40);
				_003CModule_003E.SetForegroundWindow(m_hWndFrame);
			}
		}

		private unsafe static int MouseInWindow(HWND__* hWnd)
		{
			int result = 0;
			if (hWnd != null && _003CModule_003E.IsWindow(hWnd) != 0)
			{
				tagPOINT tagPOINT;
				_003CModule_003E.GetCursorPos(&tagPOINT);
				_003CModule_003E.ScreenToClient(hWnd, &tagPOINT);
				tagRECT tagRECT;
				_003CModule_003E.GetClientRect(hWnd, &tagRECT);
				result = _003CModule_003E.PtInRect(&tagRECT, tagPOINT);
			}
			return result;
		}

		private unsafe static void StartTimer(HWND__* hWnd, ulong uTimerId, uint msDuration, ref bool fTimer)
		{
			//IL_000e: Expected I, but got I8
			if (!fTimer)
			{
				_003CModule_003E.SetTimer(hWnd, uTimerId, msDuration, null);
				fTimer = true;
			}
		}

		private unsafe static void StopTimer(HWND__* hWnd, ulong uTimerId, ref bool fTimer)
		{
			if (fTimer)
			{
				_003CModule_003E.KillTimer(hWnd, uTimerId);
				fTimer = false;
			}
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
					_007ETaskbarPlayer();
				}
				finally
				{
					base.Dispose();
				}
			}
			else
			{
				try
				{
					_0021TaskbarPlayer();
				}
				finally
				{
					base.Finalize();
				}
			}
		}

		public new void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~TaskbarPlayer()
		{
			Dispose(false);
		}
	}
}
