// Decompiled with JetBrains decompiler
// Type: ZuneUI.Purchase
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.ErrorMapperApi;
using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Util;
using System;
using System.Collections;
using ZuneXml;

namespace ZuneUI
{
    public class Purchase : ModelItem
    {
        private AlbumOfferCollection m_albumOfferCollection;
        private TrackOfferCollection m_trackOfferCollection;
        private VideoOfferCollection m_videoOfferCollection;
        private AppOfferCollection m_appOfferCollection;
        private IList m_albumOffers;
        private IList m_trackOffers;
        private IList m_videoOffers;
        private IList m_appOffers;
        private bool m_fPurchaseHD;
        private bool m_fMultipleResolutionOptions;
        private bool m_fMultiplePlaybackOptions;
        private bool m_fRentVideos;
        private bool m_fStreamVideos;
        private bool m_fPurchaseTrials;
        private bool m_fPurchaseSeason;
        private bool m_hasSubscriptionFreeTracks;
        private int m_rentDeviceId = -1;
        private DateTime _videoExpirationDate = DateTime.MaxValue;
        private bool m_fInsufficientPoints;
        private int m_pointsBalance = -1;
        private int m_subscriptionFreeTrackBalance = -1;
        private int m_totalPoints = -1;
        private double m_totalCurrencyPrice = -1.0;
        private int m_totalSubscriptionFreeTracks = -1;
        private string m_displayPointsPrice;
        private string m_displayCurrencyPrice;
        private string m_errorMessage;
        private string m_errorWebHelpUrl;
        private string m_status;
        private string m_additionalStatus;
        private string m_rentDeviceName;
        private string m_rentDeviceEndpointId;
        private bool m_rentDeviceSupportsHD = true;
        private bool m_fRequestingBalances;
        private bool m_fIsBalanceUpdated;
        private bool m_fRequestingPointsOffers;
        private bool m_fPurchaseComplete;
        private bool m_fSubscriptionFreeTracksOnly;
        private bool m_authorizationRequired;
        private ResumePurchaseData m_resumePurchaseData;
        private bool m_fCanPurchase;
        private bool m_fCanDownload;
        private bool m_fPurchasingPoints;
        private BillingOffer m_pointsOffer;
        private BillingOfferCollection m_pointsOffers;
        private BillingOfferHelper m_pointsHelper;

        internal static event EventHandler PurchaseEvent;

        public Purchase() => this.m_fPurchaseHD = ClientConfiguration.Service.PurchaseHD;

        public void GetOffers(
          IList guidAlbumIds,
          IList guidTrackIds,
          IList guidVideoIds,
          IList guidAppIds)
        {
            this.GetOffers(guidAlbumIds, guidTrackIds, guidVideoIds, guidAppIds, false, (IDictionary)null);
        }

        public void GetOffers(
          IList guidAlbumIds,
          IList guidTrackIds,
          IList guidVideoIds,
          IList guidAppIds,
          bool subscriptionFreeTracksOnly)
        {
            this.GetOffers(guidAlbumIds, guidTrackIds, guidVideoIds, guidAppIds, subscriptionFreeTracksOnly, (IDictionary)null);
        }

        public void GetOffers(
          IList guidAlbumIds,
          IList guidTrackIds,
          IList guidVideoIds,
          IList guidAppIds,
          bool subscriptionFreeTracksOnly,
          IDictionary mapIdToContext)
        {
            this.SubscriptionFreeTracksOnly = subscriptionFreeTracksOnly;
            string deviceEndpointId = string.Empty;
            if (this.RentVideos)
                deviceEndpointId = this.RentDeviceEndpointId;
            if (mapIdToContext == null)
                mapIdToContext = (IDictionary)new Hashtable();
            this.UpdateContextMap(guidAlbumIds, guidTrackIds, mapIdToContext);
            Microsoft.Zune.Service.EGetOffersFlags eGetOffersFlags = Microsoft.Zune.Service.EGetOffersFlags.None;
            if (this.PurchaseSeason)
                eGetOffersFlags |= Microsoft.Zune.Service.EGetOffersFlags.SeasonPurchase;
            if (subscriptionFreeTracksOnly)
                eGetOffersFlags |= Microsoft.Zune.Service.EGetOffersFlags.SubscriptionFreeTracks;
            ZuneApplication.Service.GetOffers(guidAlbumIds, guidTrackIds, guidVideoIds, guidAppIds, mapIdToContext, eGetOffersFlags, deviceEndpointId, new GetOffersCompleteCallback(this.OnGetOffersComplete), new GetOffersErrorCallback(this.OnGetOffersError));
        }

        private void UpdateContextMap(
          IList guidAlbumIds,
          IList guidTrackIds,
          IDictionary mapIdToContext)
        {
            ZunePage currentPage = ZuneShell.DefaultInstance.CurrentPage;
            if (currentPage.NavigationArguments == null || !currentPage.NavigationArguments.Contains((object)"ReferrerContext"))
                return;
            string navigationArgument = (string)currentPage.NavigationArguments[(object)"ReferrerContext"];
            Guid guid1 = Guid.Empty;
            Guid guid2 = Guid.Empty;
            Guid guid3 = Guid.Empty;
            if (currentPage.NavigationArguments.Contains((object)"ReferrerTrackId"))
                guid1 = (Guid)currentPage.NavigationArguments[(object)"ReferrerTrackId"];
            else if (currentPage.NavigationArguments.Contains((object)"ReferrerAlbumId"))
                guid2 = (Guid)currentPage.NavigationArguments[(object)"ReferrerAlbumId"];
            else if (currentPage.NavigationArguments.Contains((object)"ReferrerArtistId"))
                guid3 = (Guid)currentPage.NavigationArguments[(object)"ReferrerArtistId"];
            if (guidAlbumIds != null)
            {
                foreach (Guid guidAlbumId in (IEnumerable)guidAlbumIds)
                {
                    if (!mapIdToContext.Contains((object)guidAlbumId) && (guid3 != Guid.Empty || guid1 != Guid.Empty || guid2 == guidAlbumId))
                        mapIdToContext[(object)guidAlbumId] = (object)navigationArgument;
                }
            }
            if (guidTrackIds == null)
                return;
            foreach (Guid guidTrackId in (IEnumerable)guidTrackIds)
            {
                if (!mapIdToContext.Contains((object)guidTrackId) && (guid3 != Guid.Empty || guid2 != Guid.Empty || guid1 == guidTrackId))
                    mapIdToContext[(object)guidTrackId] = (object)navigationArgument;
            }
        }

