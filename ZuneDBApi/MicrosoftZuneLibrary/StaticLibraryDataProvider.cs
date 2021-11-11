using Microsoft.Iris;

namespace MicrosoftZuneLibrary
{
	public class StaticLibraryDataProvider
	{
		public static void Register()
		{
			Application.RegisterDataProvider("StaticLibraryDataProvider", StaticLibraryDataProviderQuery.CreateInstance);
		}

		private StaticLibraryDataProvider()
		{
		}
	}
}
