// Decompiled with JetBrains decompiler
// Type: ZuneXml.Network
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneXml
{
    internal class Network : Category
    {
        internal static XmlDataProviderObject ConstructNetworkObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new Network(owner, objectTypeCookie);
        }

        internal Network(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal override string Id => (string)this.GetProperty(nameof(Id));

        internal override string Title => (string)this.GetProperty(nameof(Title));
    }
}
