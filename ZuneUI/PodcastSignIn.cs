// Decompiled with JetBrains decompiler
// Type: ZuneUI.PodcastSignIn
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Subscription;
using System;
using System.Net;

namespace ZuneUI
{
    public class PodcastSignIn : NetworkSignInDialogHelper
    {
        private SubscriptonCredentialRequestArguments _credentials;
        private string _seriesTitle;

        public string TargetUrl => this._credentials.TargetUrl;

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

        public string SeriesTitle => this._seriesTitle;

        public PodcastSignIn(
          SubscriptonCredentialRequestArguments credentials,
          EventHandler signInHandler,
          EventHandler cancelHandler)
          : base(signInHandler, cancelHandler, "res://ZuneShellResources!PodcastDialogs.uix#PodcastSignInDialogUI")
        {
            this._credentials = credentials;
            this.Cancel.Invoked += new EventHandler(((NetworkSignInDialogHelper)this).OnInvoked);
            this._seriesTitle = this._credentials.SubscriptionMediaId <= 0 ? string.Empty : PlaylistManager.GetFieldValue<string>(this._credentials.SubscriptionMediaId, EListType.ePodcastList, 344, string.Empty);
            this._title = Shell.LoadString(StringId.IDS_PODCAST_SIGN_IN_TITLE);
            Uri uri = new Uri(this._credentials.TargetUrl);
            this._hostName = uri.Scheme + Uri.SchemeDelimiter + uri.Host;
            this._realmName = this._credentials.Realm;
            this._credentials.Credential = new NetworkCredential();
            this._credentials.Credential.UserName = this._credentials.LastUserName;
            if (this._credentials.LastError < 0)
                this.SetError((HRESULT)this._credentials.LastError);
            this.SetWarning(credentials);
        }

        protected override void OnInvoked(object sender, EventArgs args)
        {
            this._credentials.Save = this._rememberPassword.Value;
            base.OnInvoked(sender, args);
        }

        private void SetWarning(SubscriptonCredentialRequestArguments credentials)
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
