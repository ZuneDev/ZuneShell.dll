// Decompiled with JetBrains decompiler
// Type: ZuneUI.TrackOptionsComparer
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Collections;

namespace ZuneUI
{
    public class TrackOptionsComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            TrackOptionGroupItem trackOptionGroupItem1 = x as TrackOptionGroupItem;
            TrackOptionGroupItem trackOptionGroupItem2 = y as TrackOptionGroupItem;
            return trackOptionGroupItem1 != null && trackOptionGroupItem2 != null ? trackOptionGroupItem1.Original.CompareTo((object)trackOptionGroupItem2.Original) : 1;
        }
    }
}
