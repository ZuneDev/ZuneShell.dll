using Microsoft.Iris.Markup;
using System;

namespace Microsoft.Zune.Schemas;

public class PodcastEpisode : SubscriptionEpisode
{
    public PodcastEpisode(MarkupTypeSchema schema) : base(schema)
    {
    }

    public bool Explicit
    {
        get => GetProperty<bool>();
        set => SetProperty(value);
    }

    public string SeriesHomeUrl
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public int PlayedStatus
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public long Bookmark
    {
        get => GetProperty<long>();
        set => SetProperty(value);
    }

    public long FileSize
    {
        get => GetProperty<long>();
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

    public string ArtUrl
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public DateTime DateLastPlayed
    {
        get => GetProperty<DateTime>();
        set => SetProperty(value);
    }

    public DateTime DateAdded
    {
        get => GetProperty<DateTime>();
        set => SetProperty(value);
    }

}