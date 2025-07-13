using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration;

public class ServiceConfiguration : CConfigurationManagedBase
{
	public string RootLicenseFirstRefreshDate
	{
		get
		{
			return GetStringProperty("RootLicenseFirstRefreshDate", "");
		}
		set
		{
			SetStringProperty("RootLicenseFirstRefreshDate", value);
		}
	}

	public string MBRTelemetryEndpoint
	{
		get
		{
			return GetStringProperty("MBRTelemetryEndpoint", "");
		}
		set
		{
			SetStringProperty("MBRTelemetryEndpoint", value);
		}
	}

	public int RefreshPCLicenseResult
	{
		get
		{
			return GetIntProperty("RefreshPCLicenseResult", 0);
		}
		set
		{
			SetIntProperty("RefreshPCLicenseResult", value);
		}
	}

	public int RefreshDeviceLicenseResult
	{
		get
		{
			return GetIntProperty("RefreshDeviceLicenseResult", 0);
		}
		set
		{
			SetIntProperty("RefreshDeviceLicenseResult", value);
		}
	}

	public int AutoRefreshQueryFallback
	{
		get
		{
			return GetIntProperty("AutoRefreshQueryFallback", 1800000);
		}
		set
		{
			SetIntProperty("AutoRefreshQueryFallback", value);
		}
	}

	public bool PurchaseHD
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("PurchaseHD", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("PurchaseHD", value);
		}
	}

	public int RecommendationsMaxTrackCount
	{
		get
		{
			return GetIntProperty("RecommendationsMaxTrackCount", 25);
		}
		set
		{
			SetIntProperty("RecommendationsMaxTrackCount", value);
		}
	}

	public bool InhibitReviewRefreshWarning
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("InhibitReviewRefreshWarning", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("InhibitReviewRefreshWarning", value);
		}
	}

	public bool InhibitWinPhoneAppPurchaseConfirmation
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("InhibitWinPhoneAppPurchaseConfirmation", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("InhibitWinPhoneAppPurchaseConfirmation", value);
		}
	}

	public bool InhibitSubscriptionFreePurchasePrompt
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("InhibitSubscriptionFreePurchasePrompt", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("InhibitSubscriptionFreePurchasePrompt", value);
		}
	}

	public bool InhibitSubscriptionEndingWarning
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("InhibitSubscriptionEndingWarning", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("InhibitSubscriptionEndingWarning", value);
		}
	}

	public bool InhibitSubscriptionBillingViolationSignInWarning
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("InhibitSubscriptionBillingViolationSignInWarning", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("InhibitSubscriptionBillingViolationSignInWarning", value);
		}
	}

	public bool InhibitSubscriptionMachineCountExceededSignInWarning
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("InhibitSubscriptionMachineCountExceededSignInWarning", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("InhibitSubscriptionMachineCountExceededSignInWarning", value);
		}
	}

	public string TimeTravel
	{
		get
		{
			return GetStringProperty("TimeTravel", "");
		}
		set
		{
			SetStringProperty("TimeTravel", value);
		}
	}

	public string MarketplaceCulture
	{
		get
		{
			return GetStringProperty("MarketplaceCulture", "");
		}
		set
		{
			SetStringProperty("MarketplaceCulture", value);
		}
	}

	public string LastSignedInUserGuid
	{
		get
		{
			return GetStringProperty("LastSignedInUserGuid", "");
		}
		set
		{
			SetStringProperty("LastSignedInUserGuid", value);
		}
	}

	public int ReportUsageDataDuration
	{
		get
		{
			return GetIntProperty("ReportUsageDataDuration", 7);
		}
		set
		{
			SetIntProperty("ReportUsageDataDuration", value);
		}
	}

	public int RefreshSubscriptionLicenseDuration
	{
		get
		{
			return GetIntProperty("RefreshSubscriptionLicenseDuration", 7);
		}
		set
		{
			SetIntProperty("RefreshSubscriptionLicenseDuration", value);
		}
	}

	public string SignInAtStartupUser
	{
		get
		{
			return GetStringProperty("SignInAtStartupUser", "");
		}
		set
		{
			SetStringProperty("SignInAtStartupUser", value);
		}
	}

	public string CommerceEndpoint
	{
		get
		{
			return GetStringProperty("CommerceEndpoint", "");
		}
		set
		{
			SetStringProperty("CommerceEndpoint", value);
		}
	}

	public string WindowsLiveSignupEndpoint
	{
		get
		{
			return GetStringProperty("WindowsLiveSignupEndpoint", "");
		}
		set
		{
			SetStringProperty("WindowsLiveSignupEndpoint", value);
		}
	}

	public string BillingEndpoint
	{
		get
		{
			return GetStringProperty("BillingEndpoint", "");
		}
		set
		{
			SetStringProperty("BillingEndpoint", value);
		}
	}

	public string TelemetryEndpoint
	{
		get
		{
			return GetStringProperty("TelemetryEndpoint", "");
		}
		set
		{
			SetStringProperty("TelemetryEndpoint", value);
		}
	}

	public string WMISPartner
	{
		get
		{
			return GetStringProperty("WMISPartner", "");
		}
		set
		{
			SetStringProperty("WMISPartner", value);
		}
	}

	public string WMISEndpoints
	{
		get
		{
			return GetStringProperty("WMISEndpoints", "");
		}
		set
		{
			SetStringProperty("WMISEndpoints", value);
		}
	}

	internal ServiceConfiguration(RegistryHive hive)
		: base(hive, null, "Service")
	{
	}

	public ServiceConfiguration(RegistryHive hive, string basePath, string instance)
		: base(hive, basePath, instance)
	{
	}
}
