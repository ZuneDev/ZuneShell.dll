// Decompiled with JetBrains decompiler
// Type: ZuneUI.ProfileMultiLinePropertyDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class ProfileMultiLinePropertyDescriptor : ProfilePropertyDescriptor
    {
        private int _maxTrimmedLength;

        public ProfileMultiLinePropertyDescriptor(
          string name,
          string serviceName,
          StringId displayNameId,
          int maxTrimmedLength)
          : base(name, serviceName, displayNameId, maxTrimmedLength * 2)
        {
            this._maxTrimmedLength = maxTrimmedLength;
        }

        public int MaxTrimmedLength => this._maxTrimmedLength;

        public override string GetServiceValue(string value)
        {
            if (value != null)
                value = value.Replace("\r\n", "\n");
            return value;
        }

        public override bool IsValidInternal(string value)
        {
            value = this.GetServiceValue(value);
            return (value == null || this._maxTrimmedLength >= value.Length) && base.IsValidInternal(value);
        }

        public override object ConvertFromString(string value) => this.GetServiceValue(base.ConvertFromString(value) as string);
    }
}
