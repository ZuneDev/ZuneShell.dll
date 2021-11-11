using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class CDBurnConfiguration : CConfigurationManagedBase
	{
		public bool CheckDRM
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("CheckDRM", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("CheckDRM", value);
			}
		}

		public bool CDRecordJitterCorrect
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("CDRecordJitterCorrect", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("CDRecordJitterCorrect", value);
			}
		}

		public bool WasImapi2Checked
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("WasImapi2Checked", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("WasImapi2Checked", value);
			}
		}

		public bool DataCDCreateFolders
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("DataCDCreateFolders", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("DataCDCreateFolders", value);
			}
		}

		public int DataCDTranscodeMode
		{
			get
			{
				return GetIntProperty("DataCDTranscodeMode", -1);
			}
			set
			{
				SetIntProperty("DataCDTranscodeMode", value);
			}
		}

		public bool SimulatedBurn
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("SimulatedBurn", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("SimulatedBurn", value);
			}
		}

		public bool AutoEject
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("AutoEject", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("AutoEject", value);
			}
		}

		public int BurnSpeed
		{
			get
			{
				return GetIntProperty("BurnSpeed", 0);
			}
			set
			{
				SetIntProperty("BurnSpeed", value);
			}
		}

		public int MeasureErasing
		{
			get
			{
				return GetIntProperty("MeasureErasing", 20000);
			}
			set
			{
				SetIntProperty("MeasureErasing", value);
			}
		}

		public int MeasureAudioBurn
		{
			get
			{
				return GetIntProperty("MeasureAudioBurn", 600);
			}
			set
			{
				SetIntProperty("MeasureAudioBurn", value);
			}
		}

		public int MeasureDataBurn
		{
			get
			{
				return GetIntProperty("MeasureDataBurn", 1200);
			}
			set
			{
				SetIntProperty("MeasureDataBurn", value);
			}
		}

		public int MeasureClosingDisc
		{
			get
			{
				return GetIntProperty("MeasureClosingDisc", 10000);
			}
			set
			{
				SetIntProperty("MeasureClosingDisc", value);
			}
		}

		public int MeasureDataXcode
		{
			get
			{
				return GetIntProperty("MeasureDataXcode", 2000);
			}
			set
			{
				SetIntProperty("MeasureDataXcode", value);
			}
		}

		public int MeasureAudioXcode
		{
			get
			{
				return GetIntProperty("MeasureAudioXcode", 500);
			}
			set
			{
				SetIntProperty("MeasureAudioXcode", value);
			}
		}

		public int MeasureAudioTAO
		{
			get
			{
				return GetIntProperty("MeasureAudioTAO", 1100);
			}
			set
			{
				SetIntProperty("MeasureAudioTAO", value);
			}
		}

		public int DiscFormat
		{
			get
			{
				return GetIntProperty("DiscFormat", 0);
			}
			set
			{
				SetIntProperty("DiscFormat", value);
			}
		}

		internal CDBurnConfiguration(RegistryHive hive)
			: base(hive, null, "CDBurn")
		{
		}

		public CDBurnConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