        public void GetBalances()
        {
            if (!this.m_fRequestingBalances)
            {
                this.m_fRequestingBalances = true;
                ZuneApplication.Service.GetBalances(new GetBalancesCompleteCallback(this.OnGetBalancesComplete), new GetBalancesErrorCallback(this.OnGetBalancesError));
            }
            if (this.m_fRequestingPointsOffers || this.m_pointsOffers != null)
                return;
            this.m_fRequestingPointsOffers = true;
            ZuneApplication.Service.GetPointsOffers(new GetBillingOffersCompleteCallback(this.OnGetPointsOffersComplete), new GetBillingOffersErrorCallback(this.OnGetPointsOffersError));
        }

        public void PurchaseOffers(PaymentInstrument payment)
        {
            this.ErrorMessage = (string)null;
            this.ErrorWebHelpUrl = (string)null;
            ZuneApplication.Service.PurchaseOffers(payment, this.m_albumOfferCollection, this.m_trackOfferCollection, this.m_videoOfferCollection, this.m_appOfferCollection, this.PurchaseOffersFlags, new PurchaseOffersCompleteHandler(this.OnPurchaseOffersComplete));
            if (this.RentVideos)
                this.Status = ZuneUI.Shell.LoadString(StringId.IDS_PURCHASE_RENTAL_IN_PROGRESS);
            else
                this.Status = ZuneUI.Shell.LoadString(StringId.IDS_PURCHASE_IN_PROGRESS);
        }

        public void ResumePurchase(string purchaseHandle, string token) => ZuneApplication.Service.ResumePurchase(purchaseHandle, token, new AsyncCompleteHandler(this.OnResumePurchaseComplete));

        public void PurchaseBestPointsOffer(PaymentInstrument paymentInstrument)
        {
            if (this.m_pointsOffer == null)
                return;
            this.PurchasingPoints = true;
            this.PointsHelper.Purchase(this.m_pointsOffer, paymentInstrument);
            this.UpdateState();
        }

        public int PointsBalance
        {
            get => this.m_pointsBalance <= 0 ? 0 : this.m_pointsBalance;
            private set
            {
                if (this.m_pointsBalance == value)
                    return;
                this.m_pointsBalance = value;
                this.FirePropertyChanged(nameof(PointsBalance));
            }
        }

        public bool IsBalanceUpdated
        {
            get => this.m_fIsBalanceUpdated;
            private set
            {
                this.m_fIsBalanceUpdated = value;
                this.FirePropertyChanged(nameof(IsBalanceUpdated));
            }
        }

        public int SubscriptionFreeTrackBalance
        {
            get => this.m_subscriptionFreeTrackBalance <= 0 ? 0 : this.m_subscriptionFreeTrackBalance;
            private set
            {
                if (this.m_subscriptionFreeTrackBalance == value)
                    return;
                this.m_subscriptionFreeTrackBalance = value;
                this.FirePropertyChanged(nameof(SubscriptionFreeTrackBalance));
            }
        }

        public IList AlbumOffers
        {
            get => this.m_albumOffers;
            private set
            {
                if (this.m_albumOffers == value)
                    return;
                this.m_albumOffers = value;
                this.FirePropertyChanged(nameof(AlbumOffers));
            }
        }

        public IList TrackOffers
        {
            get => this.m_trackOffers;
            private set
            {
                if (this.m_trackOffers == value)
                    return;
                this.m_trackOffers = value;
                this.FirePropertyChanged(nameof(TrackOffers));
            }
        }

        public IList VideoOffers
        {
            get => this.m_videoOffers;
            private set
            {
                if (this.m_videoOffers == value)
                    return;
                this.m_videoOffers = value;
                this.FirePropertyChanged(nameof(VideoOffers));
            }
        }

        public IList AppOffers
        {
            get => this.m_appOffers;
            private set
            {
                if (this.m_appOffers == value)
                    return;
                this.m_appOffers = value;
                this.FirePropertyChanged(nameof(AppOffers));
            }
        }

        public bool AuthenticationRequired
        {
            get => this.m_authorizationRequired;
            set
            {
                if (this.m_authorizationRequired == value)
                    return;
                this.m_authorizationRequired = value;
                this.FirePropertyChanged(nameof(AuthenticationRequired));
            }
        }

        public ResumePurchaseData ResumePurchaseData
        {
            get => this.m_resumePurchaseData;
            set
            {
                if (this.m_resumePurchaseData != null && !this.m_resumePurchaseData.Equals(value))
                    return;
                this.m_resumePurchaseData = value;
                this.FirePropertyChanged(nameof(ResumePurchaseData));
            }
        }

