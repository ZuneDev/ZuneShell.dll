// Decompiled with JetBrains decompiler
// Type: ZuneXml.App
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneXml
{
    internal class App : MiniMedia
    {
        internal double Price
        {
            get
            {
                Right right = this.SelectBestOfferRight();
                return right == null ? 0.0 : right.CurrencyPrice;
            }
        }

        internal string DisplayPrice => this.SelectBestOfferRight()?.DisplayPrice;

        internal string DisplayPriceFull => this.Rights.GetOfferRight(MediaRightsEnum.Purchase, ClientTypeEnum.WindowsPhone, PriceTypeEnum.Currency)?.DisplayPrice;

        internal string DisplayPriceTrial => this.Rights.GetOfferRight(MediaRightsEnum.PurchaseTrial, ClientTypeEnum.WindowsPhone, PriceTypeEnum.Currency)?.DisplayPrice;

        internal bool CanDownload => this.Rights.HasRights(MediaRightsEnum.Purchase, ClientTypeEnum.Zune) || this.Rights.HasRights(MediaRightsEnum.Download, ClientTypeEnum.Zune);

        internal bool CanPurchase => this.CanPurchaseFull || this.CanPurchaseTrial;

        internal bool CanPurchaseFull => this.Rights.HasOfferRights(MediaRightsEnum.Purchase, ClientTypeEnum.WindowsPhone, PriceTypeEnum.Currency);

        internal bool CanPurchaseTrial => this.Rights.HasOfferRights(MediaRightsEnum.PurchaseTrial, ClientTypeEnum.WindowsPhone, PriceTypeEnum.Currency);

        private Right SelectBestOfferRight() => this.Rights.GetOfferRight(MediaRightsEnum.Purchase, ClientTypeEnum.WindowsPhone, PriceTypeEnum.Currency) ?? this.Rights.GetOfferRight(MediaRightsEnum.PurchaseTrial, ClientTypeEnum.WindowsPhone, PriceTypeEnum.Currency);

        internal static XmlDataProviderObject ConstructAppObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return new App(owner, objectTypeCookie);
        }

        internal App(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal string SortTitle => (string)base.GetProperty(nameof(SortTitle));

        internal Guid ImageId => (Guid)base.GetProperty(nameof(ImageId));

        internal DateTime ReleaseDate => (DateTime)base.GetProperty(nameof(ReleaseDate));

        internal string GenreName => (string)base.GetProperty(nameof(GenreName));

        internal string Version => (string)base.GetProperty(nameof(Version));

        internal string Author => (string)base.GetProperty(nameof(Author));

        internal string Publisher => (string)base.GetProperty(nameof(Publisher));

        internal string ShortDescription => (string)base.GetProperty(nameof(ShortDescription));

        internal float AverageRating => (float)base.GetProperty(nameof(AverageRating));

        internal AppMediaRights Rights => (AppMediaRights)base.GetProperty(nameof(Rights));

        internal override Guid Id => (Guid)base.GetProperty(nameof(Id));

        internal override string Title => (string)base.GetProperty(nameof(Title));

        public override object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "Price":
                    return Price;
                case "DisplayPrice":
                    return DisplayPrice;
                case "DisplayPriceFull":
                    return DisplayPriceFull;
                case "DisplayPriceTrial":
                    return DisplayPriceTrial;
                case "CanPurchase":
                    return CanPurchase;
                case "CanPurchaseFull":
                    return CanPurchaseFull;
                case "CanPurchaseTrial":
                    return CanPurchaseTrial;
                case "CanDownload":
                    return CanDownload;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
