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
			CComPtrNtv_003CIAddress_003E cComPtrNtv_003CIAddress_003E;
			*(long*)(&cComPtrNtv_003CIAddress_003E) = 0L;
			try
			{
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
											WBSTRString wBSTRString8;
											Module.WBSTRString_002E_007Bctor_007D(&wBSTRString8);
											try
											{
												_SYSTEMTIME stValue;
												*(short*)(&stValue) = 0;
                                                // IL initblk instruction
                                                Unsafe.InitBlockUnaligned(ref Unsafe.AddByteOffset(ref stValue, 2), 0, 14);
												ECreditCardType creditCardType;
												if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IAddress**, ECreditCardType*, ushort**, ushort**, ushort**, ushort**, ushort**, ushort**, ushort**, ushort**, _SYSTEMTIME*, int>)(*(ulong*)(*(long*)pAddress + 32)))((nint)pAddress, (IAddress**)(&cComPtrNtv_003CIAddress_003E), &creditCardType, (ushort**)(&wBSTRString), (ushort**)(&wBSTRString2), (ushort**)(&wBSTRString3), (ushort**)(&wBSTRString4), (ushort**)(&wBSTRString5), (ushort**)(&wBSTRString6), (ushort**)(&wBSTRString7), (ushort**)(&wBSTRString8), &stValue) >= 0)
												{
													m_address = new Address((IAddress*)(*(ulong*)(&cComPtrNtv_003CIAddress_003E)));
													m_creditCardType = (CreditCardType)creditCardType;
													m_accountHolderName = new string((char*)(*(ulong*)(&wBSTRString)));
													m_accountNumber = new string((char*)(*(ulong*)(&wBSTRString2)));
													m_ccvNumber = new string((char*)(*(ulong*)(&wBSTRString3)));
													m_locale = new string((char*)(*(ulong*)(&wBSTRString4)));
													m_phoneNumber = new string((char*)(*(ulong*)(&wBSTRString5)));
													m_email = new string((char*)(*(ulong*)(&wBSTRString6)));
													m_contactFirstName = new string((char*)(*(ulong*)(&wBSTRString7)));
													m_contactLastName = new string((char*)(*(ulong*)(&wBSTRString8)));
													DateTime dateTime = (m_expirationDate = Module.SystemTimeToDateTime(stValue));
													m_parentCreditCard = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte>)(*(ulong*)(*(long*)pAddress + 48)))((nint)pAddress) != 0;
												}
											}
											catch
											{
												//try-fault
												Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&Module.WBSTRString_002E_007Bdtor_007D), &wBSTRString8);
												throw;
											}
											Module.WBSTRString_002E_007Bdtor_007D(&wBSTRString8);
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
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIAddress_003E*, void>)(&Module.CComPtrNtv_003CIAddress_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIAddress_003E);
				throw;
			}
			Module.CComPtrNtv_003CIAddress_003E_002ERelease(&cComPtrNtv_003CIAddress_003E);
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
