// Decompiled with JetBrains decompiler
// Type: ZuneXml.SubscriptionHistoryQueryHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneXml
{
    internal class SubscriptionHistoryQueryHelper : HistoryQueryHelper
    {
        internal static SubscriptionHistoryQueryHelper ConstructSubscriptionHistoryQueryHelper(
          ZuneServiceQuery query)
        {
            return new SubscriptionHistoryQueryHelper(query);
        }

        internal SubscriptionHistoryQueryHelper(ZuneServiceQuery query)
          : base(query)
          => this._api = "/account/subscriptionhistory";
    }
}
