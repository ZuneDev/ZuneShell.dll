// Decompiled with JetBrains decompiler
// Type: ZuneXml.Track
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using System;
using System.Collections;
using ZuneUI;

namespace ZuneXml
{
    internal class Track : Media
    {
        private int _dbMediaId = -1;
        private int _userRating;
        private int _ordinal;

        internal virtual bool CanPlay
        {
            get
            {
                bool flag = this.Rights.HasRights(MediaRightsEnum.Preview, AudioEncodingEnum.WMA);
                if (!flag)
                    flag = this.CanSubscriptionPlay;
                if (!flag && this.Id != Guid.Empty)
                    flag = ZuneApplication.Service.InVisibleCollection(this.Id, EContentType.MusicTrack);
                return flag;
            }
        }

        internal virtual bool CanPreview => this.Rights.HasRights(MediaRightsEnum.Preview, AudioEncodingEnum.WMA);

        internal virtual bool CanSubscriptionPlay => this.Rights.HasRights(MediaRightsEnum.SubscriptionStream, AudioEncodingEnum.WMA) && ZuneApplication.Service.IsSignedInWithSubscription();

        internal virtual bool CanDownload
        {
            get
            {
                bool flag = false;
                if (ZuneApplication.Service.CanDownloadSubscriptionContent())
                {
                    bool fIsDownloadPending = false;
                    bool fIsHidden = false;
                    if (this.Id != Guid.Empty && (!ZuneApplication.Service.IsDownloading(this.Id, EContentType.MusicTrack, out fIsDownloadPending, out fIsHidden) || fIsHidden) && !ZuneApplication.Service.InVisibleCollection(this.Id, EContentType.MusicTrack))
                        flag = this.Rights.HasRights(MediaRightsEnum.SubscriptionDownload, AudioEncodingEnum.WMA);
                }
                return flag && !this.IsParentallyBlocked;
            }
        }

        internal virtual bool CanPurchase => this.SelectPurchaseRight() != null;

        internal virtual bool CanPurchaseFree
        {
            get
            {
                bool flag = false;
                Right offer = null;
                if (this.Rights.HasOfferRights(MediaRightsEnum.Purchase, AudioEncodingEnum.MP3, PriceTypeEnum.Points, out offer) && offer.IsFree)
                    flag = true;
                else if (this.Rights.HasOfferRights(MediaRightsEnum.AlbumPurchase, AudioEncodingEnum.MP3, PriceTypeEnum.Points, out offer) && offer.IsFree)
                    flag = true;
                else if (this.Rights.HasOfferRights(MediaRightsEnum.Purchase, AudioEncodingEnum.WMA, PriceTypeEnum.Points, out offer) && offer.IsFree)
                    flag = true;
                else if (this.Rights.HasOfferRights(MediaRightsEnum.AlbumPurchase, AudioEncodingEnum.WMA, PriceTypeEnum.Points, out offer) && offer.IsFree)
                    flag = true;
                return flag;
            }
        }

        internal virtual bool CanPurchaseMP3
        {
            get
            {
                Right offer;
                return this.Rights.HasOfferRights(MediaRightsEnum.Purchase, AudioEncodingEnum.MP3, PriceTypeEnum.Points, out offer) || this.Rights.HasOfferRights(MediaRightsEnum.AlbumPurchase, AudioEncodingEnum.MP3, PriceTypeEnum.Points, out offer);
            }
        }

        internal virtual bool CanPurchaseAlbumOnly
        {
            get
            {
                Right offer;
                return !this.Rights.HasOfferRights(MediaRightsEnum.Purchase, AudioEncodingEnum.WMA, PriceTypeEnum.Points, out offer) && !this.Rights.HasOfferRights(MediaRightsEnum.Purchase, AudioEncodingEnum.MP3, PriceTypeEnum.Points, out offer) && (this.Rights.HasOfferRights(MediaRightsEnum.AlbumPurchase, AudioEncodingEnum.WMA, PriceTypeEnum.Points, out offer) || this.Rights.HasOfferRights(MediaRightsEnum.AlbumPurchase, AudioEncodingEnum.MP3, PriceTypeEnum.Points, out offer));
            }
        }

