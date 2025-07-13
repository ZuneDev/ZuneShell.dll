using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ZuneUI;

namespace Microsoft.Zune.Service;

public class AccountManagement : IDisposable
{
	private readonly CComPtrMgd_003CIAccountManagement_003E m_spAccountManagement;

	public AccountManagement()
	{
		CComPtrMgd_003CIAccountManagement_003E spAccountManagement = new CComPtrMgd_003CIAccountManagement_003E();
		try
		{
			m_spAccountManagement = spAccountManagement;
			base._002Ector();
			return;
		}
		catch
		{
			//try-fault
			((IDisposable)m_spAccountManagement).Dispose();
			throw;
		}
	}

	public unsafe HRESULT CreateAccount(PassportIdentity passportIdentity, string zuneTag, string locale, DateTime birthday, string firstName, string lastName, string email, Address address, AccountSettings accountSettings, PassportIdentity parentPassportIdentity, CreditCard parentCreditCard, out ServiceError serviceError)
	{
		//IL_0153: Expected I, but got I8
		//IL_0112: Expected I, but got I8
		//IL_0112: Expected I, but got I8
		//IL_0112: Expected I, but got I8
		//IL_0112: Expected I, but got I8
		//IL_0112: Expected I, but got I8
		//IL_0112: Expected I, but got I8
		//IL_0112: Expected I, but got I8
		int num = CreateComObject();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CINewsletterSettings_003E cComPtrNtv_003CINewsletterSettings_003E);
		global::_003CModule_003E.CComPtrNtv_003CINewsletterSettings_003E_002E_007Bctor_007D(&cComPtrNtv_003CINewsletterSettings_003E);
		HRESULT result;
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIPrivacySettings_003E cComPtrNtv_003CIPrivacySettings_003E);
			global::_003CModule_003E.CComPtrNtv_003CIPrivacySettings_003E_002E_007Bctor_007D(&cComPtrNtv_003CIPrivacySettings_003E);
			try
			{
				if (num >= 0 && accountSettings != null)
				{
					num = CreateAccountSettings(accountSettings, global::_003CModule_003E.CComPtrNtv_003CINewsletterSettings_003E_002E_0026(&cComPtrNtv_003CINewsletterSettings_003E), global::_003CModule_003E.CComPtrNtv_003CIPrivacySettings_003E_002E_0026(&cComPtrNtv_003CIPrivacySettings_003E));
				}
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E);
				global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bctor_007D(&cComPtrNtv_003CIPassportIdentity_003E);
				try
				{
					if (num >= 0 && passportIdentity != null)
					{
						num = passportIdentity.GetComPointer((IPassportIdentity**)(&cComPtrNtv_003CIPassportIdentity_003E));
					}
					System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E2);
					global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bctor_007D(&cComPtrNtv_003CIPassportIdentity_003E2);
					try
					{
						if (num >= 0 && parentPassportIdentity != null)
						{
							num = parentPassportIdentity.GetComPointer((IPassportIdentity**)(&cComPtrNtv_003CIPassportIdentity_003E2));
						}
						System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CICreditCard_003E cComPtrNtv_003CICreditCard_003E);
						*(long*)(&cComPtrNtv_003CICreditCard_003E) = 0L;
						try
						{
							if (num >= 0 && parentCreditCard != null)
							{
								num = CreateCreditCard(parentCreditCard, (ICreditCard**)(&cComPtrNtv_003CICreditCard_003E));
							}
							System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIAddress_003E cComPtrNtv_003CIAddress_003E);
							*(long*)(&cComPtrNtv_003CIAddress_003E) = 0L;
							try
							{
								if (num >= 0 && address != null)
								{
									num = CreateAddress(address, (IAddress**)(&cComPtrNtv_003CIAddress_003E));
								}
								System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIServiceError_003E cComPtrNtv_003CIServiceError_003E);
								*(long*)(&cComPtrNtv_003CIServiceError_003E) = 0L;
								try
								{
									if (num >= 0)
									{
										_SYSTEMTIME sYSTEMTIME = global::_003CModule_003E.DateTimeToSystemTime(birthday);
										fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(firstName)))
										{
											try
											{
												fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(lastName)))
												{
													try
													{
														fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(zuneTag)))
														{
															try
															{
																fixed (ushort* ptr4 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(email)))
																{
																	try
																	{
																		fixed (ushort* ptr5 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(locale)))
																		{
																			try
																			{
																				IAccountManagement* p = m_spAccountManagement.p;
																				long num2 = *(long*)p + 72;
																				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPassportIdentity*, ushort*, ushort*, _SYSTEMTIME*, ushort*, ushort*, ushort*, IAddress*, INewsletterSettings*, IPrivacySettings*, IPassportIdentity*, ICreditCard*, IServiceError**, int>)(*(ulong*)num2))((nint)p, (IPassportIdentity*)(*(ulong*)(&cComPtrNtv_003CIPassportIdentity_003E)), ptr3, ptr5, &sYSTEMTIME, ptr, ptr2, ptr4, (IAddress*)(*(ulong*)(&cComPtrNtv_003CIAddress_003E)), (INewsletterSettings*)(*(ulong*)(&cComPtrNtv_003CINewsletterSettings_003E)), (IPrivacySettings*)(*(ulong*)(&cComPtrNtv_003CIPrivacySettings_003E)), (IPassportIdentity*)(*(ulong*)(&cComPtrNtv_003CIPassportIdentity_003E2)), (ICreditCard*)(*(ulong*)(&cComPtrNtv_003CICreditCard_003E)), (IServiceError**)(&cComPtrNtv_003CIServiceError_003E));
																			}
																			catch
																			{
																				//try-fault
																				ptr5 = null;
																				throw;
																			}
																		}
																	}
																	catch
																	{
																		//try-fault
																		ptr4 = null;
																		throw;
																	}
																}
															}
															catch
															{
																//try-fault
																ptr3 = null;
																throw;
															}
														}
													}
													catch
													{
														//try-fault
														ptr2 = null;
														throw;
													}
												}
											}
											catch
											{
												//try-fault
												ptr = null;
												throw;
											}
										}
									}
									if (*(long*)(&cComPtrNtv_003CIServiceError_003E) != 0L)
									{
										serviceError = new ServiceError((IServiceError*)(*(ulong*)(&cComPtrNtv_003CIServiceError_003E)));
									}
									result = new HRESULT(num);
								}
								catch
								{
									//try-fault
									global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIServiceError_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIServiceError_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIServiceError_003E);
									throw;
								}
								global::_003CModule_003E.CComPtrNtv_003CIServiceError_003E_002ERelease(&cComPtrNtv_003CIServiceError_003E);
							}
							catch
							{
								//try-fault
								global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIAddress_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIAddress_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIAddress_003E);
								throw;
							}
							global::_003CModule_003E.CComPtrNtv_003CIAddress_003E_002ERelease(&cComPtrNtv_003CIAddress_003E);
						}
						catch
						{
							//try-fault
							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CICreditCard_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CICreditCard_003E_002E_007Bdtor_007D), &cComPtrNtv_003CICreditCard_003E);
							throw;
						}
						global::_003CModule_003E.CComPtrNtv_003CICreditCard_003E_002ERelease(&cComPtrNtv_003CICreditCard_003E);
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E2);
						throw;
					}
					global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E2);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E);
					throw;
				}
				global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPrivacySettings_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIPrivacySettings_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPrivacySettings_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CIPrivacySettings_003E_002ERelease(&cComPtrNtv_003CIPrivacySettings_003E);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CINewsletterSettings_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CINewsletterSettings_003E_002E_007Bdtor_007D), &cComPtrNtv_003CINewsletterSettings_003E);
			throw;
		}
		global::_003CModule_003E.CComPtrNtv_003CINewsletterSettings_003E_002ERelease(&cComPtrNtv_003CINewsletterSettings_003E);
		return result;
	}

	public unsafe HRESULT ReserveZuneTag(string zuneTag, string countryCode, out IList suggestedNames, out ServiceError serviceError)
	{
		//IL_007c: Expected I, but got I8
		//IL_007c: Expected I, but got I8
		//IL_00d9: Expected I, but got I8
		//IL_008f: Expected I, but got I8
		//IL_00a3: Expected I, but got I8
		//IL_00a3: Expected I, but got I8
		//IL_004e: Expected I, but got I8
		int num = CreateComObject();
		suggestedNames = null;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIAvailableZuneTagInformation_003E cComPtrNtv_003CIAvailableZuneTagInformation_003E);
		*(long*)(&cComPtrNtv_003CIAvailableZuneTagInformation_003E) = 0L;
		HRESULT result;
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIServiceError_003E cComPtrNtv_003CIServiceError_003E);
			*(long*)(&cComPtrNtv_003CIServiceError_003E) = 0L;
			try
			{
				if (num >= 0)
				{
					fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(zuneTag)))
					{
						try
						{
							fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(countryCode)))
							{
								try
								{
									IAccountManagement* p = m_spAccountManagement.p;
									long num2 = *(long*)p + 80;
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, IAvailableZuneTagInformation**, IServiceError**, int>)(*(ulong*)num2))((nint)p, ptr, ptr2, (IAvailableZuneTagInformation**)(&cComPtrNtv_003CIAvailableZuneTagInformation_003E), (IServiceError**)(&cComPtrNtv_003CIServiceError_003E));
								}
								catch
								{
									//try-fault
									ptr2 = null;
									throw;
								}
							}
						}
						catch
						{
							//try-fault
							ptr = null;
							throw;
						}
					}
				}
				if (num == -1056857549)
				{
					int num3 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIAvailableZuneTagInformation_003E)) + 48)))((nint)(*(long*)(&cComPtrNtv_003CIAvailableZuneTagInformation_003E)));
					suggestedNames = new ArrayList(num3);
					int num4 = 0;
					if (0 < num3)
					{
						do
						{
							ushort* ptr3 = null;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, ushort**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIAvailableZuneTagInformation_003E)) + 56)))((nint)(*(long*)(&cComPtrNtv_003CIAvailableZuneTagInformation_003E)), num4, &ptr3);
							if (num >= 0)
							{
								suggestedNames.Add(new string((char*)ptr3));
							}
							global::_003CModule_003E.SysFreeString(ptr3);
							if (num < 0)
							{
								break;
							}
							num4++;
						}
						while (num4 < num3);
					}
				}
				if (*(long*)(&cComPtrNtv_003CIServiceError_003E) != 0L)
				{
					serviceError = new ServiceError((IServiceError*)(*(ulong*)(&cComPtrNtv_003CIServiceError_003E)));
				}
				result = num;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIServiceError_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIServiceError_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIServiceError_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CIServiceError_003E_002ERelease(&cComPtrNtv_003CIServiceError_003E);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIAvailableZuneTagInformation_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIAvailableZuneTagInformation_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIAvailableZuneTagInformation_003E);
			throw;
		}
		global::_003CModule_003E.CComPtrNtv_003CIAvailableZuneTagInformation_003E_002ERelease(&cComPtrNtv_003CIAvailableZuneTagInformation_003E);
		return result;
	}

	public unsafe HRESULT ValidateCreditCard(PassportIdentity parentPassportIdentity, CreditCard creditCard, out ServiceError serviceError)
	{
		//IL_005c: Expected I, but got I8
		//IL_005c: Expected I, but got I8
		//IL_005c: Expected I, but got I8
		//IL_006b: Expected I, but got I8
		int num = CreateComObject();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CICreditCard_003E cComPtrNtv_003CICreditCard_003E);
		*(long*)(&cComPtrNtv_003CICreditCard_003E) = 0L;
		HRESULT result;
		try
		{
			if (num >= 0 && creditCard != null)
			{
				num = CreateCreditCard(creditCard, (ICreditCard**)(&cComPtrNtv_003CICreditCard_003E));
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E);
			*(long*)(&cComPtrNtv_003CIPassportIdentity_003E) = 0L;
			try
			{
				if (num >= 0 && parentPassportIdentity != null)
				{
					num = parentPassportIdentity.GetComPointer((IPassportIdentity**)(&cComPtrNtv_003CIPassportIdentity_003E));
				}
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIServiceError_003E cComPtrNtv_003CIServiceError_003E);
				*(long*)(&cComPtrNtv_003CIServiceError_003E) = 0L;
				try
				{
					if (num >= 0)
					{
						IAccountManagement* p = m_spAccountManagement.p;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPassportIdentity*, ICreditCard*, IServiceError**, int>)(*(ulong*)(*(long*)p + 88)))((nint)p, (IPassportIdentity*)(*(ulong*)(&cComPtrNtv_003CIPassportIdentity_003E)), (ICreditCard*)(*(ulong*)(&cComPtrNtv_003CICreditCard_003E)), (IServiceError**)(&cComPtrNtv_003CIServiceError_003E));
					}
					if (*(long*)(&cComPtrNtv_003CIServiceError_003E) != 0L)
					{
						serviceError = new ServiceError((IServiceError*)(*(ulong*)(&cComPtrNtv_003CIServiceError_003E)));
					}
					result = new HRESULT(num);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIServiceError_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIServiceError_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIServiceError_003E);
					throw;
				}
				global::_003CModule_003E.CComPtrNtv_003CIServiceError_003E_002ERelease(&cComPtrNtv_003CIServiceError_003E);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CICreditCard_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CICreditCard_003E_002E_007Bdtor_007D), &cComPtrNtv_003CICreditCard_003E);
			throw;
		}
		global::_003CModule_003E.CComPtrNtv_003CICreditCard_003E_002ERelease(&cComPtrNtv_003CICreditCard_003E);
		return result;
	}

	public unsafe HRESULT GetAccount(PassportIdentity passportIdentity, GetAccountCompleteCallback onSuccess, AccountManagementErrorCallback onError)
	{
		//IL_000a: Expected I, but got I8
		//IL_0028: Expected I, but got I8
		//IL_008b: Expected I, but got I8
		//IL_007a: Expected I, but got I8
		//IL_007a: Expected I, but got I8
		int num = CreateComObject();
		GetAccountCallbackWrapper* ptr = null;
		if (num >= 0)
		{
			GetAccountCallbackWrapper* ptr2 = (GetAccountCallbackWrapper*)global::_003CModule_003E.@new(32uL);
			GetAccountCallbackWrapper* ptr3;
			try
			{
				ptr3 = ((ptr2 == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002EService_002EGetAccountCallbackWrapper_002E_007Bctor_007D(ptr2, onSuccess, onError));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr2);
				throw;
			}
			ptr = ptr3;
			num = (((long)(nint)ptr3 == 0) ? (-2147024882) : num);
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E);
		*(long*)(&cComPtrNtv_003CIPassportIdentity_003E) = 0L;
		HRESULT result;
		try
		{
			if (num >= 0)
			{
				if (passportIdentity != null)
				{
					num = passportIdentity.GetComPointer((IPassportIdentity**)(&cComPtrNtv_003CIPassportIdentity_003E));
				}
				if (num >= 0)
				{
					IAccountManagement* p = m_spAccountManagement.p;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPassportIdentity*, IGetAccountCallback*, int>)(*(ulong*)(*(long*)p + 96)))((nint)p, (IPassportIdentity*)(*(ulong*)(&cComPtrNtv_003CIPassportIdentity_003E)), (IGetAccountCallback*)ptr);
				}
			}
			if (ptr != null)
			{
				GetAccountCallbackWrapper* intPtr = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
			}
			result = new HRESULT(num);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E);
			throw;
		}
		global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E);
		return result;
	}

	public unsafe HRESULT GetAccount(PassportIdentity passportIdentity, out AccountUser accountUser, out ServiceError serviceError)
	{
		//IL_004a: Expected I, but got I8
		//IL_004a: Expected I, but got I8
		//IL_0059: Expected I, but got I8
		//IL_0068: Expected I, but got I8
		int num = CreateComObject();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E);
		*(long*)(&cComPtrNtv_003CIPassportIdentity_003E) = 0L;
		HRESULT result;
		try
		{
			if (num >= 0 && passportIdentity != null)
			{
				num = passportIdentity.GetComPointer((IPassportIdentity**)(&cComPtrNtv_003CIPassportIdentity_003E));
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIAccountUser_003E cComPtrNtv_003CIAccountUser_003E);
			*(long*)(&cComPtrNtv_003CIAccountUser_003E) = 0L;
			try
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIServiceError_003E cComPtrNtv_003CIServiceError_003E);
				*(long*)(&cComPtrNtv_003CIServiceError_003E) = 0L;
				try
				{
					if (num >= 0)
					{
						IAccountManagement* p = m_spAccountManagement.p;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPassportIdentity*, IAccountUser**, IServiceError**, int>)(*(ulong*)(*(long*)p + 104)))((nint)p, (IPassportIdentity*)(*(ulong*)(&cComPtrNtv_003CIPassportIdentity_003E)), (IAccountUser**)(&cComPtrNtv_003CIAccountUser_003E), (IServiceError**)(&cComPtrNtv_003CIServiceError_003E));
					}
					if (*(long*)(&cComPtrNtv_003CIServiceError_003E) != 0L)
					{
						serviceError = new ServiceError((IServiceError*)(*(ulong*)(&cComPtrNtv_003CIServiceError_003E)));
					}
					if (*(long*)(&cComPtrNtv_003CIAccountUser_003E) != 0L)
					{
						accountUser = new AccountUser((IAccountUser*)(*(ulong*)(&cComPtrNtv_003CIAccountUser_003E)));
					}
					result = new HRESULT(num);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIServiceError_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIServiceError_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIServiceError_003E);
					throw;
				}
				global::_003CModule_003E.CComPtrNtv_003CIServiceError_003E_002ERelease(&cComPtrNtv_003CIServiceError_003E);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIAccountUser_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIAccountUser_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIAccountUser_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CIAccountUser_003E_002ERelease(&cComPtrNtv_003CIAccountUser_003E);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E);
			throw;
		}
		global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E);
		return result;
	}

	public unsafe HRESULT SetAccount(PassportIdentity passportIdentity, AccountUser accountUser, out ServiceError serviceError)
	{
		//IL_005c: Expected I, but got I8
		//IL_005c: Expected I, but got I8
		//IL_005c: Expected I, but got I8
		//IL_006b: Expected I, but got I8
		int num = CreateComObject();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E);
		*(long*)(&cComPtrNtv_003CIPassportIdentity_003E) = 0L;
		HRESULT result;
		try
		{
			if (num >= 0 && passportIdentity != null)
			{
				num = passportIdentity.GetComPointer((IPassportIdentity**)(&cComPtrNtv_003CIPassportIdentity_003E));
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIAccountUser_003E cComPtrNtv_003CIAccountUser_003E);
			*(long*)(&cComPtrNtv_003CIAccountUser_003E) = 0L;
			try
			{
				if (num >= 0 && accountUser != null)
				{
					num = CreateAccountUser(accountUser, (IAccountUser**)(&cComPtrNtv_003CIAccountUser_003E));
				}
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIServiceError_003E cComPtrNtv_003CIServiceError_003E);
				*(long*)(&cComPtrNtv_003CIServiceError_003E) = 0L;
				try
				{
					if (num >= 0)
					{
						IAccountManagement* p = m_spAccountManagement.p;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPassportIdentity*, IAccountUser*, IServiceError**, int>)(*(ulong*)(*(long*)p + 112)))((nint)p, (IPassportIdentity*)(*(ulong*)(&cComPtrNtv_003CIPassportIdentity_003E)), (IAccountUser*)(*(ulong*)(&cComPtrNtv_003CIAccountUser_003E)), (IServiceError**)(&cComPtrNtv_003CIServiceError_003E));
					}
					if (*(long*)(&cComPtrNtv_003CIServiceError_003E) != 0L)
					{
						serviceError = new ServiceError((IServiceError*)(*(ulong*)(&cComPtrNtv_003CIServiceError_003E)));
					}
					result = new HRESULT(num);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIServiceError_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIServiceError_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIServiceError_003E);
					throw;
				}
				global::_003CModule_003E.CComPtrNtv_003CIServiceError_003E_002ERelease(&cComPtrNtv_003CIServiceError_003E);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIAccountUser_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIAccountUser_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIAccountUser_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CIAccountUser_003E_002ERelease(&cComPtrNtv_003CIAccountUser_003E);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E);
			throw;
		}
		global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E);
		return result;
	}

	public unsafe HRESULT SetNewsLetterSettings(AccountSettings accountSettings, out ServiceError serviceError)
	{
		//IL_001e: Expected I, but got I8
		//IL_0046: Expected I, but got I8
		//IL_0046: Expected I, but got I8
		//IL_0055: Expected I, but got I8
		int num = CreateComObject();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CINewsletterSettings_003E cComPtrNtv_003CINewsletterSettings_003E);
		*(long*)(&cComPtrNtv_003CINewsletterSettings_003E) = 0L;
		HRESULT result;
		try
		{
			if (num >= 0 && accountSettings != null)
			{
				num = CreateAccountSettings(accountSettings, (INewsletterSettings**)(&cComPtrNtv_003CINewsletterSettings_003E), null);
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIServiceError_003E cComPtrNtv_003CIServiceError_003E);
			*(long*)(&cComPtrNtv_003CIServiceError_003E) = 0L;
			try
			{
				if (num >= 0)
				{
					IAccountManagement* p = m_spAccountManagement.p;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, INewsletterSettings*, IServiceError**, int>)(*(ulong*)(*(long*)p + 120)))((nint)p, (INewsletterSettings*)(*(ulong*)(&cComPtrNtv_003CINewsletterSettings_003E)), (IServiceError**)(&cComPtrNtv_003CIServiceError_003E));
				}
				if (*(long*)(&cComPtrNtv_003CIServiceError_003E) != 0L)
				{
					serviceError = new ServiceError((IServiceError*)(*(ulong*)(&cComPtrNtv_003CIServiceError_003E)));
				}
				result = new HRESULT(num);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIServiceError_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIServiceError_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIServiceError_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CIServiceError_003E_002ERelease(&cComPtrNtv_003CIServiceError_003E);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CINewsletterSettings_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CINewsletterSettings_003E_002E_007Bdtor_007D), &cComPtrNtv_003CINewsletterSettings_003E);
			throw;
		}
		global::_003CModule_003E.CComPtrNtv_003CINewsletterSettings_003E_002ERelease(&cComPtrNtv_003CINewsletterSettings_003E);
		return result;
	}

	public unsafe HRESULT SetPrivacySettings(AccountSettings accountSettings, PassportIdentity parentPassportIdentity, out ServiceError serviceError)
	{
		//IL_001e: Expected I, but got I8
		//IL_0061: Expected I, but got I8
		//IL_0061: Expected I, but got I8
		//IL_0061: Expected I, but got I8
		//IL_0070: Expected I, but got I8
		int num = CreateComObject();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIPrivacySettings_003E cComPtrNtv_003CIPrivacySettings_003E);
		*(long*)(&cComPtrNtv_003CIPrivacySettings_003E) = 0L;
		HRESULT result;
		try
		{
			if (num >= 0 && accountSettings != null)
			{
				num = CreateAccountSettings(accountSettings, null, (IPrivacySettings**)(&cComPtrNtv_003CIPrivacySettings_003E));
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E);
			*(long*)(&cComPtrNtv_003CIPassportIdentity_003E) = 0L;
			try
			{
				if (num >= 0 && parentPassportIdentity != null)
				{
					num = parentPassportIdentity.GetComPointer((IPassportIdentity**)(&cComPtrNtv_003CIPassportIdentity_003E));
				}
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIServiceError_003E cComPtrNtv_003CIServiceError_003E);
				*(long*)(&cComPtrNtv_003CIServiceError_003E) = 0L;
				try
				{
					if (num >= 0)
					{
						IAccountManagement* p = m_spAccountManagement.p;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPrivacySettings*, IPassportIdentity*, IServiceError**, int>)(*(ulong*)(*(long*)p + 128)))((nint)p, (IPrivacySettings*)(*(ulong*)(&cComPtrNtv_003CIPrivacySettings_003E)), (IPassportIdentity*)(*(ulong*)(&cComPtrNtv_003CIPassportIdentity_003E)), (IServiceError**)(&cComPtrNtv_003CIServiceError_003E));
					}
					if (*(long*)(&cComPtrNtv_003CIServiceError_003E) != 0L)
					{
						serviceError = new ServiceError((IServiceError*)(*(ulong*)(&cComPtrNtv_003CIServiceError_003E)));
					}
					result = new HRESULT(num);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIServiceError_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIServiceError_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIServiceError_003E);
					throw;
				}
				global::_003CModule_003E.CComPtrNtv_003CIServiceError_003E_002ERelease(&cComPtrNtv_003CIServiceError_003E);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPrivacySettings_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIPrivacySettings_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPrivacySettings_003E);
			throw;
		}
		global::_003CModule_003E.CComPtrNtv_003CIPrivacySettings_003E_002ERelease(&cComPtrNtv_003CIPrivacySettings_003E);
		return result;
	}

	public unsafe HRESULT UpgradeAccount(PassportIdentity passportIdentity, AccountSettings accountSettings, PassportIdentity parentPassportIdentity, out ServiceError serviceError)
	{
		//IL_0081: Expected I, but got I8
		//IL_0081: Expected I, but got I8
		//IL_0081: Expected I, but got I8
		//IL_0081: Expected I, but got I8
		//IL_0081: Expected I, but got I8
		//IL_0091: Expected I, but got I8
		int num = CreateComObject();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CINewsletterSettings_003E cComPtrNtv_003CINewsletterSettings_003E);
		*(long*)(&cComPtrNtv_003CINewsletterSettings_003E) = 0L;
		HRESULT result;
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIPrivacySettings_003E cComPtrNtv_003CIPrivacySettings_003E);
			*(long*)(&cComPtrNtv_003CIPrivacySettings_003E) = 0L;
			try
			{
				if (num >= 0 && accountSettings != null)
				{
					num = CreateAccountSettings(accountSettings, (INewsletterSettings**)(&cComPtrNtv_003CINewsletterSettings_003E), (IPrivacySettings**)(&cComPtrNtv_003CIPrivacySettings_003E));
				}
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E);
				*(long*)(&cComPtrNtv_003CIPassportIdentity_003E) = 0L;
				try
				{
					if (num >= 0 && passportIdentity != null)
					{
						num = passportIdentity.GetComPointer((IPassportIdentity**)(&cComPtrNtv_003CIPassportIdentity_003E));
					}
					System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E2);
					*(long*)(&cComPtrNtv_003CIPassportIdentity_003E2) = 0L;
					try
					{
						if (num >= 0 && parentPassportIdentity != null)
						{
							num = parentPassportIdentity.GetComPointer((IPassportIdentity**)(&cComPtrNtv_003CIPassportIdentity_003E2));
						}
						System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIServiceError_003E cComPtrNtv_003CIServiceError_003E);
						*(long*)(&cComPtrNtv_003CIServiceError_003E) = 0L;
						try
						{
							if (num >= 0)
							{
								IAccountManagement* p = m_spAccountManagement.p;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPassportIdentity*, INewsletterSettings*, IPrivacySettings*, IPassportIdentity*, IServiceError**, int>)(*(ulong*)(*(long*)p + 136)))((nint)p, (IPassportIdentity*)(*(ulong*)(&cComPtrNtv_003CIPassportIdentity_003E)), (INewsletterSettings*)(*(ulong*)(&cComPtrNtv_003CINewsletterSettings_003E)), (IPrivacySettings*)(*(ulong*)(&cComPtrNtv_003CIPrivacySettings_003E)), (IPassportIdentity*)(*(ulong*)(&cComPtrNtv_003CIPassportIdentity_003E2)), (IServiceError**)(&cComPtrNtv_003CIServiceError_003E));
							}
							if (*(long*)(&cComPtrNtv_003CIServiceError_003E) != 0L)
							{
								serviceError = new ServiceError((IServiceError*)(*(ulong*)(&cComPtrNtv_003CIServiceError_003E)));
							}
							result = new HRESULT(num);
						}
						catch
						{
							//try-fault
							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIServiceError_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIServiceError_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIServiceError_003E);
							throw;
						}
						global::_003CModule_003E.CComPtrNtv_003CIServiceError_003E_002ERelease(&cComPtrNtv_003CIServiceError_003E);
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E2);
						throw;
					}
					global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E2);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E);
					throw;
				}
				global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPrivacySettings_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIPrivacySettings_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPrivacySettings_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CIPrivacySettings_003E_002ERelease(&cComPtrNtv_003CIPrivacySettings_003E);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CINewsletterSettings_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CINewsletterSettings_003E_002E_007Bdtor_007D), &cComPtrNtv_003CINewsletterSettings_003E);
			throw;
		}
		global::_003CModule_003E.CComPtrNtv_003CINewsletterSettings_003E_002ERelease(&cComPtrNtv_003CINewsletterSettings_003E);
		return result;
	}

	public unsafe HRESULT GetTermsOfService(string languageCode, string countryCode, out string termsOfService)
	{
		//IL_0045: Expected I, but got I8
		//IL_0065: Expected I, but got I8
		int num = CreateComObject();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out WBSTRString wBSTRString);
		global::_003CModule_003E.WBSTRString_002E_007Bctor_007D(&wBSTRString);
		HRESULT result;
		try
		{
			if (num >= 0)
			{
				fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(countryCode)))
				{
					try
					{
						fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(languageCode)))
						{
							try
							{
								IAccountManagement* p = m_spAccountManagement.p;
								long num2 = *(long*)p + 144;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, ushort**, int>)(*(ulong*)num2))((nint)p, ptr2, ptr, (ushort**)(&wBSTRString));
							}
							catch
							{
								//try-fault
								ptr2 = null;
								throw;
							}
						}
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
				}
				if (num >= 0)
				{
					termsOfService = new string((char*)(*(ulong*)(&wBSTRString)));
				}
			}
			result = new HRESULT(num);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D), &wBSTRString);
			throw;
		}
		global::_003CModule_003E.WBSTRString_002E_007Bdtor_007D(&wBSTRString);
		return result;
	}

	public unsafe HRESULT GetSubscriptionDetails(string offerId, out ArrayList bulletStrings)
	{
		//IL_000a: Expected I, but got I8
		//IL_003d: Expected I, but got I8
		//IL_0086: Expected I, but got I8
		int num = CreateComObject();
		tagSAFEARRAY* ptr = null;
		if (num >= 0)
		{
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(offerId)))
			{
				try
				{
					IAccountManagement* p = m_spAccountManagement.p;
					long num2 = *(long*)p + 152;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, tagSAFEARRAY**, int>)(*(ulong*)num2))((nint)p, ptr2, &ptr);
				}
				catch
				{
					//try-fault
					ptr2 = null;
					throw;
				}
			}
		}
		int num3 = 0;
		if (num >= 0)
		{
			num = global::_003CModule_003E.SafeArrayGetUBound(ptr, 1u, &num3);
		}
		int num4 = 0;
		if (num >= 0)
		{
			num = global::_003CModule_003E.SafeArrayGetLBound(ptr, 1u, &num4);
			if (num >= 0)
			{
				bulletStrings = new ArrayList(num3 - num4 + 1);
				int num5 = num4;
				if (num4 <= num3)
				{
					do
					{
						ushort* value = null;
						num = global::_003CModule_003E.SafeArrayGetElement(ptr, &num5, &value);
						if (num < 0)
						{
							break;
						}
						bulletStrings.Add(new string((char*)value));
						num5++;
					}
					while (num5 <= num3);
				}
			}
		}
		if (ptr != null)
		{
			global::_003CModule_003E.SafeArrayDestroy(ptr);
		}
		return new HRESULT(num);
	}

	private unsafe int CreateAddress(Address address, IAddress** ppAddress)
	{
		//IL_0033: Expected I, but got I8
		//IL_00a6: Expected I, but got I8
		//IL_00a6: Expected I, but got I8
		//IL_00f3: Expected I, but got I8
		//IL_00fc: Expected I8, but got I
		int num = CreateComObject();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIAddress_003E cComPtrNtv_003CIAddress_003E);
		*(long*)(&cComPtrNtv_003CIAddress_003E) = 0L;
		try
		{
			if (num >= 0)
			{
				IAccountManagement* p = m_spAccountManagement.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IAddress**, int>)(*(ulong*)(*(long*)p + 32)))((nint)p, (IAddress**)(&cComPtrNtv_003CIAddress_003E));
				if (num >= 0)
				{
					fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(address.Street1)))
					{
						try
						{
							fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(address.Street2)))
							{
								try
								{
									fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(address.City)))
									{
										try
										{
											fixed (ushort* ptr4 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(address.State)))
											{
												try
												{
													fixed (ushort* ptr5 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(address.District)))
													{
														try
														{
															fixed (ushort* ptr6 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(address.PostalCode)))
															{
																try
																{
																	long num2 = *(long*)(*(ulong*)(&cComPtrNtv_003CIAddress_003E)) + 24;
																	num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, ushort*, ushort*, ushort*, ushort*, int>)(*(ulong*)num2))((nint)(*(long*)(&cComPtrNtv_003CIAddress_003E)), ptr, ptr2, ptr3, ptr4, ptr5, ptr6);
																}
																catch
																{
																	//try-fault
																	ptr6 = null;
																	throw;
																}
															}
														}
														catch
														{
															//try-fault
															ptr5 = null;
															throw;
														}
													}
												}
												catch
												{
													//try-fault
													ptr4 = null;
													throw;
												}
											}
										}
										catch
										{
											//try-fault
											ptr3 = null;
											throw;
										}
									}
								}
								catch
								{
									//try-fault
									ptr2 = null;
									throw;
								}
							}
						}
						catch
						{
							//try-fault
							ptr = null;
							throw;
						}
					}
					if (num >= 0 && ppAddress != null)
					{
						IAddress* ptr7 = (IAddress*)(*(ulong*)(&cComPtrNtv_003CIAddress_003E));
						*(long*)(&cComPtrNtv_003CIAddress_003E) = 0L;
						*(long*)ppAddress = (nint)ptr7;
					}
				}
			}
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIAddress_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIAddress_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIAddress_003E);
			throw;
		}
		global::_003CModule_003E.CComPtrNtv_003CIAddress_003E_002ERelease(&cComPtrNtv_003CIAddress_003E);
		return num;
	}

	private unsafe int CreateCreditCard(CreditCard creditCard, ICreditCard** ppCreditCard)
	{
		//IL_002e: Expected I, but got I8
		//IL_00f4: Expected I, but got I8
		//IL_00f4: Expected I, but got I8
		//IL_00f4: Expected I, but got I8
		//IL_0164: Expected I, but got I8
		//IL_0164: Expected I, but got I8
		//IL_016f: Expected I, but got I8
		//IL_0178: Expected I8, but got I
		int num = CreateComObject();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CICreditCard_003E cComPtrNtv_003CICreditCard_003E);
		*(long*)(&cComPtrNtv_003CICreditCard_003E) = 0L;
		try
		{
			if (num >= 0)
			{
				IAccountManagement* p = m_spAccountManagement.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ICreditCard**, int>)(*(ulong*)(*(long*)p + 40)))((nint)p, (ICreditCard**)(&cComPtrNtv_003CICreditCard_003E));
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIAddress_003E cComPtrNtv_003CIAddress_003E);
			*(long*)(&cComPtrNtv_003CIAddress_003E) = 0L;
			try
			{
				if (num >= 0)
				{
					num = CreateAddress(creditCard.Address, (IAddress**)(&cComPtrNtv_003CIAddress_003E));
					if (num >= 0)
					{
						_SYSTEMTIME sYSTEMTIME = global::_003CModule_003E.DateTimeToSystemTime(creditCard.ExpirationDate);
						fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.AccountHolderName)))
						{
							try
							{
								fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.AccountNumber)))
								{
									try
									{
										fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.CCVNumber)))
										{
											try
											{
												fixed (ushort* ptr4 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.Locale)))
												{
													try
													{
														fixed (ushort* ptr5 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.PhoneNumber)))
														{
															try
															{
																fixed (ushort* ptr6 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.Email)))
																{
																	try
																	{
																		fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.ContactFirstName)))
																		{
																			try
																			{
																				fixed (ushort* ptr8 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(creditCard.ContactLastName)))
																				{
																					try
																					{
																						long num2 = *(long*)(*(ulong*)(&cComPtrNtv_003CICreditCard_003E)) + 24;
																						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IAddress*, ECreditCardType, ushort*, ushort*, ushort*, ushort*, ushort*, ushort*, ushort*, ushort*, _SYSTEMTIME*, int>)(*(ulong*)num2))((nint)(*(long*)(&cComPtrNtv_003CICreditCard_003E)), (IAddress*)(*(ulong*)(&cComPtrNtv_003CIAddress_003E)), (ECreditCardType)creditCard.CreditCardType, ptr, ptr2, ptr3, ptr4, ptr5, ptr6, ptr7, ptr8, &sYSTEMTIME);
																					}
																					catch
																					{
																						//try-fault
																						ptr8 = null;
																						throw;
																					}
																				}
																			}
																			catch
																			{
																				//try-fault
																				ptr7 = null;
																				throw;
																			}
																		}
																	}
																	catch
																	{
																		//try-fault
																		ptr6 = null;
																		throw;
																	}
																}
															}
															catch
															{
																//try-fault
																ptr5 = null;
																throw;
															}
														}
													}
													catch
													{
														//try-fault
														ptr4 = null;
														throw;
													}
												}
											}
											catch
											{
												//try-fault
												ptr3 = null;
												throw;
											}
										}
									}
									catch
									{
										//try-fault
										ptr2 = null;
										throw;
									}
								}
							}
							catch
							{
								//try-fault
								ptr = null;
								throw;
							}
						}
						if (num >= 0)
						{
							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CICreditCard_003E)) + 40)))((nint)(*(long*)(&cComPtrNtv_003CICreditCard_003E)), creditCard.ParentCreditCard ? ((byte)1) : ((byte)0));
							if (ppCreditCard != null)
							{
								ICreditCard* ptr9 = (ICreditCard*)(*(ulong*)(&cComPtrNtv_003CICreditCard_003E));
								*(long*)(&cComPtrNtv_003CICreditCard_003E) = 0L;
								*(long*)ppCreditCard = (nint)ptr9;
							}
						}
					}
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIAddress_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIAddress_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIAddress_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CIAddress_003E_002ERelease(&cComPtrNtv_003CIAddress_003E);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CICreditCard_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CICreditCard_003E_002E_007Bdtor_007D), &cComPtrNtv_003CICreditCard_003E);
			throw;
		}
		global::_003CModule_003E.CComPtrNtv_003CICreditCard_003E_002ERelease(&cComPtrNtv_003CICreditCard_003E);
		return num;
	}

	private unsafe int CreateAccountSettings(AccountSettings accountSettings, INewsletterSettings** ppNewsletterSettings, IPrivacySettings** ppPrivacySettings)
	{
		//IL_002b: Expected I, but got I8
		//IL_0062: Expected I, but got I8
		//IL_0062: Expected I, but got I8
		//IL_009a: Expected I, but got I8
		//IL_010f: Expected I, but got I8
		//IL_0118: Expected I8, but got I
		//IL_00ec: Expected I, but got I8
		//IL_00ec: Expected I, but got I8
		//IL_0127: Expected I, but got I8
		//IL_0130: Expected I8, but got I
		int num = CreateComObject();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CINewsletterSettings_003E cComPtrNtv_003CINewsletterSettings_003E);
		*(long*)(&cComPtrNtv_003CINewsletterSettings_003E) = 0L;
		try
		{
			if (num >= 0)
			{
				IAccountManagement* p = m_spAccountManagement.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, INewsletterSettings**, int>)(*(ulong*)(*(long*)p + 48)))((nint)p, (INewsletterSettings**)(&cComPtrNtv_003CINewsletterSettings_003E));
				if (num >= 0)
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out NewsletterOptions newsletterOptions);
					*(EmailFormat*)(&newsletterOptions) = accountSettings.EmailFormat;
					System.Runtime.CompilerServices.Unsafe.As<NewsletterOptions, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref newsletterOptions, 4)) = (accountSettings.AllowZuneEmails ? 1 : 0);
					System.Runtime.CompilerServices.Unsafe.As<NewsletterOptions, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref newsletterOptions, 8)) = (accountSettings.AllowPartnerEmails ? 1 : 0);
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, NewsletterOptions, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CINewsletterSettings_003E)) + 24)))((nint)(*(long*)(&cComPtrNtv_003CINewsletterSettings_003E)), newsletterOptions);
				}
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIPrivacySettings_003E cComPtrNtv_003CIPrivacySettings_003E);
			*(long*)(&cComPtrNtv_003CIPrivacySettings_003E) = 0L;
			try
			{
				if (num >= 0)
				{
					if (accountSettings.PrivacySettings.Count > 0)
					{
						IAccountManagement* p2 = m_spAccountManagement.p;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPrivacySettings**, int>)(*(ulong*)(*(long*)p2 + 56)))((nint)p2, (IPrivacySettings**)(&cComPtrNtv_003CIPrivacySettings_003E));
					}
					if (num >= 0)
					{
						if (*(long*)(&cComPtrNtv_003CIPrivacySettings_003E) != 0L)
						{
							foreach (KeyValuePair<PrivacySettingId, PrivacySettingValue> privacySetting in accountSettings.PrivacySettings)
							{
								EPrivacySettingId key = (EPrivacySettingId)privacySetting.Key;
								EPrivacySettingValue value = (EPrivacySettingValue)privacySetting.Value;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EPrivacySettingId, EPrivacySettingValue, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIPrivacySettings_003E)) + 24)))((nint)(*(long*)(&cComPtrNtv_003CIPrivacySettings_003E)), key, value);
								if (num >= 0)
								{
									continue;
								}
								break;
							}
						}
						if (num >= 0)
						{
							if (ppNewsletterSettings != null)
							{
								INewsletterSettings* ptr = (INewsletterSettings*)(*(ulong*)(&cComPtrNtv_003CINewsletterSettings_003E));
								*(long*)(&cComPtrNtv_003CINewsletterSettings_003E) = 0L;
								*(long*)ppNewsletterSettings = (nint)ptr;
							}
							if (ppPrivacySettings != null)
							{
								if (*(long*)(&cComPtrNtv_003CIPrivacySettings_003E) != 0L)
								{
									IPrivacySettings* ptr2 = (IPrivacySettings*)(*(ulong*)(&cComPtrNtv_003CIPrivacySettings_003E));
									*(long*)(&cComPtrNtv_003CIPrivacySettings_003E) = 0L;
									*(long*)ppPrivacySettings = (nint)ptr2;
								}
								else
								{
									*(long*)ppPrivacySettings = 0L;
								}
							}
						}
					}
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPrivacySettings_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIPrivacySettings_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPrivacySettings_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CIPrivacySettings_003E_002ERelease(&cComPtrNtv_003CIPrivacySettings_003E);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CINewsletterSettings_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CINewsletterSettings_003E_002E_007Bdtor_007D), &cComPtrNtv_003CINewsletterSettings_003E);
			throw;
		}
		global::_003CModule_003E.CComPtrNtv_003CINewsletterSettings_003E_002ERelease(&cComPtrNtv_003CINewsletterSettings_003E);
		return num;
	}

	private unsafe int CreateAccountUser(AccountUser accountUser, IAccountUser** ppAccountUser)
	{
		//IL_002e: Expected I, but got I8
		//IL_017c: Expected I, but got I8
		//IL_017c: Expected I, but got I8
		//IL_017c: Expected I, but got I8
		//IL_017c: Expected I, but got I8
		//IL_017c: Expected I, but got I8
		//IL_017c: Expected I, but got I8
		//IL_017c: Expected I, but got I8
		//IL_01d4: Expected I, but got I8
		//IL_01dd: Expected I8, but got I
		//IL_0147->IL0149: Incompatible stack types: I8 vs Ref
		int num = CreateComObject();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIAccountUser_003E cComPtrNtv_003CIAccountUser_003E);
		*(long*)(&cComPtrNtv_003CIAccountUser_003E) = 0L;
		try
		{
			if (num >= 0)
			{
				IAccountManagement* p = m_spAccountManagement.p;
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IAccountUser**, int>)(*(ulong*)(*(long*)p + 64)))((nint)p, (IAccountUser**)(&cComPtrNtv_003CIAccountUser_003E));
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIAddress_003E cComPtrNtv_003CIAddress_003E);
			*(long*)(&cComPtrNtv_003CIAddress_003E) = 0L;
			try
			{
				if (num >= 0 && accountUser.Address != null)
				{
					num = CreateAddress(accountUser.Address, (IAddress**)(&cComPtrNtv_003CIAddress_003E));
				}
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CINewsletterSettings_003E cComPtrNtv_003CINewsletterSettings_003E);
				*(long*)(&cComPtrNtv_003CINewsletterSettings_003E) = 0L;
				try
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIPrivacySettings_003E cComPtrNtv_003CIPrivacySettings_003E);
					*(long*)(&cComPtrNtv_003CIPrivacySettings_003E) = 0L;
					try
					{
						if (num >= 0 && accountUser.AccountSettings != null)
						{
							num = CreateAccountSettings(accountUser.AccountSettings, (INewsletterSettings**)(&cComPtrNtv_003CINewsletterSettings_003E), (IPrivacySettings**)(&cComPtrNtv_003CIPrivacySettings_003E));
						}
						System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CICreditCard_003E cComPtrNtv_003CICreditCard_003E);
						*(long*)(&cComPtrNtv_003CICreditCard_003E) = 0L;
						try
						{
							if (num >= 0 && accountUser.ParentCreditCard != null)
							{
								num = CreateCreditCard(accountUser.ParentCreditCard, (ICreditCard**)(&cComPtrNtv_003CICreditCard_003E));
							}
							System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E);
							*(long*)(&cComPtrNtv_003CIPassportIdentity_003E) = 0L;
							try
							{
								if (num >= 0)
								{
									if (accountUser.ParentPassportIdentity != null)
									{
										num = accountUser.ParentPassportIdentity.GetComPointer((IPassportIdentity**)(&cComPtrNtv_003CIPassportIdentity_003E));
									}
									if (num >= 0)
									{
										bool flag = accountUser.Birthday != DateTime.MinValue;
										System.Runtime.CompilerServices.Unsafe.SkipInit(out _SYSTEMTIME sYSTEMTIME);
										if (flag)
										{
											sYSTEMTIME = global::_003CModule_003E.DateTimeToSystemTime(accountUser.Birthday);
										}
										fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(accountUser.ZuneTag)))
										{
											try
											{
												fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(accountUser.Locale)))
												{
													try
													{
														fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(accountUser.FirstName)))
														{
															try
															{
																fixed (ushort* ptr4 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(accountUser.LastName)))
																{
																	try
																	{
																		fixed (ushort* ptr5 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(accountUser.Email)))
																		{
																			try
																			{
																				fixed (ushort* ptr6 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(accountUser.PhoneNumber)))
																				{
																					try
																					{
																						fixed (ushort* ptr7 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(accountUser.MobilePhoneNumber)))
																						{
																							try
																							{
																								_SYSTEMTIME* ptr8 = (_SYSTEMTIME*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref flag ? ref *(_003F*)(&sYSTEMTIME) : ref *(_003F*)null);
																								long num2 = *(long*)(*(ulong*)(&cComPtrNtv_003CIAccountUser_003E)) + 24;
																								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, _SYSTEMTIME*, ushort*, ushort*, ushort*, ushort*, ushort*, IAddress*, INewsletterSettings*, IPrivacySettings*, IPassportIdentity*, ICreditCard*, int>)(*(ulong*)num2))((nint)(*(long*)(&cComPtrNtv_003CIAccountUser_003E)), ptr, ptr2, ptr8, ptr3, ptr4, ptr5, ptr6, ptr7, (IAddress*)(*(ulong*)(&cComPtrNtv_003CIAddress_003E)), (INewsletterSettings*)(*(ulong*)(&cComPtrNtv_003CINewsletterSettings_003E)), (IPrivacySettings*)(*(ulong*)(&cComPtrNtv_003CIPrivacySettings_003E)), (IPassportIdentity*)(*(ulong*)(&cComPtrNtv_003CIPassportIdentity_003E)), (ICreditCard*)(*(ulong*)(&cComPtrNtv_003CICreditCard_003E)));
																							}
																							catch
																							{
																								//try-fault
																								ptr7 = null;
																								throw;
																							}
																						}
																					}
																					catch
																					{
																						//try-fault
																						ptr6 = null;
																						throw;
																					}
																				}
																			}
																			catch
																			{
																				//try-fault
																				ptr5 = null;
																				throw;
																			}
																		}
																	}
																	catch
																	{
																		//try-fault
																		ptr4 = null;
																		throw;
																	}
																}
															}
															catch
															{
																//try-fault
																ptr3 = null;
																throw;
															}
														}
													}
													catch
													{
														//try-fault
														ptr2 = null;
														throw;
													}
												}
											}
											catch
											{
												//try-fault
												ptr = null;
												throw;
											}
										}
										if (num >= 0 && ppAccountUser != null)
										{
											IAccountUser* ptr9 = (IAccountUser*)(*(ulong*)(&cComPtrNtv_003CIAccountUser_003E));
											*(long*)(&cComPtrNtv_003CIAccountUser_003E) = 0L;
											*(long*)ppAccountUser = (nint)ptr9;
										}
									}
								}
							}
							catch
							{
								//try-fault
								global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E);
								throw;
							}
							global::_003CModule_003E.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E);
						}
						catch
						{
							//try-fault
							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CICreditCard_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CICreditCard_003E_002E_007Bdtor_007D), &cComPtrNtv_003CICreditCard_003E);
							throw;
						}
						global::_003CModule_003E.CComPtrNtv_003CICreditCard_003E_002ERelease(&cComPtrNtv_003CICreditCard_003E);
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPrivacySettings_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIPrivacySettings_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPrivacySettings_003E);
						throw;
					}
					global::_003CModule_003E.CComPtrNtv_003CIPrivacySettings_003E_002ERelease(&cComPtrNtv_003CIPrivacySettings_003E);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CINewsletterSettings_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CINewsletterSettings_003E_002E_007Bdtor_007D), &cComPtrNtv_003CINewsletterSettings_003E);
					throw;
				}
				global::_003CModule_003E.CComPtrNtv_003CINewsletterSettings_003E_002ERelease(&cComPtrNtv_003CINewsletterSettings_003E);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIAddress_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIAddress_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIAddress_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CIAddress_003E_002ERelease(&cComPtrNtv_003CIAddress_003E);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIAccountUser_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIAccountUser_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIAccountUser_003E);
			throw;
		}
		global::_003CModule_003E.CComPtrNtv_003CIAccountUser_003E_002ERelease(&cComPtrNtv_003CIAccountUser_003E);
		return num;
	}

	private unsafe int CreateComObject()
	{
		//IL_0045: Expected I, but got I8
		//IL_0045: Expected I, but got I8
		//IL_0075: Expected I, but got I8
		//IL_0075: Expected I, but got I8
		//IL_0075: Expected I, but got I8
		//IL_008a: Expected I, but got I8
		int num = 0;
		if ((long)(nint)m_spAccountManagement.p == 0)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIService_003E cComPtrNtv_003CIService_003E);
			*(long*)(&cComPtrNtv_003CIService_003E) = 0L;
			try
			{
				num = global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_bb2d1edd_1bd5_4be1_8d38_36d4f0849911, (void**)(&cComPtrNtv_003CIService_003E));
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIUnknown_003E cComPtrNtv_003CIUnknown_003E);
				*(long*)(&cComPtrNtv_003CIUnknown_003E) = 0L;
				try
				{
					if (num >= 0)
					{
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IUnknown**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIService_003E)) + 104)))((nint)(*(long*)(&cComPtrNtv_003CIService_003E)), (IUnknown**)(&cComPtrNtv_003CIUnknown_003E));
					}
					System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIAccountManagement_003E cComPtrNtv_003CIAccountManagement_003E);
					*(long*)(&cComPtrNtv_003CIAccountManagement_003E) = 0L;
					try
					{
						if (num >= 0)
						{
							num = global::_003CModule_003E.CComPtrNtv_003CIUnknown_003E_002EQueryInterface_003Cstruct_0020IAccountManagement_003E(&cComPtrNtv_003CIUnknown_003E, (IAccountManagement**)(&cComPtrNtv_003CIAccountManagement_003E));
							if (num >= 0)
							{
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IService*, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIAccountManagement_003E)) + 24)))((nint)(*(long*)(&cComPtrNtv_003CIAccountManagement_003E)), (IService*)(*(ulong*)(&cComPtrNtv_003CIService_003E)));
								if (num >= 0)
								{
									m_spAccountManagement.op_Assign((IAccountManagement*)(*(ulong*)(&cComPtrNtv_003CIAccountManagement_003E)));
								}
							}
						}
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIAccountManagement_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIAccountManagement_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIAccountManagement_003E);
						throw;
					}
					global::_003CModule_003E.CComPtrNtv_003CIAccountManagement_003E_002ERelease(&cComPtrNtv_003CIAccountManagement_003E);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIUnknown_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIUnknown_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIUnknown_003E);
					throw;
				}
				global::_003CModule_003E.CComPtrNtv_003CIUnknown_003E_002ERelease(&cComPtrNtv_003CIUnknown_003E);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIService_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIService_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIService_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CIService_003E_002ERelease(&cComPtrNtv_003CIService_003E);
		}
		return num;
	}

	public void _007EAccountManagement()
	{
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
	{
		if (P_0)
		{
			try
			{
				return;
			}
			finally
			{
				((IDisposable)m_spAccountManagement).Dispose();
			}
		}
		Finalize();
	}

	public virtual sealed void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
