using System;
using System.Collections;
using Microsoft.Zune.Util;
using ZuneUI;

namespace Microsoft.Zune.Service;

// Original wraps a single native COM pointer (IService*) behind ~90 methods. That
// interface has not been reverse engineered (it requires Ghidra analysis of the native
// commerce/account/DRM service layer, which is well beyond what ILSpy can recover from
// this mixed-mode assembly) — see logs/Microsoft.Zune/Service/Service.md. Every member
// below is a "pure no-op stub" per CLAUDE.md's stage 2 convention: signatures are
// reconstructed in full (matching ZuneImpl's IService.cs, which was already written
// against the original API), but bodies return defaults/success rather than talking to
// a real service backend.
public class Service : IDisposable
{
    private static Service m_singletonInstance;

    public static Service Instance => m_singletonInstance ??= new Service();

    private Service()
    {
    }

    public static string GetEndPointUri(EServiceEndpointId eServiceEndpointId) => null;

    public static EListType ContentTypeToListType(EContentType contentType) => EListType.eListInvalid;

    public int Phase3Initialize() => HRESULT._S_OK.Int;

    public int InitializeWMISEndpointCollection() => HRESULT._S_OK.Int;

    public string GetWMISEndPointUri(string strEndPointName) => null;

    public void SignIn(string strUsername, string strPassword, bool fRememberUsername, bool fRememberPassword, bool fAutomaticallySignInAtStartup, AsyncCompleteHandler eventHandler)
    {
        eventHandler?.Invoke(HRESULT._E_FAIL);
    }

    public void RefreshAccount(AsyncCompleteHandler eventHandler)
    {
        eventHandler?.Invoke(HRESULT._E_FAIL);
    }

    public bool IsSigningIn() => false;

    public bool IsSignedIn() => false;

    public bool IsSignedInWithSubscription() => false;

    public bool BlockExplicitContent() => false;

    public bool BlockRatedContent(string system, string rating) => false;

    public bool CanSignedInUserPostUsageData() => false;

    public string GetSignedInUsername() => null;

    public uint GetSignedInGeoId() => 0;

    public void CancelSignIn()
    {
    }

    public void SignOut()
    {
    }

    public IList GetPersistedUsernames() => new ArrayList();

    public void RemovePersistedUsername(string strUsername)
    {
    }

    public bool SignInPasswordRequired(string strUsername) => false;

    public bool SignInAtStartup(string strUsername) => false;

    public string GetSignInAtStartupUsername() => null;

    public bool CanDownloadSubscriptionContent() => false;

    public void GetLastSignedInUserSubscriptionState(out bool activeSubscription, out ulong subscriptionId)
    {
        activeSubscription = false;
        subscriptionId = 0;
    }

    public bool GetLastSignedInUserGuid(out int iUserId, out Guid guidUserGuid)
    {
        iUserId = 0;
        guidUserGuid = Guid.Empty;
        return false;
    }

    public bool ClearLastSignedInUser() => false;

    public bool SetLastSignedInUserGuid(ref Guid guidUserGuid, out int iUserId)
    {
        iUserId = 0;
        return false;
    }

    public string GetXboxPuid() => null;

    public void RegisterForDownloadNotification(DownloadEventHandler eventHandler, DownloadEventProgressHandler progressHandler, EventHandler allPendingHandler)
    {
    }

    public void Download(IList items, EDownloadFlags eDownloadFlags, string deviceEndpointId, EDownloadContextEvent clientContextEvent, string clientContextEventData, DownloadEventHandler eventHandler, DownloadEventProgressHandler progressHandler, EventHandler allPendingHandler)
    {
    }

    public bool IsDownloading(Guid guidMediaId, EContentType eContentType, out bool fIsDownloadPending, out bool fIsHidden)
    {
        fIsDownloadPending = false;
        fIsHidden = false;
        return false;
    }

    public void CancelDownload(Guid guidMediaId, EContentType eContentType)
    {
    }

    public HRESULT GetContentUri(Guid guidMediaId, EContentType eContentType, EContentUriFlags eContentUriFlags, EMediaFormat eMediaFormat, EMediaRights eMediaRights, out string uriOut, out Guid mediaInstanceIdOut)
    {
        uriOut = null;
        mediaInstanceIdOut = Guid.Empty;
        return HRESULT._E_FAIL;
    }

    public HRESULT GetContentUri(Guid guidMediaId, EContentType eContentType, EContentUriFlags eContentUriFlags, bool fIsHD, bool fIsRental, out string uriOut)
    {
        uriOut = null;
        return HRESULT._E_FAIL;
    }

