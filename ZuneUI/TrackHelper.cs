// Decompiled with JetBrains decompiler
// Type: ZuneUI.TrackHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using ZuneXml;

namespace ZuneUI
{
    public static class TrackHelper
    {
        public static void SetOrdinal(DataProviderObject item, int ordinal)
        {
            if (!(item is Track))
                return;
            ((Track)item).Ordinal = ordinal;
        }
    }
}
