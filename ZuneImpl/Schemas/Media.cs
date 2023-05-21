using Microsoft.Iris.Markup;

namespace Microsoft.Zune.Schemas;

public class Media : MarkupDataType
{
    public Media(MarkupTypeSchema schema) : base(schema)
    {
    }

    public int LibraryId
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public string Title
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public int SyncState
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public string Type
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public long DeviceFileSize
    {
        get => GetProperty<long>();
        set => SetProperty(value);
    }

    public string Copyright
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

}