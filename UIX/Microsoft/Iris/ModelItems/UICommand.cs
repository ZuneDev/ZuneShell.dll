// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ModelItems.UICommand
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.ModelItems
{
    internal class UICommand : NotifyObjectBase, IUICommand
    {
        private bool _availableFlag = true;
        private InvokePriority _priority;

        public virtual bool Available
        {
            get => this._availableFlag;
            set
            {
                if (this._availableFlag == value)
                    return;
                this._availableFlag = value;
                this.FireNotification(NotificationID.Available);
            }
        }

        public InvokePriority Priority
        {
            get => this._priority;
            set
            {
                if (this._priority == value)
                    return;
                this._priority = value;
                this.FireNotification(NotificationID.Priority);
            }
        }

        public void Invoke()
        {
            if (this.Priority == InvokePriority.Normal)
                this.InvokeWorker();
            else
                DeferredCall.Post(DispatchPriority.Idle, new SimpleCallback(this.InvokeWorker));
        }

        public void InvokeWorker()
        {
            this.FireNotification(NotificationID.Invoked);
            this.OnInvoked();
        }

        protected virtual void OnInvoked()
        {
        }
    }
}
