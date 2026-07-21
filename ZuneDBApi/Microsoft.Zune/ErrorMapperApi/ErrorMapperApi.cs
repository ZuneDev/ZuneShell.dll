namespace Microsoft.Zune.ErrorMapperApi;

// Original wraps a native error-mapping table lookup. Not reverse engineered — see
// logs/MicrosoftZuneLibrary/ZuneLibrary.md. The eCondition parameter is the global
// (not namespaced) `eErrorCondition` enum — see eErrorCondition.cs at the project
// root; this class previously declared its own duplicate, conflicting
// Microsoft.Zune.ErrorMapperApi.eErrorCondition, which ILSpy's assembly-wide type
// listing confirms doesn't exist in the original (only one eErrorCondition exists,
// in the global namespace).
public class ErrorMapperApi
{
    public static ErrorMapperResult GetMappedErrorDescriptionAndUrl(int hrOrig, eErrorCondition eCondition)
    {
        return new ErrorMapperResult();
    }

    public static ErrorMapperResult GetMappedErrorDescriptionAndUrl(int hrOrig)
    {
        return GetMappedErrorDescriptionAndUrl(hrOrig, eErrorCondition.eEC_None);
    }
}
