using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MicrosoftZuneInterop;

// IUnknown slots [0]–[2] are implicit. User-declared methods begin at slot [3].
// Slots [3], [4], and [6] recovered from LibraryDataProviderQuery.BeginExecuteWorker's
// decompiled vtable-offset dispatch (offsets 24/32/48 from the vtable base = slots
// 3/4/6) — see logs/MicrosoftZuneLibrary/LibraryDataProviderQuery.md.
// TODO: replace placeholder GUID with the real one recovered from the native binary.
[GeneratedComInterface]
[Guid("3D8A1F2B-6C4E-4A5D-9B7F-2E0C1A8D3F4E")]
partial interface IQueryPropertyBag
{
    // [3]: SetIDList(prop, IDList*) -> HRESULT
    [PreserveSig]
    int SetIDList(EQueryPropertyBagProp prop, IntPtr idList);

    // [4]: SetMultiSortAttributes(prop, IMultiSortAttributes*) -> HRESULT
    [PreserveSig]
    int SetMultiSortAttributes(EQueryPropertyBagProp prop, IntPtr sortAttributes);

    // [5]: SetString(prop, wchar_t*) -> HRESULT
    [PreserveSig]
    int SetString(EQueryPropertyBagProp prop, [MarshalAs(UnmanagedType.LPWStr)] string value);

    // [6]: SetInt64(prop, unsigned __int64) -> HRESULT [used for DrmStateMask]
    [PreserveSig]
    int SetInt64(EQueryPropertyBagProp prop, ulong value);

    // [7]: SetInt(prop, int) -> HRESULT  [bool uses 1/0]
    [PreserveSig]
    int SetInt(EQueryPropertyBagProp prop, int value);

    // [8]–[12]: unknown
    void _Reserved8();
    void _Reserved9();
    void _Reserved10();
    void _Reserved11();
    void _Reserved12();

    // [13]: IsSet(prop, int* out) -> HRESULT
    [PreserveSig]
    int IsSet(EQueryPropertyBagProp prop, out int result);
}
