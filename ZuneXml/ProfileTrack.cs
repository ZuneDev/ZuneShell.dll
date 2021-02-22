// Decompiled with JetBrains decompiler
// Type: ZuneXml.ProfileTrack
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneXml
{
    internal class ProfileTrack : PlaylistTrack
    {
        internal static XmlDataProviderObject ConstructProfileTrackObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new ProfileTrack(owner, objectTypeCookie);
        }

        internal ProfileTrack(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal string TopArtistName => (string)base.GetProperty(nameof(TopArtistName));

        internal Guid TopArtistId => (Guid)base.GetProperty(nameof(TopArtistId));

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
                case "ImageId":
                    return (object)this.ImageId;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
