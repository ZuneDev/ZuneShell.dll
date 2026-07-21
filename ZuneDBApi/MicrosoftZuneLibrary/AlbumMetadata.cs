using System;

namespace MicrosoftZuneLibrary;

// Original wraps a native IAlbumInfo* with each property forwarding directly to a
// vtable getter/setter, plus a ReleaseYear setter that normalizes 2-digit inputs
// (00-29 -> 2000s, 30-99 -> 1900s). Not reverse engineered — see
// logs/MicrosoftZuneLibrary/Device.md. Reimplemented as a plain mutable POCO instead:
// no code in this solution constructs one from a native pointer, so ordinary backing
// fields are both simpler and behaviorally equivalent for every reachable caller. The
// ReleaseYear 2-digit normalization is preserved since it's pure, self-contained logic.
public class AlbumMetadata : IDisposable
{
    private int _releaseYear = -1;

    public bool ExactMatch { get; internal set; }

    public uint TrackCount { get; internal set; }

    public uint WMISTrackCount { get; internal set; }

    public int MediaId { get; internal set; } = -1;

    public string CoverUrl { get; set; }

    public int ReleaseYear
    {
        get => _releaseYear;
        set => _releaseYear = value switch
        {
            >= 0 and <= 29 => value + 2000,
            >= 30 and <= 99 => value + 1900,
            _ => value,
        };
    }

    public string AlbumArtistYomi { get; set; }

    public string AlbumArtist { get; set; }

    public string AlbumTitleYomi { get; set; }

    public string AlbumTitle { get; set; }

    internal AlbumMetadata()
    {
    }

    public TrackMetadata GetTrack(uint index)
    {
        throw new ArgumentOutOfRangeException(nameof(index));
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
