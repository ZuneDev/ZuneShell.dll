using System;

namespace Microsoft.Zune.Playlist;

[Flags]
public enum CreatePlaylistOption
{
    None = 0,
    PrivatePlaylist = 1,
    RenameOnConflict = 2,
    OverwriteOnConflict = 4,
    AutoPlaylist = 8,
    SyncRule = 16,
}
