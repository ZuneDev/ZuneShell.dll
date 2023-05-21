using Microsoft.Iris.Markup;
using System;
using System.Collections;

namespace Microsoft.Zune.Schemas;

public class Track : RateableMedia
{
    public Track(MarkupTypeSchema schema) : base(schema)
    {
    }

    public int AlbumLibraryId
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int ArtistLibraryId
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int AlbumArtistLibraryId
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public string ArtistName
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string ArtistNameYomi
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string TitleYomi
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public IList ContributingArtistNames
    {
        get => GetProperty<IList>();
        set => SetProperty(value);
    }

    public string AlbumArtistName
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string AlbumName
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public DateTime ReleaseDate
    {
        get => GetProperty<DateTime>();
        set => SetProperty(value);
    }

    public string Genre
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public Guid ZuneMediaId
    {
        get => GetProperty<Guid>();
        set => SetProperty(value);
    }

    public int TrackNumber
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public TimeSpan Duration
    {
        get => GetProperty<TimeSpan>();
        set => SetProperty(value);
    }

    public string ComponentId
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string FileName
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string FolderName
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string FilePath
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public bool NowPlaying
    {
        get => GetProperty<bool>();
        set => SetProperty(value);
    }

    public bool InLibrary
    {
        get => GetProperty<bool>();
        set => SetProperty(value);
    }

    public DateTime DateLastPlayed
    {
        get => GetProperty<DateTime>();
        set => SetProperty(value);
    }

    public int PlayCount
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public long FileSize
    {
        get => GetProperty<long>();
        set => SetProperty(value);
    }

    public string ComposerName
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string ConductorName
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public int IsProtected
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public DateTime DateAdded
    {
        get => GetProperty<DateTime>();
        set => SetProperty(value);
    }

    public int Bitrate
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public string MediaType
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public int DiscNumber
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int ContributingArtistCount
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public DateTime DateAlbumAdded
    {
        get => GetProperty<DateTime>();
        set => SetProperty(value);
    }

    public long FileCount
    {
        get => GetProperty<long>();
        set => SetProperty(value);
    }

    public int DrmState
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public long DrmStateMask
    {
        get => GetProperty<long>();
        set => SetProperty(value);
    }

    public int QuickMixState
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

}