    public HRESULT GetContentUri(Guid guidMediaId, EContentType eContentType, EContentUriFlags eContentUriFlags, out string uriOut, out Guid mediaInstanceIdOut)
    {
        uriOut = null;
        mediaInstanceIdOut = Guid.Empty;
        return HRESULT._E_FAIL;
    }

    public bool InCompleteCollection(Guid guidMediaId, EContentType eContentType) => false;

    public bool InCompleteCollection(Guid guidMediaId, EContentType eContentType, string strDeviceEndpointId) => false;

    public bool InCompleteCollection(Guid guidMediaId, EContentType eContentType, out int dbMediaId, out bool fHidden)
    {
        dbMediaId = 0;
        fHidden = false;
        return false;
    }

    public bool InCompleteCollection(Guid guidMediaId, EContentType eContentType, string strDeviceEndpointId, out int dbMediaId, out bool fHidden)
    {
        dbMediaId = 0;
        fHidden = false;
        return false;
    }

    public bool InVisibleCollection(Guid guidMediaId, EContentType eContentType) => false;

    public bool InVisibleCollection(Guid guidMediaId, EContentType eContentType, out int dbMediaId)
    {
        dbMediaId = 0;
        return false;
    }

    public bool InHiddenCollection(Guid guidMediaId, EContentType eContentType) => false;

    public bool SetUserTrackRating(int iUserId, int iRating, Guid guidTrackMediaId, Guid guidAlbumMediaId, int iTrackNumber, string strTitle, int msDuration, string strAlbum, string strArtist, string strGenre, string strServiceContext) => false;

    public bool SetUserArtistRating(int iUserId, int iRating, Guid guidArtistMediaId, string strTitle) => false;

    public bool GetUserRating(int iUserId, Guid guidMediaId, EContentType eContentType, ref int piRating) => false;

    public bool DeleteSubscriptionDownloads(AsyncCompleteHandler eventHandler)
    {
        eventHandler?.Invoke(HRESULT._E_FAIL);
        return false;
    }

    public string GetSubscriptionDirectory() => null;

    public EMediaStatus GetMediaStatus(Guid guidMediaId, EContentType eContentType) => default;

    public string GetZuneTag() => null;

    public string GetPassportTicket(EPassportPolicyId ePassportPolicy) => null;

    public HRESULT AuthenticatePassport(string username, string password, EPassportPolicyId ePassportPolicyId, out PassportIdentity passportIdentity)
    {
        passportIdentity = null;
        return HRESULT._E_FAIL;
    }

    public string GetXboxTicket() => null;

    public ulong GetPassportPuid() => 0;

    public ValueType GetUserGuid() => Guid.Empty;

    public string GetLocale() => null;

    public bool HasSignInLabelTakedown() => false;

    public bool HasSignInBillingViolation() => false;

    public int GetPointsBalance() => 0;

    public void GetBalances(GetBalancesCompleteCallback completeCallback, GetBalancesErrorCallback errorCallback)
    {
        errorCallback?.Invoke(HRESULT._E_FAIL);
    }

    public int GetSubscriptionFreeTrackBalance() => 0;

    public HRESULT GetOfferDetails(Guid offerId, GetOfferDetailsCompleteCallback completeCallback, GetOfferDetailsErrorCallback errorCallback, object state)
    {
        errorCallback?.Invoke(HRESULT._E_FAIL, state);
        return HRESULT._E_FAIL;
    }

    public void GetOffers(IList albumGuids, IList trackGuids, IList videoGuids, IList appGuids, IDictionary mapIdToContext, EGetOffersFlags eGetOffersFlags, string deviceEndpointId, GetOffersCompleteCallback completeCallback, GetOffersErrorCallback errorCallback)
    {
        errorCallback?.Invoke(HRESULT._E_FAIL);
    }

    public void PurchaseOffers(PaymentInstrument payment, AlbumOfferCollection albumOffers, TrackOfferCollection trackOffers, VideoOfferCollection videoOffers, AppOfferCollection appOffers, EPurchaseOffersFlags ePurchaseOffersFlags, PurchaseOffersCompleteHandler purchaseOffersHandler)
    {
        purchaseOffersHandler?.Invoke(HRESULT._E_FAIL, null, null);
    }

    public bool ReportFavouriteArtists(Guid userId, IList artists, AsyncCompleteHandler callback)
    {
        callback?.Invoke(HRESULT._E_FAIL);
        return false;
    }

    public int VerifyToken(string token, out TokenDetails tokenDetails)
    {
        tokenDetails = null;
        return HRESULT._E_FAIL.Int;
    }

