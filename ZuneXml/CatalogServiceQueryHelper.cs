// Decompiled with JetBrains decompiler
// Type: ZuneXml.CatalogServiceQueryHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Configuration;
using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using System;
using System.Text;

namespace ZuneXml
{
    internal class CatalogServiceQueryHelper : ZuneServiceQueryHelper
    {
        protected EServiceEndpointId _endPoint;

        internal static ZuneServiceQueryHelper ConstructMusicCatalogQueryHelper(
          ZuneServiceQuery query)
        {
            return (ZuneServiceQueryHelper)new CatalogServiceQueryHelper(query);
        }

        internal CatalogServiceQueryHelper(ZuneServiceQuery query)
          : base(query)
        {
            this._endPoint = EServiceEndpointId.SEID_RootCatalog;
            if (string.IsNullOrEmpty(ClientConfiguration.Service.TimeTravel) || !ZuneApplication.Service.IsSignedIn())
                return;
            query.PassportTicketType = EPassportPolicyId.MBI;
        }

        internal override string GetResourceUri() => this.BuildServiceUri(0, 0);

        protected virtual bool ResourceOnly
        {
            get
            {
                object property = this.Query.GetProperty(nameof(ResourceOnly));
                return property == null || !(bool)property;
            }
        }

        protected virtual bool RequireId => this.ResourceOnly;

        protected virtual bool RequireResource => true;

        protected virtual bool RequireRepresentation => this.ResourceOnly;

        private string BuildServiceUri(int chunkStart, int chunkSize)
        {
            StringBuilder requestUri = new StringBuilder(128);
            string property1 = (string)this.Query.GetProperty("Url");
            if (!string.IsNullOrEmpty(property1))
            {
                requestUri.Append(property1);
            }
            else
            {
                string property2 = (string)this.Query.GetProperty("Id");
                if (this.RequireId && property2 == string.Empty)
                    return (string)null;
                string property3 = (string)this.Query.GetProperty("ResourceType");
                string property4 = (string)this.Query.GetProperty("Representation");
                if (this.RequireResource && property3 == null || this.RequireRepresentation && property4 == null)
                    return (string)null;
                string endPointUri = Microsoft.Zune.Service.Service.GetEndPointUri(this._endPoint);
                requestUri.Append(endPointUri);
                requestUri.Append("/");
                if (!string.IsNullOrEmpty(property3))
                {
                    requestUri.Append(property3);
                    requestUri.Append("/");
                }
                if (!string.IsNullOrEmpty(property2))
                {
                    requestUri.Append(property2);
                    requestUri.Append("/");
                }
                if (!string.IsNullOrEmpty(property4))
                {
                    requestUri.Append(property4);
                    requestUri.Append("/");
                }
            }
            bool fFirst = true;
            this.AppendStuffAfterRepresentation(requestUri, ref fFirst);
            string property5 = (string)this.Query.GetProperty("ClientType");
            if (!string.IsNullOrEmpty(property5))
                ZuneServiceQueryHelper.AppendParam(requestUri, "clientType", property5, ref fFirst);
            string property6 = (string)this.Query.GetProperty("Cost");
            if (!string.IsNullOrEmpty(property6))
                ZuneServiceQueryHelper.AppendParam(requestUri, "cost", property6, ref fFirst);
            string property7 = (string)this.Query.GetProperty("Tag");
            if (!string.IsNullOrEmpty(property7))
                ZuneServiceQueryHelper.AppendParam(requestUri, "tag", property7, ref fFirst);
            string property8 = (string)this.Query.GetProperty("Store");
            if (!string.IsNullOrEmpty(property8))
                ZuneServiceQueryHelper.AppendParam(requestUri, "store", property8, ref fFirst);
            object property9 = this.Query.GetProperty("ChunkSize");
            if (property9 != null && (int)property9 > 0)
                ZuneServiceQueryHelper.AppendParam(requestUri, nameof(chunkSize), property9.ToString(), ref fFirst);
            object property10 = this.Query.GetProperty("IsActionable");
            if (property10 != null)
            {
                bool flag = (bool)property10;
                if (flag)
                    ZuneServiceQueryHelper.AppendParam(requestUri, "isActionable", flag.ToString(), ref fFirst);
            }
            if (chunkSize > 0)
            {
                ZuneServiceQueryHelper.AppendParam(requestUri, "count", chunkSize.ToString(), ref fFirst);
                ZuneServiceQueryHelper.AppendParam(requestUri, "startIndex", chunkStart.ToString(), ref fFirst);
            }
            string property11 = (string)this.Query.GetProperty("RequestSortBy");
            if (!string.IsNullOrEmpty(property11))
                ZuneServiceQueryHelper.AppendParam(requestUri, "orderby", property11, ref fFirst);
            string timeTravel = ClientConfiguration.Service.TimeTravel;
            if (!string.IsNullOrEmpty(timeTravel) && ZuneApplication.Service.IsSignedIn())
                ZuneServiceQueryHelper.AppendParam(requestUri, "instant", Uri.EscapeDataString(timeTravel), ref fFirst);
            return requestUri.ToString();
        }

        protected virtual void AppendStuffAfterRepresentation(StringBuilder requestUri, ref bool fFirst)
        {
            string property1 = (string)this.Query.GetProperty("StartsWith");
            if (!string.IsNullOrEmpty(property1))
                ZuneServiceQueryHelper.AppendParam(requestUri, "startsWith", property1, ref fFirst);
            string property2 = (string)this.Query.GetProperty("StartDate");
            if (!string.IsNullOrEmpty(property2))
                ZuneServiceQueryHelper.AppendParam(requestUri, "startDate", property2, ref fFirst);
            string property3 = (string)this.Query.GetProperty("EndDate");
            if (!string.IsNullOrEmpty(property3))
                ZuneServiceQueryHelper.AppendParam(requestUri, "endDate", property3, ref fFirst);
            object property4 = this.Query.GetProperty("MinWidth");
            if (property4 != null && (int)property4 > 0)
                ZuneServiceQueryHelper.AppendParam(requestUri, "minWidth", property4.ToString(), ref fFirst);
            object property5 = this.Query.GetProperty("MinHeight");
            if (property5 == null || (int)property5 <= 0)
                return;
            ZuneServiceQueryHelper.AppendParam(requestUri, "minHeight", property5.ToString(), ref fFirst);
        }
    }
}
