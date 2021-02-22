// Decompiled with JetBrains decompiler
// Type: ZuneUI.SyncErrorHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public static class SyncErrorHelper
    {
        public static Guid GetMediaGuidForLibraryId(int id, MediaType type)
        {
            if (type == MediaType.PodcastEpisode)
                id = PlaylistManager.GetFieldValue<int>(id, PlaylistManager.MediaTypeToListType(type), 311, 0);
            return PlaylistManager.GetFieldValue<Guid>(id, PlaylistManager.MediaTypeToListType(type), 451, Guid.Empty);
        }

        public static string GetEpisodeUrlForLibraryId(int id) => PlaylistManager.GetFieldValue<string>(id, PlaylistManager.MediaTypeToListType(MediaType.PodcastEpisode), 317, string.Empty);
    }
}
