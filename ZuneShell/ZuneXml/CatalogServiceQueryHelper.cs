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
            return new CatalogServiceQueryHelper(query);
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
            string url = (string)this.Query.GetProperty("Url");
            if (!string.IsNullOrEmpty(url))
            {
                requestUri.Append(url);
            }
            else
            {
                string id = (string)this.Query.GetProperty("Id");
                if (this.RequireId && id == string.Empty)
                    return null;
                string resourceType = (string)this.Query.GetProperty("ResourceType");
                string representation = (string)this.Query.GetProperty("Representation");
                if (this.RequireResource && resourceType == null || this.RequireRepresentation && representation == null)
                    return null;
                string endPointUri = Service.GetEndPointUri(this._endPoint);
                requestUri.Append(endPointUri);
                requestUri.Append("/");
                if (!string.IsNullOrEmpty(resourceType))
                {
                    requestUri.Append(resourceType);
                    requestUri.Append("/");
                }
                if (!string.IsNullOrEmpty(id))
                {
                    requestUri.Append(id);
                    requestUri.Append("/");
                }
                if (!string.IsNullOrEmpty(representation))
                {
                    requestUri.Append(representation);
                    requestUri.Append("/");
                }
            }
            bool fFirst = true;
            this.AppendStuffAfterRepresentation(requestUri, ref fFirst);
            string clientType = (string)this.Query.GetProperty("ClientType");
            if (!string.IsNullOrEmpty(clientType))
                AppendParam(requestUri, "clientType", clientType, ref fFirst);
            string cost = (string)this.Query.GetProperty("Cost");
            if (!string.IsNullOrEmpty(cost))
                AppendParam(requestUri, "cost", cost, ref fFirst);
            string tag = (string)this.Query.GetProperty("Tag");
            if (!string.IsNullOrEmpty(tag))
                AppendParam(requestUri, "tag", tag, ref fFirst);
            string store = (string)this.Query.GetProperty("Store");
            if (!string.IsNullOrEmpty(store))
                AppendParam(requestUri, "store", store, ref fFirst);
            object chunkSizeOverride = this.Query.GetProperty("ChunkSize");
            if (chunkSizeOverride != null && (int)chunkSizeOverride > 0)
                AppendParam(requestUri, nameof(chunkSize), chunkSizeOverride.ToString(), ref fFirst);
            object isActionable = this.Query.GetProperty("IsActionable");
            if (isActionable != null)
            {
                if ((bool)isActionable)
                    AppendParam(requestUri, "isActionable", isActionable.ToString(), ref fFirst);
            }
            if (chunkSize > 0)
            {
                AppendParam(requestUri, "count", chunkSize.ToString(), ref fFirst);
                AppendParam(requestUri, "startIndex", chunkStart.ToString(), ref fFirst);
            }
            string requestSortBy = (string)this.Query.GetProperty("RequestSortBy");
            if (!string.IsNullOrEmpty(requestSortBy))
                AppendParam(requestUri, "orderby", requestSortBy, ref fFirst);
            string timeTravel = ClientConfiguration.Service.TimeTravel;
            if (!string.IsNullOrEmpty(timeTravel) && ZuneApplication.Service.IsSignedIn())
                AppendParam(requestUri, "instant", Uri.EscapeDataString(timeTravel), ref fFirst);
            return requestUri.ToString();
        }

        protected virtual void AppendStuffAfterRepresentation(StringBuilder requestUri, ref bool fFirst)
        {
            string startsWith = (string)this.Query.GetProperty("StartsWith");
            if (!string.IsNullOrEmpty(startsWith))
                AppendParam(requestUri, "startsWith", startsWith, ref fFirst);
            string startDate = (string)this.Query.GetProperty("StartDate");
            if (!string.IsNullOrEmpty(startDate))
                AppendParam(requestUri, "startDate", startDate, ref fFirst);
            string endDate = (string)this.Query.GetProperty("EndDate");
            if (!string.IsNullOrEmpty(endDate))
                AppendParam(requestUri, "endDate", endDate, ref fFirst);
            object minWidth = this.Query.GetProperty("MinWidth");
            if (minWidth != null && (int)minWidth > 0)
                AppendParam(requestUri, "minWidth", minWidth.ToString(), ref fFirst);
            object minHeight = this.Query.GetProperty("MinHeight");
            if (minHeight == null || (int)minHeight <= 0)
                return;
            AppendParam(requestUri, "minHeight", minHeight.ToString(), ref fFirst);
        }
    }
}
