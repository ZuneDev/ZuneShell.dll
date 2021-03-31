// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Command
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Session;
using System;
using System.ComponentModel;

namespace Microsoft.Iris
{
    public class Command : ModelItem, ICommand, INotifyPropertyChanged
    {
        private static readonly EventCookie s_invokedEvent = EventCookie.ReserveSlot();
        private bool _availableFlag = true;
        private InvokePolicy _priority = InvokePolicy.AsynchronousNormal;

        public Command(IModelItemOwner owner, string description, EventHandler invokedHandler)
          : base(owner, description)
        {
            if (invokedHandler == null)
                return;
            this.Invoked += invokedHandler;
        }

        public Command(IModelItemOwner owner, EventHandler invokedHandler)
          : this(owner, (string)null, invokedHandler)
        {
        }

        public Command(IModelItemOwner owner)
          : this(owner, (string)null, (EventHandler)null)
        {
        }

        public Command()
          : this((IModelItemOwner)null)
        {
        }

        public virtual bool Available
        {
            get
            {
                using (this.ThreadValidator)
                    return this._availableFlag;
            }
            set
            {
                using (this.ThreadValidator)
                {
                    if (this._availableFlag == value)
                        return;
                    this._availableFlag = value;
                    this.FirePropertyChanged(nameof(Available));
                }
            }
        }

        public InvokePolicy Priority
        {
            get
            {
                using (this.ThreadValidator)
                    return this._priority;
            }
            set
            {
                using (this.ThreadValidator)
                {
                    if (this._priority == value)
                        return;
                    this._priority = value;
                    this.FirePropertyChanged(nameof(Priority));
                }
            }
        }

        public void Invoke() => this.Invoke(this.Priority);

        public void Invoke(InvokePolicy invokePolicy)
        {
            using (this.ThreadValidator)
            {
                if (invokePolicy != InvokePolicy.Synchronous)
                {
                    DispatchPriority priority = DispatchPriority.AppEvent;
                    if (invokePolicy == InvokePolicy.AsynchronousLowPri)
                        priority = DispatchPriority.Idle;
                    DeferredCall.Post(priority, new SimpleCallback(this.InvokeWorker));
                }
                else
                    this.InvokeWorker();
            }
        }

        private void InvokeWorker()
        {
            if (this.IsDisposed)
                return;
            this.FirePropertyChanged("Invoked");
            if (this.GetEventHandler(Command.s_invokedEvent) is EventHandler eventHandler)
                eventHandler((object)this, EventArgs.Empty);
            this.OnInvoked();
        }

        public event EventHandler Invoked
        {
            add
            {
                using (this.ThreadValidator)
                    this.AddEventHandler(Command.s_invokedEvent, (Delegate)value);
            }
            remove
            {
                using (this.ThreadValidator)
                    this.RemoveEventHandler(Command.s_invokedEvent, (Delegate)value);
            }
        }

        protected virtual void OnInvoked()
        {
        }
    }
}
