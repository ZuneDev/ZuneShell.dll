﻿using Microsoft.Zune.Util;
using System;
using System.Collections;
using ZuneUI;

namespace Microsoft.Zune.Service
{
    public interface IService
    {
        int AddPaymentInstrument(PaymentInstrument paymentInstrument, AddPaymentInstrumentCompleteCallback completeCallback, AddPaymentInstrumentErrorCallback errorCallback);
        int AddPaymentInstrument(PaymentInstrument paymentInstrument, out string paymentId, out ServiceError serviceError);
        HRESULT AuthenticatePassport(string username, string password, EPassportPolicyId ePassportPolicyId, out PassportIdentity passportIdentity);
        bool BlockExplicitContent();
        bool BlockRatedContent(string system, string rating);
        void CancelDownload(Guid guidMediaId, EContentType eContentType);
        void CancelSignIn();
        bool CanDownloadSubscriptionContent();
        bool CanSignedInUserPostUsageData();
        bool ClearLastSignedInUser();
        AppOfferCollection CreateEmptyAppCollection();
        bool DeleteSubscriptionDownloads(AsyncCompleteHandler eventHandler);
        void Dispose();
        void Download(IList items, EDownloadFlags eDownloadFlags, string deviceEndpointId, EDownloadContextEvent clientContextEvent, string clientContextEventData, DownloadEventHandler eventHandler, DownloadEventProgressHandler progressHandler, EventHandler allPendingHandler);
        bool GetAlbumIdFromCompId(string compId, out Guid guidAlbum);
        void GetBalances(GetBalancesCompleteCallback completeCallback, GetBalancesErrorCallback errorCallback);
        EContentType GetContentType(string contentTypeStr);
        HRESULT GetContentUri(Guid guidMediaId, EContentType eContentType, EContentUriFlags eContentUriFlags, bool fIsHD, bool fIsRental, out string uriOut);
        HRESULT GetContentUri(Guid guidMediaId, EContentType eContentType, EContentUriFlags eContentUriFlags, EMediaFormat eMediaFormat, EMediaRights eMediaRights, out string uriOut, out Guid mediaInstanceIdOut);
        HRESULT GetContentUri(Guid guidMediaId, EContentType eContentType, EContentUriFlags eContentUriFlags, out string uriOut, out Guid mediaInstanceIdOut);
        CountryBaseDetails[] GetCountryDetails();
        string GetEndPointUri(EServiceEndpointId eServiceEndpointId);
        DRMInfo GetFileDRMInfo(string filePath);
        bool GetLastSignedInUserGuid(out int iUserId, out Guid guidUserGuid);
        void GetLastSignedInUserSubscriptionState(out bool activeSubscription, out ulong subscriptionId);
        string GetLocale();
        string GetMachineId();
        DRMInfo GetMediaDRMInfo(Guid mediaId, EContentType eContentType);
        EMediaStatus GetMediaStatus(Guid guidMediaId, EContentType eContentType);
        bool GetMusicVideoIdFromCompId(string compId, out Guid guidMusicVideo);
        HRESULT GetOfferDetails(Guid offerId, GetOfferDetailsCompleteCallback completeCallback, GetOfferDetailsErrorCallback errorCallback, object state);
        void GetOffers(IList albumGuids, IList trackGuids, IList videoGuids, IList appGuids, IDictionary mapIdToContext, EGetOffersFlags eGetOffersFlags, string deviceEndpointId, GetOffersCompleteCallback completeCallback, GetOffersErrorCallback errorCallback);
        ulong GetPassportPuid();
        string GetPassportTicket(EPassportPolicyId ePassportPolicy);
        void GetPaymentInstruments(GetPaymentInstrumentsCompleteCallback completeCallback, GetPaymentInstrumentsErrorCallback errorCallback);
        IList GetPersistedUsernames();
        string GetPhoneClientType(string strPhoneOsVersion);
        int GetPointsBalance();
        void GetPointsOffers(GetBillingOffersCompleteCallback completeCallback, GetBillingOffersErrorCallback errorCallback);
        RatingSystemBase[] GetRatingSystems();
        int GetRentalTermDays(string strStudio);
        int GetRentalTermHours(string strStudio);
        uint GetSignedInGeoId();
        string GetSignedInUsername();
        string GetSignInAtStartupUsername();
        void GetSubscriptionDetails(ulong offerId, GetBillingOffersCompleteCallback completeCallback, GetBillingOffersErrorCallback errorCallback);
        string GetSubscriptionDirectory();
        DateTime GetSubscriptionEndDate();
        int GetSubscriptionFreeTrackBalance();
        DateTime GetSubscriptionFreeTrackExpiration();
        ulong GetSubscriptionOfferId();
        void GetSubscriptionOffers(GetBillingOffersCompleteCallback completeCallback, GetBillingOffersErrorCallback errorCallback);
        ulong GetSubscriptionRenewalOfferId();
        int GetSubscriptionTrialDuration();
        ValueType GetUserGuid();
        bool GetUserRating(int iUserId, Guid guidMediaId, EContentType eContentType, ref int piRating);
        string GetWMISEndPointUri(string strEndPointName);
        string GetXboxPuid();
        string GetXboxTicket();
        string GetZuneTag();
        bool HasSignInBillingViolation();
        bool HasSignInLabelTakedown();
        bool InCompleteCollection(Guid guidMediaId, EContentType eContentType);
        bool InCompleteCollection(Guid guidMediaId, EContentType eContentType, out int dbMediaId, out bool fHidden);
        bool InCompleteCollection(Guid guidMediaId, EContentType eContentType, string strDeviceEndpointId);
        bool InCompleteCollection(Guid guidMediaId, EContentType eContentType, string strDeviceEndpointId, out int dbMediaId, out bool fHidden);
        bool InHiddenCollection(Guid guidMediaId, EContentType eContentType);
        int InitializeWMISEndpointCollection();
        bool InVisibleCollection(Guid guidMediaId, EContentType eContentType);
        bool InVisibleCollection(Guid guidMediaId, EContentType eContentType, out int dbMediaId);
        bool IsDownloading(Guid guidMediaId, EContentType eContentType, out bool fIsDownloadPending, out bool fIsHidden);
        bool IsLightWeight();
        bool IsParentallyControlled();
        bool IsSignedIn();
        bool IsSignedInWithSubscription();
        bool IsSigningIn();
        bool LaunchBrowserForExternalUrl(string strUrl, EPassportPolicyId ePassportPolicy);
        int Phase3Initialize();
        bool PostAppReview(Guid mediaId, string title, string comment, int rating, AsyncCompleteHandler callback);
        int PurchaseBillingOffer(BillingOffer offer, PaymentInstrument paymentInstrument);
        int PurchaseBillingOffer(BillingOffer offer, PaymentInstrument paymentInstrument, AsyncCompleteHandler callback);
        void PurchaseOffers(PaymentInstrument payment, AlbumOfferCollection albumOffers, TrackOfferCollection trackOffers, VideoOfferCollection videoOffers, AppOfferCollection appOffers, EPurchaseOffersFlags ePurchaseOffersFlags, PurchaseOffersCompleteHandler purchaseOffersHandler);
        void RefreshAccount(AsyncCompleteHandler eventHandler);
        void RegisterForDownloadNotification(DownloadEventHandler eventHandler, DownloadEventProgressHandler progressHandler, EventHandler allPendingHandler);
        void RemovePersistedUsername(string strUsername);
        bool ReportAConcern(EConcernType concernType, EContentType contentType, Guid mediaId, string message, AsyncCompleteHandler callback);
        bool ReportFavouriteArtists(Guid userId, IList artists, AsyncCompleteHandler callback);
        void ReportStreamingAction(EStreamingActionType eStreamingActionType, Guid guidMediaInstanceId, AsyncCompleteHandler eventHandler);
        int ResumePurchase(string resumeHandle, string authorizationToken, AsyncCompleteHandler callback);
        bool SetLastSignedInUserGuid(ref Guid guidUserGuid, out int iUserId);
        bool SetUserArtistRating(int iUserId, int iRating, Guid guidArtistMediaId, string strTitle);
        bool SetUserTrackRating(int iUserId, int iRating, Guid guidTrackMediaId, Guid guidAlbumMediaId, int iTrackNumber, string strTitle, int msDuration, string strAlbum, string strArtist, string strGenre, string strServiceContext);
        void SignIn(string strUsername, string strPassword, bool fRememberUsername, bool fRememberPassword, bool fAutomaticallySignInAtStartup, AsyncCompleteHandler eventHandler);
        bool SignInAtStartup(string strUsername);
        bool SignInPasswordRequired(string strUsername);
        void SignOut();
        bool SubscriptionPendingCancel();
        int VerifyToken(string token, out TokenDetails tokenDetails);
    }
}