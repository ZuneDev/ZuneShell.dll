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
        private static string _actorFormat = ZuneUI.Shell.LoadString(StringId.IDS_ACTORS_SEPARATION_FORMAT);
        private static string _directorFormat = ZuneUI.Shell.LoadString(StringId.IDS_DIRECTORS_SEPARATION_FORMAT);
        private static string _daysExpirationFormat = ZuneUI.Shell.LoadString(StringId.IDS_VIDEO_EXPIRATION_DAYS);
        private static string _hoursExpirationFormat = ZuneUI.Shell.LoadString(StringId.IDS_VIDEO_EXPIRATION_HOURS);
        private static string _deviceRentalFormat = ZuneUI.Shell.LoadString(StringId.IDS_VIDEO_DEVICE_RENTAL_FORMAT);

        public static string ActorListToString(IList artists) => VideoDetails.ContributingArtistListToString(artists, VideoDetails._actorFormat);

        public static string DirectorListToString(IList directors) => VideoDetails.ContributingArtistListToString(directors, VideoDetails._directorFormat);

        private static string ContributingArtistListToString(IList artists, string format)
        {
            string str = "";
            if (artists != null)
            {
                foreach (string artist in (IEnumerable)artists)
                    str = str.Length != 0 ? string.Format(format, (object)str, (object)artist) : artist;
            }
            return str;
        }

        public static IList DeviceRentalStatusList(Guid zuneMediaId)
        {
            IList list = (IList)new List<string>();
            foreach (UIDevice uiDevice in SingletonModelItem<UIDeviceList>.Instance)
            {
                if (ZuneApplication.Service.InCompleteCollection(zuneMediaId, Microsoft.Zune.Service.EContentType.Video, uiDevice.EndpointId))
                    list.Add((object)string.Format(VideoDetails._deviceRentalFormat, (object)uiDevice.Name.ToUpper()));
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
                    str = ZuneUI.Shell.LoadString(StringId.IDS_VIDEO_EXPIRED);
                    break;
                case 26:
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        DRMInfo drmInfo = fileType != 43 ? ZuneApplication.Service.GetFileDRMInfo(fileName) : ZuneApplication.Service.GetMediaDRMInfo(zuneMediaId, Microsoft.Zune.Service.EContentType.Video);
                        if (drmInfo != null)
                        {
                            if (drmInfo.ValidLicense && drmInfo.HasExpiryDate)
                            {
                                DateTime localTime = DateTime.UtcNow.ToLocalTime();
                                if (drmInfo.ExpiryDate.CompareTo(localTime) > 0)
                                {
                                    TimeSpan timeSpan = drmInfo.ExpiryDate.Subtract(localTime);
                                    str = timeSpan.TotalHours < 49.0 ? (timeSpan.TotalHours < 2.0 ? (timeSpan.TotalHours < 1.0 ? ZuneUI.Shell.LoadString(StringId.IDS_VIDEO_EXPIRATION_MINUTES) : ZuneUI.Shell.LoadString(StringId.IDS_VIDEO_EXPIRATION_ONEHOUR)) : string.Format(VideoDetails._hoursExpirationFormat, (object)(int)timeSpan.TotalHours)) : string.Format(VideoDetails._daysExpirationFormat, (object)timeSpan.Days);
                                    break;
                                }
                                str = ZuneUI.Shell.LoadString(StringId.IDS_VIDEO_EXPIRED);
                                break;
                            }
                            if (drmInfo.LicenseExpired)
                            {
                                str = ZuneUI.Shell.LoadString(StringId.IDS_VIDEO_EXPIRED);
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
        (object) string.Empty,
        (object) TimeSpan.Zero,
        (object) 0,
        (object) 0,
        (object) string.Empty,
        (object) string.Empty,
        (object) string.Empty,
        (object) 0,
        (object) 0,
        (object) string.Empty,
        (object) 0L,
        (object) string.Empty,
        (object) 0,
        (object) 0,
        (object) Guid.Empty
            };
            ZuneLibrary.GetFieldValues(libraryId, EListType.eVideoList, VideoDetails.ColumnIndexes.Length, VideoDetails.ColumnIndexes, fieldValues, PlaylistManager.Instance.QueryContext);
            for (int index = 0; index < VideoDetails.ColumnIndexes.Length; ++index)
            {
                if (VideoDetails.ColumnIndexes[index] == 177)
                    dataProviderObject.SetProperty("MediaType", (object)MediaDescriptions.Map((MediaType)fieldValues[index]));
                dataProviderObject.SetProperty(VideoDetails.DataProperties[index], fieldValues[index]);
            }
        }
    }
}
