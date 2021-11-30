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
			string wBSTRString = "";
			fixed (char* wBSTRStringPtr = wBSTRString)
			{
				string wBSTRString2 = "";
				fixed (char* wBSTRStringPtr2 = wBSTRString2)
				{
					string wBSTRString3 = "";
					fixed (char* wBSTRStringPtr3 = wBSTRString3)
					{
						string wBSTRString4 = "";
						fixed (char* wBSTRStringPtr4 = wBSTRString4)
						{
							string wBSTRString5 = "";
							fixed (char* wBSTRStringPtr5 = wBSTRString5)
							{
								string wBSTRString6 = "";
								fixed (char* wBSTRStringPtr6 = wBSTRString6)
								{
									if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, ushort**, ushort**, ushort**, ushort**, ushort**, int>)(*(ulong*)(*(long*)pAddress + 32)))((nint)pAddress, (ushort**)wBSTRStringPtr, (ushort**)wBSTRStringPtr2, (ushort**)wBSTRStringPtr3, (ushort**)wBSTRStringPtr4, (ushort**)wBSTRStringPtr5, (ushort**)wBSTRStringPtr6) >= 0)
									{
										m_street1 = wBSTRString;
										m_street2 = wBSTRString2;
										m_city = wBSTRString3;
										m_state = wBSTRString4;
										m_district = wBSTRString5;
										m_postalCode = wBSTRString6;
									}
								}
							}
						}
					}
				}
			}
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
