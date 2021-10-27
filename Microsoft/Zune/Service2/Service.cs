using Microsoft.Zune.Service;
using Microsoft.Zune.Util;
using System;
using System.Collections;
using ZuneUI;

namespace Microsoft.Zune.Service2
{
    public class Service : IDisposable
    {
        private bool disposedValue;

        private static Service _instance = new Service();
        public static Service Instance => _instance;

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

        public int AddPaymentInstrument(PaymentInstrument paymentInstrument, AddPaymentInstrumentCompleteCallback completeCallback, AddPaymentInstrumentErrorCallback errorCallback)
        {
            throw new NotImplementedException();
        }

        public int AddPaymentInstrument(PaymentInstrument paymentInstrument, out string paymentId, out ServiceError serviceError)
        {
            throw new NotImplementedException();
        }

        public HRESULT AuthenticatePassport(string username, string password, EPassportPolicyId ePassportPolicyId, out PassportIdentity passportIdentity)
        {
            throw new NotImplementedException();
        }

        public bool BlockExplicitContent()
        {
            return true;
        }

        public bool BlockRatedContent(string system, string rating)
        {
            return false;
        }

        public void CancelDownload(Guid guidMediaId, EContentType eContentType)
        {

        }

        public void CancelSignIn()
        {

        }

        public bool CanDownloadSubscriptionContent()
        {
            throw new NotImplementedException();
        }

        public bool CanSignedInUserPostUsageData()
        {
            throw new NotImplementedException();
        }

        public bool ClearLastSignedInUser()
        {
            throw new NotImplementedException();
        }

        public AppOfferCollection CreateEmptyAppCollection()
        {
            throw new NotImplementedException();
        }

        public bool DeleteSubscriptionDownloads(AsyncCompleteHandler eventHandler)
        {
            throw new NotImplementedException();
        }

        public void Download(IList items, EDownloadFlags eDownloadFlags, string deviceEndpointId, EDownloadContextEvent clientContextEvent, string clientContextEventData, DownloadEventHandler eventHandler, DownloadEventProgressHandler progressHandler, EventHandler allPendingHandler)
        {
            throw new NotImplementedException();
        }

        public bool GetAlbumIdFromCompId(string compId, out Guid guidAlbum)
        {
            throw new NotImplementedException();
        }

        public void GetBalances(GetBalancesCompleteCallback completeCallback, GetBalancesErrorCallback errorCallback)
        {
            throw new NotImplementedException();
        }

        public EContentType GetContentType(string contentTypeStr)
        {
            throw new NotImplementedException();
        }

        public HRESULT GetContentUri(Guid guidMediaId, EContentType eContentType, EContentUriFlags eContentUriFlags, bool fIsHD, bool fIsRental, out string uriOut)
        {
            throw new NotImplementedException();
        }

        public HRESULT GetContentUri(Guid guidMediaId, EContentType eContentType, EContentUriFlags eContentUriFlags, out string uriOut, out Guid mediaInstanceIdOut)
        {
            throw new NotImplementedException();
        }

        public HRESULT GetContentUri(Guid guidMediaId, EContentType eContentType, EContentUriFlags eContentUriFlags, EMediaFormat eMediaFormat, EMediaRights eMediaRights, out string uriOut, out Guid mediaInstanceIdOut)
        {
            throw new NotImplementedException();
        }

        public CountryBaseDetails[] GetCountryDetails()
        {
            throw new NotImplementedException();
        }

        public DRMInfo GetFileDRMInfo(string filePath)
        {
            throw new NotImplementedException();
        }

        public bool GetLastSignedInUserGuid(out int iUserId, out Guid guidUserGuid)
        {
            iUserId = 2;
            guidUserGuid = Guid.Empty;
            return false;
            throw new NotImplementedException();
        }

        public void GetLastSignedInUserSubscriptionState(out bool activeSubscription, out ulong subscriptionId)
        {
            throw new NotImplementedException();
        }

        public string GetLocale()
        {
            throw new NotImplementedException();
        }

        public string GetMachineId()
        {
            throw new NotImplementedException();
        }

        public DRMInfo GetMediaDRMInfo(Guid mediaId, EContentType eContentType)
        {
            throw new NotImplementedException();
        }

        public EMediaStatus GetMediaStatus(Guid guidMediaId, EContentType eContentType)
        {
            throw new NotImplementedException();
        }

        public bool GetMusicVideoIdFromCompId(string compId, out Guid guidMusicVideo)
        {
            throw new NotImplementedException();
        }

        public HRESULT GetOfferDetails(Guid offerId, GetOfferDetailsCompleteCallback completeCallback, GetOfferDetailsErrorCallback errorCallback, object state)
        {
            throw new NotImplementedException();
        }

