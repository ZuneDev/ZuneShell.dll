// Decompiled with JetBrains decompiler
// Type: ZuneXml.Episode
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;

namespace ZuneXml
{
    internal class Episode : TVVideo
    {
        internal IList Languages => this.Rights.Languages;

        internal static XmlDataProviderObject ConstructEpisodeObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new Episode(owner, objectTypeCookie);
        }

        internal Episode(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal int EpisodeNumber => (int)base.GetProperty(nameof(EpisodeNumber));

        internal int SeasonNumber => (int)base.GetProperty(nameof(SeasonNumber));

        internal Series Series => (Series)base.GetProperty(nameof(Series));

        internal IList Categories => (IList)base.GetProperty(nameof(Categories));

        internal override Guid Id => (Guid)base.GetProperty(nameof(Id));

        internal override string Title => (string)base.GetProperty(nameof(Title));

        internal override string SortTitle => (string)base.GetProperty(nameof(SortTitle));

        internal override string Description => (string)base.GetProperty(nameof(Description));

        internal override DateTime ReleaseDate => (DateTime)base.GetProperty(nameof(ReleaseDate));

        internal override string Copyright => (string)base.GetProperty(nameof(Copyright));

        internal override string Rating => (string)base.GetProperty(nameof(Rating));

        internal override TimeSpan Duration => (TimeSpan)base.GetProperty(nameof(Duration));

        internal override double Popularity => (double)base.GetProperty(nameof(Popularity));

        internal override Network Network => (Network)base.GetProperty(nameof(Network));

        internal override string ProductionCompany => (string)base.GetProperty(nameof(ProductionCompany));

        internal override Guid ImageId => (Guid)base.GetProperty(nameof(ImageId));

        internal override MediaRights Rights => (MediaRights)base.GetProperty(nameof(Rights));

        public override object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
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
