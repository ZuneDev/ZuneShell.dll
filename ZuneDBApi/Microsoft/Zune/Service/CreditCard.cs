using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service
{
	public class CreditCard : PaymentInstrument
	{
		private Address m_address;

		private CreditCardType m_creditCardType;

		private string m_contactFirstName;

		private string m_contactLastName;

		private string m_accountHolderName;

		private string m_accountNumber;

		private string m_ccvNumber;

		private DateTime m_expirationDate;

		private string m_phonePrefix;

		private string m_phoneNumber;

		private string m_phoneExtension;

		private string m_email;

		private string m_locale;

		private bool m_parentCreditCard;

		public bool ParentCreditCard
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_parentCreditCard;
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				m_parentCreditCard = value;
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

		public string PhoneExtension
		{
			get
			{
				return m_phoneExtension;
			}
			set
			{
				m_phoneExtension = value;
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

		public string PhonePrefix
		{
			get
			{
				return m_phonePrefix;
			}
			set
			{
				m_phonePrefix = value;
			}
		}

		public DateTime ExpirationDate
		{
			get
			{
				return m_expirationDate;
			}
			set
			{
				m_expirationDate = value;
			}
		}

		public string CCVNumber
		{
			get
			{
				return m_ccvNumber;
			}
			set
			{
				m_ccvNumber = value;
			}
		}

		public string AccountNumber
		{
			get
			{
				return m_accountNumber;
			}
			set
			{
				m_accountNumber = value;
			}
		}

		public string ContactLastName
		{
			get
			{
				return m_contactLastName;
			}
			set
			{
				m_contactLastName = value;
			}
		}

		public string ContactFirstName
		{
			get
			{
				return m_contactFirstName;
			}
			set
			{
				m_contactFirstName = value;
			}
		}

		public string AccountHolderName
		{
			get
			{
				return m_accountHolderName;
			}
			set
			{
				m_accountHolderName = value;
			}
		}

		public CreditCardType CreditCardType
		{
			get
			{
				return m_creditCardType;
			}
			set
			{
				m_creditCardType = value;
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

		internal unsafe CreditCard(ICreditCard* pAddress)
			: base(null, PaymentType.CreditCard)
		{
			//IL_0064: Expected I4, but got I8
			//IL_0087: Expected I, but got I8
			//IL_0098: Expected I, but got I8
			//IL_00ad: Expected I, but got I8
			//IL_00bb: Expected I, but got I8
			//IL_00c9: Expected I, but got I8
			//IL_00d7: Expected I, but got I8
			//IL_00e5: Expected I, but got I8
			//IL_00f3: Expected I, but got I8
			//IL_0101: Expected I, but got I8
			//IL_010f: Expected I, but got I8
			//IL_0130: Expected I, but got I8
			if (pAddress == null)
			{
				return;
			}
			CComPtrNtv<IAddress> cComPtrNtv_003CIAddress_003E = new();
			try
			{
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
											string wBSTRString8 = "";
											fixed (char* wBSTRString8Ptr = wBSTRString8)
											{
												_SYSTEMTIME stValue = default;
												ECreditCardType creditCardType;
												if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IAddress**, ECreditCardType*, ushort**, ushort**, ushort**, ushort**, ushort**, ushort**, ushort**, ushort**, _SYSTEMTIME*, int>)(*(ulong*)(*(long*)pAddress + 32)))((nint)pAddress, (IAddress**)(cComPtrNtv_003CIAddress_003E.p), &creditCardType, (ushort**)(&wBSTRStringPtr), (ushort**)(&wBSTRString2Ptr), (ushort**)(&wBSTRString3Ptr), (ushort**)(&wBSTRString4Ptr), (ushort**)(&wBSTRString5Ptr), (ushort**)(&wBSTRString6Ptr), (ushort**)(&wBSTRString7Ptr), (ushort**)(&wBSTRString8Ptr), &stValue) >= 0)
												{
													m_address = new Address((IAddress*)(*(ulong*)(cComPtrNtv_003CIAddress_003E.p)));
													m_creditCardType = (CreditCardType)creditCardType;
													m_accountHolderName = wBSTRString;
													m_accountNumber = wBSTRString2;
													m_ccvNumber = wBSTRString3;
													m_locale = wBSTRString4;
													m_phoneNumber = wBSTRString5;
													m_email = wBSTRString6;
													m_contactFirstName = wBSTRString7;
													m_contactLastName = wBSTRString8;
													DateTime dateTime = (m_expirationDate = Module.SystemTimeToDateTime(stValue));
													m_parentCreditCard = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte>)(*(ulong*)(*(long*)pAddress + 48)))((nint)pAddress) != 0;
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			finally
			{
				cComPtrNtv_003CIAddress_003E.Dispose();
			}
		}

		public CreditCard(string id, Address address, CreditCardType creditCardType, string accountHolderName, string accountNumber, string ccvNumber, DateTime expirationDate, string phonePrefix, string phoneNumber, string phoneExtension)
			: base(id, PaymentType.CreditCard)
		{
			m_address = address;
			m_creditCardType = creditCardType;
			m_accountNumber = accountNumber;
			m_ccvNumber = ccvNumber;
			m_expirationDate = expirationDate;
			m_phonePrefix = phonePrefix;
			m_phoneNumber = phoneNumber;
			m_phoneExtension = phoneExtension;
			m_accountHolderName = accountHolderName;
		}

		public CreditCard()
			: base(null, PaymentType.CreditCard)
		{
			m_address = new Address();
		}

		public override string ToString()
		{
			if (m_accountNumber != null && m_accountNumber.Length > 4)
			{
				string accountNumber = m_accountNumber;
				string text = accountNumber;
				return accountNumber.Substring(text.Length - 4, 4);
			}
			return m_accountNumber;
		}
	}
}
