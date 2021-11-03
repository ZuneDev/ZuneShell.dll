// Decompiled with JetBrains decompiler
// Type: ZuneUI.StringCaseInsensitiveComparer
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Collections.Generic;

namespace ZuneUI
{
    internal class StringCaseInsensitiveComparer : IComparer<string>
    {
        private static StringCaseInsensitiveComparer s_instance;

        public static IComparer<string> Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new StringCaseInsensitiveComparer();
                return s_instance;
            }
        }

        private StringCaseInsensitiveComparer()
        {
        }

        int IComparer<string>.Compare(string x, string y) => string.Compare(x, y, StringComparison.CurrentCultureIgnoreCase);
    }
}
