using Microsoft.Zune.Service;
using Microsoft.Zune.Util;
using System;
using System.Collections;
using ZuneUI;
using RealService = Microsoft.Zune.Service.Service;

namespace Microsoft.Zune.Service
{
    public class Service2 : IDisposable
    {
        private bool disposedValue;

        private static Service2 _instance = new Service2();
        public static Service2 Instance => _instance;

        public static EListType ContentTypeToListType(EContentType contentType)
        {
            switch (contentType)
            {
                case EContentType.MusicTrack:
                    return EListType.eTrackList;

                case EContentType.MusicAlbum:
                    return EListType.eAlbumList;

                case EContentType.Video:
                    return EListType.eVideoList;

                case EContentType.Artist:
                    return EListType.eArtistList;

                case EContentType.PodcastEpisode:
                    return EListType.ePodcastEpisodeList;

                case EContentType.PodcastSeries:
                    return EListType.ePodcastList;

                case EContentType.App:
                    return EListType.eAppList;

                case EContentType.Playlist:
                    return EListType.ePlaylistList;

                default:
                case EContentType.Unknown:
                    return EListType.eListInvalid;
            }
        }
        public static string GetEndPointUri(EServiceEndpointId eServiceEndpointId)
        {
            return "catalog.zunes.me/service?id=" + eServiceEndpointId.ToString();
        }

        public int AddPaymentInstrument(PaymentInstrument paymentInstrument, 
            AddPaymentInstrumentCompleteCallback completeCallback,
            AddPaymentInstrumentErrorCallback errorCallback)
        {
            return RealService.Instance.AddPaymentInstrument(paymentInstrument, completeCallback, errorCallback);
        }

        public int AddPaymentInstrument(PaymentInstrument paymentInstrument, out string paymentId, out ServiceError serviceError)
        {
            return RealService.Instance.AddPaymentInstrument(paymentInstrument, out paymentId, out serviceError);
        }

        public HRESULT AuthenticatePassport(string username, string password, EPassportPolicyId ePassportPolicyId, out PassportIdentity passportIdentity)
        {
            return RealService.Instance.AuthenticatePassport(username, password, ePassportPolicyId, out passportIdentity);
        }

        public bool BlockExplicitContent()
        {
            return RealService.Instance.BlockExplicitContent();
        }

        public bool BlockRatedContent(string system, string rating)
        {
            return RealService.Instance.BlockRatedContent(system, rating);
        }

        public void CancelDownload(Guid guidMediaId, EContentType eContentType)
        {
            RealService.Instance.CancelDownload(guidMediaId, eContentType);
        }

        public void CancelSignIn()
        {
            RealService.Instance.CancelSignIn();
        }

        public bool CanDownloadSubscriptionContent()
        {
            return RealService.Instance.CanDownloadSubscriptionContent();
        }

        public bool CanSignedInUserPostUsageData()
        {
            return RealService.Instance.CanSignedInUserPostUsageData();
        }

        public bool ClearLastSignedInUser()
        {
            return RealService.Instance.ClearLastSignedInUser();
        }

        public AppOfferCollection CreateEmptyAppCollection()
        {
            return RealService.Instance.CreateEmptyAppCollection();
        }

        public bool DeleteSubscriptionDownloads(AsyncCompleteHandler eventHandler)
        {
            return RealService.Instance.DeleteSubscriptionDownloads(eventHandler);
        }

        public void Download(IList items, EDownloadFlags eDownloadFlags, string deviceEndpointId, EDownloadContextEvent clientContextEvent, string clientContextEventData, DownloadEventHandler eventHandler, DownloadEventProgressHandler progressHandler, EventHandler allPendingHandler)
        {
            RealService.Instance.Download(items, eDownloadFlags, deviceEndpointId, clientContextEvent, clientContextEventData, eventHandler, progressHandler, allPendingHandler);
        }

        public bool GetAlbumIdFromCompId(string compId, out Guid guidAlbum)
        {
            return RealService.Instance.GetAlbumIdFromCompId(compId, out guidAlbum);
        }

        public void GetBalances(GetBalancesCompleteCallback completeCallback, GetBalancesErrorCallback errorCallback)
        {
            RealService.Instance.GetBalances(completeCallback, errorCallback);
        }

        public EContentType GetContentType(string contentTypeStr)
        {
            return RealService.Instance.GetContentType(contentTypeStr);
        }

        public HRESULT GetContentUri(Guid guidMediaId, EContentType eContentType, EContentUriFlags eContentUriFlags, bool fIsHD, bool fIsRental, out string uriOut)
        {
            return RealService.Instance.GetContentUri(guidMediaId, eContentType, eContentUriFlags, fIsHD, fIsRental, out uriOut);
        }

