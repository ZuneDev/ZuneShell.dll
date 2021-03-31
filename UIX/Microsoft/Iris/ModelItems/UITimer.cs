// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ModelItems.UITimer
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.ModelItems
{
    internal sealed class UITimer : DisposableNotifyObjectBase, ITimerOwner
    {
        private DispatcherTimer _dispatcherTimer;

        public UITimer() => this._dispatcherTimer = new DispatcherTimer((ITimerOwner)this);

        protected override void OnDispose()
        {
            this.Enabled = false;
            base.OnDispose();
        }

        void ITimerOwner.OnTimerPropertyChanged(string id) => this.FireNotification(id);

        public int Interval
        {
            get => this._dispatcherTimer.Interval;
            set => this._dispatcherTimer.Interval = value;
        }

        public bool Enabled
        {
            get => this._dispatcherTimer.Enabled;
            set => this._dispatcherTimer.Enabled = value;
        }

        public bool AutoRepeat
        {
            get => this._dispatcherTimer.AutoRepeat;
            set => this._dispatcherTimer.AutoRepeat = value;
        }

        internal object UserData
        {
            get => this._dispatcherTimer.UserData;
            set => this._dispatcherTimer.UserData = value;
        }

        public void Start() => this._dispatcherTimer.Start();

        public void Stop() => this._dispatcherTimer.Stop();

        public override string ToString() => this.GetType().Name + this._dispatcherTimer.ToString();
    }
}
