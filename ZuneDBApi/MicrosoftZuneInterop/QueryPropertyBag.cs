using System;
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
            throw new ApplicationException($"SetValue failed: HRESULT 0x{hr:X8}");
    }

    public bool IsSet(string propertyName)
    {
        EQueryPropertyBagProp prop = MapNameToProp(propertyName);
        if (prop == (EQueryPropertyBagProp)(-1))
            return false;

        _bag!.IsSet(prop, out int result);
        return result == 1;
    }

    public EQueryPropertyBagProp MapNameToProp(string propertyName)
    {
        // TODO: recover the 37-entry kPropIdMap table from the native binary
        // (case-insensitive wchar_t* → EQueryPropertyBagProp, stored as PropIdMapEntry[37]
        // at MicrosoftZuneInterop.?A0x52c37a46.kPropIdMap; each entry is 16 bytes:
        //   [0..7]  wchar_t* name, [8..11] EQueryPropertyBagProp id, [12..15] padding)
        throw new NotImplementedException();
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
