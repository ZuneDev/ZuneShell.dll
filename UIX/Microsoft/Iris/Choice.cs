// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Choice
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.ModelItems;
using System;
using System.Collections;
using System.ComponentModel;

namespace Microsoft.Iris
{
    public class Choice :
      ModelItem,
      IValueRange,
      IModelItem,
      INotifyPropertyChanged,
      IModelItemOwner,
      IUIChoice,
      IUIValueRange,
      IDisposableObject,
      INotifyObject,
      AssemblyObjectProxyHelper.IFrameworkProxyObject,
      AssemblyObjectProxyHelper.IAssemblyProxyObject
    {
        private NotifyService _notifier = new NotifyService();
        private static readonly EventCookie s_chosenChangedEvent = EventCookie.ReserveSlot();
        private Microsoft.Iris.ModelItems.Choice _choice;
        private CodeListeners _listeners;
        private ModelItem _lastChosen;

        public Choice(IModelItemOwner owner, string description, IList options)
          : base(owner, description)
        {
            this.Initialize();
            this.Options = options;
        }

        public Choice(IModelItemOwner owner, string description)
          : base(owner, description)
          => this.Initialize();

        public Choice(IModelItemOwner owner)
          : this(owner, null)
        {
        }

        public Choice()
          : this(null)
        {
        }

        protected override void OnDispose(bool disposing)
        {
            base.OnDispose(disposing);
            if (disposing)
            {
                this._notifier.ClearListeners();
                this._choice.Dispose(this);
                this._listeners.Dispose(this);
            }
            this._choice = null;
        }

        object AssemblyObjectProxyHelper.IFrameworkProxyObject.FrameworkObject => this;

        object AssemblyObjectProxyHelper.IAssemblyProxyObject.AssemblyObject => this;

        public IList Options
        {
            get
            {
                using (this.ThreadValidator)
                    return (IList)AssemblyLoadResult.UnwrapObject(_choice.Options);
            }
            set
            {
                using (this.ThreadValidator)
                {
                    IList potentialOptionsWrapped = (IList)AssemblyLoadResult.WrapObject(value);
                    this.ValidateOptionsList(potentialOptionsWrapped, value);
                    this._choice.Options = potentialOptionsWrapped;
                }
            }
        }

        public object ChosenValue
        {
            get
            {
                using (this.ThreadValidator)
                    return AssemblyLoadResult.UnwrapObject(this._choice.ChosenValue);
            }
            set
            {
                using (this.ThreadValidator)
                {
                    int index;
                    string error;
                    if (!this._choice.ValidateOption(AssemblyLoadResult.WrapObject(value), out index, out error))
                        throw new ArgumentException(error);
                    this._choice.ChosenIndex = index;
                }
            }
        }

        public int ChosenIndex
        {
            get
            {
                using (this.ThreadValidator)
                    return this._choice.ChosenIndex;
            }
            set
            {
                using (this.ThreadValidator)
                {
                    string error;
                    this._choice.ChosenIndex = this._choice.ValidateIndex(value, out error) ? value : throw new ArgumentException(error);
                }
            }
        }

        public int DefaultIndex
        {
            get
            {
                using (this.ThreadValidator)
                    return this._choice.DefaultIndex;
            }
            set
            {
                using (this.ThreadValidator)
                    this._choice.DefaultIndex = value;
            }
        }

        object IValueRange.Value
        {
            get
            {
                using (this.ThreadValidator)
                    return AssemblyLoadResult.UnwrapObject(((IUIValueRange)this._choice).ObjectValue);
            }
        }

        object IUIValueRange.ObjectValue => ((IUIValueRange)this._choice).ObjectValue;

        IList IUIChoice.Options
        {
            get => this._choice.Options;
            set => this._choice.Options = value;
        }

        object IUIChoice.ChosenValue => this._choice.ChosenValue;

        bool IUIChoice.ValidateIndex(int index, out string error) => this._choice.ValidateIndex(index, out error);

        bool IUIChoice.ValidateOption(object value, out int index, out string error) => this._choice.ValidateOption(value, out index, out error);

        bool IUIChoice.ValidateOptionsList(IList value, out string error) => this._choice.ValidateOptionsList(value, out error);

        public bool HasNextValue
        {
            get
            {
                using (this.ThreadValidator)
                    return this._choice.HasNextValue;
            }
        }

        public bool HasSelection
        {
            get
            {
                using (this.ThreadValidator)
                    return this._choice.HasSelection;
            }
        }

        public bool HasPreviousValue
        {
            get
            {
                using (this.ThreadValidator)
                    return this._choice.HasPreviousValue;
            }
        }

        public bool Wrap
        {
            get
            {
                using (this.ThreadValidator)
                    return this._choice.Wrap;
            }
            set
            {
                using (this.ThreadValidator)
                    this._choice.Wrap = value;
            }
        }

        public void PreviousValue()
        {
            using (this.ThreadValidator)
                this._choice.PreviousValue();
        }

        public void PreviousValue(bool wrap)
        {
            using (this.ThreadValidator)
                this._choice.PreviousValue(wrap);
        }

        public void NextValue()
        {
            using (this.ThreadValidator)
                this._choice.NextValue();
        }

        public void NextValue(bool wrap)
        {
            using (this.ThreadValidator)
                this._choice.NextValue(wrap);
        }

        public void Clear()
        {
            using (this.ThreadValidator)
                this._choice.Clear();
        }

        public void DefaultValue()
        {
            using (this.ThreadValidator)
                this._choice.DefaultValue();
        }

        protected virtual void OnChosenChanged()
        {
        }

        public event EventHandler ChosenChanged
        {
            add
            {
                using (this.ThreadValidator)
                    this.AddEventHandler(Choice.s_chosenChangedEvent, value);
            }
            remove
            {
                using (this.ThreadValidator)
                    this.RemoveEventHandler(Choice.s_chosenChangedEvent, value);
            }
        }

        private void Initialize()
        {
            this._choice = this.CreateInternalChoice();
            this._choice.DeclareOwner(this);
            Vector<Listener> listeners = new Vector<Listener>(9);
            DelegateListener.OnNotifyCallback callback = new DelegateListener.OnNotifyCallback(this.OnInternalChoicePropertyChanged);
            listeners.Add(new DelegateListener(_choice, NotificationID.Options, callback));
            listeners.Add(new DelegateListener(_choice, NotificationID.DefaultIndex, callback));
            listeners.Add(new DelegateListener(_choice, NotificationID.ChosenIndex, callback));
            listeners.Add(new DelegateListener(_choice, NotificationID.ChosenValue, new DelegateListener.OnNotifyCallback(this.OnChosenValueChanged)));
            listeners.Add(new DelegateListener(_choice, NotificationID.Value, callback));
            listeners.Add(new DelegateListener(_choice, NotificationID.HasSelection, callback));
            listeners.Add(new DelegateListener(_choice, NotificationID.Wrap, callback));
            listeners.Add(new DelegateListener(_choice, NotificationID.HasPreviousValue, callback));
            listeners.Add(new DelegateListener(_choice, NotificationID.HasNextValue, callback));
            this._listeners = new CodeListeners(listeners);
            this._listeners.DeclareOwner(this);
        }

        protected override void OnPropertyChanged(string property)
        {
            base.OnPropertyChanged(property);
            this._notifier.FireThreadSafe(property);
        }

        private void OnInternalChoicePropertyChanged(DelegateListener listener) => this.FirePropertyChanged(listener.Watch);

        private void OnChosenValueChanged(DelegateListener listener)
        {
            if (this._lastChosen != null)
                this._lastChosen.Selected = false;
            this._lastChosen = this.ChosenValue as ModelItem;
            if (this._lastChosen != null)
                this._lastChosen.Selected = true;
            this.FireChangedChosenEvent();
            this.OnInternalChoicePropertyChanged(listener);
        }

        internal virtual Microsoft.Iris.ModelItems.Choice CreateInternalChoice() => new Microsoft.Iris.ModelItems.Choice();

        private void ValidateOptionsList(IList potentialOptionsWrapped, IList potentialOptions)
        {
            using (this.ThreadValidator)
            {
                string error;
                if (!this._choice.ValidateOptionsList(potentialOptionsWrapped, out error))
                    throw new ArgumentException(error);
                this.ValidateOptionsListWorker(potentialOptions);
            }
        }

        protected virtual void ValidateOptionsListWorker(IList potentialOptions)
        {
        }

        private void FireChangedChosenEvent()
        {
            if (this.GetEventHandler(Choice.s_chosenChangedEvent) is EventHandler eventHandler)
                eventHandler(this, EventArgs.Empty);
            this.OnChosenChanged();
        }

        private void SetChosenSelected(bool selectedFlag)
        {
            if (!(this.ChosenValue is ModelItem chosenValue))
                return;
            chosenValue.Selected = selectedFlag;
        }

        void IDisposableObject.DeclareOwner(object owner)
        {
        }

        void IDisposableObject.TransferOwnership(object owner)
        {
        }

        void IDisposableObject.Dispose(object owner) => this.Dispose();

        void INotifyObject.AddListener(Listener listener) => this._notifier.AddListener(listener);
    }
}
