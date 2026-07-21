// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type with an empty namespace). Physically placed under
// MicrosoftZuneInterop/ since IQueryPropertyBag and QueryPropertyBag are its only
// consumers.
//
// Values and name strings recovered from the native kPropIdMap table in
// ZuneDBApi.dll (see logs/MicrosoftZuneInterop/QueryPropertyBag.md, 2026-07-21
// entry): the field's file offset was located via its .NET FieldRVA metadata
// (System.Reflection.Metadata on the <Module>.MicrosoftZuneInterop.?A0x52c37a46.kPropIdMap
// field), then the 37 PropIdMapEntry records (wchar_t* name, int id) were read
// directly from the PE file. The `id` values below are therefore verified, not
// guessed.
//
// TODO: the *member identifiers* below (e.g. eQueryPropertyBagPropUserId) are an
// assumption — the native anonymous-namespace enum carries no member names in
// either the .NET metadata or recoverable native symbols, so the `e<TypeName><Member>`
// convention already used by EMediaTypes/EQueryType in this codebase was applied
// to the verified kPropIdMap key strings. Only the underlying int values (and the
// string keys used by MapNameToProp) are load-bearing for interop; these
// identifiers are cosmetic and can be renamed without breaking compatibility if
// the real names are ever recovered (e.g. from a symbol server or leaked PDB).
public enum EQueryPropertyBagProp
{
    eQueryPropertyBagPropUserId = 0,
    eQueryPropertyBagPropDeviceId = 1,
    eQueryPropertyBagPropRuleTypeId = 2,
    eQueryPropertyBagPropArtistId = 3,
    eQueryPropertyBagPropArtistIds = 4,
    eQueryPropertyBagPropContributingArtistId = 5,
    eQueryPropertyBagPropAlbumId = 6,
    eQueryPropertyBagPropAlbumIds = 7,
    eQueryPropertyBagPropSeriesId = 8,
    eQueryPropertyBagPropFolderId = 9,
    eQueryPropertyBagPropPlaylistId = 10,
    eQueryPropertyBagPropGenreId = 11,
    eQueryPropertyBagPropGenreIds = 12,
    eQueryPropertyBagPropMediaType = 13,
    eQueryPropertyBagPropQueryType = 14,
    eQueryPropertyBagPropQueryView = 15,
    eQueryPropertyBagPropOperation = 16,
    eQueryPropertyBagPropInitTime = 17,
    eQueryPropertyBagPropSyncMappedError = 18,
    eQueryPropertyBagPropKeywords = 19,
    eQueryPropertyBagPropTOC = 20,
    eQueryPropertyBagPropSortColumnId = 21,
    eQueryPropertyBagPropSortTypeId = 22,
    eQueryPropertyBagPropSortAttributesId = 23,
    eQueryPropertyBagPropPlaylistType = 24,
    eQueryPropertyBagPropPlaylistTypeMask = 25,
    eQueryPropertyBagPropInLibrary = 26,
    eQueryPropertyBagPropCategoryId = 27,
    eQueryPropertyBagPropPersonType = 28,
    eQueryPropertyBagPropMediaId = 29,
    eQueryPropertyBagPropUserCardIds = 30,
    eQueryPropertyBagPropMaxResultCount = 31,
    eQueryPropertyBagPropWatchType = 32,
    eQueryPropertyBagPropExpiresOnly = 33,
    eQueryPropertyBagPropDrmStateMask = 34,
    eQueryPropertyBagPropPinType = 35,
    eQueryPropertyBagPropRecursive = 36,
}
