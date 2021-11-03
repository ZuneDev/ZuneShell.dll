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
        internal static readonly Guid ZUNE_ETW_CONTROL_GUID = new Guid(1496399467U, 53017, 20072, 142, 243, 135, 107, 12, 8, 232, 1);
        internal static readonly Guid PERFTRACE_LAUNCHEVENT_GUID = new Guid(1815200785, 23272, 18087, 190, 166, 76, 236, 117, 51, 196, 218);
        internal static readonly Guid PerftraceUICollectionGuid = new Guid(235915657U, 37778, 18043, 146, 252, 128, 212, 193, 153, 154, 151);

        private PerfTrace()
        {
        }

        static PerfTrace() => _EventProvider = new EtwTraceProvider(ZUNE_ETW_CONTROL_GUID, "SOFTWARE\\Microsoft\\Zune");

        internal static bool IsEnabled(
          EtwTraceProvider provider,
          Flags flag,
          Level level)
        {
            return (uint)level <= provider.Level && provider.IsEnabled && (flag & (Flags)provider.Flags) > ~Flags.All;
        }

        internal static bool IsEnabled(Flags flags, Level level) => IsEnabled(_EventProvider, flags, level);

        internal static bool IsEnabled(Flags flags) => IsEnabled(_EventProvider, flags, Level.Normal);

        public static void PERFTRACE_LAUNCHEVENT(LAUNCH_EVENT launchEvent, uint data)
        {
            if (!IsEnabled(Flags.Launch, Level.Normal))
                return;
            _EventProvider.TraceEvent(4, PERFTRACE_LAUNCHEVENT_GUID, (byte)launchEvent, data);
        }

        public static void TraceUICollectionEvent(UICollectionEvent traceEvent, string eventDetail)
        {
            if (!IsEnabled(Flags.Collection, Level.Normal))
                return;
            _EventProvider.TraceEvent(4, PerftraceUICollectionGuid, (byte)traceEvent, eventDetail);
        }

        [Flags]
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
