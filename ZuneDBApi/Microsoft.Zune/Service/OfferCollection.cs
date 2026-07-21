using System;
using System.Collections;

namespace Microsoft.Zune.Service;

public abstract class OfferCollection
{
    // Original signature also accepted a native IContextData* to look up a
    // recommendation context string when the id wasn't already cached in
    // mapIdToContext. The native lookup path isn't reimplemented yet — see
    // logs/Microsoft.Zune/Service/OfferCollection.md.
    protected string GetRecommendationContext(Guid id, IDictionary mapIdToContext)
    {
        if (mapIdToContext != null && mapIdToContext.Contains(id))
        {
            return (string)mapIdToContext[id];
        }
        return null;
    }
}
