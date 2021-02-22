// Decompiled with JetBrains decompiler
// Type: ZuneXml.AppGenresQueryHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneXml
{
    internal class AppGenresQueryHelper : CatalogServiceQueryHelper
    {
        internal static AppGenresQueryHelper ConstructAppGenresQueryHelper(
          ZuneServiceQuery query)
        {
            return new AppGenresQueryHelper(query);
        }

        internal AppGenresQueryHelper(ZuneServiceQuery query)
          : base(query)
        {
        }

        internal override bool OnQueryFilterDataProviderObject(XmlDataProviderObject dataObject)
        {
            bool flag = false;
            if (dataObject.GetProperty("Id") is string property && property.Equals("apps.games", StringComparison.OrdinalIgnoreCase))
                flag = true;
            return flag;
        }
    }
}
