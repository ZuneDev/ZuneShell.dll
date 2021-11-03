// Decompiled with JetBrains decompiler
// Type: ZuneUI.QuickMixProgress
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.QuickMix;
using System.ComponentModel;

namespace ZuneUI
{
    public class QuickMixProgress : INotifyPropertyChanged
    {
        private float _progress;
        private int _secondsLeft;

        public event PropertyChangedEventHandler PropertyChanged;

        public QuickMixProgress() => QuickMix.Instance.OnProgress += new QuickMixProgressHandler(this.UpdateProgress);

        protected void FirePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged == null)
                return;
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateProgress(float progress, int secondsLeft) => Application.DeferredInvoke(delegate
       {
           this.Progress = progress;
           this.SecondsLeft = secondsLeft;
       }, null);

        public float Progress
        {
            get => this._progress;
            private set
            {
                if (value == (double)this._progress)
                    return;
                this._progress = value;
                this.FirePropertyChanged(nameof(Progress));
            }
        }

        public int SecondsLeft
        {
            get => this._secondsLeft;
            private set
            {
                if (value == this._secondsLeft)
                    return;
                this._secondsLeft = value;
                this.FirePropertyChanged(nameof(SecondsLeft));
            }
        }
    }
}
