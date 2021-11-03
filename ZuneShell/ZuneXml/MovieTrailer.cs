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
            return new MovieTrailer(owner, objectTypeCookie);
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
                    return PointsRental;
                case "Languages":
                    return Languages;
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
                case "PrimaryArtist":
                    return PrimaryArtist;
                case "Artists":
                    return Artists;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
