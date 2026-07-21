using System;
using System.Collections.Generic;
using ZuneUI;

namespace Microsoft.Zune.Playlist;

// Original wraps a native IAutoPlaylistRules*. Not reverse engineered — see
// logs/Microsoft.Zune/Playlist/PlaylistManager.md.
public class AutoPlaylistBuilder : IDisposable
{
    private Dictionary<GroupAndAtom, AtomRules> m_groupAndAtomToRules = new();
    private EMediaTypes m_type;

    public EMediaTypes Schema => m_type;

    // Original loads existing rules for playlistId from the native IAutoPlaylistRules*
    // this constructor obtains via PlaylistManager — not reverse engineered (see
    // logs/Microsoft.Zune/Playlist/PlaylistManager.md), so this starts as an empty
    // builder rather than reflecting the playlist's actual saved rules.
    public AutoPlaylistBuilder(int playlistId)
    {
        PlaylistManager.Instance.GetAutoPlaylistSchema(playlistId, out m_type);
    }

    public AutoPlaylistBuilder(EMediaTypes type)
    {
        m_type = type;
    }

    public AutoPlaylistBuilder()
        : this(EMediaTypes.eMediaTypeAudio)
    {
    }

    public void Clear()
    {
        m_groupAndAtomToRules.Clear();
    }

    public HRESULT AddCriterion(string atomName, PlaylistRuleOperator op, object value) => HRESULT._E_FAIL;

    public HRESULT AddSort(string sort) => HRESULT._E_FAIL;

    public HRESULT AddFilter(string atomName, object value) => HRESULT._E_FAIL;

    public HRESULT SetRules(int playlistId) => HRESULT._E_FAIL;

    public AtomRules GetCriterionByAtomName(int ruleSetGroup, string atomName) => null;

    public object GetFilterByAtomName(string atomName) => null;

    public string GetSort(int ruleSetGroup) => null;

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
