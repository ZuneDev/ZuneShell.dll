// Decompiled with JetBrains decompiler
// Type: ZuneXml.XmlDataProviderQuery
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.PerfTrace;
using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;

namespace ZuneXml
{
    public class XmlDataProviderQuery : DataProviderQuery
    {
        protected EPassportPolicyId _passportTicketType;
        protected HttpRequestCachePolicy _cachePolicy;
        private string _lastUri;
        internal int _requestGeneration;
        protected bool _keepAlive;
        protected bool _ignoreNamespacePrefix;
        protected bool _ignoreDuplicateGenerationRequests;
        protected bool _acceptGZipEncoding;
        private object _errorCode;
        private bool _refreshOnCacheExpire;
        private Microsoft.Iris.Timer _autoRefreshTimer;

        protected XmlDataProviderQuery(object queryTypeCookie)
          : base(queryTypeCookie)
        {
            this._passportTicketType = EPassportPolicyId.None;
            this._cachePolicy = HttpRequestCachePolicy.Default;
            this._keepAlive = true;
            this._ignoreNamespacePrefix = true;
            this._ignoreDuplicateGenerationRequests = true;
            this.Result = (object)XmlDataProviderObjectFactory.CreateObject((DataProviderQuery)this, this.ResultTypeCookie);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            if (this._autoRefreshTimer == null)
                return;
            this._autoRefreshTimer.Stop();
            this._autoRefreshTimer.Dispose();
            this._autoRefreshTimer = (Microsoft.Iris.Timer)null;
        }

        public object ErrorCode => this._errorCode;

        public HttpRequestCachePolicy CachePolicy
        {
            get => this._cachePolicy;
            set => this._cachePolicy = value;
        }

        public EPassportPolicyId PassportTicketType
        {
            get => this._passportTicketType;
            set => this._passportTicketType = value;
        }

        public bool IgnoreDuplicateGenerationRequests
        {
            get => this._ignoreDuplicateGenerationRequests;
            set => this._ignoreDuplicateGenerationRequests = value;
        }

        public bool RefreshOnCacheExpire
        {
            get => this._refreshOnCacheExpire;
            set => this._refreshOnCacheExpire = value;
        }

        protected override void BeginExecute()
        {
            if (this.GetProperty("SendPassportTicket") is bool passportProperty)
                this._passportTicketType = passportProperty ? EPassportPolicyId.MBI : EPassportPolicyId.None;
            if (this.GetProperty("SendSecurePassportTicket") is bool securePassportProperty)
                this._passportTicketType = securePassportProperty ? EPassportPolicyId.MBI_SSL : EPassportPolicyId.None;
            if (this.GetProperty("RefreshOnCacheExpire") is bool expireProperty)
                this._refreshOnCacheExpire = expireProperty;
            string resourceUri = this.GetResourceUri();
            string postBody = this.GetPostBody();
            if (string.IsNullOrEmpty(resourceUri))
                return;
            this.GetDataFromResource(resourceUri, postBody, true);
        }

        protected virtual string GetResourceUri() => (string)null;

        protected virtual string GetPostBody() => (string)null;

