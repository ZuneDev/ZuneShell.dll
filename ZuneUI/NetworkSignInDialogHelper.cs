// Decompiled with JetBrains decompiler
// Type: ZuneUI.NetworkSignInDialogHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.ErrorMapperApi;
using System;
using System.Net;
using UIXControls;

namespace ZuneUI
{
    public abstract class NetworkSignInDialogHelper : DialogHelper
    {
        private string _helpUrl;
        protected string _error;
        protected string _title;
        protected string _warning;
        protected string _hostName;
        protected string _realmName;
        protected Command _signInCommand;
        protected BooleanChoice _rememberPassword;

        public NetworkSignInDialogHelper(
          EventHandler signInHandler,
          EventHandler cancelHandler,
          string contentUI)
          : base(contentUI)
        {
            this._signInCommand = new Command((IModelItemOwner)this, Shell.LoadString(StringId.IDS_DIALOG_OK), new EventHandler(this.OnInvoked));
            if (signInHandler != null)
                this._signInCommand.Invoked += signInHandler;
            if (cancelHandler != null)
                this.Cancel.Invoked += cancelHandler;
            this._rememberPassword = new BooleanChoice((IModelItemOwner)this, Shell.LoadString(StringId.IDS_PODCAST_SIGN_IN_SAVE));
        }

        public string Title => this._title;

        public Command SignIn => this._signInCommand;

        public string Error => this._error;

        public string HelpUrl => this._helpUrl;

        public string Warning => this._warning;

        public string RealmName => this._realmName;

        public string HostName => this._hostName;

        public BooleanChoice RememberPassword => this._rememberPassword;

        public abstract string UserName { get; set; }

        public abstract string Password { get; set; }

        protected virtual void OnInvoked(object sender, EventArgs args) => this.Hide();

        protected override void OnDispose(bool disposing)
        {
            base.OnDispose(disposing);
            if (!disposing)
                return;
            if (this._rememberPassword != null)
            {
                this._rememberPassword.Dispose();
                this._rememberPassword = (BooleanChoice)null;
            }
            if (this._signInCommand == null)
                return;
            this._signInCommand.Dispose();
            this._signInCommand = (Command)null;
        }

        protected void SetError(HRESULT hrError)
        {
            if (!hrError.IsError)
                return;
            ErrorMapperResult descriptionAndUrl = Microsoft.Zune.ErrorMapperApi.ErrorMapperApi.GetMappedErrorDescriptionAndUrl(hrError.Int);
            if (descriptionAndUrl == null)
                return;
            this._error = descriptionAndUrl.Description;
            this._helpUrl = descriptionAndUrl.WebHelpUrl;
        }

        protected string AuthSchemeToString(AuthenticationSchemes authScheme)
        {
            switch (authScheme)
            {
                case AuthenticationSchemes.Digest:
                    return Shell.LoadString(StringId.IDS_PODCAST_SIGN_IN_DIGEST_AUTH);
                case AuthenticationSchemes.Negotiate:
                    return Shell.LoadString(StringId.IDS_PODCAST_SIGN_IN_NEGOTIATE_AUTH);
                case AuthenticationSchemes.Ntlm:
                    return Shell.LoadString(StringId.IDS_PODCAST_SIGN_IN_NTLM_AUTH);
                case AuthenticationSchemes.Basic:
                    return Shell.LoadString(StringId.IDS_PODCAST_SIGN_IN_BASIC_AUTH);
                default:
                    return (string)null;
            }
        }
    }
}