        internal virtual bool CanPurchaseSubscriptionFree
        {
            get
            {
                Right offer;
                return ZuneApplication.Service.GetSubscriptionFreeTrackBalance() > 0 && (this.Rights.HasOfferRights(MediaRightsEnum.SubscriptionFreePurchase, AudioEncodingEnum.WMA, PriceTypeEnum.Points, out offer) || this.Rights.HasOfferRights(MediaRightsEnum.SubscriptionFreePurchase, AudioEncodingEnum.MP3, PriceTypeEnum.Points, out offer));
            }
        }

        internal virtual bool HasPoints
        {
            get
            {
                bool flag = false;
                Right right = this.SelectPurchaseRight();
                if (right != null)
                    flag = right.HasPoints;
                return flag;
            }
        }

        internal virtual bool InCollection => this.GetInCollection();

        internal virtual bool IsDownloading => this.GetIsDownloading();

        internal virtual int LibraryId
        {
            get => this.GetLibraryId();
            set
            {
                if (this._dbMediaId == value)
                    return;
                this._dbMediaId = value;
                this.FirePropertyChanged(nameof(LibraryId));
            }
        }

        internal virtual bool CanSync => this.CanPurchase || this.InCollection || this.IsDownloading || this.CanDownload;

        internal virtual bool CanBurn => this.CanPurchase;

        internal virtual int PointsPrice
        {
            get
            {
                int num = -1;
                Right offer;
                if (this.Rights.HasOfferRights(MediaRightsEnum.Purchase, AudioEncodingEnum.MP3, PriceTypeEnum.Points, out offer))
                    num = offer.PointsPrice;
                else if (this.Rights.HasOfferRights(MediaRightsEnum.Purchase, AudioEncodingEnum.WMA, PriceTypeEnum.Points, out offer))
                    num = offer.PointsPrice;
                else if (this.Rights.HasOfferRights(MediaRightsEnum.AlbumPurchase, AudioEncodingEnum.MP3, PriceTypeEnum.Points, out offer))
                    num = offer.PointsPrice;
                else if (this.Rights.HasOfferRights(MediaRightsEnum.AlbumPurchase, AudioEncodingEnum.WMA, PriceTypeEnum.Points, out offer))
                    num = offer.PointsPrice;
                return num;
            }
        }

        internal virtual bool Actionable => this.Rights.HasAnyRights();

        internal virtual int Ordinal
        {
            get => this._ordinal;
            set
            {
                if (this._ordinal == value)
                    return;
                this._ordinal = value;
                this.FirePropertyChanged(nameof(Ordinal));
            }
        }

        internal virtual int UserRating
        {
            get
            {
                this._userRating = RecommendationsHelper.GetUserRating(this);
                return this._userRating;
            }
            set
            {
                if (this._userRating == value)
                    return;
                this._userRating = value;
                this.FirePropertyChanged(nameof(UserRating));
            }
        }

        internal virtual bool IsParentallyBlocked => this.Explicit && ZuneApplication.Service.BlockExplicitContent();

        protected bool GetInCollection()
        {
            bool flag = false;
            if (this.Id != Guid.Empty)
                flag = ZuneApplication.Service.InVisibleCollection(this.Id, EContentType.MusicTrack, out int _);
            return flag;
        }

        protected bool GetIsDownloading()
        {
            bool flag = false;
            if (this.Id != Guid.Empty)
            {
                bool fIsDownloadPending = false;
                bool fIsHidden = false;
                flag = ZuneApplication.Service.IsDownloading(this.Id, EContentType.MusicTrack, out fIsDownloadPending, out fIsHidden);
            }
            return flag;
        }

        protected int GetLibraryId()
        {
            int dbMediaId = -1;
            if (this.Id != Guid.Empty)
                ZuneApplication.Service.InVisibleCollection(this.Id, EContentType.MusicTrack, out dbMediaId);
            return dbMediaId;
        }

