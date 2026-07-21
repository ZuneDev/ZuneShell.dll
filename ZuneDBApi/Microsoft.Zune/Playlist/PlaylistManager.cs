using System;
using ZuneUI;

namespace Microsoft.Zune.Playlist;

// Original wraps a native IPlaylistManager* singleton. Not reverse engineered — see
// logs/Microsoft.Zune/Playlist/PlaylistManager.md.
public class PlaylistManager : IDisposable
{
    private static PlaylistManager sm_instance;

    public static PlaylistManager Instance => sm_instance ??= new PlaylistManager();

    private PlaylistManager()
    {
    }

    public HRESULT CreatePlaylist(string title, string author, ValueType serviceMediaId, CreatePlaylistOption options, out int playlistId)
    {
        playlistId = 0;
        return HRESULT._E_FAIL;
    }

    public HRESULT AddMediaToPlaylist(int playlistId, int cMediaCount, int[] mediaIds, int[] mediaTypeIds, int insertPos, int[] playlistContentIds)
    {
        return HRESULT._E_FAIL;
    }

    public void RemoveMediaFromPlaylist(int playlistId, int cIdCount, int[] playlistContentIds)
    {
    }

    public HRESULT UpdateMediaPositionInPlaylist(int playlistId, int cPlaylistContentCount, int[] playlistContentIds, int newPosition)
    {
        return HRESULT._E_FAIL;
    }

    public void DedupPlaylist(int playlistId)
    {
    }

    public HRESULT RenamePlaylist(int playlistId, string newTitle)
    {
        return HRESULT._E_FAIL;
    }

    public HRESULT SavePlaylistAsStatic(int playlistId, PlaylistAsyncOperationCompleted playlistAsyncOperationCompleted)
    {
        playlistAsyncOperationCompleted?.Invoke(HRESULT._E_FAIL);
        return HRESULT._E_FAIL;
    }

    public string GetUniquePlaylistTitle(string candidateTitle) => candidateTitle;

    public HRESULT GetPlaylistByServiceMediaId(Guid serviceMediaId, out int playlistId)
    {
        playlistId = 0;
        return HRESULT._E_FAIL;
    }

    public HRESULT GetAutoPlaylistSchema(int playlistId, out EMediaTypes schema)
    {
        schema = EMediaTypes.eMediaTypeInvalid;
        return HRESULT._E_FAIL;
    }

    public HRESULT EnableAutomaticRefresh(int playlistId) => HRESULT._E_FAIL;

    public HRESULT DisableAutomaticRefresh(int playlistId) => HRESULT._E_FAIL;

    public HRESULT RefreshAutoPlaylist(int playlistId) => HRESULT._E_FAIL;

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
