// Decompiled with JetBrains decompiler
// Type: ZuneXml.ZuneHDApp
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneXml
{
    internal class ZuneHDApp : App
    {
        internal static XmlDataProviderObject ConstructZuneHDAppObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return new ZuneHDApp(owner, objectTypeCookie);
        }

        internal ZuneHDApp(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

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
