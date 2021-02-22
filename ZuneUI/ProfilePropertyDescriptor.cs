// Decompiled with JetBrains decompiler
// Type: ZuneUI.ProfilePropertyDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class ProfilePropertyDescriptor : PropertyDescriptor
    {
        private string _serviceName;
        private StringId _displayNameId;

        public ProfilePropertyDescriptor(
          string name,
          string serviceName,
          StringId displayNameId,
          int maxTextLength)
          : base(name, string.Empty, string.Empty, maxTextLength)
        {
            this._serviceName = serviceName;
            this._displayNameId = displayNameId;
        }

        public string ServiceName => this._serviceName;

        public StringId DisplayNameId => this._displayNameId;

        public virtual string GetServiceValue(string value) => value;
    }
}
