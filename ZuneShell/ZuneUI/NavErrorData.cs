// Decompiled with JetBrains decompiler
// Type: ZuneUI.NavErrorData
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public class NavErrorData : IEquatable<NavErrorData>
    {
        public string FailureUrl { get; private set; }

        public int ErrorCode { get; private set; }

        public NavErrorData(string failureUrl, int errorCode)
        {
            this.FailureUrl = failureUrl;
            this.ErrorCode = errorCode;
        }

        public bool Equals(NavErrorData other) => this.ErrorCode == other.ErrorCode && string.Compare(this.FailureUrl, other.FailureUrl, false) == 0;

        private NavErrorData()
        {
        }
    }
}
