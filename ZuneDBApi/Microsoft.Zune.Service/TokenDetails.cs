using System;
using System.Runtime.CompilerServices;

namespace Microsoft.Zune.Service;

public class TokenDetails : PaymentInstrument
{
	private string m_token;

	private string m_offerName;

	private string m_sellText;

	private ETokenType m_eTokenType;

	private EPurchaseOfferType m_ePurchaseOfferType;

	private Guid m_mediaId;

	private Guid m_mediaOfferId;

	private ulong m_billingOfferId;

	public Guid MediaOfferId => m_mediaOfferId;

	public Guid MediaId => m_mediaId;

	public EPurchaseOfferType PurchaseOfferType => m_ePurchaseOfferType;

	public string SellText => m_sellText;

	public string OfferName => m_offerName;

	public ulong BillingOfferId => m_billingOfferId;

	public ETokenType TokenType => m_eTokenType;

	public string Token => base.Id;

	internal unsafe TokenDetails(ITokenDetails* pTokenDetails)
		: base(null, PaymentType.Token)
	{
		//IL_0059: Expected I, but got I8
		//IL_0067: Expected I, but got I8
		//IL_0075: Expected I, but got I8
		//IL_0083: Expected I, but got I8
		if (pTokenDetails == null)
		{
			return;
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out WBSTRString wBSTRString);
		global::_003CModule_003E.WBSTRString_002E_007Bctor_007D(&wBSTRString);
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out WBSTRString wBSTRString2);
			global::_003CModule_003E.WBSTRString_002E_007Bctor_007D(&wBSTRString2);
			try
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out WBSTRString wBSTRString3);
				global::_003CModule_003E.WBSTRString_002E_007Bctor_007D(&wBSTRString3);
				try
				{
					global::ETokenType eTokenType = (global::ETokenType)(-1);
					global::EPurchaseOfferType ePurchaseOfferType = (global::EPurchaseOfferType)(-1);
					_GUID gUID_NULL = global::_003CModule_003E.GUID_NULL;
					_GUID gUID_NULL2 = global::_003CModule_003E.GUID_NULL;
					ulong billingOfferId = 0uL;
					if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, ushort**, ushort**, global::ETokenType*, global::EPurchaseOfferType*, ulong*, _GUID*, _GUID*, int>)(*(ulong*)(*(long*)pTokenDetails + 24)))((nint)pTokenDetails, (ushort**)(&wBSTRString), (ushort**)(&wBSTRString2), (ushort**)(&wBSTRString3), &eTokenType, &ePurchaseOfferType, &billingOfferId, &gUID_NULL, &gUID_NULL2) >= 0)
					{
						base.Id = new string((char*)(*(ulong*)(&wBSTRString)));
						m_offerName = new string((char*)(*(ulong*)(&wBSTRString2)));
						m_sellText = new string((char*)(*(ulong*)(&wBSTRString3)));
						m_eTokenType = (ETokenType)eTokenType;
						m_ePurchaseOfferType = (EPurchaseOfferType)ePurchaseOfferType;
						Guid mediaOfferId = global::_003CModule_003E.GUIDToGuid(gUID_NULL);
						m_mediaOfferId = mediaOfferId;
						Guid mediaId = global::_003CModule_003E.GUIDToGuid(gUID_NULL2);
						m_mediaId = mediaId;
						m_billingOfferId = billingOfferId;
					}
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D), &wBSTRString3);
					throw;
				}
				global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D(&wBSTRString3);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D), &wBSTRString2);
				throw;
			}
			global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D(&wBSTRString2);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D), &wBSTRString);
			throw;
		}
		global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D(&wBSTRString);
	}
}
