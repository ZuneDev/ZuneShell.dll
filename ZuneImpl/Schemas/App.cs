using Microsoft.Iris.Drawing;
using Microsoft.Iris.Markup;
using System;

namespace Microsoft.Zune.Schemas;

public class App : Media
{
    public App(MarkupTypeSchema schema) : base(schema)
    {
    }

    public UIImage Thumbnail
    {
        get => GetProperty<UIImage>();
        set => SetProperty(value);
    }

    public int CategoryId
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public Guid ZuneMediaId
    {
        get => GetProperty<Guid>();
        set => SetProperty(value);
    }

    public string Description
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string Version
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string Author
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string Genre
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public DateTime DateAdded
    {
        get => GetProperty<DateTime>();
        set => SetProperty(value);
    }

    public DateTime ReleaseDate
    {
        get => GetProperty<DateTime>();
        set => SetProperty(value);
    }

    public string ParentalRating
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public DateTime DateModified
    {
        get => GetProperty<DateTime>();
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

    public string FilePath
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public long FileSize
    {
        get => GetProperty<long>();
        set => SetProperty(value);
    }

}