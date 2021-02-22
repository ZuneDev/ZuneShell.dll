// Decompiled with JetBrains decompiler
// Type: ZuneUI.PodcastCredentials
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Subscription;
using System;
using System.Threading;

namespace ZuneUI
{
    public class PodcastCredentials : NetworkSignInCredentials
    {
        private static PodcastCredentials _instance;
        private PodcastSignIn _inputDialog;

        private PodcastCredentials()
        {
        }

        public static PodcastCredentials Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PodcastCredentials();
                return _instance;
            }
        }

        public static bool HasInstance => _instance != null;

        public override void Phase2Init() => SubscriptionManager.Instance.SetCredentialHandler(EMediaTypes.eMediaTypePodcastSeries, new SubscriptionCredentialHandler(this.GetCredentials));

        private bool GetCredentials(SubscriptonCredentialRequestArguments args)
        {
            this._completed = false;
            if (!Application.IsApplicationThread && this._dialogClosed == null)
            {
                this._dialogClosed = new EventWaitHandle(false, EventResetMode.ManualReset);
                Application.DeferredInvoke(new DeferredInvokeHandler(this.ShowDialog), args);
                this._dialogClosed.WaitOne();
                this._dialogClosed.Close();
                this._dialogClosed = null;
            }
            return this._completed;
        }

        private void ShowDialog(object args)
        {
            if (!Application.IsApplicationThread || !(args is SubscriptonCredentialRequestArguments))
                return;
            if (this._inputDialog != null)
            {
                this._inputDialog.Dispose();
                this._inputDialog = null;
            }
            this._inputDialog = new PodcastSignIn((SubscriptonCredentialRequestArguments)args, new EventHandler(OnDialogSignIn), new EventHandler(OnDialogCanceled));
            this._inputDialog.Show();
        }

        public override void Dispose()
        {
            SubscriptionManager.Instance.SetCredentialHandler(EMediaTypes.eMediaTypePodcastSeries, null);
            if (this._inputDialog != null)
            {
                this._inputDialog.Dispose();
                this._inputDialog = null;
            }
            base.Dispose();
        }
    }
}
