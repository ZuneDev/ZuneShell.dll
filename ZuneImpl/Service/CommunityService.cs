using Microsoft.Zune.Util;
using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;
using ZuneUI;

namespace Microsoft.Zune.Service
{
    internal class CommunityService : IService
    {
        private bool disposedValue;

        public static string AppUserAgent { get; set; }
        public static string ApiVerionStr { get; set; }

        public EListType ContentTypeToListType(EContentType contentType)
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
        public string GetEndPointUri(EServiceEndpointId eServiceEndpointId)
        {
            // Allow override from registry, like the internal dogfood version did
            string keyName = eServiceEndpointId.ToString().Substring(5) + "Endpoint";
            var key = Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Zune\Service");
            var keyVal = key?.GetValue(keyName);
            if (keyVal is string keyValUri)
                return keyValUri;

            string uri = null;
            string culture = CultureInfo.CurrentCulture.ToString();
            switch (eServiceEndpointId)
            {
                case EServiceEndpointId.SEID_RootCatalog:
                    uri = "http://catalog.zunes.me/v3.2/" + culture;
                    break;
                case EServiceEndpointId.SEID_RootCatalogSSL:
                    uri = "https://catalog.zunes.me/v3.2/" + culture;
                    break;
                case EServiceEndpointId.SEID_Stats:
                    uri = "https://stat.zunes.me";
                    break;
                case EServiceEndpointId.SEID_ImageCatalog:
                    uri = "https://image.catalog.zunes.me/" + culture;
                    break;
                case EServiceEndpointId.SEID_Comments:
                    uri = "https://comments.zunes.me";
                    break;
                case EServiceEndpointId.SEID_QuickMixSecure:
                    uri = "http://mix.zunes.me";
                    break;
                case EServiceEndpointId.SEID_QuickMix:
                    uri = "https://mix.zunes.me";
                    break;
                case EServiceEndpointId.SEID_Commerce:
                case EServiceEndpointId.SEID_CommerceV3:
                    uri = "https://commerce.zunes.me/v3.2/" + culture;
                    break;
                case EServiceEndpointId.SEID_CommerceV2:
                    uri = "https://stat.zunes.me/v2.0/" + culture;
                    break;
                case EServiceEndpointId.SEID_WindowsLiveSignup:
                    uri = "https://login.zunes.me";
                    break;
                case EServiceEndpointId.SEID_WMISEndpoints:
                    uri = "https://metaservices.zunes.me/endpoints";
                    break;
                case EServiceEndpointId.SEID_WMISRedirEndpoint:
                    uri = "https://metaservices.zunes.me/redir";
                    break;
                case EServiceEndpointId.SEID_WMISImageEndpoint:
                    uri = "https://metaservices.zunes.me/image";
                    break;
                case EServiceEndpointId.SEID_Tuners:
                    uri = "https://tuners.zunes.me/" + culture;
                    break;
                case EServiceEndpointId.SEID_Resources:
                    uri = "https://resources.zunes.me";
                    break;
                case EServiceEndpointId.SEID_ZuneNet:
                    uri = "https://zunes.me/" + culture;
                    break;
                case EServiceEndpointId.SEID_Messaging:
                    uri = "https://messaging.zunes.me";
                    break;
                case EServiceEndpointId.SEID_SocialApi:
                    uri = "https://socialapi.zunes.me/" + culture;
                    break;
                case EServiceEndpointId.SEID_Forums:
                    uri = "https://forums.zunes.me/" + culture;
                    break;
            };

            return uri ?? Service.GetEndPointUri(eServiceEndpointId);
        }

        public int AddPaymentInstrument(PaymentInstrument paymentInstrument,
            AddPaymentInstrumentCompleteCallback completeCallback,
            AddPaymentInstrumentErrorCallback errorCallback)
        {
            return Service.Instance.AddPaymentInstrument(paymentInstrument, completeCallback, errorCallback);
        }

        public int AddPaymentInstrument(PaymentInstrument paymentInstrument, out string paymentId, out ServiceError serviceError)
        {
            return Service.Instance.AddPaymentInstrument(paymentInstrument, out paymentId, out serviceError);
        }

        public HRESULT AuthenticatePassport(string username, string password, EPassportPolicyId ePassportPolicyId, out PassportIdentity passportIdentity)
        {
            return Service.Instance.AuthenticatePassport(username, password, ePassportPolicyId, out passportIdentity);
        }

