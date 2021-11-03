// Decompiled with JetBrains decompiler
// Type: ZuneUI.EmailPropertyDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class EmailPropertyDescriptor : PropertyDescriptor
    {
        public EmailPropertyDescriptor(
          string name,
          string multiValueString,
          string unknownString,
          bool required)
          : base(name, multiValueString, unknownString, required)
        {
        }

        public override bool IsValidInternal(string value) => string.IsNullOrEmpty(value) && !this.Required || EmailHelper.IsValid(value);
    }
}