        internal virtual bool FilterDataProviderObject(XmlDataProviderObject dataObject) => false;

        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == "LocalSortBy" && this.Result != null)
                ((XmlDataProviderObject)this.Result).ChangeListSort((string)this.GetProperty(propertyName));
            base.OnPropertyChanged(propertyName);
        }

        internal void GetDataFromResource(string uri, string body, bool fNewGeneration)
        {
            XmlDataProviderQuery.GetDataFromResourceArgs fromResourceArgs = new XmlDataProviderQuery.GetDataFromResourceArgs(uri, body, fNewGeneration);
            if (Application.IsApplicationThread)
                this.GetDataFromResource((object)fromResourceArgs);
            else
                Application.DeferredInvoke(new DeferredInvokeHandler(this.GetDataFromResource), (object)fromResourceArgs);
        }

        private void GetDataFromResource(object obj)
        {
            XmlDataProviderQuery.GetDataFromResourceArgs fromResourceArgs = (XmlDataProviderQuery.GetDataFromResourceArgs)obj;
            string requestUri = fromResourceArgs.Uri;
            string requestBody = fromResourceArgs.Body;
            bool newGeneration = fromResourceArgs.NewGeneration;
            string property1 = (string)this.GetProperty("LocalSortBy");
            bool fPaged = this.GetProperty("Paged") is bool property2 && property2;
            XmlDataProviderObject result = (XmlDataProviderObject)null;
            if (newGeneration)
            {
                ++this._requestGeneration;
                result = XmlDataProviderObjectFactory.CreateObject((DataProviderQuery)this, this.ResultTypeCookie);
                if (property1 != null)
                    result.ChangeListSort(property1);
            }
            else
            {
                result = (XmlDataProviderObject)this.Result;
                if (this._ignoreDuplicateGenerationRequests && string.Equals(requestUri, this._lastUri, StringComparison.CurrentCultureIgnoreCase))
                    return;
            }
            this._lastUri = requestUri;
            this.SetWorkerStatus(this._requestGeneration, DataProviderQueryStatus.RequestingData);
            bool flag = false;
            string localUri = (string)null;
            if (!flag)
                ThreadPool.QueueUserWorkItem((WaitCallback)(arg =>
               {
                   int requestGeneration = (int)arg;
                   Microsoft.Zune.Service.HttpWebRequest httpWebRequest = this.ConstructWebRequest(requestUri, requestBody);
                   XmlDataProviderQuery.RequestArgs requestArgs = new XmlDataProviderQuery.RequestArgs(requestGeneration, requestUri, requestBody, localUri, result, fPaged, DateTime.Now, Environment.TickCount, httpWebRequest.CachePolicy);
                   httpWebRequest.GetResponseAsync(new AsyncRequestComplete(this.OnRequestComplete), (object)requestArgs);
               }), (object)this._requestGeneration);
            Microsoft.Zune.PerfTrace.PerfTrace.TraceUICollectionEvent(UICollectionEvent.DataProviderQueryBegin, this._lastUri);
        }

        private void OnRequestComplete(Microsoft.Zune.Service.HttpWebResponse response, object requestArgs)
        {
            XmlDataProviderQuery.RequestArgs requestArgs1 = (XmlDataProviderQuery.RequestArgs)requestArgs;
            Stream xmlStream = (Stream)null;
            try
            {
                xmlStream = response.StatusCode == HttpStatusCode.OK ? response.GetResponseStream() : throw new HttpWebException(response);
                this.ParseXml(xmlStream, requestArgs1.m_requestGeneration, requestArgs1.m_requestUri, requestArgs1.m_requestBody, requestArgs1.m_result, requestArgs1.m_fPaged, requestArgs1.m_tmStartTime, requestArgs1.m_tcStart);
                if (requestArgs1.m_cachePolicy == HttpRequestCachePolicy.Refresh && this._cachePolicy == HttpRequestCachePolicy.Default && requestArgs1.m_result.NextPage != null)
                    UriResourceTracker.Instance.SetResourceModified(requestArgs1.m_result.NextPage.GetPageUrl(0), true);
                this.SetAutoRefreshTimer(response.Expires);
            }
            catch (Exception ex)
            {
                if (ex is HttpWebException)
                {
                    object statusCode = (object)((HttpWebException)ex).Response.StatusCode;
                    this.SetWorkerStatus(requestArgs1.m_requestGeneration, DataProviderQueryStatus.Error, statusCode);
                }
                else
                    this.SetWorkerStatus(requestArgs1.m_requestGeneration, DataProviderQueryStatus.Error, (object)null);
                int num = TraceSwitches.DataProviderSwitch.TraceWarning ? 1 : 0;
            }
            finally
            {
                xmlStream?.Close();
                response.Close();
                Microsoft.Zune.PerfTrace.PerfTrace.TraceUICollectionEvent(UICollectionEvent.DataProviderQueryComplete, this._lastUri);
            }
        }

        private void ParseXml(
          Stream xmlStream,
          int requestGeneration,
          string requestUri,
          string requestBody,
          XmlDataProviderObject result,
          bool fPaged,
          DateTime tmStartTime,
          int tcStart)
        {
            int num1 = 0;
            int num2 = 0;
            bool flag1 = false;
            XmlDataProviderReader xmlReader = (XmlDataProviderReader)null;
            try
            {
                num1 = Environment.TickCount - tcStart;
                this.SetWorkerStatus(requestGeneration, DataProviderQueryStatus.ProcessingData);
                StringBuilder stringBuilder = new StringBuilder(80);
                int[] numArray = new int[20];
                xmlReader = new XmlDataProviderReader(xmlStream);
                List<XmlDataProviderQuery.XPathMatch> xpathMatchList = new List<XmlDataProviderQuery.XPathMatch>(2);
                bool flag2 = false;
                int num3 = TraceSwitches.DataProviderSwitch.TraceVerbose ? 1 : 0;
                XmlNodeType xmlNodeType = XmlNodeType.None;
                while (xmlNodeType != XmlNodeType.None || xmlReader.Read())
                {
                    xmlNodeType = XmlNodeType.None;
                    if (requestGeneration != this._requestGeneration)
                    {
                        flag2 = true;
                        break;
                    }
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Depth < numArray.Length)
                    {
                        int depth = xmlReader.Depth;
                        stringBuilder.Length = depth > 0 ? numArray[depth - 1] : 0;
                        stringBuilder.Append('/');
                        if (this._ignoreNamespacePrefix)
                            stringBuilder.Append(xmlReader.LocalName);
                        else
                            stringBuilder.Append(xmlReader.Name);
                        numArray[depth] = stringBuilder.Length;
                        string str = stringBuilder.ToString();
                        if (fPaged)
                        {
                            IPageInfo nextPageInfo = this.ExtractNextPageInfo(requestUri, requestBody, str, xmlReader);
                            if (nextPageInfo != null)
                                result.NextPage = nextPageInfo;
                        }
                        Hashtable attributes = new Hashtable(3);
                        if (xmlReader.MoveToFirstAttribute())
                        {
                            do
                            {
                                attributes[(object)xmlReader.Name] = (object)xmlReader.Value;
                            }
                            while (xmlReader.MoveToNextAttribute());
                        }
                        result.ProcessXPath(str, attributes, xpathMatchList);
                        if (xpathMatchList.Count > 0)
                        {
                            flag1 = true;
                            string xmlValue = (string)null;
                            if (xmlReader.Read() && (xmlNodeType = xmlReader.NodeType) == XmlNodeType.Text)
                            {
                                xmlNodeType = XmlNodeType.None;
                                xmlValue = xmlReader.Value;
                            }
                            XmlDataProviderQuery.ProcessXPathMatches(xpathMatchList, attributes, xmlValue, xmlReader);
                        }
                        foreach (string key in (IEnumerable)attributes.Keys)
                        {
                            result.ProcessXPath(str + "@" + key, attributes, xpathMatchList);
                            if (xpathMatchList.Count > 0)
                            {
                                flag1 = true;
                                XmlDataProviderQuery.ProcessXPathMatches(xpathMatchList, attributes, (string)null, (XmlDataProviderReader)null);
                            }
                        }
                    }
                }
                if (!flag2)
                {
                    Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredSetResult), (object)new XmlDataProviderQuery.DeferredSetResultArgs(requestGeneration, result));
                    this.SetWorkerStatus(requestGeneration, DataProviderQueryStatus.Complete);
                    if (TraceSwitches.DataProviderSwitch.TraceWarning)
                    {
                        num2 = Environment.TickCount - tcStart;
                        int num4 = flag1 ? 1 : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredSetResult), (object)new XmlDataProviderQuery.DeferredSetResultArgs(requestGeneration, result));
                this.SetWorkerStatus(requestGeneration, DataProviderQueryStatus.Error);
                throw ex;
            }
            finally
            {
                xmlReader?.Close();
            }
            int num5 = TraceSwitches.DataProviderSwitch.TraceVerbose ? 1 : 0;
        }

        internal virtual IPageInfo ExtractNextPageInfo(
          string requestUri,
          string requestBody,
          string strElementPath,
          XmlDataProviderReader xmlReader)
        {
            if (strElementPath == "/feed/link")
            {
                bool flag = false;
                if (xmlReader.MoveToFirstAttribute())
                {
                    do
                    {
                        if (xmlReader.Name == "rel")
                            flag = xmlReader.Value == "next";
                        else if (xmlReader.Name == "href" && flag && !string.IsNullOrEmpty(xmlReader.Value))
                            return (IPageInfo)new LinkPageInfo(XmlDataProviderQuery.ConstructLinkUrl(requestUri, xmlReader.Value), requestBody);
                    }
                    while (xmlReader.MoveToNextAttribute());
                }
            }
            return (IPageInfo)null;
        }

        private Microsoft.Zune.Service.HttpWebRequest ConstructWebRequest(
          string requestUri,
          string requestBody)
        {
            return requestBody != null ? WebRequestHelper.ConstructWebPostRequest(requestUri, requestBody, this._passportTicketType, this._cachePolicy, this._keepAlive, this._acceptGZipEncoding) : WebRequestHelper.ConstructWebRequest(requestUri, this._passportTicketType, this._cachePolicy, this._keepAlive, this._acceptGZipEncoding);
        }

        private static string ConstructLinkUrl(string baseUrl, string relativeUrl)
        {
            string str = (string)null;
            if (relativeUrl.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
            {
                str = relativeUrl;
            }
            else
            {
                int num = baseUrl.IndexOf("//");
                if (num > 0)
                {
                    int length = baseUrl.IndexOf('/', num + 2);
                    if (length > 0)
                        str = baseUrl.Substring(0, length) + relativeUrl;
                }
            }
            return str;
        }

        private static void ProcessXPathMatches(
          List<XmlDataProviderQuery.XPathMatch> xpathMatches,
          Hashtable attributes,
          string xmlValue,
          XmlDataProviderReader xmlReader)
        {
            foreach (XmlDataProviderQuery.XPathMatch xpathMatch in xpathMatches)
            {
                string str = xmlValue;
                if (xpathMatch.matchingAttributeName != null)
                    str = (string)attributes[(object)xpathMatch.matchingAttributeName];
                if (xpathMatch.encodedXml && str != null && xmlReader != null)
                    xmlReader.PushElement(str);
                else if (str != null)
                    xpathMatch.instance.SetPropertyFromStringValue(xpathMatch.propertyMapping, str);
            }
            xpathMatches.Clear();
        }

        protected void SetWorkerStatus(int requestGeneration, DataProviderQueryStatus eStatus) => this.SetWorkerStatus(requestGeneration, eStatus, (object)null);

        protected void SetWorkerStatus(
          int requestGeneration,
          DataProviderQueryStatus eStatus,
          object errorCode)
        {
            Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredSetStatus), (object)new XmlDataProviderQuery.DeferredSetStatusArgs(requestGeneration, eStatus, errorCode));
        }

        private void DeferredSetStatus(object args)
        {
            if (!(args is XmlDataProviderQuery.DeferredSetStatusArgs deferredSetStatusArgs) || deferredSetStatusArgs.m_requestGeneration != this._requestGeneration)
                return;
            this.Status = deferredSetStatusArgs.m_eStatus;
            this._errorCode = this.Status != DataProviderQueryStatus.Error ? (object)null : deferredSetStatusArgs.m_errorCode;
            if (this.Status == DataProviderQueryStatus.Error)
                this._lastUri = (string)null;
            if (this.Result == null || this.Status != DataProviderQueryStatus.Complete)
                return;
            ((XmlDataProviderObject)this.Result).OnQueryComplete();
        }

        private void SetAutoRefreshTimer(DateTime expires)
        {
            if (!this._refreshOnCacheExpire)
                return;
            Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredSetAutoRefreshTimer), (object)expires);
        }

        private void DeferredSetAutoRefreshTimer(object args)
        {
            DateTime utcNow = DateTime.UtcNow;
            DateTime dateTime = (DateTime)args;
            if (dateTime == DateTime.MaxValue || dateTime <= utcNow)
            {
                int refreshQueryFallback = ClientConfiguration.Service.AutoRefreshQueryFallback;
                dateTime = utcNow.AddMilliseconds((double)refreshQueryFallback);
            }
            if (!(dateTime != DateTime.MaxValue))
                return;
            if (this._autoRefreshTimer == null)
            {
                this._autoRefreshTimer = new Microsoft.Iris.Timer();
                this._autoRefreshTimer.Tick += (EventHandler)((sender, eventArgs) =>
               {
                   this._autoRefreshTimer.Stop();
                   this.Refresh();
               });
            }
            int num1 = new Random().Next(1, 300000);
            if (dateTime > utcNow)
            {
                long num2 = dateTime.Subtract(utcNow).Ticks / 10000L;
                if (num2 > (long)(int.MaxValue - num1))
                    num1 += 900000;
                else
                    num1 += (int)num2;
            }
            this._autoRefreshTimer.Stop();
            this._autoRefreshTimer.Interval = num1;
            this._autoRefreshTimer.Start();
        }

        private void DeferredSetResult(object argsObj)
        {
            if (!(argsObj is XmlDataProviderQuery.DeferredSetResultArgs deferredSetResultArgs) || deferredSetResultArgs.m_requestGeneration != this._requestGeneration)
                return;
            deferredSetResultArgs.m_result.TransferToAppThread();
            if (this.Result == deferredSetResultArgs.m_result)
                return;
            this.Result = (object)deferredSetResultArgs.m_result;
        }

        private class GetDataFromResourceArgs
        {
            public readonly string Uri;
            public readonly string Body;
            public readonly bool NewGeneration;

            public GetDataFromResourceArgs(string uri, string body, bool fNewGeneration)
            {
                this.Uri = uri;
                this.Body = body;
                this.NewGeneration = fNewGeneration;
            }
        }

        private class RequestArgs
        {
            public readonly int m_requestGeneration;
            public readonly string m_requestUri;
            public readonly string m_requestBody;
            public readonly string m_localUri;
            public readonly XmlDataProviderObject m_result;
            public readonly bool m_fPaged;
            public readonly DateTime m_tmStartTime;
            public readonly int m_tcStart;
            public readonly HttpRequestCachePolicy m_cachePolicy;

            public RequestArgs(
              int requestGeneration,
              string requestUri,
              string requestBody,
              string localUri,
              XmlDataProviderObject result,
              bool fPaged,
              DateTime tmStartTime,
              int tcStart,
              HttpRequestCachePolicy cachePolicy)
            {
                this.m_requestGeneration = requestGeneration;
                this.m_requestUri = requestUri;
                this.m_requestBody = requestBody;
                this.m_localUri = localUri;
                this.m_result = result;
                this.m_fPaged = fPaged;
                this.m_tmStartTime = tmStartTime;
                this.m_tcStart = tcStart;
                this.m_cachePolicy = cachePolicy;
            }
        }

        private class DeferredSetStatusArgs
        {
            public readonly DataProviderQueryStatus m_eStatus;
            public readonly int m_requestGeneration;
            public readonly object m_errorCode;

            public DeferredSetStatusArgs(
              int requestGeneration,
              DataProviderQueryStatus eStatus,
              object errorCode)
            {
                this.m_eStatus = eStatus;
                this.m_requestGeneration = requestGeneration;
                this.m_errorCode = errorCode;
            }
        }

        private class DeferredSetResultArgs
        {
            public readonly XmlDataProviderObject m_result;
            public readonly int m_requestGeneration;

            public DeferredSetResultArgs(int requestGeneration, XmlDataProviderObject result)
            {
                this.m_requestGeneration = requestGeneration;
                this.m_result = result;
            }
        }

        public struct XPathMatch
        {
            public XmlDataProviderObject instance;
            public DataProviderMapping propertyMapping;
            public string matchingAttributeName;
            public bool encodedXml;

            public XPathMatch(
              XmlDataProviderObject instance,
              DataProviderMapping propertyMapping,
              string matchingAttributeName,
              bool encodedXml)
            {
                this.instance = instance;
                this.propertyMapping = propertyMapping;
                this.matchingAttributeName = matchingAttributeName;
                this.encodedXml = encodedXml;
            }
        }
    }
}
