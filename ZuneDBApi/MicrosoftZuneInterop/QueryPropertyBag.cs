using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MicrosoftZuneInterop;

// Native COM interface IMultiSortAttributes — vtable layout (x64, IUnknown = slots 0–2):
//   [3]  unknown
//   [4]  GetSortAttributes() -> int*   (parallel array of SchemaMap indices)
//   [5]  GetSortOrders()     -> int*   (parallel array of EQuerySortType values)
//
// Native struct IDList (16 bytes):
//   int   Count
//   (4 bytes padding)
//   int*  Ids   (heap-allocated array of Count media IDs; freed by the native caller)

public class QueryPropertyBag : IDisposable
{
    // StrategyBasedComWrappers has no singleton `Instance`/`Default` (unlike some other
    // ComWrappers-derived types) — it has a public parameterless constructor and is meant
    // to be instantiated once and reused, so it's cached here.
    private static readonly StrategyBasedComWrappers s_comWrappers = new();

    private IQueryPropertyBag? _bag;

    public QueryPropertyBag()
    {
        // TODO: P/Invoke ZuneLibraryExports.CreatePropertyBag to get an IQueryPropertyBag* pointer,
        // then wrap it:
        //   IntPtr ptr = ...; // from CreatePropertyBag
        //   _bag = (IQueryPropertyBag)s_comWrappers
        //              .GetOrCreateObjectForComInstance(ptr, CreateObjectFlags.None);
    }

    public void SetValue(string propertyName, object? value)
    {
        EQueryPropertyBagProp prop = MapNameToProp(propertyName);
        if (prop == (EQueryPropertyBagProp)(-1) || value is null)
            return;

        int hr = value switch
        {
            int i    => _bag!.SetInt(prop, i),
            bool b   => _bag!.SetInt(prop, b ? 1 : 0),
            string s => _bag!.SetString(prop, s),
            _        => throw new ApplicationException($"Unsupported property value type: {value.GetType()}")
        };

        if (hr < 0)
            throw new ApplicationException(GetErrorDescription(hr));
    }

    public bool IsSet(string propertyName)
    {
        EQueryPropertyBagProp prop = MapNameToProp(propertyName);
        if (prop == (EQueryPropertyBagProp)(-1))
            return false;

        _bag!.IsSet(prop, out int result);
        return result == 1;
    }

    // Reimplements the native kPropIdMap linear scan (37 entries, case-insensitive
    // wchar_t* name -> EQueryPropertyBagProp) as a managed lookup table. The original
    // walks the array with _wcsicmp and throws on no match rather than returning a
    // sentinel (verified via ILSpy decompilation of the original MapNameToProp IL —
    // see logs/MicrosoftZuneInterop/QueryPropertyBag.md); a Dictionary with an
    // ordinal case-insensitive comparer reproduces both behaviors without needing
    // to replicate the native array's raw memory layout.
    private static readonly Dictionary<string, EQueryPropertyBagProp> s_propIdMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["UserId"] = EQueryPropertyBagProp.eQueryPropertyBagPropUserId,
        ["DeviceId"] = EQueryPropertyBagProp.eQueryPropertyBagPropDeviceId,
        ["RuleTypeId"] = EQueryPropertyBagProp.eQueryPropertyBagPropRuleTypeId,
        ["ArtistId"] = EQueryPropertyBagProp.eQueryPropertyBagPropArtistId,
        ["ArtistIds"] = EQueryPropertyBagProp.eQueryPropertyBagPropArtistIds,
        ["ContributingArtistId"] = EQueryPropertyBagProp.eQueryPropertyBagPropContributingArtistId,
        ["AlbumId"] = EQueryPropertyBagProp.eQueryPropertyBagPropAlbumId,
        ["AlbumIds"] = EQueryPropertyBagProp.eQueryPropertyBagPropAlbumIds,
        ["SeriesId"] = EQueryPropertyBagProp.eQueryPropertyBagPropSeriesId,
        ["FolderID"] = EQueryPropertyBagProp.eQueryPropertyBagPropFolderId,
        ["PlaylistId"] = EQueryPropertyBagProp.eQueryPropertyBagPropPlaylistId,
        ["GenreId"] = EQueryPropertyBagProp.eQueryPropertyBagPropGenreId,
        ["GenreIds"] = EQueryPropertyBagProp.eQueryPropertyBagPropGenreIds,
        ["MediaType"] = EQueryPropertyBagProp.eQueryPropertyBagPropMediaType,
        ["QueryType"] = EQueryPropertyBagProp.eQueryPropertyBagPropQueryType,
        ["QueryView"] = EQueryPropertyBagProp.eQueryPropertyBagPropQueryView,
        ["Operation"] = EQueryPropertyBagProp.eQueryPropertyBagPropOperation,
        ["InitTime"] = EQueryPropertyBagProp.eQueryPropertyBagPropInitTime,
        ["SyncMappedError"] = EQueryPropertyBagProp.eQueryPropertyBagPropSyncMappedError,
        ["Keywords"] = EQueryPropertyBagProp.eQueryPropertyBagPropKeywords,
        ["TOC"] = EQueryPropertyBagProp.eQueryPropertyBagPropTOC,
        ["SortColumnId"] = EQueryPropertyBagProp.eQueryPropertyBagPropSortColumnId,
        ["SortTypeId"] = EQueryPropertyBagProp.eQueryPropertyBagPropSortTypeId,
        ["SortAttributesId"] = EQueryPropertyBagProp.eQueryPropertyBagPropSortAttributesId,
        ["PlaylistType"] = EQueryPropertyBagProp.eQueryPropertyBagPropPlaylistType,
        ["PlaylistTypeMask"] = EQueryPropertyBagProp.eQueryPropertyBagPropPlaylistTypeMask,
        ["InLibrary"] = EQueryPropertyBagProp.eQueryPropertyBagPropInLibrary,
        ["CategoryId"] = EQueryPropertyBagProp.eQueryPropertyBagPropCategoryId,
        ["PersonType"] = EQueryPropertyBagProp.eQueryPropertyBagPropPersonType,
        ["MediaId"] = EQueryPropertyBagProp.eQueryPropertyBagPropMediaId,
        ["UserCardIds"] = EQueryPropertyBagProp.eQueryPropertyBagPropUserCardIds,
        ["MaxResultCount"] = EQueryPropertyBagProp.eQueryPropertyBagPropMaxResultCount,
        ["WatchType"] = EQueryPropertyBagProp.eQueryPropertyBagPropWatchType,
        ["ExpiresOnly"] = EQueryPropertyBagProp.eQueryPropertyBagPropExpiresOnly,
        ["DrmStateMask"] = EQueryPropertyBagProp.eQueryPropertyBagPropDrmStateMask,
        ["PinType"] = EQueryPropertyBagProp.eQueryPropertyBagPropPinType,
        ["Recursive"] = EQueryPropertyBagProp.eQueryPropertyBagPropRecursive,
    };

    public EQueryPropertyBagProp MapNameToProp(string propertyName)
    {
        if (s_propIdMap.TryGetValue(propertyName, out EQueryPropertyBagProp prop))
            return prop;

        throw new ArgumentException("Invalid property name: " + propertyName, nameof(propertyName));
    }

    // Mirrors the original <Module>.GetErrorDescription helper (FormatMessage over
    // the Win32 facility of the HRESULT). Windows-only by nature of FormatMessage;
    // see the class-level TODO for the non-Windows fallback.
