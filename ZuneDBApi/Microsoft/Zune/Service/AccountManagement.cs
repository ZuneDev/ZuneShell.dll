using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ZuneUI;

namespace Microsoft.Zune.Service
{
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
			//IL_0112: Expected I, but got I8
			//IL_0112: Expected I, but got I8
			//IL_0112: Expected I, but got I8
			//IL_0112: Expected I, but got I8
			//IL_0112: Expected I, but got I8
			//IL_0112: Expected I, but got I8
			//IL_0112: Expected I, but got I8
			//IL_0153: Expected I, but got I8
			int num = CreateComObject();
			CComPtrNtv_003CINewsletterSettings_003E cComPtrNtv_003CINewsletterSettings_003E;
			Module.CComPtrNtv_003CINewsletterSettings_003E_002E_007Bctor_007D(&cComPtrNtv_003CINewsletterSettings_003E);
			HRESULT result;
			try
			{
				CComPtrNtv_003CIPrivacySettings_003E cComPtrNtv_003CIPrivacySettings_003E;
				Module.CComPtrNtv_003CIPrivacySettings_003E_002E_007Bctor_007D(&cComPtrNtv_003CIPrivacySettings_003E);
				try
				{
					if (num >= 0 && accountSettings != null)
					{
						num = CreateAccountSettings(accountSettings, Module.CComPtrNtv_003CINewsletterSettings_003E_002E_0026(&cComPtrNtv_003CINewsletterSettings_003E), Module.CComPtrNtv_003CIPrivacySettings_003E_002E_0026(&cComPtrNtv_003CIPrivacySettings_003E));
					}
					CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E;
					Module.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bctor_007D(&cComPtrNtv_003CIPassportIdentity_003E);
					try
					{
						if (num >= 0 && passportIdentity != null)
						{
							num = passportIdentity.GetComPointer((IPassportIdentity**)(&cComPtrNtv_003CIPassportIdentity_003E));
						}
						CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E2;
						Module.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bctor_007D(&cComPtrNtv_003CIPassportIdentity_003E2);
						try
						{
							if (num >= 0 && parentPassportIdentity != null)
							{
								num = parentPassportIdentity.GetComPointer((IPassportIdentity**)(&cComPtrNtv_003CIPassportIdentity_003E2));
							}
							CComPtrNtv_003CICreditCard_003E cComPtrNtv_003CICreditCard_003E;
							*(long*)(&cComPtrNtv_003CICreditCard_003E) = 0L;
							try
							{
								if (num >= 0 && parentCreditCard != null)
								{
									num = CreateCreditCard(parentCreditCard, (ICreditCard**)(&cComPtrNtv_003CICreditCard_003E));
								}
								CComPtrNtv_003CIAddress_003E cComPtrNtv_003CIAddress_003E;
								*(long*)(&cComPtrNtv_003CIAddress_003E) = 0L;
								try
								{
									if (num >= 0 && address != null)
									{
										num = CreateAddress(address, (IAddress**)(&cComPtrNtv_003CIAddress_003E));
									}
									CComPtrNtv_003CIServiceError_003E cComPtrNtv_003CIServiceError_003E;
									*(long*)(&cComPtrNtv_003CIServiceError_003E) = 0L;
									try
									{
										if (num >= 0)
										{
											_SYSTEMTIME sYSTEMTIME = Module.DateTimeToSystemTime(birthday);
											fixed (char* firstNamePtr = firstName.ToCharArray())
											{
												ushort* ptr3 = (ushort*)firstNamePtr;
												try
												{
													fixed (char* lastNamePtr = lastName.ToCharArray())
													{
														ushort* ptr4 = (ushort*)lastNamePtr;
														try
														{
															fixed (char* zuneTagPtr = zuneTag.ToCharArray())
															{
																ushort* ptr = (ushort*)zuneTagPtr;
																try
																{
																	fixed (char* emailPtr = email.ToCharArray())
																	{
																		ushort* ptr5 = (ushort*)emailPtr;
																		try
																		{
																			fixed (char* localePtr = locale.ToCharArray())
																			{
																				ushort* ptr2 = (ushort*)localePtr;
																				try
																				{
																					IAccountManagement* p = m_spAccountManagement.p;
																					long num2 = *(long*)p + 72;
																					long num3 = *(long*)(&cComPtrNtv_003CIPassportIdentity_003E);
																					_003F val = ptr;
																					_003F val2 = ptr2;
																					_003F val3 = ptr3;
																					_003F val4 = ptr4;
																					_003F val5 = ptr5;
																					long num4 = *(long*)(&cComPtrNtv_003CIAddress_003E);
																					long num5 = *(long*)(&cComPtrNtv_003CINewsletterSettings_003E);
																					long num6 = *(long*)(&cComPtrNtv_003CIPrivacySettings_003E);
																					long num7 = *(long*)(&cComPtrNtv_003CIPassportIdentity_003E2);
																					long num8 = *(long*)(&cComPtrNtv_003CICreditCard_003E);
																					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPassportIdentity*, ushort*, ushort*, _SYSTEMTIME*, ushort*, ushort*, ushort*, IAddress*, INewsletterSettings*, IPrivacySettings*, IPassportIdentity*, ICreditCard*, IServiceError**, int>)(*(ulong*)num2))((nint)p, (IPassportIdentity*)num3, (ushort*)(nint)val, (ushort*)(nint)val2, &sYSTEMTIME, (ushort*)(nint)val3, (ushort*)(nint)val4, (ushort*)(nint)val5, (IAddress*)num4, (INewsletterSettings*)num5, (IPrivacySettings*)num6, (IPassportIdentity*)num7, (ICreditCard*)num8, (IServiceError**)(&cComPtrNtv_003CIServiceError_003E));
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
																			ptr5 = null;
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
										if (*(long*)(&cComPtrNtv_003CIServiceError_003E) != 0L)
										{
											serviceError = new ServiceError((IServiceError*)(*(ulong*)(&cComPtrNtv_003CIServiceError_003E)));
										}
										result = new HRESULT(num);
									}
									catch
									{
										//try-fault
										Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIServiceError_003E*, void>)(&Module.CComPtrNtv_003CIServiceError_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIServiceError_003E);
										throw;
									}
									Module.CComPtrNtv_003CIServiceError_003E_002ERelease(&cComPtrNtv_003CIServiceError_003E);
								}
								catch
								{
									//try-fault
									Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIAddress_003E*, void>)(&Module.CComPtrNtv_003CIAddress_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIAddress_003E);
									throw;
								}
								Module.CComPtrNtv_003CIAddress_003E_002ERelease(&cComPtrNtv_003CIAddress_003E);
							}
							catch
							{
								//try-fault
								Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CICreditCard_003E*, void>)(&Module.CComPtrNtv_003CICreditCard_003E_002E_007Bdtor_007D), &cComPtrNtv_003CICreditCard_003E);
								throw;
							}
							Module.CComPtrNtv_003CICreditCard_003E_002ERelease(&cComPtrNtv_003CICreditCard_003E);
						}
						catch
						{
							//try-fault
							Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&Module.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E2);
							throw;
						}
						Module.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E2);
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&Module.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E);
						throw;
					}
					Module.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E);
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPrivacySettings_003E*, void>)(&Module.CComPtrNtv_003CIPrivacySettings_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPrivacySettings_003E);
					throw;
				}
				Module.CComPtrNtv_003CIPrivacySettings_003E_002ERelease(&cComPtrNtv_003CIPrivacySettings_003E);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CINewsletterSettings_003E*, void>)(&Module.CComPtrNtv_003CINewsletterSettings_003E_002E_007Bdtor_007D), &cComPtrNtv_003CINewsletterSettings_003E);
				throw;
			}
			Module.CComPtrNtv_003CINewsletterSettings_003E_002ERelease(&cComPtrNtv_003CINewsletterSettings_003E);
			return result;
		}

		public unsafe HRESULT ReserveZuneTag(string zuneTag, string countryCode, out IList suggestedNames, out ServiceError serviceError)
		{
			//IL_004e: Expected I, but got I8
			//IL_007c: Expected I, but got I8
			//IL_007c: Expected I, but got I8
			//IL_008f: Expected I, but got I8
			//IL_00a3: Expected I, but got I8
			//IL_00a3: Expected I, but got I8
			//IL_00d9: Expected I, but got I8
			int num = CreateComObject();
			suggestedNames = null;
			CComPtrNtv_003CIAvailableZuneTagInformation_003E cComPtrNtv_003CIAvailableZuneTagInformation_003E;
			*(long*)(&cComPtrNtv_003CIAvailableZuneTagInformation_003E) = 0L;
			HRESULT result;
			try
			{
				CComPtrNtv_003CIServiceError_003E cComPtrNtv_003CIServiceError_003E;
				*(long*)(&cComPtrNtv_003CIServiceError_003E) = 0L;
				try
				{
					if (num >= 0)
					{
						fixed (char* zuneTagPtr = zuneTag.ToCharArray())
						{
							ushort* ptr = (ushort*)zuneTagPtr;
							try
							{
								fixed (char* countryCodePtr = countryCode.ToCharArray())
								{
									ushort* ptr2 = (ushort*)countryCodePtr;
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
						long num3 = *(long*)(&cComPtrNtv_003CIAvailableZuneTagInformation_003E);
						int num4 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIAvailableZuneTagInformation_003E)) + 48)))((nint)num3);
						suggestedNames = new ArrayList(num4);
						int num5 = 0;
						if (0 < num4)
						{
							do
							{
								ushort* ptr3 = null;
								long num6 = *(long*)(&cComPtrNtv_003CIAvailableZuneTagInformation_003E);
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, ushort**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIAvailableZuneTagInformation_003E)) + 56)))((nint)num6, num5, &ptr3);
								if (num >= 0)
								{
									suggestedNames.Add(new string((char*)ptr3));
								}
								Module.SysFreeString(ptr3);
								if (num < 0)
								{
									break;
								}
								num5++;
							}
							while (num5 < num4);
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
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIServiceError_003E*, void>)(&Module.CComPtrNtv_003CIServiceError_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIServiceError_003E);
					throw;
				}
				Module.CComPtrNtv_003CIServiceError_003E_002ERelease(&cComPtrNtv_003CIServiceError_003E);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIAvailableZuneTagInformation_003E*, void>)(&Module.CComPtrNtv_003CIAvailableZuneTagInformation_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIAvailableZuneTagInformation_003E);
				throw;
			}
			Module.CComPtrNtv_003CIAvailableZuneTagInformation_003E_002ERelease(&cComPtrNtv_003CIAvailableZuneTagInformation_003E);
			return result;
		}

		public unsafe HRESULT ValidateCreditCard(PassportIdentity parentPassportIdentity, CreditCard creditCard, out ServiceError serviceError)
		{
			//IL_005c: Expected I, but got I8
			//IL_005c: Expected I, but got I8
			//IL_005c: Expected I, but got I8
			//IL_006b: Expected I, but got I8
			int num = CreateComObject();
			CComPtrNtv_003CICreditCard_003E cComPtrNtv_003CICreditCard_003E;
			*(long*)(&cComPtrNtv_003CICreditCard_003E) = 0L;
			HRESULT result;
			try
			{
				if (num >= 0 && creditCard != null)
				{
					num = CreateCreditCard(creditCard, (ICreditCard**)(&cComPtrNtv_003CICreditCard_003E));
				}
				CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E;
				*(long*)(&cComPtrNtv_003CIPassportIdentity_003E) = 0L;
				try
				{
					if (num >= 0 && parentPassportIdentity != null)
					{
						num = parentPassportIdentity.GetComPointer((IPassportIdentity**)(&cComPtrNtv_003CIPassportIdentity_003E));
					}
					CComPtrNtv_003CIServiceError_003E cComPtrNtv_003CIServiceError_003E;
					*(long*)(&cComPtrNtv_003CIServiceError_003E) = 0L;
					try
					{
						if (num >= 0)
						{
							IAccountManagement* p = m_spAccountManagement.p;
							long num2 = *(long*)(&cComPtrNtv_003CIPassportIdentity_003E);
							long num3 = *(long*)(&cComPtrNtv_003CICreditCard_003E);
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPassportIdentity*, ICreditCard*, IServiceError**, int>)(*(ulong*)(*(long*)p + 88)))((nint)p, (IPassportIdentity*)num2, (ICreditCard*)num3, (IServiceError**)(&cComPtrNtv_003CIServiceError_003E));
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
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIServiceError_003E*, void>)(&Module.CComPtrNtv_003CIServiceError_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIServiceError_003E);
						throw;
					}
					Module.CComPtrNtv_003CIServiceError_003E_002ERelease(&cComPtrNtv_003CIServiceError_003E);
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&Module.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E);
					throw;
				}
				Module.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CICreditCard_003E*, void>)(&Module.CComPtrNtv_003CICreditCard_003E_002E_007Bdtor_007D), &cComPtrNtv_003CICreditCard_003E);
				throw;
			}
			Module.CComPtrNtv_003CICreditCard_003E_002ERelease(&cComPtrNtv_003CICreditCard_003E);
			return result;
		}

		public unsafe HRESULT GetAccount(PassportIdentity passportIdentity, GetAccountCompleteCallback onSuccess, AccountManagementErrorCallback onError)
		{
			//IL_000a: Expected I, but got I8
			//IL_0028: Expected I, but got I8
			//IL_007a: Expected I, but got I8
			//IL_007a: Expected I, but got I8
			//IL_008b: Expected I, but got I8
			int num = CreateComObject();
			GetAccountCallbackWrapper* ptr = null;
			if (num >= 0)
			{
				GetAccountCallbackWrapper* ptr2 = (GetAccountCallbackWrapper*)Module.@new(32uL);
				GetAccountCallbackWrapper* ptr3;
				try
				{
					ptr3 = ((ptr2 == null) ? null : Module.Microsoft_002EZune_002EService_002EGetAccountCallbackWrapper_002E_007Bctor_007D(ptr2, onSuccess, onError));
				}
				catch
				{
					//try-fault
					Module.delete(ptr2);
					throw;
				}
				ptr = ptr3;
				num = (((long)(nint)ptr3 == 0) ? (-2147024882) : num);
			}
			CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E;
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
						long num2 = *(long*)(&cComPtrNtv_003CIPassportIdentity_003E);
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPassportIdentity*, IGetAccountCallback*, int>)(*(ulong*)(*(long*)p + 96)))((nint)p, (IPassportIdentity*)num2, (IGetAccountCallback*)ptr);
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
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&Module.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E);
				throw;
			}
			Module.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E);
			return result;
		}

		public unsafe HRESULT GetAccount(PassportIdentity passportIdentity, out AccountUser accountUser, out ServiceError serviceError)
		{
			//IL_004a: Expected I, but got I8
			//IL_004a: Expected I, but got I8
			//IL_0059: Expected I, but got I8
			//IL_0068: Expected I, but got I8
			int num = CreateComObject();
			CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E;
			*(long*)(&cComPtrNtv_003CIPassportIdentity_003E) = 0L;
			HRESULT result;
			try
			{
				if (num >= 0 && passportIdentity != null)
				{
					num = passportIdentity.GetComPointer((IPassportIdentity**)(&cComPtrNtv_003CIPassportIdentity_003E));
				}
				CComPtrNtv_003CIAccountUser_003E cComPtrNtv_003CIAccountUser_003E;
				*(long*)(&cComPtrNtv_003CIAccountUser_003E) = 0L;
				try
				{
					CComPtrNtv_003CIServiceError_003E cComPtrNtv_003CIServiceError_003E;
					*(long*)(&cComPtrNtv_003CIServiceError_003E) = 0L;
					try
					{
						if (num >= 0)
						{
							IAccountManagement* p = m_spAccountManagement.p;
							long num2 = *(long*)(&cComPtrNtv_003CIPassportIdentity_003E);
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPassportIdentity*, IAccountUser**, IServiceError**, int>)(*(ulong*)(*(long*)p + 104)))((nint)p, (IPassportIdentity*)num2, (IAccountUser**)(&cComPtrNtv_003CIAccountUser_003E), (IServiceError**)(&cComPtrNtv_003CIServiceError_003E));
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
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIServiceError_003E*, void>)(&Module.CComPtrNtv_003CIServiceError_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIServiceError_003E);
						throw;
					}
					Module.CComPtrNtv_003CIServiceError_003E_002ERelease(&cComPtrNtv_003CIServiceError_003E);
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIAccountUser_003E*, void>)(&Module.CComPtrNtv_003CIAccountUser_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIAccountUser_003E);
					throw;
				}
				Module.CComPtrNtv_003CIAccountUser_003E_002ERelease(&cComPtrNtv_003CIAccountUser_003E);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&Module.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E);
				throw;
			}
			Module.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E);
			return result;
		}

		public unsafe HRESULT SetAccount(PassportIdentity passportIdentity, AccountUser accountUser, out ServiceError serviceError)
		{
			//IL_005c: Expected I, but got I8
			//IL_005c: Expected I, but got I8
			//IL_005c: Expected I, but got I8
			//IL_006b: Expected I, but got I8
			int num = CreateComObject();
			CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E;
			*(long*)(&cComPtrNtv_003CIPassportIdentity_003E) = 0L;
			HRESULT result;
			try
			{
				if (num >= 0 && passportIdentity != null)
				{
					num = passportIdentity.GetComPointer((IPassportIdentity**)(&cComPtrNtv_003CIPassportIdentity_003E));
				}
				CComPtrNtv_003CIAccountUser_003E cComPtrNtv_003CIAccountUser_003E;
				*(long*)(&cComPtrNtv_003CIAccountUser_003E) = 0L;
				try
				{
					if (num >= 0 && accountUser != null)
					{
						num = CreateAccountUser(accountUser, (IAccountUser**)(&cComPtrNtv_003CIAccountUser_003E));
					}
					CComPtrNtv_003CIServiceError_003E cComPtrNtv_003CIServiceError_003E;
					*(long*)(&cComPtrNtv_003CIServiceError_003E) = 0L;
					try
					{
						if (num >= 0)
						{
							IAccountManagement* p = m_spAccountManagement.p;
							long num2 = *(long*)(&cComPtrNtv_003CIPassportIdentity_003E);
							long num3 = *(long*)(&cComPtrNtv_003CIAccountUser_003E);
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPassportIdentity*, IAccountUser*, IServiceError**, int>)(*(ulong*)(*(long*)p + 112)))((nint)p, (IPassportIdentity*)num2, (IAccountUser*)num3, (IServiceError**)(&cComPtrNtv_003CIServiceError_003E));
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
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIServiceError_003E*, void>)(&Module.CComPtrNtv_003CIServiceError_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIServiceError_003E);
						throw;
					}
					Module.CComPtrNtv_003CIServiceError_003E_002ERelease(&cComPtrNtv_003CIServiceError_003E);
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIAccountUser_003E*, void>)(&Module.CComPtrNtv_003CIAccountUser_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIAccountUser_003E);
					throw;
				}
				Module.CComPtrNtv_003CIAccountUser_003E_002ERelease(&cComPtrNtv_003CIAccountUser_003E);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&Module.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E);
				throw;
			}
			Module.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E);
			return result;
		}

		public unsafe HRESULT SetNewsLetterSettings(AccountSettings accountSettings, out ServiceError serviceError)
		{
			//IL_001e: Expected I, but got I8
			//IL_0046: Expected I, but got I8
			//IL_0046: Expected I, but got I8
			//IL_0055: Expected I, but got I8
			int num = CreateComObject();
			CComPtrNtv_003CINewsletterSettings_003E cComPtrNtv_003CINewsletterSettings_003E;
			*(long*)(&cComPtrNtv_003CINewsletterSettings_003E) = 0L;
			HRESULT result;
			try
			{
				if (num >= 0 && accountSettings != null)
				{
					num = CreateAccountSettings(accountSettings, (INewsletterSettings**)(&cComPtrNtv_003CINewsletterSettings_003E), null);
				}
				CComPtrNtv_003CIServiceError_003E cComPtrNtv_003CIServiceError_003E;
				*(long*)(&cComPtrNtv_003CIServiceError_003E) = 0L;
				try
				{
					if (num >= 0)
					{
						IAccountManagement* p = m_spAccountManagement.p;
						long num2 = *(long*)(&cComPtrNtv_003CINewsletterSettings_003E);
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, INewsletterSettings*, IServiceError**, int>)(*(ulong*)(*(long*)p + 120)))((nint)p, (INewsletterSettings*)num2, (IServiceError**)(&cComPtrNtv_003CIServiceError_003E));
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
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIServiceError_003E*, void>)(&Module.CComPtrNtv_003CIServiceError_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIServiceError_003E);
					throw;
				}
				Module.CComPtrNtv_003CIServiceError_003E_002ERelease(&cComPtrNtv_003CIServiceError_003E);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CINewsletterSettings_003E*, void>)(&Module.CComPtrNtv_003CINewsletterSettings_003E_002E_007Bdtor_007D), &cComPtrNtv_003CINewsletterSettings_003E);
				throw;
			}
			Module.CComPtrNtv_003CINewsletterSettings_003E_002ERelease(&cComPtrNtv_003CINewsletterSettings_003E);
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
			CComPtrNtv_003CIPrivacySettings_003E cComPtrNtv_003CIPrivacySettings_003E;
			*(long*)(&cComPtrNtv_003CIPrivacySettings_003E) = 0L;
			HRESULT result;
			try
			{
				if (num >= 0 && accountSettings != null)
				{
					num = CreateAccountSettings(accountSettings, null, (IPrivacySettings**)(&cComPtrNtv_003CIPrivacySettings_003E));
				}
				CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E;
				*(long*)(&cComPtrNtv_003CIPassportIdentity_003E) = 0L;
				try
				{
					if (num >= 0 && parentPassportIdentity != null)
					{
						num = parentPassportIdentity.GetComPointer((IPassportIdentity**)(&cComPtrNtv_003CIPassportIdentity_003E));
					}
					CComPtrNtv_003CIServiceError_003E cComPtrNtv_003CIServiceError_003E;
					*(long*)(&cComPtrNtv_003CIServiceError_003E) = 0L;
					try
					{
						if (num >= 0)
						{
							IAccountManagement* p = m_spAccountManagement.p;
							long num2 = *(long*)(&cComPtrNtv_003CIPrivacySettings_003E);
							long num3 = *(long*)(&cComPtrNtv_003CIPassportIdentity_003E);
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPrivacySettings*, IPassportIdentity*, IServiceError**, int>)(*(ulong*)(*(long*)p + 128)))((nint)p, (IPrivacySettings*)num2, (IPassportIdentity*)num3, (IServiceError**)(&cComPtrNtv_003CIServiceError_003E));
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
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIServiceError_003E*, void>)(&Module.CComPtrNtv_003CIServiceError_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIServiceError_003E);
						throw;
					}
					Module.CComPtrNtv_003CIServiceError_003E_002ERelease(&cComPtrNtv_003CIServiceError_003E);
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&Module.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E);
					throw;
				}
				Module.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPrivacySettings_003E*, void>)(&Module.CComPtrNtv_003CIPrivacySettings_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPrivacySettings_003E);
				throw;
			}
			Module.CComPtrNtv_003CIPrivacySettings_003E_002ERelease(&cComPtrNtv_003CIPrivacySettings_003E);
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
			CComPtrNtv_003CINewsletterSettings_003E cComPtrNtv_003CINewsletterSettings_003E;
			*(long*)(&cComPtrNtv_003CINewsletterSettings_003E) = 0L;
			HRESULT result;
			try
			{
				CComPtrNtv_003CIPrivacySettings_003E cComPtrNtv_003CIPrivacySettings_003E;
				*(long*)(&cComPtrNtv_003CIPrivacySettings_003E) = 0L;
				try
				{
					if (num >= 0 && accountSettings != null)
					{
						num = CreateAccountSettings(accountSettings, (INewsletterSettings**)(&cComPtrNtv_003CINewsletterSettings_003E), (IPrivacySettings**)(&cComPtrNtv_003CIPrivacySettings_003E));
					}
					CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E;
					*(long*)(&cComPtrNtv_003CIPassportIdentity_003E) = 0L;
					try
					{
						if (num >= 0 && passportIdentity != null)
						{
							num = passportIdentity.GetComPointer((IPassportIdentity**)(&cComPtrNtv_003CIPassportIdentity_003E));
						}
						CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E2;
						*(long*)(&cComPtrNtv_003CIPassportIdentity_003E2) = 0L;
						try
						{
							if (num >= 0 && parentPassportIdentity != null)
							{
								num = parentPassportIdentity.GetComPointer((IPassportIdentity**)(&cComPtrNtv_003CIPassportIdentity_003E2));
							}
							CComPtrNtv_003CIServiceError_003E cComPtrNtv_003CIServiceError_003E;
							*(long*)(&cComPtrNtv_003CIServiceError_003E) = 0L;
							try
							{
								if (num >= 0)
								{
									IAccountManagement* p = m_spAccountManagement.p;
									long num2 = *(long*)(&cComPtrNtv_003CIPassportIdentity_003E);
									long num3 = *(long*)(&cComPtrNtv_003CINewsletterSettings_003E);
									long num4 = *(long*)(&cComPtrNtv_003CIPrivacySettings_003E);
									long num5 = *(long*)(&cComPtrNtv_003CIPassportIdentity_003E2);
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPassportIdentity*, INewsletterSettings*, IPrivacySettings*, IPassportIdentity*, IServiceError**, int>)(*(ulong*)(*(long*)p + 136)))((nint)p, (IPassportIdentity*)num2, (INewsletterSettings*)num3, (IPrivacySettings*)num4, (IPassportIdentity*)num5, (IServiceError**)(&cComPtrNtv_003CIServiceError_003E));
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
								Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIServiceError_003E*, void>)(&Module.CComPtrNtv_003CIServiceError_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIServiceError_003E);
								throw;
							}
							Module.CComPtrNtv_003CIServiceError_003E_002ERelease(&cComPtrNtv_003CIServiceError_003E);
						}
						catch
						{
							//try-fault
							Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&Module.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E2);
							throw;
						}
						Module.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E2);
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&Module.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E);
						throw;
					}
					Module.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E);
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPrivacySettings_003E*, void>)(&Module.CComPtrNtv_003CIPrivacySettings_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPrivacySettings_003E);
					throw;
				}
				Module.CComPtrNtv_003CIPrivacySettings_003E_002ERelease(&cComPtrNtv_003CIPrivacySettings_003E);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CINewsletterSettings_003E*, void>)(&Module.CComPtrNtv_003CINewsletterSettings_003E_002E_007Bdtor_007D), &cComPtrNtv_003CINewsletterSettings_003E);
				throw;
			}
			Module.CComPtrNtv_003CINewsletterSettings_003E_002ERelease(&cComPtrNtv_003CINewsletterSettings_003E);
			return result;
		}

		public unsafe HRESULT GetTermsOfService(string languageCode, string countryCode, out string termsOfService)
		{
			//IL_0045: Expected I, but got I8
			//IL_0065: Expected I, but got I8
			int num = CreateComObject();
			WBSTRString wBSTRString;
			Module.WBSTRString_002E_007Bctor_007D(&wBSTRString);
			HRESULT result;
			try
			{
				if (num >= 0)
				{
					fixed (char* countryCodePtr = countryCode.ToCharArray())
					{
						ushort* ptr2 = (ushort*)countryCodePtr;
						try
						{
							fixed (char* languageCodePtr = languageCode.ToCharArray())
							{
								ushort* ptr = (ushort*)languageCodePtr;
								try
								{
									IAccountManagement* p = m_spAccountManagement.p;
									long num2 = *(long*)p + 144;
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, ushort**, int>)(*(ulong*)num2))((nint)p, ptr, ptr2, (ushort**)(&wBSTRString));
								}
								catch
								{
									//try-fault
									ptr = null;
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
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&Module.WBSTRString_002E_007Bdtor_007D), &wBSTRString);
				throw;
			}
			Module.WBSTRString_002E_007Bdtor_007D(&wBSTRString);
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
				fixed (char* offerIdPtr = offerId.ToCharArray())
				{
					ushort* ptr2 = (ushort*)offerIdPtr;
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
				num = Module.SafeArrayGetUBound(ptr, 1u, &num3);
			}
			int num4 = 0;
			if (num >= 0)
			{
				num = Module.SafeArrayGetLBound(ptr, 1u, &num4);
				if (num >= 0)
				{
					bulletStrings = new ArrayList(num3 - num4 + 1);
					int num5 = num4;
					if (num4 <= num3)
					{
						do
						{
							ushort* value = null;
							num = Module.SafeArrayGetElement(ptr, &num5, &value);
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
				Module.SafeArrayDestroy(ptr);
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
			CComPtrNtv_003CIAddress_003E cComPtrNtv_003CIAddress_003E;
			*(long*)(&cComPtrNtv_003CIAddress_003E) = 0L;
			try
			{
				if (num >= 0)
				{
					IAccountManagement* p = m_spAccountManagement.p;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IAddress**, int>)(*(ulong*)(*(long*)p + 32)))((nint)p, (IAddress**)(&cComPtrNtv_003CIAddress_003E));
					if (num >= 0)
					{
						fixed (char* address.Street1Ptr = address.Street1.ToCharArray())
						{
							ushort* ptr = (ushort*)address.Street1Ptr;
							try
							{
								fixed (char* address.Street2Ptr = address.Street2.ToCharArray())
								{
									ushort* ptr2 = (ushort*)address.Street2Ptr;
									try
									{
										fixed (char* address.CityPtr = address.City.ToCharArray())
										{
											ushort* ptr3 = (ushort*)address.CityPtr;
											try
											{
												fixed (char* address.StatePtr = address.State.ToCharArray())
												{
													ushort* ptr4 = (ushort*)address.StatePtr;
													try
													{
														fixed (char* address.DistrictPtr = address.District.ToCharArray())
														{
															ushort* ptr5 = (ushort*)address.DistrictPtr;
															try
															{
																fixed (char* address.PostalCodePtr = address.PostalCode.ToCharArray())
																{
																	ushort* ptr6 = (ushort*)address.PostalCodePtr;
																	try
																	{
																		long num2 = *(long*)(*(ulong*)(&cComPtrNtv_003CIAddress_003E)) + 24;
																		long num3 = *(long*)(&cComPtrNtv_003CIAddress_003E);
																		num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, ushort*, ushort*, ushort*, ushort*, int>)(*(ulong*)num2))((nint)num3, ptr, ptr2, ptr3, ptr4, ptr5, ptr6);
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
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIAddress_003E*, void>)(&Module.CComPtrNtv_003CIAddress_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIAddress_003E);
				throw;
			}
			Module.CComPtrNtv_003CIAddress_003E_002ERelease(&cComPtrNtv_003CIAddress_003E);
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
			CComPtrNtv_003CICreditCard_003E cComPtrNtv_003CICreditCard_003E;
			*(long*)(&cComPtrNtv_003CICreditCard_003E) = 0L;
			try
			{
				if (num >= 0)
				{
					IAccountManagement* p = m_spAccountManagement.p;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ICreditCard**, int>)(*(ulong*)(*(long*)p + 40)))((nint)p, (ICreditCard**)(&cComPtrNtv_003CICreditCard_003E));
				}
				CComPtrNtv_003CIAddress_003E cComPtrNtv_003CIAddress_003E;
				*(long*)(&cComPtrNtv_003CIAddress_003E) = 0L;
				try
				{
					if (num >= 0)
					{
						num = CreateAddress(creditCard.Address, (IAddress**)(&cComPtrNtv_003CIAddress_003E));
						if (num >= 0)
						{
							_SYSTEMTIME sYSTEMTIME = Module.DateTimeToSystemTime(creditCard.ExpirationDate);
							fixed (char* creditCard.AccountHolderNamePtr = creditCard.AccountHolderName.ToCharArray())
							{
								ushort* ptr = (ushort*)creditCard.AccountHolderNamePtr;
								try
								{
									fixed (char* creditCard.AccountNumberPtr = creditCard.AccountNumber.ToCharArray())
									{
										ushort* ptr2 = (ushort*)creditCard.AccountNumberPtr;
										try
										{
											fixed (char* creditCard.CCVNumberPtr = creditCard.CCVNumber.ToCharArray())
											{
												ushort* ptr3 = (ushort*)creditCard.CCVNumberPtr;
												try
												{
													fixed (char* creditCard.LocalePtr = creditCard.Locale.ToCharArray())
													{
														ushort* ptr4 = (ushort*)creditCard.LocalePtr;
														try
														{
															fixed (char* creditCard.PhoneNumberPtr = creditCard.PhoneNumber.ToCharArray())
															{
																ushort* ptr5 = (ushort*)creditCard.PhoneNumberPtr;
																try
																{
																	fixed (char* creditCard.EmailPtr = creditCard.Email.ToCharArray())
																	{
																		ushort* ptr6 = (ushort*)creditCard.EmailPtr;
																		try
																		{
																			fixed (char* creditCard.ContactFirstNamePtr = creditCard.ContactFirstName.ToCharArray())
																			{
																				ushort* ptr7 = (ushort*)creditCard.ContactFirstNamePtr;
																				try
																				{
																					fixed (char* creditCard.ContactLastNamePtr = creditCard.ContactLastName.ToCharArray())
																					{
																						ushort* ptr8 = (ushort*)creditCard.ContactLastNamePtr;
																						try
																						{
																							long num2 = *(long*)(*(ulong*)(&cComPtrNtv_003CICreditCard_003E)) + 24;
																							long num3 = *(long*)(&cComPtrNtv_003CICreditCard_003E);
																							long num4 = *(long*)(&cComPtrNtv_003CIAddress_003E);
																							CreditCardType creditCardType = creditCard.CreditCardType;
																							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IAddress*, ECreditCardType, ushort*, ushort*, ushort*, ushort*, ushort*, ushort*, ushort*, ushort*, _SYSTEMTIME*, int>)(*(ulong*)num2))((nint)num3, (IAddress*)num4, (ECreditCardType)creditCardType, ptr, ptr2, ptr3, ptr4, ptr5, ptr6, ptr7, ptr8, &sYSTEMTIME);
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
								long num5 = *(long*)(&cComPtrNtv_003CICreditCard_003E);
								bool parentCreditCard = creditCard.ParentCreditCard;
								((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CICreditCard_003E)) + 40)))((nint)num5, parentCreditCard ? ((byte)1) : ((byte)0));
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
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIAddress_003E*, void>)(&Module.CComPtrNtv_003CIAddress_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIAddress_003E);
					throw;
				}
				Module.CComPtrNtv_003CIAddress_003E_002ERelease(&cComPtrNtv_003CIAddress_003E);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CICreditCard_003E*, void>)(&Module.CComPtrNtv_003CICreditCard_003E_002E_007Bdtor_007D), &cComPtrNtv_003CICreditCard_003E);
				throw;
			}
			Module.CComPtrNtv_003CICreditCard_003E_002ERelease(&cComPtrNtv_003CICreditCard_003E);
			return num;
		}

		private unsafe int CreateAccountSettings(AccountSettings accountSettings, INewsletterSettings** ppNewsletterSettings, IPrivacySettings** ppPrivacySettings)
		{
			//IL_002b: Expected I, but got I8
			//IL_0062: Expected I, but got I8
			//IL_0062: Expected I, but got I8
			//IL_009a: Expected I, but got I8
			//IL_00ec: Expected I, but got I8
			//IL_00ec: Expected I, but got I8
			//IL_010f: Expected I, but got I8
			//IL_0118: Expected I8, but got I
			//IL_0127: Expected I, but got I8
			//IL_0130: Expected I8, but got I
			int num = CreateComObject();
			CComPtrNtv_003CINewsletterSettings_003E cComPtrNtv_003CINewsletterSettings_003E;
			*(long*)(&cComPtrNtv_003CINewsletterSettings_003E) = 0L;
			try
			{
				if (num >= 0)
				{
					IAccountManagement* p = m_spAccountManagement.p;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, INewsletterSettings**, int>)(*(ulong*)(*(long*)p + 48)))((nint)p, (INewsletterSettings**)(&cComPtrNtv_003CINewsletterSettings_003E));
					if (num >= 0)
					{
						NewsletterOptions newsletterOptions;
						*(EmailFormat*)(&newsletterOptions) = accountSettings.EmailFormat;
                        Unsafe.As<NewsletterOptions, int>(ref Unsafe.AddByteOffset(ref newsletterOptions, 4)) = (accountSettings.AllowZuneEmails ? 1 : 0);
                        Unsafe.As<NewsletterOptions, int>(ref Unsafe.AddByteOffset(ref newsletterOptions, 8)) = (accountSettings.AllowPartnerEmails ? 1 : 0);
						long num2 = *(long*)(&cComPtrNtv_003CINewsletterSettings_003E);
						NewsletterOptions newsletterOptions2 = newsletterOptions;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, NewsletterOptions, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CINewsletterSettings_003E)) + 24)))((nint)num2, newsletterOptions2);
					}
				}
				CComPtrNtv_003CIPrivacySettings_003E cComPtrNtv_003CIPrivacySettings_003E;
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
									long num3 = *(long*)(&cComPtrNtv_003CIPrivacySettings_003E);
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EPrivacySettingId, EPrivacySettingValue, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIPrivacySettings_003E)) + 24)))((nint)num3, key, value);
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
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPrivacySettings_003E*, void>)(&Module.CComPtrNtv_003CIPrivacySettings_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPrivacySettings_003E);
					throw;
				}
				Module.CComPtrNtv_003CIPrivacySettings_003E_002ERelease(&cComPtrNtv_003CIPrivacySettings_003E);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CINewsletterSettings_003E*, void>)(&Module.CComPtrNtv_003CINewsletterSettings_003E_002E_007Bdtor_007D), &cComPtrNtv_003CINewsletterSettings_003E);
				throw;
			}
			Module.CComPtrNtv_003CINewsletterSettings_003E_002ERelease(&cComPtrNtv_003CINewsletterSettings_003E);
			return num;
		}

		private unsafe int CreateAccountUser(AccountUser accountUser, IAccountUser** ppAccountUser)
		{
			//IL_002e: Expected I, but got I8
			//IL_0147: Incompatible stack types: I8 vs Ref
			//IL_017c: Expected I, but got I8
			//IL_017c: Expected I, but got I8
			//IL_017c: Expected I, but got I8
			//IL_017c: Expected I, but got I8
			//IL_017c: Expected I, but got I8
			//IL_017c: Expected I, but got I8
			//IL_017c: Expected I, but got I8
			//IL_01d4: Expected I, but got I8
			//IL_01dd: Expected I8, but got I
			int num = CreateComObject();
			CComPtrNtv_003CIAccountUser_003E cComPtrNtv_003CIAccountUser_003E;
			*(long*)(&cComPtrNtv_003CIAccountUser_003E) = 0L;
			try
			{
				if (num >= 0)
				{
					IAccountManagement* p = m_spAccountManagement.p;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IAccountUser**, int>)(*(ulong*)(*(long*)p + 64)))((nint)p, (IAccountUser**)(&cComPtrNtv_003CIAccountUser_003E));
				}
				CComPtrNtv_003CIAddress_003E cComPtrNtv_003CIAddress_003E;
				*(long*)(&cComPtrNtv_003CIAddress_003E) = 0L;
				try
				{
					if (num >= 0 && accountUser.Address != null)
					{
						num = CreateAddress(accountUser.Address, (IAddress**)(&cComPtrNtv_003CIAddress_003E));
					}
					CComPtrNtv_003CINewsletterSettings_003E cComPtrNtv_003CINewsletterSettings_003E;
					*(long*)(&cComPtrNtv_003CINewsletterSettings_003E) = 0L;
					try
					{
						CComPtrNtv_003CIPrivacySettings_003E cComPtrNtv_003CIPrivacySettings_003E;
						*(long*)(&cComPtrNtv_003CIPrivacySettings_003E) = 0L;
						try
						{
							if (num >= 0 && accountUser.AccountSettings != null)
							{
								num = CreateAccountSettings(accountUser.AccountSettings, (INewsletterSettings**)(&cComPtrNtv_003CINewsletterSettings_003E), (IPrivacySettings**)(&cComPtrNtv_003CIPrivacySettings_003E));
							}
							CComPtrNtv_003CICreditCard_003E cComPtrNtv_003CICreditCard_003E;
							*(long*)(&cComPtrNtv_003CICreditCard_003E) = 0L;
							try
							{
								if (num >= 0 && accountUser.ParentCreditCard != null)
								{
									num = CreateCreditCard(accountUser.ParentCreditCard, (ICreditCard**)(&cComPtrNtv_003CICreditCard_003E));
								}
								CComPtrNtv_003CIPassportIdentity_003E cComPtrNtv_003CIPassportIdentity_003E;
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
											_SYSTEMTIME sYSTEMTIME;
											if (flag)
											{
												sYSTEMTIME = Module.DateTimeToSystemTime(accountUser.Birthday);
											}
											fixed (char* accountUser.ZuneTagPtr = accountUser.ZuneTag.ToCharArray())
											{
												ushort* ptr2 = (ushort*)accountUser.ZuneTagPtr;
												try
												{
													fixed (char* accountUser.LocalePtr = accountUser.Locale.ToCharArray())
													{
														ushort* ptr3 = (ushort*)accountUser.LocalePtr;
														try
														{
															fixed (char* accountUser.FirstNamePtr = accountUser.FirstName.ToCharArray())
															{
																ushort* ptr4 = (ushort*)accountUser.FirstNamePtr;
																try
																{
																	fixed (char* accountUser.LastNamePtr = accountUser.LastName.ToCharArray())
																	{
																		ushort* ptr5 = (ushort*)accountUser.LastNamePtr;
																		try
																		{
																			fixed (char* accountUser.EmailPtr = accountUser.Email.ToCharArray())
																			{
																				ushort* ptr6 = (ushort*)accountUser.EmailPtr;
																				try
																				{
																					fixed (char* accountUser.PhoneNumberPtr = accountUser.PhoneNumber.ToCharArray())
																					{
																						ushort* ptr7 = (ushort*)accountUser.PhoneNumberPtr;
																						try
																						{
																							fixed (char* accountUser.MobilePhoneNumberPtr = accountUser.MobilePhoneNumber.ToCharArray())
																							{
																								ushort* ptr8 = (ushort*)accountUser.MobilePhoneNumberPtr;
																								try
																								{
																									_SYSTEMTIME* ptr = (_SYSTEMTIME*)Unsafe.AsPointer(ref flag ? ref sYSTEMTIME : ref *(_SYSTEMTIME*)null);
																									long num2 = *(long*)(*(ulong*)(&cComPtrNtv_003CIAccountUser_003E)) + 24;
																									long num3 = *(long*)(&cComPtrNtv_003CIAccountUser_003E);
																									_003F val = ptr2;
																									_003F val2 = ptr3;
																									_003F val3 = ptr4;
																									_003F val4 = ptr5;
																									_003F val5 = ptr6;
																									_003F val6 = ptr7;
																									_003F val7 = ptr8;
																									long num4 = *(long*)(&cComPtrNtv_003CIAddress_003E);
																									long num5 = *(long*)(&cComPtrNtv_003CINewsletterSettings_003E);
																									long num6 = *(long*)(&cComPtrNtv_003CIPrivacySettings_003E);
																									long num7 = *(long*)(&cComPtrNtv_003CIPassportIdentity_003E);
																									long num8 = *(long*)(&cComPtrNtv_003CICreditCard_003E);
																									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, _SYSTEMTIME*, ushort*, ushort*, ushort*, ushort*, ushort*, IAddress*, INewsletterSettings*, IPrivacySettings*, IPassportIdentity*, ICreditCard*, int>)(*(ulong*)num2))((nint)num3, (ushort*)(nint)val, (ushort*)(nint)val2, ptr, (ushort*)(nint)val3, (ushort*)(nint)val4, (ushort*)(nint)val5, (ushort*)(nint)val6, (ushort*)(nint)val7, (IAddress*)num4, (INewsletterSettings*)num5, (IPrivacySettings*)num6, (IPassportIdentity*)num7, (ICreditCard*)num8);
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
									Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPassportIdentity_003E*, void>)(&Module.CComPtrNtv_003CIPassportIdentity_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPassportIdentity_003E);
									throw;
								}
								Module.CComPtrNtv_003CIPassportIdentity_003E_002ERelease(&cComPtrNtv_003CIPassportIdentity_003E);
							}
							catch
							{
								//try-fault
								Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CICreditCard_003E*, void>)(&Module.CComPtrNtv_003CICreditCard_003E_002E_007Bdtor_007D), &cComPtrNtv_003CICreditCard_003E);
								throw;
							}
							Module.CComPtrNtv_003CICreditCard_003E_002ERelease(&cComPtrNtv_003CICreditCard_003E);
						}
						catch
						{
							//try-fault
							Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPrivacySettings_003E*, void>)(&Module.CComPtrNtv_003CIPrivacySettings_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPrivacySettings_003E);
							throw;
						}
						Module.CComPtrNtv_003CIPrivacySettings_003E_002ERelease(&cComPtrNtv_003CIPrivacySettings_003E);
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CINewsletterSettings_003E*, void>)(&Module.CComPtrNtv_003CINewsletterSettings_003E_002E_007Bdtor_007D), &cComPtrNtv_003CINewsletterSettings_003E);
						throw;
					}
					Module.CComPtrNtv_003CINewsletterSettings_003E_002ERelease(&cComPtrNtv_003CINewsletterSettings_003E);
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIAddress_003E*, void>)(&Module.CComPtrNtv_003CIAddress_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIAddress_003E);
					throw;
				}
				Module.CComPtrNtv_003CIAddress_003E_002ERelease(&cComPtrNtv_003CIAddress_003E);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIAccountUser_003E*, void>)(&Module.CComPtrNtv_003CIAccountUser_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIAccountUser_003E);
				throw;
			}
			Module.CComPtrNtv_003CIAccountUser_003E_002ERelease(&cComPtrNtv_003CIAccountUser_003E);
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
				CComPtrNtv_003CIService_003E cComPtrNtv_003CIService_003E;
				*(long*)(&cComPtrNtv_003CIService_003E) = 0L;
				try
				{
					num = Module.GetSingleton((_GUID)Module._GUID_bb2d1edd_1bd5_4be1_8d38_36d4f0849911, (void**)(&cComPtrNtv_003CIService_003E));
					CComPtrNtv_003CIUnknown_003E cComPtrNtv_003CIUnknown_003E;
					*(long*)(&cComPtrNtv_003CIUnknown_003E) = 0L;
					try
					{
						if (num >= 0)
						{
							long num2 = *(long*)(&cComPtrNtv_003CIService_003E);
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IUnknown**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIService_003E)) + 104)))((nint)num2, (IUnknown**)(&cComPtrNtv_003CIUnknown_003E));
						}
						CComPtrNtv_003CIAccountManagement_003E cComPtrNtv_003CIAccountManagement_003E;
						*(long*)(&cComPtrNtv_003CIAccountManagement_003E) = 0L;
						try
						{
							if (num >= 0)
							{
								num = Module.CComPtrNtv_003CIUnknown_003E_002EQueryInterface_003Cstruct_0020IAccountManagement_003E(&cComPtrNtv_003CIUnknown_003E, (IAccountManagement**)(&cComPtrNtv_003CIAccountManagement_003E));
								if (num >= 0)
								{
									long num3 = *(long*)(&cComPtrNtv_003CIAccountManagement_003E);
									long num4 = *(long*)(&cComPtrNtv_003CIService_003E);
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IService*, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIAccountManagement_003E)) + 24)))((nint)num3, (IService*)num4);
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
							Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIAccountManagement_003E*, void>)(&Module.CComPtrNtv_003CIAccountManagement_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIAccountManagement_003E);
							throw;
						}
						Module.CComPtrNtv_003CIAccountManagement_003E_002ERelease(&cComPtrNtv_003CIAccountManagement_003E);
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIUnknown_003E*, void>)(&Module.CComPtrNtv_003CIUnknown_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIUnknown_003E);
						throw;
					}
					Module.CComPtrNtv_003CIUnknown_003E_002ERelease(&cComPtrNtv_003CIUnknown_003E);
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIService_003E*, void>)(&Module.CComPtrNtv_003CIService_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIService_003E);
					throw;
				}
				Module.CComPtrNtv_003CIService_003E_002ERelease(&cComPtrNtv_003CIService_003E);
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
				}
				finally
				{
					((IDisposable)m_spAccountManagement).Dispose();
				}
			}
			else
			{
				Finalize();
			}
		}

		public sealed override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
