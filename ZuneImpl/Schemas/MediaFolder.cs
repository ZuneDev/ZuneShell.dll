using Microsoft.Iris.Drawing;
using Microsoft.Iris.Markup;
using System;

namespace Microsoft.Zune.Schemas;

public class MediaFolder : Media
{
    public MediaFolder(MarkupTypeSchema schema) : base(schema)
    {
    }

    public int ParentId
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public string FolderPath
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public int Count
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public bool HasChildFolders
    {
        get => GetProperty<bool>();
        set => SetProperty(value);
    }

    public int TotalCount
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public UIImage Thumbnail
    {
        get => GetProperty<UIImage>();
        set => SetProperty(value);
    }

    public DateTime LastPlayed
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