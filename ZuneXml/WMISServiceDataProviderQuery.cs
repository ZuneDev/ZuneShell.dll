// Decompiled with JetBrains decompiler
// Type: ZuneXml.WMISServiceDataProviderQuery
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using System;
using System.Text;
using System.Threading;
using ZuneUI;

namespace ZuneXml
{
    internal class WMISServiceDataProviderQuery : XmlDataProviderQuery
    {
        private string m_strPostBody;
        private bool _endpointsInitialized;

        internal static DataProviderQuery ConstructWmisQuery(object queryTypeCookie) => (DataProviderQuery)new WMISServiceDataProviderQuery(queryTypeCookie);

        public WMISServiceDataProviderQuery(object queryTypeCookie)
          : base(queryTypeCookie)
          => this._keepAlive = false;

        protected override void BeginExecute()
        {
            if (this._endpointsInitialized)
                base.BeginExecute();
            else
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.BackgroundInitializeWMISEndpointCollection));
        }

        private void BackgroundInitializeWMISEndpointCollection(object unused)
        {
            HRESULT hresult = (HRESULT)ZuneApplication.Service.InitializeWMISEndpointCollection();
            if (hresult.IsSuccess)
            {
                this._endpointsInitialized = true;
                Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredInvokeBeginExecute), (object)null);
            }
            else
            {
                ++this._requestGeneration;
                this.SetWorkerStatus(this._requestGeneration, DataProviderQueryStatus.RequestingData);
                this.SetWorkerStatus(this._requestGeneration, DataProviderQueryStatus.Error, (object)hresult);
            }
        }

        private void DeferredInvokeBeginExecute(object unused) => base.BeginExecute();

        protected override string GetResourceUri() => this.BuildServiceUri(0, 0);

        protected override string GetPostBody() => this.m_strPostBody;

        private string BuildServiceUri(int chunkStart, int chunkSize)
        {
            this.m_strPostBody = (string)null;
            string property1 = (string)this.GetProperty("SearchString");
            string property2 = (string)this.GetProperty("artistId");
            string property3 = (string)this.GetProperty("albumId");
            string strEndPointName;
            string paramName;
            string paramValue1;
            if (!string.IsNullOrEmpty(property1))
            {
                strEndPointName = WMISEndpointIds.WMISEID_Search;
                paramName = "SearchString";
                paramValue1 = property1;
            }
            else if (!string.IsNullOrEmpty(property2))
            {
                strEndPointName = WMISEndpointIds.WMISEID_GetResultsForArtist;
                paramName = "artistId";
                paramValue1 = property2;
            }
            else
            {
                if (string.IsNullOrEmpty(property3))
                    return (string)null;
                strEndPointName = WMISEndpointIds.WMISEID_GetAlbumDetailsByAlbumId;
                paramName = "albumId";
                paramValue1 = property3;
                Guid.NewGuid();
                this.m_strPostBody = string.Empty;
            }
            string wmisEndPointUri = ZuneApplication.Service.GetWMISEndPointUri(strEndPointName);
            if (string.IsNullOrEmpty(wmisEndPointUri))
                return (string)null;
            UriBuilder uriBuilder = new UriBuilder(wmisEndPointUri);
            StringBuilder args = new StringBuilder();
            UrlHelper.AppendParam(true, args, paramName, paramValue1);
            string wmisPartner = ClientConfiguration.Service.WMISPartner;
            if (!string.IsNullOrEmpty(wmisPartner))
                UrlHelper.AppendParam(false, args, "Partner", wmisPartner);
            string[] strArray = new string[5]
            {
        "locale",
        "maxNumberOfResults",
        "resultTypeString",
        "countOnly",
        "volume"
            };
            foreach (string str in strArray)
            {
                string paramValue2 = (string)null;
                object property4 = this.GetProperty(str);
                if (property4 != null)
                    paramValue2 = property4.ToString();
                if (!string.IsNullOrEmpty(paramValue2))
                    UrlHelper.AppendParam(false, args, str, paramValue2);
            }
            string str1 = args.ToString();
            if (str1.Length > 0 && str1[0] == '?')
                str1 = str1.Remove(0, 1);
            uriBuilder.Query = str1;
            return uriBuilder.Uri.AbsoluteUri;
        }
    }
}
