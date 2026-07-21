// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type with an empty namespace, like EMediaTypes.cs — see that
// file's header comment for the full rationale).
public enum ESyncOperation
{
    eSyncOperationInvalid = -1,
    eSyncOperationSendToDevice = 0,
    eSyncOperationDeleteFromDevice = 1,
    eSyncOperationUpdateMetadataOnDevice = 2,
    eSyncOperationCopyFromDevice = 3,
    eSyncOperationHijackOnDevice = 4,
    eSyncOperationReplaceContentOnDevice = 5,
}
