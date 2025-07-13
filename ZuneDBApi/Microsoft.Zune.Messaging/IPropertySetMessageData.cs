using System;

namespace Microsoft.Zune.Messaging;

public interface IPropertySetMessageData : IDisposable
{
	unsafe int GetPropertySet(IMSMediaSchemaPropertySet** ppPropSet);
}
