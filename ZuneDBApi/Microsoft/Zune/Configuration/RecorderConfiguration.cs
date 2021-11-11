using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class RecorderConfiguration : CConfigurationManagedBase
	{
		public int AutoEjectCD
		{
			get
			{
				return GetIntProperty("AutoEjectCD", 0);
			}
			set
			{
				SetIntProperty("AutoEjectCD", value);
			}
		}

		public int AutoCopyCD
		{
			get
			{
				return GetIntProperty("AutoCopyCD", 0);
			}
			set
			{
				SetIntProperty("AutoCopyCD", value);
			}
		}

		public string PreferredCodecPath
		{
			get
			{
				return GetStringProperty("PreferredCodecPath", "");
			}
			set
			{
				SetStringProperty("PreferredCodecPath", value);
			}
		}

		public bool UseTrackNumberInFileName
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("UseTrackNumberInFileName", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("UseTrackNumberInFileName", value);
			}
		}

		public int WMARecordRate
		{
			get
			{
				return GetIntProperty("WMARecordRate", 192000);
			}
			set
			{
				SetIntProperty("WMARecordRate", value);
			}
		}

		public int WMAVBRRecordQuality
		{
			get
			{
				return GetIntProperty("WMAVBRRecordQuality", 75);
			}
			set
			{
				SetIntProperty("WMAVBRRecordQuality", value);
			}
		}

		public int MP3RecordRate
		{
			get
			{
				return GetIntProperty("MP3RecordRate", 192000);
			}
			set
			{
				SetIntProperty("MP3RecordRate", value);
			}
		}

		public int RecordMode
		{
			get
			{
				return GetIntProperty("RecordMode", 2);
			}
			set
			{
				SetIntProperty("RecordMode", value);
			}
		}

		internal RecorderConfiguration(RegistryHive hive)
			: base(hive, null, "Recorder")
		{
		}

		public RecorderConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
