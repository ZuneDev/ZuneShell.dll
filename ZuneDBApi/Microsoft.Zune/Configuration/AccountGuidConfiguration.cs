using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
    public class AccountGuidConfiguration : CConfigurationManagedBase
    {
        public IList<string> RegisteredDevices
        {
            get { return GetStringListProperty("RegisteredDevices"); }
            set { SetStringListProperty("RegisteredDevices", value); }
        }

        public DateTime ReportUsageDataDueDate
        {
            get { return GetDateTimeProperty("ReportUsageDataDueDate", new DateTime(1999, 1, 1)); }
            set { SetDateTimeProperty("ReportUsageDataDueDate", value); }
        }

        public DateTime RefreshSubscriptionLicenseDueDate
        {
            get { return GetDateTimeProperty("RefreshSubscriptionLicenseDueDate", new DateTime(1999, 1, 1)); }
            set { SetDateTimeProperty("RefreshSubscriptionLicenseDueDate", value); }
        }

        internal AccountGuidConfiguration(RegistryHive hive) : base(hive, null, "AccountGuid") { }

        public AccountGuidConfiguration(RegistryHive hive, string basePath, string instance) : base(hive, basePath, instance) { }
    }
}