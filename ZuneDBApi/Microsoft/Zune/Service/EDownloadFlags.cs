namespace Microsoft.Zune.Service
{
	public enum EDownloadFlags
	{
		Stream = 0x40,
		HD = 0x80,
		Rental = 0x20,
		DeviceLicensed = 8,
		CanBeOffline = 4,
		UserCard = 2,
		Channel = 1,
		None = 0,
		Subscription = 0x10
	}
}
