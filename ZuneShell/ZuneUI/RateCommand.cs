// Decompiled with JetBrains decompiler
// Type: ZuneUI.RateCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class RateCommand : UIXControls.MenuItemCommand
    {
        private int m_rating;

        public RateCommand() => this.Description = Shell.LoadString(StringId.IDS_LIBRARY_RATE_MENU_ITEM);

        public int Rating
        {
            get => this.m_rating;
            set
            {
                this.m_rating = value;
                this.FirePropertyChanged(nameof(Rating));
            }
        }
    }
}