    public bool ReportAConcern(EConcernType concernType, EContentType contentType, Guid mediaId, string message, AsyncCompleteHandler callback)
    {
        callback?.Invoke(HRESULT._E_FAIL);
        return false;
    }

    public bool PostAppReview(Guid mediaId, string title, string comment, int rating, AsyncCompleteHandler callback)
    {
        callback?.Invoke(HRESULT._E_FAIL);
        return false;
    }

    public bool LaunchBrowserForExternalUrl(string strUrl, EPassportPolicyId ePassportPolicy) => false;

    public bool GetAlbumIdFromCompId(string compId, out Guid guidAlbum)
    {
        guidAlbum = Guid.Empty;
        return false;
    }

    public bool GetMusicVideoIdFromCompId(string compId, out Guid guidMusicVideo)
    {
        guidMusicVideo = Guid.Empty;
        return false;
    }

    public DRMInfo GetFileDRMInfo(string filePath) => null;

    public DRMInfo GetMediaDRMInfo(Guid mediaId, EContentType eContentType) => null;

    public ulong GetSubscriptionOfferId() => 0;

    public ulong GetSubscriptionRenewalOfferId() => 0;

    public DateTime GetSubscriptionEndDate() => DateTime.MinValue;

    public DateTime GetSubscriptionFreeTrackExpiration() => DateTime.MinValue;

    public bool SubscriptionPendingCancel() => false;

    public bool IsParentallyControlled() => false;

    public bool IsLightWeight() => false;

    public void GetPaymentInstruments(GetPaymentInstrumentsCompleteCallback completeCallback, GetPaymentInstrumentsErrorCallback errorCallback)
    {
        errorCallback?.Invoke(HRESULT._E_FAIL);
    }

    public int AddPaymentInstrument(PaymentInstrument paymentInstrument, AddPaymentInstrumentCompleteCallback completeCallback, AddPaymentInstrumentErrorCallback errorCallback)
    {
        errorCallback?.Invoke(HRESULT._E_FAIL);
        return HRESULT._E_FAIL.Int;
    }

    public int AddPaymentInstrument(PaymentInstrument paymentInstrument, out string paymentId, out ServiceError serviceError)
    {
        paymentId = null;
        serviceError = null;
        return HRESULT._E_FAIL.Int;
    }

    public void GetSubscriptionOffers(GetBillingOffersCompleteCallback completeCallback, GetBillingOffersErrorCallback errorCallback)
    {
        errorCallback?.Invoke(HRESULT._E_FAIL);
    }

    public void GetSubscriptionDetails(ulong offerId, GetBillingOffersCompleteCallback completeCallback, GetBillingOffersErrorCallback errorCallback)
    {
        errorCallback?.Invoke(HRESULT._E_FAIL);
    }

    public void GetPointsOffers(GetBillingOffersCompleteCallback completeCallback, GetBillingOffersErrorCallback errorCallback)
    {
        errorCallback?.Invoke(HRESULT._E_FAIL);
    }

    public int PurchaseBillingOffer(BillingOffer offer, PaymentInstrument paymentInstrument, AsyncCompleteHandler callback)
    {
        callback?.Invoke(HRESULT._E_FAIL);
        return HRESULT._E_FAIL.Int;
    }

    public int PurchaseBillingOffer(BillingOffer offer, PaymentInstrument paymentInstrument) => HRESULT._E_FAIL.Int;

    public string GetMachineId() => null;

    public int ResumePurchase(string resumeHandle, string authorizationToken, AsyncCompleteHandler callback)
    {
        callback?.Invoke(HRESULT._E_FAIL);
        return HRESULT._E_FAIL.Int;
    }

    public CountryBaseDetails[] GetCountryDetails() => Array.Empty<CountryBaseDetails>();

    public RatingSystemBase[] GetRatingSystems() => Array.Empty<RatingSystemBase>();

    public int GetRentalTermDays(string strStudio) => 0;

    public int GetRentalTermHours(string strStudio) => 0;

    public string GetPhoneClientType(string strPhoneOsVersion) => null;

    public int GetSubscriptionTrialDuration() => 0;

    public AppOfferCollection CreateEmptyAppCollection() => new();

    public EContentType GetContentType(string contentTypeStr) => default;

    public void ReportStreamingAction(EStreamingActionType eStreamingActionType, Guid guidMediaInstanceId, AsyncCompleteHandler eventHandler)
    {
        eventHandler?.Invoke(HRESULT._E_FAIL);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
