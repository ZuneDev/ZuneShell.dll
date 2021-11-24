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
        private CComPtrNtv<ITrayDeskBand> s_spTrayDeskBand;

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
				if (!s_spTrayDeskBand)
				{
					Module.CoCreateInstance(Module.CLSID_TrayDeskBand, null, 4u, Module.IID_ITrayDeskBand, (void**)Unsafe.AsPointer(ref s_spTrayDeskBand));
				}
				else
				{
					long microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand = Unsafe.As<CComPtrNtv<ITrayDeskBand>, long>(ref s_spTrayDeskBand);
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int>)(*(ulong*)(*(long*)Unsafe.As<CComPtrNtv<ITrayDeskBand>, ulong>(ref s_spTrayDeskBand) + 40)))((nint)microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand, (_GUID*)Unsafe.AsPointer(ref Module.GUID_ITrayDeskband)) == 0;
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
				if (!s_spTrayDeskBand)
				{
					Module.CoCreateInstance(Module.CLSID_TrayDeskBand, null, 4u, Module.IID_ITrayDeskBand, (void**)Unsafe.AsPointer(ref s_spTrayDeskBand));
				}
				else
				{
					long microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand = Unsafe.As<CComPtrNtv<ITrayDeskBand>, long>(ref s_spTrayDeskBand);
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)Unsafe.As<CComPtrNtv<ITrayDeskBand>, ulong>(ref s_spTrayDeskBand) + 48)))((nint)microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand);
					if (value)
					{
						long microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand2 = Unsafe.As<CComPtrNtv<ITrayDeskBand>, long>(ref s_spTrayDeskBand);
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int>)(*(ulong*)(*(long*)Unsafe.As<CComPtrNtv<ITrayDeskBand>, ulong>(ref s_spTrayDeskBand) + 24)))((nint)microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand2, (_GUID*)Unsafe.AsPointer(ref Module.GUID_ITrayDeskband));
					}
					else
					{
						long microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand3 = Unsafe.As<CComPtrNtv<ITrayDeskBand>, long>(ref s_spTrayDeskBand);
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int>)(*(ulong*)(*(long*)Unsafe.As<CComPtrNtv<ITrayDeskBand>, ulong>(ref s_spTrayDeskBand) + 32)))((nint)microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand3, (_GUID*)Unsafe.AsPointer(ref Module.GUID_ITrayDeskband));
					}
					long microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand4 = Unsafe.As<CComPtrNtv<ITrayDeskBand>, long>(ref s_spTrayDeskBand);
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)Unsafe.As<CComPtrNtv<ITrayDeskBand>, ulong>(ref s_spTrayDeskBand) + 48)))((nint)microsoft_002EZune_002EUtil_002E_003FA0x0277dc26_002Es_spTrayDeskBand4);
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
							MONITORINFO tagMONITORINFO = new() { cbSize = 40 };
							WINDOWPLACEMENT tagWINDOWPLACEMENT = new() { length = 44 };
							if (Module.GetMonitorInfoW(ptr, &tagMONITORINFO) != 0 && Module.GetWindowPlacement(m_hWndFrame, &tagWINDOWPLACEMENT) != 0)
							{
								RestoreState = Unsafe.As<WindowPlacementFlags, WindowState>(ref tagWINDOWPLACEMENT.flags) & WindowState.Maximized;
								Module.OffsetRect(&tagWINDOWPLACEMENT.rcNormalPosition, tagMONITORINFO.rcWork.left, tagMONITORINFO.rcWork.top);
								RECT tagRECT;
								if (Module.IntersectRect(&tagRECT, &tagWINDOWPLACEMENT.rcNormalPosition, &tagMONITORINFO.rcWork) == 0)
								{
									Module.CopyRect(&tagWINDOWPLACEMENT.rcNormalPosition, &tagMONITORINFO.rcWork);
									Module.InflateRect(&tagWINDOWPLACEMENT.rcNormalPosition, -100, -100);
								}
								RestorePosition.X = tagWINDOWPLACEMENT.rcNormalPosition.left;
								RestorePosition.Y = tagWINDOWPLACEMENT.rcNormalPosition.top;
								RestoreSize.Width = tagWINDOWPLACEMENT.rcNormalPosition.right - tagWINDOWPLACEMENT.rcNormalPosition.left;
								RestoreSize.Height = tagWINDOWPLACEMENT.rcNormalPosition.bottom - tagWINDOWPLACEMENT.rcNormalPosition.top;
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
				m_uTaskbarPlayerStateMsg = RegisterWindowMessage(Module.WNDMSG_ZuneTaskbarPlayerStateMsg);
				m_uTaskbarPlayerCommandMsg = RegisterWindowMessage(Module.WNDMSG_ZuneTaskbarPlayerCommandMsg);
				HINSTANCE* ptr = (HINSTANCE*)Module.GetWindowLongPtrW(m_hWndFrame, -6);
				WNDCLASS tagWNDCLASSW = new();
				if (!GetClassInfo(*ptr, Module.WNDCLASS_ZuneTaskbarPlayerCommandDispatch, out tagWNDCLASSW))
				{
					tagWNDCLASSW.style = WindowClassStyles.CS_NOCLOSE;
					tagWNDCLASSW.lpfnWndProc = (nint)Module.__unep_0040_003FCommandDispatcherWindowProc_0040Util_0040Zune_0040Microsoft_0040_0040_0024_0024FYA_JPEAUHWND___0040_0040I_K_J_0040Z;
					tagWNDCLASSW.hInstance = *ptr;
					tagWNDCLASSW.hCursor = *Module.LoadCursorW(null, (ushort*)32514);
					tagWNDCLASSW.hbrBackground = GetSysColorBrush(SystemColorIndex.COLOR_BACKGROUND);
					RegisterClass(tagWNDCLASSW);
				}
				if (CreateWindowEx(WindowStylesEx.WS_EX_TOOLWINDOW, Module.WNDCLASS_ZuneTaskbarPlayerCommandDispatch,
					Module.WNDCLASS_ZuneTaskbarPlayerCommandDispatch, WindowStyles.WS_POPUP, 0, 0, 0, 0, hInstance: *ptr) != null)
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
			if (s_spTrayDeskBand)
			{
				s_spTrayDeskBand.Dispose();
			}
		}

		private unsafe void DisplayPopup(int x, int y)
		{
			if (PopupVisible)
				return;
			POINTS tagPOINT;

			tagPOINT.x = (short)y;
			tagPOINT.y = (short)y;
			HMONITOR* ptr = Module.MonitorFromPoint(tagPOINT, 2u);
			if (ptr != null)
			{
				MONITORINFO tagMONITORINFO = new();
				tagMONITORINFO.cbSize = 40;
				if (Module.GetMonitorInfoW(ptr, &tagMONITORINFO) != 0)
				{
					if (x < tagMONITORINFO.rcMonitor.left)
					{
						PopupPosition.X = tagMONITORINFO.rcMonitor.left;
					}
					else if (x > tagMONITORINFO.rcMonitor.right - PopupSize.Width)
					{
						PopupPosition.X = tagMONITORINFO.rcMonitor.left - PopupSize.Width;
					}
					else
					{
						PopupPosition.X = x;
					}
					if (y < tagMONITORINFO.rcMonitor.top)
					{
						PopupPosition.Y = tagMONITORINFO.rcMonitor.top;
					}
					else if (y > tagMONITORINFO.rcMonitor.bottom - PopupSize.Height)
					{
						PopupPosition.Y = tagMONITORINFO.rcMonitor.bottom - PopupSize.Height;
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
			HWND* hWndFrame = m_hWndFrame;
			if (hWndFrame != null && Module.IsWindow(hWndFrame) != 0 && Module.GetForegroundWindow() != m_hWndFrame)
			{
				INPUT tagINPUT = new();
				tagINPUT.type = INPUTTYPE.INPUT_KEYBOARD;
				tagINPUT.ki = default;
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
					//base.Finalize();
				}
			}
		}

		public new void Dispose()
		{
			Dispose(true);
		}

		~TaskbarPlayer()
		{
			Dispose(false);
		}
	}
}
