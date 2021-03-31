// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.OS.DllResource
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using System;

namespace Microsoft.Iris.OS
{
    internal class DllResource : Resource
    {
        private string _dll;
        private string _identifier;
        private IntPtr _buffer;
        private uint _length;

        internal DllResource(string uri, string dll, string identifier)
          : base(uri, true)
        {
            this._dll = dll;
            this._identifier = identifier;
        }

        public override string Identifier => this._identifier;

        protected override void StartAcquisition(bool forceSynchronous)
        {
            string errorDetails = null;
            if (this._buffer == IntPtr.Zero && !NativeApi.SpLoadBinaryResource(this._dll, this._identifier, !DllResources.StaticDllResourcesOnly, out this._buffer, out this._length))
                errorDetails = string.Format("Resource not found: res://{0}!{1}", _dll, _identifier);
            this.NotifyAcquisitionComplete(this._buffer, this._length, false, errorDetails);
        }

        protected override void CancelAcquisition()
        {
        }

        public override string ToString() => this._dll + "|" + this._identifier.ToLowerInvariant();
    }
}
