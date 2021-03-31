// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Session.DispatcherTimer
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Debug;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Queues;
using System;

namespace Microsoft.Iris.Session
{
    internal class DispatcherTimer
    {
        private TimeSpan _interval;
        private bool _autoRepeat;
        private object _userData;
        private DispatcherTimer.TimerCallback _callback;
        private long _timeBase;
        private TimeoutManager _timeoutManager;
        private ITimerOwner _owner;

        public DispatcherTimer(ITimerOwner owner)
        {
            this._owner = owner;
            this._interval = TimeSpan.FromMilliseconds(100.0);
            this._autoRepeat = true;
            this._timeBase = SystemTickCount.Milliseconds;
            this._timeoutManager = UIDispatcher.CurrentDispatcher.TimeoutManager;
        }

        public DispatcherTimer()
          : this(null)
        {
        }

        public TimeSpan TimeSpanInterval
        {
            get => this._interval;
            set
            {
                if (!(this._interval != value))
                    return;
                this._interval = value;
                if (this.Enabled)
                    this.Start();
                this.FireNotification(NotificationID.Interval);
            }
        }

        public int Interval
        {
            get => (int)this.TimeSpanInterval.TotalMilliseconds;
            set => this.TimeSpanInterval = TimeSpan.FromMilliseconds(value);
        }

        public bool Enabled
        {
            get => this._callback != null;
            set
            {
                if (this.Enabled == value)
                    return;
                if (value)
                    this.Start();
                else
                    this.Stop();
            }
        }

        public bool AutoRepeat
        {
            get => this._autoRepeat;
            set
            {
                if (this._autoRepeat == value)
                    return;
                this._autoRepeat = value;
                this.FireNotification(NotificationID.AutoRepeat);
            }
        }

        internal object UserData
        {
            get => this._userData;
            set
            {
                if (this._userData == value)
                    return;
                this._userData = value;
            }
        }

        public void Start()
        {
            bool enabled = this.Enabled;
            this.StopWorker();
            this._timeBase = SystemTickCount.Milliseconds;
            this._callback = new DispatcherTimer.TimerCallback(this);
            this.ScheduleCallback(this._timeBase);
            this.FireEnabledChange(enabled);
        }

        public void Stop()
        {
            bool enabled = this.Enabled;
            this.StopWorker();
            this.FireEnabledChange(enabled);
        }

        private void StopWorker()
        {
            if (this._callback == null)
                return;
            this._timeoutManager.CancelTimeout(_callback);
            this._callback = null;
        }

        private void FireEnabledChange(bool wasEnabled)
        {
            if (this.Enabled == wasEnabled)
                return;
            this.FireNotification(NotificationID.Enabled);
        }

        public override string ToString()
        {
            string str = " one-shot";
            if (this._autoRepeat)
                str = " repeating";
            return "[" + Interval + str + "] -> " + DebugHelpers.DEBUG_ObjectToString(Tick);
        }

        public event EventHandler Tick;

        private void ScheduleCallback(long currentTimeInMilliseconds)
        {
            if (this._callback == null)
                return;
            TimeSpan timeSpan1 = this._interval;
            TimeSpan timeSpan2 = TimeSpan.FromMilliseconds(currentTimeInMilliseconds - this._timeBase);
            if (timeSpan2 >= timeSpan1)
            {
                long ticks = timeSpan1.Ticks;
                if (ticks > 0L)
                    timeSpan1 = TimeSpan.FromTicks(ticks * ((timeSpan2.Ticks + ticks / 2L) / ticks));
            }
            this._timeBase += (long)timeSpan1.TotalMilliseconds;
            this._timeoutManager.SetTimeoutRelative(_callback, TimeSpan.FromMilliseconds(this._timeBase - currentTimeInMilliseconds));
        }

        private void CallTickHandlers(DispatcherTimer.TimerCallback callback)
        {
            if (!this.CallbackValid(callback))
                return;
            if (this._autoRepeat)
            {
                this.ScheduleCallback(SystemTickCount.Milliseconds);
            }
            else
            {
                this._callback = null;
                this.FireEnabledChange(true);
            }
            if (this.Tick != null)
                this.Tick(this._owner != null ? _owner : (object)this, EventArgs.Empty);
            this.FireNotification(NotificationID.Tick);
        }

        private bool CallbackValid(DispatcherTimer.TimerCallback callback) => callback == this._callback;

        private void FireNotification(string id)
        {
            if (this._owner == null)
                return;
            this._owner.OnTimerPropertyChanged(id);
        }

        private class TimerCallback : QueueItem
        {
            private DispatcherTimer _timer;

            public TimerCallback(DispatcherTimer timer) => this._timer = timer;

            public override void Dispatch() => this._timer.CallTickHandlers(this);

            public override string ToString()
            {
                string str = "";
                if (!this._timer.CallbackValid(this))
                    str = "CANCELED ";
                return str + this.GetType().Name + " -> " + _timer;
            }
        }

        internal static class SystemTickCount
        {
            private static long s_tickCount;
            private static int s_lastTickCount;

            public static long Milliseconds
            {
                get
                {
                    Refresh();
                    return s_tickCount;
                }
            }

            private static void Refresh()
            {
                int tickCount = Environment.TickCount;
                long num = tickCount < s_lastTickCount ? int.MaxValue - s_lastTickCount + tickCount : tickCount - s_lastTickCount;
                s_tickCount += num;
                s_lastTickCount = tickCount;
            }
        }
    }
}
