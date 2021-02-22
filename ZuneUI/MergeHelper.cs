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
            IList artistIds = new ArrayList(sourceArtists.Count);
            foreach (LibraryDataProviderListItem sourceArtist in sourceArtists)
                artistIds.Add((int)sourceArtist.GetProperty("LibraryId"));
            ZuneQueryList albumsByArtists = ZuneApplication.ZuneLibrary.GetAlbumsByArtists(artistIds, null);
            int num1 = (int)albumsByArtists.AddRef();
            uint count = (uint)albumsByArtists.Count;
            for (uint index = 0; index < count; ++index)
                albumsByArtists.SetFieldValue(index, 380U, targetArtist);
            int num2 = (int)albumsByArtists.Release();
            albumsByArtists.Dispose();
        }

        public static void MergeAlbumsToArtist(string targetArtist, IList sourceAlbums)
        {
            foreach (DataProviderObject sourceAlbum in sourceAlbums)
                sourceAlbum.SetProperty("ArtistName", targetArtist);
        }

        public static void MergeTracksToArtist(string targetArtist, IList sourceTracks)
        {
            foreach (DataProviderObject sourceTrack in sourceTracks)
                sourceTrack.SetProperty("AlbumArtistName", targetArtist);
        }

        public static void MergeAlbumsToAlbum(
          string targetAlbumTitle,
          string targetAlbumArtistName,
          IList sourceAlbums)
        {
            foreach (LibraryDataProviderListItem sourceAlbum in sourceAlbums)
            {
                sourceAlbum.SetProperty("ArtistName", targetAlbumArtistName);
                sourceAlbum.SetProperty("Title", targetAlbumTitle);
            }
        }

        public static void MergeTracksToAlbum(
          string targetAlbumTitle,
          string targetAlbumArtistName,
          IList sourceTracks)
        {
            foreach (LibraryDataProviderListItem sourceTrack in sourceTracks)
            {
                sourceTrack.SetProperty("AlbumArtistName", targetAlbumArtistName);
                sourceTrack.SetProperty("AlbumName", targetAlbumTitle);
            }
        }

        public static void MergeTracksToGenre(string targetGenre, IList sourceTracks)
        {
            foreach (DataProviderObject sourceTrack in sourceTracks)
                sourceTrack.SetProperty("Genre", targetGenre);
        }

        public static void MergeAlbumsToGenre(string targetGenre, IList sourceAlbums)
        {
            IList albumIds = new ArrayList(sourceAlbums.Count);
            foreach (LibraryDataProviderListItem sourceAlbum in sourceAlbums)
                albumIds.Add((int)sourceAlbum.GetProperty("LibraryId"));
            ZuneQueryList tracksByAlbums = ZuneApplication.ZuneLibrary.GetTracksByAlbums(albumIds, null);
            int num1 = (int)tracksByAlbums.AddRef();
            uint count = (uint)tracksByAlbums.Count;
            for (uint index = 0; index < count; ++index)
                tracksByAlbums.SetFieldValue(index, 398U, targetGenre);
            int num2 = (int)tracksByAlbums.Release();
            tracksByAlbums.Dispose();
        }

        public static void MergeGenresToGenre(string targetGenre, IList sourceGenres)
        {
            foreach (DataProviderObject sourceGenre in sourceGenres)
                sourceGenre.SetProperty("Title", targetGenre);
        }
    }
}
