// Decompiled with JetBrains decompiler
// Type: ZuneXml.MovieTrailer
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneXml
{
    internal class MovieTrailer : Movie
    {
        internal static XmlDataProviderObject ConstructMovieTrailerObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new MovieTrailer(owner, objectTypeCookie);
        }

        internal MovieTrailer(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

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
