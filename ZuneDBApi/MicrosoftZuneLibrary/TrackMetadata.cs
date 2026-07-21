using System;
using System.ComponentModel;

namespace MicrosoftZuneLibrary;

// Original wraps a native ITrackInfo* with each property forwarding directly to a
// vtable getter/setter. Not reverse engineered — see logs/MicrosoftZuneLibrary/Device.md.
// Reimplemented as a plain mutable POCO instead: no code in this solution constructs
// one from a native pointer (the original constructor is only ever called from
// AlbumMetadata.GetTrack, itself unreachable without a native IAlbumInfo*), so ordinary
// backing fields are both simpler and behaviorally equivalent for every reachable caller.
public class TrackMetadata : INotifyPropertyChanged
{
    private int _mediaId = -1;
    private int _discNumber = -1;
    private int _trackNumber = -1;
    private string _composer;
    private string _conductor;
    private string _genre;
    private string _trackArtist;
    private string _trackTitle;

    public event PropertyChangedEventHandler PropertyChanged;

    public int MediaId
    {
        get => _mediaId;
        set => SetField(ref _mediaId, value, nameof(MediaId));
    }

    public int DiscNumber
    {
        get => _discNumber;
        set => SetField(ref _discNumber, value, nameof(DiscNumber));
    }

    public int TrackNumber
    {
        get => _trackNumber;
        set => SetField(ref _trackNumber, value, nameof(TrackNumber));
    }

    public string Composer
    {
        get => _composer;
        set => SetField(ref _composer, value, nameof(Composer));
    }

    public string Conductor
    {
        get => _conductor;
        set => SetField(ref _conductor, value, nameof(Conductor));
    }

    public string Genre
    {
        get => _genre;
        set => SetField(ref _genre, value, nameof(Genre));
    }

    public string TrackArtist
    {
        get => _trackArtist;
        set => SetField(ref _trackArtist, value, nameof(TrackArtist));
    }

    public string TrackTitle
    {
        get => _trackTitle;
        set => SetField(ref _trackTitle, value, nameof(TrackTitle));
    }

    internal TrackMetadata()
    {
    }

    private void SetField<T>(ref T field, T value, string propName)
    {
        if (!Equals(field, value))
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
