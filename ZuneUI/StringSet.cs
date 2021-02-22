// Decompiled with JetBrains decompiler
// Type: ZuneUI.StringSet
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class StringSet
    {
        private SortedDictionary<string, object> _set = new SortedDictionary<string, object>();

        public void Add(string s) => this._set[s] = (object)null;

        public void Clear() => this._set.Clear();

        public IList ToList()
        {
            List<string> stringList = new List<string>(this._set.Count);
            foreach (KeyValuePair<string, object> keyValuePair in this._set)
                stringList.Add(keyValuePair.Key);
            return (IList)stringList;
        }
    }
}
