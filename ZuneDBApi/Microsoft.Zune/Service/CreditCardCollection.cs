using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Microsoft.Zune.Service;

public class CreditCardCollection : IDisposable
{
    private static readonly StrategyBasedComWrappers s_comWrappers = new();

    private IList m_items;
    private ICreditCardCollection? m_pCollection;

    public IList Items => m_items;

    internal CreditCardCollection()
    {
        m_items = new ArrayList();
    }

    // Matches the original Init(ICreditCardCollection*)'s decompiled body (ILSpy
    // decompilation of Microsoft.Zune.Service.CreditCardCollection in
    // ZuneShell/lib/ZuneDBApi.dll) — see logs/Microsoft.Zune/Service/OfferCollection.md.
    // Faithfully reproduces one quirk of the original: an entry whose expiration date
    // string fails to parse is silently skipped rather than added with a default date.
    internal int Init(nint pCollectionPtr)
    {
        var pCollection = (ICreditCardCollection)s_comWrappers.GetOrCreateObjectForComInstance(pCollectionPtr, CreateObjectFlags.None);

        int hr = 0;
        int count = pCollection.GetCount();
        var list = new ArrayList(count);

        for (int i = 0; i < count && hr >= 0; i++)
        {
            hr = pCollection.GetItem(i, out _, out string id, out string street1, out string street2,
                out string city, out string district, out string state, out string postalCode,
                out string phonePrefix, out string phoneNumber, out string phoneExtension,
                out ECreditCardType creditCardType, out string accountHolderName, out string accountNumber,
                out string ccvNumber, out string expirationDateString);
            if (hr < 0)
                break;

            if (!DateTime.TryParse(expirationDateString, out DateTime expirationDate))
                continue;

            var address = new Address(street1, street2, city, district, state, postalCode);
            list.Add(new CreditCard(id, address, (CreditCardType)creditCardType, accountHolderName, accountNumber,
                ccvNumber, expirationDate, phonePrefix, phoneNumber, phoneExtension));
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

    ~CreditCardCollection()
    {
        Dispose(false);
    }
}
