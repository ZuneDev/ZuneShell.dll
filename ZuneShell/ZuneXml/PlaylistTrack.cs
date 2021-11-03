// Decompiled with JetBrains decompiler
// Type: ZuneXml.PlaylistTrack
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneXml
{
    internal class PlaylistTrack : Track
    {
        internal static XmlDataProviderObject ConstructPlaylistTrackObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return new PlaylistTrack(owner, objectTypeCookie);
        }

        internal PlaylistTrack(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal int Index => (int)base.GetProperty(nameof(Index));

        internal string Content => (string)base.GetProperty(nameof(Content));

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
                case "ImageId":
                    return ImageId;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
