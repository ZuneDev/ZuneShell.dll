using System;
using System.Collections;

namespace Microsoft.Zune.Service;

public class BillingOfferCollection : IDisposable
{
    private IList m_items;

    public IList Items => m_items;

    internal BillingOfferCollection()
    {
        m_items = new ArrayList();
    }

    protected virtual void Dispose(bool disposing)
    {
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
