using Microsoft.Iris.Markup;

namespace Microsoft.Zune.Schemas;

public class RateableMedia : Media
{
    public RateableMedia(MarkupTypeSchema schema) : base(schema)
    {
    }

    public int UserRating
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

}