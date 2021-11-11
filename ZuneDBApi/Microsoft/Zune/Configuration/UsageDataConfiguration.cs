using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class UsageDataConfiguration : CConfigurationManagedBase
	{
		internal UsageDataConfiguration(RegistryHive hive)
			: base(hive, null, "UsageData")
		{
		}

		public UsageDataConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