        public HRESULT GetContentUri(Guid guidMediaId, EContentType eContentType, EContentUriFlags eContentUriFlags, out string uriOut, out Guid mediaInstanceIdOut)
        {
            return RealService.Instance.GetContentUri(guidMediaId, eContentType, eContentUriFlags, out uriOut, out mediaInstanceIdOut);
        }

        public HRESULT GetContentUri(Guid guidMediaId, EContentType eContentType, EContentUriFlags eContentUriFlags, EMediaFormat eMediaFormat, EMediaRights eMediaRights, out string uriOut, out Guid mediaInstanceIdOut)
        {
            throw new NotImplementedException();
            //return RealService.Instance.GetContentUri(guidMediaId, eContentType, eContentUriFlags, eMediaFormat, eMediaRights, out uriOut, out mediaInstanceIdOut);
        }

        public CountryBaseDetails[] GetCountryDetails()
        {
            return RealService.Instance.GetCountryDetails();
        }

        public DRMInfo GetFileDRMInfo(string filePath)
        {
            return RealService.Instance.GetFileDRMInfo(filePath);
        }

        public bool GetLastSignedInUserGuid(out int iUserId, out Guid guidUserGuid)
        {
            return RealService.Instance.GetLastSignedInUserGuid(out iUserId, out guidUserGuid);
        }

        public void GetLastSignedInUserSubscriptionState(out bool activeSubscription, out ulong subscriptionId)
        {
            RealService.Instance.GetLastSignedInUserSubscriptionState(out activeSubscription, out subscriptionId);
        }

        public string GetLocale()
        {
            return RealService.Instance.GetLocale();
        }

        public string GetMachineId()
        {
            return RealService.Instance.GetMachineId();
        }

        public DRMInfo GetMediaDRMInfo(Guid mediaId, EContentType eContentType)
        {
            return RealService.Instance.GetMediaDRMInfo(mediaId, eContentType);
        }

        public EMediaStatus GetMediaStatus(Guid guidMediaId, EContentType eContentType)
        {
            return RealService.Instance.GetMediaStatus(guidMediaId, eContentType);
        }

        public bool GetMusicVideoIdFromCompId(string compId, out Guid guidMusicVideo)
        {
            return RealService.Instance.GetMusicVideoIdFromCompId(compId, out guidMusicVideo);
        }

        public HRESULT GetOfferDetails(Guid offerId, GetOfferDetailsCompleteCallback completeCallback, GetOfferDetailsErrorCallback errorCallback, object state)
        {
            return RealService.Instance.GetOfferDetails(offerId, completeCallback, errorCallback, state);
        }

        public void GetOffers(IList albumGuids, IList trackGuids, IList videoGuids, IList appGuids, IDictionary mapIdToContext, EGetOffersFlags eGetOffersFlags, string deviceEndpointId, GetOffersCompleteCallback completeCallback, GetOffersErrorCallback errorCallback)
        {
            RealService.Instance.GetOffers(albumGuids, trackGuids, videoGuids, appGuids, mapIdToContext, eGetOffersFlags, deviceEndpointId, completeCallback, errorCallback);
        }

        public ulong GetPassportPuid()
        {
            return RealService.Instance.GetPassportPuid();
        }

        public string GetPassportTicket(EPassportPolicyId ePassportPolicy)
        {
            return RealService.Instance.GetPassportTicket(ePassportPolicy);
        }

        public void GetPaymentInstruments(GetPaymentInstrumentsCompleteCallback completeCallback, GetPaymentInstrumentsErrorCallback errorCallback)
        {
            RealService.Instance.GetPaymentInstruments(completeCallback, errorCallback);
        }

        public IList GetPersistedUsernames()
        {
            return RealService.Instance.GetPersistedUsernames();
        }

        public string GetPhoneClientType(string strPhoneOsVersion)
        {
            return RealService.Instance.GetPhoneClientType(strPhoneOsVersion);
        }

        public int GetPointsBalance() => RealService.Instance.GetPointsBalance();

        public void GetPointsOffers(GetBillingOffersCompleteCallback completeCallback, GetBillingOffersErrorCallback errorCallback)
            => RealService.Instance.GetPointsOffers(completeCallback, errorCallback);

        public RatingSystemBase[] GetRatingSystems() => RealService.Instance.GetRatingSystems();

        public int GetRentalTermDays(string strStudio) => RealService.Instance.GetRentalTermDays(strStudio);

        public int GetRentalTermHours(string strStudio) => RealService.Instance.GetRentalTermHours(strStudio);

        public uint GetSignedInGeoId() => RealService.Instance.GetSignedInGeoId();

