using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Microsoft.Zune.Service;

// Native COM interface IPriceInfo — vtable layout (x64, IUnknown = slots 0–2):
//   [3]  GetPointsPrice()    -> int
//   [4]  GetCurrencyPrice()  -> double
//   [5]  HasPoints()         -> BOOL
//   [6]  HasCurrency()       -> BOOL
//   [7]  unknown — never observed at any managed call site
//   [8]  GetDisplayPrice(out BSTR)  -> HRESULT
//   [9]  GetCurrencyCode(out BSTR)  -> HRESULT
//
// Recovered from ILSpy decompilation of Microsoft.Zune.Service.PriceInfo.Init in
// ZuneShell/lib/ZuneDBApi.dll (see logs/Microsoft.Zune/Service/OfferCollection.md) —
// every slot up to 9 is backed by a real call site except [7], which is skipped
// entirely (offset 56 is never dereferenced). No GUID recoverable; objects of this
// type are only ever received via IMediaRights::GetPriceInfo/GetPriceInfo2 out params.
[GeneratedComInterface]
[Guid("8f1c3a6e-4b2d-4a9f-8e5c-1d7b3a9f2c6e")]
internal partial interface IPriceInfo
{
    [PreserveSig]
    int GetPointsPrice();

    [PreserveSig]
    double GetCurrencyPrice();

    [PreserveSig]
    int HasPoints();

    [PreserveSig]
    int HasCurrency();

    void _Reserved7();

    [PreserveSig]
    int GetDisplayPrice([MarshalAs(UnmanagedType.BStr)] out string value);

    [PreserveSig]
    int GetCurrencyCode([MarshalAs(UnmanagedType.BStr)] out string value);
}
