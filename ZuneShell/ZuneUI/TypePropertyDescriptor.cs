// Decompiled with JetBrains decompiler
// Type: ZuneUI.TypePropertyDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class TypePropertyDescriptor : PropertyDescriptor
    {
        public TypePropertyDescriptor(string name, string multiValueString, string unknownString)
          : base(name, multiValueString, unknownString)
        {
        }

        public override string ConvertToString(object value) => VideoDescriptions.GetDescription((int)value);

        public override object ConvertFromString(string value) => (int)VideoDescriptions.GetCategory(value);
    }
}
