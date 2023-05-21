using Microsoft.Iris.Markup;

namespace Microsoft.Zune.Schemas;

public class RankedArtist : Artist
{
    public RankedArtist(MarkupTypeSchema schema) : base(schema)
    {
    }

    public int GenreId
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

}