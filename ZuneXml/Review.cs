// Decompiled with JetBrains decompiler
// Type: ZuneXml.Review
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneXml
{
    internal class Review : XmlDataProviderObject
    {
        internal static XmlDataProviderObject ConstructReviewObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return new Review(owner, objectTypeCookie);
        }

        internal Review(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal DateTime Date => (DateTime)this.GetProperty(nameof(Date));

        internal string Title => (string)this.GetProperty(nameof(Title));

        internal string Comment => (string)this.GetProperty(nameof(Comment));

        internal string UserName => (string)this.GetProperty(nameof(UserName));

        internal float Rating => (float)this.GetProperty(nameof(Rating));
    }
}
