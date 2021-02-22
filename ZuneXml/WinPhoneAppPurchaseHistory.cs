// Decompiled with JetBrains decompiler
// Type: ZuneXml.WinPhoneAppPurchaseHistory
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;

namespace ZuneXml
{
    internal class WinPhoneAppPurchaseHistory : WinPhoneAppHistory
    {
        internal static XmlDataProviderObject ConstructWinPhoneAppPurchaseHistoryObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new WinPhoneAppPurchaseHistory(owner, objectTypeCookie);
        }

        internal WinPhoneAppPurchaseHistory(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal override string Title => (string)base.GetProperty(nameof(Title));

        internal override Guid Id => (Guid)base.GetProperty(nameof(Id));

        internal override DateTime Date => (DateTime)base.GetProperty(nameof(Date));

        internal override IList MediaInstances => (IList)base.GetProperty(nameof(MediaInstances));

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
