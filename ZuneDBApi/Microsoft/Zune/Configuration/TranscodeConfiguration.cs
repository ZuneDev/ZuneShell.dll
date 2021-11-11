using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class TranscodeConfiguration : CConfigurationManagedBase
	{
		public int AudioTargetBitRate
		{
			get
			{
				return GetIntProperty("AudioTargetBitRate", 192);
			}
			set
			{
				SetIntProperty("AudioTargetBitRate", value);
			}
		}

		public int AudioThresholdBitRate
		{
			get
			{
				return GetIntProperty("AudioThresholdBitRate", 320);
			}
			set
			{
				SetIntProperty("AudioThresholdBitRate", value);
			}
		}

		public string TranscodedFilesCachePath
		{
			get
			{
				return GetStringProperty("TranscodedFilesCachePath", "");
			}
			set
			{
				SetStringProperty("TranscodedFilesCachePath", value);
			}
		}

		public int TranscodedVideoComplexity
		{
			get
			{
				return GetIntProperty("TranscodedVideoComplexity", 20);
			}
			set
			{
				SetIntProperty("TranscodedVideoComplexity", value);
			}
		}

		public int TranscodedFilesCacheSize
		{
			get
			{
				return GetIntProperty("TranscodedFilesCacheSize", 2048);
			}
			set
			{
				SetIntProperty("TranscodedFilesCacheSize", value);
			}
		}

		public int ThumbnailSize
		{
			get
			{
				return GetIntProperty("ThumbnailSize", 240);
			}
			set
			{
				SetIntProperty("ThumbnailSize", value);
			}
		}

		public int MinimumTranscodeFPTS
		{
			get
			{
				return GetIntProperty("MinimumTranscodeFPTS", 1000);
			}
			set
			{
				SetIntProperty("MinimumTranscodeFPTS", value);
			}
		}

		public int DiskFreeSpaceBuffer
		{
			get
			{
				return GetIntProperty("DiskFreeSpaceBuffer", 200);
			}
			set
			{
				SetIntProperty("DiskFreeSpaceBuffer", value);
			}
		}

		public int DefaultBitsPerSample
		{
			get
			{
				return GetIntProperty("DefaultBitsPerSample", 16);
			}
			set
			{
				SetIntProperty("DefaultBitsPerSample", value);
			}
		}

		public int DefaultTranscodeChannels
		{
			get
			{
				return GetIntProperty("DefaultTranscodeChannels", 2);
			}
			set
			{
				SetIntProperty("DefaultTranscodeChannels", value);
			}
		}

		public int DefaultTranscodeAudioFormatTag
		{
			get
			{
				return GetIntProperty("DefaultTranscodeAudioFormatTag", 353);
			}
			set
			{
				SetIntProperty("DefaultTranscodeAudioFormatTag", value);
			}
		}

		public int DefaultTranscodeHeight
		{
			get
			{
				return GetIntProperty("DefaultTranscodeHeight", 30000000);
			}
			set
			{
				SetIntProperty("DefaultTranscodeHeight", value);
			}
		}

		public int DefaultTranscodeWidth
		{
			get
			{
				return GetIntProperty("DefaultTranscodeWidth", 40000000);
			}
			set
			{
				SetIntProperty("DefaultTranscodeWidth", value);
			}
		}

		public int BackgroundTranscodeRepeatTimeout
		{
			get
			{
				return GetIntProperty("BackgroundTranscodeRepeatTimeout", 180000);
			}
			set
			{
				SetIntProperty("BackgroundTranscodeRepeatTimeout", value);
			}
		}

		public bool BackgroundTranscode
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("BackgroundTranscode", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("BackgroundTranscode", value);
			}
		}

		public bool PhotoTranscodeUseWIC
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("PhotoTranscodeUseWIC", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("PhotoTranscodeUseWIC", value);
			}
		}

		public int DefaultTranscodeFourCC
		{
			get
			{
				return GetIntProperty("DefaultTranscodeFourCC", 861293911);
			}
			set
			{
				SetIntProperty("DefaultTranscodeFourCC", value);
			}
		}

		public int DefaultTranscodeSecPerKey
		{
			get
			{
				return GetIntProperty("DefaultTranscodeSecPerKey", 4);
			}
			set
			{
				SetIntProperty("DefaultTranscodeSecPerKey", value);
			}
		}

		public int DefaultTranscodeQuality
		{
			get
			{
				return GetIntProperty("DefaultTranscodeQuality", 50);
			}
			set
			{
				SetIntProperty("DefaultTranscodeQuality", value);
			}
		}

		public int AudioBitrateThreshold
		{
			get
			{
				return GetIntProperty("AudioBitrateThreshold", 64000);
			}
			set
			{
				SetIntProperty("AudioBitrateThreshold", value);
			}
		}

		public bool TraceMatchParameter
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("TraceMatchParameter", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("TraceMatchParameter", value);
			}
		}

		public bool DumpTranscodeProfile
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("DumpTranscodeProfile", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("DumpTranscodeProfile", value);
			}
		}

		public bool DumpGetTranscodeProfile
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("DumpGetTranscodeProfile", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("DumpGetTranscodeProfile", value);
			}
		}

		public bool DisableFileConverterRecovery
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("DisableFileConverterRecovery", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("DisableFileConverterRecovery", value);
			}
		}

		public bool DeinterlaceVideo
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("DeinterlaceVideo", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("DeinterlaceVideo", value);
			}
		}

		internal TranscodeConfiguration(RegistryHive hive)
			: base(hive, null, "Transcode")
		{
		}

		public TranscodeConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
