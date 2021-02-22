// Decompiled with JetBrains decompiler
// Type: ZuneUI.DocumentHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Text;

namespace ZuneUI
{
    public static class DocumentHelper
    {
        public static string SeparateParagraphs(string content)
        {
            if (content != null)
                content = content.Replace("\r\n", "\r\n\r\n");
            return content;
        }

        public static string CleanseWhitespace(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            char[] chArray = new char[4] { ' ', '\n', '\r', '\t' };
            StringBuilder stringBuilder = new StringBuilder();
            string[] strArray = str.Split(chArray);
            bool flag = false;
            foreach (string str1 in strArray)
            {
                if (flag)
                {
                    stringBuilder.Append(" ");
                    flag = false;
                }
                if (!string.IsNullOrEmpty(str1))
                {
                    stringBuilder.Append(str1);
                    flag = true;
                }
            }
            return stringBuilder.ToString();
        }
    }
}
