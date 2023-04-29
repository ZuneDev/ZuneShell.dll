#if OPENZUNE

using Microsoft.Iris;

namespace Microsoft.Zune.Library
{
    public static class StrixLibraryDataProvider
    {
        public static void Register()
        {
            Application.RegisterDataProvider("Library", new DataProviderQueryFactory(ConstructQuery));
        }

        private static DataProviderQuery ConstructQuery(object queryTypeCookie)
        {
            return new StrixLibraryDataProviderQuery(queryTypeCookie);
        }
    }
}

#endif
