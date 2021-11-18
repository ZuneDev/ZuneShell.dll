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

		private unsafe HWND* m_hWndCommandDispatcher = null;

		private unsafe HWND* m_hWndTaskbarPlayer = null;

		private unsafe HWND* m_hWndFrame = null;

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
				if (Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, long>(ref Module.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand) == 0)
				{
					Module.CoCreateInstance((_GUID*)Unsafe.AsPointer(ref Module.CLSID_TrayDeskBand), null, 4u, (_GUID*)Unsafe.AsPointer(ref Module.IID_ITrayDeskBand), (void**)Unsafe.AsPointer(ref Module.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand));
				}
				if (Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, long>(ref Module.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand) != 0L)
				{
					long microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand = Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, long>(ref Module.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand);
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int>)(*(ulong*)(*(long*)Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, ulong>(ref Module.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand) + 40)))((nint)microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand, (_GUID*)Unsafe.AsPointer(ref Module._GUID_f2d3efa4_12f4_466b_a41c_d9ec613ad509)) == 0;
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
				if (Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, long>(ref Module.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand) == 0)
				{
					Module.CoCreateInstance((_GUID*)Unsafe.AsPointer(ref Module.CLSID_TrayDeskBand), null, 4u, (_GUID*)Unsafe.AsPointer(ref Module.IID_ITrayDeskBand), (void**)Unsafe.AsPointer(ref Module.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand));
				}
				if (Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, long>(ref Module.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand) != 0L)
				{
					long microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand = Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, long>(ref Module.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand);
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, ulong>(ref Module.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand) + 48)))((nint)microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand);
					if (value)
					{
						long microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand2 = Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, long>(ref Module.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand);
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int>)(*(ulong*)(*(long*)Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, ulong>(ref Module.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand) + 24)))((nint)microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand2, (_GUID*)Unsafe.AsPointer(ref Module._GUID_f2d3efa4_12f4_466b_a41c_d9ec613ad509));
					}
					else
					{
						long microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand3 = Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, long>(ref Module.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand);
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int>)(*(ulong*)(*(long*)Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, ulong>(ref Module.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand) + 32)))((nint)microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand3, (_GUID*)Unsafe.AsPointer(ref Module._GUID_f2d3efa4_12f4_466b_a41c_d9ec613ad509));
					}
					long microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand4 = Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, long>(ref Module.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand);
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, ulong>(ref Module.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand) + 48)))((nint)microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand4);
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
						HMONITOR* ptr = Module.MonitorFromWindow(m_hWndFrame, 2u);
						if (ptr != null)
						{
							MONITORINFO tagMONITORINFO;
                            // IL initblk instruction
                            Unsafe.InitBlockUnaligned(ref Module.AddByteOffset(ref tagMONITORINFO, 4), 0, 36);
							*(int*)(&tagMONITORINFO) = 40;
							WINDOWPLACEMENT tagWINDOWPLACEMENT;
                            // IL initblk instruction
                            Unsafe.InitBlockUnaligned(ref Module.AddByteOffset(ref tagWINDOWPLACEMENT, 4), 0, 40);
							*(int*)(&tagWINDOWPLACEMENT) = 44;
							if (Module.GetMonitorInfoW(ptr, &tagMONITORINFO) != 0 && Module.GetWindowPlacement(m_hWndFrame, &tagWINDOWPLACEMENT) != 0)
							{
								WindowState windowState2 = (RestoreState = Unsafe.As<WINDOWPLACEMENT, WindowState>(ref Module.AddByteOffset(ref tagWINDOWPLACEMENT, 4)) & WindowState.Maximized);
								Module.OffsetRect((RECT*)Unsafe.AsPointer(ref Module.AddByteOffset(ref tagWINDOWPLACEMENT, 28)), Unsafe.As<MONITORINFO, int>(ref Module.AddByteOffset(ref tagMONITORINFO, 20)), Unsafe.As<MONITORINFO, int>(ref Module.AddByteOffset(ref tagMONITORINFO, 24)));
								RECT tagRECT;
								if (Module.IntersectRect(&tagRECT, (RECT*)Unsafe.AsPointer(ref Module.AddByteOffset(ref tagWINDOWPLACEMENT, 28)), (RECT*)Unsafe.AsPointer(ref Module.AddByteOffset(ref tagMONITORINFO, 20))) == 0)
								{
									Module.CopyRect((RECT*)Unsafe.AsPointer(ref Module.AddByteOffset(ref tagWINDOWPLACEMENT, 28)), (RECT*)Unsafe.AsPointer(ref Module.AddByteOffset(ref tagMONITORINFO, 20)));
									Module.InflateRect((RECT*)Unsafe.AsPointer(ref Module.AddByteOffset(ref tagWINDOWPLACEMENT, 28)), -100, -100);
								}
								RestorePosition.X = Unsafe.As<WINDOWPLACEMENT, int>(ref Module.AddByteOffset(ref tagWINDOWPLACEMENT, 28));
								RestorePosition.Y = Unsafe.As<WINDOWPLACEMENT, int>(ref Module.AddByteOffset(ref tagWINDOWPLACEMENT, 32));
								RestoreSize.Width = Unsafe.As<WINDOWPLACEMENT, int>(ref Module.AddByteOffset(ref tagWINDOWPLACEMENT, 36)) - Unsafe.As<WINDOWPLACEMENT, int>(ref Module.AddByteOffset(ref tagWINDOWPLACEMENT, 28));
								RestoreSize.Height = Unsafe.As<WINDOWPLACEMENT, int>(ref Module.AddByteOffset(ref tagWINDOWPLACEMENT, 40)) - Unsafe.As<WINDOWPLACEMENT, int>(ref Module.AddByteOffset(ref tagWINDOWPLACEMENT, 32));
							}
						}
					}
				}
				DispatchCommand(ETaskbarPlayerCommand.PC_Connect, 0);
				if (value)
				{
					HWND* hWndTaskbarPlayer = m_hWndTaskbarPlayer;
					if (hWndTaskbarPlayer == null || Module.IsWindow(hWndTaskbarPlayer) == 0)
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
				m_hWndFrame = (HWND*)hWndFrame.ToPointer();
				m_commandHandler = commandHandler;
				Module.gcroot_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003ATaskbarPlayer_0020_005E_003E_002E_003D((gcroot_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003ATaskbarPlayer_0020_005E_003E*)Unsafe.AsPointer(ref Module.Microsoft_002EZune_002EUtil_002Esm_gcTaskbarPlayer), this);
				m_uTaskbarPlayerStateMsg = Module.RegisterWindowMessageW((ushort*)Unsafe.AsPointer(ref Module._003F_003F_C_0040_1DE_0040LJLIMGOK_0040_003F_0024AAZ_003F_0024AAu_003F_0024AAn_003F_0024AAe_003F_0024AAT_003F_0024AAa_003F_0024AAs_003F_0024AAk_003F_0024AAb_003F_0024AAa_003F_0024AAr_003F_0024AAP_003F_0024AAl_003F_0024AAa_003F_0024AAy_003F_0024AAe_003F_0024AAr_003F_0024AAS_003F_0024AAt_003F_0024AAa_003F_0024AAt_003F_0024AAe_003F_0024AAM_003F_0024AAs_003F_0024AAg_003F_0024AA_003F_0024AA_0040));
				m_uTaskbarPlayerCommandMsg = Module.RegisterWindowMessageW((ushort*)Unsafe.AsPointer(ref Module._003F_003F_C_0040_1DI_0040BOCHIFKJ_0040_003F_0024AAZ_003F_0024AAu_003F_0024AAn_003F_0024AAe_003F_0024AAT_003F_0024AAa_003F_0024AAs_003F_0024AAk_003F_0024AAb_003F_0024AAa_003F_0024AAr_003F_0024AAP_003F_0024AAl_003F_0024AAa_003F_0024AAy_003F_0024AAe_003F_0024AAr_003F_0024AAC_003F_0024AAo_003F_0024AAm_003F_0024AAm_003F_0024AAa_003F_0024AAn_003F_0024AAd_003F_0024AAM_003F_0024AAs_003F_0024AAg_003F_0024AA_003F_0024AA_0040));
				HINSTANCE* ptr = (HINSTANCE*)Module.GetWindowLongPtrW(m_hWndFrame, -6);
				WNDCLASS tagWNDCLASSW = default;
                // IL initblk instruction
                Unsafe.InitBlock(ref Module.AddByteOffset(ref tagWNDCLASSW, 8), 0, 64);
				if (Module.GetClassInfoW(ptr, (ushort*)Unsafe.AsPointer(ref Module._003F_003F_C_0040_1EG_0040LILHHEEP_0040_003F_0024AAZ_003F_0024AAu_003F_0024AAn_003F_0024AAe_003F_0024AAT_003F_0024AAa_003F_0024AAs_003F_0024AAk_003F_0024AAb_003F_0024AAa_003F_0024AAr_003F_0024AAP_003F_0024AAl_003F_0024AAa_003F_0024AAy_003F_0024AAe_003F_0024AAr_003F_0024AAC_003F_0024AAo_003F_0024AAm_003F_0024AAm_003F_0024AAa_003F_0024AAn_003F_0024AAd_003F_0024AAD_003F_0024AAi_003F_0024AAs_003F_0024AAp_003F_0024AAa_003F_0024AAt_003F_0024AAc_003F_0024AAh_0040), ref tagWNDCLASSW) == 0)
				{
					*(int*)(&tagWNDCLASSW) = 512;
                    Unsafe.As<WNDCLASS, long>(ref Module.AddByteOffset(ref tagWNDCLASSW, 8)) = (nint)Module.__unep_0040_003FCommandDispatcherWindowProc_0040Util_0040Zune_0040Microsoft_0040_0040_0024_0024FYA_JPEAUHWND___0040_0040I_K_J_0040Z;
                    Unsafe.As<WNDCLASS, long>(ref Module.AddByteOffset(ref tagWNDCLASSW, 24)) = (nint)ptr;
                    Unsafe.As<WNDCLASS, long>(ref Module.AddByteOffset(ref tagWNDCLASSW, 40)) = (nint)Module.LoadCursorW(null, (ushort*)32514);
                    Unsafe.As<WNDCLASS, long>(ref Module.AddByteOffset(ref tagWNDCLASSW, 48)) = 1L;
                    Unsafe.As<WNDCLASS, long>(ref Module.AddByteOffset(ref tagWNDCLASSW, 64)) = (nint)Unsafe.AsPointer(ref Module._003F_003F_C_0040_1EG_0040LILHHEEP_0040_003F_0024AAZ_003F_0024AAu_003F_0024AAn_003F_0024AAe_003F_0024AAT_003F_0024AAa_003F_0024AAs_003F_0024AAk_003F_0024AAb_003F_0024AAa_003F_0024AAr_003F_0024AAP_003F_0024AAl_003F_0024AAa_003F_0024AAy_003F_0024AAe_003F_0024AAr_003F_0024AAC_003F_0024AAo_003F_0024AAm_003F_0024AAm_003F_0024AAa_003F_0024AAn_003F_0024AAd_003F_0024AAD_003F_0024AAi_003F_0024AAs_003F_0024AAp_003F_0024AAa_003F_0024AAt_003F_0024AAc_003F_0024AAh_0040);
					Module.RegisterClassW(&tagWNDCLASSW);
				}
				if (Module.CreateWindowExW(128u, (ushort*)Unsafe.AsPointer(ref Module._003F_003F_C_0040_1EG_0040LILHHEEP_0040_003F_0024AAZ_003F_0024AAu_003F_0024AAn_003F_0024AAe_003F_0024AAT_003F_0024AAa_003F_0024AAs_003F_0024AAk_003F_0024AAb_003F_0024AAa_003F_0024AAr_003F_0024AAP_003F_0024AAl_003F_0024AAa_003F_0024AAy_003F_0024AAe_003F_0024AAr_003F_0024AAC_003F_0024AAo_003F_0024AAm_003F_0024AAm_003F_0024AAa_003F_0024AAn_003F_0024AAd_003F_0024AAD_003F_0024AAi_003F_0024AAs_003F_0024AAp_003F_0024AAa_003F_0024AAt_003F_0024AAc_003F_0024AAh_0040), (ushort*)Unsafe.AsPointer(ref Module._003F_003F_C_0040_1EG_0040LILHHEEP_0040_003F_0024AAZ_003F_0024AAu_003F_0024AAn_003F_0024AAe_003F_0024AAT_003F_0024AAa_003F_0024AAs_003F_0024AAk_003F_0024AAb_003F_0024AAa_003F_0024AAr_003F_0024AAP_003F_0024AAl_003F_0024AAa_003F_0024AAy_003F_0024AAe_003F_0024AAr_003F_0024AAC_003F_0024AAo_003F_0024AAm_003F_0024AAm_003F_0024AAa_003F_0024AAn_003F_0024AAd_003F_0024AAD_003F_0024AAi_003F_0024AAs_003F_0024AAp_003F_0024AAa_003F_0024AAt_003F_0024AAc_003F_0024AAh_0040), 2147483648u, 0, 0, 0, 0, null, null, ptr, null) != null)
				{
					m_fInitialized = true;
				}
			}
			return m_fInitialized;
		}

		public unsafe void UpdateToolbar(ETaskbarPlayerState state)
		{
			HWND* hWndTaskbarPlayer = m_hWndTaskbarPlayer;
			if (hWndTaskbarPlayer != null && Module.IsWindow(hWndTaskbarPlayer) != 0)
			{
				if (ShowToolbar)
				{
					state |= ETaskbarPlayerState.PS_Minimized;
				}
				Module.PostMessageW(m_hWndTaskbarPlayer, m_uTaskbarPlayerStateMsg, (ulong)state, 0L);
			}
		}

		internal unsafe void OnCreate(HWND* hWnd)
		{
			m_hWndCommandDispatcher = hWnd;
		}

		internal unsafe void OnDestory(HWND* hWnd)
		{
			//IL_0024: Expected I, but got I8
			StopTimer(hWnd, 1uL, ref m_fInteractivePopupTimer);
			StopTimer(hWnd, 2uL, ref m_fHidePopupTimer);
			m_hWndCommandDispatcher = null;
		}

		internal unsafe void OnTimer(HWND* hWnd, uint dwTimerId)
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
				m_hWndTaskbarPlayer = (HWND*)param;
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
			HWND* hWndCommandDispatcher = m_hWndCommandDispatcher;
			if (hWndCommandDispatcher != null && Module.IsWindow(hWndCommandDispatcher) != 0)
			{
				Module.PostMessageW(m_hWndCommandDispatcher, 16u, 0uL, 0L);
			}
			if (Unsafe.As<CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E, long>(ref Module.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand) != 0L)
			{
				Module.CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E_002ERelease((CComPtrNtv_003CMicrosoft_003A_003AZune_003A_003AUtil_003A_003AITrayDeskBand_003E*)Unsafe.AsPointer(ref Module.Microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand));
			}
		}

		private unsafe void DisplayPopup(int x, int y)
		{
			//IL_0030: Expected I4, but got I8
			if (PopupVisible)
			{
				return;
			}
			POINTS tagPOINT;
			*(int*)(&tagPOINT) = x;
            Unsafe.As<POINTS, int>(ref Module.AddByteOffset(ref tagPOINT, 4)) = y;
			HMONITOR* ptr = Module.MonitorFromPoint(tagPOINT, 2u);
			if (ptr != null)
			{
				MONITORINFO tagMONITORINFO;
                // IL initblk instruction
                Unsafe.InitBlockUnaligned(ref Module.AddByteOffset(ref tagMONITORINFO, 4), 0, 36);
				*(int*)(&tagMONITORINFO) = 40;
				if (Module.GetMonitorInfoW(ptr, &tagMONITORINFO) != 0)
				{
					if (x < Unsafe.As<MONITORINFO, int>(ref Module.AddByteOffset(ref tagMONITORINFO, 20)))
					{
						PopupPosition.X = Unsafe.As<MONITORINFO, int>(ref Module.AddByteOffset(ref tagMONITORINFO, 20));
					}
					else if (x > Unsafe.As<MONITORINFO, int>(ref Module.AddByteOffset(ref tagMONITORINFO, 28)) - PopupSize.Width)
					{
						PopupPosition.X = Unsafe.As<MONITORINFO, int>(ref Module.AddByteOffset(ref tagMONITORINFO, 28)) - PopupSize.Width;
					}
					else
					{
						PopupPosition.X = x;
					}
					if (y < Unsafe.As<MONITORINFO, int>(ref Module.AddByteOffset(ref tagMONITORINFO, 24)))
					{
						PopupPosition.Y = Unsafe.As<MONITORINFO, int>(ref Module.AddByteOffset(ref tagMONITORINFO, 24));
					}
					else if (y > Unsafe.As<MONITORINFO, int>(ref Module.AddByteOffset(ref tagMONITORINFO, 32)) - PopupSize.Height)
					{
						PopupPosition.Y = Unsafe.As<MONITORINFO, int>(ref Module.AddByteOffset(ref tagMONITORINFO, 32)) - PopupSize.Height;
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
			HWND* hWndFrame = m_hWndFrame;
			if (hWndFrame != null && Module.IsWindow(hWndFrame) != 0 && Module.GetForegroundWindow() != m_hWndFrame)
			{
				INPUT tagINPUT;
                // IL initblk instruction
                Unsafe.InitBlock(ref Module.AddByteOffset(ref tagINPUT, 8), 0, 32);
				*(int*)(&tagINPUT) = 1;
                Unsafe.As<INPUT, short>(ref Module.AddByteOffset(ref tagINPUT, 8)) = 0;
				Module.SendInput(1u, &tagINPUT, 40);
				Module.SetForegroundWindow(m_hWndFrame);
			}
		}

		private unsafe static int MouseInWindow(HWND* hWnd)
		{
			int result = 0;
			if (hWnd != null && Module.IsWindow(hWnd) != 0)
			{
				POINTS tagPOINT;
				Module.GetCursorPos(&tagPOINT);
				Module.ScreenToClient(hWnd, &tagPOINT);
				RECT tagRECT;
				Module.GetClientRect(hWnd, &tagRECT);
				result = Module.PtInRect(&tagRECT, tagPOINT);
			}
			return result;
		}

		private unsafe static void StartTimer(HWND* hWnd, ulong uTimerId, uint msDuration, ref bool fTimer)
		{
			//IL_000e: Expected I, but got I8
			if (!fTimer)
			{
				Module.SetTimer(hWnd, uTimerId, msDuration, null);
				fTimer = true;
			}
		}

		private unsafe static void StopTimer(HWND* hWnd, ulong uTimerId, ref bool fTimer)
		{
			if (fTimer)
			{
				Module.KillTimer(hWnd, uTimerId);
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
