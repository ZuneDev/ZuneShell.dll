global using System.Runtime.InteropServices.ComTypes;
global using Vanara.PInvoke;
global using static Vanara.PInvoke.User32;
global using static Vanara.PInvoke.Kernel32;
global using static Vanara.PInvoke.Ole32;
global using static Vanara.PInvoke.OleAut32;
global using static Vanara.PInvoke.Gdi32;
global using static Vanara.PInvoke.Shell32;
global using static Vanara.PInvoke.AdvApi32;
global using ZuneDBApi;
global using GC = System.GC;
global using HRESULT = ZuneUI.HRESULT;
global using _GUID = System.Guid;
global using _SYSTEMTIME = Vanara.PInvoke.SYSTEMTIME;

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace ZuneDBApi
{
    internal static unsafe class Module
    {
        internal const string ZUNENATIVELIB_DLL = "ZuneNativeLib";
        internal const string ZUNESERVICE_DLL = "ZuneService";

        private static bool s_bIsLonghornOrBetter;
        private static bool s_bIsLonghornOrBetterInitialized;

        internal static readonly string WINDOWCLASS_MsnMsgrUIManager = "MsnMsgrUIManager";

        internal static readonly _GUID GUID_NULL = _GUID.Empty;
        internal static readonly _GUID GUID_IService = new("bb2d1edd-1bd5-4be1-8d38-36d4f0849911");
        internal static readonly _GUID GUID_IUserManager = new("c9e0f18a-6c53-47d0-991e-dbd4fe395101");
        internal static readonly _GUID GUID_IFeatureEnablementManager = new("9581b41a-b5cf-4ebf-9d1a-975477e081ca");
        internal static readonly _GUID GUID_ITelemetryManager = new("ab28333b-a55c-4312-a7a3-2dd60d4a7154");
        internal static readonly _GUID GUID_IMetadataManager = new("6dd7146d-7a19-4fbb-9235-9e6c382fcc71");
        internal static readonly _GUID GUID_IDeviceSyncRulesProvider = new("b12dc962-cc1b-46c5-a92a-68f1f2b9bff3");
        internal static readonly _GUID GUID_IFirmwareUpdater2 = new("f066fc29-e525-4ddc-abe6-5213d22c14d2");
        internal static readonly _GUID GUID_IRecordManager = new("c1cad55a-5652-40c7-842c-39cbe209379e");
        internal static readonly _GUID GUID_IQuickMixManager = new("d69e22ae-7e21-4959-be6e-14462eb96f64");
        internal static readonly _GUID GUID_IPlaylistManager = new("c2d9122b-f648-4b95-92fc-11f2e7f326d7");
        internal static readonly _GUID GUID_IDeviceContentProvider = new("7472ae89-073d-420b-9828-51f9d80ca2a6");
        internal static readonly _GUID GUID_ISubscriptionManager = new("9dc7c984-41d5-4130-a5ac-46d0825cd29d");
        internal static readonly _GUID GUID_IUsageDataManager = new("2f36e709-c431-4836-ab2b-ab57aef0cf1a");
        internal static readonly _GUID GUID_IFamilySettingsProvider = new("04f38ab5-391b-4b5a-a2c1-d4b74aeb4be9");

        internal static void _ZuneShipAssert(uint v1, uint v2)
        {
            System.Diagnostics.Debug.WriteLine($"ShipAssert: {v1:X2} {v2:X2}");
        }

        public static HANDLE ToHandle(void* h) => new(new IntPtr(h));
        public static void* ToPointer(this HANDLE h) => h.DangerousGetHandle().ToPointer();

        public static HINSTANCE ToHInstance(void* h) => new(new IntPtr(h));
        public static void* ToPointer(this HINSTANCE h) => h.DangerousGetHandle().ToPointer();

        public static T AddByteOffset<T>(ref T obj, int offset)
        {
            return System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, new IntPtr(offset));
        }

        public static int CloseClipboard()
        {
            return User32.CloseClipboard() ? 1 : 0;
        }

        public static int CloseHandle(void* h)
        {
            return Kernel32.CloseHandle(new IntPtr(h)) ? 1 : 0;
        }

        public static int CoCreateInstance(_GUID rclsid, object pUnkOuter, uint dwClsContext, _GUID riid, out object ppv)
        {
            return Ole32.CoCreateInstance(rclsid, pUnkOuter, (CLSCTX)dwClsContext, riid, out ppv).Code;
        }

        public static int CompareStringW(uint localeId, uint dwCmpFlags, ushort* lpString1, int cchCount1, ushort* lpString2, int cchCount2)
        {
            return CompareString(new(localeId), (COMPARE_STRING)dwCmpFlags, new((char*)lpString1), cchCount1, new((char*)lpString2), cchCount2);
        }

        public static void* CopyImage(void* h, uint type, int cx, int cy, uint flags)
        {
            return User32.CopyImage(ToHandle(h), (LoadImageType)type, cx, cy, (CopyImageOptions)flags).ToPointer();
        }

        public static int CopyRect(RECT* lprcDst, RECT* lprcSrc)
        {
            return User32.CopyRect(out *lprcDst, in *lprcSrc) ? 1 : 0;
        }

        public static void* CreateEventW(SECURITY_ATTRIBUTES lpEventAttributes, int bManualReset, int bInitialState, ushort* lpName)
        {
            return ((IntPtr)Kernel32.CreateEvent(lpEventAttributes, bManualReset != 0, bInitialState != 0, new((char*)lpName))).ToPointer();
        }

        public static int CreateHMESettings(IHMESettings** obj)
        {
            throw new NotImplementedException();
        }

        public static HWND* CreateWindowExW(uint dwExStyle, ushort* lpClassName, ushort* lpWindowName,
            uint dwStyle, int X, int Y, int nWidth, int nHeight, HWND* hWndParent, HMENU* hMenu, HINSTANCE* hInstance, void* lpParam)
        {
            return (HWND*)User32.CreateWindowEx(
                (WindowStylesEx)dwExStyle,
                new string((char*)lpClassName),
                new string((char*)lpWindowName),
                (WindowStyles)dwStyle,
                X, Y, nWidth, nHeight,
                *hWndParent,
                *hMenu,
                *hInstance,
                new IntPtr(lpParam)).DangerousGetHandle().ToPointer();
        }

        internal static FILETIME DateTimeToFileTime(DateTime dateTime)
        {
            FILETIME result = default;
            try
            {
                result.dwLowDateTime = (int)(dateTime.ToFileTimeUtc() & uint.MaxValue);
                result.dwHighDateTime = (int)(dateTime.ToFileTimeUtc() >> sizeof(int));
            }
            catch (Exception)
            {
            }
            return result;
        }

        internal static _SYSTEMTIME DateTimeToSystemTime(DateTime dateTime)
        {
            _SYSTEMTIME result = default;
            FILETIME filetime = DateTimeToFileTime(dateTime);

            Module.FileTimeToSystemTime(&filetime, &result);
            return result;
        }

        public static long DefWindowProcW(HWND* hWnd, uint Msg, ulong wParam, long lParam)
        {
            return User32.DefWindowProc(*hWnd, Msg, new IntPtr((int)wParam), new IntPtr(lParam)).ToInt64();
        }

        public static void DeleteCriticalSection(CRITICAL_SECTION* lpCriticalSection)
        {
            Kernel32.DeleteCriticalSection(ref *lpCriticalSection);
        }

        public static int DeleteObject(void* obj)
        {
            return Gdi32.DeleteObject(*(HGDIOBJ*)obj) ? 1 : 0;
        }

        public static uint DragQueryFileW(HDROP* hDrop, uint iFile, ushort* lpszFile, uint cch)
        {
            return Shell32.DragQueryFile(*hDrop, iFile, new((char*)lpszFile), cch);
        }

        public static void EnterCriticalSection(CRITICAL_SECTION* lpCriticalSection)
        {
            Kernel32.EnterCriticalSection(ref *lpCriticalSection);
        }

        internal static DateTime FileTimeToDateTime(FILETIME ftValue)
        {
            long fileTime = ftValue.dwLowDateTime | ftValue.dwHighDateTime >> sizeof(int);
            DateTime result = DateTime.MinValue;
            try
            {
                result = DateTime.FromFileTimeUtc(fileTime);
            }
            catch (Exception)
            {
            }
            return result;
        }

        public static int FileTimeToSystemTime(FILETIME* lpFileTime, _SYSTEMTIME* lpSystemTime)
        {
            return Kernel32.FileTimeToSystemTime(*lpFileTime, out *lpSystemTime) ? 1 : 0;
        }

        public static HWND* FindWindowExW(HWND* hWndParent, HWND* hWndChildAfter, ushort* lpszClass, ushort* lpszWindow)
        {
            var window = User32.FindWindowEx(*hWndParent, *hWndChildAfter, new((char*)lpszClass), new((char*)lpszWindow));
            return &window;
        }

        internal static uint FormatMessage(uint dwFlags, void* lpSource, uint dwMessageId, uint dwLanguageId, ushort* lpBuffer, uint nSize, sbyte** Arguments)
        {
            return FormatMessageW(dwFlags, lpSource, dwMessageId, dwLanguageId, lpBuffer, nSize, Arguments);
        }

        public static uint FormatMessageW(uint dwFlags, void* lpSource, uint dwMessageId, uint dwLanguageId, ushort* lpBuffer, uint nSize, sbyte** Arguments)
        {
            System.Text.StringBuilder builder = new(new string((char*)lpBuffer));
            return (uint)Kernel32.FormatMessage((FormatMessageFlags)dwFlags, ToHInstance(lpSource), dwMessageId, dwLanguageId, builder, nSize, new IntPtr(Arguments));
        }

        public static int GetClassInfoW(HINSTANCE* hInstance, ushort* lpClassName, ref WNDCLASS lpWndClass)
        {
            return User32.GetClassInfo(*hInstance, new((char*)lpClassName), out lpWndClass) ? 1 : 0;
        }

        public static int GetClientRect(HWND* hWnd, RECT* lpRect)
        {
            return User32.GetClientRect(*hWnd, out *lpRect) ? 1 : 0;
        }

        public static void* GetClipboardData(uint uFormat)
        {
            return User32.GetClipboardData(uFormat).ToPointer();
        }

        public static int GetCursorPos(POINTS* lpPoint)
        {
            var success = User32.GetCursorPos(out var point);
            *lpPoint = new((short)point.X, (short)point.Y);
            return success ? 1 : 0;
        }

        internal static string GetErrorDescription(int hr)
        {
            void* ptr = null;
            if (FormatMessage(4864, null, (uint)(hr & 65535), 0, (ushort*)(&ptr), 0, null) == 0)
            {
                return string.Format("Unknown Error: 0x{0:x}", hr);
            }
            string arg = new((char*)ptr);
            string result = string.Format("{0} Error: 0x{1:x}", arg, hr);
            if (ptr != null)
            {
                LocalFree(ptr);
            }
            return result;
        }

        public static HWND* GetForegroundWindow()
        {
            HWND hWnd = User32.GetForegroundWindow();
            return &hWnd;
        }

        public static uint GetLastError()
        {
            return (uint)Kernel32.GetLastError();
        }

        public static int GetMonitorInfoW(HMONITOR* hMonitor, MONITORINFO* lpmi)
        {
            return User32.GetMonitorInfo(*hMonitor, ref *lpmi) ? 1 : 0;
        }

        public static int GetObjectW(void* hgdiobj, int bufferSize, void* lpvObject)
        {
            return Gdi32.GetObject(new(new IntPtr(hgdiobj)), bufferSize, new IntPtr(lpvObject));
        }

        [DllImport(ZUNESERVICE_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int GetServiceEndPointUri(EServiceEndpointId endpointId, ushort** uri);

        public static int GetVersionExW(ref OSVERSIONINFOEX lpVersionInformation)
        {
            return Kernel32.GetVersionEx(ref lpVersionInformation) ? 1 : 0;
        }

        public static HWND* GetWindow(HWND* hWnd, uint uCmd)
        {
            var window = User32.GetWindow(*hWnd, (GetWindowCmd)uCmd);
            return &window;
        }

        public static long GetWindowLongPtrW(HWND* hWnd, int nIndex)
        {
            return User32.GetWindowLongPtr(*hWnd, (WindowLongFlags)nIndex).ToInt64();
        }

        public static int GetWindowPlacement(HWND* hWnd, WINDOWPLACEMENT* lpwndpl)
        {
            return User32.GetWindowPlacement(*hWnd, ref *lpwndpl) ? 1 : 0;
        }

        public static void* GlobalLock(void* hMem)
        {
            return Kernel32.GlobalLock(new(new IntPtr(hMem))).ToPointer();
        }

        public static int InflateRect(RECT* lprc, int dx, int dy)
        {
            return User32.InflateRect(ref *lprc, dx, dy) ? 1 : 0;
        }

        public static void InitializeCriticalSection(CRITICAL_SECTION* lpCriticalSection)
        {
            Kernel32.InitializeCriticalSection(out *lpCriticalSection);
        }

        internal unsafe static void InitIsLonghornOrBetter()
        {
            // This is kinda pointless, since .NET 5 barely runs on Windows 7, let alone Longhorn...
            // ...but you never know when someone's going to try it anyway.
            s_bIsLonghornOrBetter = false;
            OSVERSIONINFOEX osversioninfow = default;
            if (GetVersionExW(ref osversioninfow) == 1)
            {
                s_bIsLonghornOrBetter = (osversioninfow.dwPlatformId == PlatformID.Win32NT)
                    && (osversioninfow.dwMajorVersion >= 6);
            }
            s_bIsLonghornOrBetterInitialized = true;
        }

        public static int IntersectRect(RECT* lprcDest, RECT* lprcSrc1, RECT* lprcSrc2)
        {
            return User32.IntersectRect(out *lprcDest, *lprcSrc1, *lprcSrc2)? 1 : 0;
        }

        public static int IsClipboardFormatAvailable(uint uFormat)
        {
            return User32.IsClipboardFormatAvailable(uFormat) ? 1 : 0;
        }

        internal static bool IsLonghornOrBetter()
        {
            if (!s_bIsLonghornOrBetterInitialized)
                InitIsLonghornOrBetter();
            return s_bIsLonghornOrBetter;
        }

        public static int IsWindow(HWND* hWnd)
        {
            return User32.IsWindow(*hWnd) ? 1 : 0;
        }

        public static int KillTimer(HWND* hWnd, ulong uIDEvent)
        {
            return User32.KillTimer(*hWnd, new IntPtr((void*)uIDEvent)) ? 1 : 0;
        }

        public static void LeaveCriticalSection(CRITICAL_SECTION* lpCriticalSection)
        {
            Kernel32.LeaveCriticalSection(ref *lpCriticalSection);
        }

        public static HICON* LoadCursorW(HINSTANCE* hInstance, ushort* lpCursorName)
        {
            var cur = User32.LoadCursor(*hInstance, new string((char*)lpCursorName));
            return (HICON*)cur.DangerousGetHandle().ToPointer();
        }

        public static int LoadStringW(HINSTANCE* hInstance, uint uID, ushort* lpBuffer, int nBufferMax)
        {
            var result = User32.LoadString(*hInstance, (int)uID, out IntPtr buffer, nBufferMax);
            lpBuffer = (ushort*)buffer.ToPointer();
            return result;
        }

        public static void* LocalFree(void* hMem)
        {
            return Kernel32.LocalFree(new(new IntPtr(hMem))).DangerousGetHandle().ToPointer();
        }

        public static int lstrlenW(ushort* lpString)
        {
            return Kernel32.lstrlen(new((char*)lpString));
        }

        public static HMONITOR* MonitorFromPoint(POINTS point, uint dwFlags)
        {
            var mon = User32.MonitorFromPoint(point.ToPoint(), (MonitorFlags)dwFlags);
            return &mon;
        }

        public static HMONITOR* MonitorFromWindow(HWND* hWnd, uint dwFlags)
        {
            var mon = User32.MonitorFromWindow(*hWnd, (MonitorFlags)dwFlags);
            return &mon;
        }

        public static int MoveWindow(HWND* hWnd, int X, int Y, int nWidth, int nHeight, int bRepaint)
        {
            return User32.MoveWindow(*hWnd, X, Y, nWidth, nHeight, bRepaint == 1) ? 1 : 0;
        }

        public static int OffsetRect(RECT* lprc, int dx, int dy)
        {
            return User32.OffsetRect(ref *lprc, dx, dy) ? 1 : 0;
        }

        public static int OpenClipboard(HWND* hWndNewOwner)
        {
            return User32.OpenClipboard(*hWndNewOwner) ? 1 : 0;
        }

        public static int PostMessageW(HWND* hWnd, uint Msg, ulong wParam, long lParam)
        {
            return User32.PostMessage(*hWnd, Msg, new IntPtr((void*)wParam), new IntPtr((void*)lParam)) ? 1 : 0;
        }

        public static int PropVariantClear(PROPVARIANT pvar)
        {
            return Ole32.PropVariantClear(pvar).Code;
        }

        public static int PropVariantCopy(PROPVARIANT pDst, PROPVARIANT pSrc)
        {
            return Ole32.PropVariantCopy(pDst, pSrc).Code;
        }

        public static int PtInRect(RECT* lprc, POINTS pt)
        {
            return User32.PtInRect(*lprc, pt.ToPoint()) ? 1 : 0;
        }

        public static ushort RegisterClassW(ref WNDCLASS lpWndClass)
        {
            return User32.RegisterClass(lpWndClass);
        }

        public static uint RegisterWindowMessageW(ushort* lpString)
        {
            return User32.RegisterWindowMessage(new((char*)lpString));
        }

        public static int ScreenToClient(HWND* hWnd, POINTS* lpPoint)
        {
            var point = (*lpPoint).ToPoint();
            var result = User32.ScreenToClient(*hWnd, ref point);
            (*lpPoint) = new((short)point.X, (short)point.Y);
            return result ? 1 : 0;
        }

        public static uint SendInput(uint cInputs, INPUT* pInputs, int cbSize)
        {
            // TODO: This ain't right
            return User32.SendInput(cInputs, new[] { *pInputs }, cbSize);
        }

        public static long SendMessageTimeoutW(HWND* hWnd, uint Msg, ulong wParam, long lParam, uint fuFlags, uint uTimeout, ulong* lpdwResult)
        {
            IntPtr msgResult = new(lpdwResult);
            IntPtr result = User32.SendMessageTimeout(*hWnd, Msg, new((long)wParam), new(lParam), (SMTO)fuFlags, uTimeout, ref msgResult);
            lpdwResult = (ulong*)msgResult.ToPointer();
            return result.ToInt64();
        }

        public static int SetEvent(void* hEvent)
        {
            return Kernel32.SetEvent(new IntPtr(hEvent)) ? 1 : 0;
        }

        public static int SetForegroundWindow(HWND* hWnd)
        {
            return User32.SetForegroundWindow(*hWnd) ? 1 : 0;
        }

        public static uint SetThreadExecutionState(uint esFlags)
        {
            return (uint)Kernel32.SetThreadExecutionState((EXECUTION_STATE)esFlags);
        }

        public static ulong SetTimer(HWND* hWnd, ulong nIDEvent, uint uElapse, Timerproc lpTimerFunc)
        {
            return (ulong)User32.SetTimer(*hWnd, new((long)nIDEvent), uElapse, lpTimerFunc).ToInt64();
        }

        [DllImport(ZUNENATIVELIB_DLL)]
        public unsafe static extern int SetUIThreadCBWorker(IUIThreadCallbackWorker* pUIThreadCallbackWorker);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void SQMAddNumbersToStream(string sqmDataId, uint countTotal, uint dw1);

        [DllImport(ZUNENATIVELIB_DLL, CharSet = CharSet.Unicode)]
        public static extern void SQMAddWrapper(string sqmDataId, int nData);

        public static ushort* SysAllocString(ushort* psz)
        {
            return (ushort*)OleAut32.SysAllocString(new string((char*)psz)).DangerousGetHandle().ToPointer();
        }

        public static void SysFreeString(ushort* psz)
        {
            OleAut32.SysFreeString(new IntPtr(psz));
        }

        public static uint SysStringLen(ushort* psz)
        {
            return OleAut32.SysStringLen(new SafeBSTR(new(psz)));
        }

        internal unsafe static DateTime SystemTimeToDateTime(_SYSTEMTIME stValue)
        {
            FILETIME ftValue = default;

            Module.SystemTimeToFileTime(&stValue, &ftValue);
            return Module.FileTimeToDateTime(ftValue);
        }

        public static int SystemTimeToFileTime(_SYSTEMTIME* lpSystemTime, FILETIME* lpFileTime)
        {
            return Kernel32.SystemTimeToFileTime(in *lpSystemTime, out *lpFileTime) ? 1 : 0;
        }

        public static uint TraceEvent(ulong sessionHandle, PEVENT_TRACE_HEADER EventTrace)
        {
            return (uint)AdvApi32.TraceEvent(new TRACEHANDLE(sessionHandle), EventTrace).ToHRESULT().Code;
        }

        public static uint TraceMessage(ulong sessionHandle, uint MessageFlags, _GUID MessageGuid, ushort MessageNumber, void* arglist)
        {
            return (uint)AdvApi32.TraceMessageVa(new TRACEHANDLE(sessionHandle), (TRACE_MESSAGE)MessageFlags, in MessageGuid, MessageNumber, new IntPtr(arglist)).ToHRESULT().Code;
        }

        public static int VariantClear(VARIANT* pvarg)
        {
            return OleAut32.VariantClear(new IntPtr(pvarg)).Code;
        }

        public static void VariantInit(VARIANT* pvarg)
        {
            OleAut32.VariantInit(ref *pvarg);
        }

        public static uint WaitForSingleObject(void* hHandle, uint dwMilliseconds)
        {
            return (uint)WaitForSingleObject(new IntPtr(hHandle), dwMilliseconds);
        }
        [DllImport(Lib.Kernel32, SetLastError = true, ExactSpelling = true)]
        private static extern WAIT_STATUS WaitForSingleObject([In] IntPtr hHandle, uint dwMilliseconds);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern int AddDeviceSyncRule(EDeviceSyncRuleType syncRuleType, [MarshalAs(UnmanagedType.U1)] bool b1, int i1, EMediaTypes mediaType, int i2);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern int AddDeviceSyncRuleWithValue(EDeviceSyncRuleType syncRuleType, int i1, EMediaTypes mediaType, int i2, int i3);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int AddGrovelerScanDirectory(ushort* directory, EMediaTypes mediaType);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int AddItemToPlaylist(int i1, int i2, IPlaylist* playlist);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int AddMedia(IMSMediaSchemaPropertySet* properties, EMediaTypes mediaType, int* i1);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int AddMedia(ushort* path, EMediaTypes mediaType, uint u1, bool* b1, int* i1);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int AddTransientMedia(ushort* path, EMediaTypes mediaType, int* i1);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int CanAddFromFolder(ushort* folder);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int CanAddMedia(ushort* folder, EMediaTypes mediaType);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern int CleanupTransientMedia();

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int CompareWithoutArticles(ushort* str1, ushort* str2, int* i1);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int GetSyncRuleForMedia(int rule1, EMediaTypes mediaType, int rule2, EDeviceSyncRuleType* syncRuleType);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int LocateArt(int mediaId, EMediaTypes mediaType, [MarshalAs(UnmanagedType.U1)] bool cacheOnly, ushort** outString);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int QueryDatabase(EQueryType queryType, IQueryPropertyBag* bag, IDatabaseQueryResults** results, ushort** outString);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int CopyThumbnailBitmapData(HBITMAP* src, HBITMAP** dst, void** pData);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int CopyThumbnailBitmapData(HBITMAP* src, int srcX, int srcY, int srcWidth, int srcHeight,
            int dstWidth, int dstHeight, HBITMAP** dst, void** pData);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int CreateAlbumPropSet(_GUID albumId, ushort* albumTitle, ushort* artistTitle, IMSMediaSchemaPropertySet** propSet);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int CreateContentRefreshTask(IAsyncCallback* callback, IContentRefreshTask** task);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int CreateDataObjectEnum(IDataObjectEnumerator** objEnum);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int CreateDRMQuery(IDRMQuery** drmQuery);

        [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int CreateEmptyPlaylist(IPlaylist** outPlaylist);
    }
}
