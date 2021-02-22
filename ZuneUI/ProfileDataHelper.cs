// Decompiled with JetBrains decompiler
// Type: ZuneUI.ProfileDataHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Win32;
using Microsoft.Zune.Configuration;
using System;

namespace ZuneUI
{
    public static class ProfileDataHelper
    {
        private const int c_updateServicePlayCountHours = 2;

        public static int ProfilePlayCount
        {
            get => GetSetPlayCountCache(-1);
            set => GetSetPlayCountCache(value);
        }

        public static DateTime CommentsLastRead
        {
            get => GetCommentsLastRead();
            set => SetCommentsLastRead(value);
        }

        private static SocialUserGuidConfiguration GetSocialUserGuidConfiguration(
          string userGuid)
        {
            return new SocialUserGuidConfiguration(RegistryHive.CurrentUser, ClientConfiguration.Social.ConfigurationPath, userGuid);
        }

        public static void InitializePlayCountCache() => GetSetPlayCountCache(ClientConfiguration.Service.LastSignedInUserGuid, -1, true);

        private static int GetSetPlayCountCache(int newValue) => GetSetPlayCountCache(ClientConfiguration.Service.LastSignedInUserGuid, newValue, false);

        private static int GetSetPlayCountCache(string userGuid, int newValue, bool clearStaleCache)
        {
            int num = -1;
            if (!string.IsNullOrEmpty(userGuid) && userGuid != Guid.Empty.ToString())
            {
                SocialUserGuidConfiguration guidConfiguration = GetSocialUserGuidConfiguration(userGuid);
                num = guidConfiguration.ProfilePlayCount;
                bool flag = false;
                if (num < newValue)
                    flag = true;
                else if (clearStaleCache)
                    flag = guidConfiguration.ProfilePlayCountUpdated.AddHours(2.0) <= DateTime.UtcNow;
                if (flag)
                {
                    guidConfiguration.ProfilePlayCount = newValue;
                    num = newValue;
                    if (clearStaleCache)
                        guidConfiguration.ProfilePlayCountUpdated = DateTime.UtcNow;
                }
            }
            return num;
        }

        private static DateTime GetCommentsLastRead() => GetCommentsLastRead(ClientConfiguration.Service.LastSignedInUserGuid);

        private static DateTime GetCommentsLastRead(string userGuid)
        {
            DateTime dateTime = DateTime.MinValue;
            if (!string.IsNullOrEmpty(userGuid) && userGuid != Guid.Empty.ToString())
                dateTime = GetSocialUserGuidConfiguration(userGuid).ProfileCommentsLastRead;
            return dateTime;
        }

        private static void SetCommentsLastRead(DateTime commentsLastRead) => SetCommentsLastRead(ClientConfiguration.Service.LastSignedInUserGuid, commentsLastRead);

        private static void SetCommentsLastRead(string userGuid, DateTime commentsLastRead)
        {
            if (string.IsNullOrEmpty(userGuid) || !(userGuid != Guid.Empty.ToString()))
                return;
            GetSocialUserGuidConfiguration(userGuid).ProfileCommentsLastRead = commentsLastRead;
        }
    }
}
