using System.Collections.Generic;
using Silk.NET.GLFW;

namespace ZuneUI;

internal class GlfwMonitorDetector : IMonitorDetector
{
    public unsafe List<MonitorSize> DetectMonitors()
    {
        var glfw = Glfw.GetApi();
        var monitorHandles = glfw.GetMonitors(out var count);
        
        var monitors = new List<MonitorSize>(count);
        for (var m = 0; m < count; m++)
        {
            var monitor = monitorHandles[m];
            
            var videoMode = glfw.GetVideoMode(monitor);
            RECT totalRect = new()
            {
                Left = 0,
                Top = 0,
                Right = videoMode->Width,
                Bottom = videoMode->Height
            };
            
            glfw.GetMonitorWorkarea(monitor, out var workX, out var workY, out var workWidth, out var workHeight);
            RECT workRect = new()
            {
                Left = workX,
                Top = workY,
                Right = workX + workWidth,
                Bottom = workY + workHeight
            };
            
            monitors.Add(new MonitorSize(totalRect, workRect));
        }

        return monitors;
    }
}