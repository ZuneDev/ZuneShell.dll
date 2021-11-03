// Decompiled with JetBrains decompiler
// Type: ZuneUI.LanguagePropertyDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Globalization;

namespace ZuneUI
{
    public class LanguagePropertyDescriptor : PropertyDescriptor
    {
        public LanguagePropertyDescriptor(
          string name,
          string multiValueString,
          string unknownString,
          bool required)
          : base(name, multiValueString, unknownString, 1000, required, CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
        {
            this.DefaultValue = CultureHelper.GetDefaultLanguage();
        }

        public override object ConvertFromString(string value) => LanguageHelper.GetAbbreviation(value);

        public override string ConvertToString(object value) => LanguageHelper.GetDisplayName(value as string);
    }
}
