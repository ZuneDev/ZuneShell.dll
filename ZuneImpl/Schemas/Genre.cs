using Microsoft.Iris.Markup;
using Microsoft.Iris.UI;
using System;

namespace Microsoft.Zune.Schemas;

public class Genre : Media
{
    public Genre(MarkupTypeSchema schema) : base(schema)
    {
    }

    public DateTime DateLastPlayed
    {
        get => GetProperty<DateTime>();
        set => SetProperty(value);
    }

}