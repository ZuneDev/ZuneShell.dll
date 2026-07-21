using System;

namespace Microsoft.Zune.Util;

// Original wraps a native IRadioStationManager*. Not reverse engineered — see
// logs/Microsoft.Zune/Util/DownloadManager.md.
public class RadioStationManager : IDisposable
{
    private static RadioStationManager sm_radioStationManager;

    public static RadioStationManager Instance => sm_radioStationManager ??= new RadioStationManager();

    private RadioStationManager()
    {
    }

    public RadioPlaylist GetRadioPlaylist(string uri) => null;

    public void AddStation(string title, string sourceUrl, string imageUrl, RadioStationProgressHandler radioStationProgressHandler)
    {
        radioStationProgressHandler?.Invoke(ZuneUI.HRESULT._E_FAIL);
    }

    public void DeleteStation(string title, RadioStationProgressHandler radioStationProgressHandler)
    {
        radioStationProgressHandler?.Invoke(ZuneUI.HRESULT._E_FAIL);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
