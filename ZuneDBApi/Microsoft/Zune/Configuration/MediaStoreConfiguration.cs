using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class MediaStoreConfiguration : CConfigurationManagedBase
	{
		public bool EnableLongMutexHoldMiniDump
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("EnableLongMutexHoldMiniDump", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("EnableLongMutexHoldMiniDump", value);
			}
		}

		public int DBMutextLongHoldTresholdMS
		{
			get
			{
				return GetIntProperty("DBMutextLongHoldTresholdMS", 2000);
			}
			set
			{
				SetIntProperty("DBMutextLongHoldTresholdMS", value);
			}
		}

		public bool EnableLongQueriesLog
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("EnableLongQueriesLog", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("EnableLongQueriesLog", value);
			}
		}

		public int LongQueryThresholdMS
		{
			get
			{
				return GetIntProperty("LongQueryThresholdMS", 2000);
			}
			set
			{
				SetIntProperty("LongQueryThresholdMS", value);
			}
		}

		public int AutoPlaylistQueryThresholdMS
		{
			get
			{
				return GetIntProperty("AutoPlaylistQueryThresholdMS", 5000);
			}
			set
			{
				SetIntProperty("AutoPlaylistQueryThresholdMS", value);
			}
		}

		public int ArtistMetadataLookupBackoff
		{
			get
			{
				return GetIntProperty("ArtistMetadataLookupBackoff", 250);
			}
			set
			{
				SetIntProperty("ArtistMetadataLookupBackoff", value);
			}
		}

		public bool UseOldRequeryCompare
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("UseOldRequeryCompare", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("UseOldRequeryCompare", value);
			}
		}

		public bool SharedUserRatings
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("SharedUserRatings", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("SharedUserRatings", value);
			}
		}

		public int HiddenContentCleanupIntervalMinutes
		{
			get
			{
				return GetIntProperty("HiddenContentCleanupIntervalMinutes", 1440);
			}
			set
			{
				SetIntProperty("HiddenContentCleanupIntervalMinutes", value);
			}
		}

		public double HiddenContentCleanupTime
		{
			get
			{
				return GetDoubleProperty("HiddenContentCleanupTime", 0.0);
			}
			set
			{
				SetDoubleProperty("HiddenContentCleanupTime", value);
			}
		}

		public bool EnableFuzzyMatching
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("EnableFuzzyMatching", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("EnableFuzzyMatching", value);
			}
		}

		public string LastServiceIdFlushDate
		{
			get
			{
				return GetStringProperty("LastServiceIdFlushDate", "");
			}
			set
			{
				SetStringProperty("LastServiceIdFlushDate", value);
			}
		}

		public bool AlertSyncAllFriendsBehavior
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("AlertSyncAllFriendsBehavior", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("AlertSyncAllFriendsBehavior", value);
			}
		}

		public int ArtCheckRunTimeMs
		{
			get
			{
				return GetIntProperty("ArtCheckRunTimeMs", 60000);
			}
			set
			{
				SetIntProperty("ArtCheckRunTimeMs", value);
			}
		}

		public int CachedAllTracksCount
		{
			get
			{
				return GetIntProperty("CachedAllTracksCount", 10000);
			}
			set
			{
				SetIntProperty("CachedAllTracksCount", value);
			}
		}

		public bool ComputeFingerprints
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("ComputeFingerprints", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("ComputeFingerprints", value);
			}
		}

		public bool RanUnknownTrackCheck
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("RanUnknownTrackCheck", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("RanUnknownTrackCheck", value);
			}
		}

		public bool SaveMDQCD
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("SaveMDQCD", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("SaveMDQCD", value);
			}
		}

		public bool SaveMDRCD
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("SaveMDRCD", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("SaveMDRCD", value);
			}
		}

		public int LifecycleTimeoutMs
		{
			get
			{
				return GetIntProperty("LifecycleTimeoutMs", 600000);
			}
			set
			{
				SetIntProperty("LifecycleTimeoutMs", value);
			}
		}

		public bool WriteOutMetadata
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("WriteOutMetadata", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("WriteOutMetadata", value);
			}
		}

		public int AutoOrganize
		{
			get
			{
				return GetIntProperty("AutoOrganize", 0);
			}
			set
			{
				SetIntProperty("AutoOrganize", value);
			}
		}

		public int AutoRename
		{
			get
			{
				return GetIntProperty("AutoRename", 0);
			}
			set
			{
				SetIntProperty("AutoRename", value);
			}
		}

		public int AutoUpdateMetadata
		{
			get
			{
				return GetIntProperty("AutoUpdateMetadata", 1);
			}
			set
			{
				SetIntProperty("AutoUpdateMetadata", value);
			}
		}

		public bool OverwriteAllMetadata
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("OverwriteAllMetadata", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("OverwriteAllMetadata", value);
			}
		}

		public bool ConnectToInternetForAlbumMetadata
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("ConnectToInternetForAlbumMetadata", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("ConnectToInternetForAlbumMetadata", value);
			}
		}

		public bool ConfirmPasteAlbumArt
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("ConfirmPasteAlbumArt", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("ConfirmPasteAlbumArt", value);
			}
		}

		public bool ConfirmMultiVideoEdit
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("ConfirmMultiVideoEdit", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("ConfirmMultiVideoEdit", value);
			}
		}

		public bool ConfirmMultiSongEdit
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("ConfirmMultiSongEdit", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("ConfirmMultiSongEdit", value);
			}
		}

		public bool ConfirmMultiAlbumEdit
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("ConfirmMultiAlbumEdit", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("ConfirmMultiAlbumEdit", value);
			}
		}

		public bool ConfirmDeviceMediaDeletion
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("ConfirmDeviceMediaDeletion", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("ConfirmDeviceMediaDeletion", value);
			}
		}

		public bool ConfirmAccountDevicePortableDeletion
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("ConfirmAccountDevicePortableDeletion", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("ConfirmAccountDevicePortableDeletion", value);
			}
		}

		public bool ConfirmAccountDevicePCDeletion
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("ConfirmAccountDevicePCDeletion", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("ConfirmAccountDevicePCDeletion", value);
			}
		}

		public int PlaylistDefaultDeleteChoice
		{
			get
			{
				return GetIntProperty("PlaylistDefaultDeleteChoice", 0);
			}
			set
			{
				SetIntProperty("PlaylistDefaultDeleteChoice", value);
			}
		}

		public int LibraryDefaultDeleteChoice
		{
			get
			{
				return GetIntProperty("LibraryDefaultDeleteChoice", 0);
			}
			set
			{
				SetIntProperty("LibraryDefaultDeleteChoice", value);
			}
		}

		public int MaxSearchResults
		{
			get
			{
				return GetIntProperty("MaxSearchResults", 200);
			}
			set
			{
				SetIntProperty("MaxSearchResults", value);
			}
		}

		public string IgnoredChars
		{
			get
			{
				return GetStringProperty("IgnoredChars", "\"%\n\t");
			}
			set
			{
				SetStringProperty("IgnoredChars", value);
			}
		}

		public int GrovelTimeListUpdateMinimumFrequency
		{
			get
			{
				return GetIntProperty("GrovelTimeListUpdateMinimumFrequency", 5000);
			}
			set
			{
				SetIntProperty("GrovelTimeListUpdateMinimumFrequency", value);
			}
		}

		public int FileOpenWatchdog
		{
			get
			{
				return GetIntProperty("FileOpenWatchdog", 10000);
			}
			set
			{
				SetIntProperty("FileOpenWatchdog", value);
			}
		}

		public int ListUpdateMinimumFrequency
		{
			get
			{
				return GetIntProperty("ListUpdateMinimumFrequency", 500);
			}
			set
			{
				SetIntProperty("ListUpdateMinimumFrequency", value);
			}
		}

		internal MediaStoreConfiguration(RegistryHive hive)
			: base(hive, null, "MediaStore")
		{
		}

		public MediaStoreConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
