// Decompiled with JetBrains decompiler
// Type: ZuneXml.Video
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using System;

namespace ZuneXml
{
    internal abstract class Video : Media
    {
        private int _dbMediaId = -1;

        internal void SetLibraryId(int dbMediaId)
        {
            if (this._dbMediaId == dbMediaId)
                return;
            this._dbMediaId = dbMediaId;
            this.FirePropertyChanged("LibraryId");
        }

        internal int GetLibraryId()
        {
            int dbMediaId = -1;
            if (this.Id != Guid.Empty)
                ZuneApplication.Service.InVisibleCollection(this.Id, EContentType.Video, out dbMediaId);
            return dbMediaId;
        }

        internal int GetPointsPrice()
        {
            int num1 = -1;
            int num2 = -1;
            Right right1 = this.Rights.GetOfferRight(MediaRightsEnum.Purchase, VideoDefinitionEnum.HD, VideoDefinitionEnum.XD, PriceTypeEnum.Points) ?? this.Rights.GetOfferRight(MediaRightsEnum.AlbumPurchase, VideoDefinitionEnum.HD, VideoDefinitionEnum.XD, PriceTypeEnum.Points);
            if (right1 != null)
                num2 = right1.PointsPrice;
            int num3 = -1;
            Right right2 = this.Rights.GetOfferRight(MediaRightsEnum.Purchase, VideoDefinitionEnum.SD, VideoDefinitionEnum.XD, PriceTypeEnum.Points) ?? this.Rights.GetOfferRight(MediaRightsEnum.AlbumPurchase, VideoDefinitionEnum.SD, VideoDefinitionEnum.XD, PriceTypeEnum.Points);
            if (right2 != null)
                num3 = right2.PointsPrice;
            if (num2 > 0 && num3 > 0)
                num1 = ClientConfiguration.Service.PurchaseHD ? num2 : num3;
            else if (num2 > 0)
                num1 = num2;
            else if (num3 > 0)
                num1 = num3;
            return num1;
        }

        internal int GetPointsRental()
        {
            int num1 = -1;
            int num2 = -1;
            Right offerRight1 = this.Rights.GetOfferRight(MediaRightsEnum.Rent, VideoDefinitionEnum.HD, VideoDefinitionEnum.XD, PriceTypeEnum.Points);
            if (offerRight1 != null)
                num2 = offerRight1.PointsPrice;
            int num3 = -1;
            Right offerRight2 = this.Rights.GetOfferRight(MediaRightsEnum.Rent, VideoDefinitionEnum.SD, VideoDefinitionEnum.XD, PriceTypeEnum.Points);
            if (offerRight2 != null)
                num3 = offerRight2.PointsPrice;
            if (num2 > 0 && num3 > 0)
                num1 = ClientConfiguration.Service.PurchaseHD ? num2 : num3;
            else if (num2 > 0)
                num1 = num2;
            else if (num3 > 0)
                num1 = num3;
            return num1;
        }

        internal bool GetHasPreview() => this.Rights.HasRights(MediaRightsEnum.Preview, VideoDefinitionEnum.None, PriceTypeEnum.None) || this.Rights.HasRights(MediaRightsEnum.PreviewStream, VideoDefinitionEnum.None, PriceTypeEnum.None);

        internal bool GetCanPreview() => (this.Rights.HasRights(MediaRightsEnum.Preview, VideoDefinitionEnum.None, PriceTypeEnum.None) || this.Rights.HasRights(MediaRightsEnum.PreviewStream, VideoDefinitionEnum.None, PriceTypeEnum.None)) && !this.IsParentallyBlocked;

        internal bool GetCanSubscriptionPlay() => this.Rights.HasRights(MediaRightsEnum.SubscriptionStream, VideoDefinitionEnum.None, PriceTypeEnum.None) && ZuneApplication.Service.IsSignedInWithSubscription() && !this.IsParentallyBlocked;

        internal bool GetCanPurchase() => this.GetCanPurchaseHD() || this.GetCanPurchaseSD();

        internal bool GetCanPurchaseHD() => this.Rights.HasRights(MediaRightsEnum.Purchase, VideoDefinitionEnum.HD, VideoDefinitionEnum.XD, PriceTypeEnum.Points) || this.Rights.HasRights(MediaRightsEnum.PurchaseStream, VideoDefinitionEnum.HD, PriceTypeEnum.Points) || this.Rights.HasRights(MediaRightsEnum.AlbumPurchase, VideoDefinitionEnum.HD, PriceTypeEnum.Points);

