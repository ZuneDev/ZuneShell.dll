using System.Runtime.InteropServices;

namespace ZuneUI;

public struct HRESULT
{
    public int hr;

    public static readonly HRESULT _S_OK = 0;
    public static readonly HRESULT _S_FALSE = 1;
    public static readonly HRESULT _E_ABORT = -2147467260;
    public static readonly HRESULT _E_PENDING = -2147483638;
    public static readonly HRESULT _E_FAIL = -2147467259;
    public static readonly HRESULT _E_UNEXPECTED = -2147418113;
    public static readonly HRESULT _E_ACCESSDENIED = -2147024891;
    public static readonly HRESULT _ERROR_SERVICE_DISABLED = 1058;
    public static readonly HRESULT _NS_E_WMP_UNKNOWN_ERROR = -1072885299;
    public static readonly HRESULT _NS_E_WMP_PLAYLIST_IMPORT_ERROR = -1072885765;
    public static readonly HRESULT _NS_E_CD_BUSY = -1072885764;
    public static readonly HRESULT _E_ALREADY_EXISTS = -2147024713;
    public static readonly HRESULT _DB_E_NOTFOUND = -2147217895;
    public static readonly HRESULT _DB_E_RESOURCEEXISTS = -2147217768;
    public static readonly HRESULT _DB_E_BADPARAMETERNAME = -2147217827;
    public static readonly HRESULT _ZUNE_E_NO_AVAILABLE_RESTORE_POINT = -1056899026;
    public static readonly HRESULT _NS_E_MTPZ_WLAN_TEST_UNKNOWN = -1072887866;
    public static readonly HRESULT _NS_E_MTPZ_WLAN_TEST_RUNNING = -1072887865;
    public static readonly HRESULT _NS_E_MTPZ_WLAN_TEST_FAIL_NO_CONFIG = -1072887864;
    public static readonly HRESULT _NS_E_MTPZ_WLAN_TEST_FAIL_ASSOCIATE = -1072887863;
    public static readonly HRESULT _NS_E_MTPZ_WLAN_TEST_FAIL_DHCP = -1072887862;
    public static readonly HRESULT _NS_E_MTPZ_WLAN_TEST_FAIL_TIMEOUT = -1072887861;
    public static readonly HRESULT _NS_E_MTPZ_WLAN_TEST_FAIL_CANCELLED = -1072887860;
    public static readonly HRESULT _NS_E_MTPZ_WLAN_TEST_FAIL_INTERNAL = -1072887859;
    public static readonly HRESULT _ZEST_E_TOO_MANY_DEREGISTRATIONS_WITHIN_MONTH = -1056857072;
    public static readonly HRESULT _NS_E_MEDIA_DOWNLOAD_MAXIMUM_EXCEEDED = -1072884900;
    public static readonly HRESULT _ZUNE_E_NO_SUBSCRIPTION_DOWNLOAD_RIGHTS = -1056899031;
    public static readonly HRESULT _NS_E_MEDIA_NOT_PURCHASED = -1072884891;
    public static readonly HRESULT _NS_E_MESSAGING_CLIENT_ERROR = -1072885517;
    public static readonly HRESULT _NS_E_MESSAGING_RECIPIENT_ERROR = -1072885515;
    public static readonly HRESULT _NS_E_MESSAGING_PARENTAL_CONTROL_ERROR = -1072885513;
    public static readonly HRESULT _NS_E_CART_FULL = -1072885493;
    public static readonly HRESULT _NS_E_CART_TOO_MANY_NEW_ITEMS = -1072885492;
    public static readonly HRESULT _NS_E_CART_MORE_ITEMS_AVAILABLE = -2146627315;
    public static readonly HRESULT _NS_E_SIGNIN_TERMS_OF_SERVICE = -1072884874;
    public static readonly HRESULT _NS_E_SERVER_ACCESSDENIED = -1072877829;
    public static readonly HRESULT _NS_E_PASSPORT_LOGIN_FAILED = -1072884892;
    public static readonly HRESULT _NS_E_SUBSCRIPTIONSERVICE_LOGIN_FAILED = -1072884897;
    public static readonly HRESULT _NS_E_INVALID_USERNAME_AND_PASSWORD = -1072884910;
    public static readonly HRESULT _NS_E_SIGNIN_ACCOUNTS_INVALID_USER = -1072884884;
    public static readonly HRESULT _NS_E_SIGNIN_ACCOUNTS_NOT_XENON_USER = -1072884882;
    public static readonly HRESULT _NS_E_SIGNIN_WCMUSIC_ACCOUNT_NOT_ELIGIBLE = -1072884883;
    public static readonly HRESULT _ZUNE_E_SIGNIN_TERMS_OF_SERVICE_CHILD = -1056899049;
    public static readonly HRESULT _NS_E_SIGNIN_INVALID_REGION = -1072884909;
    public static readonly HRESULT _ZEST_E_UNAUTHENTICATED = -1056858108;
    public static readonly HRESULT _NS_E_SIGNIN_HTTP_GONE = -1072885082;
    public static readonly HRESULT _NS_E_BILLING_LIGHTWEIGHT_ACCOUNT = -1072885473;
    public static readonly HRESULT _NS_E_FIRMWARE_UPDATE_DISK_FULL = -1072885167;
    public static readonly HRESULT _NS_E_WINLIVE_SIGNIN_NAME_TOO_SHORT = -1072875780;
    public static readonly HRESULT _NS_E_WINLIVE_SIGNIN_NAME_TOO_LONG = -1072875804;
    public static readonly HRESULT _NS_E_WINLIVE_SIGNIN_NAME_INVALID = -1072875803;
    public static readonly HRESULT _NS_E_WINLIVE_EMAIL_INVALID = -1072875802;
    public static readonly HRESULT _NS_E_WINLIVE_NAME_INVALID = -1072875801;
    public static readonly HRESULT _NS_E_WINLIVE_BIRTH_YEAR_INVALID = -1072875800;
    public static readonly HRESULT _NS_E_WINLIVE_BIRTH_DAY_INVALID = -1072875799;
    public static readonly HRESULT _NS_E_WINLIVE_BIRTH_MONTH_INVALID = -1072875798;
    public static readonly HRESULT _NS_E_WINLIVE_BIRTH_DATE_FUTURE = -1072875797;
    public static readonly HRESULT _NS_E_WINLIVE_HIP_SOLUTION_INVALID = -1072875796;
    public static readonly HRESULT _NS_E_WINLIVE_PASSWORD_TOO_LONG = -1072875795;
    public static readonly HRESULT _NS_E_WINLIVE_PASSWORD_TOO_SHORT = -1072875794;
    public static readonly HRESULT _NS_E_WINLIVE_PASSWORD_INVALID = -1072875793;
    public static readonly HRESULT _NS_E_WINLIVE_MEMBER_EXISTS = -1072875792;
    public static readonly HRESULT _NS_E_WINLIVE_DOMAIN_IS_MANAGED = -1072875791;
    public static readonly HRESULT _NS_E_WINLIVE_SECRET_QUESTION_TOO_SHORT = -1072875790;
    public static readonly HRESULT _NS_E_WINLIVE_SECRET_QUESTION_TOO_LONG = -1072875789;
    public static readonly HRESULT _NS_E_WINLIVE_SECRET_ANSWER_TOO_SHORT = -1072875788;
    public static readonly HRESULT _NS_E_WINLIVE_SECRET_ANSWER_TOO_LONG = -1072875787;
    public static readonly HRESULT _NS_E_WINLIVE_SECRET_QUESTION_CONTAINS_ANSWER = -1072875786;
    public static readonly HRESULT _NS_E_WINLIVE_SECRET_ANSWER_CONTAINS_MEMBER_NAME = -1072875785;
    public static readonly HRESULT _NS_E_WINLIVE_SECRET_ANSWER_CONTAINS_PASSWORD = -1072875784;
    public static readonly HRESULT _NS_E_WINLIVE_SECRET_QUESTION_CONTAINS_PASSWORD = -1072875783;
    public static readonly HRESULT _ZUNE_E_WINLIVE_UNAUTHORIZED_DOMAIN = -1056899039;
    public static readonly HRESULT _ZEST_E_ACCOUNT_ZUNETAG_OCCUPIED = -1056857549;
    public static readonly HRESULT _ZUNE_E_SIGNUP_INVALID_PARENT_EMAIL = -1056899047;
    public static readonly HRESULT _ZUNE_E_SIGNUP_INVALID_PARENT_AGE = -1056899046;
    public static readonly HRESULT _ZUNE_E_SIGNUP_PASSWORDS_DONT_MATCH = -1056899045;
    public static readonly HRESULT _ZUNE_E_UPDATE_ACCOUNT_INFO_FAILED = -1056899040;
    public static readonly HRESULT _ZEST_E_INVALID_POSTALCODE = -1056857603;
    public static readonly HRESULT _ZEST_E_LIVEACCOUNT_INVALIDPHONE = -1056857601;
    public static readonly HRESULT _ZEST_E_LIVEACCOUNT_ADDRESS_INVALID = -1056857537;
    public static readonly HRESULT _ZEST_E_LIVEACCOUNT_PAYMENT_INSTRUMENT_INVALID = -1056857540;
    public static readonly HRESULT _ZEST_E_CREDITCARD_ADDRESS_INVALID = -1056857570;
    public static readonly HRESULT _ZEST_E_CREDITCARD_ADDRESS_STREET1_INVALID = -1056857568;
    public static readonly HRESULT _ZEST_E_CREDITCARD_ADDRESS_CITY_INVALID = -1056857567;
    public static readonly HRESULT _ZEST_E_CREDITCARD_ADDRESS_STATE_INVALID = -1056857566;
    public static readonly HRESULT _ZEST_E_CREDITCARD_ADDRESS_POSTALCODE_INVALID = -1056857565;
    public static readonly HRESULT _ZEST_E_CREDITCARD_PARENTPHONE_INVALID = -1056857564;
    public static readonly HRESULT _ZEST_E_CREDITCARD_INVALID = -1056857563;
    public static readonly HRESULT _ZEST_E_CREDITCARD_VALIDATE_FAILED = -1056857562;
    public static readonly HRESULT _ZEST_E_CREDITCARD_ADD_FAILED = -1056857561;
    public static readonly HRESULT _ZEST_E_INVALID_ARG_ZUNETAG = -1056858064;
    public static readonly HRESULT _ZEST_E_INVALID_ARG_PARENT_PHONE_INVALID = -1056858078;
    public static readonly HRESULT _ZEST_E_INVALID_ARG_CONTACT_INFO = -1056858076;
    public static readonly HRESULT _ZEST_E_INVALID_ARG_LANGUAGE = -1056858057;
    public static readonly HRESULT _ZEST_E_ZUNETAG_OFFENSIVE = -1056857522;
    public static readonly HRESULT _ZUNE_E_PURCHASE_REQUIRES_AUTHORIZATION = -1056899019;
    public static readonly HRESULT _ZEST_E_PARTNER_SERVICE_UNKNOWN_ERROR = -1056848896;
    public static readonly HRESULT _NS_E_DRM_RENTAL_LICENSE_EXPIRED = -1072879537;
    public static readonly HRESULT _NS_E_DRM_LICENSE_ALREADY_ACQUIRED = -1072879536;
    public static readonly HRESULT _NS_E_DRM_DEVICE_RENTAL_LICENSE = -1072879535;
    public static readonly HRESULT _ZUNE_E_CONTENT_NOT_SUPPORTED_ON_TUNER = -1056899034;
    public static readonly HRESULT _ZUNE_E_QUICKMIX_MEDIA_NOT_FOUND = -1056898969;
    public static readonly HRESULT _ZUNE_E_QUICKMIX_SESSION_IN_USE = -1056898964;
    public static readonly HRESULT _ZUNE_E_UNKNOWN_REGION_OR_LANGUAGE = -1056899029;
    public static readonly HRESULT _NS_E_CURL_INVALIDCHAR = -1072884955;
    public static readonly HRESULT _NS_E_CURL_INVALIDPATH = -1072884953;
    public static readonly HRESULT _ZUNE_E_ADD_REVIEW_FAILED = -1056898960;
    public static readonly HRESULT _ZEST_E_MAX_CONCURRENTSTREAMING_EXCEEDED = -1056856102;
    public static readonly HRESULT _ZEST_E_MULTITUNER_CONCURRENTSTREAMING_DETECTED = -1056856104;
    public static readonly HRESULT _ZEST_E_MEDIAINSTANCE_STREAMING_OCCUPIED = -1056856095;

    public readonly int Int => hr;

    public readonly bool IsSuccess
    {
        [return: MarshalAs(UnmanagedType.U1)]
        get => hr >= 0;
    }

    public readonly bool IsError
    {
        [return: MarshalAs(UnmanagedType.U1)]
        get => hr < 0;
    }

    public HRESULT(int hr)
    {
        this.hr = hr;
    }

    [return: MarshalAs(UnmanagedType.U1)]
    public static bool operator ==(HRESULT hrA, HRESULT hrB) => hrA.hr == hrB.hr;

    [return: MarshalAs(UnmanagedType.U1)]
    public static bool operator !=(HRESULT hrA, HRESULT hrB) => hrA.hr != hrB.hr;

    public static implicit operator HRESULT(int hr) => new(hr);
    public static implicit operator int(HRESULT hr) => hr.Int;

    [return: MarshalAs(UnmanagedType.U1)]
    public override readonly bool Equals(object? oCompare) => oCompare is HRESULT other && hr == other.hr;

    public override readonly int GetHashCode() => hr;

    public override readonly string ToString() => "hr:" + hr.ToString("X");
}
