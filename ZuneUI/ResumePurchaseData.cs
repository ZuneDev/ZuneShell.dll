// Decompiled with JetBrains decompiler
// Type: ZuneUI.ResumePurchaseData
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public class ResumePurchaseData : IEquatable<ResumePurchaseData>
    {
        public ResumePurchaseData(string purchaseHandle, string redirectUrl)
        {
            this.PurchaseHandle = purchaseHandle;
            this.UserRedirectUrl = redirectUrl;
        }

        public bool Equals(ResumePurchaseData other) => string.CompareOrdinal(this.PurchaseHandle, other.PurchaseHandle) == 0 && string.CompareOrdinal(this.UserRedirectUrl, other.UserRedirectUrl) == 0;

        public string PurchaseHandle { get; private set; }

        public string UserRedirectUrl { get; private set; }
    }
}
