// Decompiled with JetBrains decompiler
// Type: ZuneXml.Movie
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;

namespace ZuneXml
{
    internal class Movie : RatableVideo
    {
        internal override int PointsPrice => this.GetPointsPrice();

        internal int PointsRental => this.GetPointsRental();

        internal override bool HasPreview => this.GetHasPreview();

        internal override bool CanPreview => this.GetCanPreview();

        internal override bool CanSubscriptionPlay => this.GetCanSubscriptionPlay();

        internal override bool CanPurchase => this.GetCanPurchase();

        internal override bool CanPurchaseHD => this.GetCanPurchaseHD();

        internal override bool CanPurchaseSD => this.GetCanPurchaseSD();

        internal override bool CanPurchaseSeason => false;

        internal override bool CanPurchaseSeasonHD => false;

        internal override bool CanPurchaseSeasonSD => false;

        internal override bool CanPurchaseAlbumOnly => false;

        internal override bool CanRent => this.GetCanRent();

        internal override bool CanRentHD => this.GetCanRentHD();

        internal override bool CanRentSD => this.GetCanRentSD();

        internal override bool InCollection => this.GetInCollection();

        internal override bool InCollectionShortcut => this.GetInCollectionShortcut();

        internal override bool IsDownloading => this.GetIsDownloading();

        internal override bool IsParentallyBlocked => this.GetIsParentallyBlocked(this.Rating);

        internal override MiniArtist PrimaryArtist => (MiniArtist)null;

        internal override IList Artists => (IList)null;

        internal IList Languages => this.Rights.Languages;

        internal static XmlDataProviderObject ConstructMovieObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new Movie(owner, objectTypeCookie);
        }

        internal Movie(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal string ProductionCompany => (string)base.GetProperty(nameof(ProductionCompany));

        internal IList Contributors => (IList)base.GetProperty(nameof(Contributors));

        internal IList Genres => (IList)base.GetProperty(nameof(Genres));

        internal override Guid Id => (Guid)base.GetProperty(nameof(Id));

        internal override string Title => (string)base.GetProperty(nameof(Title));

        internal override string Rating => (string)base.GetProperty(nameof(Rating));

        internal override string Description => (string)base.GetProperty(nameof(Description));

        internal override string SortTitle => (string)base.GetProperty(nameof(SortTitle));

        internal override double Popularity => (double)base.GetProperty(nameof(Popularity));

        internal override DateTime ReleaseDate => (DateTime)base.GetProperty(nameof(ReleaseDate));

        internal override Guid ImageId => (Guid)base.GetProperty(nameof(ImageId));

        internal override MediaRights Rights => (MediaRights)base.GetProperty(nameof(Rights));

        internal override TimeSpan Duration => (TimeSpan)base.GetProperty(nameof(Duration));

        public override object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "PointsRental":
                    return (object)this.PointsRental;
                case "Languages":
                    return (object)this.Languages;
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
                case "PrimaryArtist":
                    return (object)this.PrimaryArtist;
                case "Artists":
                    return (object)this.Artists;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
