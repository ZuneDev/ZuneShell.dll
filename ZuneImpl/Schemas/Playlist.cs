using Microsoft.Iris.Markup;
using System;

namespace Microsoft.Zune.Schemas;

public class Playlist : SubscriptionSeries
{
    public Playlist(MarkupTypeSchema schema) : base(schema)
    {
    }

    public int PlaylistType
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int Count
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public TimeSpan TotalTime
    {
        get => GetProperty<TimeSpan>();
        set => SetProperty(value);
    }

    public int Status
    {
        get => GetProperty<int>();
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

    public DateTime DateLastPlayed
    {
        get => GetProperty<DateTime>();
        set => SetProperty(value);
    }

    public string PublishInterval
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string Genre
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string Category
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public long MaxChannelSize
    {
        get => GetProperty<long>();
        set => SetProperty(value);
    }

    public bool AutoRefresh
    {
        get => GetProperty<bool>();
        set => SetProperty(value);
    }

    public int AutoRefreshFreq
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int LimitType
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int LimitValue
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int SubType
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

}