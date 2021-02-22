// Decompiled with JetBrains decompiler
// Type: ZuneUI.FindAlbumInfoHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Shell;
using MicrosoftZuneLibrary;
using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class FindAlbumInfoHelper
    {
        public static AlbumMetadata GetAlbumMetadata(int libraryId) => ZuneApplication.ZuneLibrary.GetAlbumMetadata(libraryId);

        public static List<SongMatchData> GetSongMatchDataList(
          AlbumMetadata localAlbum,
          AlbumMetadata mergedAlbum,
          uint wmisAlbumTrackCount)
        {
            List<SongMatchData> songMatchDataList = new List<SongMatchData>((int)localAlbum.TrackCount);
            for (uint index = 0; index < localAlbum.TrackCount; ++index)
            {
                TrackMetadata track = localAlbum.GetTrack(index);
                GroupedList optionsForTrack = FindAlbumInfoHelper.GetOptionsForTrack(track, mergedAlbum, wmisAlbumTrackCount);
                int fromAlbumMetadata = FindAlbumInfoHelper.GetTrackIndexFromAlbumMetadata(mergedAlbum, track.MediaId);
                int selectedMatchIndex = (long)fromAlbumMetadata < (long)wmisAlbumTrackCount ? fromAlbumMetadata + 1 : -1;
                SongMatchData songMatchData = new SongMatchData(mergedAlbum, track, optionsForTrack, fromAlbumMetadata, selectedMatchIndex);
                songMatchDataList.Add(songMatchData);
            }
            return songMatchDataList;
        }

        private static int GetTrackIndexFromAlbumMetadata(AlbumMetadata albumMetadata, int mediaId)
        {
            for (uint index = 0; index < albumMetadata.TrackCount; ++index)
            {
                if (albumMetadata.GetTrack(index).MediaId == mediaId)
                    return (int)index;
            }
            return -1;
        }

        public static void SetAlbumTrackMediaId(
          AlbumMetadata albumMetadata,
          int trackIndex,
          int mediaId)
        {
            for (uint index = 0; index < albumMetadata.TrackCount; ++index)
            {
                TrackMetadata track = albumMetadata.GetTrack(index);
                if (track.MediaId == mediaId)
                {
                    track.MediaId = -1;
                    break;
                }
            }
            albumMetadata.GetTrack((uint)trackIndex).MediaId = mediaId;
        }

        private static GroupedList GetOptionsForTrack(
          TrackMetadata track,
          AlbumMetadata mergedAlbum,
          uint wmisAlbumTrackCount)
        {
            List<TrackOptionGroupItem> trackOptionGroupItemList = new List<TrackOptionGroupItem>((int)wmisAlbumTrackCount + 1);
            trackOptionGroupItemList.Add(new TrackOptionGroupItem()
            {
                TrackMetadata = track,
                Original = true
            });
            for (int index = 0; (long)index < (long)wmisAlbumTrackCount; ++index)
                trackOptionGroupItemList.Add(new TrackOptionGroupItem()
                {
                    TrackMetadata = mergedAlbum.GetTrack((uint)index),
                    Original = false
                });
            return new GroupedList()
            {
                Comparer = (IComparer)new TrackOptionsComparer(),
                Source = (IList)trackOptionGroupItemList
            };
        }
    }
}
