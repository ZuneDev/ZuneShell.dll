using Microsoft.Iris.Markup;

namespace Microsoft.Zune.Schemas;

public class PodcastSeries : SubscriptionSeries
{
    public PodcastSeries(MarkupTypeSchema schema) : base(schema)
    {
    }

    public string HomeUrl
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public bool Explicit
    {
        get => GetProperty<bool>();
        set => SetProperty(value);
    }

    public string Copyright
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string Author
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string OwnerName
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public bool HasUnplayedItems
    {
        get => GetProperty<bool>();
        set => SetProperty(value);
    }

}