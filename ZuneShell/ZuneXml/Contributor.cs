// Decompiled with JetBrains decompiler
// Type: ZuneXml.Contributor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneXml
{
    internal class Contributor : XmlDataProviderObject
    {
        internal static XmlDataProviderObject ConstructContributorObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return new Contributor(owner, objectTypeCookie);
        }

        internal Contributor(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal Guid ContributorId => (Guid)this.GetProperty(nameof(ContributorId));

        internal string ContributorName => (string)this.GetProperty(nameof(ContributorName));

        internal int RoleId => (int)this.GetProperty(nameof(RoleId));

        internal string RoleName => (string)this.GetProperty(nameof(RoleName));
    }
}
