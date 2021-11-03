// Decompiled with JetBrains decompiler
// Type: ZuneUI.TelemetryInfo
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Collections;

namespace ZuneUI
{
    public class TelemetryInfo
    {
        public ETelemetryEvent eEvent = ETelemetryEvent.eTelemetryEventUndefined;
        public string dcsUri = "";
        public IDictionary Args;
        public DateTime utcTime;
        public long elapsedTime;
        public bool fSessionDatapoint;

        public TelemetryInfo(ETelemetryEvent evt, string uri, IDictionary args, bool fSession)
        {
            this.eEvent = evt;
            this.dcsUri = uri;
            this.Args = args;
            this.utcTime = DateTime.UtcNow;
            this.fSessionDatapoint = fSession;
            this.elapsedTime = (long)this.utcTime.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
    }
}
