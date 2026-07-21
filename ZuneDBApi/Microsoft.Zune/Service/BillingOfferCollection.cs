using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Microsoft.Zune.Service;

public class BillingOfferCollection : IDisposable
{
    private static readonly StrategyBasedComWrappers s_comWrappers = new();

    private IList m_items;
    private IBillingOfferCollection? m_pCollection;

    public IList Items => m_items;

    internal BillingOfferCollection()
    {
        m_items = new ArrayList();
    }

    // Matches the original Init(IBillingOfferCollection*)'s decompiled body (ILSpy
    // decompilation of Microsoft.Zune.Service.BillingOfferCollection in
    // ZuneShell/lib/ZuneDBApi.dll) — see logs/Microsoft.Zune/Service/OfferCollection.md.
    internal int Init(nint pCollectionPtr)
    {
        var pCollection = (IBillingOfferCollection)s_comWrappers.GetOrCreateObjectForComInstance(pCollectionPtr, CreateObjectFlags.None);

        int hr = 0;
        int count = pCollection.GetCount();
        var list = new ArrayList(count);

        for (int i = 0; i < count && hr >= 0; i++)
        {
            hr = pCollection.GetItem(i, out ulong id, out EBillingOfferType offerType, out string offerName,
                out string displayPrice, out uint points, out float price, out int taxesIncluded, out int isTrial);
            if (hr < 0)
                break;

            list.Add(new BillingOffer(id, offerType, offerName, displayPrice, points, price,
                taxesIncluded != 0, isTrial != 0));
        }

        if (hr >= 0)
        {
            m_items = list;
            m_pCollection = pCollection;
        }
        return hr;
    }

    protected virtual void Dispose(bool disposing)
    {
        m_pCollection = null;
        m_items = null;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~BillingOfferCollection()
    {
        Dispose(false);
    }
}
