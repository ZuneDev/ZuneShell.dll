using Microsoft.Win32;

namespace Microsoft.Zune.Configuration;

public class ChannelsConfiguration : CConfigurationManagedBase
{
	internal ChannelsConfiguration(RegistryHive hive)
		: base(hive, null, "Channels")
	{
	}

	public ChannelsConfiguration(RegistryHive hive, string basePath, string instance)
		: base(hive, basePath, instance)
	{
	}
}
