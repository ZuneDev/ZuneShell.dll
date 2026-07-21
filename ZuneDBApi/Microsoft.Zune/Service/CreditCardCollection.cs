using System;
using System.Collections;

namespace Microsoft.Zune.Service;

public class CreditCardCollection : IDisposable
{
    private IList m_items;

    public IList Items => m_items;

    internal CreditCardCollection()
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

    ~CreditCardCollection()
    {
        Dispose(false);
    }
}
