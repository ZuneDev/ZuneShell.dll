using Microsoft.Win32;

namespace Microsoft.Zune.Configuration;

public class MBRConfiguration : CConfigurationManagedBase
{
	public int MaxMissingOrCorruptedChunks
	{
		get
		{
			return GetIntProperty("MaxMissingOrCorruptedChunks", 10);
		}
		set
		{
			SetIntProperty("MaxMissingOrCorruptedChunks", value);
		}
	}

	public int ExcessiveBufferingPositionThresholdMs
	{
		get
		{
			return GetIntProperty("ExcessiveBufferingPositionThresholdMs", 7000);
		}
		set
		{
			SetIntProperty("ExcessiveBufferingPositionThresholdMs", value);
		}
	}

	public int ExcessiveBufferingMaxAttempts
	{
		get
		{
			return GetIntProperty("ExcessiveBufferingMaxAttempts", 3);
		}
		set
		{
			SetIntProperty("ExcessiveBufferingMaxAttempts", value);
		}
	}

	public int ExcessiveBufferingTimeoutMs
	{
		get
		{
			return GetIntProperty("ExcessiveBufferingTimeoutMs", 7000);
		}
		set
		{
			SetIntProperty("ExcessiveBufferingTimeoutMs", value);
		}
	}

	public int OpenTimeoutMs
	{
		get
		{
			return GetIntProperty("OpenTimeoutMs", 40000);
		}
		set
		{
			SetIntProperty("OpenTimeoutMs", value);
		}
	}

	internal MBRConfiguration(RegistryHive hive)
		: base(hive, null, "MBR")
	{
	}

	public MBRConfiguration(RegistryHive hive, string basePath, string instance)
		: base(hive, basePath, instance)
	{
	}
}
