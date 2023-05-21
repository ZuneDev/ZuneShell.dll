using Microsoft.Iris.Drawing;
using Microsoft.Iris.Markup;
using System;
using System.Collections;

namespace Microsoft.Zune.Schemas;

public class Video : RateableMedia
{
    public Video(MarkupTypeSchema schema) : base(schema)
    {
    }

    public string Description
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public TimeSpan Duration
    {
        get => GetProperty<TimeSpan>();
        set => SetProperty(value);
    }

    public DateTime DateAdded
    {
        get => GetProperty<DateTime>();
        set => SetProperty(value);
    }

    public DateTime ReleaseDate
    {
        get => GetProperty<DateTime>();
        set => SetProperty(value);
    }

    public bool Explicit
    {
        get => GetProperty<bool>();
        set => SetProperty(value);
    }

    public UIImage Thumbnail
    {
        get => GetProperty<UIImage>();
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

    public int FileType
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int ArtistLibraryId
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

    public int Width
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int Height
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int Definition
    {
        get => GetProperty<int>();
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

    public long FileSize
    {
        get => GetProperty<long>();
        set => SetProperty(value);
    }

    public string ProductionCompany
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public IList DirectorNames
    {
        get => GetProperty<IList>();
        set => SetProperty(value);
    }

    public IList ActorNames
    {
        get => GetProperty<IList>();
        set => SetProperty(value);
    }

    public string Network
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string SeriesTitle
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public int SeasonNumber
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int EpisodeNumber
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public string Genre
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public int CategoryId
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public string ParentalRating
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public Guid ZuneMediaId
    {
        get => GetProperty<Guid>();
        set => SetProperty(value);
    }

    public DateTime DateLastPlayed
    {
        get => GetProperty<DateTime>();
        set => SetProperty(value);
    }

    public int DrmState
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

}