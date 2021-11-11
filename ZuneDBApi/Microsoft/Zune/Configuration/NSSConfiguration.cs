using System.Collections.Generic;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class NSSConfiguration : CConfigurationManagedBase
	{
		public int UnShareFoldersProcessed
		{
			get
			{
				return GetIntProperty("UnShareFoldersProcessed", 0);
			}
			set
			{
				SetIntProperty("UnShareFoldersProcessed", value);
			}
		}

		public int ShareFoldersProcessed
		{
			get
			{
				return GetIntProperty("ShareFoldersProcessed", 0);
			}
			set
			{
				SetIntProperty("ShareFoldersProcessed", value);
			}
		}

		public IList<string> ErrorFolders
		{
			get
			{
				return GetStringListProperty("ErrorFolders");
			}
			set
			{
				SetStringListProperty("ErrorFolders", value);
			}
		}

		public IList<string> UnShareFolders
		{
			get
			{
				return GetStringListProperty("UnShareFolders");
			}
			set
			{
				SetStringListProperty("UnShareFolders", value);
			}
		}

		public IList<string> ShareFolders
		{
			get
			{
				return GetStringListProperty("ShareFolders");
			}
			set
			{
				SetStringListProperty("ShareFolders", value);
			}
		}

		internal NSSConfiguration(RegistryHive hive)
			: base(hive, null, "NSS")
		{
		}

		public NSSConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
