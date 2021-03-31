// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ModelItems.Choice
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using System.Collections;

namespace Microsoft.Iris.ModelItems
{
    internal class Choice :
      DisposableNotifyObjectBase,
      IUIChoice,
      IUIValueRange,
      IDisposableObject,
      INotifyObject
    {
        protected IList _options;
        protected int _chosen;
        private int _default;
        private bool _wrap;
        private static int s_noSelectionSentinal = -1;

        public Choice()
        {
            this._chosen = Choice.s_noSelectionSentinal;
            this._default = 0;
        }

        protected override void OnDispose()
        {
            this.SetOptions(null, true);
            base.OnDispose();
        }

        public IList Options
        {
            get => this._options;
            set => this.SetOptions(value, false);
        }

        private void SetOptions(IList value, bool disposing)
        {
            if (this._options == value)
                return;
            if (this._options != null && this._options is INotifyList options)
                options.ContentsChanged -= new UIListContentsChangedHandler(this.OnListContentsChanged);
            this._options = value;
            if (value != null && value is INotifyList notifyList)
                notifyList.ContentsChanged += new UIListContentsChangedHandler(this.OnListContentsChanged);
            if (disposing)
                return;
            int chosen = this._chosen;
            this.Clear();
            if (this.ValidateIndex(chosen))
                this.SetChosenIndex(chosen);
            else if (!ListUtility.IsNullOrEmpty(this._options))
                this.SetChosenIndex(0);
            this.FireNotification(NotificationID.Options);
        }

        private bool ValidateIndex(int index) => this.ValidateIndex(index, out string _);

        public bool ValidateIndex(int index, out string error)
        {
            bool flag = true;
            error = null;
            if (this._options != null && (index < 0 || index >= this._options.Count))
            {
                error = string.Format("Selected Index {0} is not a valid index in SourceList of size {1}", index, _options.Count);
                flag = false;
            }
            return flag;
        }

        public bool ValidateOption(object option, out int index, out string error)
        {
            bool flag = false;
            index = -1;
            if (this._options != null)
            {
                index = this._options.IndexOf(option);
                if (index >= 0)
                    flag = true;
            }
            error = !flag ? string.Format("Script runtime failure: Invalid '{0}' value  for '{1}'", option, "ChosenValue") : null;
            return flag;
        }

        public virtual bool ValidateOptionsList(IList options, out string error)
        {
            error = null;
            return true;
        }

        public object ChosenValue => !this.HasSelection || this.OptionsCount == 0 ? null : this._options[this._chosen];

        public int ChosenIndex
        {
            get => this._chosen;
            set => this.SetChosenIndex(value);
        }

        public int DefaultIndex
        {
            get => this._default;
            set
            {
                if (this._default == value)
                    return;
                this._default = value;
                this.FireNotification(NotificationID.DefaultIndex);
            }
        }

        private void SetChosenIndex(int index)
        {
            if (this._chosen == index)
                return;
            bool hasSelection = this.HasSelection;
            bool hasPreviousValue;
            bool hasNextValue;
            this.CapturePrevNextState(out hasPreviousValue, out hasNextValue);
            this._chosen = index;
            this.FireNotification(NotificationID.ChosenIndex);
            this.FireNotification(NotificationID.ChosenValue);
            this.FireNotification(NotificationID.Value);
            if (this.HasSelection != hasSelection)
                this.FireNotification(NotificationID.HasSelection);
            this.FirePrevNextNotifications(hasPreviousValue, hasNextValue);
        }

        public bool HasSelection => this._chosen != Choice.s_noSelectionSentinal && this._options != null;

        public bool HasPreviousValue => this.HasPreviousValueWorker(this._wrap);

        public bool HasPreviousValueWorker(bool wrap)
        {
            if (!this.HasSelection)
                return false;
            return wrap || this._chosen > 0;
        }

        public bool HasNextValue => this.HasNextValueWorker(this._wrap);

        public bool HasNextValueWorker(bool wrap)
        {
            if (!this.HasSelection)
                return false;
            return wrap || this._chosen < this.OptionsCount - 1;
        }

        public bool Wrap
        {
            get => this._wrap;
            set
            {
                if (this._wrap == value)
                    return;
                bool hasPreviousValue;
                bool hasNextValue;
                this.CapturePrevNextState(out hasPreviousValue, out hasNextValue);
                this._wrap = value;
                this.FireNotification(NotificationID.Wrap);
                this.FirePrevNextNotifications(hasPreviousValue, hasNextValue);
            }
        }

        private void CapturePrevNextState(out bool hasPreviousValue, out bool hasNextValue)
        {
            hasPreviousValue = this.HasPreviousValue;
            hasNextValue = this.HasNextValue;
        }

        private void FirePrevNextNotifications(bool hadPreviousValue, bool hadNextValue)
        {
            if (hadPreviousValue != this.HasPreviousValue)
                this.FireNotification(NotificationID.HasPreviousValue);
            if (hadNextValue == this.HasNextValue)
                return;
            this.FireNotification(NotificationID.HasNextValue);
        }

        private int OptionsCount
        {
            get
            {
                int num = 0;
                if (this._options != null)
                    num = this._options.Count;
                return num;
            }
        }

        public void PreviousValue() => this.PreviousValue(this._wrap);

        public void PreviousValue(bool wrap)
        {
            if (!this.HasPreviousValueWorker(wrap))
                return;
            int index = this._chosen - 1;
            if (index < 0)
                index = this.OptionsCount - 1;
            this.SetChosenIndex(index);
        }

        public void NextValue() => this.NextValue(this._wrap);

        public void NextValue(bool wrap)
        {
            if (!this.HasNextValueWorker(wrap))
                return;
            int index = this._chosen + 1;
            if (index >= this.OptionsCount)
                index = 0;
            this.SetChosenIndex(index);
        }

        public void DefaultValue()
        {
            if (this._default < 0 || this._default >= this.OptionsCount)
                return;
            this.SetChosenIndex(this._default);
        }

        object IUIValueRange.ObjectValue => this.ChosenValue;

        public void Clear() => this.SetChosenIndex(Choice.s_noSelectionSentinal);

        private void OnListContentsChanged(IList senderList, UIListContentsChangedArgs args)
        {
            UIListContentsChangeType type = args.Type;
            int oldIndex = args.OldIndex;
            int newIndex = args.NewIndex;
            int count = args.Count;
            switch (type)
            {
                case UIListContentsChangeType.Add:
                case UIListContentsChangeType.AddRange:
                case UIListContentsChangeType.Insert:
                case UIListContentsChangeType.InsertRange:
                    if (this._chosen < newIndex)
                        break;
                    this.SetChosenIndex(this._chosen + count);
                    break;
                case UIListContentsChangeType.Remove:
                    if (oldIndex == this._chosen)
                    {
                        this.Clear();
                        break;
                    }
                    if (this._chosen <= oldIndex)
                        break;
                    this.SetChosenIndex(this._chosen - 1);
                    break;
                case UIListContentsChangeType.Move:
                    if (oldIndex == newIndex || this._chosen < oldIndex)
                        break;
                    if (this._chosen == oldIndex)
                    {
                        this.SetChosenIndex(newIndex);
                        break;
                    }
                    if (this._chosen >= newIndex)
                        break;
                    this.SetChosenIndex(newIndex - 1);
                    break;
                case UIListContentsChangeType.Clear:
                case UIListContentsChangeType.Reset:
                    this.Clear();
                    break;
            }
        }
    }
}
