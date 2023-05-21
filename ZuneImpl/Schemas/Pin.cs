using Microsoft.Iris.Markup;
using System;

namespace Microsoft.Zune.Schemas;

public class Pin : MarkupDataType
{
    public Pin(MarkupTypeSchema schema) : base(schema)
    {
    }

    public int PinId
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int PinType
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int Ordinal
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public string Description
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public int MediaId
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int MediaType
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public string ZuneMediaRef
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public int ZuneMediaType
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public int UserId
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public DateTime DateModified
    {
        get => GetProperty<DateTime>();
        set => SetProperty(value);
    }

}