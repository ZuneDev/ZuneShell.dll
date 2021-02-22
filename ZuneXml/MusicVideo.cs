// Decompiled with JetBrains decompiler
// Type: ZuneXml.MusicVideo
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Util;
using System;
using System.Collections;

namespace ZuneXml
{
    internal class MusicVideo : Video
    {
        internal virtual int LibraryId
        {
            get => this.GetLibraryId();
            set => this.SetLibraryId(value);
        }

        internal override bool CanSync => this.CanPurchase || this.InCollection || this.IsDownloading;

        internal override int PointsPrice
        {
            get
            {
                int num = -1;
                Right right = this.Rights.GetOfferRight(MediaRightsEnum.Purchase, VideoDefinitionEnum.None, PriceTypeEnum.Points) ?? this.Rights.GetOfferRight(MediaRightsEnum.AlbumPurchase, VideoDefinitionEnum.None, PriceTypeEnum.Points);
                if (right != null)
                    num = right.PointsPrice;
                return num;
            }
        }

        internal override bool HasPreview => this.GetHasPreview();

        internal override bool CanPreview => this.GetCanPreview();

        internal override bool CanSubscriptionPlay => FeatureEnablement.IsFeatureEnabled(Features.eSubscriptionMusicVideoStreaming) && this.GetCanSubscriptionPlay();

        internal override bool CanPurchase
        {
            get
            {
                if (this.Rights.HasRights(MediaRightsEnum.Purchase, VideoDefinitionEnum.None, PriceTypeEnum.Points))
                    return true;
                return this.Rights.HasRights(MediaRightsEnum.AlbumPurchase, VideoDefinitionEnum.None, PriceTypeEnum.Points) && this.Album.Id != Guid.Empty;
            }
        }

        internal override bool CanPurchaseHD => false;

        internal override bool CanPurchaseSD => false;

        internal override bool CanPurchaseSeason => false;

        internal override bool CanPurchaseSeasonHD => false;

        internal override bool CanPurchaseSeasonSD => false;

        internal override bool CanPurchaseAlbumOnly => this.GetCanPurchaseAlbumOnly();

        internal override bool CanRent => false;

        internal override bool CanRentHD => false;

        internal override bool CanRentSD => false;

        internal override bool InCollection => this.GetInCollection();

        internal override bool InCollectionShortcut => this.GetInCollectionShortcut();

        internal override bool IsDownloading => this.GetIsDownloading();

        internal override bool IsParentallyBlocked => this.Explicit && ZuneApplication.Service.BlockExplicitContent();

        internal static XmlDataProviderObject ConstructMusicVideoObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new MusicVideo(owner, objectTypeCookie);
        }

        internal MusicVideo(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal string Label => (string)base.GetProperty(nameof(Label));

        internal Guid TrackId => (Guid)base.GetProperty(nameof(TrackId));

        internal MiniAlbum Album => (MiniAlbum)base.GetProperty(nameof(Album));

        internal Genre PrimaryGenre => (Genre)base.GetProperty(nameof(PrimaryGenre));

        internal bool Explicit => (bool)base.GetProperty(nameof(Explicit));

        internal int TrackNumber => (int)base.GetProperty(nameof(TrackNumber));

        internal override string Title => (string)base.GetProperty(nameof(Title));

        internal override Guid Id => (Guid)base.GetProperty(nameof(Id));

        internal override string SortTitle => (string)base.GetProperty(nameof(SortTitle));

        internal override double Popularity => (double)base.GetProperty(nameof(Popularity));

        internal override DateTime ReleaseDate => (DateTime)base.GetProperty(nameof(ReleaseDate));

        internal override TimeSpan Duration => (TimeSpan)base.GetProperty(nameof(Duration));

        internal override Guid ImageId => (Guid)base.GetProperty(nameof(ImageId));

        internal override MiniArtist PrimaryArtist => (MiniArtist)base.GetProperty(nameof(PrimaryArtist));

        internal override IList Artists => (IList)base.GetProperty(nameof(Artists));

        internal override MediaRights Rights => (MediaRights)base.GetProperty(nameof(Rights));

        public override object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "LibraryId":
                    return (object)this.LibraryId;
                case "PointsPrice":
                    return (object)this.PointsPrice;
                case "HasPreview":
                    return (object)this.HasPreview;
                case "CanPreview":
                    return (object)this.CanPreview;
                case "CanSubscriptionPlay":
                    return (object)this.CanSubscriptionPlay;
                case "CanPurchase":
                    return (object)this.CanPurchase;
                case "CanPurchaseHD":
                    return (object)this.CanPurchaseHD;
                case "CanPurchaseSD":
                    return (object)this.CanPurchaseSD;
                case "CanPurchaseSeason":
                    return (object)this.CanPurchaseSeason;
                case "CanPurchaseSeasonHD":
                    return (object)this.CanPurchaseSeasonHD;
                case "CanPurchaseSeasonSD":
                    return (object)this.CanPurchaseSeasonSD;
                case "CanRent":
                    return (object)this.CanRent;
                case "CanRentHD":
                    return (object)this.CanRentHD;
                case "CanRentSD":
                    return (object)this.CanRentSD;
                case "CanPurchaseAlbumOnly":
                    return (object)this.CanPurchaseAlbumOnly;
                case "CanSync":
                    return (object)this.CanSync;
                case "InCollection":
                    return (object)this.InCollection;
                case "InCollectionShortcut":
                    return (object)this.InCollectionShortcut;
                case "IsDownloading":
                    return (object)this.IsDownloading;
                case "IsParentallyBlocked":
                    return (object)this.IsParentallyBlocked;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
