using Microsoft.Iris.Drawing;
using Microsoft.Iris.Markup;
using System;

namespace Microsoft.Zune.Schemas;

public class Album : Media
{
    public Album(MarkupTypeSchema schema) : base(schema)
    {
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

    public Guid ZuneMediaId
    {
        get => GetProperty<Guid>();
        set => SetProperty(value);
    }

    public bool HasAlbumArt
    {
        get => GetProperty<bool>();
        set => SetProperty(value);
    }

    public UIImage AlbumArtSmall
    {
        get => GetProperty<UIImage>();
        set => SetProperty(value);
    }

    public UIImage AlbumArtLarge
    {
        get => GetProperty<UIImage>();
        set => SetProperty(value);
    }

    public UIImage AlbumArtSuperLarge
    {
        get => GetProperty<UIImage>();
        set => SetProperty(value);
    }

    public string ThumbnailPath
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public DateTime ReleaseDate
    {
        get => GetProperty<DateTime>();
        set => SetProperty(value);
    }

    public DateTime DateAdded
    {
        get => GetProperty<DateTime>();
        set => SetProperty(value);
    }

    public int ContributingArtistCount
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int DisplayArtistCount
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public DateTime DateLastPlayed
    {
        get => GetProperty<DateTime>();
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