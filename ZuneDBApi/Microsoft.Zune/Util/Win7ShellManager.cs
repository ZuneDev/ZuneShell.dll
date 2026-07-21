using System;
using MicrosoftZuneLibrary;

namespace Microsoft.Zune.Util;

// Original wraps native Windows 7 shell integration (libraries, jump lists, thumbnail
// toolbars, taskbar window messages). Not reverse engineered — see
// logs/Microsoft.Zune/Util/DownloadManager.md.
public class Win7ShellManager
{
    private static Win7ShellManager sm_win7ShellManager;

    public static Win7ShellManager Instance => sm_win7ShellManager ??= new Win7ShellManager();

    public event MonitorChangeHandler OnMonitorChange;
    public event ThumbBarButtonPressHandler OnThumbBarButtonPress;
    public event WindowPositionKeyPressHandler OnWindowPositionKeyPress;

    private Win7ShellManager()
    {
    }

    public int AddLocationToLibrary(EWin7LibraryKind libraryKind, bool defaultSaveFolder, string path) => unchecked((int)0x80004005);

    public int RemoveLocationFromLibrary(EWin7LibraryKind libraryKind, out bool defaultSaveFolder, string path)
    {
        defaultSaveFolder = false;
        return unchecked((int)0x80004005);
    }

    public int BeginJumpListSession(out JumpListSession session)
    {
        session = null;
        return unchecked((int)0x80004005);
    }

    public int BeginThumbBarSession(IntPtr hWnd, out ThumbBar thumbBar)
    {
        thumbBar = null;
        return unchecked((int)0x80004005);
    }

    public int SubprocWindow(IntPtr hWnd) => unchecked((int)0x80004005);

    public int ShowLibraryDialog(EWin7LibraryKind libraryKind, IntPtr hWnd, string title, string helpText) => unchecked((int)0x80004005);

    public int SyncLibraryFolders() => unchecked((int)0x80004005);

    public int CreatePodcastLibraryTemplate() => unchecked((int)0x80004005);

    protected void raise_OnWindowPositionKeyPress(WindowPositionKeys value0)
    {
        OnWindowPositionKeyPress?.Invoke(value0);
    }

    protected void raise_OnThumbBarButtonPress(uint value0)
    {
        OnThumbBarButtonPress?.Invoke(value0);
    }

    protected void raise_OnMonitorChange()
    {
        OnMonitorChange?.Invoke();
    }

    internal int WindowPositionKeyPressDetected(WindowPositionKeys key)
    {
        raise_OnWindowPositionKeyPress(key);
        return 0;
    }

    internal int ThumbBarButtonPressed(uint iUniqueID)
    {
        raise_OnThumbBarButtonPress(iUniqueID);
        return 0;
    }

    internal int MonitorChanged()
    {
        raise_OnMonitorChange();
        return 0;
    }
}
