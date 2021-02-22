// Decompiled with JetBrains decompiler
// Type: ZuneUI.RecommendationsHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Win32;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using System;
using System.Collections;
using System.Collections.Generic;
using ZuneXml;

namespace ZuneUI
{
    public class RecommendationsHelper
    {
        private static List<int> _uploadedArtistList;
        private static int _maxSongsForYouTrackCount;

        public static bool IncludeTrackCollection(
          ICollection tracks,
          TrackCollectionFilterType filterType)
        {
            bool flag1 = false;
            if (tracks != null)
            {
                foreach (Track track in tracks)
                {
                    if (!(track.Id == Guid.Empty))
                    {
                        bool flag2 = ZuneApplication.Service.InVisibleCollection(track.Id, EContentType.MusicTrack);
                        if (filterType == TrackCollectionFilterType.IncludeIfAnyTrackNotInCollection)
                        {
                            if (!flag2)
                            {
                                flag1 = true;
                                break;
                            }
                        }
                        else
                        {
                            if (flag2)
                            {
                                flag1 = false;
                                break;
                            }
                            flag1 = true;
                        }
                    }
                }
            }
            return flag1;
        }

        public static bool IsArtistInCollection(Guid artistId)
        {
            bool flag = false;
            if (artistId != Guid.Empty)
                flag = ZuneApplication.Service.InVisibleCollection(artistId, EContentType.Artist);
            return flag;
        }

        public static object GetKeyValue(IDictionary dictionary, object key)
        {
            object obj = null;
            if (dictionary != null && dictionary.Contains(key))
                obj = dictionary[key];
            return obj;
        }

        public static int GetKeyValueInt(IDictionary dictionary, object key, int defaultValue)
        {
            int num = defaultValue;
            if (dictionary != null && dictionary.Contains(key) && dictionary[key] is int val)
                num = val;
            return num;
        }

        public static DateTime RecommendationsDate
        {
            get => ServiceUserGuidConfiguration.RecommendationsDate.ToUniversalTime();
            set => ServiceUserGuidConfiguration.RecommendationsDate = value;
        }

        public static DateTime LastRecommendationsShuffleDate
        {
            get => ServiceUserGuidConfiguration.RecommendationsRefreshDate.ToUniversalTime();
            set => ServiceUserGuidConfiguration.RecommendationsRefreshDate = value;
        }

        public static TimeSpan RefreshPeriod => new TimeSpan(24, 0, 0);

        public static int ShuffleSeed
        {
            get => ServiceUserGuidConfiguration.Seed;
            set => ServiceUserGuidConfiguration.Seed = value;
        }

        public static int MaxSongsForYouTrackCount
        {
            get
            {
                if (_maxSongsForYouTrackCount == 0)
                    _maxSongsForYouTrackCount = ClientConfiguration.Service.RecommendationsMaxTrackCount;
                return _maxSongsForYouTrackCount;
            }
        }

        public static ServiceUserGuidConfiguration ServiceUserGuidConfiguration => new ServiceUserGuidConfiguration(RegistryHive.CurrentUser, ClientConfiguration.Service.ConfigurationPath, string.Format("{0}", ClientConfiguration.Service.LastSignedInUserGuid));

        public static int GetUserRating(DataProviderObject item)
        {
            int num = 0;
            if (item is Track)
            {
                Guid id = ((MiniMedia)item).Id;
                if (id != Guid.Empty)
                {
                    int piRating = 0;
                    if (ZuneApplication.Service.GetUserRating(SignIn.Instance.LastSignedInUserId, id, EContentType.MusicTrack, ref piRating))
                        num = piRating;
                }
            }
            return num;
        }

        public static bool SetUserRating(DataProviderObject item, string propertyName, int rating)
        {
            bool flag = false;
            if (item is Track)
            {
                Track track = (Track)item;
                flag = ZuneApplication.Service.SetUserTrackRating(SignIn.Instance.LastSignedInUserId, rating, track.Id, track.AlbumId, track.TrackNumber, track.Title, track.Duration.Milliseconds, track.AlbumTitle, track.Artist, track.PrimaryGenre.Title, (string)track.GetProperty("ReferrerContext"));
                if (flag)
                    track.UserRating = rating;
            }
            return flag;
        }

        public static bool SetArtistUserRating(Guid id, string name, int rating) => ZuneApplication.Service.SetUserArtistRating(SignIn.Instance.LastSignedInUserId, rating, id, name);

        public static string GetRecommendationsEndpoint() => Service.GetEndPointUri(EServiceEndpointId.SEID_Recommendations);

        private static List<int> GetUploadedArtistUserList()
        {
            if (_uploadedArtistList != null)
                return _uploadedArtistList;
            string haveUploadedArtists = ClientConfiguration.Picks.UsersWhoHaveUploadedArtists;
            if (!string.IsNullOrEmpty(haveUploadedArtists))
            {
                string[] strArray = haveUploadedArtists.Split(',');
                _uploadedArtistList = new List<int>(strArray.Length);
                foreach (string str in strArray)
                {
                    int result;
                    if (!string.IsNullOrEmpty(str) && int.TryParse(str.Trim(), out result))
                        _uploadedArtistList.Add(result);
                }
            }
            return _uploadedArtistList;
        }

        private static void AddUserToArtistUploadList(int userId)
        {
            if (_uploadedArtistList == null)
                _uploadedArtistList = new List<int>();
            foreach (int uploadedArtist in _uploadedArtistList)
            {
                if (uploadedArtist == userId)
                    return;
            }
            _uploadedArtistList.Add(userId);
        }

        private static void SaveArtistUserListToRegistry()
        {
            string str = string.Empty;
            foreach (int uploadedArtist in _uploadedArtistList)
                str = !string.IsNullOrEmpty(str) ? string.Format("{0},{1}", str, uploadedArtist.ToString()) : uploadedArtist.ToString();
            ClientConfiguration.Picks.UsersWhoHaveUploadedArtists = str;
        }

        public static bool HasUserUploadedArtists(int userId)
        {
            List<int> uploadedArtistUserList = GetUploadedArtistUserList();
            if (uploadedArtistUserList != null)
            {
                foreach (int num in uploadedArtistUserList)
                {
                    if (num == userId)
                        return true;
                }
            }
            return false;
        }

        public static void UserHasUploadArtists(int userId)
        {
            AddUserToArtistUploadList(userId);
            SaveArtistUserListToRegistry();
        }
    }
}
