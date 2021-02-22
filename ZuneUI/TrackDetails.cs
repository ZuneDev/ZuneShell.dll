// Decompiled with JetBrains decompiler
// Type: ZuneUI.TrackDetails
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;
using System;
using System.Collections;

namespace ZuneUI
{
    public class TrackDetails
    {
        private static int[] ColumnIndexes = new int[19]
        {
      344,
      151,
      32,
      177,
      181,
      317,
      138,
      65,
      382,
      380,
      292,
      135,
      437,
      389,
      390,
      398,
      175,
      176,
      68
        };
        private static string[] DataProperties = new string[19]
        {
      "Title",
      "Duration",
      "Bitrate",
      "MediaType",
      "FolderName",
      "FilePath",
      "ArtistName",
      "ContributingArtistNames",
      "AlbumName",
      "AlbumArtistName",
      "ReleaseDate",
      "DiscNumber",
      "TrackNumber",
      "ComposerName",
      "ConductorName",
      "Genre",
      "FileName",
      "FileSize",
      "Copyright"
        };
        private static string _contributingArtistFormat = Shell.LoadString(StringId.IDS_CONTRIBUTING_ARTISTS_SEPARATION_FORMAT);
        private static string _contributingArtistSeperator = Shell.LoadString(StringId.IDS_CONTRIBUTING_ARTISTS_SEPARATOR);

        public static void Populate(object dataContainer, int libraryId)
        {
            DataProviderObject dataProviderObject = (DataProviderObject)dataContainer;
            object[] fieldValues = new object[19]
            {
         string.Empty,
         TimeSpan.Zero,
         0,
         0,
         string.Empty,
         string.Empty,
         string.Empty,
         new ArrayList(),
         string.Empty,
         string.Empty,
         DateTime.MinValue,
         0,
         0,
         string.Empty,
         string.Empty,
         string.Empty,
         string.Empty,
         0L,
         string.Empty
            };
            bool[] isEmptyValues = new bool[fieldValues.Length];
            ZuneLibrary.GetFieldValues(libraryId, EListType.eTrackList, ColumnIndexes.Length, ColumnIndexes, fieldValues, isEmptyValues, PlaylistManager.Instance.QueryContext);
            for (int index = 0; index < ColumnIndexes.Length; ++index)
            {
                if (ColumnIndexes[index] == 177)
                    fieldValues[index] = MediaDescriptions.Map((MediaType)fieldValues[index]);
                dataProviderObject.SetProperty(DataProperties[index], fieldValues[index]);
            }
        }

        public static string GetGenreHelper(DataProviderObject item) => PlaylistManager.GetFieldValue((int)item.GetProperty("LibraryId"), EListType.eTrackList, 398, "");

        public static void SetGenreHelper(DataProviderObject item, string genre) => PlaylistManager.SetFieldValue((int)item.GetProperty("LibraryId"), EListType.eTrackList, 398, genre);

        public static Guid GetServiceId(int mediaId) => PlaylistManager.GetFieldValue(mediaId, EListType.eTrackList, 451, Guid.Empty);

        public static string ContributingArtistListToString(IList artists) => ContributingArtistListToString(artists, _contributingArtistFormat);

        private static string ContributingArtistListToString(IList artists, string format)
        {
            string str = "";
            if (artists != null)
            {
                foreach (string artist in artists)
                    str = str.Length != 0 ? string.Format(format, str, artist) : artist;
            }
            return str;
        }

        public static IList ContributingArtistStringToList(string contributingArtists) => ContributingArtistStringToList(contributingArtists, _contributingArtistSeperator);

        private static IList ContributingArtistStringToList(
          string contributingArtists,
          string separator)
        {
            ArrayList arrayList = null;
            foreach (string str1 in contributingArtists.Split(separator.ToCharArray(0, 1)))
            {
                string str2 = str1.Trim();
                if (str2.Length > 0)
                {
                    if (arrayList == null)
                        arrayList = new ArrayList();
                    arrayList.Add(str2);
                }
            }
            return arrayList;
        }
    }
}
