// Decompiled with JetBrains decompiler
// Type: ZuneUI.MetadataEditProperty
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.ErrorMapperApi;

namespace ZuneUI
{
    public class MetadataEditProperty : INotifyPropertyChangedImpl
    {
        private PropertyDescriptor _descriptor;
        private string _originalValue;
        private string _currentValue;
        private bool _modified;
        private bool _valid = true;
        private object _state;
        private HRESULT _externalError = HRESULT._S_OK;

        internal MetadataEditProperty(PropertyDescriptor descriptor, string originalValue)
        {
            this._descriptor = descriptor;
            this._originalValue = originalValue;
            this._currentValue = originalValue;
            this.UpdateValidAndModified();
        }

        internal void SetValue(string value, bool propagate)
        {
            if (!(this._currentValue != value))
                return;
            this._currentValue = value;
            this.NotifyPropertyChanged("Value", propagate);
            this.UpdateValidAndModified();
        }

        public string OriginalValue
        {
            get => this._originalValue;
            set
            {
                if (this._originalValue != value)
                {
                    this._originalValue = value;
                    this.NotifyPropertyChanged(nameof(OriginalValue), true);
                }
                this.Value = this._originalValue;
            }
        }

        public string Value
        {
            get => this._currentValue;
            set => this.SetValue(value, true);
        }

        public bool Valid
        {
            get => this._valid;
            internal set
            {
                if (this._valid == value)
                    return;
                this._valid = value;
                this.NotifyPropertyChanged(nameof(Valid), true);
                this.NotifyPropertyChanged("ErrorMessage", true);
            }
        }

        public bool Required => this._descriptor.IsRequired(this._state);

        public bool Modified
        {
            get => this._modified;
            internal set
            {
                if (this._modified == value)
                    return;
                this._modified = value;
                this.NotifyPropertyChanged(nameof(Modified), true);
            }
        }

        public PropertyDescriptor Descriptor => this._descriptor;

        public HRESULT ExternalError
        {
            get => this._externalError;
            set
            {
                if (this._externalError.Int == value.Int)
                    return;
                this._externalError = value;
                this.NotifyPropertyChanged(nameof(ExternalError), true);
                this.UpdateValid();
            }
        }

        public string ErrorMessage
        {
            get
            {
                string str = null;
                if (this._externalError.IsError)
                {
                    ErrorMapperResult descriptionAndUrl = ErrorMapperApi.GetMappedErrorDescriptionAndUrl(this._externalError.Int);
                    if (descriptionAndUrl != null)
                        str = descriptionAndUrl.Description;
                }
                else if (!this.Valid)
                    str = this._descriptor.DefaultError;
                return str;
            }
        }

        public object State
        {
            get => this._state;
            set
            {
                if (this._state == value)
                    return;
                this._state = value;
                this.NotifyPropertyChanged(nameof(State), true);
                this.OnStateChanged();
            }
        }

        public string OverlayContent => this._descriptor.GetOverlayString(this._state);

        public string LabelContent => this._descriptor.GetLabelString(this._state);

        public object ConvertToData() => this._descriptor.ConvertFromString(this._currentValue, this._state);

        public void ConvertFromData(object data) => this.OriginalValue = this._descriptor.ConvertToString(data, this._state);

        private void OnStateChanged()
        {
            this.UpdateValid();
            this.NotifyPropertyChanged("OverlayContent", true);
            this.NotifyPropertyChanged("LabelContent", true);
        }

        private void UpdateValidAndModified()
        {
            this.Modified = this._currentValue != this._originalValue;
            if (this.Modified && this._externalError.IsError)
                this._externalError = HRESULT._S_OK;
            this.UpdateValid();
        }

        private void UpdateValid() => this.Valid = this._descriptor.IsValid(this._currentValue, this._state) && this._externalError.IsSuccess;
    }
}
