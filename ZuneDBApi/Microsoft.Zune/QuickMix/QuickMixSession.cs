using System;
using ZuneUI;

namespace Microsoft.Zune.QuickMix;

// Original wraps a native IQuickMixSession*. Not reverse engineered — see
// logs/Microsoft.Zune/QuickMix/QuickMix.md.
public class QuickMixSession : IDisposable
{
    internal QuickMixSession()
    {
    }

    public EQuickMixType GetQuickMixType() => EQuickMixType.eQuickMixTypeInvalid;

    public HRESULT SetQuickMixType(EQuickMixType eQuickMixType) => HRESULT._E_FAIL;

    public bool GetQuickMixTypeAvailable(EQuickMixType eQuickMixType) => false;

    public HRESULT GetSimilarMedia(uint maxBatchTracks, TimeSpan maxBatchTimeout, SimilarMediaBatchHandler similarBatchHandler, BatchEndHandler batchEndHandler)
    {
        batchEndHandler?.Invoke(HRESULT._E_FAIL);
        return HRESULT._E_FAIL;
    }

    public HRESULT GetSimilarMedia(TimeSpan maxBatchDuration, TimeSpan maxBatchTimeout, SimilarMediaBatchHandler similarBatchHandler, BatchEndHandler batchEndHandler)
    {
        batchEndHandler?.Invoke(HRESULT._E_FAIL);
        return HRESULT._E_FAIL;
    }

    public HRESULT Refresh(TimeSpan maxBatchTimeout, bool reloadSettings, SimilarMediaBatchHandler similarBatchHandler, BatchEndHandler batchEndHandler)
    {
        batchEndHandler?.Invoke(HRESULT._E_FAIL);
        return HRESULT._E_FAIL;
    }

    public HRESULT GetPlaylistTitle(out string playlistTitle)
    {
        playlistTitle = null;
        return HRESULT._E_FAIL;
    }

    public HRESULT SaveAsPlaylist(string playlistTitle, Playlist.CreatePlaylistOption createOption, out int playlistId)
    {
        playlistId = 0;
        return HRESULT._E_FAIL;
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
