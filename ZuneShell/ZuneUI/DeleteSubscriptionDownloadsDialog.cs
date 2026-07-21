// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeleteSubscriptionDownloadsDialog
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using Microsoft.Iris;
using Microsoft.Zune.Shell;
using UIXControls;

namespace ZuneUI
{
    public class DeleteSubscriptionDownloadsDialog : DialogHelper
    {
        private bool m_enabled;

        public static void ShowDialog()
        {
            string subscriptionDirectory = ZuneApplication.Service2.GetSubscriptionDirectory();
            if (string.IsNullOrEmpty(subscriptionDirectory))
                MessageBox.Show(Shell.LoadString(StringId.IDS_ACCOUNT_CLEAR_SUB_FAIL_TITLE), Shell.LoadString(StringId.IDS_ACCOUNT_CLEAR_SUB_NO_DIRECTORY), null);
            else
                new DeleteSubscriptionDownloadsDialog(subscriptionDirectory).Show();
        }

        public Command Delete { get; }

        public bool Enabled
        {
            get => m_enabled;
            private set
            {
                if (m_enabled == value)
                    return;
                Delete.Available = value;
                Cancel.Available = value;
                m_enabled = value;
                FirePropertyChanged(nameof(Enabled));
            }
        }

        public string Title { get; }

        protected DeleteSubscriptionDownloadsDialog(string subscriptionDirectory)
          : base("res://ZuneShellResources!ManagementAccount.uix#DeleteSubscriptionDownloadsDialogContentUI")
        {
            m_enabled = true;
            Title = Shell.LoadString(StringId.IDS_ACCOUNT_CLEAR_SUBSCRIPTION_TITLE);
            Description = string.Format(Shell.LoadString(StringId.IDS_ACCOUNT_CLEAR_SUBSCRIPTION_CONFIRM), subscriptionDirectory);
            Delete = new Command(this, Shell.LoadString(StringId.IDS_DIALOG_OK), OnDeleteInvoked);
            Cancel.Invoked += OnCancel;
        }

        private void OnCancel(object sender, EventArgs args) => Hide();

        private void OnDeleteInvoked(object sender, EventArgs args)
        {
            if (!Enabled)
                return;
            Enabled = false;
            ZuneApplication.Service2.DeleteSubscriptionDownloads(OnDeleteComplete);
        }

        private void OnDeleteComplete(HRESULT hr) => Application.DeferredInvoke(DeferredDeleteCompleteEvent, hr);

        private void DeferredDeleteCompleteEvent(object arg)
        {
            Enabled = true;
            var hresult = (HRESULT)arg;
            Hide();
            if (!hresult.IsError)
                return;
            Shell.ShowErrorDialog(hresult.Int, StringId.IDS_ACCOUNT_CLEAR_SUB_FAIL_TITLE, StringId.IDS_ACCOUNT_CLEAR_SUB_FAIL_MESSAGE);
        }
    }
}
