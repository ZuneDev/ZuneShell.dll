using System;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service
{
	public class PriceInfo
	{
		private int m_pointsPrice;

		private double m_currencyPrice;

		private bool m_hasPoints;

		private bool m_hasCurrency;

		private string m_displayPrice;

		private string m_currencyCode;

		public string CurrencyCode => m_currencyCode;

		public string DisplayPrice => m_displayPrice;

		public bool HasCurrency
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_hasCurrency;
			}
		}

		public bool HasPoints
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_hasPoints;
			}
		}

		public double CurrencyPrice => m_currencyPrice;

		public int PointsPrice => m_pointsPrice;

		internal unsafe PriceInfo(IPriceInfo* pPriceInfo)
		{
			Init(pPriceInfo);
		}

		internal unsafe PriceInfo()
		{
			//IL_000e: Expected I, but got I8
			Init(null);
		}

		public unsafe PriceInfo(int pointsPrice)
		{
			//IL_000e: Expected I, but got I8
			Init(null);
			m_pointsPrice = pointsPrice;
		}

		public void MakeFree()
		{
			m_pointsPrice = 0;
			m_currencyPrice = 0.0;
			m_displayPrice = null;
		}

		public static PriceInfo FreeWithPoints()
		{
			return new PriceInfo();
		}

		internal unsafe void Init(IPriceInfo* pPriceInfo)
		{
			//IL_0038: Expected I, but got I8
			//IL_004b: Expected I, but got I8
			//IL_005d: Expected I, but got I8
			//IL_0079: Expected I, but got I8
			//IL_00a7: Expected I, but got I8
			//IL_00b7: Expected I, but got I8
			//IL_00cb: Expected I, but got I8
			//IL_00db: Expected I, but got I8
			m_pointsPrice = 0;
			m_currencyPrice = 0.0;
			m_hasPoints = true;
			m_hasCurrency = false;
			if (pPriceInfo == null)
			{
				return;
			}
			m_pointsPrice = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pPriceInfo + 24)))((nint)pPriceInfo);
			m_currencyPrice = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, double>)(*(ulong*)(*(long*)pPriceInfo + 32)))((nint)pPriceInfo);
			bool flag = (m_hasPoints = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pPriceInfo + 40)))((nint)pPriceInfo) != 0) ? true : false));
			bool flag2 = (m_hasCurrency = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pPriceInfo + 48)))((nint)pPriceInfo) != 0) ? true : false));
			string wBSTRString = "";
			fixed (char* wBSTRStringPtr = wBSTRString)
			{
				string wBSTRString2 = "";
				fixed (char* wBSTRStringPtr2 = wBSTRString2)
				{
					int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pPriceInfo + 64)))((nint)pPriceInfo, (ushort**)wBSTRStringPtr);
					if (num >= 0)
					{
						m_displayPrice = wBSTRString;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pPriceInfo + 72)))((nint)pPriceInfo, (ushort**)wBSTRString2Ptr);
						if (num >= 0)
						{
							m_currencyCode = wBSTRString2;
						}
					}
				}
			}
		}
	}
}
