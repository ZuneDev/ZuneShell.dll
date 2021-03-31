// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ModelItems.EditableTextData
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Session;
using System;

namespace Microsoft.Iris.ModelItems
{
    internal class EditableTextData : NotifyObjectBase
    {
        private int _maxLength;
        private bool _readOnly;
        private string _value;

        public EditableTextData()
        {
            this._value = string.Empty;
            this._maxLength = int.MaxValue;
        }

        public string Value
        {
            get => this._value;
            set
            {
                if (this.ReadOnly || !(this._value != value))
                    return;
                this._value = value;
                this.FireCodeEvent(this.ValueChanged);
                this.FireThreadSafeNotification(NotificationID.Value);
            }
        }

        public int MaxLength
        {
            get => this._maxLength;
            set
            {
                if (this._maxLength == value)
                    return;
                this._maxLength = value;
                this.FireCodeEvent(this.MaxLengthChanged);
                this.FireThreadSafeNotification(NotificationID.MaxLength);
            }
        }

        private void FireCodeEvent(EventHandler eventToFire)
        {
            if (eventToFire == null)
                return;
            eventToFire(this, EventArgs.Empty);
        }

        public bool ReadOnly
        {
            get => this._readOnly;
            set
            {
                if (this._readOnly == value)
                    return;
                this._readOnly = value;
                this.FireCodeEvent(this.ReadOnlyChanged);
                this.FireThreadSafeNotification(NotificationID.ReadOnly);
            }
        }

        public event EventHandler Submitted;

        public event EventHandler MaxLengthChanged;

        public event EventHandler ReadOnlyChanged;

        public event EventHandler ValueChanged;

        public void Submit() => DeferredCall.Post(DispatchPriority.AppEvent, new SimpleCallback(this.AsyncInvoke));

        private void AsyncInvoke()
        {
            this.FireCodeEvent(this.Submitted);
            this.FireThreadSafeNotification(NotificationID.Submitted);
            this.OnSubmitted();
        }

        protected virtual void OnSubmitted()
        {
        }
    }
}
