// Decompiled with JetBrains decompiler
// Type: ZuneUI.SearchableArrayList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class SearchableArrayList : ArrayListDataSet, ISearchableList
    {
        public int SearchForString(string str)
        {
            int num = -1;
            if (this.Source is ISearchableList)
                num = ((ISearchableList)this.Source).SearchForString(str);
            else if (this.Source is ArrayList)
            {
                num = ((ArrayList)this.Source).BinarySearch(str, ToStringCaseInsensitiveComparer.Instance);
                if (num < 0)
                    num = ~num;
            }
            else if (this.Source is Array)
            {
                num = Array.BinarySearch((Array)this.Source, str, ToStringCaseInsensitiveComparer.Instance);
                if (num < 0)
                    num = ~num;
            }
            else if (this.Source is List<string>)
            {
                num = ((List<string>)this.Source).BinarySearch(str, StringCaseInsensitiveComparer.Instance);
                if (num < 0)
                    num = ~num;
            }
            return num;
        }
    }
}
