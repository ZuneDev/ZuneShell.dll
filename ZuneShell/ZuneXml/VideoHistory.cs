// Decompiled with JetBrains decompiler
// Type: ZuneXml.VideoHistory
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;
using ZuneUI;

namespace ZuneXml
{
    internal class VideoHistory : Video
    {
        private MediaTypeEnum _mediaTypeEnum;

        public override void SetProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case "MediaType":
                    this._mediaTypeEnum = SchemaHelper.ToMediaTypeEnum((string)value);
                    break;
            }
            base.SetProperty(propertyName, value);
        }

        internal override bool CanSync => false;

        internal override bool InCollection => this.GetInCollection();

        internal override bool InCollectionShortcut => this.GetInCollectionShortcut();

        internal override bool IsDownloading => this.GetIsDownloading();

        internal override bool CanPreview => false;

        internal override bool CanSubscriptionPlay => false;

        internal override int PointsPrice => 0;

        internal bool CanDownloadHD
        {
            get
            {
                bool flag = false;
                if (!this.InCollection && this.MediaInstances != null)
                {
                    foreach (MediaInstance mediaInstance in MediaInstances)
                    {
                        if ((mediaInstance.RightEnum == MediaRightsEnum.AlbumPurchase || mediaInstance.RightEnum == MediaRightsEnum.Purchase || (mediaInstance.RightEnum == MediaRightsEnum.SeasonPurchase || mediaInstance.RightEnum == MediaRightsEnum.Rent)) && (mediaInstance.VideoDefinitionEnum == VideoDefinitionEnum.HD && mediaInstance.VideoResolutionEnum == VideoResolutionEnum.VR_720P))
                        {
                            flag = mediaInstance.IsDownloadable;
                            if (!flag)
                                break;
                        }
                    }
                }
                if (flag && Download.Instance.GetErrorCode(this.Id) == HRESULT._NS_E_MEDIA_DOWNLOAD_MAXIMUM_EXCEEDED.Int)
                    flag = false;
                return flag;
            }
        }

        internal bool CanDownload
        {
            get
            {
                bool flag = false;
                if (!this.InCollection && this.MediaInstances != null)
                {
                    foreach (MediaInstance mediaInstance in MediaInstances)
                    {
                        if ((mediaInstance.RightEnum == MediaRightsEnum.AlbumPurchase || mediaInstance.RightEnum == MediaRightsEnum.Purchase || (mediaInstance.RightEnum == MediaRightsEnum.SeasonPurchase || mediaInstance.RightEnum == MediaRightsEnum.Rent)) && (mediaInstance.VideoDefinitionEnum == VideoDefinitionEnum.XD || mediaInstance.VideoDefinitionEnum == VideoDefinitionEnum.SD || mediaInstance.VideoDefinitionEnum == VideoDefinitionEnum.HD && mediaInstance.VideoResolutionEnum == VideoResolutionEnum.VR_720P))
                        {
                            flag = mediaInstance.IsDownloadable;
                            if (!flag)
                                break;
                        }
                    }
                }
                if (flag && Download.Instance.GetErrorCode(this.Id) == HRESULT._NS_E_MEDIA_DOWNLOAD_MAXIMUM_EXCEEDED.Int)
                    flag = false;
                return flag;
            }
        }

        internal override bool CanPurchase => !this.CanDownload && this.MediaInstances != null && this.MediaInstances.Count > 0;

        internal override bool CanPurchaseAlbumOnly => false;

        internal override bool CanRent => !this.CanDownload && this.MediaInstances != null && this.MediaInstances.Count > 0;

        internal override DateTime ReleaseDate => DateTime.MinValue;

        internal override TimeSpan Duration => TimeSpan.Zero;

        internal override double Popularity => 0.0;

        internal override bool HasPreview => false;

        internal override bool CanPurchaseHD => false;

        internal override bool CanPurchaseSD => false;

        internal override bool CanPurchaseSeason => false;

        internal override bool CanPurchaseSeasonHD => false;

        internal override bool CanPurchaseSeasonSD => false;

        internal override bool CanRentHD => false;

        internal override bool CanRentSD => false;

        internal override bool IsParentallyBlocked => false;

        internal override Guid ImageId => Guid.Empty;

        internal override string SortTitle => string.Empty;

        internal override MediaRights Rights => (MediaRights)null;

        internal override IList Artists => (IList)null;

        internal virtual bool IsTVSeason => this._mediaTypeEnum == MediaTypeEnum.TVSeason;

        internal static XmlDataProviderObject ConstructVideoHistoryObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return new VideoHistory(owner, objectTypeCookie);
        }

        internal VideoHistory(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal DateTime Date => (DateTime)base.GetProperty(nameof(Date));

        internal string MediaType => (string)base.GetProperty(nameof(MediaType));

        internal string SeasonTitle => (string)base.GetProperty(nameof(SeasonTitle));

        internal int SeasonNumber => (int)base.GetProperty(nameof(SeasonNumber));

        internal string SeriesTitle => (string)base.GetProperty(nameof(SeriesTitle));

        internal IList SeasonEpisodes => (IList)base.GetProperty(nameof(SeasonEpisodes));

        internal IList MediaInstances => (IList)base.GetProperty(nameof(MediaInstances));

        internal override string Title => (string)base.GetProperty(nameof(Title));

        internal override Guid Id => (Guid)base.GetProperty(nameof(Id));

        internal override MiniArtist PrimaryArtist => (MiniArtist)base.GetProperty(nameof(PrimaryArtist));

        public override object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "CanDownload":
                    return CanDownload;
                case "CanDownloadHD":
                    return CanDownloadHD;
                case "IsTVSeason":
                    return IsTVSeason;
                case "ReleaseDate":
                    return ReleaseDate;
                case "Duration":
                    return Duration;
                case "PointsPrice":
                    return PointsPrice;
                case "HasPreview":
                    return HasPreview;
                case "CanPreview":
                    return CanPreview;
                case "CanSubscriptionPlay":
                    return CanSubscriptionPlay;
                case "CanPurchase":
                    return CanPurchase;
                case "CanPurchaseHD":
                    return CanPurchaseHD;
                case "CanPurchaseSD":
                    return CanPurchaseSD;
                case "CanPurchaseSeason":
                    return CanPurchaseSeason;
                case "CanPurchaseSeasonHD":
                    return CanPurchaseSeasonHD;
                case "CanPurchaseSeasonSD":
                    return CanPurchaseSeasonSD;
                case "CanRent":
                    return CanRent;
                case "CanRentHD":
                    return CanRentHD;
                case "CanRentSD":
                    return CanRentSD;
                case "CanPurchaseAlbumOnly":
                    return CanPurchaseAlbumOnly;
                case "CanSync":
                    return CanSync;
                case "InCollection":
                    return InCollection;
                case "InCollectionShortcut":
                    return InCollectionShortcut;
                case "IsDownloading":
                    return IsDownloading;
                case "IsParentallyBlocked":
                    return IsParentallyBlocked;
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
