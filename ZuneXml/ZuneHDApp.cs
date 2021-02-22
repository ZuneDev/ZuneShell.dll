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
            return (XmlDataProviderObject)new ZuneHDApp(owner, objectTypeCookie);
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
                    return (object)this.Price;
                case "DisplayPrice":
                    return (object)this.DisplayPrice;
                case "DisplayPriceFull":
                    return (object)this.DisplayPriceFull;
                case "DisplayPriceTrial":
                    return (object)this.DisplayPriceTrial;
                case "CanPurchase":
                    return (object)this.CanPurchase;
                case "CanPurchaseFull":
                    return (object)this.CanPurchaseFull;
                case "CanPurchaseTrial":
                    return (object)this.CanPurchaseTrial;
                case "CanDownload":
                    return (object)this.CanDownload;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
