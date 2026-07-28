// Decompiled with JetBrains decompiler
// Type: Microsoft.Zune.PerfTrace.EtwTraceProvider
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll
//
// Reimplemented on top of System.Diagnostics.Tracing.EventSource instead of
// the original advapi32 classic-ETW P/Invokes (RegisterTraceGuidsW,
// UnregisterTraceGuids, GetTraceEnableFlags, GetTraceEnableLevel, TraceEvent)
// so this type works cross-platform. See
// logs/Microsoft.Zune/PerfTrace/EtwTraceProvider.md for the rationale and the
// resulting behavior changes.

using Microsoft.Win32;
using System;
using System.Diagnostics.Tracing;
using System.Globalization;
using Microsoft.Iris.Data.Registry;

namespace Microsoft.Zune.PerfTrace
{
    internal class EtwTraceProvider
    {
        private readonly EtwEventSource? _eventSource;

        internal EtwTraceProvider(Guid controlGuid, string regPath)
        {
            using var registryKey = RegistryProviderFactory.TryOpen(RegistryHive.LocalMachine, regPath);
            var etwEnabled = registryKey?.GetIntValue("EtwEnabled", 1) ?? 1;
            if (etwEnabled > 0)
                _eventSource = new EtwEventSource(controlGuid);
        }

        internal bool IsEnabled => _eventSource?.IsEnabled() ?? false;

        internal bool IsLevelEnabled(byte level, uint flags) =>
            _eventSource is not null && _eventSource.IsLevelEnabled(level, flags);

        internal void TraceEvent(byte level, Guid eventGuid, byte eventType) => TraceEvent(level, eventGuid, eventType, null, null);

        internal void TraceEvent(byte level, Guid eventGuid, byte eventType, object data0) => TraceEvent(level, eventGuid, eventType, data0, null);

        internal void TraceEvent(
          byte level,
          Guid eventGuid,
          byte eventType,
          object data0,
          object data1)
        {
            int num = (int)TraceEvent(level, eventGuid, eventType, data0, data1, null, null, null, null, null, null, null);
        }

        internal void TraceEvent(
          byte level,
          Guid eventGuid,
          byte eventType,
          object data0,
          object data1,
          object data2)
        {
            int num = (int)TraceEvent(level, eventGuid, eventType, data0, data1, data2, null, null, null, null, null, null);
        }

        internal void TraceEvent(
          byte level,
          Guid eventGuid,
          byte eventType,
          object data0,
          object data1,
          object data2,
          object data3)
        {
            int num = (int)TraceEvent(level, eventGuid, eventType, data0, data1, data2, data3, null, null, null, null, null);
        }

        internal void TraceEvent(
          byte level,
          Guid eventGuid,
          byte eventType,
          object data0,
          object data1,
          object data2,
          object data3,
          object data4)
        {
            int num = (int)TraceEvent(level, eventGuid, eventType, data0, data1, data2, data3, data4, null, null, null, null);
        }

        internal void TraceEvent(
          byte level,
          Guid eventGuid,
          byte eventType,
          object data0,
          object data1,
          object data2,
          object data3,
          object data4,
          object data5)
        {
            int num = (int)TraceEvent(level, eventGuid, eventType, data0, data1, data2, data3, data4, data5, null, null, null);
        }

        internal void TraceEvent(
          byte level,
          Guid eventGuid,
          byte eventType,
          object data0,
          object data1,
          object data2,
          object data3,
          object data4,
          object data5,
          object data6)
        {
            int num = (int)TraceEvent(level, eventGuid, eventType, data0, data1, data2, data3, data4, data5, data6, null, null);
        }

        internal void TraceEvent(
          byte level,
          Guid eventGuid,
          byte eventType,
          object data0,
          object data1,
          object data2,
          object data3,
          object data4,
          object data5,
          object data6,
          object data7)
        {
            int num = (int)TraceEvent(level, eventGuid, eventType, data0, data1, data2, data3, data4, data5, data6, data7, null);
        }

        internal uint TraceEvent(
          byte level,
          Guid eventGuid,
          byte evtype,
          object data0,
          object data1,
          object data2,
          object data3,
          object data4,
          object data5,
          object data6,
          object data7,
          object data8)
        {
            if (_eventSource is null || !_eventSource.IsEnabled())
                return 0;

            var options = new EventSourceOptions
            {
                Level = (EventLevel)level,
                Opcode = (EventOpcode)evtype,
            };
            var payload = new EtwTracePayload(
                FormatField(data0), FormatField(data1), FormatField(data2),
                FormatField(data3), FormatField(data4), FormatField(data5),
                FormatField(data6), FormatField(data7), FormatField(data8));

            _eventSource.Write(eventGuid.ToString("N"), options, payload);
            return 0;
        }

        private static string FormatField(object? data)
        {
            if (data == null)
                return string.Empty;
            Type type = data.GetType();
            if (type.IsEnum)
                data = Convert.ChangeType(data, Enum.GetUnderlyingType(type), CultureInfo.InvariantCulture);
            return data is IFormattable formattable
                ? formattable.ToString(null, CultureInfo.InvariantCulture)
                : data.ToString() ?? string.Empty;
        }

        // Self-describing ETW/EventPipe payload: one string field per MOF
        // data slot the original raw TraceEvent header supported.
        private readonly struct EtwTracePayload
        {
            public EtwTracePayload(
              string data0, string data1, string data2,
              string data3, string data4, string data5,
              string data6, string data7, string data8)
            {
                Data0 = data0;
                Data1 = data1;
                Data2 = data2;
                Data3 = data3;
                Data4 = data4;
                Data5 = data5;
                Data6 = data6;
                Data7 = data7;
                Data8 = data8;
            }

            public string Data0 { get; }
            public string Data1 { get; }
            public string Data2 { get; }
            public string Data3 { get; }
            public string Data4 { get; }
            public string Data5 { get; }
            public string Data6 { get; }
            public string Data7 { get; }
            public string Data8 { get; }
        }

        // TODO: EventSource has no public/protected constructor that accepts
        // an explicit provider Guid (only (Guid, string, ...) overloads,
        // which are `internal` to System.Private.CoreLib - confirmed via
        // reflection, not guessed) or a runtime-settable Guid; the provider
        // Guid EventSource actually registers is always deterministically
        // derived from its name. So `controlGuid` (e.g.
        // PerfTrace.ZUNE_ETW_CONTROL_GUID) can no longer be the literal
        // provider Guid a native ETW controller would see on Windows - it is
        // instead folded into a stable provider *name* here. Documented per
        // CLAUDE.md "Dealing with unknowns and uncertainty"; see the log for
        // the full explanation.
        private sealed class EtwEventSource : EventSource
        {
            internal EtwEventSource(Guid controlGuid)
                : base($"Zune-{controlGuid:N}", EventSourceSettings.EtwSelfDescribingEventFormat)
            {
            }

            internal bool IsLevelEnabled(byte level, uint flags) =>
                IsEnabled((EventLevel)level, (EventKeywords)flags);
        }
    }
}
