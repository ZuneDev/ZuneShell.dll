// Decompiled with JetBrains decompiler
// Type: ZuneUI.VideoDetails
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using MicrosoftZuneLibrary;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class VideoDetails
    {
        private static int[] ColumnIndexes = new int[15]
        {
      344,
      151,
      32,
      177,
      181,
      317,
      138,
      443,
      442,
      175,
      176,
      68,
      49,
      149,
      451
        };
        private static string[] DataProperties = new string[15]
        {
      "Title",
      "Duration",
      "Bitrate",
      "FileType",
      "FolderName",
      "FilePath",
      "ArtistName",
      "Width",
      "Height",
      "FileName",
      "FileSize",
      "Copyright",
      "CategoryId",
      "DrmState",
      "ZuneMediaId"
        };
        private static string _actorFormat = Shell.LoadString(StringId.IDS_ACTORS_SEPARATION_FORMAT);
        private static string _directorFormat = Shell.LoadString(StringId.IDS_DIRECTORS_SEPARATION_FORMAT);
        private static string _daysExpirationFormat = Shell.LoadString(StringId.IDS_VIDEO_EXPIRATION_DAYS);
        private static string _hoursExpirationFormat = Shell.LoadString(StringId.IDS_VIDEO_EXPIRATION_HOURS);
        private static string _deviceRentalFormat = Shell.LoadString(StringId.IDS_VIDEO_DEVICE_RENTAL_FORMAT);

        public static string ActorListToString(IList artists) => ContributingArtistListToString(artists, _actorFormat);

        public static string DirectorListToString(IList directors) => ContributingArtistListToString(directors, _directorFormat);

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

        public static IList DeviceRentalStatusList(Guid zuneMediaId)
        {
            IList list = new List<string>();
            foreach (UIDevice uiDevice in SingletonModelItem<UIDeviceList>.Instance)
            {
                if (ZuneApplication.Service2.InCompleteCollection(zuneMediaId, EContentType.Video, uiDevice.EndpointId))
                    list.Add(string.Format(_deviceRentalFormat, uiDevice.Name.ToUpper()));
            }
            return list;
        }

        public static string PCExpirationToString(
          int drmState,
          string fileName,
          int fileType,
          Guid zuneMediaId)
        {
            string str = "";
            switch (drmState)
            {
                case 20:
                    str = Shell.LoadString(StringId.IDS_VIDEO_EXPIRED);
                    break;
                case 26:
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        DRMInfo drmInfo = fileType != 43 ? ZuneApplication.Service2.GetFileDRMInfo(fileName) : ZuneApplication.Service2.GetMediaDRMInfo(zuneMediaId, EContentType.Video);
                        if (drmInfo != null)
                        {
                            if (drmInfo.ValidLicense && drmInfo.HasExpiryDate)
                            {
                                DateTime localTime = DateTime.UtcNow.ToLocalTime();
                                if (drmInfo.ExpiryDate.CompareTo(localTime) > 0)
                                {
                                    TimeSpan timeSpan = drmInfo.ExpiryDate.Subtract(localTime);
                                    str = timeSpan.TotalHours < 49.0 ? (timeSpan.TotalHours < 2.0 ? (timeSpan.TotalHours < 1.0 ? Shell.LoadString(StringId.IDS_VIDEO_EXPIRATION_MINUTES) : Shell.LoadString(StringId.IDS_VIDEO_EXPIRATION_ONEHOUR)) : string.Format(_hoursExpirationFormat, (int)timeSpan.TotalHours)) : string.Format(_daysExpirationFormat, timeSpan.Days);
                                    break;
                                }
                                str = Shell.LoadString(StringId.IDS_VIDEO_EXPIRED);
                                break;
                            }
                            if (drmInfo.LicenseExpired)
                            {
                                str = Shell.LoadString(StringId.IDS_VIDEO_EXPIRED);
                                break;
                            }
                            break;
                        }
                        break;
                    }
                    break;
            }
            return str;
        }

        public static void Populate(object dataContainer, int libraryId)
        {
            DataProviderObject dataProviderObject = (DataProviderObject)dataContainer;
            object[] fieldValues = new object[15]
            {
         string.Empty,
         TimeSpan.Zero,
         0,
         0,
         string.Empty,
         string.Empty,
         string.Empty,
         0,
         0,
         string.Empty,
         0L,
         string.Empty,
         0,
         0,
         Guid.Empty
            };
            ZuneLibrary.GetFieldValues(libraryId, EListType.eVideoList, ColumnIndexes.Length, ColumnIndexes, fieldValues, PlaylistManager.Instance.QueryContext);
            for (int index = 0; index < ColumnIndexes.Length; ++index)
            {
                if (ColumnIndexes[index] == 177)
                    dataProviderObject.SetProperty("MediaType", MediaDescriptions.Map((MediaType)fieldValues[index]));
                dataProviderObject.SetProperty(DataProperties[index], fieldValues[index]);
            }
        }
    }
}