        public string AdditionalStatus
        {
            get => this.m_additionalStatus;
            set
            {
                if (!(this.m_additionalStatus != value))
                    return;
                this.m_additionalStatus = value;
                this.FirePropertyChanged(nameof(AdditionalStatus));
                this.UpdateState();
            }
        }

        public bool MultiplePlaybackOptions
        {
            get => this.m_fMultiplePlaybackOptions;
            private set
            {
                if (this.m_fMultiplePlaybackOptions == value)
                    return;
                this.m_fMultiplePlaybackOptions = value;
                this.FirePropertyChanged(nameof(MultiplePlaybackOptions));
            }
        }

        public bool MultipleResolutionOptions
        {
            get => this.m_fMultipleResolutionOptions;
            private set
            {
                if (this.m_fMultipleResolutionOptions == value)
                    return;
                this.m_fMultipleResolutionOptions = value;
                this.FirePropertyChanged(nameof(MultipleResolutionOptions));
            }
        }

        public bool PurchaseHD
        {
            get => this.m_fPurchaseHD;
            set
            {
                if (this.m_fPurchaseHD == value)
                    return;
                this.m_fPurchaseHD = value;
                this.FirePropertyChanged(nameof(PurchaseHD));
                this.UpdateVideoOffers();
                this.CalculateTotal();
                this.UpdateState();
            }
        }

        public bool RentVideos
        {
            get => this.m_fRentVideos;
            set
            {
                if (this.m_fRentVideos == value)
                    return;
                this.m_fRentVideos = value;
                this.FirePropertyChanged(nameof(RentVideos));
                this.UpdateVideoOffers();
                this.CalculateTotal();
                this.UpdateState();
            }
        }

        public bool StreamVideos
        {
            get => this.m_fStreamVideos;
            set
            {
                if (this.m_fStreamVideos == value)
                    return;
                this.m_fStreamVideos = value;
                this.FirePropertyChanged(nameof(StreamVideos));
                this.UpdateVideoOffers();
                this.CalculateTotal();
                this.UpdateState();
            }
        }

        public bool PurchaseTrials
        {
            get => this.m_fPurchaseTrials;
            set
            {
                if (this.m_fPurchaseTrials == value)
                    return;
                this.m_fPurchaseTrials = value;
                this.FirePropertyChanged(nameof(PurchaseTrials));
                this.UpdateAppOffers();
                this.CalculateTotal();
                this.UpdateState();
            }
        }

        public bool PurchaseSeason
        {
            get => this.m_fPurchaseSeason;
            set
            {
                if (this.m_fPurchaseSeason == value)
                    return;
                this.m_fPurchaseSeason = value;
                this.FirePropertyChanged(nameof(PurchaseSeason));
                this.UpdateVideoOffers();
                this.CalculateTotal();
                this.UpdateState();
            }
        }

        public int RentDeviceId
        {
            get => this.m_rentDeviceId;
            set
            {
                if (this.m_rentDeviceId == value)
                    return;
                this.m_rentDeviceId = value;
                this.FirePropertyChanged(nameof(RentDeviceId));
                this.UpdateState();
            }
        }

        public string RentDeviceName
        {
            get => this.m_rentDeviceName ?? string.Empty;
            set
            {
                if (!(this.m_rentDeviceName != value))
                    return;
                this.m_rentDeviceName = value;
                this.FirePropertyChanged(nameof(RentDeviceName));
            }
        }

        public string RentDeviceEndpointId
        {
            get => this.m_rentDeviceEndpointId ?? string.Empty;
            set
            {
                if (!(this.m_rentDeviceEndpointId != value))
                    return;
                this.m_rentDeviceEndpointId = value;
                this.FirePropertyChanged(nameof(RentDeviceEndpointId));
            }
        }

        public bool RentDeviceSupportsHD
        {
            get => this.m_rentDeviceSupportsHD;
            set
            {
                if (this.m_rentDeviceSupportsHD == value)
                    return;
                this.m_rentDeviceSupportsHD = value;
                this.FirePropertyChanged(nameof(RentDeviceSupportsHD));
            }
        }

        public void ChangeRentDevice(
          int deviceId,
          string deviceEndpointId,
          string deviceName,
          bool deviceSupportsHD)
        {
            if (deviceId == this.RentDeviceId)
                return;
            if (this.m_albumOfferCollection != null && this.m_albumOfferCollection.Items != null && (this.m_trackOfferCollection != null && this.m_trackOfferCollection.Items != null) && (this.m_videoOfferCollection != null && this.m_videoOfferCollection.Items != null && (this.m_appOfferCollection != null && this.m_appOfferCollection.Items != null)))
            {
                this.m_albumOfferCollection.Items.Clear();
                this.m_trackOfferCollection.Items.Clear();
                this.m_videoOfferCollection.Items.Clear();
                this.m_appOfferCollection.Items.Clear();
                this.UpdateOffers(this.m_albumOfferCollection, this.m_trackOfferCollection, this.m_videoOfferCollection, this.m_appOfferCollection, 0);
                this.AlbumOffers = (IList)null;
                this.TrackOffers = (IList)null;
                this.VideoOffers = (IList)null;
                this.AppOffers = (IList)null;
            }
            this.RentDeviceId = deviceId;
            this.RentDeviceEndpointId = deviceEndpointId;
            this.RentDeviceName = deviceName;
            this.RentDeviceSupportsHD = deviceSupportsHD;
        }

        public void RentDeviceCountMaxExceeded() => ShipAssert.Assert(false);

        public string DisplayPointsPrice
        {
            get => this.m_displayPointsPrice;
            private set
            {
                if (!(value != this.m_displayPointsPrice))
                    return;
                this.m_displayPointsPrice = value;
                this.FirePropertyChanged(nameof(DisplayPointsPrice));
            }
        }

