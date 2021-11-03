// Decompiled with JetBrains decompiler
// Type: ZuneXml.PurchaseHistoryQueryHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneXml
{
    internal class PurchaseHistoryQueryHelper : HistoryQueryHelper
    {
        internal static PurchaseHistoryQueryHelper ConstructPurchaseHistoryQueryHelper(
          ZuneServiceQuery query)
        {
            return new PurchaseHistoryQueryHelper(query);
        }

        internal PurchaseHistoryQueryHelper(ZuneServiceQuery query)
          : base(query)
          => this._api = "/billing/purchaseHistory";

        internal override bool OnQueryFilterDataProviderObject(XmlDataProviderObject dataObject)
        {
            bool flag1 = false;
            if (dataObject is VideoHistory)
            {
                bool flag2 = false;
                VideoHistory videoHistory = (VideoHistory)dataObject;
                if (videoHistory.MediaInstances != null)
                {
                    foreach (MediaInstance mediaInstance in videoHistory.MediaInstances)
                    {
                        if (mediaInstance.LicenseRight == "Rent" || mediaInstance.LicenseRight == "RentStream")
                        {
                            flag2 = true;
                            break;
                        }
                    }
                }
                bool flag3 = !(this.Query.GetProperty("Rentals") is bool property) || !property;
                flag1 = flag2 == flag3;
            }
            return flag1;
        }
    }
}
