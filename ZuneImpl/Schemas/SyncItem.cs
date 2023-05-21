using Microsoft.Iris.Markup;
using System;

namespace Microsoft.Zune.Schemas;

public class SyncItem : MarkupDataType
{
    public SyncItem(MarkupTypeSchema schema) : base(schema)
    {
    }

    public int LibraryId
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int MediaType
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public string Title
    {
        get => GetProperty<string>();
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

    public string Folder
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string Series
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public DateTime DateAdded
    {
        get => GetProperty<DateTime>();
        set => SetProperty(value);
    }

    public DateTime DateTaken
    {
        get => GetProperty<DateTime>();
        set => SetProperty(value);
    }

    public int Error
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int MappedError
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public TimeSpan Duration
    {
        get => GetProperty<TimeSpan>();
        set => SetProperty(value);
    }

    public int PlaylistType
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

}