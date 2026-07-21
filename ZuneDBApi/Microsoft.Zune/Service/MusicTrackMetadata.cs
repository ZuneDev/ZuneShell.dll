using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service;

// Native struct MusicTrackMetadata — verified total size 160 bytes (ILSpy metadata;
// see MusicAlbumMetadata.cs for the general recovery methodology). Field offsets
// recovered from TrackOfferCollection.Init's decompiled body — see
// logs/Microsoft.Zune/Service/OfferCollection.md. Bytes not covered below are never
// read there and are left undeclared rather than guessed.
[StructLayout(LayoutKind.Explicit, Size = 160)]
internal struct MusicTrackMetadata
{
    [FieldOffset(8)]
    public Guid Id;

    [FieldOffset(72)]
    public int TrackNumber;

    [FieldOffset(80)]
    public nint TitlePtr;

    [FieldOffset(88)]
    public nint AlbumPtr;

    [FieldOffset(96)]
    public nint ArtistPtr;

    [FieldOffset(152)]
    public nint MediaRightsPtr;
}
