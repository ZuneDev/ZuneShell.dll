// Decompiled with JetBrains decompiler
// Type: ZuneUI.LanguageNameComparer
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Collections.Generic;

namespace ZuneUI
{
    internal class LanguageNameComparer : IComparer<string>
    {
        private static LanguageNameComparer s_instance;

        public static IComparer<string> Instance
        {
            get
            {
                if (LanguageNameComparer.s_instance == null)
                    LanguageNameComparer.s_instance = new LanguageNameComparer();
                return (IComparer<string>)LanguageNameComparer.s_instance;
            }
        }

        private LanguageNameComparer()
        {
        }

        int IComparer<string>.Compare(string x, string y) => string.Compare(LanguageHelper.GetDisplayName(x), LanguageHelper.GetDisplayName(y), StringComparison.CurrentCultureIgnoreCase);
    }
}
