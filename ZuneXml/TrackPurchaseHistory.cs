// Decompiled with JetBrains decompiler
// Type: ZuneXml.TrackPurchaseHistory
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;
using ZuneUI;

namespace ZuneXml
{
    internal class TrackPurchaseHistory : TrackHistory
    {
        internal override bool InCollection => this.GetInCollection();

        internal override bool IsDownloading => this.GetIsDownloading();

        internal override bool CanPlay => false;

        internal override bool CanSync => false;

        internal override bool CanBurn => false;

        internal override int PointsPrice => 0;

        internal override bool CanPurchaseSubscriptionFree => false;

        internal override bool CanPurchaseAlbumOnly => false;

        internal override bool CanDownload
        {
            get
            {
                bool flag = false;
                if (!this.InCollection && this.MediaInstances != null)
                {
                    foreach (MediaInstance mediaInstance in MediaInstances)
                    {
                        flag = mediaInstance.IsDownloadable;
                        if (!flag)
                            break;
                    }
                }
                if (flag && Download.Instance.GetErrorCode(this.Id) == HRESULT._NS_E_MEDIA_DOWNLOAD_MAXIMUM_EXCEEDED.Int)
                    flag = false;
                return flag;
            }
        }

        internal override bool CanPurchase => !this.CanDownload;

        internal override int LibraryId => -1;

        internal override bool CanPreview => false;

        internal override bool CanSubscriptionPlay => false;

        internal override bool CanPurchaseMP3 => false;

        internal override bool Actionable => false;

        internal override int UserRating => 0;

        internal override bool IsParentallyBlocked => false;

        internal static XmlDataProviderObject ConstructTrackPurchaseHistoryObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return new TrackPurchaseHistory(owner, objectTypeCookie);
        }

        internal TrackPurchaseHistory(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal override DateTime Date => (DateTime)base.GetProperty(nameof(Date));

        internal override IList MediaInstances => (IList)base.GetProperty(nameof(MediaInstances));

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
                case "SortTitle":
                    return SortTitle;
                case "ImageId":
                    return ImageId;
                case "Rights":
                    return Rights;
                case "Artists":
                    return Artists;
                case "Popularity":
                    return Popularity;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
