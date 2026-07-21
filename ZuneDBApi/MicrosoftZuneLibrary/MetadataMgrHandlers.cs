using System;

namespace MicrosoftZuneLibrary;

// Grouped together since each is a one-line delegate declaration recovered from
// MetadataMgrNotifications's event fields — see logs/MicrosoftZuneLibrary/ZuneLibrary.md.
// The IntPtr parameters match the original exactly: ILSpy resolved these particular
// native wchar_t* callback parameters as raw IntPtr rather than string (unlike most
// other delegates in this codebase), so no different a translation was invented here.
public delegate void OnFingerprintingFile(IntPtr sourceUrl);
public delegate void OnFileDeleteFailed(IntPtr sourceUrl);
public delegate void OnEndMigrateHandler();
public delegate void OnBeginMigrateHandler();
public delegate void OnEndMetadataLifecycleHandler();
public delegate void OnBeginMetadataLifecycleHandler();
public delegate void OnMetadataUpdateHandler(IntPtr artistName, IntPtr albumTitle);
public delegate void OnScanCompletedHandler();
public delegate void OnEndFileChangeHandler();
public delegate void OnBeginFileChangeHandler();
public delegate void OnEndScanDirectoryHandler(IntPtr sourceUrl);
public delegate void OnBeginScanDirectoryHandler(IntPtr sourceUrl);
public delegate void OnFileRemovedHandler(IntPtr sourceUrl);
public delegate void OnFileErrorHandler(IntPtr sourceUrl);
public delegate void OnFileAddedHandler(IntPtr sourceUrl, EMediaTypes mediaType);
