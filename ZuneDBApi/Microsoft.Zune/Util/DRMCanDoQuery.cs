using System;

namespace Microsoft.Zune.Util;

// Original wraps a native IDRMQuery* obtained via ZuneLibraryExports.CreateDRMQuery,
// neither of which has been reverse engineered — see
// logs/Microsoft.Zune/Util/DownloadManager.md. Public surface preserved so callers keep
// compiling; every query answers "no" since there's no native DRM engine to ask.
public class DRMCanDoQuery : IDisposable
{
    public DRMCanDoQuery()
    {
    }

    public void SetDeviceInfo(bool fHasSerialNumber, string deviceCert)
    {
    }

    public bool CanBurnFile(string path) => false;

    public bool CanBurnKID(string drmKeyId) => false;

    public bool CanSyncFile(string path) => false;

    public bool CanSyncKID(string drmKeyId) => false;

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~DRMCanDoQuery()
    {
        Dispose(false);
    }
}
