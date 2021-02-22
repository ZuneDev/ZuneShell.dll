// Decompiled with JetBrains decompiler
// Type: ZuneXml.RecommendedAlbum
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneXml
{
    internal class RecommendedAlbum : Album
    {
        internal static XmlDataProviderObject ConstructRecommendedAlbumObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new RecommendedAlbum(owner, objectTypeCookie);
        }

        internal RecommendedAlbum(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal IList Reasons => (IList)base.GetProperty(nameof(Reasons));

        internal string ReferrerContext => (string)base.GetProperty(nameof(ReferrerContext));

        public override object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "PointsPrice":
                    return (object)this.PointsPrice;
                case "CanPurchase":
                    return (object)this.CanPurchase;
                case "CanPurchaseMP3":
                    return (object)this.CanPurchaseMP3;
                case "InCollection":
                    return (object)this.InCollection;
                case "LibraryId":
                    return (object)this.LibraryId;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
