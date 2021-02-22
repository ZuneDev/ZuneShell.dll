// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeleteSubscriptionDownloadsDialog
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Util;
using System;
using UIXControls;

namespace ZuneUI
{
    public class DeleteSubscriptionDownloadsDialog : DialogHelper
    {
        private Command m_delete;
        private string m_title;
        private bool m_enabled;

        public static void ShowDialog()
        {
            string subscriptionDirectory = ZuneApplication.Service.GetSubscriptionDirectory();
            if (string.IsNullOrEmpty(subscriptionDirectory))
                MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_ACCOUNT_CLEAR_SUB_FAIL_TITLE), ZuneUI.Shell.LoadString(StringId.IDS_ACCOUNT_CLEAR_SUB_NO_DIRECTORY), (EventHandler)null);
            else
                new DeleteSubscriptionDownloadsDialog(subscriptionDirectory).Show();
        }

        public Command Delete => this.m_delete;

        public bool Enabled
        {
            get => this.m_enabled;
            private set
            {
                if (this.m_enabled == value)
                    return;
                this.Delete.Available = value;
                this.Cancel.Available = value;
                this.m_enabled = value;
                this.FirePropertyChanged(nameof(Enabled));
            }
        }

        public string Title => this.m_title;

        protected DeleteSubscriptionDownloadsDialog(string subscriptionDirectory)
          : base("res://ZuneShellResources!ManagementAccount.uix#DeleteSubscriptionDownloadsDialogContentUI")
        {
            this.m_enabled = true;
            this.m_title = ZuneUI.Shell.LoadString(StringId.IDS_ACCOUNT_CLEAR_SUBSCRIPTION_TITLE);
            this.Description = string.Format(ZuneUI.Shell.LoadString(StringId.IDS_ACCOUNT_CLEAR_SUBSCRIPTION_CONFIRM), (object)subscriptionDirectory);
            this.m_delete = new Command((IModelItemOwner)this, ZuneUI.Shell.LoadString(StringId.IDS_DIALOG_OK), new EventHandler(this.OnDeleteInvoked));
            this.Cancel.Invoked += new EventHandler(this.OnCancel);
        }

        private void OnCancel(object sender, EventArgs args) => this.Hide();

        private void OnDeleteInvoked(object sender, EventArgs args)
        {
            if (!this.Enabled)
                return;
            this.Enabled = false;
            ZuneApplication.Service.DeleteSubscriptionDownloads(new AsyncCompleteHandler(this.OnDeleteComplete));
        }

        private void OnDeleteComplete(HRESULT hr) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredDeleteCompleteEvent), (object)hr);

        private void DeferredDeleteCompleteEvent(object arg)
        {
            this.Enabled = true;
            HRESULT hresult = (HRESULT)arg;
            this.Hide();
            if (!hresult.IsError)
                return;
            ZuneUI.Shell.ShowErrorDialog(hresult.Int, StringId.IDS_ACCOUNT_CLEAR_SUB_FAIL_TITLE, StringId.IDS_ACCOUNT_CLEAR_SUB_FAIL_MESSAGE);
        }
    }
}
