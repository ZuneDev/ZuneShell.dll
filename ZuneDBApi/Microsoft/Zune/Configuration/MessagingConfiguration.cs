using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class MessagingConfiguration : CConfigurationManagedBase
	{
		public int MaxServerCartItems
		{
			get
			{
				return GetIntProperty("MaxServerCartItems", 50);
			}
			set
			{
				SetIntProperty("MaxServerCartItems", value);
			}
		}

		public int MaxSubMessagesPerMessage
		{
			get
			{
				return GetIntProperty("MaxSubMessagesPerMessage", 10);
			}
			set
			{
				SetIntProperty("MaxSubMessagesPerMessage", value);
			}
		}

		public int MaxTracksPerMessage
		{
			get
			{
				return GetIntProperty("MaxTracksPerMessage", 200);
			}
			set
			{
				SetIntProperty("MaxTracksPerMessage", value);
			}
		}

		public int MaxPhotosPerMessage
		{
			get
			{
				return GetIntProperty("MaxPhotosPerMessage", 30);
			}
			set
			{
				SetIntProperty("MaxPhotosPerMessage", value);
			}
		}

		public int MessageMaxSize
		{
			get
			{
				return GetIntProperty("MessageMaxSize", 5242880);
			}
			set
			{
				SetIntProperty("MessageMaxSize", value);
			}
		}

		public int LastMessageId
		{
			get
			{
				return GetIntProperty("LastMessageId", 0);
			}
			set
			{
				SetIntProperty("LastMessageId", value);
			}
		}

		public int ServiceThrottlingInterval
		{
			get
			{
				return GetIntProperty("ServiceThrottlingInterval", 5000);
			}
			set
			{
				SetIntProperty("ServiceThrottlingInterval", value);
			}
		}

		public int UnreadMessageCountPollInterval
		{
			get
			{
				return GetIntProperty("UnreadMessageCountPollInterval", 600000);
			}
			set
			{
				SetIntProperty("UnreadMessageCountPollInterval", value);
			}
		}

		internal MessagingConfiguration(RegistryHive hive)
			: base(hive, null, "Messaging")
		{
		}

		public MessagingConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
