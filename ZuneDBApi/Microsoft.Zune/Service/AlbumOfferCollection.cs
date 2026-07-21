using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Microsoft.Zune.Service;

public class AlbumOfferCollection : OfferCollection, IDisposable
{
    private static readonly StrategyBasedComWrappers s_comWrappers = new();

    private IList m_items;
    private IMusicAlbumCollection? m_pCollection;

    public IList Items => m_items;

    internal AlbumOfferCollection()
    {
        m_items = new ArrayList();
    }

    // Matches the original Init(IMusicAlbumCollection*, IDictionary)'s decompiled body
    // (ILSpy decompilation of Microsoft.Zune.Service.AlbumOfferCollection in
    // ZuneShell/lib/ZuneDBApi.dll) — see logs/Microsoft.Zune/Service/OfferCollection.md
    // for the full vtable-offset recovery. `pCollection` is the raw native
    // IMusicAlbumCollection* the caller received (from the not-yet-reverse-engineered
    // Service backend — logs/Microsoft.Zune/Service/Service.md); `internal` methods
    // aren't part of the original's public surface so the parameter type was changed
    // from a raw pointer to `nint` per CLAUDE.md's COM-objects rule.
    internal int Init(nint pCollectionPtr, IDictionary mapIdToContext)
    {
        var pCollection = (IMusicAlbumCollection)s_comWrappers.GetOrCreateObjectForComInstance(pCollectionPtr, CreateObjectFlags.None);

        int hr = 0;
        int count = pCollection.GetCount();
        var list = new ArrayList(count);

        for (int i = 0; i < count && hr >= 0; i++)
        {
            hr = pCollection.GetItem(i, out MusicAlbumMetadata metadata, out IContextData? context);
            if (hr < 0)
                break;

            IMediaRights? rights = metadata.MediaRightsPtr != 0
                ? (IMediaRights)s_comWrappers.GetOrCreateObjectForComInstance(metadata.MediaRightsPtr, CreateObjectFlags.None)
                : null;

            int releaseYear = 0;
            IPriceInfo? priceInfo = null;
            bool inCollection = false;
            bool previouslyPurchased = false;
            bool isMP = false;

            if (rights != null)
            {
                inCollection = rights.IsInCollection() != 0;
                previouslyPurchased = rights.GetPreviouslyPurchased(1, 1) != 0;

                // Matches the original exactly: probe format 0 first (num6 != 0 => isMP),
                // then re-probe with format 1 if format 0 wasn't available before fetching
                // price info for whichever format actually has rights.
                int hasFormat0 = rights.HasRights((EMediaRights)4, (EMediaFormat)0);
                isMP = hasFormat0 != 0;
                EMediaFormat format = (hasFormat0 == 0) ? (EMediaFormat)1 : (EMediaFormat)0;
                if (rights.HasRights((EMediaRights)4, format) != 0)
                    rights.GetPriceInfo((EMediaRights)4, format, out priceInfo);
            }

            string releaseDateString = ReadAndFreeBSTR(metadata.ReleaseDatePtr);
            if (!string.IsNullOrEmpty(releaseDateString))
                int.TryParse(releaseDateString, out releaseYear);

            Guid id = metadata.Id;
            string title = ReadAndFreeBSTR(metadata.TitlePtr);
            string artist = ReadAndFreeBSTR(metadata.ArtistPtr);
            string genre = ReadAndFreeBSTR(metadata.GenrePtr);
            string coverArtUrl = ReadAndFreeBSTR(metadata.CoverArtUrlPtr);
            string recommendationContext = GetRecommendationContext(id, mapIdToContext, context);
            bool premium = metadata.Premium != 0;

            list.Add(new AlbumOffer(id, title, artist, genre, releaseYear, coverArtUrl,
                new PriceInfo(priceInfo), isMP, premium, previouslyPurchased, inCollection, recommendationContext));
        }

        if (hr >= 0)
        {
            m_items = list;
            m_pCollection = pCollection;
        }
        return hr;
    }

    // Equivalent of the original's SysFreeString(ptr) calls: the native GetItem
    // allocates each string field as a BSTR that the caller owns once GetItem returns.
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

    ~AlbumOfferCollection()
    {
        Dispose(false);
    }
}
