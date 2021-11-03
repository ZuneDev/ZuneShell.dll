// Decompiled with JetBrains decompiler
// Type: ZuneXml.AppData
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;

namespace ZuneXml
{
    internal class AppData : App
    {
        internal static XmlDataProviderObject ConstructAppDataObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return new AppData(owner, objectTypeCookie);
        }

        internal AppData(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal string Copyright => (string)base.GetProperty(nameof(Copyright));

        internal string Description => (string)base.GetProperty(nameof(Description));

        internal IList Screenshots => (IList)base.GetProperty(nameof(Screenshots));

        internal IList Genres => (IList)base.GetProperty(nameof(Genres));

        internal AppCapabilities Capabilities => (AppCapabilities)base.GetProperty(nameof(Capabilities));

        internal Guid BackgroundImageId => (Guid)base.GetProperty(nameof(BackgroundImageId));

        internal Guid RatingImageId => (Guid)base.GetProperty(nameof(RatingImageId));

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
