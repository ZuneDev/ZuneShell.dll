using Microsoft.Iris.Markup;
using Microsoft.Iris.UI;
using System.Runtime.CompilerServices;

namespace Microsoft.Zune.Schemas;

public abstract class MarkupDataType
{
    private readonly Class _item;

    public MarkupDataType(MarkupTypeSchema schema)
    {
        _item = new Class(schema);
    }

    public Class Item => _item;

    protected void SetProperty<T>(T value, [CallerMemberName] string name = "") => _item.SetProperty(name, value);

    protected T GetProperty<T>([CallerMemberName] string name = "") => (T)_item.GetProperty(name);
}