        public string GetSignedInUsername() => RealService.Instance.GetSignedInUsername();

        public string GetSignInAtStartupUsername() => RealService.Instance.GetSignInAtStartupUsername();

        public void GetSubscriptionDetails(ulong offerId, GetBillingOffersCompleteCallback completeCallback, GetBillingOffersErrorCallback errorCallback)
            => RealService.Instance.GetSubscriptionDetails(offerId, completeCallback, errorCallback);

        public string GetSubscriptionDirectory() => RealService.Instance.GetSubscriptionDirectory();

        public DateTime GetSubscriptionEndDate() => RealService.Instance.GetSubscriptionEndDate();

        public int GetSubscriptionFreeTrackBalance() => RealService.Instance.GetSubscriptionFreeTrackBalance();

        public DateTime GetSubscriptionFreeTrackExpiration() => RealService.Instance.GetSubscriptionFreeTrackExpiration();

        public ulong GetSubscriptionOfferId() => RealService.Instance.GetSubscriptionOfferId();

        public void GetSubscriptionOffers(GetBillingOffersCompleteCallback completeCallback, GetBillingOffersErrorCallback errorCallback)
            => RealService.Instance.GetSubscriptionOffers(completeCallback, errorCallback);

        public ulong GetSubscriptionRenewalOfferId() => RealService.Instance.GetSubscriptionRenewalOfferId();

        public int GetSubscriptionTrialDuration() => RealService.Instance.GetSubscriptionTrialDuration();

        public ValueType GetUserGuid() => RealService.Instance.GetUserGuid();

        public bool GetUserRating(int iUserId, Guid guidMediaId, EContentType eContentType, ref int piRating)
            => RealService.Instance.GetUserRating(iUserId, guidMediaId, eContentType, ref piRating);

        public string GetWMISEndPointUri(string strEndPointName)
            => RealService.Instance.GetWMISEndPointUri(strEndPointName);

        public string GetXboxPuid() => RealService.Instance.GetXboxPuid();

        public string GetXboxTicket() => RealService.Instance.GetXboxTicket();

        public string GetZuneTag() => RealService.Instance.GetZuneTag();

        public bool HasSignInBillingViolation() => RealService.Instance.HasSignInBillingViolation();

        public bool HasSignInLabelTakedown() => RealService.Instance.HasSignInLabelTakedown();

        public bool InCompleteCollection(Guid guidMediaId, EContentType eContentType, string strDeviceEndpointId)
            => RealService.Instance.InCompleteCollection(guidMediaId, eContentType, strDeviceEndpointId);

        public bool InCompleteCollection(Guid guidMediaId, EContentType eContentType, out int dbMediaId, out bool fHidden)
            => RealService.Instance.InCompleteCollection(guidMediaId, eContentType, out dbMediaId, out fHidden);

        public bool InCompleteCollection(Guid guidMediaId, EContentType eContentType, string strDeviceEndpointId, out int dbMediaId, out bool fHidden)
            => RealService.Instance.InCompleteCollection(guidMediaId, eContentType, strDeviceEndpointId, out dbMediaId, out fHidden);

        public bool InCompleteCollection(Guid guidMediaId, EContentType eContentType)
            => RealService.Instance.InCompleteCollection(guidMediaId, eContentType);

        public bool InHiddenCollection(Guid guidMediaId, EContentType eContentType)
            => RealService.Instance.InHiddenCollection(guidMediaId, eContentType);

        public int InitializeWMISEndpointCollection()
            => RealService.Instance.InitializeWMISEndpointCollection();

        public bool InVisibleCollection(Guid guidMediaId, EContentType eContentType)
            => RealService.Instance.InVisibleCollection(guidMediaId, eContentType);

        public bool InVisibleCollection(Guid guidMediaId, EContentType eContentType, out int dbMediaId)
            => RealService.Instance.InVisibleCollection(guidMediaId, eContentType, out dbMediaId);

        public bool IsDownloading(Guid guidMediaId, EContentType eContentType, out bool fIsDownloadPending, out bool fIsHidden)
            => RealService.Instance.IsDownloading(guidMediaId, eContentType, out fIsDownloadPending, out fIsHidden);

        public bool IsLightWeight() => RealService.Instance.IsLightWeight();

        public bool IsParentallyControlled() => RealService.Instance.IsParentallyControlled();

        public bool IsSignedIn() => RealService.Instance.IsSignedIn();

        public bool IsSignedInWithSubscription() => RealService.Instance.IsSignedInWithSubscription();

        public bool IsSigningIn() => RealService.Instance.IsSigningIn();