        public void GetOffers(IList albumGuids, IList trackGuids, IList videoGuids, IList appGuids, IDictionary mapIdToContext, EGetOffersFlags eGetOffersFlags, string deviceEndpointId, GetOffersCompleteCallback completeCallback, GetOffersErrorCallback errorCallback)
        {
            throw new NotImplementedException();
        }

        public ulong GetPassportPuid()
        {
            throw new NotImplementedException();
        }

        public string GetPassportTicket(EPassportPolicyId ePassportPolicy)
        {
            throw new NotImplementedException();
        }

        public void GetPaymentInstruments(GetPaymentInstrumentsCompleteCallback completeCallback, GetPaymentInstrumentsErrorCallback errorCallback)
        {
            throw new NotImplementedException();
        }

        public IList GetPersistedUsernames()
        {
            return new System.Collections.Generic.List<string> () { };
            throw new NotImplementedException();
        }

        public string GetPhoneClientType(string strPhoneOsVersion)
        {
            throw new NotImplementedException();
        }

        public int GetPointsBalance()
        {
            throw new NotImplementedException();
        }

        public void GetPointsOffers(GetBillingOffersCompleteCallback completeCallback, GetBillingOffersErrorCallback errorCallback)
        {
            throw new NotImplementedException();
        }

        public RatingSystemBase[] GetRatingSystems()
        {
            throw new NotImplementedException();
        }

        public int GetRentalTermDays(string strStudio)
        {
            throw new NotImplementedException();
        }

        public int GetRentalTermHours(string strStudio)
        {
            throw new NotImplementedException();
        }

        public uint GetSignedInGeoId()
        {
            throw new NotImplementedException();
        }

        public string GetSignedInUsername()
        {
            throw new NotImplementedException();
        }

        public string GetSignInAtStartupUsername()
        {
            return string.Empty;
            throw new NotImplementedException();
        }

        public void GetSubscriptionDetails(ulong offerId, GetBillingOffersCompleteCallback completeCallback, GetBillingOffersErrorCallback errorCallback)
        {
            throw new NotImplementedException();
        }

        public string GetSubscriptionDirectory()
        {
            throw new NotImplementedException();
        }

        public DateTime GetSubscriptionEndDate()
        {
            throw new NotImplementedException();
        }

        public int GetSubscriptionFreeTrackBalance()
        {
            return 0;
            throw new NotImplementedException();
        }

        public DateTime GetSubscriptionFreeTrackExpiration()
        {
            throw new NotImplementedException();
        }

        public ulong GetSubscriptionOfferId()
        {
            throw new NotImplementedException();
        }

        public void GetSubscriptionOffers(GetBillingOffersCompleteCallback completeCallback, GetBillingOffersErrorCallback errorCallback)
        {
            throw new NotImplementedException();
        }

        public ulong GetSubscriptionRenewalOfferId()
        {
            throw new NotImplementedException();
        }

        public int GetSubscriptionTrialDuration()
        {
            throw new NotImplementedException();
        }

        public ValueType GetUserGuid()
        {
            throw new NotImplementedException();
        }

        public bool GetUserRating(int iUserId, Guid guidMediaId, EContentType eContentType, ref int piRating)
        {
            throw new NotImplementedException();
        }

        public string GetWMISEndPointUri(string strEndPointName)
        {
            throw new NotImplementedException();
        }

        public string GetXboxPuid()
        {
            throw new NotImplementedException();
        }

        public string GetXboxTicket()
        {
            throw new NotImplementedException();
        }

        public string GetZuneTag()
        {
            throw new NotImplementedException();
        }

        public bool HasSignInBillingViolation()
        {
            throw new NotImplementedException();
        }

        public bool HasSignInLabelTakedown()
        {
            throw new NotImplementedException();
        }

        public bool InCompleteCollection(Guid guidMediaId, EContentType eContentType, string strDeviceEndpointId)
        {
            throw new NotImplementedException();
        }

        public bool InCompleteCollection(Guid guidMediaId, EContentType eContentType, out int dbMediaId, out bool fHidden)
        {
            throw new NotImplementedException();
        }

        public bool InCompleteCollection(Guid guidMediaId, EContentType eContentType, string strDeviceEndpointId, out int dbMediaId, out bool fHidden)
        {
            throw new NotImplementedException();
        }

        public bool InCompleteCollection(Guid guidMediaId, EContentType eContentType)
        {
            throw new NotImplementedException();
        }

        public bool InHiddenCollection(Guid guidMediaId, EContentType eContentType)
        {
            throw new NotImplementedException();
        }

        public int InitializeWMISEndpointCollection()
        {
            throw new NotImplementedException();
        }

