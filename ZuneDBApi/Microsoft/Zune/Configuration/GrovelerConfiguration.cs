using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class GrovelerConfiguration : CConfigurationManagedBase
	{
		public int LibrarySync
		{
			get
			{
				return GetIntProperty("LibrarySync", 0);
			}
			set
			{
				SetIntProperty("LibrarySync", value);
			}
		}

		public int Configuration
		{
			get
			{
				return GetIntProperty("Configuration", 1);
			}
			set
			{
				SetIntProperty("Configuration", value);
			}
		}

		public bool AllowDVRMSOnMediaCenter
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("AllowDVRMSOnMediaCenter", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("AllowDVRMSOnMediaCenter", value);
			}
		}

		public bool AllowDiscMedia
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("AllowDiscMedia", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("AllowDiscMedia", value);
			}
		}

		public bool ScanRemovedFiles
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("ScanRemovedFiles", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("ScanRemovedFiles", value);
			}
		}

		public bool LegacyImportComplete
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("LegacyImportComplete", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("LegacyImportComplete", value);
			}
		}

		public string ApplicationsDownloadFolder
		{
			get
			{
				return GetStringProperty("ApplicationsDownloadFolder", "");
			}
			set
			{
				SetStringProperty("ApplicationsDownloadFolder", value);
			}
		}

		public string PodcastMediaFolder
		{
			get
			{
				return GetStringProperty("PodcastMediaFolder", "");
			}
			set
			{
				SetStringProperty("PodcastMediaFolder", value);
			}
		}

		public string PhotoMediaFolder
		{
			get
			{
				return GetStringProperty("PhotoMediaFolder", "");
			}
			set
			{
				SetStringProperty("PhotoMediaFolder", value);
			}
		}

		public string VideoMediaFolder
		{
			get
			{
				return GetStringProperty("VideoMediaFolder", "");
			}
			set
			{
				SetStringProperty("VideoMediaFolder", value);
			}
		}

		public string RipDirectory
		{
			get
			{
				return GetStringProperty("RipDirectory", "");
			}
			set
			{
				SetStringProperty("RipDirectory", value);
			}
		}

		public IList<string> MonitoredApplicationsFolders
		{
			get
			{
				return GetStringListProperty("MonitoredApplicationsFolders");
			}
			set
			{
				SetStringListProperty("MonitoredApplicationsFolders", value);
			}
		}

		public IList<string> MonitoredVideoFolders
		{
			get
			{
				return GetStringListProperty("MonitoredVideoFolders");
			}
			set
			{
				SetStringListProperty("MonitoredVideoFolders", value);
			}
		}

		public IList<string> MonitoredPodcastFolders
		{
			get
			{
				return GetStringListProperty("MonitoredPodcastFolders");
			}
			set
			{
				SetStringListProperty("MonitoredPodcastFolders", value);
			}
		}

		public IList<string> MonitoredPhotoFolders
		{
			get
			{
				return GetStringListProperty("MonitoredPhotoFolders");
			}
			set
			{
				SetStringListProperty("MonitoredPhotoFolders", value);
			}
		}

		public IList<string> MonitoredAudioFolders
		{
			get
			{
				return GetStringListProperty("MonitoredAudioFolders");
			}
			set
			{
				SetStringListProperty("MonitoredAudioFolders", value);
			}
		}

		internal GrovelerConfiguration(RegistryHive hive)
			: base(hive, null, "Groveler")
		{
		}

		public GrovelerConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
