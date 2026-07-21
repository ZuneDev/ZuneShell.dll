using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service;

// Native struct MusicAlbumMetadata — verified total size 112 bytes (ILSpy metadata:
// `[StructLayout(LayoutKind.Sequential, Size = 112)]` on the original type, which
// exists in ZuneShell/lib/ZuneDBApi.dll only as an opaque native value type with no
// reflectable fields). Field offsets below are recovered from
// AlbumOfferCollection.Init's decompiled body (raw `Unsafe.AddByteOffset` reads) —
// see logs/Microsoft.Zune/Service/OfferCollection.md. Bytes 0–7, 24–39, and 96–111
// are never read there and are left undeclared rather than guessed, matching the
// precedent set by MCHResultAnnouncement (see
// logs/MicrosoftZunePlayback/PlayerInterop.md). The overall Size is verified (not
// guessed), so leaving gaps undeclared is safe: native GetItem always writes exactly
// 112 bytes regardless of which fields we've named.
//
// String fields are BSTRs owned by the caller after GetItem returns (the original's
// MusicAlbumMetadata::{ctor}/{dtor} pair frees them automatically on native scope
// exit; our managed Init loop must free them explicitly via Marshal.FreeBSTR after
// copying to a managed string, matching the explicit-free idiom already used in
// BillingOfferCollection/CreditCardCollection).
[StructLayout(LayoutKind.Explicit, Size = 112)]
internal struct MusicAlbumMetadata
{
    [FieldOffset(8)]
    public Guid Id;

    [FieldOffset(40)]
    public nint TitlePtr;

    [FieldOffset(48)]
    public nint ArtistPtr;

    [FieldOffset(56)]
    public nint GenrePtr;

    [FieldOffset(64)]
    public nint ReleaseDatePtr;

    [FieldOffset(72)]
    public nint CoverArtUrlPtr;

    [FieldOffset(84)]
    public int Premium;

    [FieldOffset(88)]
    public nint MediaRightsPtr;
}
