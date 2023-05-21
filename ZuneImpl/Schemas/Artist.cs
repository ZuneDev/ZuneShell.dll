using Microsoft.Iris.Drawing;
using Microsoft.Iris.Markup;
using System;

namespace Microsoft.Zune.Schemas;

public class Artist : RateableMedia
{
    public Artist(MarkupTypeSchema schema) : base(schema)
    {
    }

    public Guid ZuneMediaId
    {
        get => GetProperty<Guid>();
        set => SetProperty(value);
    }

    public int NumberOfAlbums
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

    public UIImage Image
    {
        get => GetProperty<UIImage>();
        set => SetProperty(value);
    }

}