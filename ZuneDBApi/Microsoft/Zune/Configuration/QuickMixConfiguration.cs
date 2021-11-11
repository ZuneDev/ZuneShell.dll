using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class QuickMixConfiguration : CConfigurationManagedBase
	{
		public bool AlwaysRefreshSmartDJ
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("AlwaysRefreshSmartDJ", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("AlwaysRefreshSmartDJ", value);
			}
		}

		public double AlgorithmThresholdAverage
		{
			get
			{
				return GetDoubleProperty("AlgorithmThresholdAverage", 0.2);
			}
			set
			{
				SetDoubleProperty("AlgorithmThresholdAverage", value);
			}
		}

		public byte[] AlgorithmThresholds
		{
			get
			{
				return GetBinaryProperty("AlgorithmThresholds");
			}
			set
			{
				SetBinaryProperty("AlgorithmThresholds", value);
			}
		}

		public byte[] AlgorithmWeights
		{
			get
			{
				return GetBinaryProperty("AlgorithmWeights");
			}
			set
			{
				SetBinaryProperty("AlgorithmWeights", value);
			}
		}

		public bool OnlyEnableItemsWithQuickMix
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("OnlyEnableItemsWithQuickMix", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("OnlyEnableItemsWithQuickMix", value);
			}
		}

		public bool RetrieveRelatedArtistsMetadata
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("RetrieveRelatedArtistsMetadata", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("RetrieveRelatedArtistsMetadata", value);
			}
		}

		public int DefaultPlaylistLength
		{
			get
			{
				return GetIntProperty("DefaultPlaylistLength", 30);
			}
			set
			{
				SetIntProperty("DefaultPlaylistLength", value);
			}
		}

		public int MinimumPlaylistLength
		{
			get
			{
				return GetIntProperty("MinimumPlaylistLength", 10);
			}
			set
			{
				SetIntProperty("MinimumPlaylistLength", value);
			}
		}

		public int InitializationLevel
		{
			get
			{
				return GetIntProperty("InitializationLevel", 0);
			}
			set
			{
				SetIntProperty("InitializationLevel", value);
			}
		}

		public bool ForceInitialize
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("ForceInitialize", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("ForceInitialize", value);
			}
		}

		public int DisableInLibraryCheck
		{
			get
			{
				return GetIntProperty("DisableInLibraryCheck", 0);
			}
			set
			{
				SetIntProperty("DisableInLibraryCheck", value);
			}
		}

		public bool Debug
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("Debug", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("Debug", value);
			}
		}

		public int CriticalMass
		{
			get
			{
				return GetIntProperty("CriticalMass", 80);
			}
			set
			{
				SetIntProperty("CriticalMass", value);
			}
		}

		internal QuickMixConfiguration(RegistryHive hive)
			: base(hive, null, "QuickMix")
		{
		}

		public QuickMixConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
