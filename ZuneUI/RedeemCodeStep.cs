// Decompiled with JetBrains decompiler
// Type: ZuneUI.RedeemCodeStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Service;
using System;
using System.Collections;

namespace ZuneUI
{
    public class RedeemCodeStep : AccountManagementStep
    {
        private TokenDetails _tokenDetails;
        private TokenDetails _confirmedTokenDetails;
        private EClientTypeFlags _tokenClientTypes;
        private bool _tokenExpired;
        private bool _tokenInvalid;
        private Command _redeemMediaCommand;
        private AlbumOfferCollection _albumOffers;
        private TrackOfferCollection _trackOffers;
        private VideoOfferCollection _videoOffers;
        private AppOfferCollection _appOffers;

        public RedeemCodeStep(Wizard owner, AccountManagementWizardState state)
          : base(owner, state, false)
        {
            this.Description = Shell.LoadString(StringId.IDS_BILLING_PREPAID_CODE_TITLE);
            this.DetailDescription = Shell.LoadString(StringId.IDS_BILLING_PREPAID_CODE_HEADER);
            WizardPropertyEditor wizardPropertyEditor = (WizardPropertyEditor)new RedeemCodePropertyEditor();
            this._redeemMediaCommand = new Command();
            this.Initialize(wizardPropertyEditor);
        }

        public override string UI => "res://ZuneShellResources!AccountInfo.uix#RedeemCodeStep";

        public TokenDetails TokenDetails
        {
            get => this._tokenDetails;
            private set
            {
                if (this._tokenDetails == value)
                    return;
                this._tokenDetails = value;
                this.FirePropertyChanged(nameof(TokenDetails));
                this.FirePropertyChanged("TokenSupported");
                this.FirePropertyChanged("MatchingBillingOffer");
            }
        }

        public TokenDetails ConfirmedTokenDetails
        {
            get => this._confirmedTokenDetails;
            private set
            {
                if (this._confirmedTokenDetails == value)
                    return;
                this._confirmedTokenDetails = value;
                this.FirePropertyChanged(nameof(ConfirmedTokenDetails));
            }
        }

        public bool TokenSupported
        {
            get
            {
                if (this._tokenDetails != null)
                {
                    switch (this._tokenDetails.TokenType)
                    {
                        case ETokenType.Media:
                            if (!this.TokenClientTypesSupported)
                                return false;
                            return this._tokenDetails.PurchaseOfferType == EPurchaseOfferType.TVEpisode || this._tokenDetails.PurchaseOfferType == EPurchaseOfferType.Movie;
                        case ETokenType.Points:
                        case ETokenType.Subscription:
                            return true;
                    }
                }
                return false;
            }
        }

        public EClientTypeFlags TokenClientTypes
        {
            get => this._tokenClientTypes;
            private set
            {
                if (this._tokenClientTypes == value)
                    return;
                this._tokenClientTypes = value;
                this.FirePropertyChanged(nameof(TokenClientTypes));
            }
        }

        private bool TokenClientTypesSupported => this._tokenClientTypes == EClientTypeFlags.None || (this._tokenClientTypes & EClientTypeFlags.PC) == EClientTypeFlags.PC;

        public bool TokenInvalid
        {
            get => this._tokenInvalid;
            private set
            {
                if (this._tokenInvalid == value)
                    return;
                this._tokenInvalid = value;
                this.FirePropertyChanged(nameof(TokenInvalid));
            }
        }

        public bool TokenExpired
        {
            get => this._tokenExpired;
            private set
            {
                if (this._tokenExpired == value)
                    return;
                this._tokenExpired = value;
                this.FirePropertyChanged(nameof(TokenExpired));
            }
        }

        public BillingOffer MatchingBillingOffer
        {
            get
            {
                BillingOffer billingOffer = (BillingOffer)null;
                if (this.TokenDetails != null)
                {
                    if (this.TokenDetails.TokenType == ETokenType.Points)
                        billingOffer = new BillingOffer(this.TokenDetails.BillingOfferId, EBillingOfferType.Points, this.TokenDetails.OfferName);
                    else if (this.TokenDetails.TokenType == ETokenType.Subscription)
                        billingOffer = new BillingOffer(this.TokenDetails.BillingOfferId, EBillingOfferType.Subscription, this.TokenDetails.OfferName);
                }
                return billingOffer;
            }
        }

        public AlbumOfferCollection AlbumOffers
        {
            get => this._albumOffers;
            private set
            {
                if (this._albumOffers == value)
                    return;
                this._albumOffers = value;
                this.FirePropertyChanged(nameof(AlbumOffers));
            }
        }