        private Right SelectPurchaseRight()
        {
            Right offer;
            if (!this.Rights.HasOfferRights(MediaRightsEnum.Purchase, AudioEncodingEnum.MP3, PriceTypeEnum.Points, out offer) && !this.Rights.HasOfferRights(MediaRightsEnum.AlbumPurchase, AudioEncodingEnum.MP3, PriceTypeEnum.Points, out offer) && !this.Rights.HasOfferRights(MediaRightsEnum.Purchase, AudioEncodingEnum.WMA, PriceTypeEnum.Points, out offer) && !this.Rights.HasOfferRights(MediaRightsEnum.AlbumPurchase, AudioEncodingEnum.WMA, PriceTypeEnum.Points, out offer))
                offer = null;
            return offer;
        }

        internal static XmlDataProviderObject ConstructTrackObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return new Track(owner, objectTypeCookie);
        }

        internal Track(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal TimeSpan Duration => (TimeSpan)base.GetProperty(nameof(Duration));

        internal int TrackNumber => (int)base.GetProperty(nameof(TrackNumber));

        internal int DiscNumber => (int)base.GetProperty(nameof(DiscNumber));

        internal string AlbumTitle => (string)base.GetProperty(nameof(AlbumTitle));

        internal Guid AlbumId => (Guid)base.GetProperty(nameof(AlbumId));

        internal MiniArtist AlbumArtist => (MiniArtist)base.GetProperty(nameof(AlbumArtist));

        internal string ArtistName => (string)base.GetProperty(nameof(ArtistName));

        internal Genre PrimaryGenre => (Genre)base.GetProperty(nameof(PrimaryGenre));

        internal bool Explicit => (bool)base.GetProperty(nameof(Explicit));

        internal int PlayCount => (int)base.GetProperty(nameof(PlayCount));

        internal string ReferrerContext => (string)base.GetProperty(nameof(ReferrerContext));

        internal Guid MusicVideoId => (Guid)base.GetProperty(nameof(MusicVideoId));

        internal override Guid Id => (Guid)base.GetProperty(nameof(Id));

        internal override string Title => (string)base.GetProperty(nameof(Title));

        internal override string SortTitle => (string)base.GetProperty(nameof(SortTitle));

        internal override MiniArtist PrimaryArtist => (MiniArtist)base.GetProperty(nameof(PrimaryArtist));

        internal override IList Artists => (IList)base.GetProperty(nameof(Artists));

        internal override double Popularity => (double)base.GetProperty(nameof(Popularity));

        internal override Guid ImageId => (Guid)base.GetProperty(nameof(ImageId));

        internal override MediaRights Rights => (MediaRights)base.GetProperty(nameof(Rights));

        public override object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "Actionable":
                    return Actionable;
                case "UserRating":
                    return UserRating;
                case "LibraryId":
                    return LibraryId;
                case "PointsPrice":
                    return PointsPrice;
                case "HasPoints":
                    return HasPoints;
                case "CanPlay":
                    return CanPlay;
                case "CanPreview":
                    return CanPreview;
                case "CanSubscriptionPlay":
                    return CanSubscriptionPlay;
                case "CanDownload":
                    return CanDownload;
                case "CanPurchase":
                    return CanPurchase;
                case "CanPurchaseFree":
                    return CanPurchaseFree;
                case "CanPurchaseMP3":
                    return CanPurchaseMP3;
                case "CanPurchaseAlbumOnly":
                    return CanPurchaseAlbumOnly;
                case "CanPurchaseSubscriptionFree":
                    return CanPurchaseSubscriptionFree;
                case "CanSync":
                    return CanSync;
                case "CanBurn":
                    return CanBurn;
                case "InCollection":
                    return InCollection;
                case "IsDownloading":
                    return IsDownloading;
                case "IsParentallyBlocked":
                    return IsParentallyBlocked;
                case "Ordinal":
                    return Ordinal;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
