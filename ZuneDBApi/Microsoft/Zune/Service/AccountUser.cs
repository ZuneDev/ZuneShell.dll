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
			WBSTRString wBSTRString;
			Module.WBSTRString_002E_007Bctor_007D(&wBSTRString);
			try
			{
				WBSTRString wBSTRString2;
				Module.WBSTRString_002E_007Bctor_007D(&wBSTRString2);
				try
				{
					WBSTRString wBSTRString3;
					Module.WBSTRString_002E_007Bctor_007D(&wBSTRString3);
					try
					{
						WBSTRString wBSTRString4;
						Module.WBSTRString_002E_007Bctor_007D(&wBSTRString4);
						try
						{
							WBSTRString wBSTRString5;
							Module.WBSTRString_002E_007Bctor_007D(&wBSTRString5);
							try
							{
								WBSTRString wBSTRString6;
								Module.WBSTRString_002E_007Bctor_007D(&wBSTRString6);
								try
								{
									WBSTRString wBSTRString7;
									Module.WBSTRString_002E_007Bctor_007D(&wBSTRString7);
									try
									{
										_SYSTEMTIME stValue;
										*(short*)(&stValue) = 0;
                                        // IL initblk instruction
                                        Unsafe.InitBlockUnaligned(ref Unsafe.AddByteOffset(ref stValue, 2), 0, 14);
										CComPtrNtv<IAddress> cComPtrNtv_003CIAddress_003E = new();
										try
										{
											CComPtrNtv<INewsletterSettings> cComPtrNtv_003CINewsletterSettings_003E = new();
											try
											{
												CComPtrNtv<IPrivacySettings> cComPtrNtv_003CIPrivacySettings_003E = new();
												try
												{
													if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, ushort**, _SYSTEMTIME*, ushort**, ushort**, ushort**, ushort**, ushort**, IAddress**, INewsletterSettings**, IPrivacySettings**, IPassportIdentity**, ICreditCard**, int>)(*(ulong*)(*(long*)pAccountUser + 32)))((nint)pAccountUser, (ushort**)(&wBSTRString), (ushort**)(&wBSTRString2), &stValue, (ushort**)(&wBSTRString3), (ushort**)(&wBSTRString4), (ushort**)(&wBSTRString5), (ushort**)(&wBSTRString6), (ushort**)(&wBSTRString7), (IAddress**)(cComPtrNtv_003CIAddress_003E.p), (INewsletterSettings**)(cComPtrNtv_003CINewsletterSettings_003E.p), (IPrivacySettings**)(&ptrNtvIPrivacySettings), null, null) >= 0)
													{
														m_zuneTag = new string((char*)(*(ulong*)(&wBSTRString)));
														m_locale = new string((char*)(*(ulong*)(&wBSTRString2)));
														m_firstName = new string((char*)(*(ulong*)(&wBSTRString3)));
														m_lastName = new string((char*)(*(ulong*)(&wBSTRString4)));
														m_email = new string((char*)(*(ulong*)(&wBSTRString5)));
														m_phoneNumber = new string((char*)(*(ulong*)(&wBSTRString6)));
														m_mobilePhoneNumber = new string((char*)(*(ulong*)(&wBSTRString7)));
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
												catch
												{
													//try-fault
													Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<IPrivacySettings*, void>)(&Module.CComPtrNtv_003CIPrivacySettings_003E_002E_007Bdtor_007D), cComPtrNtv_003CIPrivacySettings_003E.p);
													throw;
												}
												cComPtrNtv_003CIPrivacySettings_003E.Dispose();
											}
											catch
											{
												//try-fault
												Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<INewsletterSettings*, void>)(&Module.CComPtrNtv_003CINewsletterSettings_003E_002E_007Bdtor_007D), cComPtrNtv_003CINewsletterSettings_003E.p);
												throw;
											}
											cComPtrNtv_003CINewsletterSettings_003E.Dispose();
										}
										catch
										{
											//try-fault
											Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<IAddress*, void>)(&Module.CComPtrNtv_003CIAddress_003E_002E_007Bdtor_007D), cComPtrNtv_003CIAddress_003E.p);
											throw;
										}
										cComPtrNtv_003CIAddress_003E.Dispose();
									}
									catch
									{
										//try-fault
										Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&Module.WBSTRString_002E_007Bdtor_007D), &wBSTRString7);
										throw;
									}
									Module.WBSTRString_002E_007Bdtor_007D(&wBSTRString7);
								}
								catch
								{
									//try-fault
									Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&Module.WBSTRString_002E_007Bdtor_007D), &wBSTRString6);
									throw;
								}
								Module.WBSTRString_002E_007Bdtor_007D(&wBSTRString6);
							}
							catch
							{
								//try-fault
								Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&Module.WBSTRString_002E_007Bdtor_007D), &wBSTRString5);
								throw;
							}
							Module.WBSTRString_002E_007Bdtor_007D(&wBSTRString5);
						}
						catch
						{
							//try-fault
							Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&Module.WBSTRString_002E_007Bdtor_007D), &wBSTRString4);
							throw;
						}
						Module.WBSTRString_002E_007Bdtor_007D(&wBSTRString4);
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&Module.WBSTRString_002E_007Bdtor_007D), &wBSTRString3);
						throw;
					}
					Module.WBSTRString_002E_007Bdtor_007D(&wBSTRString3);
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&Module.WBSTRString_002E_007Bdtor_007D), &wBSTRString2);
					throw;
				}
				Module.WBSTRString_002E_007Bdtor_007D(&wBSTRString2);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&Module.WBSTRString_002E_007Bdtor_007D), &wBSTRString);
				throw;
			}
			Module.WBSTRString_002E_007Bdtor_007D(&wBSTRString);
		}

		public AccountUser()
		{
			m_accountUserType = AccountUserType.Unknown;
		}
	}
}
