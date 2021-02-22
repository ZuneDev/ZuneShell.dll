// Decompiled with JetBrains decompiler
// Type: ZuneUI.SubscriptionState
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneUI
{
    public class SubscriptionState : ModelItem
    {
        private bool _isSubscribed;
        private bool _seriesFound;
        private int _seriesId;

        public SubscriptionState(bool isSubscribed, bool seriesFound, int seriesId)
        {
            this._seriesFound = seriesFound;
            this._isSubscribed = isSubscribed;
            this._seriesId = seriesId;
        }

        public SubscriptionState()
        {
            this._seriesFound = false;
            this._isSubscribed = false;
            this._seriesId = -1;
        }

        public bool IsSubscribed
        {
            get => this._isSubscribed;
            set
            {
                if (this._isSubscribed == value)
                    return;
                this._isSubscribed = value;
                this.FirePropertyChanged(nameof(IsSubscribed));
            }
        }

        public bool SeriesFound
        {
            get => this._seriesFound;
            set
            {
                if (this._seriesFound == value)
                    return;
                this._seriesFound = value;
                this.FirePropertyChanged(nameof(SeriesFound));
            }
        }

        public int SeriesId
        {
            get => this._seriesId;
            set
            {
                if (this._seriesId == value)
                    return;
                this._seriesId = value;
                this.FirePropertyChanged(nameof(SeriesId));
            }
        }
    }
}
