// Decompiled with JetBrains decompiler
// Type: ZuneUI.TextChopper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Collections.Generic;
using System.Text;

namespace ZuneUI
{
    public class TextChopper
    {
        private static Random _random = new Random();

        public static string Chop(string text, int minChunkSize, int maxChunkSize)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                    return text;
                if (minChunkSize < 1)
                    minChunkSize = 1;
                List<string> stringList = new List<string>();
                stringList.Add("<C>");
                StringBuilder stringBuilder = new StringBuilder(text.Length * 8);
                int num1 = 0;
                int num2 = 0;
                bool flag = false;
                while (num1 < text.Length)
                {
                    if (text[num1] == '<')
                    {
                        num2 = 0;
                        if (flag)
                        {
                            flag = false;
                            for (int count = stringList.Count; count > 0; --count)
                            {
                                string str = stringList[count - 1].Insert(1, "/");
                                stringBuilder.Append(str);
                            }
                        }
                        int num3 = text.IndexOf('>', num1);
                        if (num3 == -1)
                            throw new TextChopperException("Missing closing > in tag for string : " + text);
                        string str1 = text.Substring(num1, num3 - num1 + 1);
                        if (str1[1] == '/')
                            stringList.RemoveAt(stringList.Count - 1);
                        else
                            stringList.Add(str1);
                        num1 += str1.Length;
                    }
                    else
                    {
                        if (num2 == 0)
                        {
                            num2 = Math.Min(maxChunkSize <= minChunkSize ? minChunkSize : _random.Next(minChunkSize, maxChunkSize), text.Length - num1);
                            foreach (string str in stringList)
                                stringBuilder.Append(str);
                            flag = true;
                        }
                        string str1;
                        int num3;
                        if (text[num1] == '&')
                        {
                            int num4 = text.IndexOf(';', num1);
                            if (num4 == -1)
                                throw new TextChopperException("Missing closing ; in escaped text for string : " + text);
                            int length = num4 - num1 + 1;
                            str1 = text.Substring(num1, length);
                            num3 = length;
                        }
                        else
                        {
                            str1 = text[num1].ToString();
                            num3 = 1;
                        }
                        stringBuilder.Append(str1);
                        num1 += num3;
                        --num2;
                        if (num2 == 0 || num1 == text.Length)
                        {
                            for (int count = stringList.Count; count > 0; --count)
                            {
                                string str2 = stringList[count - 1].Insert(1, "/");
                                stringBuilder.Append(str2);
                            }
                            flag = false;
                        }
                    }
                }
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                return text;
            }
        }

        public class TextChopperException : Exception
        {
            public TextChopperException(string message)
              : base(message)
            {
            }
        }
    }
}
