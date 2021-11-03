// Decompiled with JetBrains decompiler
// Type: ZuneUI.ZuneTagPropertyDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class ZuneTagPropertyDescriptor : PropertyDescriptor
    {
        public ZuneTagPropertyDescriptor(string name, string multiValueString, string unknownString)
          : base(name, multiValueString, unknownString, ZuneTagHelper.MaxLength, true)
        {
        }

        public override bool IsValidInternal(string value) => ZuneTagHelper.IsValid(value);
    }
}
