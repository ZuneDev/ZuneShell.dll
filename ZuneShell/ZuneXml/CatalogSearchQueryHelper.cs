// Decompiled with JetBrains decompiler
// Type: ZuneXml.CatalogSearchQueryHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Configuration;
using Microsoft.Zune.Shell;
using System;
using System.Text;

namespace ZuneXml
{
    internal class CatalogSearchQueryHelper : CatalogServiceQueryHelper
    {
        internal static ZuneServiceQueryHelper ConstructSearchQueryHelper(
          ZuneServiceQuery query)
        {
            return new CatalogSearchQueryHelper(query);
        }

        internal CatalogSearchQueryHelper(ZuneServiceQuery query)
          : base(query)
        {
        }

        internal override string GetResourceUri()
        {
            string property1 = (string)this.Query.GetProperty("ResourceType");
            string property2 = (string)this.Query.GetProperty("Keywords");
            if (string.IsNullOrEmpty(property1) || string.IsNullOrEmpty(property2))
                return null;
            string endPointUri = Microsoft.Zune.Service.Service2.GetEndPointUri(this._endPoint);
            StringBuilder stringBuilder = new StringBuilder(128);
            stringBuilder.Append(endPointUri);
            stringBuilder.Append("/");
            stringBuilder.Append(property1);
            stringBuilder.Append("?q=");
            stringBuilder.Append(Uri.EscapeDataString(property2));
            string property3 = (string)this.Query.GetProperty("ClientType");
            if (!string.IsNullOrEmpty(property3))
            {
                stringBuilder.Append("&clientType=");
                stringBuilder.Append(property3);
            }
            string property4 = (string)this.Query.GetProperty("Store");
            if (!string.IsNullOrEmpty(property4))
            {
                stringBuilder.Append("&store=");
                stringBuilder.Append(property4);
            }
            string timeTravel = ClientConfiguration.Service.TimeTravel;
            if (!string.IsNullOrEmpty(timeTravel) && ZuneApplication.Service2.IsSignedIn())
            {
                stringBuilder.Append("&instant=");
                stringBuilder.Append(Uri.EscapeDataString(timeTravel));
            }
            return stringBuilder.ToString();
        }
    }
}
