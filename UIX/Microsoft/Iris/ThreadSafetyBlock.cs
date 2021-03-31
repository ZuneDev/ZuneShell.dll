// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ThreadSafetyBlock
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Session;
using System;
using System.Threading;

namespace Microsoft.Iris
{
    public struct ThreadSafetyBlock : IDisposable
    {
        private IThreadSafeObject _safe;
        private Thread _currentThread;
        private bool _error;

        public ThreadSafetyBlock(IThreadSafeObject safe)
        {
            this._safe = safe != null ? safe : throw new ArgumentNullException(nameof(safe));
            this._currentThread = Thread.CurrentThread;
            this._error = false;
            if (this._safe.Affinity == this._currentThread)
                return;
            if (this._currentThread == UIDispatcher.MainUIThread)
                this._safe.Affinity = UIDispatcher.MainUIThread;
            else
                this.ThrowError();
        }

        public void Dispose()
        {
            if (this._error || this._safe.Affinity == this._currentThread)
                return;
            this.ThrowError();
        }

        private void ThrowError()
        {
            this._error = true;
            throw new InvalidOperationException("Access to object occurred on an invalid thread");
        }
    }
}
