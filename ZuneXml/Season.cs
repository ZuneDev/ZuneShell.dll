// Decompiled with JetBrains decompiler
// Type: ZuneXml.Season
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneXml
{
    internal class Season : XmlDataProviderObject
    {
        internal bool IsPriceDiscounted
        {
            get
            {
                Right right = this.Rights.GetOfferRight(MediaRightsEnum.SeasonPurchase, ClientTypeEnum.None, PriceTypeEnum.Points) ?? this.Rights.GetOfferRight(MediaRightsEnum.SeasonPurchaseStream, ClientTypeEnum.None, PriceTypeEnum.Points);
                return right != null && right.Price < right.OriginalPrice;
            }
        }

        internal static XmlDataProviderObject ConstructSeasonObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return new Season(owner, objectTypeCookie);
        }

        internal Season(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal int Id => (int)base.GetProperty(nameof(Id));

        internal string Title => (string)base.GetProperty(nameof(Title));

        internal Guid ImageId => (Guid)base.GetProperty(nameof(ImageId));

        internal DateTime ReleaseDate => (DateTime)base.GetProperty(nameof(ReleaseDate));

        internal int EpisodeCount => (int)base.GetProperty(nameof(EpisodeCount));

        internal string Description => (string)base.GetProperty(nameof(Description));

        internal string Rating => (string)base.GetProperty(nameof(Rating));

        internal MediaRights Rights => (MediaRights)base.GetProperty(nameof(Rights));

        internal bool IsComplete => (bool)base.GetProperty(nameof(IsComplete));

        public override object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "IsPriceDiscounted":
                    return IsPriceDiscounted;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
