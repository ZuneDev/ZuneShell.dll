using Microsoft.Iris;

namespace Microsoft.Zune.Util;

// Original wraps native Windows 7 taskbar integration (thumbnail preview window, jump
// list command dispatch via window messages). Not reverse engineered — see
// logs/Microsoft.Zune/Util/DownloadManager.md.
// Original derives from Microsoft.Iris.ModelItem, which is where INotifyPropertyChanged
// (PropertyChanged event, FirePropertyChanged) comes from.
public class TaskbarPlayer : ModelItem, IDisposable
{
    private static TaskbarPlayer sm_taskbarPlayer;

    public static TaskbarPlayer Instance => sm_taskbarPlayer ??= new TaskbarPlayer();

    public Size RestoreSize { get; private set; }

    public Point RestorePosition { get; private set; }

    private WindowState m_restoreState = WindowState.Maximized;

    public WindowState RestoreState
    {
        get => m_restoreState;
        set
        {
            if (m_restoreState != value)
            {
                m_restoreState = value;
                FirePropertyChanged(nameof(RestoreState));
            }
        }
    }

    public Size PopupSize { get; private set; }

    public Point PopupPosition { get; private set; }

    private bool m_fPopupVisible;

    public bool PopupVisible
    {
        get => m_fPopupVisible;
        set
        {
            if (m_fPopupVisible == value) return;
            m_fPopupVisible = value;
            FirePropertyChanged(nameof(PopupVisible));
        }
    }

    private bool m_fToolbarVisible;

    public bool ToolbarVisible
    {
        get => m_fToolbarVisible;
        set
        {
            if (m_fToolbarVisible == value) return;
            m_fToolbarVisible = value;
            FirePropertyChanged(nameof(ToolbarVisible));
        }
    }

    public bool EnableToolbar { get; set; }

    private bool m_fShowToolbar;

    public bool ShowToolbar
    {
        get => m_fShowToolbar;
        set
        {
            if (m_fShowToolbar == value) return;
            m_fShowToolbar = value;
            FirePropertyChanged(nameof(ShowToolbar));
        }
    }

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
        if (disposing)
            base.Dispose();
    }

    public new void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
