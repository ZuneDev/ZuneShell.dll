// Decompiled with JetBrains decompiler
// Type: Microsoft.Zune.PerfTrace.PerfTrace
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace Microsoft.Zune.PerfTrace
{
    public sealed class PerfTrace
    {
        private static readonly EtwTraceProvider _EventProvider;
        internal static readonly Guid ZUNE_ETW_CONTROL_GUID = new Guid(1496399467U, (ushort)53017, (ushort)20072, (byte)142, (byte)243, (byte)135, (byte)107, (byte)12, (byte)8, (byte)232, (byte)1);
        internal static readonly Guid PERFTRACE_LAUNCHEVENT_GUID = new Guid(1815200785, (short)23272, (short)18087, (byte)190, (byte)166, (byte)76, (byte)236, (byte)117, (byte)51, (byte)196, (byte)218);
        internal static readonly Guid PerftraceUICollectionGuid = new Guid(235915657U, (ushort)37778, (ushort)18043, (byte)146, (byte)252, (byte)128, (byte)212, (byte)193, (byte)153, (byte)154, (byte)151);

        private PerfTrace()
        {
        }

        static PerfTrace() => Microsoft.Zune.PerfTrace.PerfTrace._EventProvider = new EtwTraceProvider(Microsoft.Zune.PerfTrace.PerfTrace.ZUNE_ETW_CONTROL_GUID, "SOFTWARE\\Microsoft\\Zune");

        internal static bool IsEnabled(
          EtwTraceProvider provider,
          Microsoft.Zune.PerfTrace.PerfTrace.Flags flag,
          Microsoft.Zune.PerfTrace.PerfTrace.Level level)
        {
            return (uint)level <= (uint)provider.Level && provider.IsEnabled && (flag & (Microsoft.Zune.PerfTrace.PerfTrace.Flags)provider.Flags) > ~Microsoft.Zune.PerfTrace.PerfTrace.Flags.All;
        }

        internal static bool IsEnabled(Microsoft.Zune.PerfTrace.PerfTrace.Flags flags, Microsoft.Zune.PerfTrace.PerfTrace.Level level) => Microsoft.Zune.PerfTrace.PerfTrace.IsEnabled(Microsoft.Zune.PerfTrace.PerfTrace._EventProvider, flags, level);

        internal static bool IsEnabled(Microsoft.Zune.PerfTrace.PerfTrace.Flags flags) => Microsoft.Zune.PerfTrace.PerfTrace.IsEnabled(Microsoft.Zune.PerfTrace.PerfTrace._EventProvider, flags, Microsoft.Zune.PerfTrace.PerfTrace.Level.Normal);

        public static void PERFTRACE_LAUNCHEVENT(Microsoft.Zune.PerfTrace.PerfTrace.LAUNCH_EVENT launchEvent, uint data)
        {
            if (!Microsoft.Zune.PerfTrace.PerfTrace.IsEnabled(Microsoft.Zune.PerfTrace.PerfTrace.Flags.Launch, Microsoft.Zune.PerfTrace.PerfTrace.Level.Normal))
                return;
            Microsoft.Zune.PerfTrace.PerfTrace._EventProvider.TraceEvent((byte)4, Microsoft.Zune.PerfTrace.PerfTrace.PERFTRACE_LAUNCHEVENT_GUID, (byte)launchEvent, (object)data);
        }

        public static void TraceUICollectionEvent(UICollectionEvent traceEvent, string eventDetail)
        {
            if (!Microsoft.Zune.PerfTrace.PerfTrace.IsEnabled(Microsoft.Zune.PerfTrace.PerfTrace.Flags.Collection, Microsoft.Zune.PerfTrace.PerfTrace.Level.Normal))
                return;
            Microsoft.Zune.PerfTrace.PerfTrace._EventProvider.TraceEvent((byte)4, Microsoft.Zune.PerfTrace.PerfTrace.PerftraceUICollectionGuid, (byte)traceEvent, (object)eventDetail);
        }

        [System.Flags]
        internal enum Flags : uint
        {
            DB_Mutex = 1,
            Launch = 2,
            QRS = 4,
            Collection = 16, // 0x00000010
            All = 4294967295, // 0xFFFFFFFF
        }

        internal enum Level : byte
        {
            Fatal = 1,
            Error = 2,
            Warning = 3,
            Normal = 4,
            Verbose = 5,
        }

        internal enum Tag
        {
            COMPONENT1 = 1129270577, // 0x434F4D31
            COMPONENT2 = 1129270578, // 0x434F4D32
        }

        public enum LAUNCH_EVENT
        {
            LAUNCHER_AT_WINMAIN = 1,
            WINMAIN_ABOUT_TO_LAUNCH = 2,
            START_PHASE2_INIT = 3,
            END_PHASE2_INIT = 4,
            ZUNENATIVELIB_STARTUP = 5,
            LAUNCHING_MANAGED_APP = 6,
            IN_MANAGED_LAUNCH = 7,
            REQUEST_UI_LOAD = 8,
            REQUEST_UI_LOAD_COMPLETE = 9,
            QUERYDB_BEGIN = 10, // 0x0000000A
            QUERYDB_END = 11, // 0x0000000B
            DEVICE_CONNECTED = 12, // 0x0000000C
        }
    }
}
