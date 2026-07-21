// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type with an empty namespace, like EMediaTypes.cs — see that
// file's header comment for the full rationale).
public enum EServiceMediaType
{
    eServiceMediaTypeInvalid = -1,
    eServiceMediaTypeAlbum = 0,
    eServiceMediaTypePlaylist = 1,
    eServiceMediaTypeChannel = 2,
    eServiceMediaTypeArtist = 3,
    eServiceMediaTypeCount = 4,
}
