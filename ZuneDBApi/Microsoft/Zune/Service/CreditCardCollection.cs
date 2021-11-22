using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service
{
	public class CreditCardCollection : IDisposable
	{
		private ArrayList m_items;

		private unsafe ICreditCardCollection* m_pCollection;

		public IList Items => m_items;

		internal unsafe CreditCardCollection()
		{
			//IL_0015: Expected I, but got I8
			m_items = null;
			m_pCollection = null;
		}

		private void _007ECreditCardCollection()
		{
			_0021CreditCardCollection();
		}

		private unsafe void _0021CreditCardCollection()
		{
			//IL_0017: Expected I, but got I8
			//IL_0020: Expected I, but got I8
			ICreditCardCollection* pCollection = m_pCollection;
			if (pCollection != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pCollection + 16)))((nint)pCollection);
				m_pCollection = null;
			}
			m_items = null;
		}

		internal unsafe int Init(ICreditCardCollection* pCollection)
		{
			//IL_000f: Expected I, but got I8
			//IL_0029: Expected I, but got I8
			//IL_002d: Expected I, but got I8
			//IL_0031: Expected I, but got I8
			//IL_0035: Expected I, but got I8
			//IL_0039: Expected I, but got I8
			//IL_003d: Expected I, but got I8
			//IL_0041: Expected I, but got I8
			//IL_0045: Expected I, but got I8
			//IL_0049: Expected I, but got I8
			//IL_004d: Expected I, but got I8
			//IL_0054: Expected I, but got I8
			//IL_0058: Expected I, but got I8
			//IL_005b: Expected I, but got I8
			//IL_005e: Expected I, but got I8
			//IL_0093: Expected I, but got I8
			//IL_01b4: Expected I, but got I8
			int num = 0;
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pCollection + 24)))((nint)pCollection);
			ArrayList arrayList = new ArrayList();
			int num3 = 0;
			if (0 < num2)
			{
				do
				{
					EBillingPaymentType eBillingPaymentType = (EBillingPaymentType)(-1);
					ushort* ptr = null;
					ushort* ptr2 = null;
					ushort* ptr3 = null;
					ushort* ptr4 = null;
					ushort* ptr5 = null;
					ushort* ptr6 = null;
					ushort* ptr7 = null;
					ushort* ptr8 = null;
					ushort* ptr9 = null;
					ushort* ptr10 = null;
					ECreditCardType creditCardType = (ECreditCardType)(-1);
					ushort* ptr11 = null;
					ushort* ptr12 = null;
					ushort* ptr13 = null;
					ushort* ptr14 = null;
					if (num >= 0)
					{
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, EBillingPaymentType*, ushort**, ushort**, ushort**, ushort**, ushort**, ushort**, ushort**, ushort**, ushort**, ushort**, ECreditCardType*, ushort**, ushort**, ushort**, ushort**, int>)(*(ulong*)(*(long*)pCollection + 32)))((nint)pCollection, num3, &eBillingPaymentType, &ptr, &ptr2, &ptr3, &ptr4, &ptr5, &ptr6, &ptr7, &ptr8, &ptr9, &ptr10, &creditCardType, &ptr11, &ptr12, &ptr13, &ptr14);
						if (num >= 0)
						{
							DateTime result = default(DateTime);
							if (DateTime.TryParse(new string((char*)ptr14), out result))
							{
								Address address = new Address(new string((char*)ptr2), new string((char*)ptr3), new string((char*)ptr4), new string((char*)ptr5), new string((char*)ptr6), new string((char*)ptr7));
								arrayList.Add(new CreditCard(new string((char*)ptr), address, (CreditCardType)creditCardType, new string((char*)ptr11), new string((char*)ptr12), new string((char*)ptr13), result, new string((char*)ptr8), new string((char*)ptr9), new string((char*)ptr10)));
							}
						}
					}
					Module.SysFreeString(ptr);
					Module.SysFreeString(ptr2);
					Module.SysFreeString(ptr3);
					Module.SysFreeString(ptr4);
					Module.SysFreeString(ptr5);
					Module.SysFreeString(ptr6);
					Module.SysFreeString(ptr7);
					Module.SysFreeString(ptr8);
					Module.SysFreeString(ptr9);
					Module.SysFreeString(ptr10);
					Module.SysFreeString(ptr11);
					Module.SysFreeString(ptr12);
					Module.SysFreeString(ptr13);
					Module.SysFreeString(ptr14);
					num3++;
				}
				while (num3 < num2);
				if (num < 0)
				{
					goto IL_01b5;
				}
			}
			m_items = arrayList;
			m_pCollection = pCollection;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pCollection + 8)))((nint)pCollection);
			goto IL_01b5;
			IL_01b5:
			return num;
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_0021CreditCardCollection();
				return;
			}
			try
			{
				_0021CreditCardCollection();
			}
			finally
			{
				//base.Finalize();
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		~CreditCardCollection()
		{
			Dispose(false);
		}
	}
}