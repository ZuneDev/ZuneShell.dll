// Decompiled with JetBrains decompiler
// Type: ZuneUI.SubscriptionEndingDialog
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;
using UIXControls;

namespace ZuneUI
{
    public class SubscriptionEndingDialog : DialogHelper
    {
        private BooleanChoice m_neverShowAgain;
        private Command m_subscribe;
        private Command m_delete;
        private string m_title;

        public static void Show(DateTime endDate)
        {
            if (ZuneShell.DefaultInstance.Management.InhibitSubscriptionEndingWarning || ZuneShell.DefaultInstance.Management.CurrentCategoryPage != null && ZuneShell.DefaultInstance.Management.CurrentCategoryPage.IsWizard)
                return;
            new SubscriptionEndingDialog(endDate).Show();
        }

        public Command Delete => this.m_delete;

        public Command Subscribe => this.m_subscribe;

        public BooleanChoice NeverShowAgain => this.m_neverShowAgain;

        public string Title => this.m_title;

        protected SubscriptionEndingDialog(DateTime endDate)
          : base("res://ZuneShellResources!BillingOffer.uix#SubscriptionEndingDialogContentUI")
        {
            if (endDate >= DateTime.Today)
            {
                this.m_title = Shell.LoadString(StringId.IDS_BILLING_SUBSCRIPTION_ENDING_TITLE);
                this.Description = string.Format(Shell.LoadString(StringId.IDS_BILLING_SUBSCRIPTION_ENDING_WARNING), endDate);
            }
            else
            {
                this.m_title = Shell.LoadString(StringId.IDS_BILLING_SUBSCRIPTION_ENDED_TITLE);
                this.Description = Shell.LoadString(StringId.IDS_BILLING_SUBSCRIPTION_ENDED_WARNING);
            }
            this.m_subscribe = new Command(this, Shell.LoadString(StringId.IDS_BILLING_RENEW_SUBSCRIPTION), new EventHandler(this.OnSubscribe));
            this.m_delete = new Command(this, Shell.LoadString(StringId.IDS_ACCOUNT_CLEAR_SUBSCRIPTION_BUTTON), new EventHandler(this.OnDeleteInvoked));
            this.Cancel.Invoked += new EventHandler(this.OnCancel);
            this.m_neverShowAgain = new BooleanChoice(this, Shell.LoadString(StringId.IDS_DONT_SHOW_THIS_MESSAGE_AGAIN));
            this.m_neverShowAgain.Value = false;
            this.m_neverShowAgain.ChosenChanged += new EventHandler(this.OnNeverShowAgain);
        }

        private void OnCancel(object sender, EventArgs args) => this.Hide();

        private void OnNeverShowAgain(object sender, EventArgs args) => ZuneShell.DefaultInstance.Management.InhibitSubscriptionEndingWarning = this.m_neverShowAgain.Value;

        private void OnSubscribe(object sender, EventArgs args)
        {
            ZuneShell.DefaultInstance.Execute("Settings\\Account\\PurchaseSubscription", null);
            this.Hide();
        }

        private void OnDeleteInvoked(object sender, EventArgs args)
        {
            DeleteSubscriptionDownloadsDialog.ShowDialog();
            this.Hide();
        }
    }
}
