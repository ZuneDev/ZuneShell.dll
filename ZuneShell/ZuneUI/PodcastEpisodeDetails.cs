// Decompiled with JetBrains decompiler
// Type: ZuneUI.PodcastEpisodeDetails
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;
using System;

namespace ZuneUI
{
    public class PodcastEpisodeDetails
    {
        private static int[] ColumnIndexes = new int[14]
        {
      344,
      312,
      172,
      24,
      292,
      151,
      177,
      317,
      222,
      175,
      181,
      176,
      32,
      68
        };
        private static string[] DataProperties = new string[14]
        {
      "Title",
      "SeriesTitle",
      "SeriesFeedUrl",
      "Author",
      "ReleaseDate",
      "Duration",
      "MediaType",
      "SourceUrl",
      "EnclosureUrl",
      "FileName",
      "FolderName",
      "FileSize",
      "Bitrate",
      "Copyright"
        };

        public static void Populate(object dataContainer, int libraryId)
        {
            DataProviderObject dataProviderObject = (DataProviderObject)dataContainer;
            object[] fieldValues = new object[14]
            {
         string.Empty,
         string.Empty,
         string.Empty,
         string.Empty,
         DateTime.MinValue,
         TimeSpan.Zero,
         EMediaTypes.eMediaTypeAudio,
         string.Empty,
         string.Empty,
         string.Empty,
         string.Empty,
         0L,
         0,
         string.Empty
            };
            ZuneLibrary.GetFieldValues(libraryId, EListType.ePodcastEpisodeList, ColumnIndexes.Length, ColumnIndexes, fieldValues, PlaylistManager.Instance.QueryContext);
            for (int index = 0; index < ColumnIndexes.Length; ++index)
            {
                if (ColumnIndexes[index] == 177)
                    fieldValues[index] = MediaDescriptions.Map((MediaType)fieldValues[index]);
                dataProviderObject.SetProperty(DataProperties[index], fieldValues[index]);
            }
        }
    }
}
