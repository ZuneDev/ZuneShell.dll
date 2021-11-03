// Decompiled with JetBrains decompiler
// Type: ZuneUI.TimeSpanHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public static class TimeSpanHelper
    {
        public static bool IsEmpty(TimeSpan span) => span == TimeSpan.Zero;

        public static TimeSpan Empty => TimeSpan.Zero;
    }
}
