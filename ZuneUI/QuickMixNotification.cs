// Decompiled with JetBrains decompiler
// Type: ZuneUI.QuickMixNotification
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class QuickMixNotification : Notification
    {
        private readonly string _title;
        private readonly string _text;
        private readonly bool _showWebHelpLink;

        public QuickMixNotification(
          string title,
          string text,
          NotificationState state,
          bool showWebHelpLink,
          int timeout)
          : base(NotificationTask.QuickMix, state, timeout)
        {
            this._title = title;
            this._text = text;
            this._showWebHelpLink = showWebHelpLink;
        }

        public string Title => this._title;

        public string Text => this._text;

        public bool ShowWebHelpLink => this._showWebHelpLink;
    }
}
