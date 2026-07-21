using System;
using System.Collections;

namespace Microsoft.Zune.Service;

public abstract class OfferCollection
{
    // Restored to the original 3-arg shape now that IContextData has been reverse
    // engineered (see logs/Microsoft.Zune/Service/OfferCollection.md): a single
    // GetContextString(out BSTR) call on vtable slot 3, matched exactly against the
    // decompiled original (which falls back to pContextData only when the id isn't
    // already cached in mapIdToContext, and only trusts a non-empty result).
    private protected string GetRecommendationContext(Guid id, IDictionary mapIdToContext, IContextData? contextData)
    {
        if (mapIdToContext != null && mapIdToContext.Contains(id))
        {
            return (string)mapIdToContext[id];
        }
        if (contextData != null && contextData.GetContextString(out string value) >= 0 && !string.IsNullOrEmpty(value))
        {
            return value;
        }
        return null;
    }
}
