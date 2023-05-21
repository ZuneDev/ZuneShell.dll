using Microsoft.Iris.Markup;
using System;

namespace Microsoft.Zune.Schemas;

public class SubscriptionEpisode : Media
{
    public SubscriptionEpisode(MarkupTypeSchema schema) : base(schema)
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

    public DateTime ReleaseDate
    {
        get => GetProperty<DateTime>();
        set => SetProperty(value);
    }

    public string Author
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string SourceUrl
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string EnclosureUrl
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public int EpisodeMediaType
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int DownloadType
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int DownloadState
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int DownloadErrorCode
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int SeriesId
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public string SeriesTitle
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string SeriesFeedUrl
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

}