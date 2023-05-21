using Microsoft.Iris.Markup;
using System;

namespace Microsoft.Zune.Schemas;

public class PlaylistContentItem : RateableMedia
{
    public PlaylistContentItem(MarkupTypeSchema schema) : base(schema)
    {
    }

    public int MediaId
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int MediaType
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int Ordinal
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public string ArtistName
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string AlbumName
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public int AlbumLibraryId
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int AlbumArtistLibraryId
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public string FilePath
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public TimeSpan Duration
    {
        get => GetProperty<TimeSpan>();
        set => SetProperty(value);
    }

    public long FileSize
    {
        get => GetProperty<long>();
        set => SetProperty(value);
    }

    public Guid ZuneMediaId
    {
        get => GetProperty<Guid>();
        set => SetProperty(value);
    }

}