        public bool BlockExplicitContent()
        {
            return Service.Instance.BlockExplicitContent();
        }

        public bool BlockRatedContent(string system, string rating)
        {
            return Service.Instance.BlockRatedContent(system, rating);
        }

        public void CancelDownload(Guid guidMediaId, EContentType eContentType)
        {
            Service.Instance.CancelDownload(guidMediaId, eContentType);
        }

        public void CancelSignIn()
        {
            Service.Instance.CancelSignIn();
        }

        public bool CanDownloadSubscriptionContent()
        {
            return Service.Instance.CanDownloadSubscriptionContent();
        }

        public bool CanSignedInUserPostUsageData()
        {
            return Service.Instance.CanSignedInUserPostUsageData();
        }

        public bool ClearLastSignedInUser()
        {
            return Service.Instance.ClearLastSignedInUser();
        }

        public AppOfferCollection CreateEmptyAppCollection()
        {
            return Service.Instance.CreateEmptyAppCollection();
        }

        public bool DeleteSubscriptionDownloads(AsyncCompleteHandler eventHandler)
        {
            return Service.Instance.DeleteSubscriptionDownloads(eventHandler);
        }

        public void Download(IList items, EDownloadFlags eDownloadFlags, string deviceEndpointId, EDownloadContextEvent clientContextEvent, string clientContextEventData, DownloadEventHandler eventHandler, DownloadEventProgressHandler progressHandler, EventHandler allPendingHandler)
        {
            Service.Instance.Download(items, eDownloadFlags, deviceEndpointId, clientContextEvent, clientContextEventData, eventHandler, progressHandler, allPendingHandler);
        }

        public bool GetAlbumIdFromCompId(string compId, out Guid guidAlbum)
        {
            return Service.Instance.GetAlbumIdFromCompId(compId, out guidAlbum);
        }

        public void GetBalances(GetBalancesCompleteCallback completeCallback, GetBalancesErrorCallback errorCallback)
        {
            Service.Instance.GetBalances(completeCallback, errorCallback);
        }

        public EContentType GetContentType(string contentTypeStr)
        {
            return Service.Instance.GetContentType(contentTypeStr);
        }

        public HRESULT GetContentUri(Guid guidMediaId, EContentType eContentType, EContentUriFlags eContentUriFlags, bool fIsHD, bool fIsRental, out string uriOut)
        {
            return Service.Instance.GetContentUri(guidMediaId, eContentType, eContentUriFlags, fIsHD, fIsRental, out uriOut);
        }

        public HRESULT GetContentUri(Guid guidMediaId, EContentType eContentType, EContentUriFlags eContentUriFlags, out string uriOut, out Guid mediaInstanceIdOut)
        {
            return Service.Instance.GetContentUri(guidMediaId, eContentType, eContentUriFlags, out uriOut, out mediaInstanceIdOut);
        }

        public HRESULT GetContentUri(Guid guidMediaId, EContentType eContentType, EContentUriFlags eContentUriFlags, EMediaFormat eMediaFormat, EMediaRights eMediaRights, out string uriOut, out Guid mediaInstanceIdOut)
        {
            throw new NotImplementedException();
            //return Service.Instance.GetContentUri(guidMediaId, eContentType, eContentUriFlags, eMediaFormat, eMediaRights, out uriOut, out mediaInstanceIdOut);
        }

        public CountryBaseDetails[] GetCountryDetails()
        {
            return Service.Instance.GetCountryDetails();
        }

        public DRMInfo GetFileDRMInfo(string filePath)
        {
            return Service.Instance.GetFileDRMInfo(filePath);
        }

        public bool GetLastSignedInUserGuid(out int iUserId, out Guid guidUserGuid)
        {
            return Service.Instance.GetLastSignedInUserGuid(out iUserId, out guidUserGuid);
        }

        public void GetLastSignedInUserSubscriptionState(out bool activeSubscription, out ulong subscriptionId)
        {
            Service.Instance.GetLastSignedInUserSubscriptionState(out activeSubscription, out subscriptionId);
        }

        public string GetLocale()
        {
            return CommunityCommerce.IsSignedIn
                ? CommunityCommerce.MemberInfo.AccountInfo.Locale
                : null;
        }

        public string GetMachineId()
        {
            return Service.Instance.GetMachineId();
        }

