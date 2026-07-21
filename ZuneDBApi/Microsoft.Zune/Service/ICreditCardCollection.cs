using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Microsoft.Zune.Service;

// Native COM interface ICreditCardCollection — vtable layout (x64, IUnknown = slots 0–2).
// Fully recovered from CreditCardCollection.Init/!CreditCardCollection's decompiled
// bodies in ZuneShell/lib/ZuneDBApi.dll — see
// logs/Microsoft.Zune/Service/OfferCollection.md. Like IBillingOfferCollection,
// GetItem returns flat scalar/BSTR fields rather than a native metadata struct.
// paymentType (slot 4's first out param) is fetched but never read anywhere in the
// original Init body — kept here for vtable-shape fidelity, but callers can ignore it.
//   [3]  GetCount() -> int
//   [4]  GetItem(index, out EBillingPaymentType, out 10 BSTRs [id, address x6,
//                phone x3], out ECreditCardType, out 4 BSTRs [accountHolderName,
//                accountNumber, ccvNumber, expirationDate]) -> HRESULT
[GeneratedComInterface]
[Guid("3d6e9a2c-7f1b-4d8e-8a3c-1f5b9e2d6a7c")]
internal partial interface ICreditCardCollection
{
    [PreserveSig]
    int GetCount();

    [PreserveSig]
    int GetItem(
        int index,
        out EBillingPaymentType paymentType,
        [MarshalAs(UnmanagedType.BStr)] out string id,
        [MarshalAs(UnmanagedType.BStr)] out string street1,
        [MarshalAs(UnmanagedType.BStr)] out string street2,
        [MarshalAs(UnmanagedType.BStr)] out string city,
        [MarshalAs(UnmanagedType.BStr)] out string district,
        [MarshalAs(UnmanagedType.BStr)] out string state,
        [MarshalAs(UnmanagedType.BStr)] out string postalCode,
        [MarshalAs(UnmanagedType.BStr)] out string phonePrefix,
        [MarshalAs(UnmanagedType.BStr)] out string phoneNumber,
        [MarshalAs(UnmanagedType.BStr)] out string phoneExtension,
        out ECreditCardType creditCardType,
        [MarshalAs(UnmanagedType.BStr)] out string accountHolderName,
        [MarshalAs(UnmanagedType.BStr)] out string accountNumber,
        [MarshalAs(UnmanagedType.BStr)] out string ccvNumber,
        [MarshalAs(UnmanagedType.BStr)] out string expirationDate);
}
