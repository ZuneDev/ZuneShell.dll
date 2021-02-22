// Decompiled with JetBrains decompiler
// Type: ZuneUI.InboxPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;
using ZuneXml;

namespace ZuneUI
{
    public class InboxPage : LibraryPage
    {
        private InboxPanel _inboxPanel;
        private MessageDetailsPanel _messageDetailsPanel;
        private Command _refreshPageCommand;
        public static readonly string InboxPageTemplate = "res://ZuneShellResources!InboxMainPanel.uix#InboxLibrary";

        public InboxPage()
        {
            this.PivotPreference = Shell.MainFrame.Social.Inbox;
            this.IsRootPage = true;
            this.UI = InboxPageTemplate;
            this.UIPath = "Social\\Inbox";
            this._refreshPageCommand = new Command(this);
            this._inboxPanel = new InboxPanel(this);
            this._messageDetailsPanel = new MessageDetailsPanel(this, true);
        }

        public override void InvokeSettings() => Shell.SettingsFrame.Settings.Account.Invoke();

        public override IPageState SaveAndRelease()
        {
            if (this.NavigationArguments == null)
                this.NavigationArguments = new Hashtable(1);
            if (this.Details.SelectedItem != null)
            {
                this.NavigationArguments["MessageId"] = ((MessageRoot)Details.SelectedItem).MessagingId;
                this.Details.SelectedItem = null;
            }
            else
                this.NavigationArguments.Remove("MessageId");
            this._inboxPanel.Release();
            this._messageDetailsPanel.Release();
            return base.SaveAndRelease();
        }

        public InboxPanel MainPanel => this._inboxPanel;

        public MessageDetailsPanel Details => this._messageDetailsPanel;

        public Command RefreshPageCommand => this._refreshPageCommand;
    }
}