        public DRMInfo GetMediaDRMInfo(Guid mediaId, EContentType eContentType)
        {
            return Service.Instance.GetMediaDRMInfo(mediaId, eContentType);
        }

        public EMediaStatus GetMediaStatus(Guid guidMediaId, EContentType eContentType)
        {
            return Service.Instance.GetMediaStatus(guidMediaId, eContentType);
        }

        public bool GetMusicVideoIdFromCompId(string compId, out Guid guidMusicVideo)
        {
            return Service.Instance.GetMusicVideoIdFromCompId(compId, out guidMusicVideo);
        }

        public HRESULT GetOfferDetails(Guid offerId, GetOfferDetailsCompleteCallback completeCallback, GetOfferDetailsErrorCallback errorCallback, object state)
        {
            return Service.Instance.GetOfferDetails(offerId, completeCallback, errorCallback, state);
        }

        public void GetOffers(IList albumGuids, IList trackGuids, IList videoGuids, IList appGuids, IDictionary mapIdToContext, EGetOffersFlags eGetOffersFlags, string deviceEndpointId, GetOffersCompleteCallback completeCallback, GetOffersErrorCallback errorCallback)
        {
            Service.Instance.GetOffers(albumGuids, trackGuids, videoGuids, appGuids, mapIdToContext, eGetOffersFlags, deviceEndpointId, completeCallback, errorCallback);
        }

        public ulong GetPassportPuid()
        {
            return Service.Instance.GetPassportPuid();
        }

        public string GetPassportTicket(EPassportPolicyId ePassportPolicy)
        {
            return Service.Instance.GetPassportTicket(ePassportPolicy);
        }

        public void GetPaymentInstruments(GetPaymentInstrumentsCompleteCallback completeCallback, GetPaymentInstrumentsErrorCallback errorCallback)
        {
            Service.Instance.GetPaymentInstruments(completeCallback, errorCallback);
        }

        public IList GetPersistedUsernames()
        {
            return Service.Instance.GetPersistedUsernames();
        }

        public string GetPhoneClientType(string strPhoneOsVersion)
        {
            return Service.Instance.GetPhoneClientType(strPhoneOsVersion);
        }

        public int GetPointsBalance()
        {
            return CommunityCommerce.IsSignedIn
                ? (int)CommunityCommerce.MemberInfo.Balances.PointsBalance
                : 0;
        }

        public void GetPointsOffers(GetBillingOffersCompleteCallback completeCallback, GetBillingOffersErrorCallback errorCallback)
            => Service.Instance.GetPointsOffers(completeCallback, errorCallback);

        public RatingSystemBase[] GetRatingSystems() => Service.Instance.GetRatingSystems();

        public int GetRentalTermDays(string strStudio) => Service.Instance.GetRentalTermDays(strStudio);

        public int GetRentalTermHours(string strStudio) => Service.Instance.GetRentalTermHours(strStudio);

        public uint GetSignedInGeoId() => Service.Instance.GetSignedInGeoId();

        public string GetSignedInUsername() => Escargot.Username;

        public string GetSignInAtStartupUsername() => Service.Instance.GetSignInAtStartupUsername();

        public void GetSubscriptionDetails(ulong offerId, GetBillingOffersCompleteCallback completeCallback, GetBillingOffersErrorCallback errorCallback)
            => Service.Instance.GetSubscriptionDetails(offerId, completeCallback, errorCallback);

        public string GetSubscriptionDirectory() => Service.Instance.GetSubscriptionDirectory();

        public DateTime GetSubscriptionEndDate() => Service.Instance.GetSubscriptionEndDate();

        public int GetSubscriptionFreeTrackBalance() => Service.Instance.GetSubscriptionFreeTrackBalance();

        public DateTime GetSubscriptionFreeTrackExpiration() => Service.Instance.GetSubscriptionFreeTrackExpiration();

        public ulong GetSubscriptionOfferId() => Service.Instance.GetSubscriptionOfferId();

        public void GetSubscriptionOffers(GetBillingOffersCompleteCallback completeCallback, GetBillingOffersErrorCallback errorCallback)
            => Service.Instance.GetSubscriptionOffers(completeCallback, errorCallback);

        public ulong GetSubscriptionRenewalOfferId() => Service.Instance.GetSubscriptionRenewalOfferId();

        public int GetSubscriptionTrialDuration() => Service.Instance.GetSubscriptionTrialDuration();

        public ValueType GetUserGuid() => Service.Instance.GetUserGuid();

