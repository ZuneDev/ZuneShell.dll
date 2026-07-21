using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Microsoft.Zune.Service;

public class AppOfferCollection : OfferCollection, IDisposable
{
    private static readonly StrategyBasedComWrappers s_comWrappers = new();

    private IList m_items;
    private IAppCollection? m_pCollection;

    public IList Items => m_items;

    internal AppOfferCollection()
    {
        m_items = new ArrayList();
    }

    // Matches the original Init(IAppCollection*)'s decompiled body (ILSpy decompilation
    // of Microsoft.Zune.Service.AppOfferCollection in ZuneShell/lib/ZuneDBApi.dll) — see
    // logs/Microsoft.Zune/Service/OfferCollection.md. Notably tries EMediaRights 3
    // ("Subscription", per the shared numbering with Album/Track) and 11 ("Trial")
    // for each app in turn, adding one AppOffer per rights tier that GetPriceInfo2
    // succeeds for — an app can therefore appear as both a subscription and a trial
    // offer, exactly as the original does.
    internal unsafe int Init(nint pCollectionPtr)
    {
        var pCollection = (IAppCollection)s_comWrappers.GetOrCreateObjectForComInstance(pCollectionPtr, CreateObjectFlags.None);

        int hr = 0;
        int count = pCollection.GetCount();
        var list = new ArrayList(count);
        Span<int> rightsTiers = [3, 11];

        for (int i = 0; i < count && hr >= 0; i++)
        {
            hr = pCollection.GetItem(i, out AppMetadata metadata, out _);
            if (hr < 0)
                break;

            if (!DateTime.TryParse(ReadAndFreeBSTR(metadata.ReleaseDatePtr), out DateTime releaseDateTime))
                releaseDateTime = DateTime.MinValue;

            Guid id = metadata.Id;
            string title = ReadAndFreeBSTR(metadata.TitlePtr);
            string publisher = ReadAndFreeBSTR(metadata.PublisherPtr);
            string developer = ReadAndFreeBSTR(metadata.DeveloperPtr);
            string genre = ReadAndFreeBSTR(metadata.GenrePtr);
            string version = ReadAndFreeBSTR(metadata.VersionPtr);
            string previewImageUrl = ReadAndFreeBSTR(metadata.PreviewImageUrlPtr);
            string ratingImageUrl = ReadAndFreeBSTR(metadata.RatingImageUrlPtr);

            IMediaRights? rights = metadata.MediaRightsPtr != 0
                ? (IMediaRights)s_comWrappers.GetOrCreateObjectForComInstance(metadata.MediaRightsPtr, CreateObjectFlags.None)
                : null;
            if (rights is null)
                continue;

            bool inCollection = rights.IsInCollection() != 0;

            foreach (int rightsValue in rightsTiers)
            {
                var rightsTier = (EMediaRights)rightsValue;
                Guid guidNull1 = Guid.Empty;
                Guid guidNull2 = Guid.Empty;
                if (rights.GetPriceInfo2(rightsTier, (EMediaFormat)5, &guidNull2, &guidNull1, out IPriceInfo? priceInfo, out _) < 0)
                    continue;

                bool isTrialPurchase = rightsValue == 11;
                bool previouslyPurchased = rights.GetPreviouslyPurchased2(rightsTier, (EMediaFormat)5, 1, 1) != 0;

                list.Add(new AppOffer(id, title, publisher, developer, genre, version, previewImageUrl, ratingImageUrl,
                    new PriceInfo(priceInfo), releaseDateTime, previouslyPurchased, inCollection, isTrialPurchase));
            }
        }

        if (hr >= 0)
        {
            m_items = list;
            m_pCollection = pCollection;
        }
        return hr;
    }

    private static string ReadAndFreeBSTR(nint ptr)
    {
        if (ptr == 0)
            return null;
        string value = Marshal.PtrToStringUni(ptr);
        Marshal.FreeBSTR(ptr);
        return value;
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

    ~AppOfferCollection()
    {
        Dispose(false);
    }
}
