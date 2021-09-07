// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Debug.TraceSettings
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.OS;
using System;
using System.Security;

namespace Microsoft.Iris.Debug
{
    [SuppressUnmanagedCodeSecurity]
    internal static class TraceSettings
    {
        private static string s_debugTraceFile;

        public static void ListenForRegistryUpdates()
        {
        }

        public static void StopListeningForRegistryUpdates()
        {
        }

        public static void Refresh()
        {
            s_debugTraceFile = Environment.GetEnvironmentVariable("SPLASH_TRACE_FILE");
            NativeApi.SpUpdateTraceSettings(s_debugTraceFile, string.Empty, true, false, false);
        }

        public static byte GetCategoryLevel(TraceCategory cat) => 0;

        public static void SetCategoryLevel(TraceCategory cat, byte level)
        {
        }

        public static bool IsFlagsCategory(TraceCategory cat) => false;

        private static bool IsExternalCategory(TraceCategory cat) => (uint)cat < 25U;

        public static bool SendOutputToDebugger => true;

        public static bool TimedWriteLines => false;

        public static bool ShowCategories => false;

        public static bool AlwaysShowBraces
        {
            get => false;
            set
            {
            }
        }

        public static string WriteLinePrefix => string.Empty;

        public static string RendererWriteLinePrefix => string.Empty;

        public static bool DebugTraceToFile => !string.IsNullOrEmpty(s_debugTraceFile);

        public static string DebugTraceFile => s_debugTraceFile;
    }
}
