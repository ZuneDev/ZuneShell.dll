using System.Collections.Generic;

namespace ZuneUI;

internal interface IMonitorDetector
{
    List<MonitorSize> DetectMonitors();
}