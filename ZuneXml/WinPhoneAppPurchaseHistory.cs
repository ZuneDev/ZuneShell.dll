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
            return new WinPhoneAppPurchaseHistory(owner, objectTypeCookie);
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