        public TrackOfferCollection TrackOffers
        {
            get => this._trackOffers;
            private set
            {
                if (this._trackOffers == value)
                    return;
                this._trackOffers = value;
                this.FirePropertyChanged(nameof(TrackOffers));
            }
        }

        public VideoOfferCollection VideoOffers
        {
            get => this._videoOffers;
            private set
            {
                if (this._videoOffers == value)
                    return;
                this._videoOffers = value;
                this.FirePropertyChanged(nameof(VideoOffers));
            }
        }

        public AppOfferCollection AppOffers
        {
            get => this._appOffers;
            private set
            {
                if (this._appOffers == value)
                    return;
                this._appOffers = value;
                this.FirePropertyChanged(nameof(AppOffers));
            }
        }

        public Command RedeemMediaCommand => this._redeemMediaCommand;

        public string MediaTitle => this._videoOffers != null && this._videoOffers.Items.Count > 0 ? ((Offer)this._videoOffers.Items[0]).Title : (string)null;

        public bool IsMediaRental => this._videoOffers != null && this.VideoOffers.Items.Count > 0 && ((VideoOffer)this.VideoOffers.Items[0]).IsRental;

        public bool IsMediaSD
        {
            get
            {
                if (this._videoOffers != null)
                {
                    foreach (VideoOffer videoOffer in (IEnumerable)this._videoOffers.Items)
                    {
                        if (!videoOffer.IsHD)
                            return true;
                    }
                }
                return false;
            }
        }

        public bool IsMediaHD
        {
            get
            {
                if (this._videoOffers != null)
                {
                    foreach (VideoOffer videoOffer in (IEnumerable)this._videoOffers.Items)
                    {
                        if (videoOffer.IsHD)
                            return true;
                    }
                }
                return false;
            }
        }

        public string CreateRedeemCodeUrl(bool returnArguments)
        {
            string endPointUri = Microsoft.Zune.Service.Service.GetEndPointUri(EServiceEndpointId.SEID_AccountManagement);
            string urlPath = endPointUri + "/client/RedeemCode.ashx";
            string str1;
            if (returnArguments)
            {
                string str2 = Microsoft.Zune.Service.Service.GetEndPointUri(EServiceEndpointId.SEID_ZuneNet) + "/social/articles/backtosoftware.htm";
                str1 = UrlHelper.MakeUrl(endPointUri + "/client/RedeemCode.ashx", "ru", str2, "aru", str2);
            }
            else
                str1 = UrlHelper.MakeUrl(urlPath);
            return str1;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            this.ServiceDeactivationRequestsDone = this.TokenDetails != null;
        }

        internal override bool OnMovingNext()
        {
            string uncommittedValue = this.GetUncommittedValue(RedeemCodePropertyEditor.Code) as string;
            string committedValue = this.GetCommittedValue(RedeemCodePropertyEditor.Code) as string;
            if (!uncommittedValue.Equals(committedValue, StringComparison.InvariantCultureIgnoreCase))
            {
                this.TokenDetails = (TokenDetails)null;
                this.TokenExpired = false;
                this.TokenClientTypes = EClientTypeFlags.None;
                this.ConfirmedTokenDetails = (TokenDetails)null;
                this.ServiceDeactivationRequestsDone = false;
                this.AlbumOffers = (AlbumOfferCollection)null;
                this.TrackOffers = (TrackOfferCollection)null;
                this.VideoOffers = (VideoOfferCollection)null;
                this.AppOffers = (AppOfferCollection)null;
            }
            if (this.ServiceDeactivationRequestsDone)
            {
                if (this.ConfirmedTokenDetails != null)
                {
                    if (!this.TokenSupported)
                    {
                        ZuneShell.DefaultInstance.Execute("Web\\" + this.CreateRedeemCodeUrl(true), (IDictionary)null);
                        this._owner.Cancel();
                        return false;
                    }
                    if (this.AlbumOffers == null && this.TrackOffers == null && (this.VideoOffers == null && this.AppOffers == null))
                        return base.OnMovingNext();
                    this._redeemMediaCommand.Invoke(InvokePolicy.Synchronous);
                    return false;
                }
                bool flag1 = this.TokenDetails != null && (this.TokenDetails.TokenType == ETokenType.Points || this.TokenDetails.TokenType == ETokenType.Subscription);
                bool flag2 = !this.TokenExpired && this.TokenClientTypes != EClientTypeFlags.None;
                if (flag1 || flag2)
                    this.ConfirmedTokenDetails = this.TokenDetails;
                return false;
            }
            this.StartDeactivationRequests((object)uncommittedValue);
            this.WizardPropertyEditor.Commit();
            return false;
        }