        public bool InVisibleCollection(Guid guidMediaId, EContentType eContentType)
        {
            throw new NotImplementedException();
        }

        public bool InVisibleCollection(Guid guidMediaId, EContentType eContentType, out int dbMediaId)
        {
            throw new NotImplementedException();
        }

        public bool IsDownloading(Guid guidMediaId, EContentType eContentType, out bool fIsDownloadPending, out bool fIsHidden)
        {
            throw new NotImplementedException();
        }

        public bool IsLightWeight()
        {
            throw new NotImplementedException();
        }

        public bool IsParentallyControlled()
        {
            throw new NotImplementedException();
        }

        public bool IsSignedIn()
        {
            throw new NotImplementedException();
        }

        public bool IsSignedInWithSubscription()
        {
            throw new NotImplementedException();
        }

        public bool IsSigningIn()
        {
            throw new NotImplementedException();
        }

        public bool LaunchBrowserForExternalUrl(string strUrl, EPassportPolicyId ePassportPolicy)
        {
            throw new NotImplementedException();
        }

        public int Phase3Initialize()
        {
            ZuneShell.DefaultInstance.CurrentPage.AutoHideToolbars = true;
            //Win32MessageBox.Show("Navigated to page", "Welcome to the social", Win32MessageBoxType.MB_OK, null);
            return 0;

            throw new NotImplementedException();
        }

        public bool PostAppReview(Guid mediaId, string title, string comment, int rating, AsyncCompleteHandler callback)
        {
            throw new NotImplementedException();
        }

        public int PurchaseBillingOffer(BillingOffer offer, PaymentInstrument paymentInstrument, AsyncCompleteHandler callback)
        {
            throw new NotImplementedException();
        }

        public int PurchaseBillingOffer(BillingOffer offer, PaymentInstrument paymentInstrument)
        {
            throw new NotImplementedException();
        }

        public void PurchaseOffers(PaymentInstrument payment, AlbumOfferCollection albumOffers, TrackOfferCollection trackOffers, VideoOfferCollection videoOffers, AppOfferCollection appOffers, EPurchaseOffersFlags ePurchaseOffersFlags, PurchaseOffersCompleteHandler purchaseOffersHandler)
        {
            throw new NotImplementedException();
        }

        public void RefreshAccount(AsyncCompleteHandler eventHandler)
        {
            throw new NotImplementedException();
        }

        public void RegisterForDownloadNotification(DownloadEventHandler eventHandler, DownloadEventProgressHandler progressHandler, EventHandler allPendingHandler)
        {
            return;
            throw new NotImplementedException();
        }

        public void RemovePersistedUsername(string strUsername)
        {
            throw new NotImplementedException();
        }

        public bool ReportAConcern(EConcernType concernType, EContentType contentType, Guid mediaId, string message, AsyncCompleteHandler callback)
        {
            throw new NotImplementedException();
        }

        public bool ReportFavouriteArtists(Guid userId, IList artists, AsyncCompleteHandler callback)
        {
            throw new NotImplementedException();
        }

        public void ReportStreamingAction(EStreamingActionType eStreamingActionType, Guid guidMediaInstanceId, AsyncCompleteHandler eventHandler)
        {
            throw new NotImplementedException();
        }

        public int ResumePurchase(string resumeHandle, string authorizationToken, AsyncCompleteHandler callback)
        {
            throw new NotImplementedException();
        }

        public bool SetLastSignedInUserGuid(ref Guid guidUserGuid, out int iUserId)
        {
            throw new NotImplementedException();
        }

        public bool SetUserArtistRating(int iUserId, int iRating, Guid guidArtistMediaId, string strTitle)
        {
            throw new NotImplementedException();
        }

        public bool SetUserTrackRating(int iUserId, int iRating, Guid guidTrackMediaId, Guid guidAlbumMediaId, int iTrackNumber, string strTitle, int msDuration, string strAlbum, string strArtist, string strGenre, string strServiceContext)
        {
            throw new NotImplementedException();
        }

        public void SignIn(string strUsername, string strPassword, bool fRememberUsername, bool fRememberPassword, bool fAutomaticallySignInAtStartup, AsyncCompleteHandler eventHandler)
        {
            return;
            throw new NotImplementedException();
        }

        public bool SignInAtStartup(string strUsername)
        {
            throw new NotImplementedException();
        }

        public bool SignInPasswordRequired(string strUsername)
        {
            throw new NotImplementedException();
        }

        public void SignOut()
        {
            throw new NotImplementedException();
        }

        public bool SubscriptionPendingCancel()
        {
            throw new NotImplementedException();
        }

        public int VerifyToken(string token, out TokenDetails tokenDetails)
        {
            throw new NotImplementedException();
        }


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
        ~Service()
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