        public string DisplayCurrencyPrice
        {
            get => this.m_displayCurrencyPrice;
            private set
            {
                if (!(value != this.m_displayCurrencyPrice))
                    return;
                this.m_displayCurrencyPrice = value;
                this.FirePropertyChanged(nameof(DisplayCurrencyPrice));
            }
        }

        public bool IsFree => this.HasPrice && !this.HasSubscriptionFreeTracks && this.TotalPoints == 0 && this.TotalCurrencyPrice == 0.0;

        private bool HasSubscriptionFreeTracks
        {
            get => this.m_hasSubscriptionFreeTracks;
            set
            {
                if (this.m_hasSubscriptionFreeTracks == value)
                    return;
                this.m_hasSubscriptionFreeTracks = value;
                this.FirePropertyChanged(nameof(HasSubscriptionFreeTracks));
                this.FirePropertyChanged("IsFree");
            }
        }

        private bool HasPrice => this.m_totalPoints >= 0 || this.m_totalCurrencyPrice >= 0.0;

        public int TotalPoints
        {
            get => this.m_totalPoints <= 0 ? 0 : this.m_totalPoints;
            private set
            {
                if (this.m_totalPoints == value)
                    return;
                this.m_totalPoints = value;
                this.FirePropertyChanged(nameof(TotalPoints));
                this.FirePropertyChanged("IsFree");
            }
        }

        public double TotalCurrencyPrice
        {
            get => this.m_totalCurrencyPrice <= 0.0 ? 0.0 : this.m_totalCurrencyPrice;
            private set
            {
                if (this.m_totalCurrencyPrice == value)
                    return;
                this.m_totalCurrencyPrice = value;
                this.FirePropertyChanged(nameof(TotalCurrencyPrice));
                this.FirePropertyChanged("IsFree");
            }
        }

        public int TotalSubscriptionFreeTracks
        {
            get => this.m_totalSubscriptionFreeTracks <= 0 ? 0 : this.m_totalSubscriptionFreeTracks;
            private set
            {
                if (this.m_totalSubscriptionFreeTracks == value)
                    return;
                this.m_totalSubscriptionFreeTracks = value;
                this.FirePropertyChanged(nameof(TotalSubscriptionFreeTracks));
            }
        }

        public bool InsufficientPoints
        {
            get => this.m_fInsufficientPoints;
            private set
            {
                if (this.m_fInsufficientPoints == value)
                    return;
                this.m_fInsufficientPoints = value;
                this.FirePropertyChanged(nameof(InsufficientPoints));
            }
        }

        public bool CanPurchase
        {
            get => this.m_fCanPurchase;
            private set
            {
                if (this.m_fCanPurchase == value)
                    return;
                this.m_fCanPurchase = value;
                this.FirePropertyChanged(nameof(CanPurchase));
            }
        }

        public bool CanDownload
        {
            get => this.m_fCanDownload;
            private set
            {
                if (this.m_fCanDownload == value)
                    return;
                this.m_fCanDownload = value;
                this.FirePropertyChanged(nameof(CanDownload));
            }
        }

        public string ErrorMessage
        {
            get => this.m_errorMessage ?? string.Empty;
            set
            {
                if (!(this.m_errorMessage != value))
                    return;
                this.m_errorMessage = value;
                this.FirePropertyChanged(nameof(ErrorMessage));
            }
        }

        public string ErrorWebHelpUrl
        {
            get => this.m_errorWebHelpUrl ?? string.Empty;
            set
            {
                if (!(this.m_errorWebHelpUrl != value))
                    return;
                this.m_errorWebHelpUrl = value;
                this.FirePropertyChanged(nameof(ErrorWebHelpUrl));
            }
        }

        public bool SubscriptionFreeTracksOnly
        {
            get => this.m_fSubscriptionFreeTracksOnly;
            private set
            {
                if (this.m_fSubscriptionFreeTracksOnly == value)
                    return;
                this.m_fSubscriptionFreeTracksOnly = value;
                this.FirePropertyChanged(nameof(SubscriptionFreeTracksOnly));
            }
        }

        public string Status
        {
            get => this.m_status ?? string.Empty;
            set
            {
                if (!(this.m_status != value))
                    return;
                this.m_status = value;
                this.FirePropertyChanged(nameof(Status));
            }
        }

        public DateTime VideoExpirationDate
        {
            get => this._videoExpirationDate;
            set
            {
                if (!(this._videoExpirationDate != value))
                    return;
                this._videoExpirationDate = value;
                this.FirePropertyChanged("ExpirationDate");
            }
        }

        public bool PurchaseComplete
        {
            get => this.m_fPurchaseComplete;
            set
            {
                if (this.m_fPurchaseComplete == value)
                    return;
                this.m_fPurchaseComplete = value;
                this.FirePropertyChanged(nameof(PurchaseComplete));
            }
        }

        public bool PurchasingPoints
        {
            get => this.m_fPurchasingPoints;
            private set
            {
                if (this.m_fPurchasingPoints == value)
                    return;
                this.m_fPurchasingPoints = value;
                this.FirePropertyChanged(nameof(PurchasingPoints));
            }
        }

        public BillingOffer BestPointsOffer
        {
            get => this.m_pointsOffer;
            private set
            {
                if (this.m_pointsOffer == value)
                    return;
                this.m_pointsOffer = value;
                this.FirePropertyChanged(nameof(BestPointsOffer));
            }
        }

