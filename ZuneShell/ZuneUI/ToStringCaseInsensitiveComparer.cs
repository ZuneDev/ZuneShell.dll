// Decompiled with JetBrains decompiler
// Type: ZuneUI.ToStringCaseInsensitiveComparer
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Collections;

namespace ZuneUI
{
    internal class ToStringCaseInsensitiveComparer : IComparer
    {
        private static ToStringCaseInsensitiveComparer s_instance;

        public static IComparer Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new ToStringCaseInsensitiveComparer();
                return s_instance;
            }
        }

        private ToStringCaseInsensitiveComparer()
        {
        }

        int IComparer.Compare(object x, object y) => string.Compare(x.ToString(), y.ToString(), StringComparison.CurrentCultureIgnoreCase);
    }
}
