using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service;

// Native struct VideoMetadata — verified total size 200 bytes (ILSpy metadata; see
// MusicAlbumMetadata.cs for the general recovery methodology). Field offsets
// recovered from VideoOfferCollection.Init's decompiled body — see
// logs/Microsoft.Zune/Service/OfferCollection.md. That decompilation's *control flow*
// is corrupted (ILSpy itself reports "Incompatible stack types" on this method), but
// individual field-offset reads are still legible and independently corroborated
// (e.g. the IMediaRights pointer at 192 dispatches through the same vtable slots
// confirmed by Album/App/Track). Bytes not covered below are never read there and
// are left undeclared rather than guessed.
[StructLayout(LayoutKind.Explicit, Size = 200)]
internal struct VideoMetadata
{
    [FieldOffset(8)]
    public Guid Id;

    [FieldOffset(24)]
    public int IsMusicVideoFlag;

    [FieldOffset(32)]
    public nint TitlePtr;

    [FieldOffset(48)]
    public nint ArtistPtr;

    [FieldOffset(80)]
    public Guid AlbumId;

    [FieldOffset(96)]
    public nint GenrePtr;

    [FieldOffset(112)]
    public nint ProductionCompanyPtr;

    [FieldOffset(120)]
    public nint SeriesTitlePtr;

    [FieldOffset(128)]
    public int SeasonNumber;

    [FieldOffset(132)]
    public int EpisodeNumber;

    [FieldOffset(144)]
    public nint ReleaseDatePtr;

    [FieldOffset(160)]
    public nint PreviewImageUrlPtr;

    [FieldOffset(192)]
    public nint MediaRightsPtr;
}
