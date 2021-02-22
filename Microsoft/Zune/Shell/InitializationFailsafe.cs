// Decompiled with JetBrains decompiler
// Type: Microsoft.Zune.Shell.InitializationFailsafe
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Threading;

namespace Microsoft.Zune.Shell
{
    internal class InitializationFailsafe
    {
        private bool _complete;
        private DeferredInvokeHandler _onCompleteHandler;
        private System.Threading.Timer _failsafeTimer;

        public void Initialize(DeferredInvokeHandler onCompleteHandler)
        {
            if (this._complete)
            {
                Application.DeferredInvoke(onCompleteHandler, DeferredInvokePriority.Low);
            }
            else
            {
                int dueTime = 30000;
                this._onCompleteHandler = onCompleteHandler;
                this._failsafeTimer = new System.Threading.Timer(new TimerCallback(this.FailsafeCallback), (object)null, dueTime, -1);
            }
        }

        public void Complete()
        {
            if (this._complete)
                return;
            this._complete = true;
            if (this._onCompleteHandler != null)
                Application.DeferredInvoke(this._onCompleteHandler, DeferredInvokePriority.Low);
            if (this._failsafeTimer == null)
                return;
            this._failsafeTimer.Change(-1, -1);
        }

        private void FailsafeCallback(object state)
        {
            if (this._complete)
                return;
            this._complete = true;
            Application.DeferredInvoke(this._onCompleteHandler, DeferredInvokePriority.Low);
        }
    }
}
