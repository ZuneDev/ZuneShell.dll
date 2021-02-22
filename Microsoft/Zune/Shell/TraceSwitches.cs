// Decompiled with JetBrains decompiler
// Type: Microsoft.Zune.Shell.TraceSwitches
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace Microsoft.Zune.Shell
{
    internal static class TraceSwitches
    {
        private static ZuneTraceSwitch collectionSwitch;
        private static ZuneTraceSwitch shellSwitch;
        private static ZuneTraceSwitch dataProviderSwitch;

        public static ZuneTraceSwitch CollectionSwitch
        {
            get
            {
                if (TraceSwitches.collectionSwitch == null)
                    TraceSwitches.collectionSwitch = new ZuneTraceSwitch("Collection", "Collection page traces");
                return TraceSwitches.collectionSwitch;
            }
        }

        public static ZuneTraceSwitch ShellSwitch
        {
            get
            {
                if (TraceSwitches.shellSwitch == null)
                    TraceSwitches.shellSwitch = new ZuneTraceSwitch("Shell", "Shell traces");
                return TraceSwitches.shellSwitch;
            }
        }

        public static ZuneTraceSwitch DataProviderSwitch
        {
            get
            {
                if (TraceSwitches.dataProviderSwitch == null)
                    TraceSwitches.dataProviderSwitch = new ZuneTraceSwitch("DataProvider", "Data provider traces");
                return TraceSwitches.dataProviderSwitch;
            }
        }
    }
}
