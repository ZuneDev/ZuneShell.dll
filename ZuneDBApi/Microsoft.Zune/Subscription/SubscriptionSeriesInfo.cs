using System;
using Microsoft.Iris;

namespace Microsoft.Zune.Subscription;

// Original reads podcast series metadata out of a native IMSMediaSchemaPropertySet*
// via a hardcoded PROPERTY_TO_PID_MAP lookup table (10 entries: LibraryId, ErrorCode,
// Title, HomeUrl, ArtUrl, Description, Copyright, Author, OwnerName). Not reverse
// engineered — see logs/Microsoft.Zune/Subscription/SubscriptionManager.md. GetProperty
// falls back to the DataProviderObject mapping default, same as the original's
// no-native-property-set path.
public class SubscriptionSeriesInfo : DataProviderObject
{
    private string m_serviceId;
    private ESubscriptionState m_eSubscriptionState;

    internal SubscriptionSeriesInfo(DataProviderQuery owner, object typeCookie, string serviceId)
        : base(owner, typeCookie)
    {
        m_serviceId = serviceId;
    }

    public override object GetProperty(string propertyName)
    {
        return Mappings[propertyName].DefaultValue;
    }

    public override void SetProperty(string propertyName, object value)
    {
        throw new NotSupportedException();
    }
}
