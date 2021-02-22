// Decompiled with JetBrains decompiler
// Type: ZuneXml.AppScreenshot
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneXml
{
    internal class AppScreenshot : Thumbnail
    {
        internal static XmlDataProviderObject ConstructAppScreenshotObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new AppScreenshot(owner, objectTypeCookie);
        }

        internal AppScreenshot(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal override Guid ImageId => (Guid)this.GetProperty(nameof(ImageId));

        internal override string Id => (string)this.GetProperty(nameof(Id));

        internal override string Title => (string)this.GetProperty(nameof(Title));

        internal override string SortTitle => (string)this.GetProperty(nameof(SortTitle));
    }
}
