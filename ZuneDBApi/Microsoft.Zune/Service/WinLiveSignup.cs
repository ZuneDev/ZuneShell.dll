using System;
using ZuneUI;

namespace Microsoft.Zune.Service;

// Original obtains a native IWinLiveSignup* via a singleton IService QueryInterface.
// Not reverse engineered — see logs/Microsoft.Zune/Service/WinLiveSignup.md.
public class WinLiveSignup : IDisposable
{
    public WinLiveSignup()
    {
    }

    public HRESULT GetInformation(string locale, EHipType hipType, out WinLiveInformation information, out ServiceError serviceError)
    {
        information = null;
        serviceError = null;
        return HRESULT._E_FAIL;
    }

    public HRESULT CheckAvailableSigninName(string signinName, bool needSuggestedNames, string firstName, string lastName, out WinLiveAvailableInformation information, out ServiceError serviceError)
    {
        information = null;
        serviceError = null;
        return HRESULT._E_FAIL;
    }

    public HRESULT CreateAccount(string signinName, string signinPassword, string secretQuestion, string secretAnswer, string countryCode, string hipChallenge, string hipSolution, DateTime birthday, int termsOfServiceVersion, int languagePreference, out ServiceError serviceError)
    {
        serviceError = null;
        return HRESULT._E_FAIL;
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
