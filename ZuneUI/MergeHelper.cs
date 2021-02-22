// Decompiled with JetBrains decompiler
// Type: ZuneUI.MergeHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Shell;
using MicrosoftZuneLibrary;
using System.Collections;

namespace ZuneUI
{
    public class MergeHelper
    {
        public static void MergeArtistsToArtist(string targetArtist, IList sourceArtists)
        {
            IList artistIds = (IList)new ArrayList(sourceArtists.Count);
            foreach (LibraryDataProviderListItem sourceArtist in (IEnumerable)sourceArtists)
                artistIds.Add((object)(int)sourceArtist.GetProperty("LibraryId"));
            ZuneQueryList albumsByArtists = ZuneApplication.ZuneLibrary.GetAlbumsByArtists(artistIds, (string)null);
            int num1 = (int)albumsByArtists.AddRef();
            uint count = (uint)albumsByArtists.Count;
            for (uint index = 0; index < count; ++index)
                albumsByArtists.SetFieldValue(index, 380U, (object)targetArtist);
            int num2 = (int)albumsByArtists.Release();
            albumsByArtists.Dispose();
        }

        public static void MergeAlbumsToArtist(string targetArtist, IList sourceAlbums)
        {
            foreach (DataProviderObject sourceAlbum in (IEnumerable)sourceAlbums)
                sourceAlbum.SetProperty("ArtistName", (object)targetArtist);
        }

        public static void MergeTracksToArtist(string targetArtist, IList sourceTracks)
        {
            foreach (DataProviderObject sourceTrack in (IEnumerable)sourceTracks)
                sourceTrack.SetProperty("AlbumArtistName", (object)targetArtist);
        }

        public static void MergeAlbumsToAlbum(
          string targetAlbumTitle,
          string targetAlbumArtistName,
          IList sourceAlbums)
        {
            foreach (LibraryDataProviderListItem sourceAlbum in (IEnumerable)sourceAlbums)
            {
                sourceAlbum.SetProperty("ArtistName", (object)targetAlbumArtistName);
                sourceAlbum.SetProperty("Title", (object)targetAlbumTitle);
            }
        }

        public static void MergeTracksToAlbum(
          string targetAlbumTitle,
          string targetAlbumArtistName,
          IList sourceTracks)
        {
            foreach (LibraryDataProviderListItem sourceTrack in (IEnumerable)sourceTracks)
            {
                sourceTrack.SetProperty("AlbumArtistName", (object)targetAlbumArtistName);
                sourceTrack.SetProperty("AlbumName", (object)targetAlbumTitle);
            }
        }

        public static void MergeTracksToGenre(string targetGenre, IList sourceTracks)
        {
            foreach (DataProviderObject sourceTrack in (IEnumerable)sourceTracks)
                sourceTrack.SetProperty("Genre", (object)targetGenre);
        }

        public static void MergeAlbumsToGenre(string targetGenre, IList sourceAlbums)
        {
            IList albumIds = (IList)new ArrayList(sourceAlbums.Count);
            foreach (LibraryDataProviderListItem sourceAlbum in (IEnumerable)sourceAlbums)
                albumIds.Add((object)(int)sourceAlbum.GetProperty("LibraryId"));
            ZuneQueryList tracksByAlbums = ZuneApplication.ZuneLibrary.GetTracksByAlbums(albumIds, (string)null);
            int num1 = (int)tracksByAlbums.AddRef();
            uint count = (uint)tracksByAlbums.Count;
            for (uint index = 0; index < count; ++index)
                tracksByAlbums.SetFieldValue(index, 398U, (object)targetGenre);
            int num2 = (int)tracksByAlbums.Release();
            tracksByAlbums.Dispose();
        }

        public static void MergeGenresToGenre(string targetGenre, IList sourceGenres)
        {
            foreach (DataProviderObject sourceGenre in (IEnumerable)sourceGenres)
                sourceGenre.SetProperty("Title", (object)targetGenre);
        }
    }
}
