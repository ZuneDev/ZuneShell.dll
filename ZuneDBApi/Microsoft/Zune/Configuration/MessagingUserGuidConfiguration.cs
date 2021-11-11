using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class MessagingUserGuidConfiguration : CConfigurationManagedBase
	{
		public int CartItemsToUploadCount
		{
			get
			{
				return GetIntProperty("CartItemsToUploadCount", -1);
			}
			set
			{
				SetIntProperty("CartItemsToUploadCount", value);
			}
		}

		internal MessagingUserGuidConfiguration(RegistryHive hive)
			: base(hive, null, "MessagingUserGuid")
		{
		}

		public MessagingUserGuidConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
