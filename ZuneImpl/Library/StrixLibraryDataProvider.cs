using Microsoft.Iris;
using StrixMusic.Sdk.AppModels;

namespace Microsoft.Zune.Library;

public static class StrixLibraryDataProvider
{
    public static IStrixDataRoot DataRoot { get; set; }

    public static void Register()
    {
        Application.RegisterDataProvider("Library", new DataProviderQueryFactory(ConstructQuery));
    }

    private static DataProviderQuery ConstructQuery(object queryTypeCookie)
    {
        return new StrixLibraryDataProviderQuery(queryTypeCookie, DataRoot);
    }
}
