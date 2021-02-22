// Decompiled with JetBrains decompiler
// Type: ZuneUI.PropertyDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class PropertyDescriptor
    {
        private string _name;
        private string _unknownString;
        private string _multiValueString;
        private int _maxTextLength;
        private bool _required;
        private object _defaultValue;

        public PropertyDescriptor(
          string name,
          string multiValueString,
          string unknownString,
          int maxTextLength,
          bool required,
          object defaultValue)
        {
            this._name = name;
            this._unknownString = unknownString;
            this._multiValueString = multiValueString;
            this._maxTextLength = maxTextLength;
            this._required = required;
            this._defaultValue = defaultValue;
        }

        public PropertyDescriptor(
          string name,
          string multiValueString,
          string unknownString,
          int maxTextLength,
          bool required)
          : this(name, multiValueString, unknownString, maxTextLength, required, (object)null)
        {
        }

        public PropertyDescriptor(
          string name,
          string multiValueString,
          string unknownString,
          int maxTextLength)
          : this(name, multiValueString, unknownString, maxTextLength, false, (object)null)
        {
        }

        public PropertyDescriptor(string name, string multiValueString, string unknownString)
          : this(name, multiValueString, unknownString, 1000, false, (object)null)
        {
        }

        public PropertyDescriptor(
          string name,
          string multiValueString,
          string unknownString,
          bool required)
          : this(name, multiValueString, unknownString, 1000, required, (object)null)
        {
            this._required = required;
        }

        public virtual string ConvertToString(object value, object state) => this.ConvertToString(value);

        public virtual string ConvertToString(object value) => value?.ToString();

        public virtual object ConvertFromString(string value, object state) => this.ConvertFromString(value);

        public virtual object ConvertFromString(string value) => value != null ? (object)value.Trim() : (object)null;

        public bool IsValid(string value, object state)
        {
            string str = value;
            if (str != null)
                str = str.Trim();
            return (!this._required || !string.IsNullOrEmpty(str) && !(str == this._unknownString)) && (str == null || str.Length <= this._maxTextLength) && this.IsValidInternal(str, state);
        }

        public virtual bool IsValidInternal(string value, object state) => this.IsValidInternal(value);

        public virtual bool IsValidInternal(string value) => true;

        public bool IsRequired(object state) => this.IsRequiredInternal(state);

        public virtual bool IsRequiredInternal(object state) => this.IsRequiredInternal();

        public virtual bool IsRequiredInternal() => this._required;

        public bool Required => this.IsRequiredInternal();

        internal virtual string GetOverlayString(object state) => string.Empty;

        internal virtual string GetLabelString(object state) => string.Empty;

        public object DefaultValue
        {
            get => this._defaultValue;
            protected set => this._defaultValue = value;
        }

        public virtual string DefaultError => (string)null;

        public string DescriptorName => this._name;

        public string UnknownString => this._unknownString;

        public string MultiValueString => this._multiValueString;

        public int MaxTextLength => this._maxTextLength;
    }
}
