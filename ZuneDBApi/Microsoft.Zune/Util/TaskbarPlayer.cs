using System;
using System.Drawing;
using Microsoft.Iris;

namespace Microsoft.Zune.Util;

// Original wraps native Windows 7 taskbar integration (thumbnail preview window, jump
// list command dispatch via window messages). Not reverse engineered — see
// logs/Microsoft.Zune/Util/DownloadManager.md.
public class TaskbarPlayer : IDisposable
{
    private static TaskbarPlayer sm_taskbarPlayer;

    public static TaskbarPlayer Instance => sm_taskbarPlayer ??= new TaskbarPlayer();

    public Size RestoreSize { get; private set; }

    public Point RestorePosition { get; private set; }

    public WindowState RestoreState { get; set; }

    public Size PopupSize { get; private set; }

    public Point PopupPosition { get; private set; }

    public bool PopupVisible { get; set; }

    public bool ToolbarVisible { get; set; }

    public bool EnableToolbar { get; set; }

    public bool ShowToolbar { get; set; }

    public Command Restore { get; } = new();

    private TaskbarPlayer()
    {
    }

    public bool Initialize(IntPtr hWndFrame, TaskbarPlayerCommandHandler commandHandler) => false;

    public void UpdateToolbar(ETaskbarPlayerState state)
    {
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
