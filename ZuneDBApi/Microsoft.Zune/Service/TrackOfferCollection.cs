using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Microsoft.Zune.Service;

public class TrackOfferCollection : OfferCollection, IDisposable
{
    private static readonly StrategyBasedComWrappers s_comWrappers = new();

    private IList m_items;
    private IMusicTrackCollection? m_pCollection;

    public IList Items => m_items;

    internal TrackOfferCollection()
    {
        m_items = new ArrayList();
    }

    // Matches the original Init(IMusicTrackCollection*, IDictionary)'s decompiled body
    // (ILSpy decompilation of Microsoft.Zune.Service.TrackOfferCollection in
    // ZuneShell/lib/ZuneDBApi.dll) — see logs/Microsoft.Zune/Service/OfferCollection.md.
    // Rights tier 5 ("subscription-free", per IMediaRights.IsSubscriptionFree) skips
    // the price-info probe entirely, unlike the tier-3 ("owned") path Album also uses.
    internal int Init(nint pCollectionPtr, IDictionary mapIdToContext)
    {
        var pCollection = (IMusicTrackCollection)s_comWrappers.GetOrCreateObjectForComInstance(pCollectionPtr, CreateObjectFlags.None);

        int hr = 0;
        int count = pCollection.GetCount();
        var list = new ArrayList(count);

        for (int i = 0; i < count && hr >= 0; i++)
        {
            hr = pCollection.GetItem(i, out MusicTrackMetadata metadata, out IContextData? context);
            if (hr < 0)
                break;

            IMediaRights? rights = metadata.MediaRightsPtr != 0
                ? (IMediaRights)s_comWrappers.GetOrCreateObjectForComInstance(metadata.MediaRightsPtr, CreateObjectFlags.None)
                : null;

            bool inCollection = false, previouslyPurchased = false, isMP = false, subscriptionFree = false;
            IPriceInfo? priceInfo = null;

            if (rights != null)
            {
                var rightsTier = rights.IsSubscriptionFree() != 0 ? (EMediaRights)5 : (EMediaRights)3;
                inCollection = rights.IsInCollection() != 0;
                previouslyPurchased = rights.GetPreviouslyPurchased(1, 1) != 0;

                int hasFormat0 = rights.HasRights(rightsTier, (EMediaFormat)0);
                isMP = hasFormat0 != 0;

                if (rightsTier != (EMediaRights)5)
                {
                    EMediaFormat format = (hasFormat0 == 0) ? (EMediaFormat)1 : (EMediaFormat)0;
                    if (rights.HasRights(rightsTier, format) != 0)
                        rights.GetPriceInfo(rightsTier, format, out priceInfo);
                }

                subscriptionFree = rights.IsSubscriptionFree() != 0;
            }

            Guid id = metadata.Id;
            string recommendationContext = GetRecommendationContext(id, mapIdToContext, context);
            string title = ReadAndFreeBSTR(metadata.TitlePtr);
            string album = ReadAndFreeBSTR(metadata.AlbumPtr);
            string artist = ReadAndFreeBSTR(metadata.ArtistPtr);

            list.Add(new TrackOffer(id, metadata.TrackNumber, title, album, artist, recommendationContext,
                new PriceInfo(priceInfo), isMP, previouslyPurchased, inCollection, subscriptionFree));
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

    ~TrackOfferCollection()
    {
        Dispose(false);
    }
}
