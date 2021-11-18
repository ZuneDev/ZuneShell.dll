using System;

namespace Microsoft.Zune.Service
{
	public class Address
	{
		private string m_street1;

		private string m_street2;

		private string m_city;

		private string m_district;

		private string m_state;

		private string m_postalCode;

		public string PostalCode
		{
			get
			{
				return m_postalCode;
			}
			set
			{
				m_postalCode = value;
			}
		}

		public string State
		{
			get
			{
				return m_state;
			}
			set
			{
				m_state = value;
			}
		}

		public string District
		{
			get
			{
				return m_district;
			}
			set
			{
				m_district = value;
			}
		}

		public string City
		{
			get
			{
				return m_city;
			}
			set
			{
				m_city = value;
			}
		}

		public string Street2
		{
			get
			{
				return m_street2;
			}
			set
			{
				m_street2 = value;
			}
		}

		public string Street1
		{
			get
			{
				return m_street1;
			}
			set
			{
				m_street1 = value;
			}
		}

		internal unsafe Address(IAddress* pAddress)
		{
			//IL_0055: Expected I, but got I8
			//IL_0063: Expected I, but got I8
			//IL_0071: Expected I, but got I8
			//IL_007f: Expected I, but got I8
			//IL_008d: Expected I, but got I8
			//IL_009b: Expected I, but got I8
			//IL_00a9: Expected I, but got I8
			if (pAddress == null)
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
									if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, ushort**, ushort**, ushort**, ushort**, ushort**, int>)(*(ulong*)(*(long*)pAddress + 32)))((nint)pAddress, (ushort**)(&wBSTRString), (ushort**)(&wBSTRString2), (ushort**)(&wBSTRString3), (ushort**)(&wBSTRString4), (ushort**)(&wBSTRString5), (ushort**)(&wBSTRString6)) >= 0)
									{
										m_street1 = new string((char*)(*(ulong*)(&wBSTRString)));
										m_street2 = new string((char*)(*(ulong*)(&wBSTRString2)));
										m_city = new string((char*)(*(ulong*)(&wBSTRString3)));
										m_state = new string((char*)(*(ulong*)(&wBSTRString4)));
										m_district = new string((char*)(*(ulong*)(&wBSTRString5)));
										m_postalCode = new string((char*)(*(ulong*)(&wBSTRString6)));
									}
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

		public Address(string street1, string street2, string city, string district, string state, string postalCode)
		{
			m_street1 = street1;
			m_street2 = street2;
			m_city = city;
			m_district = district;
			m_state = state;
			m_postalCode = postalCode;
		}

		public Address()
		{
		}
	}
}