        protected override void OnStartDeactivationRequests(object state)
        {
            RedeemCodeStep.ServiceData serviceData = this.VerifyToken(state as string);
            if (!serviceData.TokenInvalid && serviceData.TokenDetails.TokenType == ETokenType.Media)
                Microsoft.Zune.Service.Service.Instance.GetOfferDetails(serviceData.TokenDetails.MediaOfferId, new GetOfferDetailsCompleteCallback(this.OnGetOfferDetailsSuccess), new GetOfferDetailsErrorCallback(this.OnGetOfferDetailsError), (object)serviceData);
            else
                this.EndDeactivationRequests((object)serviceData);
        }

        protected override void OnEndDeactivationRequests(object args)
        {
            RedeemCodeStep.ServiceData serviceData = (RedeemCodeStep.ServiceData)args;
            this.TokenClientTypes = serviceData.TokenClientTypes;
            this.TokenDetails = serviceData.TokenDetails;
            this.TokenInvalid = serviceData.TokenInvalid;
            this.TokenExpired = serviceData.TokenExpired;
            this.AlbumOffers = serviceData.AlbumOffers;
            this.TrackOffers = serviceData.TrackOffers;
            this.VideoOffers = serviceData.VideoOffers;
            this.AppOffers = serviceData.AppOffers;
        }

        private RedeemCodeStep.ServiceData VerifyToken(string token)
        {
            RedeemCodeStep.ServiceData serviceData = new RedeemCodeStep.ServiceData();
            if (((HRESULT)Microsoft.Zune.Service.Service.Instance.VerifyToken(token, out serviceData.TokenDetails)).IsError)
                serviceData.TokenInvalid = true;
            return serviceData;
        }

        private void OnGetOfferDetailsSuccess(
          AlbumOfferCollection albums,
          TrackOfferCollection tracks,
          VideoOfferCollection videos,
          EClientTypeFlags clientTypes,
          object state)
        {
            if (albums == null || tracks == null || videos == null)
            {
                this.OnGetOfferDetailsError(HRESULT._E_UNEXPECTED, state);
            }
            else
            {
                RedeemCodeStep.ServiceData serviceData = (RedeemCodeStep.ServiceData)state;
                this.ZeroOfferPoints((IEnumerable)albums.Items);
                this.ZeroOfferPoints((IEnumerable)tracks.Items);
                this.ZeroOfferPoints((IEnumerable)videos.Items);
                serviceData.AlbumOffers = albums;
                serviceData.TrackOffers = tracks;
                serviceData.VideoOffers = videos;
                serviceData.TokenClientTypes = clientTypes;
                serviceData.TokenExpired = this.IsTokenExpired(albums.Items, tracks.Items, videos.Items);
                serviceData.AppOffers = Microsoft.Zune.Service.Service.Instance.CreateEmptyAppCollection();
                this.EndDeactivationRequests((object)serviceData);
            }
        }

        private bool IsTokenExpired(IList albums, IList tracks, IList videos)
        {
            if (albums != null && albums.Count > 0 || tracks != null && tracks.Count > 0 || (videos == null || videos.Count <= 0))
                return false;
            foreach (VideoOffer video in (IEnumerable)videos)
            {
                if (!video.ExpirationDate.HasValue || video.ExpirationDate.Value > DateTime.UtcNow)
                    return false;
            }
            return true;
        }

        private void OnGetOfferDetailsError(HRESULT hr, object state)
        {
            RedeemCodeStep.ServiceData serviceData = (RedeemCodeStep.ServiceData)state;
            if (hr.IsError)
                serviceData.TokenInvalid = true;
            this.EndDeactivationRequests((object)serviceData);
        }

        private void ZeroOfferPoints(IEnumerable offers)
        {
            if (offers == null)
                return;
            foreach (Offer offer in offers)
                offer.PriceInfo.MakeFree();
        }

        private class ServiceData
        {
            public TokenDetails TokenDetails;
            public EClientTypeFlags TokenClientTypes;
            public bool TokenInvalid;
            public bool TokenExpired;
            public AlbumOfferCollection AlbumOffers;
            public TrackOfferCollection TrackOffers;
            public VideoOfferCollection VideoOffers;
            public AppOfferCollection AppOffers;
        }
    }
}
