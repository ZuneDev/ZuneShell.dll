// Decompiled with JetBrains decompiler
// Type: ZuneUI.NewReview
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public class NewReview : NotifyPropertyChangedImpl
    {
        private DateTime _date;
        private string _userName;
        private string _comment;
        private string _title;
        private float _rating;

        public DateTime Date
        {
            get => this._date;
            set
            {
                if (!(this._date != value))
                    return;
                this._date = value;
                this.FirePropertyChanged(nameof(Date));
            }
        }

        public string UserName
        {
            get => this._userName;
            set
            {
                if (!(this._userName != value))
                    return;
                this._userName = value;
                this.FirePropertyChanged(nameof(UserName));
            }
        }

        public string Comment
        {
            get => this._comment;
            set
            {
                if (!(this._comment != value))
                    return;
                this._comment = value;
                this.FirePropertyChanged(nameof(Comment));
            }
        }

        public string Title
        {
            get => this._title;
            set
            {
                if (!(this._title != value))
                    return;
                this._title = value;
                this.FirePropertyChanged(nameof(Title));
            }
        }

        public float Rating
        {
            get => this._rating;
            set
            {
                if ((double)this._rating == (double)value)
                    return;
                this._rating = value;
                this.FirePropertyChanged(nameof(Rating));
            }
        }
    }
}
