using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class PicturesConfiguration : CConfigurationManagedBase
	{
		public int WinLivePhotoGalleryMinorVersion
		{
			get
			{
				return GetIntProperty("WinLivePhotoGalleryMinorVersion", 0);
			}
			set
			{
				SetIntProperty("WinLivePhotoGalleryMinorVersion", value);
			}
		}

		public int WinLivePhotoGalleryMajorVersion
		{
			get
			{
				return GetIntProperty("WinLivePhotoGalleryMajorVersion", 14);
			}
			set
			{
				SetIntProperty("WinLivePhotoGalleryMajorVersion", value);
			}
		}

		public string WinLivePhotoGalleryUpgradeCode
		{
			get
			{
				return GetStringProperty("WinLivePhotoGalleryUpgradeCode", "F81F501C-236B-4B4A-8E92-0575EAAD06FA");
			}
			set
			{
				SetStringProperty("WinLivePhotoGalleryUpgradeCode", value);
			}
		}

		public int WinLiveMovieMakerMinorVersion
		{
			get
			{
				return GetIntProperty("WinLiveMovieMakerMinorVersion", 0);
			}
			set
			{
				SetIntProperty("WinLiveMovieMakerMinorVersion", value);
			}
		}

		public int WinLiveMovieMakerMajorVersion
		{
			get
			{
				return GetIntProperty("WinLiveMovieMakerMajorVersion", 14);
			}
			set
			{
				SetIntProperty("WinLiveMovieMakerMajorVersion", value);
			}
		}

		public string WinLiveMovieMakerUpgradeCode
		{
			get
			{
				return GetStringProperty("WinLiveMovieMakerUpgradeCode", "D12D72AE-1B05-4DD3-8EAD-06A40BE6E02F");
			}
			set
			{
				SetStringProperty("WinLiveMovieMakerUpgradeCode", value);
			}
		}

		public bool DisplayAutouploadNotification
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("DisplayAutouploadNotification", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("DisplayAutouploadNotification", value);
			}
		}

		internal PicturesConfiguration(RegistryHive hive)
			: base(hive, null, "Pictures")
		{
		}

		public PicturesConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
