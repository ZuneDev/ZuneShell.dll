namespace Microsoft.Zune.Configuration
{
	public class VersionInfo
	{
		public static string BuildNumber =>
#if ZUNE_5
			"5.0.0000.0";
#else
			"4.8.2345.0";
#endif
	}
}
