// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Data.Resource
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.OS;
using System;

namespace Microsoft.Iris.Data
{
    internal abstract class Resource
    {
        protected string _uri;
        private bool _forceSynchronous;
        private IntPtr _buffer;
        private uint _length;
        private bool _requiresMemoryFree;
        private ResourceStatus _status;
        private string _errorDetails;
        private int _acquisitions;
        private ResourceAcquisitionCompleteHandler _completeHandlers;

        public Resource(string uri, bool forceSynchronous)
        {
            this._uri = uri;
            this._forceSynchronous = forceSynchronous;
            this._status = ResourceStatus.NeedsAcquire;
        }

        public string Uri => this._uri;

        public abstract string Identifier { get; }

        public void Acquire() => this.Acquire(null);

        public void Acquire(ResourceAcquisitionCompleteHandler completeHandler)
        {
            ++this._acquisitions;
            if (completeHandler != null)
                this._completeHandlers += completeHandler;
            if (this._status == ResourceStatus.Acquiring)
                return;
            if (this._status != ResourceStatus.Available)
            {
                this._status = ResourceStatus.Acquiring;
                this._errorDetails = null;
                this.StartAcquisition(this._forceSynchronous);
            }
            else
            {
                if (completeHandler == null)
                    return;
                completeHandler(this);
            }
        }

        public void Free() => this.Free(null);

        public void Free(ResourceAcquisitionCompleteHandler completeHandler)
        {
            --this._acquisitions;
            if (completeHandler != null)
                this._completeHandlers -= completeHandler;
            if (this._acquisitions != 0)
                return;
            if (this._status == ResourceStatus.Acquiring)
                this.CancelAcquisition();
            else if (this._buffer != IntPtr.Zero)
            {
                if (this._requiresMemoryFree)
                    Resource.FreeNativeBuffer(this._buffer);
                this._buffer = IntPtr.Zero;
            }
            this._status = ResourceStatus.NeedsAcquire;
        }

        public ResourceStatus Status
        {
            get => this._status;
            set => this._status = value;
        }

        public string ErrorDetails => this._errorDetails;

        public IntPtr Buffer => this._buffer;

        public uint Length => this._length;

        public bool ForceSynchronous => this._forceSynchronous;

        protected abstract void StartAcquisition(bool forceSynchronous);

        protected abstract void CancelAcquisition();

        protected void NotifyAcquisitionComplete(
          IntPtr buffer,
          uint length,
          bool requiresMemoryFree,
          string errorDetails)
        {
            this._buffer = buffer;
            this._length = length;
            this._requiresMemoryFree = requiresMemoryFree;
            if (buffer != IntPtr.Zero)
            {
                this._status = ResourceStatus.Available;
            }
            else
            {
                this._status = ResourceStatus.Error;
                if (errorDetails == null)
                    errorDetails = string.Format("Failed to acquire resource '{0}'", Identifier);
                this._errorDetails = errorDetails;
            }
            if (this._completeHandlers == null)
                return;
            this._completeHandlers(this);
        }

        protected static IntPtr AllocNativeBuffer(uint length) => NativeApi.MemAlloc(length, false);

        protected static void FreeNativeBuffer(IntPtr buffer) => NativeApi.MemFree(buffer);

        private void FireAcquisitionCompleteHandlers()
        {
            if (this._completeHandlers == null)
                return;
            this._completeHandlers(this);
            this._completeHandlers = null;
        }

        public override string ToString() => this._uri;
    }
}
