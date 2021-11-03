// Decompiled with JetBrains decompiler
// Type: ZuneUI.NetworkSignInCredentials
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Threading;

namespace ZuneUI
{
    public abstract class NetworkSignInCredentials : IDisposable
    {
        protected EventWaitHandle _dialogClosed;
        protected bool _completed;
        protected uint _cancelCount;
        protected uint _signInCount;

        protected NetworkSignInCredentials() => this._cancelCount = 0U;

        public virtual void Dispose()
        {
            if (this._dialogClosed == null)
                return;
            this._dialogClosed.Close();
        }

        public abstract void Phase2Init();

        protected virtual void OnDialogSignIn(object sender, EventArgs args)
        {
            if (this._dialogClosed != null)
                this._dialogClosed.Set();
            ++this._signInCount;
            this._completed = true;
        }

        protected virtual void OnDialogCanceled(object sender, EventArgs args)
        {
            if (this._dialogClosed != null)
                this._dialogClosed.Set();
            ++this._cancelCount;
            this._completed = false;
        }

        protected uint CancelCount => this._cancelCount;

        protected uint SignInCount => this._signInCount;
    }
}
