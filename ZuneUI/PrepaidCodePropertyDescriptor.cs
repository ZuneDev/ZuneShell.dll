// Decompiled with JetBrains decompiler
// Type: ZuneUI.PrepaidCodePropertyDescriptor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Text;

namespace ZuneUI
{
    public class PrepaidCodePropertyDescriptor : PropertyDescriptor
    {
        private static int s_subLength = 5;
        private static int s_subCount = 5;
        private static int s_length = s_subLength * s_subCount;
        private static int s_lengthWithDelimiters = s_length + s_subCount - 1;
        private static string s_delimiter = "-";

        public PrepaidCodePropertyDescriptor(
          string name,
          string multiValueString,
          string unknownString,
          bool required)
          : base(name, multiValueString, unknownString, s_lengthWithDelimiters, required)
        {
        }

        public override bool IsValidInternal(string value)
        {
            bool flag = false;
            if (value != null)
                flag = value.Replace(s_delimiter, "").Length == s_length;
            return flag;
        }

        public override object ConvertFromString(string value)
        {
            string str = string.Empty;
            if (value != null)
                str = AddDelimiters(value);
            return str;
        }

        public override string ConvertToString(object value) => AddDelimiters(value as string);

        public static string AddDelimiters(string value)
        {
            StringBuilder stringBuilder = new StringBuilder(s_length);
            if (value != null)
            {
                int num = 0;
                for (int index = 0; index < value.Length; ++index)
                {
                    if (char.IsLetterOrDigit(value[index]))
                    {
                        if (num % s_subLength == 0 && num != 0)
                            stringBuilder.Append(s_delimiter);
                        stringBuilder.Append(value[index]);
                        ++num;
                    }
                }
            }
            return stringBuilder.ToString().ToUpper();
        }
    }
}
