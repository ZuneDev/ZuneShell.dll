using System;

namespace MicrosoftZuneLibrary;

// Original advises a native metadata-manager notification sink covering file
// add/remove/error, directory scan begin/end, metadata lifecycle, migration, and
// fingerprinting events. Not reverse engineered — see
// logs/MicrosoftZuneLibrary/ZuneLibrary.md.
public class MetadataMgrNotifications : IDisposable
{
    public event OnFingerprintingFile FingerprintingFile;
    public event OnFileDeleteFailed FileDeleteFailed;
    public event OnEndMigrateHandler EndMigrate;
    public event OnBeginMigrateHandler BeginMigrate;
    public event OnEndMetadataLifecycleHandler EndMetadataLifecycle;
    public event OnBeginMetadataLifecycleHandler BeginMetadataLifecycle;
    public event OnMetadataUpdateHandler MetadataUpdate;
    public event OnScanCompletedHandler ScanCompleted;
    public event OnEndFileChangeHandler EndFileChange;
    public event OnBeginFileChangeHandler BeginFileChange;
    public event OnEndScanDirectoryHandler EndScanDirectory;
    public event OnBeginScanDirectoryHandler BeginScanDirectory;
    public event OnFileRemovedHandler FileRemoved;
    public event OnFileErrorHandler FileError;
    public event OnFileAddedHandler FileAdded;

    public MetadataMgrNotifications()
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
