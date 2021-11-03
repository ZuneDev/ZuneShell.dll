// Decompiled with JetBrains decompiler
// Type: ZuneXml.BadgeData
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneXml
{
    internal class BadgeData : XmlDataProviderObject
    {
        internal int TypeId => (int)GetBadgeType(this);

        internal static BadgeType GetBadgeType(BadgeData badgeData)
        {
            BadgeType badgeType = BadgeType.Invalid;
            switch (badgeData.Type)
            {
                case "ActiveAlbumListener_Bronze":
                    badgeType = BadgeType.BronzeAlbum;
                    break;
                case "ActiveAlbumListener_Silver":
                    badgeType = BadgeType.SilverAlbum;
                    break;
                case "ActiveAlbumListener_Gold":
                    badgeType = BadgeType.GoldAlbum;
                    break;
                case "ActiveArtistListener_Bronze":
                    badgeType = BadgeType.BronzeArtist;
                    break;
                case "ActiveArtistListener_Silver":
                    badgeType = BadgeType.SilverArtist;
                    break;
                case "ActiveArtistListener_Gold":
                    badgeType = BadgeType.GoldArtist;
                    break;
                case "ActiveForumsBadge_Bronze":
                    badgeType = BadgeType.BronzeForums;
                    break;
                case "ActiveForumsBadge_Silver":
                    badgeType = BadgeType.SilverForums;
                    break;
                case "ActiveForumsBadge_Gold":
                    badgeType = BadgeType.GoldForums;
                    break;
                case "ActiveReviewBadge_Bronze":
                    badgeType = BadgeType.BronzeReview;
                    break;
                case "ActiveReviewBadge_Silver":
                    badgeType = BadgeType.SilverReview;
                    break;
                case "ActiveReviewBadge_Gold":
                    badgeType = BadgeType.GoldReview;
                    break;
            }
            return badgeType;
        }

        internal static XmlDataProviderObject ConstructBadgeDataObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return new BadgeData(owner, objectTypeCookie);
        }

        internal BadgeData(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal string Title => (string)base.GetProperty(nameof(Title));

        internal string Image => (string)base.GetProperty(nameof(Image));

        internal string Type => (string)base.GetProperty(nameof(Type));

        internal Guid MediaId => (Guid)base.GetProperty(nameof(MediaId));

        internal string MediaType => (string)base.GetProperty(nameof(MediaType));

        internal string Description => (string)base.GetProperty(nameof(Description));

        public override object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "TypeId":
                    return TypeId;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
