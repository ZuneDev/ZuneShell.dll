// Decompiled with JetBrains decompiler
// Type: ZuneUI.ProxySignIn
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.UserCredential;
using System;
using System.Net;

namespace ZuneUI
{
    public class ProxySignIn : NetworkSignInDialogHelper
    {
        private UserCredentialRequestArguments _credentials;

        public override string UserName
        {
            get => this._credentials.Credential.UserName;
            set => this._credentials.Credential.UserName = value;
        }

        public override string Password
        {
            get => this._credentials.Credential.Password;
            set => this._credentials.Credential.Password = value;
        }

        public ProxySignIn(
          UserCredentialRequestArguments credentials,
          EventHandler signInHandler,
          EventHandler cancelHandler)
          : base(signInHandler, cancelHandler, "res://ZuneShellResources!SignInDialog.uix#ProxySignInDialogUI")
        {
            this._credentials = credentials;
            this.Cancel.Invoked += new EventHandler(this.CancelInvoked);
            this._credentials.Credential = new NetworkCredential();
            this._credentials.Credential.UserName = this._credentials.LastUserName;
            if (this._credentials.LastError < 0)
                this._error = Shell.LoadString(StringId.IDS_PROXY_LOGIN_BAD_CREDENTIALS);
            this._title = Shell.LoadString(StringId.IDS_PROXY_LOGIN_TITLE);
            this._realmName = this._credentials.Realm;
            this._hostName = this._credentials.Host;
            this.SetWarning(credentials);
        }

        private void CancelInvoked(object sender, EventArgs args)
        {
            this.UserName = string.Empty;
            this.Password = string.Empty;
            this.Hide();
        }

        protected override void OnInvoked(object sender, EventArgs args)
        {
            this._credentials.Save = this._rememberPassword.Value;
            base.OnInvoked(sender, args);
        }

        private void SetWarning(UserCredentialRequestArguments credentials)
        {
            string str = (string)null;
            if (!credentials.IsAuthenticationSchemeSafe)
                str = this.AuthSchemeToString(credentials.AuthScheme);
            if (str != null)
                this._warning = string.Format(Shell.LoadString(StringId.IDS_PODCAST_SIGN_IN_INSECURE_AUTH_WARNING), (object)str);
            else
                this._warning = (string)null;
        }
    }
}