#if WINDOWS
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int FormatMessageW(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, out IntPtr lpBuffer, uint nSize, IntPtr Arguments);

    [DllImport("kernel32.dll")]
    private static extern IntPtr LocalFree(IntPtr hMem);
#endif

    private static string GetErrorDescription(int hr)
    {
#if WINDOWS
        const uint FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x100;
        const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x1000;
        const uint FORMAT_MESSAGE_IGNORE_INSERTS = 0x200;

        if (FormatMessageW(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
                IntPtr.Zero, (uint)(hr & 0xFFFF), 0, out IntPtr buffer, 0, IntPtr.Zero) == 0)
        {
            return $"Unknown Error: 0x{hr:x}";
        }

        string message = Marshal.PtrToStringUni(buffer) ?? string.Empty;
        LocalFree(buffer);
        return $"{message} Error: 0x{hr:x}";
#else
        // TODO: implement HRESULT-to-string mapping for non-Windows platforms.
        return $"Error: 0x{hr:x}";
#endif
    }

    // Returns the underlying COM pointer for callers that pass it to native query APIs.
    public IntPtr GetIQueryPropertyBag()
    {
        if (_bag is null) return IntPtr.Zero;
        return s_comWrappers.GetOrCreateComInterfaceForObject(_bag, CreateComInterfaceFlags.None);
    }

    // Packs multiIds into a native IDList: { int Count; (4 pad); int* Ids }.
    // The returned IntPtr points to native heap memory; the native caller is responsible for freeing it.
    public unsafe IntPtr PackIDList(IList multiIds)
    {
        int count = multiIds.Count;

        // Allocate the 16-byte IDList struct and zero the Ids pointer slot before filling it,
        // so the struct is never in a half-initialized state if AllocHGlobal below throws.
        IntPtr listPtr = Marshal.AllocHGlobal(16);
        *(int*)listPtr = count;
        *(long*)(listPtr + 8) = 0L;

        IntPtr idsPtr = Marshal.AllocHGlobal(count * sizeof(int));
        *(IntPtr*)(listPtr + 8) = idsPtr;

        for (int i = 0; i < count; i++)
            *(int*)(idsPtr + i * sizeof(int)) = (int)multiIds[i]!;

        return listPtr;
    }

    // Packs parallel sort-direction/attribute arrays into a native IMultiSortAttributes COM object.
    // Requires CSchemaMap.GetIndex to map attribute name strings to schema indices,
    // and a native ZuneLibraryExports.CreateMultiSortAttributes factory.
    public IntPtr PackMultiSortAttributes(string[] sortStrings, bool[] sortAscendings)
    {
        throw new NotImplementedException();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            (_bag as IDisposable)?.Dispose();
        _bag = null;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~QueryPropertyBag()
    {
        Dispose(false);
    }
}
