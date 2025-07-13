using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using _003CCppImplementationDetails_003E;

namespace Microsoft.Zune.Service;

public class AppOfferCollection : OfferCollection, IDisposable
{
	private IList m_items;

	private unsafe IAppCollection* m_pCollection;

	public IList Items => m_items;

	internal unsafe AppOfferCollection()
	{
		//IL_0015: Expected I, but got I8
		m_items = null;
		m_pCollection = null;
	}

	private void _007EAppOfferCollection()
	{
		_0021AppOfferCollection();
	}

	private unsafe void _0021AppOfferCollection()
	{
		//IL_0017: Expected I, but got I8
		//IL_0020: Expected I, but got I8
		IAppCollection* pCollection = m_pCollection;
		if (pCollection != null)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pCollection + 16)))((nint)pCollection);
			m_pCollection = null;
		}
		m_items = null;
	}

	internal unsafe int Init(IAppCollection* pCollection)
	{
		//IL_000f: Expected I, but got I8
		//IL_0216: Expected I, but got I8
		//IL_0042: Expected I, but got I8
		//IL_0042: Expected I, but got I8
		//IL_0067: Expected I, but got I8
		//IL_00a7: Expected I, but got I8
		//IL_00a7: Expected I, but got I8
		//IL_0102: Expected I, but got I8
		//IL_0102: Expected I, but got I8
		//IL_0102: Expected I, but got I8
		//IL_0136: Expected I, but got I8
		//IL_0136: Expected I, but got I8
		//IL_01c9: Expected I, but got I8
		//IL_014e: Expected I, but got I8
		//IL_0159: Expected I, but got I8
		//IL_0164: Expected I, but got I8
		//IL_016f: Expected I, but got I8
		//IL_017a: Expected I, but got I8
		//IL_0185: Expected I, but got I8
		//IL_0190: Expected I, but got I8
		//IL_0198: Expected I, but got I8
		int num = 0;
		int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pCollection + 24)))((nint)pCollection);
		IList list = new ArrayList(num2);
		int num3 = 0;
		if (0 < num2)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out AppMetadata appMetadata);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out _0024ArrayType_0024_0024_0024BY01W4EMediaRights_0040_0040 _0024ArrayType_0024_0024_0024BY01W4EMediaRights_0040_0040);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIPriceInfo_003E cComPtrNtv_003CIPriceInfo_003E);
			do
			{
				global::_003CModule_003E.AppMetadata_002E_007Bctor_007D(&appMetadata);
				try
				{
					if (num >= 0)
					{
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, AppMetadata*, IContextData**, int>)(*(ulong*)(*(long*)pCollection + 32)))((nint)pCollection, num3, &appMetadata, null);
					}
					DateTime result = default(DateTime);
					if (num >= 0)
					{
						if (System.Runtime.CompilerServices.Unsafe.As<AppMetadata, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref appMetadata, 72)) == 0L || !DateTime.TryParse(new string((char*)System.Runtime.CompilerServices.Unsafe.As<AppMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref appMetadata, 72))), out result))
						{
							result = DateTime.MinValue;
						}
						Guid id = global::_003CModule_003E.GUIDToGuid(System.Runtime.CompilerServices.Unsafe.As<AppMetadata, _GUID>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref appMetadata, 8)));
						bool inCollection = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)System.Runtime.CompilerServices.Unsafe.As<AppMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref appMetadata, 136)) + 144)))((nint)System.Runtime.CompilerServices.Unsafe.As<AppMetadata, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref appMetadata, 136))) != 0) ? true : false);
						*(int*)(&_0024ArrayType_0024_0024_0024BY01W4EMediaRights_0040_0040) = 3;
						System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY01W4EMediaRights_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY01W4EMediaRights_0040_0040, 4)) = 11;
						EMediaRights* ptr = (EMediaRights*)(&_0024ArrayType_0024_0024_0024BY01W4EMediaRights_0040_0040);
						uint num4 = 2u;
						do
						{
							*(long*)(&cComPtrNtv_003CIPriceInfo_003E) = 0L;
							try
							{
								_GUID gUID_NULL = global::_003CModule_003E.GUID_NULL;
								_GUID gUID_NULL2 = global::_003CModule_003E.GUID_NULL;
								int num5 = (int)(*ptr);
								if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, _GUID*, _GUID*, IPriceInfo**, ushort**, int>)(*(ulong*)(*(long*)System.Runtime.CompilerServices.Unsafe.As<AppMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref appMetadata, 136)) + 136)))((nint)System.Runtime.CompilerServices.Unsafe.As<AppMetadata, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref appMetadata, 136)), (EMediaRights)num5, (EMediaFormat)5, &gUID_NULL2, &gUID_NULL, (IPriceInfo**)(&cComPtrNtv_003CIPriceInfo_003E), null) >= 0)
								{
									bool isTrialPurchase = num5 == 11;
									bool previouslyPurchased = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, int, int, int>)(*(ulong*)(*(long*)System.Runtime.CompilerServices.Unsafe.As<AppMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref appMetadata, 136)) + 168)))((nint)System.Runtime.CompilerServices.Unsafe.As<AppMetadata, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref appMetadata, 136)), (EMediaRights)num5, (EMediaFormat)5, 1, 1) != 0) ? true : false);
									list.Add(new AppOffer(id, new string((char*)System.Runtime.CompilerServices.Unsafe.As<AppMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref appMetadata, 24))), new string((char*)System.Runtime.CompilerServices.Unsafe.As<AppMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref appMetadata, 48))), new string((char*)System.Runtime.CompilerServices.Unsafe.As<AppMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref appMetadata, 56))), new string((char*)System.Runtime.CompilerServices.Unsafe.As<AppMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref appMetadata, 64))), new string((char*)System.Runtime.CompilerServices.Unsafe.As<AppMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref appMetadata, 88))), new string((char*)System.Runtime.CompilerServices.Unsafe.As<AppMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref appMetadata, 96))), new string((char*)System.Runtime.CompilerServices.Unsafe.As<AppMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref appMetadata, 112))), new PriceInfo((IPriceInfo*)(*(ulong*)(&cComPtrNtv_003CIPriceInfo_003E))), result, previouslyPurchased, inCollection, isTrialPurchase));
								}
							}
							catch
							{
								//try-fault
								global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPriceInfo_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIPriceInfo_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPriceInfo_003E);
								throw;
							}
							global::_003CModule_003E.CComPtrNtv_003CIPriceInfo_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIPriceInfo_003E);
							ptr = (EMediaRights*)((ulong)(nint)ptr + 4uL);
							num4 += uint.MaxValue;
						}
						while (num4 != 0);
					}
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<AppMetadata*, void>)(&global::_003CModule_003E.AppMetadata_002E_007Bdtor_007D), &appMetadata);
					throw;
				}
				global::_003CModule_003E.AppMetadata_002E_007Bdtor_007D(&appMetadata);
				num3++;
			}
			while (num3 < num2);
			if (num < 0)
			{
				goto IL_0217;
			}
		}
		m_items = list;
		m_pCollection = pCollection;
		((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pCollection + 8)))((nint)pCollection);
		goto IL_0217;
		IL_0217:
		return num;
	}

	internal unsafe IAppCollection* GetCollection()
	{
		//IL_0016: Expected I, but got I8
		IAppCollection* pCollection = m_pCollection;
		if (pCollection != null)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pCollection + 8)))((nint)pCollection);
		}
		return m_pCollection;
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
	{
		if (P_0)
		{
			_0021AppOfferCollection();
			return;
		}
		try
		{
			_0021AppOfferCollection();
		}
		finally
		{
			base.Finalize();
		}
	}

	public virtual sealed void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	~AppOfferCollection()
	{
		Dispose(false);
	}
}
