// Decompiled with JetBrains decompiler
// Type: ZuneUI.XmlHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Security;
using System.Text.RegularExpressions;

namespace ZuneUI
{
    public static class XmlHelper
    {
        private static Regex _xmlRegex = new Regex("<[^<>]+>", RegexOptions.IgnoreCase);

        public static string Escape(string text) => SecurityElement.Escape(text);

        public static string Unescape(string text)
        {
            if (text != null)
            {
                text = text.Replace("&quot;", "\"");
                text = text.Replace("&apos;", "'");
                text = text.Replace("&amp;", "&");
                text = text.Replace("&lt;", "<");
                text = text.Replace("&gt;", ">");
            }
            return text;
        }

        public static string Strip(string text)
        {
            if (text != null)
                text = XmlHelper._xmlRegex.Replace(text, string.Empty);
            return text;
        }
    }
}
