// Decompiled with JetBrains decompiler
// Type: ZuneUI.DrmStateMask
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public static class DrmStateMask
    {
        private static readonly long _drmStateMaskAll = 1100653528065;
        private static readonly long _drmStateMaskProtected = 1073742848;
        private static readonly long _drmStateMaskZunePass = 68157440;
        private static readonly long _drmStateMaskPersonal = 1099511627776;
        private static readonly long _drmStateMaskUnknown = 1;

        public static bool Match(long drmStateMask1, long drmStateMask2)
        {
            if (drmStateMask1 == 0L)
                drmStateMask1 = _drmStateMaskUnknown;
            if (drmStateMask2 == 0L)
                drmStateMask2 = _drmStateMaskUnknown;
            return (drmStateMask1 & drmStateMask2) != 0L;
        }

        public static bool IsMixed(long drmStateMask)
        {
            int num = 0;
            if ((drmStateMask & _drmStateMaskPersonal) != 0L)
                ++num;
            if ((drmStateMask & _drmStateMaskProtected) != 0L)
                ++num;
            if ((drmStateMask & _drmStateMaskZunePass) != 0L)
                ++num;
            return num > 1;
        }

        public static long Combine(long drmStateMask1, long drmStateMask2) => drmStateMask1 | drmStateMask2;

        public static long Diff(long drmStateMask1, long drmStateMask2) => drmStateMask1 ^ drmStateMask1 & drmStateMask2;

        public static long All() => _drmStateMaskAll;

        public static long Unknown() => _drmStateMaskUnknown;

        public static long Personal() => _drmStateMaskPersonal;

        public static long Protected() => _drmStateMaskProtected;

        public static long ZunePass() => _drmStateMaskZunePass;
    }
}
