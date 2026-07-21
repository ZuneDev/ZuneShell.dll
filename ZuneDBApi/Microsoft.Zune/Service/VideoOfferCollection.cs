using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Microsoft.Zune.Service;

public class VideoOfferCollection : OfferCollection, IDisposable
{
    private static readonly StrategyBasedComWrappers s_comWrappers = new();

    private IList m_items;
    private IVideoCollection? m_pCollection;

    public IList Items => m_items;

    internal VideoOfferCollection()
    {
        m_items = new ArrayList();
    }

    // Partial reconstruction of the original Init(IVideoCollection*)'s decompiled body
    // (ILSpy decompilation of Microsoft.Zune.Service.VideoOfferCollection in
    // ZuneShell/lib/ZuneDBApi.dll) — see logs/Microsoft.Zune/Service/OfferCollection.md.
    // The base metadata read (title/artist/genre/season/episode/etc.) and the
    // IMediaRights vtable slots it calls are faithfully recovered and used here. The
    // original additionally walks a hardcoded 13-entry VideoOfferParams table
    // (EMediaRights/EMediaFormat combinations covering rental/HD/season-pass tiers)
    // to emit *multiple* VideoOffer entries per video with different isRental/isHD/
    // isSeasonPurchase flags — but ILSpy's decompilation of that loop is corrupted
    // (it self-reports "Incompatible stack types: I vs Ref", and several booleans are
    // read before any visible assignment), so that tier-selection algorithm could not
    // be verified rather than guessed, per CLAUDE.md's decompilation rules. This
    // reimplementation emits a single VideoOffer per item using rights tier 12
    // ("owned", by analogy with Album/Track's tier-4/3 "owned" probes — one of the two
    // literal values, 12 and 13, that VideoOfferParams' recovered table pairs with
    // format -1, the same "no format" sentinel Album/Track never use, so tier 13 was
    // not picked without further evidence).
    //
    // TODO: recover the real per-tier VideoOffer enumeration (rental/HD/season-pass)
    // via Ghidra once the corrupted control flow can be cross-checked against the
    // native binary.
    internal unsafe int Init(nint pCollectionPtr)
    {
        var pCollection = (IVideoCollection)s_comWrappers.GetOrCreateObjectForComInstance(pCollectionPtr, CreateObjectFlags.None);

        int hr = 0;
        int count = pCollection.GetCount();
        var list = new ArrayList(count);

        for (int i = 0; i < count && hr >= 0; i++)
        {
            hr = pCollection.GetItem(i, out VideoMetadata metadata, out _);
            if (hr < 0)
                break;

            Guid id = metadata.Id;
            Guid albumId = metadata.AlbumId;
            string title = ReadAndFreeBSTR(metadata.TitlePtr);
            string seriesTitle = ReadAndFreeBSTR(metadata.SeriesTitlePtr);
            string artist = ReadAndFreeBSTR(metadata.ArtistPtr);
            string genre = ReadAndFreeBSTR(metadata.GenrePtr);
            string previewImageUrl = ReadAndFreeBSTR(metadata.PreviewImageUrlPtr);
            string productionCompany = ReadAndFreeBSTR(metadata.ProductionCompanyPtr);
            bool isMusicVideo = metadata.IsMusicVideoFlag == 0;

            string releaseDateString = ReadAndFreeBSTR(metadata.ReleaseDatePtr);
            int releaseYear = 0;
            if (!string.IsNullOrEmpty(releaseDateString))
                int.TryParse(releaseDateString, out releaseYear);

            IMediaRights? rights = metadata.MediaRightsPtr != 0
                ? (IMediaRights)s_comWrappers.GetOrCreateObjectForComInstance(metadata.MediaRightsPtr, CreateObjectFlags.None)
                : null;

            bool inCollection = false;
            IPriceInfo? priceInfo = null;
            string expirationDate = null;
            const EMediaRights ownedTier = (EMediaRights)12;

            if (rights != null)
            {
                inCollection = rights.IsInCollection() != 0;

                Guid guidNull1 = Guid.Empty;
                Guid guidNull2 = Guid.Empty;
                if (rights.GetPriceInfo2(ownedTier, (EMediaFormat)2, &guidNull2, &guidNull1, out priceInfo, out string expiration) >= 0)
                    expirationDate = expiration;
            }

            list.Add(new VideoOffer(id, title, seriesTitle, metadata.SeasonNumber, metadata.EpisodeNumber, artist,
                albumId, genre, releaseYear, previewImageUrl, productionCompany, new PriceInfo(priceInfo),
                isHD: false, isRental: false, isStream: false, isMusicVideo, isSeasonPurchase: false,
                previouslyPurchased: false, inCollection, expirationDate));
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

    ~VideoOfferCollection()
    {
        Dispose(false);
    }
}
