using Microsoft.Iris.Markup;
using System;

namespace Microsoft.Zune.Schemas;

public class SubscriptionSeries : Media
{
    public SubscriptionSeries(MarkupTypeSchema schema) : base(schema)
    {
    }

    public string ArtUrl
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string Description
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public int SeriesState
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int ErrorCode
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int NumberOfEpisodes
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public Guid ZuneMediaId
    {
        get => GetProperty<Guid>();
        set => SetProperty(value);
    }

    public string FeedUrl
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

}