        private BillingOfferHelper PointsHelper
        {
            get
            {
                if (this.m_pointsHelper == null)
                {
                    this.m_pointsHelper = new BillingOfferHelper();
                    EventHandler eventHandler = new EventHandler(this.OnPointsPurchaseCompletedOrFailed);
                    this.m_pointsHelper.PurchaseComplete += eventHandler;
                    this.m_pointsHelper.PurchaseFailed += eventHandler;
                }
                return this.m_pointsHelper;
            }
        }

        private void UpdateState()
        {
            this.InsufficientPoints = this.m_totalPoints >= 0 && this.m_pointsBalance >= 0 && this.m_totalPoints > this.m_pointsBalance;
            if (this.AlbumOffers != null && this.TrackOffers != null && (this.VideoOffers != null && this.AppOffers != null))
            {
                if (this.AlbumOffers.Count == 0 && this.TrackOffers.Count == 0 && (this.VideoOffers.Count == 0 && this.AppOffers.Count == 0))
                    this.Status = ZuneUI.Shell.LoadString(StringId.IDS_PURCHASE_NO_ITEMS);
                else if (this.PurchasingPoints)
                    this.Status = ZuneUI.Shell.LoadString(StringId.IDS_PURCHASE_POINTS_IN_PROGRESS);
                else if (this.InsufficientPoints)
                {
                    this.CalculateBestPointsOffer();
                    this.Status = ZuneUI.Shell.LoadString(StringId.IDS_PURCHASE_INSUFFICIENT_POINTS);
                }
                else
                    this.Status = this.TotalSubscriptionFreeTracks <= 0 ? (!this.MultipleResolutionOptions ? this.ConditionsOfPurchase : string.Format(ZuneUI.Shell.LoadString(this.PurchaseHD ? StringId.IDS_PURCHASE_HD_DESC_AND_CONDITIONS : StringId.IDS_PURCHASE_SD_DESC_AND_CONDITIONS), (object)this.ConditionsOfPurchase)) : (!this.SubscriptionFreeTracksOnly ? string.Format(ZuneUI.Shell.LoadString(StringId.IDS_PURCHASE_FREE_TRACKS_AND_NO_REFUNDS), (object)this.TotalSubscriptionFreeTracks) : ZuneUI.Shell.LoadString(StringId.IDS_PURCHASE_SUGGESTED_SONGS_NOTICE));
            }
            else
                this.Status = ZuneUI.Shell.LoadString(StringId.IDS_PURCHASE_CALC_TOTAL);
            bool flag1 = false;
            bool flag2 = false;
            if (this.AlbumOffers != null)
            {
                foreach (AlbumOffer albumOffer in (IEnumerable)this.AlbumOffers)
                {
                    if (!albumOffer.InCollection)
                    {
                        if (!albumOffer.PreviouslyPurchased)
                            flag2 = true;
                        else
                            flag1 = true;
                    }
                }
            }
            if (this.TrackOffers != null)
            {
                foreach (TrackOffer trackOffer in (IEnumerable)this.TrackOffers)
                {
                    if (!trackOffer.InCollection)
                    {
                        if (!trackOffer.PreviouslyPurchased)
                            flag2 = true;
                        else
                            flag1 = true;
                    }
                }
            }
            if (this.VideoOffers != null)
            {
                foreach (VideoOffer videoOffer in (IEnumerable)this.VideoOffers)
                {
                    if (!videoOffer.InCollection)
                    {
                        if (!videoOffer.PreviouslyPurchased)
                            flag2 = true;
                        else
                            flag1 = true;
                    }
                }
            }
            if (this.AppOffers != null)
            {
                foreach (AppOffer appOffer in (IEnumerable)this.AppOffers)
                {
                    if (!appOffer.InCollection)
                    {
                        if (!appOffer.PreviouslyPurchased)
                            flag2 = true;
                        else
                            flag1 = true;
                    }
                }
            }
            this.CanPurchase = flag2 && !this.InsufficientPoints;
            this.CanDownload = flag1 && !flag2;
        }

        private string ConditionsOfPurchase
        {
            get
            {
                string empty = string.Empty;
                string str = this.VideoOffers.Count <= 0 || this.RentVideos ? ZuneUI.Shell.LoadString(StringId.IDS_PURCHASE_NO_REFUNDS) : ((this.VideoExpirationDate - DateTime.UtcNow).Days <= 365 ? string.Format(ZuneUI.Shell.LoadString(StringId.IDS_PURCHASE_NO_REFUNDS_KNOWN_BLACKOUT), (object)this.VideoExpirationDate.ToLocalTime().ToShortDateString()) : ZuneUI.Shell.LoadString(StringId.IDS_PURCHASE_NO_REFUNDS_UNKNOWN_BLACKOUT));
                return !string.IsNullOrEmpty(this.AdditionalStatus) ? string.Format("{0} {1}", (object)str, (object)this.AdditionalStatus) : str;
            }
        }

        private void CalculateBestPointsOffer()
        {
            BillingOffer billingOffer1 = (BillingOffer)null;
            if (this.m_pointsOffers != null)
            {
                int num = this.TotalPoints - this.PointsBalance;
                foreach (BillingOffer billingOffer2 in (IEnumerable)this.m_pointsOffers.Items)
                {
                    if ((long)billingOffer2.Points >= (long)num && (billingOffer1 == null || billingOffer1.Points > billingOffer2.Points))
                        billingOffer1 = billingOffer2;
                }
            }
            this.BestPointsOffer = billingOffer1;
        }

        private void SetError(HRESULT hrError)
        {
            ErrorMapperResult descriptionAndUrl = Microsoft.Zune.ErrorMapperApi.ErrorMapperApi.GetMappedErrorDescriptionAndUrl(hrError.Int, eErrorCondition.eEC_Purchase);
            this.ErrorMessage = descriptionAndUrl.Description;
            this.ErrorWebHelpUrl = descriptionAndUrl.WebHelpUrl;
            this.CanPurchase = false;
            this.CanDownload = false;
            this.Status = (string)null;
        }

