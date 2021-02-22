// Decompiled with JetBrains decompiler
// Type: ZuneUI.Enum
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public static class Enum
    {
        public static string GetDescription(System.Enum value)
        {
            object[] customAttributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return customAttributes == null || customAttributes.Length <= 0 ? value.ToString() : Shell.LoadString((StringId)((DescriptionAttribute)customAttributes[0]).StringId);
        }
    }
}
