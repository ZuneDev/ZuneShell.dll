using System;

namespace Microsoft.Zune.Util;

// Original wraps a native IZuneWebHost* singleton service plus its own event sink and
// heavy ETW (WPP) tracing throughout. Not reverse engineered — see
// logs/Microsoft.Zune/Util/DownloadManager.md. The ETW tracing calls in the original
// are diagnostic-only and were dropped rather than translated.
public class ZuneWebHost
{
    private static ZuneWebHost s_zuneWebHost;

    private NavigationCompleteHandler m_navCompleteHandler;
    private NavigationErrorHandler m_navErrorHandler;

    public static ZuneWebHost Instance => s_zuneWebHost ??= new ZuneWebHost();

    private ZuneWebHost()
    {
    }

    public long Initialize(string navUrl, long hWndHost, int width, int height) => 0;

    public bool SetSize(long hWndHost, int width, int height) => false;

    public void SetNavigationCompleteHandler(NavigationCompleteHandler navigationCompleteHandler)
    {
        m_navCompleteHandler = navigationCompleteHandler;
    }

    public void SetNavigationErrorHandler(NavigationErrorHandler navigationErrorHandler)
    {
        m_navErrorHandler = navigationErrorHandler;
    }

    public void OnNavigationComplete(string data)
    {
        m_navCompleteHandler?.Invoke(data);
    }

    public void OnNavigationError(string navUrl, int errorCode)
    {
        m_navErrorHandler?.Invoke(navUrl, errorCode);
    }
}