        public bool LaunchBrowserForExternalUrl(string strUrl, EPassportPolicyId ePassportPolicy)
            => RealService.Instance.LaunchBrowserForExternalUrl(strUrl, ePassportPolicy);

        public int Phase3Initialize() => RealService.Instance.Phase3Initialize();

        public bool PostAppReview(Guid mediaId, string title, string comment, int rating, AsyncCompleteHandler callback)
            => RealService.Instance.PostAppReview(mediaId, title, comment, rating, callback);

        public int PurchaseBillingOffer(BillingOffer offer, PaymentInstrument paymentInstrument, AsyncCompleteHandler callback)
            => RealService.Instance.PurchaseBillingOffer(offer, paymentInstrument, callback);

        public int PurchaseBillingOffer(BillingOffer offer, PaymentInstrument paymentInstrument)
            => RealService.Instance.PurchaseBillingOffer(offer, paymentInstrument);

        public void PurchaseOffers(PaymentInstrument payment, AlbumOfferCollection albumOffers, TrackOfferCollection trackOffers, VideoOfferCollection videoOffers, AppOfferCollection appOffers, EPurchaseOffersFlags ePurchaseOffersFlags, PurchaseOffersCompleteHandler purchaseOffersHandler)
            => RealService.Instance.PurchaseOffers(payment, albumOffers, trackOffers, videoOffers, appOffers, ePurchaseOffersFlags, purchaseOffersHandler);

        public void RefreshAccount(AsyncCompleteHandler eventHandler) => RealService.Instance.RefreshAccount(eventHandler);

        public void RegisterForDownloadNotification(DownloadEventHandler eventHandler, DownloadEventProgressHandler progressHandler, EventHandler allPendingHandler)
            => RealService.Instance.RegisterForDownloadNotification(eventHandler, progressHandler, allPendingHandler);

        public void RemovePersistedUsername(string strUsername)
            => RealService.Instance.RemovePersistedUsername(strUsername);

        public bool ReportAConcern(EConcernType concernType, EContentType contentType, Guid mediaId, string message, AsyncCompleteHandler callback)
            => RealService.Instance.ReportAConcern(concernType, contentType, mediaId, message, callback);

        public bool ReportFavouriteArtists(Guid userId, IList artists, AsyncCompleteHandler callback)
            => RealService.Instance.ReportFavouriteArtists(userId, artists, callback);

        public void ReportStreamingAction(EStreamingActionType eStreamingActionType, Guid guidMediaInstanceId, AsyncCompleteHandler eventHandler)
            => RealService.Instance.ReportStreamingAction(eStreamingActionType, guidMediaInstanceId, eventHandler);

        public int ResumePurchase(string resumeHandle, string authorizationToken, AsyncCompleteHandler callback)
            => RealService.Instance.ResumePurchase(resumeHandle, authorizationToken, callback);

        public bool SetLastSignedInUserGuid(ref Guid guidUserGuid, out int iUserId)
            => RealService.Instance.SetLastSignedInUserGuid(ref guidUserGuid, out iUserId);

        public bool SetUserArtistRating(int iUserId, int iRating, Guid guidArtistMediaId, string strTitle)
            => RealService.Instance.SetUserArtistRating(iUserId, iRating, guidArtistMediaId, strTitle);

        public bool SetUserTrackRating(int iUserId, int iRating, Guid guidTrackMediaId, Guid guidAlbumMediaId, int iTrackNumber, string strTitle, int msDuration, string strAlbum, string strArtist, string strGenre, string strServiceContext)
            => RealService.Instance.SetUserTrackRating(iUserId, iRating, guidTrackMediaId, guidAlbumMediaId, iTrackNumber, strTitle, msDuration, strAlbum, strArtist, strGenre, strServiceContext);

        public void SignIn(string strUsername, string strPassword, bool fRememberUsername, bool fRememberPassword, bool fAutomaticallySignInAtStartup, AsyncCompleteHandler eventHandler)
            => RealService.Instance.SignIn(strUsername, strPassword, fRememberUsername, fRememberPassword, fAutomaticallySignInAtStartup, eventHandler);

        public bool SignInAtStartup(string strUsername) => RealService.Instance.SignInAtStartup(strUsername);

        public bool SignInPasswordRequired(string strUsername)
            => RealService.Instance.SignInPasswordRequired(strUsername);

        public void SignOut() => RealService.Instance.SignOut();

        public bool SubscriptionPendingCancel() => RealService.Instance.SubscriptionPendingCancel();

        public int VerifyToken(string token, out TokenDetails tokenDetails)
            => RealService.Instance.VerifyToken(token, out tokenDetails);


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // Override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Service2()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public enum EMediaFormat
        {
        }

        public enum EMediaRights
        {
        }
    }
}