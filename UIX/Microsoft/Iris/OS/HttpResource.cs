// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.OS.HttpResource
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using System;

namespace Microsoft.Iris.OS
{
    internal class HttpResource : Resource
    {
        private IntPtr _handle;
        private NativeApi.DownloadCompleteHandler _pendingCallback;

        public HttpResource(string uri, bool forceSynchronous)
          : base(uri, forceSynchronous)
        {
        }

        public override string Identifier => this._uri;

        protected override void StartAcquisition(bool forceSynchronous)
        {
            this._pendingCallback = new NativeApi.DownloadCompleteHandler(this.OnHttpDownloadComplete);
            int num = (int)NativeApi.SpHttpDownload(this._uri, this._pendingCallback, IntPtr.Zero, out this._handle);
        }

        private void OnHttpDownloadComplete(IntPtr handle, int error, uint length, IntPtr context)
        {
            IntPtr buffer = IntPtr.Zero;
            string errorDetails = null;
            switch (error)
            {
                case 0:
                    buffer = NativeApi.DownloadGetBuffer(this._handle);
                    break;
                case 1:
                    errorDetails = string.Format("Invalid URI: '{0}'", _uri);
                    break;
                case 2:
                    errorDetails = string.Format("Unable to connect to web host: '{0}'", _uri);
                    break;
                default:
                    errorDetails = string.Format("Failed to complete download from '{0}'", _uri);
                    break;
            }
            int num = (int)NativeApi.SpDownloadClose(this._handle);
            this._handle = IntPtr.Zero;
            this._pendingCallback = null;
            this.NotifyAcquisitionComplete(buffer, length, true, errorDetails);
        }

        protected override void CancelAcquisition()
        {
            if (!(this._handle != IntPtr.Zero))
                return;
            int num = (int)NativeApi.SpDownloadClose(this._handle);
            this._handle = IntPtr.Zero;
            this._pendingCallback = null;
        }
    }
}
