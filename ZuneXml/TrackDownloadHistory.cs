// Decompiled with JetBrains decompiler
// Type: ZuneXml.TrackDownloadHistory
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;
using ZuneUI;

namespace ZuneXml
{
    internal class TrackDownloadHistory : TrackHistory
    {
        internal override bool InCollection => this.GetInCollection();

        internal override bool IsDownloading => this.GetIsDownloading();

        internal override int LibraryId => this.GetLibraryId();

        internal override bool CanPlay => false;

        internal override bool CanSync => false;

        internal override bool CanBurn => false;

        internal override int PointsPrice => 0;

        internal override bool CanPurchaseSubscriptionFree => false;

        internal override bool CanDownload
        {
            get
            {
                bool flag = false;
                if (!this.InCollection && this.MediaInstances != null)
                {
                    foreach (MediaInstance mediaInstance in (IEnumerable)this.MediaInstances)
                    {
                        flag = mediaInstance.IsDownloadable;
                        if (!flag)
                            break;
                    }
                }
                if (flag && Download.Instance.GetErrorCode(this.Id) == HRESULT._ZUNE_E_NO_SUBSCRIPTION_DOWNLOAD_RIGHTS.Int)
                    flag = false;
                return flag;
            }
        }

        internal override bool CanPurchase => true;

        internal override bool CanPreview => false;

        internal override bool CanSubscriptionPlay => false;

        internal override bool CanPurchaseMP3 => false;

        internal override bool CanPurchaseAlbumOnly => false;

        internal override bool Actionable => false;

        internal override int UserRating => 0;

        internal override bool IsParentallyBlocked => false;

        internal static XmlDataProviderObject ConstructTrackDownloadHistoryObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new TrackDownloadHistory(owner, objectTypeCookie);
        }

        internal TrackDownloadHistory(DataProviderQuery owner, object resultTypeCookie)
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
                    return (object)this.Actionable;
                case "UserRating":
                    return (object)this.UserRating;
                case "LibraryId":
                    return (object)this.LibraryId;
                case "PointsPrice":
                    return (object)this.PointsPrice;
                case "HasPoints":
                    return (object)this.HasPoints;
                case "CanPlay":
                    return (object)this.CanPlay;
                case "CanPreview":
                    return (object)this.CanPreview;
                case "CanSubscriptionPlay":
                    return (object)this.CanSubscriptionPlay;
                case "CanDownload":
                    return (object)this.CanDownload;
                case "CanPurchase":
                    return (object)this.CanPurchase;
                case "CanPurchaseFree":
                    return (object)this.CanPurchaseFree;
                case "CanPurchaseMP3":
                    return (object)this.CanPurchaseMP3;
                case "CanPurchaseAlbumOnly":
                    return (object)this.CanPurchaseAlbumOnly;
                case "CanPurchaseSubscriptionFree":
                    return (object)this.CanPurchaseSubscriptionFree;
                case "CanSync":
                    return (object)this.CanSync;
                case "CanBurn":
                    return (object)this.CanBurn;
                case "InCollection":
                    return (object)this.InCollection;
                case "IsDownloading":
                    return (object)this.IsDownloading;
                case "IsParentallyBlocked":
                    return (object)this.IsParentallyBlocked;
                case "Ordinal":
                    return (object)this.Ordinal;
                case "SortTitle":
                    return (object)this.SortTitle;
                case "ImageId":
                    return (object)this.ImageId;
                case "Rights":
                    return (object)this.Rights;
                case "Artists":
                    return (object)this.Artists;
                case "Popularity":
                    return (object)this.Popularity;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
