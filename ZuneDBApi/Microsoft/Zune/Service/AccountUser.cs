using System;
using System.Runtime.CompilerServices;

namespace Microsoft.Zune.Service
{
	public class AccountUser
	{
		private string m_zuneTag;

		private string m_locale;

		private string m_firstName;

		private string m_lastName;

		private string m_email;

		private string m_phoneNumber;

		private string m_mobilePhoneNumber;

		private DateTime m_birthday;

		private Address m_address;

		private AccountSettings m_accountSettings;

		private PassportIdentity m_parentPassportIdentity;

		private CreditCard m_parentCreditCard;

		private AccountUserType m_accountUserType;

		public AccountUserType AccountUserType
		{
			get
			{
				return m_accountUserType;
			}
			set
			{
				m_accountUserType = value;
			}
		}

		public CreditCard ParentCreditCard
		{
			get
			{
				return m_parentCreditCard;
			}
			set
			{
				m_parentCreditCard = value;
			}
		}

		public PassportIdentity ParentPassportIdentity
		{
			get
			{
				return m_parentPassportIdentity;
			}
			set
			{
				m_parentPassportIdentity = value;
			}
		}

		public AccountSettings AccountSettings
		{
			get
			{
				return m_accountSettings;
			}
			set
			{
				m_accountSettings = value;
			}
		}

		public Address Address
		{
			get
			{
				return m_address;
			}
			set
			{
				m_address = value;
			}
		}

		public DateTime Birthday
		{
			get
			{
				return m_birthday;
			}
			set
			{
				m_birthday = value;
			}
		}

		public string MobilePhoneNumber
		{
			get
			{
				return m_mobilePhoneNumber;
			}
			set
			{
				m_mobilePhoneNumber = value;
			}
		}

		public string PhoneNumber
		{
			get
			{
				return m_phoneNumber;
			}
			set
			{
				m_phoneNumber = value;
			}
		}

		public string Email
		{
			get
			{
				return m_email;
			}
			set
			{
				m_email = value;
			}
		}

		public string LastName
		{
			get
			{
				return m_lastName;
			}
			set
			{
				m_lastName = value;
			}
		}

		public string FirstName
		{
			get
			{
				return m_firstName;
			}
			set
			{
				m_firstName = value;
			}
		}

		public string Locale
		{
			get
			{
				return m_locale;
			}
			set
			{
				m_locale = value;
			}
		}

		public string ZuneTag
		{
			get
			{
				return m_zuneTag;
			}
			set
			{
				m_zuneTag = value;
			}
		}

		internal unsafe AccountUser(IAccountUser* pAccountUser)
		{
			//IL_0055: Expected I4, but got I8
			//IL_008b: Expected I, but got I8
			//IL_008b: Expected I, but got I8
			//IL_008b: Expected I, but got I8
			//IL_009c: Expected I, but got I8
			//IL_00aa: Expected I, but got I8
			//IL_00b8: Expected I, but got I8
			//IL_00c6: Expected I, but got I8
			//IL_00d4: Expected I, but got I8
			//IL_00e2: Expected I, but got I8
			//IL_00f0: Expected I, but got I8
			//IL_0111: Expected I, but got I8
			//IL_0124: Expected I, but got I8
			//IL_0143: Expected I, but got I8
			//IL_0143: Expected I, but got I8
			if (pAccountUser == null)
			{
				return;
			}
			string wBSTRString = "";
			fixed (char* wBSTRStringPtr = wBSTRString)
			{
				string wBSTRString2 = "";
				fixed (char* wBSTRString2Ptr = wBSTRString2)
				{
					string wBSTRString3 = "";
					fixed (char* wBSTRString3Ptr = wBSTRString3)
					{
						string wBSTRString4 = "";
						fixed (char* wBSTRString4Ptr = wBSTRString4)
						{
							string wBSTRString5 = "";
							fixed (char* wBSTRString5Ptr = wBSTRString5)
							{
								string wBSTRString6 = "";
								fixed (char* wBSTRString6Ptr = wBSTRString6)
								{
									string wBSTRString7 = "";
									fixed (char* wBSTRString7Ptr = wBSTRString7)
									{
										_SYSTEMTIME stValue = default;
										CComPtrNtv<IAddress> cComPtrNtv_003CIAddress_003E = new();
										try
										{
											CComPtrNtv<INewsletterSettings> cComPtrNtv_003CINewsletterSettings_003E = new();
											try
											{
												CComPtrNtv<IPrivacySettings> cComPtrNtv_003CIPrivacySettings_003E = new();
												try
												{
													if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, ushort**, _SYSTEMTIME*, ushort**, ushort**, ushort**, ushort**, ushort**, IAddress**, INewsletterSettings**, IPrivacySettings**, IPassportIdentity**, ICreditCard**, int>)(*(ulong*)(*(long*)pAccountUser + 32)))((nint)pAccountUser, (ushort**)wBSTRStringPtr, (ushort**)&wBSTRString2Ptr, &stValue, (ushort**)wBSTRString3Ptr, (ushort**)wBSTRString4Ptr, (ushort**)wBSTRString5Ptr, (ushort**)wBSTRString6Ptr, (ushort**)wBSTRString7Ptr, cComPtrNtv_003CIAddress_003E.GetPtrToPtr(), cComPtrNtv_003CINewsletterSettings_003E.GetPtrToPtr(), (IPrivacySettings**)(&ptrNtvIPrivacySettings), null, null) >= 0)
													{
														m_zuneTag = wBSTRString;
														m_locale = wBSTRString2;
														m_firstName = wBSTRString3;
														m_lastName = wBSTRString4;
														m_email = wBSTRString5;
														m_phoneNumber = wBSTRString6;
														m_mobilePhoneNumber = wBSTRString7;
														DateTime dateTime = (m_birthday = Module.SystemTimeToDateTime(stValue));
														m_accountUserType = (AccountUserType)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EAccountUserType>)(*(ulong*)(*(long*)pAccountUser + 48)))((nint)pAccountUser);
														if (*(long*)(cComPtrNtv_003CIAddress_003E.p) != 0L)
														{
															m_address = new Address((IAddress*)(*(ulong*)(cComPtrNtv_003CIAddress_003E.p)));
														}
														if (*(long*)(cComPtrNtv_003CINewsletterSettings_003E.p) != 0L && *(long*)(cComPtrNtv_003CIPrivacySettings_003E.p) != 0L)
														{
															m_accountSettings = new AccountSettings((INewsletterSettings*)(*(ulong*)(cComPtrNtv_003CINewsletterSettings_003E.p)), (IPrivacySettings*)(*(ulong*)(cComPtrNtv_003CIPrivacySettings_003E.p)));
														}
													}
												}
												finally
												{
													cComPtrNtv_003CIPrivacySettings_003E.Dispose();
												}
											}
											finally
											{
												cComPtrNtv_003CINewsletterSettings_003E.Dispose();
											}
										}
										finally
										{
											cComPtrNtv_003CIAddress_003E.Dispose();
										}
									}
								}
							}
						}
					}
				}
			}
		}

		public AccountUser()
		{
			m_accountUserType = AccountUserType.Unknown;
		}
	}
}