        internal bool GetCanPurchaseSD() => this.Rights.HasRights(MediaRightsEnum.Purchase, VideoDefinitionEnum.SD, VideoDefinitionEnum.XD, PriceTypeEnum.Points) || this.Rights.HasRights(MediaRightsEnum.PurchaseStream, VideoDefinitionEnum.SD, PriceTypeEnum.Points) || this.Rights.HasRights(MediaRightsEnum.AlbumPurchase, VideoDefinitionEnum.SD, VideoDefinitionEnum.XD, PriceTypeEnum.Points);

        internal bool GetCanPurchaseSeason() => this.GetCanPurchaseSeasonHD() || this.GetCanPurchaseSeasonSD();

        internal bool GetCanPurchaseSeasonHD() => this.Rights.HasRights(MediaRightsEnum.SeasonPurchase, VideoDefinitionEnum.HD, VideoDefinitionEnum.XD, PriceTypeEnum.Points) || this.Rights.HasRights(MediaRightsEnum.SeasonPurchaseStream, VideoDefinitionEnum.HD, PriceTypeEnum.Points);

        internal bool GetCanPurchaseSeasonSD() => this.Rights.HasRights(MediaRightsEnum.SeasonPurchase, VideoDefinitionEnum.SD, VideoDefinitionEnum.XD, PriceTypeEnum.Points) || this.Rights.HasRights(MediaRightsEnum.SeasonPurchaseStream, VideoDefinitionEnum.SD, PriceTypeEnum.Points);

        internal bool GetCanPurchaseAlbumOnly() => !this.Rights.HasRights(MediaRightsEnum.Purchase, VideoDefinitionEnum.None, PriceTypeEnum.Points) && this.Rights.HasRights(MediaRightsEnum.AlbumPurchase, VideoDefinitionEnum.None, PriceTypeEnum.Points);

        internal bool GetCanRent() => this.GetCanRentHD() || this.GetCanRentSD();

        internal bool GetCanRentHD() => this.Rights.HasRights(MediaRightsEnum.Rent, VideoDefinitionEnum.HD, PriceTypeEnum.Points) || this.Rights.HasRights(MediaRightsEnum.RentStream, VideoDefinitionEnum.HD, PriceTypeEnum.Points);

        internal bool GetCanRentSD() => this.Rights.HasRights(MediaRightsEnum.Rent, VideoDefinitionEnum.SD, PriceTypeEnum.Points) || this.Rights.HasRights(MediaRightsEnum.RentStream, VideoDefinitionEnum.SD, PriceTypeEnum.Points);

        internal bool GetInCollection()
        {
            bool flag = false;
            if (this.Id != Guid.Empty)
                flag = ZuneApplication.Service.InVisibleCollection(this.Id, EContentType.Video);
            return flag;
        }

        internal bool GetInCollectionShortcut()
        {
            bool flag = false;
            if (this.Id != Guid.Empty)
                flag = ZuneApplication.Service.GetMediaStatus(this.Id, EContentType.Video) == EMediaStatus.StatusInCollectionShortcut;
            return flag;
        }

        internal bool GetIsDownloading()
        {
            bool flag = false;
            if (this.Id != Guid.Empty)
            {
                bool fIsHidden = false;
                flag = ZuneApplication.Service.IsDownloading(this.Id, EContentType.Video, out bool _, out fIsHidden);
            }
            return flag;
        }

        internal bool GetIsParentallyBlocked(string rating)
        {
            string empty = string.Empty;
            string system;
            switch (this)
            {
                case Movie _:
                    system = "Movies";
                    break;
                case TVVideo _:
                    system = "TV";
                    break;
                default:
                    return false;
            }
            return ZuneApplication.Service.BlockRatedContent(system, rating);
        }

        protected Video(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal abstract DateTime ReleaseDate { get; }

        internal abstract TimeSpan Duration { get; }

        internal abstract int PointsPrice { get; }

        internal abstract bool HasPreview { get; }

        internal abstract bool CanPreview { get; }

        internal abstract bool CanSubscriptionPlay { get; }

        internal abstract bool CanPurchase { get; }

        internal abstract bool CanPurchaseHD { get; }

        internal abstract bool CanPurchaseSD { get; }

        internal abstract bool CanPurchaseSeason { get; }

        internal abstract bool CanPurchaseSeasonHD { get; }

        internal abstract bool CanPurchaseSeasonSD { get; }

        internal abstract bool CanRent { get; }

        internal abstract bool CanRentHD { get; }

        internal abstract bool CanRentSD { get; }

        internal abstract bool CanPurchaseAlbumOnly { get; }

        internal abstract bool CanSync { get; }

        internal abstract bool InCollection { get; }

        internal abstract bool InCollectionShortcut { get; }

        internal abstract bool IsDownloading { get; }

        internal abstract bool IsParentallyBlocked { get; }
    }
}
