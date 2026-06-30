using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ZuneUI;

namespace Microsoft.Zune.Service;

public class AccountManagement : IDisposable
{
    private readonly object m_spAccountManagement; // Stub for CComPtrMgd<IAccountManagement>

    public AccountManagement()
    {
    }

    public HRESULT CreateAccount(PassportIdentity passportIdentity, string zuneTag, string locale, DateTime birthday, string firstName, string lastName, string email, Address address, AccountSettings accountSettings, PassportIdentity parentPassportIdentity, CreditCard parentCreditCard, out ServiceError serviceError)
    {
        serviceError = null;
        return HRESULT._S_OK;
    }

    public HRESULT ReserveZuneTag(string zuneTag, string countryCode, out IList suggestedNames, out ServiceError serviceError)
    {
        suggestedNames = null;
        serviceError = null;
        return HRESULT._S_OK;
    }

    public HRESULT ValidateCreditCard(PassportIdentity parentPassportIdentity, CreditCard creditCard, out ServiceError serviceError)
    {
        serviceError = null;
        return HRESULT._S_OK;
    }

    public HRESULT GetAccount(PassportIdentity passportIdentity, GetAccountCompleteCallback onSuccess, AccountManagementErrorCallback onError)
    {
        return HRESULT._S_OK;
    }

    public HRESULT GetAccount(PassportIdentity passportIdentity, out AccountUser accountUser, out ServiceError serviceError)
    {
        accountUser = null;
        serviceError = null;
        return HRESULT._S_OK;
    }

    public HRESULT SetAccount(PassportIdentity passportIdentity, AccountUser accountUser, out ServiceError serviceError)
    {
        serviceError = null;
        return HRESULT._S_OK;
    }

    public HRESULT SetNewsLetterSettings(AccountSettings accountSettings, out ServiceError serviceError)
    {
        serviceError = null;
        return HRESULT._S_OK;
    }

    public HRESULT SetPrivacySettings(AccountSettings accountSettings, PassportIdentity parentPassportIdentity, out ServiceError serviceError)
    {
        serviceError = null;
        return HRESULT._S_OK;
    }

    public HRESULT UpgradeAccount(PassportIdentity passportIdentity, AccountSettings accountSettings, PassportIdentity parentPassportIdentity, out ServiceError serviceError)
    {
        serviceError = null;
        return HRESULT._S_OK;
    }

    public HRESULT GetTermsOfService(string languageCode, string countryCode, out string termsOfService)
    {
        termsOfService = null;
        return HRESULT._S_OK;
    }

    public HRESULT GetSubscriptionDetails(string offerId, out ArrayList bulletStrings)
    {
        bulletStrings = null;
        return HRESULT._S_OK;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    ~AccountManagement()
    {
    }
}