        public bool GetUserRating(int iUserId, Guid guidMediaId, EContentType eContentType, ref int piRating)
            => Service.Instance.GetUserRating(iUserId, guidMediaId, eContentType, ref piRating);

        public string GetWMISEndPointUri(string strEndPointName)
            => Service.Instance.GetWMISEndPointUri(strEndPointName);

        public string GetXboxPuid()
        {
            return CommunityCommerce.IsSignedIn
                ? CommunityCommerce.MemberInfo.AccountInfo.Xuid
                : null;
        }

        public string GetXboxTicket() => Service.Instance.GetXboxTicket();

        public string GetZuneTag()
        {
            return CommunityCommerce.IsSignedIn
                ? CommunityCommerce.MemberInfo.AccountInfo.ZuneTag
                : null;
        }

        public bool HasSignInBillingViolation() => Service.Instance.HasSignInBillingViolation();

        public bool HasSignInLabelTakedown() => Service.Instance.HasSignInLabelTakedown();

        public bool InCompleteCollection(Guid guidMediaId, EContentType eContentType, string strDeviceEndpointId)
            => Service.Instance.InCompleteCollection(guidMediaId, eContentType, strDeviceEndpointId);

        public bool InCompleteCollection(Guid guidMediaId, EContentType eContentType, out int dbMediaId, out bool fHidden)
            => Service.Instance.InCompleteCollection(guidMediaId, eContentType, out dbMediaId, out fHidden);

        public bool InCompleteCollection(Guid guidMediaId, EContentType eContentType, string strDeviceEndpointId, out int dbMediaId, out bool fHidden)
            => Service.Instance.InCompleteCollection(guidMediaId, eContentType, strDeviceEndpointId, out dbMediaId, out fHidden);

        public bool InCompleteCollection(Guid guidMediaId, EContentType eContentType)
            => Service.Instance.InCompleteCollection(guidMediaId, eContentType);

        public bool InHiddenCollection(Guid guidMediaId, EContentType eContentType)
            => Service.Instance.InHiddenCollection(guidMediaId, eContentType);

        public int InitializeWMISEndpointCollection()
            => Service.Instance.InitializeWMISEndpointCollection();

        public bool InVisibleCollection(Guid guidMediaId, EContentType eContentType)
            => Service.Instance.InVisibleCollection(guidMediaId, eContentType);

        public bool InVisibleCollection(Guid guidMediaId, EContentType eContentType, out int dbMediaId)
            => Service.Instance.InVisibleCollection(guidMediaId, eContentType, out dbMediaId);

        public bool IsDownloading(Guid guidMediaId, EContentType eContentType, out bool fIsDownloadPending, out bool fIsHidden)
            => Service.Instance.IsDownloading(guidMediaId, eContentType, out fIsDownloadPending, out fIsHidden);

        public bool IsLightWeight() => CommunityCommerce.IsSignedIn && CommunityCommerce.MemberInfo.AccountInfo.Lightweight;

        public bool IsParentallyControlled() => CommunityCommerce.IsSignedIn && CommunityCommerce.MemberInfo.AccountInfo.ParentallyControlled;

        public bool IsSignedIn() => CommunityCommerce.IsSignedIn;

        public bool IsSignedInWithSubscription() => CommunityCommerce.IsSignedIn && CommunityCommerce.MemberInfo.SubscriptionInfo.SubscriptionEnabled;

        public bool IsSigningIn() => Service.Instance.IsSigningIn();

        public bool LaunchBrowserForExternalUrl(string strUrl, EPassportPolicyId ePassportPolicy)
            => Service.Instance.LaunchBrowserForExternalUrl(strUrl, ePassportPolicy);

        public int Phase3Initialize()
        {
            var name = System.Reflection.Assembly.GetEntryAssembly()?.GetName();
            if (ApiVerionStr == null)
                ApiVerionStr = name?.Version?.ToString(2);
            if (AppUserAgent == null)
                AppUserAgent = $"{name?.Name}/{ApiVerionStr}";

            return Service.Instance.Phase3Initialize();
        }

        public bool PostAppReview(Guid mediaId, string title, string comment, int rating, AsyncCompleteHandler callback)
            => Service.Instance.PostAppReview(mediaId, title, comment, rating, callback);

        public int PurchaseBillingOffer(BillingOffer offer, PaymentInstrument paymentInstrument, AsyncCompleteHandler callback)
            => Service.Instance.PurchaseBillingOffer(offer, paymentInstrument, callback);