        private void OnGetBalancesComplete(int pointsBalance, int freeTrackBalance) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredGetBalancesComplete), (object)new int[2]
        {
      pointsBalance,
      freeTrackBalance
        });

        private void OnGetBalancesError(HRESULT hrError) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredGetBalancesError), (object)hrError);

        private void OnGetPointsOffersComplete(BillingOfferCollection pointsOffers) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredGetPointsOffersComplete), (object)pointsOffers);

        private void OnGetPointsOffersError(HRESULT hrError) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredGetPointsOffersError), (object)hrError);

        private void OnGetOffersComplete(
          AlbumOfferCollection albumOffers,
          TrackOfferCollection trackOffers,
          VideoOfferCollection videoOffers,
          AppOfferCollection appOffers,
          int subscriptionFreeTracks)
        {
            Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredGetOffersComplete), (object)new object[5]
            {
        (object) albumOffers,
        (object) trackOffers,
        (object) videoOffers,
        (object) appOffers,
        (object) subscriptionFreeTracks
            });
        }

        private void OnGetOffersError(HRESULT hrError) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredSetError), (object)hrError);

        private void OnResumePurchaseComplete(HRESULT hr) => this.OnPurchaseOffersComplete(hr, (string)null, (string)null);

        private void OnPurchaseOffersComplete(HRESULT hr, string redirectUrl, string handle)
        {
            if (this.AlbumOffers == null || this.TrackOffers == null || (this.VideoOffers == null || this.AppOffers == null))
                return;
            if (hr.IsSuccess)
            {
                ArrayList arrayList = new ArrayList(this.AlbumOffers.Count + this.TrackOffers.Count + this.VideoOffers.Count + this.AppOffers.Count);
                VideoPlaybackTrack videoPlaybackTrack = (VideoPlaybackTrack)null;
                foreach (AlbumOffer albumOffer in (IEnumerable)this.AlbumOffers)
                    arrayList.Add((object)albumOffer);
                foreach (TrackOffer trackOffer in (IEnumerable)this.TrackOffers)
                    arrayList.Add((object)trackOffer);
                foreach (VideoOffer videoOffer in (IEnumerable)this.VideoOffers)
                {
                    arrayList.Add((object)videoOffer);
                    if (videoOffer.IsStream && videoPlaybackTrack == null)
                        videoPlaybackTrack = new VideoPlaybackTrack(videoOffer.Id, videoOffer.Title, (string)null, (string)null, false, true, false, false, false, videoOffer.IsHD ? VideoDefinitionEnum.HD : VideoDefinitionEnum.SD);
                }
                foreach (AppOffer appOffer in (IEnumerable)this.AppOffers)
                    arrayList.Add((object)appOffer);
                if (this.StreamVideos && videoPlaybackTrack != null)
                    Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredPlayPurchasedStreams), (object)videoPlaybackTrack);
                Microsoft.Zune.Service.EDownloadFlags eDownloadFlags = Microsoft.Zune.Service.EDownloadFlags.None;
                if (this.RentVideos)
                {
                    eDownloadFlags |= Microsoft.Zune.Service.EDownloadFlags.Rental;
                    if (this.RentDeviceId > 0)
                        eDownloadFlags |= Microsoft.Zune.Service.EDownloadFlags.DeviceLicensed;
                }
                if (this.StreamVideos)
                    eDownloadFlags |= Microsoft.Zune.Service.EDownloadFlags.Stream;
                if (this.PurchaseHD)
                    eDownloadFlags |= Microsoft.Zune.Service.EDownloadFlags.HD;
                Download.Instance.DownloadContent((IList)arrayList, eDownloadFlags, this.RentDeviceEndpointId, new EventHandler(this.OnDownloadsAllPending));
            }
            else
                Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredSetError), (object)hr);
        }

        private void OnDownloadsAllPending(object sender, EventArgs e) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredDownloadsAllPending), (object)null);

        private void OnPointsPurchaseCompletedOrFailed(object sender, EventArgs args)
        {
            this.PurchasingPoints = false;
            if (this.PointsHelper.ErrorCode.IsError)
            {
                this.SetError(this.PointsHelper.ErrorCode);
                this.UpdateState();
            }
            else
            {
                if (this.m_pointsOffer != null)
                {
                    int num = this.m_pointsBalance + (int)this.m_pointsOffer.Points;
                    this.InsufficientPoints = this.m_totalPoints >= 0 && num >= 0 && this.m_totalPoints > num;
                }
                this.GetBalances();
            }
        }

        private void DeferredGetOffersComplete(object args)
        {
            object[] objArray = (object[])args;
            this.UpdateOffers((AlbumOfferCollection)objArray[0], (TrackOfferCollection)objArray[1], (VideoOfferCollection)objArray[2], (AppOfferCollection)objArray[3], (int)objArray[4]);
        }

        public void UpdateOffers(
          AlbumOfferCollection albums,
          TrackOfferCollection tracks,
          VideoOfferCollection videos,
          AppOfferCollection apps,
          int totalSubscriptionFreeTracks)
        {
            this.m_albumOfferCollection = albums;
            this.m_trackOfferCollection = tracks;
            this.m_videoOfferCollection = videos;
            this.m_appOfferCollection = apps;
            this.TotalSubscriptionFreeTracks = totalSubscriptionFreeTracks;
            this.AlbumOffers = this.m_albumOfferCollection != null ? this.m_albumOfferCollection.Items : (IList)null;
            this.TrackOffers = this.m_trackOfferCollection != null ? this.m_trackOfferCollection.Items : (IList)null;
            this.UpdateAppOffers();
            this.UpdateVideoOffers();
            this.CalculateTotal();
            this.UpdateState();
        }

        private void UpdateAppOffers()
        {
            IList list = (IList)null;
            if (this.m_appOfferCollection != null && this.m_appOfferCollection.Items != null)
            {
                list = (IList)new ArrayList(this.m_appOfferCollection.Items.Count);
                if (this.PurchaseTrials)
                {
                    foreach (AppOffer appOffer in (IEnumerable)this.m_appOfferCollection.Items)
                    {
                        if (appOffer.IsTrialPurchase)
                            list.Add((object)appOffer);
                    }
                }
                else
                {
                    foreach (AppOffer appOffer in (IEnumerable)this.m_appOfferCollection.Items)
                    {
                        if (!appOffer.IsTrialPurchase)
                            list.Add((object)appOffer);
                    }
                }
            }
            this.AppOffers = list;
        }

        private void UpdateVideoOffers()
        {
            bool flag = false;
            IList list1 = this.m_videoOfferCollection != null ? this.m_videoOfferCollection.Items : (IList)null;
            if (list1 != null)
            {
                foreach (VideoOffer videoOffer in (IEnumerable)list1)
                {
                    if (videoOffer.PreviouslyPurchased && !videoOffer.IsRental)
                        flag = true;
                }
            }
            int num1 = 0;
            int num2 = 0;
            if (list1 != null)
            {
                foreach (VideoOffer videoOffer in (IEnumerable)list1)
                {
                    if (videoOffer.IsRental == this.RentVideos && (!this.RentVideos || !flag))
                    {
                        if (videoOffer.IsStream)
                            ++num1;
                        else
                            ++num2;
                    }
                }
            }
            this.MultiplePlaybackOptions = num1 > 0 && num2 > 0;
            if (!this.MultiplePlaybackOptions)
                this.StreamVideos = num1 > 0;
            int num3 = 0;
            int num4 = 0;
            if (list1 != null)
            {
                foreach (VideoOffer videoOffer in (IEnumerable)list1)
                {
                    if (videoOffer.IsRental == this.RentVideos && videoOffer.IsStream == this.StreamVideos && (!this.RentVideos || !flag))
                    {
                        if (videoOffer.IsHD)
                            ++num3;
                        else
                            ++num4;
                    }
                }
            }
            this.MultipleResolutionOptions = num3 > 0 && num4 > 0;
            if (!this.MultipleResolutionOptions && list1 != null)
                this.PurchaseHD = num3 > 0;
            IList list2 = (IList)null;
            if (list1 != null)
            {
                list2 = (IList)new ArrayList(this.PurchaseHD ? num3 : num4);
                foreach (VideoOffer videoOffer in (IEnumerable)list1)
                {
                    if (videoOffer.IsRental == this.RentVideos && videoOffer.IsStream == this.StreamVideos && videoOffer.IsHD == this.PurchaseHD && (!this.RentVideos || !flag))
                        list2.Add((object)videoOffer);
                }
            }
            this.VideoExpirationDate = DateTime.MaxValue;
            if (list2 != null)
            {
                foreach (VideoOffer videoOffer in (IEnumerable)list2)
                {
                    DateTime? expirationDate = videoOffer.ExpirationDate;
                    DateTime? nullable = expirationDate;
                    DateTime videoExpirationDate = this.VideoExpirationDate;
                    if ((nullable.HasValue ? (nullable.GetValueOrDefault() < videoExpirationDate ? 1 : 0) : 0) != 0)
                        this.VideoExpirationDate = expirationDate.Value;
                }
            }
            this.VideoOffers = list2;
        }

        public int CalculateTotalPoints(bool isRental, bool isStream, bool isHD)
        {
            int num = 0;
            if (this.m_videoOfferCollection != null)
            {
                foreach (VideoOffer videoOffer in (IEnumerable)this.m_videoOfferCollection.Items)
                {
                    if (videoOffer.IsRental == isRental && videoOffer.IsStream == isStream && (videoOffer.IsHD == isHD && !videoOffer.InCollection) && !videoOffer.PreviouslyPurchased)
                        num += videoOffer.PriceInfo.PointsPrice;
                }
            }
            return num;
        }

        private void CalculateTotal()
        {
            int num = -1;
            bool flag = false;
            double price = -1.0;
            string str1 = (string)null;
            string str2 = (string)null;
            string currencyCode = (string)null;
            if (this.AlbumOffers != null)
            {
                foreach (AlbumOffer albumOffer in (IEnumerable)this.AlbumOffers)
                {
                    if (!albumOffer.InCollection && !albumOffer.PreviouslyPurchased)
                    {
                        if (num < 0)
                            num = 0;
                        num += albumOffer.PriceInfo.PointsPrice;
                    }
                }
            }
            if (this.TrackOffers != null)
            {
                foreach (TrackOffer trackOffer in (IEnumerable)this.TrackOffers)
                {
                    if (!trackOffer.InCollection && !trackOffer.PreviouslyPurchased)
                    {
                        if (num < 0)
                            num = 0;
                        num += trackOffer.PriceInfo.PointsPrice;
                        flag |= trackOffer.SubscriptionFree;
                    }
                }
            }
            if (this.VideoOffers != null)
            {
                foreach (VideoOffer videoOffer in (IEnumerable)this.VideoOffers)
                {
                    if (!videoOffer.InCollection && !videoOffer.PreviouslyPurchased)
                    {
                        if (num < 0)
                            num = 0;
                        num += videoOffer.PriceInfo.PointsPrice;
                    }
                }
            }
            if (this.AppOffers != null)
            {
                foreach (AppOffer appOffer in (IEnumerable)this.AppOffers)
                {
                    if (!appOffer.InCollection && !appOffer.PreviouslyPurchased)
                    {
                        if (price < 0.0)
                        {
                            str2 = appOffer.PriceInfo.DisplayPrice;
                            price = 0.0;
                        }
                        else
                            str2 = (string)null;
                        price += appOffer.PriceInfo.CurrencyPrice;
                        if (currencyCode == null || currencyCode.Equals(appOffer.PriceInfo.CurrencyCode, StringComparison.InvariantCultureIgnoreCase))
                            currencyCode = appOffer.PriceInfo.CurrencyCode;
                    }
                }
            }
            if (num > -1)
                str1 = string.Format(ZuneUI.Shell.LoadString(StringId.IDS_POINTS_TOTAL_FORMAT), (object)num);
            if (price > -1.0 && str2 == null)
                str2 = StringFormatHelper.FormatPrice(price, currencyCode);
            if (price > 0.0 && !string.IsNullOrEmpty(str2))
            {
                string taxString = FeatureEnablement.GetTaxString();
                if (!string.IsNullOrEmpty(taxString))
                    str2 = string.Format(ZuneUI.Shell.LoadString(StringId.IDS_CURRENCY_WITH_TAX), (object)str2, (object)taxString);
            }
            this.TotalPoints = num;
            this.TotalCurrencyPrice = price;
            this.DisplayPointsPrice = str1;
            this.DisplayCurrencyPrice = str2;
            this.HasSubscriptionFreeTracks = flag;
        }

        private void DeferredSetError(object args) => this.SetError((HRESULT)args);

        private void DeferredGetBalancesComplete(object arg)
        {
            int[] numArray = (int[])arg;
            this.m_fRequestingBalances = false;
            this.PointsBalance = numArray[0];
            this.SubscriptionFreeTrackBalance = numArray[1];
            this.IsBalanceUpdated = true;
            this.UpdateState();
        }

        private void DeferredGetBalancesError(object args)
        {
            this.m_fRequestingBalances = false;
            this.IsBalanceUpdated = false;
            this.SetError((HRESULT)args);
        }

        private void DeferredGetPointsOffersComplete(object args)
        {
            this.m_fRequestingPointsOffers = false;
            this.m_pointsOffers = (BillingOfferCollection)args;
            this.UpdateState();
        }

        private void DeferredGetPointsOffersError(object args)
        {
            this.m_fRequestingPointsOffers = false;
            this.SetError((HRESULT)args);
        }

        private void DeferredDownloadsAllPending(object args)
        {
            this.PurchaseComplete = true;
            if (this.SubscriptionFreeTrackBalance > 0 && (this.TrackOffers != null && this.TrackOffers.Count > 0 || this.AlbumOffers != null && this.AlbumOffers.Count > 0))
                SignIn.Instance.UpdateSubscriptionFreeTrackBalance();
            ClientConfiguration.Service.PurchaseHD = this.PurchaseHD;
            if (Purchase.PurchaseEvent == null)
                return;
            Purchase.PurchaseEvent((object)this, EventArgs.Empty);
        }

        private void DeferredPlayPurchasedStreams(object args) => SingletonModelItem<TransportControls>.Instance.PlayItem((object)(VideoPlaybackTrack)args);

        protected override void OnDispose(bool disposing)
        {
            base.OnDispose(disposing);
            if (!disposing)
                return;
            if (this.m_albumOfferCollection != null)
            {
                this.m_albumOfferCollection.Dispose();
                this.m_albumOfferCollection = (AlbumOfferCollection)null;
            }
            if (this.m_trackOfferCollection != null)
            {
                this.m_trackOfferCollection.Dispose();
                this.m_trackOfferCollection = (TrackOfferCollection)null;
            }
            if (this.m_videoOfferCollection != null)
            {
                this.m_videoOfferCollection.Dispose();
                this.m_videoOfferCollection = (VideoOfferCollection)null;
            }
            if (this.m_appOfferCollection != null)
            {
                this.m_appOfferCollection.Dispose();
                this.m_appOfferCollection = (AppOfferCollection)null;
            }
            if (this.m_pointsHelper != null)
            {
                this.m_pointsHelper.Dispose();
                this.m_pointsHelper = (BillingOfferHelper)null;
            }
            if (this.m_pointsOffers == null)
                return;
            this.m_pointsOffers.Dispose();
            this.m_pointsOffers = (BillingOfferCollection)null;
        }

        private Microsoft.Zune.Service.EPurchaseOffersFlags PurchaseOffersFlags
        {
            get
            {
                Microsoft.Zune.Service.EPurchaseOffersFlags epurchaseOffersFlags = Microsoft.Zune.Service.EPurchaseOffersFlags.None;
                if (this.PurchaseHD)
                    epurchaseOffersFlags |= Microsoft.Zune.Service.EPurchaseOffersFlags.PurchaseHD;
                if (this.RentVideos)
                    epurchaseOffersFlags |= Microsoft.Zune.Service.EPurchaseOffersFlags.RentVideos;
                if (this.StreamVideos)
                    epurchaseOffersFlags |= Microsoft.Zune.Service.EPurchaseOffersFlags.StreamVideos;
                if (this.PurchaseTrials)
                    epurchaseOffersFlags |= Microsoft.Zune.Service.EPurchaseOffersFlags.PurchaseTrials;
                return epurchaseOffersFlags;
            }
        }
    }
}
