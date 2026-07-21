using System;

namespace MicrosoftZuneLibrary;

// Original advises a native IInteropNotify* for ShowErrorDialog callbacks via
// ZuneLibraryExports.InteropNotifyAdvise. Not reverse engineered — see
// logs/MicrosoftZuneLibrary/ZuneLibrary.md.
public class InteropNotifications : IDisposable
{
    public event OnShowErrorDialogHandler ShowErrorDialog;

    public InteropNotifications()
    {
    }

    internal void RaiseShowErrorDialog(int hr, uint uiStringId)
    {
        ShowErrorDialog?.Invoke(hr, uiStringId);
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