        public int PurchaseBillingOffer(BillingOffer offer, PaymentInstrument paymentInstrument)
            => Service.Instance.PurchaseBillingOffer(offer, paymentInstrument);

        public void PurchaseOffers(PaymentInstrument payment, AlbumOfferCollection albumOffers, TrackOfferCollection trackOffers, VideoOfferCollection videoOffers, AppOfferCollection appOffers, EPurchaseOffersFlags ePurchaseOffersFlags, PurchaseOffersCompleteHandler purchaseOffersHandler)
            => Service.Instance.PurchaseOffers(payment, albumOffers, trackOffers, videoOffers, appOffers, ePurchaseOffersFlags, purchaseOffersHandler);

        public void RefreshAccount(AsyncCompleteHandler eventHandler) => Service.Instance.RefreshAccount(eventHandler);

        public void RegisterForDownloadNotification(DownloadEventHandler eventHandler, DownloadEventProgressHandler progressHandler, EventHandler allPendingHandler)
            => Service.Instance.RegisterForDownloadNotification(eventHandler, progressHandler, allPendingHandler);

        public void RemovePersistedUsername(string strUsername)
            => Service.Instance.RemovePersistedUsername(strUsername);

        public bool ReportAConcern(EConcernType concernType, EContentType contentType, Guid mediaId, string message, AsyncCompleteHandler callback)
            => Service.Instance.ReportAConcern(concernType, contentType, mediaId, message, callback);

        public bool ReportFavouriteArtists(Guid userId, IList artists, AsyncCompleteHandler callback)
            => Service.Instance.ReportFavouriteArtists(userId, artists, callback);

        public void ReportStreamingAction(EStreamingActionType eStreamingActionType, Guid guidMediaInstanceId, AsyncCompleteHandler eventHandler)
            => Service.Instance.ReportStreamingAction(eStreamingActionType, guidMediaInstanceId, eventHandler);

        public int ResumePurchase(string resumeHandle, string authorizationToken, AsyncCompleteHandler callback)
            => Service.Instance.ResumePurchase(resumeHandle, authorizationToken, callback);

        public bool SetLastSignedInUserGuid(ref Guid guidUserGuid, out int iUserId)
            => Service.Instance.SetLastSignedInUserGuid(ref guidUserGuid, out iUserId);

        public bool SetUserArtistRating(int iUserId, int iRating, Guid guidArtistMediaId, string strTitle)
            => Service.Instance.SetUserArtistRating(iUserId, iRating, guidArtistMediaId, strTitle);

        public bool SetUserTrackRating(int iUserId, int iRating, Guid guidTrackMediaId, Guid guidAlbumMediaId, int iTrackNumber, string strTitle, int msDuration, string strAlbum, string strArtist, string strGenre, string strServiceContext)
            => Service.Instance.SetUserTrackRating(iUserId, iRating, guidTrackMediaId, guidAlbumMediaId, iTrackNumber, strTitle, msDuration, strAlbum, strArtist, strGenre, strServiceContext);

        public void SignIn(string strUsername, string strPassword, bool fRememberUsername, bool fRememberPassword, bool fAutomaticallySignInAtStartup, AsyncCompleteHandler eventHandler)
        {
            HRESULT hr = Escargot.TrySignIn(strUsername, strPassword) ? HRESULT._S_OK : HRESULT._NS_E_PASSPORT_LOGIN_FAILED;

            if (hr.IsSuccess)
            {
#if OPENZUNE
                hr = CommunityCommerce.TrySignIn();
#endif

                if (hr.IsSuccess)
                {
                    if (fRememberPassword)
                        Escargot.CacheToken();
                }
            }

            eventHandler(hr);
        }

        public bool SignInAtStartup(string strUsername) => Service.Instance.SignInAtStartup(strUsername);

        public bool SignInPasswordRequired(string strUsername)
            => Service.Instance.SignInPasswordRequired(strUsername);

        public void SignOut()
        {
            Escargot.ClearToken();
            CommunityCommerce.SignOut();
        }

        public bool SubscriptionPendingCancel() => Service.Instance.SubscriptionPendingCancel();

        public int VerifyToken(string token, out TokenDetails tokenDetails)
            => Service.Instance.VerifyToken(token, out tokenDetails);


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
        ~CommunityService()
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
    }
}
