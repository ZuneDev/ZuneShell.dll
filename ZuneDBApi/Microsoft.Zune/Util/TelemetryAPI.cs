using System.Collections;

namespace Microsoft.Zune.Util;

// Original reports to a native telemetry service. Not reverse engineered — see
// logs/Microsoft.Zune/Util/DownloadManager.md.
public class TelemetryAPI
{
    public static void SendDatapoint(string command, IDictionary dictionary)
    {
    }

    public static void AddToSessionEvent(ETelemetryEvent evt, string key, int value)
    {
    }

    public static void SendEvent(ETelemetryEvent eEvent, string eventParameter)
    {
    }
}
