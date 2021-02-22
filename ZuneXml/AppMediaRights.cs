// Decompiled with JetBrains decompiler
// Type: ZuneXml.AppMediaRights
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneXml
{
    internal class AppMediaRights : MediaRights
    {
        internal static XmlDataProviderObject ConstructAppMediaRightsObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new AppMediaRights(owner, objectTypeCookie);
        }

        internal AppMediaRights(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        public override object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "Languages":
                    return (object)this.Languages;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
