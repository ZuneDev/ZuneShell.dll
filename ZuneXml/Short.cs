// Decompiled with JetBrains decompiler
// Type: ZuneXml.Short
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneXml
{
    internal class Short : TVVideo
    {
        internal static XmlDataProviderObject ConstructShortObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new Short(owner, objectTypeCookie);
        }

        internal Short(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal Guid BackgroundImageId => (Guid)base.GetProperty(nameof(BackgroundImageId));

        internal override Guid Id => (Guid)base.GetProperty(nameof(Id));

        internal override string Title => (string)base.GetProperty(nameof(Title));

        internal override string SortTitle => (string)base.GetProperty(nameof(SortTitle));

        internal override string Description => (string)base.GetProperty(nameof(Description));

        internal override DateTime ReleaseDate => (DateTime)base.GetProperty(nameof(ReleaseDate));

        internal override string Copyright => (string)base.GetProperty(nameof(Copyright));

        internal override string Rating => (string)base.GetProperty(nameof(Rating));

        internal override TimeSpan Duration => (TimeSpan)base.GetProperty(nameof(Duration));

        internal override string ProductionCompany => (string)base.GetProperty(nameof(ProductionCompany));

        internal override Guid ImageId => (Guid)base.GetProperty(nameof(ImageId));

        internal override MediaRights Rights => (MediaRights)base.GetProperty(nameof(Rights));

        internal override Network Network => (Network)base.GetProperty(nameof(Network));

        internal override double Popularity => (double)base.GetProperty(nameof(Popularity));

        public override object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
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
