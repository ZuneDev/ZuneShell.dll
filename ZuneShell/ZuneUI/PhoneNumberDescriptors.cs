// Decompiled with JetBrains decompiler
// Type: ZuneUI.PhoneNumberDescriptors
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Text;

namespace ZuneUI
{
    public class PhoneNumberDescriptors : CountryFieldValidationPropertyDescriptor
    {
        public PhoneNumberDescriptors(string name, CountryFieldValidatorType type)
          : base(name, type)
        {
        }

        public bool Split(string value, out string areaCode, out string number)
        {
            value = RemoveNonDigits(value);
            if (value.Length >= 10)
            {
                number = value.Substring(value.Length - 7, 7);
                areaCode = value.Substring(value.Length - 10, 3);
                return true;
            }
            areaCode = string.Empty;
            number = string.Empty;
            return false;
        }

        public string Combine(string areaCode, string number)
        {
            string str = null;
            if (!string.IsNullOrEmpty(number))
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (!string.IsNullOrEmpty(areaCode))
                {
                    stringBuilder.Append(areaCode);
                    stringBuilder.Append('-');
                }
                if (number.Length >= 7)
                {
                    stringBuilder.Append(number.Substring(0, 3));
                    stringBuilder.Append('-');
                    stringBuilder.Append(number.Substring(3));
                }
                else
                    stringBuilder.Append(number);
                str = stringBuilder.ToString();
            }
            return str;
        }

        private static string RemoveNonDigits(string value)
        {
            if (value == null)
                return string.Empty;
            StringBuilder stringBuilder = new StringBuilder(value.Length);
            for (int index = 0; index < value.Length; ++index)
            {
                if (char.IsDigit(value[index]))
                    stringBuilder.Append(value[index]);
            }
            return stringBuilder.ToString();
        }
    }
}
