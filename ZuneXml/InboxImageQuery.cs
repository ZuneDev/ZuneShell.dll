// Decompiled with JetBrains decompiler
// Type: ZuneXml.InboxImageQuery
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Messaging;
using Microsoft.Zune.Service;
using System;
using System.IO;
using System.Net;

namespace ZuneXml
{
    internal class InboxImageQuery : DataProviderQuery
    {
        public static readonly string PropertyName_Title = "Title";
        public static readonly string PropertyName_CollectionName = "CollectionName";
        public static readonly string PropertyName_URL = "URL";
        private string _title;
        private string _collectionName;
        private string _url;

        public static DataProviderQuery ConstructQuery(object queryTypeCookie) => (DataProviderQuery)new InboxImageQuery(queryTypeCookie);

        public InboxImageQuery(object queryTypeCookie)
          : base(queryTypeCookie)
          => this.Result = (object)new InboxImageDataProviderObject((DataProviderQuery)this, this.ResultTypeCookie);

        public override void SetProperty(string propertyName, object value)
        {
            if (propertyName == InboxImageQuery.PropertyName_Title)
            {
                this._title = (string)value;
                this.UpdateLibraryState();
            }
            else if (propertyName == InboxImageQuery.PropertyName_CollectionName)
            {
                this._collectionName = (string)value;
                this.UpdateLibraryState();
            }
            else
            {
                if (!(propertyName == InboxImageQuery.PropertyName_URL))
                    throw new ApplicationException("unexpected property name");
                this._url = (string)value;
                this.BeginExecute();
            }
        }

        public override object GetProperty(string propertyName)
        {
            if (propertyName == InboxImageQuery.PropertyName_Title)
                return (object)this._title;
            if (propertyName == InboxImageQuery.PropertyName_CollectionName)
                return (object)this._collectionName;
            return propertyName == InboxImageQuery.PropertyName_URL ? (object)this._url : (object)null;
        }

        protected override void BeginExecute()
        {
            this.DisposeLocalFile();
            if (string.IsNullOrEmpty(this._url) || !(this.Result is InboxImageDataProviderObject result))
                return;
            this.SetWorkerStatus(DataProviderQueryStatus.RequestingData);
            InboxImageQuery.RequestArgs requestArgs = new InboxImageQuery.RequestArgs(this._url, result, Environment.TickCount);
            WebRequestHelper.ConstructWebRequest(this._url, EPassportPolicyId.MBI_SSL, HttpRequestCachePolicy.Default, true, false).GetResponseAsync(new AsyncRequestComplete(this.OnRequestComplete), (object)requestArgs);
        }

        protected override void OnDispose()
        {
            this.DisposeLocalFile();
            base.OnDispose();
        }

        private void UpdateLibraryState()
        {
            this.DisposeLocalFile();
            InboxImageDataProviderObject result = this.Result as InboxImageDataProviderObject;
            string inboxPhotoUrl = MessagingService.Instance.GetInboxPhotoUrl(this._title, this._collectionName);
            result.InLibrary = inboxPhotoUrl != null;
            result.ImagePath = inboxPhotoUrl;
        }

        private void DisposeLocalFile()
        {
            if (!(this.Result is InboxImageDataProviderObject result) || result.InLibrary)
                return;
            string imagePath = result.ImagePath;
            if (string.IsNullOrEmpty(imagePath))
                return;
            try
            {
                System.IO.File.Delete(imagePath);
            }
            catch (Exception ex)
            {
            }
        }

        private void OnRequestComplete(Microsoft.Zune.Service.HttpWebResponse response, object requestArgs)
        {
            InboxImageQuery.RequestArgs requestArgs1 = (InboxImageQuery.RequestArgs)requestArgs;
            string requestUri = requestArgs1.m_requestUri;
            InboxImageDataProviderObject result = requestArgs1.m_result;
            int tcStart = requestArgs1.m_tcStart;
            Stream stream = (Stream)null;
            FileStream fileStream = (FileStream)null;
            try
            {
                stream = response.StatusCode == HttpStatusCode.OK ? response.GetResponseStream() : throw new HttpWebException(response);
                int tickCount1 = Environment.TickCount;
                this.SetWorkerStatus(DataProviderQueryStatus.ProcessingData);
                int contentLength = (int)response.ContentLength;
                byte[] buffer = new byte[40960];
                string tempFileName = Path.GetTempFileName();
                fileStream = new FileStream(tempFileName, FileMode.Truncate, FileAccess.Write, FileShare.None, 40960, FileOptions.SequentialScan);
                int count;
                for (; contentLength > 0 && (count = stream.Read(buffer, 0, 40960)) > 0; contentLength -= count)
                    fileStream.Write(buffer, 0, count);
                fileStream.Close();
                result.ImagePath = tempFileName;
                Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredSetResult), (object)new InboxImageQuery.DeferredSetResultArgs(result));
                this.SetWorkerStatus(DataProviderQueryStatus.Complete);
                int tickCount2 = Environment.TickCount;
            }
            catch (Exception ex)
            {
                this.SetWorkerStatus(DataProviderQueryStatus.Error);
            }
            finally
            {
                fileStream?.Close();
                stream?.Close();
            }
        }

        private void SetWorkerStatus(DataProviderQueryStatus eStatus) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredSetStatus), (object)new InboxImageQuery.DeferredSetStatusArgs(eStatus));

        private void DeferredSetStatus(object args)
        {
            if (!(args is InboxImageQuery.DeferredSetStatusArgs deferredSetStatusArgs))
                return;
            this.Status = deferredSetStatusArgs.m_eStatus;
        }

        private void DeferredSetResult(object argsObj)
        {
            if (!(argsObj is InboxImageQuery.DeferredSetResultArgs deferredSetResultArgs))
                return;
            deferredSetResultArgs.m_result.TransferToAppThread();
            if (this.Result == deferredSetResultArgs.m_result)
                return;
            this.Result = (object)deferredSetResultArgs.m_result;
        }

        private class RequestArgs
        {
            public readonly string m_requestUri;
            public readonly InboxImageDataProviderObject m_result;
            public readonly int m_tcStart;

            public RequestArgs(string requestUri, InboxImageDataProviderObject result, int tcStart)
            {
                this.m_requestUri = requestUri;
                this.m_result = result;
                this.m_tcStart = tcStart;
            }
        }

        private class DeferredSetStatusArgs
        {
            public readonly DataProviderQueryStatus m_eStatus;

            public DeferredSetStatusArgs(DataProviderQueryStatus eStatus) => this.m_eStatus = eStatus;
        }

        private class DeferredSetResultArgs
        {
            public readonly InboxImageDataProviderObject m_result;

            public DeferredSetResultArgs(InboxImageDataProviderObject result) => this.m_result = result;
        }
    }
}
