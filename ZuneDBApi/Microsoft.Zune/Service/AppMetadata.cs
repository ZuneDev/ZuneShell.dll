using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service;

// Native struct AppMetadata — verified total size 144 bytes (ILSpy metadata; see
// MusicAlbumMetadata.cs for the general recovery methodology). Field offsets
// recovered from AppOfferCollection.Init's decompiled body — see
// logs/Microsoft.Zune/Service/OfferCollection.md. Bytes not covered below (0–7,
// 16–23, 32–47, 80–87, 104–111, 116–143) are never read there and are left
// undeclared rather than guessed.
[StructLayout(LayoutKind.Explicit, Size = 144)]
internal struct AppMetadata
{
    [FieldOffset(8)]
    public Guid Id;

    [FieldOffset(24)]
    public nint TitlePtr;

    [FieldOffset(48)]
    public nint PublisherPtr;

    [FieldOffset(56)]
    public nint DeveloperPtr;

    [FieldOffset(64)]
    public nint GenrePtr;

    [FieldOffset(72)]
    public nint ReleaseDatePtr;

    [FieldOffset(88)]
    public nint VersionPtr;

    [FieldOffset(96)]
    public nint PreviewImageUrlPtr;

    [FieldOffset(112)]
    public nint RatingImageUrlPtr;

    [FieldOffset(136)]
    public nint MediaRightsPtr;
}
