using System;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
    public class SocialUserGuidConfiguration : CConfigurationManagedBase
    {
        public DateTime ProfileCommentsLastRead
        {
            get { return GetDateTimeProperty("ProfileCommentsLastRead", new DateTime(1999, 1, 1)); }
            set { SetDateTimeProperty("ProfileCommentsLastRead", value); }
        }

        public DateTime ProfilePlayCountUpdated
        {
            get { return GetDateTimeProperty("ProfilePlayCountUpdated", new DateTime(1999, 1, 1)); }
            set { SetDateTimeProperty("ProfilePlayCountUpdated", value); }
        }

        public int ProfilePlayCount
        {
            get { return GetIntProperty("ProfilePlayCount", -1); }
            set { SetIntProperty("ProfilePlayCount", value); }
        }

        internal SocialUserGuidConfiguration(RegistryHive hive) : base(hive, null, "SocialUserGuid") { }

        public SocialUserGuidConfiguration(RegistryHive hive, string basePath, string instance) : base(hive, basePath, instance) { }
    }
}