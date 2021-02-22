// Decompiled with JetBrains decompiler
// Type: ZuneUI.ProxyCredentials
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.UserCredential;
using System;
using System.Threading;

namespace ZuneUI
{
    public class ProxyCredentials : NetworkSignInCredentials
    {
        private const int _maxCancelCount = 3;
        private const int _maxFailedCount = 3;
        private UserCredentialRequestArguments _credentials;
        private static ProxyCredentials _instance;
        private ProxySignIn _inputDialog;
        private string _lastUIPathPrompted;
        private string _lastSignInTargetUrl;

        private ProxyCredentials()
        {
        }

        public static ProxyCredentials Instance
        {
            get
            {
                if (ProxyCredentials._instance == null)
                    ProxyCredentials._instance = new ProxyCredentials();
                return ProxyCredentials._instance;
            }
        }

        public static bool HasInstance => ProxyCredentials._instance != null;

        public void Reset()
        {
            this._lastUIPathPrompted = "";
            this._signInCount = 0U;
            this._cancelCount = 0U;
        }

        public override void Phase2Init() => UserCredentialManager.Instance.SetCredentialHandler(new UserCredentialHandler(this.GetCredentials));

        private bool GetCredentials(UserCredentialRequestArguments args)
        {
            this._completed = false;
            if (this.CancelCount >= 3U || this.SignInCount >= 3U || this._lastUIPathPrompted == ZuneShell.DefaultInstance.CurrentPage.UIPath && this._lastSignInTargetUrl != args.TargetUrl)
            {
                if (args != null && args.Credential != null)
                {
                    args.Credential.UserName = string.Empty;
                    args.Credential.Password = string.Empty;
                }
            }
            else if (!Application.IsApplicationThread && this._dialogClosed == null)
            {
                this._dialogClosed = new EventWaitHandle(false, EventResetMode.ManualReset);
                Application.DeferredInvoke(new DeferredInvokeHandler(this.ShowDialog), (object)args);
                this._dialogClosed.WaitOne();
                this._dialogClosed.Close();
                this._dialogClosed = (EventWaitHandle)null;
            }
            return this._completed;
        }

        private void ShowDialog(object args)
        {
            if (!Application.IsApplicationThread || !(args is UserCredentialRequestArguments))
                return;
            if (this._inputDialog != null)
            {
                this._inputDialog.Dispose();
                this._inputDialog = null;
            }
            this._credentials = (UserCredentialRequestArguments)args;
            this._inputDialog = new ProxySignIn(this._credentials, new EventHandler(OnDialogSignIn), new EventHandler(OnDialogCanceled));
            this._inputDialog.Show();
            this._lastUIPathPrompted = ZuneShell.DefaultInstance.CurrentPage.UIPath;
        }

        protected override void OnDialogSignIn(object sender, EventArgs args)
        {
            if (this._credentials != null)
                this._lastSignInTargetUrl = this._credentials.TargetUrl;
            base.OnDialogSignIn(sender, args);
        }

        public override void Dispose()
        {
            UserCredentialManager.Instance.SetCredentialHandler(null);
            if (this._inputDialog != null)
            {
                this._inputDialog.Dispose();
                this._inputDialog = null;
            }
            base.Dispose();
        }
    }
}
