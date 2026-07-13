using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MicrosoftZuneInterop;

// IUnknown slots [0]–[2] are implicit. User-declared methods begin at slot [3].
// TODO: replace placeholder GUID with the real one recovered from the native binary.
[GeneratedComInterface]
[Guid("3D8A1F2B-6C4E-4A5D-9B7F-2E0C1A8D3F4E")]
partial interface IQueryPropertyBag
{
    // [3]–[4]: unknown native slots; declared only to preserve vtable ordering.
    void _Reserved3();
    void _Reserved4();

    // [5]: SetString(prop, wchar_t*) -> HRESULT
    [PreserveSig]
    int SetString(EQueryPropertyBagProp prop, [MarshalAs(UnmanagedType.LPWStr)] string value);

    // [6]: unknown
    void _Reserved6();

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
