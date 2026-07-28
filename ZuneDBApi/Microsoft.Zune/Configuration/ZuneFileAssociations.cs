using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Zune.Configuration
{
    /// <summary>
    /// Shared table of the file extensions Zune can register itself as the
    /// handler for, used by every <see cref="IFileAssociationHandler"/>
    /// implementation so the platform-specific handlers only need to know how
    /// to read/write a single association, not which extensions exist.
    /// </summary>
    /// <remarks>
    /// The native ZuneDBApi.dll's own association table has not been recovered
    /// (stage 1 decompilation was skipped for this surface per the user's
    /// request to jump straight to a stage-3 cross-platform implementation).
    /// The extension list and <see cref="EMediaTypes"/> classification below
    /// are grounded in evidence already decompiled elsewhere in this
    /// repository rather than guessed from scratch:
    /// <list type="bullet">
    /// <item>the extension set matches <c>ZuneUI.Management._defaultFileTypeExtensions</c>
    /// (<c>ZuneShell/ZuneUI/Management.cs</c>);</item>
    /// <item><c>.mbr</c> and <c>.zpl</c> are classified as Video/Playlist
    /// respectively because <see cref="EMediaTypes"/> already defines
    /// <c>eMediaTypeVideoMBR</c> and <c>eMediaTypePlaylistZPL</c>
    /// (<c>ZuneDBApi/EMediaTypes.cs</c>); <c>.mp3</c>/<c>.m4a</c>/<c>.m4b</c>
    /// (audio) and <c>.mp4</c>/<c>.m4v</c> (video) follow their unambiguous,
    /// well-known container formats.</item>
    /// </list>
    /// <c>ProgId</c> and <c>Description</c> are placeholders (a per-extension
    /// <c>"ZuneShell.&lt;ext&gt;"</c> ProgId and a generic "&lt;EXT&gt; File"
    /// description) pending recovery of the original strings — see
    /// logs/Microsoft.Zune/Configuration/FileAssociationHandler.md.
    /// <c>MimeType</c> is populated only where a type is unambiguously
    /// registered with shared-mime-info/IANA; <c>.mbr</c> and <c>.zpl</c> have
    /// no registered MIME type, so they're left <see langword="null"/> and are
    /// skipped by the Linux (xdg-mime) handler.
    /// </remarks>
    internal static class ZuneFileAssociations
    {
        internal sealed record Entry(string Extension, string ProgId, string Description, EMediaTypes MediaType, string? MimeType);

        internal static readonly IReadOnlyList<Entry> All = new[]
        {
            new Entry(".mp3", "ZuneShell.mp3", "MP3 File", EMediaTypes.eMediaTypeAudio, "audio/mpeg"),
            new Entry(".m4a", "ZuneShell.m4a", "M4A File", EMediaTypes.eMediaTypeAudio, "audio/mp4"),
            new Entry(".mp4", "ZuneShell.mp4", "MP4 File", EMediaTypes.eMediaTypeVideo, "video/mp4"),
            new Entry(".m4b", "ZuneShell.m4b", "M4B File", EMediaTypes.eMediaTypeAudio, "audio/x-m4b"),
            new Entry(".m4v", "ZuneShell.m4v", "M4V File", EMediaTypes.eMediaTypeVideo, "video/mp4"),
            new Entry(".mbr", "ZuneShell.mbr", "MBR File", EMediaTypes.eMediaTypeVideo, null),
            new Entry(".zpl", "ZuneShell.zpl", "ZPL File", EMediaTypes.eMediaTypePlaylist, null),
        };

        internal static Entry? Find(string extension) =>
            All.FirstOrDefault(e => string.Equals(e.Extension, extension, System.StringComparison.OrdinalIgnoreCase));
    }
}
