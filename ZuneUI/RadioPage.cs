// Decompiled with JetBrains decompiler
// Type: ZuneUI.RadioPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class RadioPage : LibraryPage
    {
        private const string _UI = "res://ZuneShellResources!RadioPanel.uix#RadioLibrary";
        private const string _UIPath = "Collection\\Radio";
        private CollectionRadioPanel m_radioPanel;

        public CollectionRadioPanel RadioPanel
        {
            get => this.m_radioPanel;
            set
            {
                if (value == this.m_radioPanel)
                    return;
                this.m_radioPanel = value;
                this.FirePropertyChanged(nameof(RadioPanel));
            }
        }

        public RadioPage()
        {
            this.PivotPreference = Shell.MainFrame.Collection.Radio;
            this.IsRootPage = true;
            this.UI = "res://ZuneShellResources!RadioPanel.uix#RadioLibrary";
            this.UIPath = "Collection\\Radio";
            this.TransportControlStyle = TransportControlStyle.Music;
            this.PlaybackContext = PlaybackContext.Music;
            this.RadioPanel = new CollectionRadioPanel(this);
        }
    }
}
