// Decompiled with JetBrains decompiler
// Type: ZuneUI.CreditCardNumberDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Text.RegularExpressions;

namespace ZuneUI
{
    public class CreditCardNumberDescriptor : PropertyDescriptor
    {
        private bool _allowSeperators;
        private static Regex s_numbersWithSeperatorsRegex = new Regex("^\\d(\\d*(-)*( )*)*$", RegexOptions.IgnoreCase);
        private static Regex s_numbersRegex = new Regex("^\\d+$", RegexOptions.IgnoreCase);

        public CreditCardNumberDescriptor(
          string name,
          string multiValueString,
          string unknownString,
          bool required,
          bool allowSeperators)
          : base(name, multiValueString, unknownString, required)
        {
            this._allowSeperators = allowSeperators;
        }

        public override string ConvertToString(object value) => value == null ? string.Empty : value as string;

        public override object ConvertFromString(string value) => value == null ? (object)string.Empty : (object)this.RemoveSeperators(value);

        public override bool IsValidInternal(string value)
        {
            if (string.IsNullOrEmpty(value))
                return !this.Required;
            return this._allowSeperators ? CreditCardNumberDescriptor.s_numbersWithSeperatorsRegex.IsMatch(value) : CreditCardNumberDescriptor.s_numbersRegex.IsMatch(value);
        }

        private string RemoveSeperators(string value)
        {
            if (this._allowSeperators)
            {
                value = value.Replace("-", "");
                value = value.Replace(" ", "");
            }
            return value;
        }
    }
}
