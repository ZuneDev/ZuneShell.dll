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
		private readonly CComPtrMgd<IAccountManagement> m_spAccountManagement;

		public AccountManagement()
		{
			CComPtrMgd<IAccountManagement> spAccountManagement = new CComPtrMgd<IAccountManagement>();
			try
			{
				m_spAccountManagement = spAccountManagement;
			}
			catch
			{
				//try-fault
				m_spAccountManagement.Dispose();
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
			int hresult = CreateComObject();
			serviceError = null;
			CComPtrNtv<INewsletterSettings> cComPtrNtv_003CINewsletterSettings_003E = new();
			HRESULT result;
			try
			{
				CComPtrNtv<IPrivacySettings> cComPtrNtv_003CIPrivacySettings_003E = new();
				try
				{
					if (hresult >= 0 && accountSettings != null)
					{
						hresult = CreateAccountSettings(accountSettings, Module.CComPtrNtv_003CINewsletterSettings_003E_002E_0026(cComPtrNtv_003CINewsletterSettings_003E.p), Module.CComPtrNtv_003CIPrivacySettings_003E_002E_0026(cComPtrNtv_003CIPrivacySettings_003E.p));
					}
					CComPtrNtv<IPassportIdentity> cComPtrNtv_003CIPassportIdentity_003E = new();
					try
					{
						if (hresult >= 0 && passportIdentity != null)
						{
							IPassportIdentity* ptr = cComPtrNtv_003CIPassportIdentity_003E.p;
							hresult = passportIdentity.GetComPointer(&ptr);
						}
						CComPtrNtv<IPassportIdentity> cComPtrNtv_003CIPassportIdentity_003E2 = new();
						try
						{
							if (hresult >= 0 && parentPassportIdentity != null)
							{
								IPassportIdentity* ptr = cComPtrNtv_003CIPassportIdentity_003E2.p;
								hresult = parentPassportIdentity.GetComPointer(&ptr);
							}
							CComPtrNtv<ICreditCard> cComPtrNtv_003CICreditCard_003E = new();
							try
							{
								if (hresult >= 0 && parentCreditCard != null)
								{
									hresult = CreateCreditCard(parentCreditCard, cComPtrNtv_003CICreditCard_003E.GetPtrToPtr());
								}
								CComPtrNtv<IAddress> cComPtrNtv_003CIAddress_003E = new();
								try
								{
									if (hresult >= 0 && address != null)
									{
										hresult = CreateAddress(address, cComPtrNtv_003CIAddress_003E.GetPtrToPtr());
									}
									CComPtrNtv<IServiceError> cComPtrNtv_003CIServiceError_003E = new();
									try
									{
										if (hresult >= 0)
										{
											_SYSTEMTIME sYSTEMTIME = Module.DateTimeToSystemTime(birthday);
											fixed (char* firstNamePtr = firstName)
											{
												ushort* ptr3 = (ushort*)firstNamePtr;
												try
												{
													fixed (char* lastNamePtr = lastName)
													{
														ushort* ptr4 = (ushort*)lastNamePtr;
														try
														{
															fixed (char* zuneTagPtr = zuneTag)
															{
																ushort* ptr = (ushort*)zuneTagPtr;
																try
																{
																	fixed (char* emailPtr = email)
																	{
																		ushort* ptr5 = (ushort*)emailPtr;
																		try
																		{
																			fixed (char* localePtr = locale)
																			{
																				ushort* ptr2 = (ushort*)localePtr;
																				try
																				{
																					IAccountManagement* p = m_spAccountManagement.p;
																					long num2 = *(long*)p + 72;
																					long num3 = *(long*)(cComPtrNtv_003CIPassportIdentity_003E.p);
																					long num4 = *(long*)(cComPtrNtv_003CIAddress_003E.p);
																					long num5 = *(long*)(cComPtrNtv_003CINewsletterSettings_003E.p);
																					long num6 = *(long*)(cComPtrNtv_003CIPrivacySettings_003E.p);
																					long num7 = *(long*)(cComPtrNtv_003CIPassportIdentity_003E2.p);
																					long num8 = *(long*)(cComPtrNtv_003CICreditCard_003E.p);
																					hresult = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPassportIdentity*, ushort*, ushort*, _SYSTEMTIME*, ushort*, ushort*, ushort*, IAddress*, INewsletterSettings*, IPrivacySettings*, IPassportIdentity*, ICreditCard*, IServiceError**, int>)(*(ulong*)num2))((nint)p, (IPassportIdentity*)num3, (ushort*)(nint)ptr, (ushort*)(nint)ptr2, &sYSTEMTIME, (ushort*)(nint)ptr3, (ushort*)(nint)ptr4, (ushort*)(nint)ptr5, (IAddress*)num4, (INewsletterSettings*)num5, (IPrivacySettings*)num6, (IPassportIdentity*)num7, (ICreditCard*)num8, (IServiceError**)(cComPtrNtv_003CIServiceError_003E.p));
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
										if (*(long*)(cComPtrNtv_003CIServiceError_003E.p) != 0L)
										{
											serviceError = new ServiceError((IServiceError*)(*(ulong*)(cComPtrNtv_003CIServiceError_003E.p)));
										}
										result = new HRESULT(hresult);
									}
									finally
									{
										cComPtrNtv_003CIServiceError_003E.Dispose();
									}
								}
								finally
								{
									cComPtrNtv_003CIAddress_003E.Dispose();
								}
							}
							finally
							{
								cComPtrNtv_003CICreditCard_003E.Dispose();
							}
						}
						finally
						{
							cComPtrNtv_003CIPassportIdentity_003E2.Dispose();
						}
					}
					finally
					{
						cComPtrNtv_003CIPassportIdentity_003E.Dispose();
					}
				}
				finally
				{
					cComPtrNtv_003CIPrivacySettings_003E.Dispose();
				}
			}
			finally
			{
				cComPtrNtv_003CINewsletterSettings_003E.Dispose();
			}
			return result;
		}

		public unsafe HRESULT ReserveZuneTag(string zuneTag, string countryCode, out IList suggestedNames, out ServiceError serviceError)
        {
            serviceError = null;
            //IL_004e: Expected I, but got I8
            //IL_007c: Expected I, but got I8
            //IL_007c: Expected I, but got I8
            //IL_008f: Expected I, but got I8
            //IL_00a3: Expected I, but got I8
            //IL_00a3: Expected I, but got I8
            //IL_00d9: Expected I, but got I8
            int num = CreateComObject();
			suggestedNames = null;
			CComPtrNtv<IAvailableZuneTagInformation> cComPtrNtv_003CIAvailableZuneTagInformation_003E = new();
			HRESULT result;
			try
			{
				CComPtrNtv<IServiceError> cComPtrNtv_003CIServiceError_003E = new();
				try
				{
					if (num >= 0)
					{
						fixed (char* zuneTagPtr = zuneTag)
						{
							ushort* ptr = (ushort*)zuneTagPtr;
							try
							{
								fixed (char* countryCodePtr = countryCode)
								{
									ushort* ptr2 = (ushort*)countryCodePtr;
									try
									{
										IAccountManagement* p = m_spAccountManagement.p;
										long num2 = *(long*)p + 80;
										num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, IAvailableZuneTagInformation**, IServiceError**, int>)(*(ulong*)num2))((nint)p, ptr, ptr2, (IAvailableZuneTagInformation**)cComPtrNtv_003CIAvailableZuneTagInformation_003E.p, (IServiceError**)cComPtrNtv_003CIServiceError_003E.p);
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
						long num3 = *(long*)cComPtrNtv_003CIAvailableZuneTagInformation_003E.p;
						int num4 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)*(ulong*)cComPtrNtv_003CIAvailableZuneTagInformation_003E.p + 48)))((nint)num3);
						suggestedNames = new ArrayList(num4);
						int num5 = 0;
						if (0 < num4)
						{
							do
							{
								ushort* ptr3 = null;
								long num6 = *(long*)cComPtrNtv_003CIAvailableZuneTagInformation_003E.p;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, ushort**, int>)(*(ulong*)(*(long*)*(ulong*)cComPtrNtv_003CIAvailableZuneTagInformation_003E.p + 56)))((nint)num6, num5, &ptr3);
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
					if (*(long*)cComPtrNtv_003CIServiceError_003E.p != 0L)
					{
						serviceError = new ServiceError((IServiceError*)*(ulong*)cComPtrNtv_003CIServiceError_003E.p);
					}
					result = num;
				}
				catch
				{
					//try-fault
					cComPtrNtv_003CIServiceError_003E.Dispose();
					throw;
				}
				cComPtrNtv_003CIServiceError_003E.Dispose();
			}
			catch
			{
				//try-fault
				cComPtrNtv_003CIAvailableZuneTagInformation_003E.Dispose();
				throw;
			}
			cComPtrNtv_003CIAvailableZuneTagInformation_003E.Dispose();
			return result;
        }

        public unsafe HRESULT ValidateCreditCard(PassportIdentity parentPassportIdentity, CreditCard creditCard, out ServiceError serviceError)
        {
            serviceError = null;
            //IL_005c: Expected I, but got I8
            //IL_005c: Expected I, but got I8
            //IL_005c: Expected I, but got I8
            //IL_006b: Expected I, but got I8
            int num = CreateComObject();
			CComPtrNtv<ICreditCard> cComPtrNtv_003CICreditCard_003E = new();
			HRESULT result;
			try
			{
				if (num >= 0 && creditCard != null)
				{
					num = CreateCreditCard(creditCard, cComPtrNtv_003CICreditCard_003E.GetPtrToPtr());
				}
				CComPtrNtv<IPassportIdentity> cComPtrNtv_003CIPassportIdentity_003E = new();
				try
				{
					if (num >= 0 && parentPassportIdentity != null)
					{
						num = parentPassportIdentity.GetComPointer(cComPtrNtv_003CIPassportIdentity_003E.GetPtrToPtr());
					}
					CComPtrNtv<IServiceError> cComPtrNtv_003CIServiceError_003E = new();
					try
					{
						if (num >= 0)
						{
							IAccountManagement* p = m_spAccountManagement.p;
							long num2 = *(long*)cComPtrNtv_003CIPassportIdentity_003E.p;
							long num3 = *(long*)cComPtrNtv_003CICreditCard_003E.p;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPassportIdentity*, ICreditCard*, IServiceError**, int>)(*(ulong*)(*(long*)p + 88)))((nint)p, (IPassportIdentity*)num2, (ICreditCard*)num3, (IServiceError**)cComPtrNtv_003CIServiceError_003E.p);
						}
						if (*(long*)cComPtrNtv_003CIServiceError_003E.p != 0L)
						{
							serviceError = new ServiceError((IServiceError*)*(ulong*)cComPtrNtv_003CIServiceError_003E.p);
						}
						result = new HRESULT(num);
					}
					finally
					{
						cComPtrNtv_003CIServiceError_003E.Dispose();
					}
				}
				finally
				{
					cComPtrNtv_003CIPassportIdentity_003E.Dispose();
				}
			}
			finally
			{
				cComPtrNtv_003CICreditCard_003E.Dispose();
			}
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
			CComPtrNtv<IPassportIdentity> cComPtrNtv_003CIPassportIdentity_003E = new();
			HRESULT result;
			try
			{
				if (num >= 0)
				{
					if (passportIdentity != null)
					{
						num = passportIdentity.GetComPointer(cComPtrNtv_003CIPassportIdentity_003E.GetPtrToPtr());
					}
					if (num >= 0)
					{
						IAccountManagement* p = m_spAccountManagement.p;
						long num2 = *(long*)(cComPtrNtv_003CIPassportIdentity_003E.p);
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
			finally
			{
				cComPtrNtv_003CIPassportIdentity_003E.Dispose();
			}
			return result;
		}

		public unsafe HRESULT GetAccount(PassportIdentity passportIdentity, out AccountUser accountUser, out ServiceError serviceError)
		{
			//IL_004a: Expected I, but got I8
			//IL_004a: Expected I, but got I8
			//IL_0059: Expected I, but got I8
			//IL_0068: Expected I, but got I8
			serviceError = null;
			accountUser = null;
			int num = CreateComObject();
			CComPtrNtv<IPassportIdentity> cComPtrNtv_003CIPassportIdentity_003E = new();
			HRESULT result;
			try
			{
				if (num >= 0 && passportIdentity != null)
				{
					num = passportIdentity.GetComPointer(cComPtrNtv_003CIPassportIdentity_003E.GetPtrToPtr());
				}
				CComPtrNtv<IAccountUser> cComPtrNtv_003CIAccountUser_003E = new();
				try
				{
					CComPtrNtv<IServiceError> cComPtrNtv_003CIServiceError_003E = new();
					try
					{
						if (num >= 0)
						{
							IAccountManagement* p = m_spAccountManagement.p;
							long num2 = *(long*)(cComPtrNtv_003CIPassportIdentity_003E.p);
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPassportIdentity*, IAccountUser**, IServiceError**, int>)(*(ulong*)(*(long*)p + 104)))((nint)p, (IPassportIdentity*)num2, (IAccountUser**)(cComPtrNtv_003CIAccountUser_003E.p), (IServiceError**)(cComPtrNtv_003CIServiceError_003E.p));
						}
						if (*(long*)(cComPtrNtv_003CIServiceError_003E.p) != 0L)
						{
							serviceError = new ServiceError((IServiceError*)(*(ulong*)(cComPtrNtv_003CIServiceError_003E.p)));
						}
						if (*(long*)(cComPtrNtv_003CIAccountUser_003E.p) != 0L)
						{
							accountUser = new AccountUser((IAccountUser*)(*(ulong*)(cComPtrNtv_003CIAccountUser_003E.p)));
						}
						result = new HRESULT(num);
					}
					finally
					{
						cComPtrNtv_003CIServiceError_003E.Dispose();
					}
				}
				finally
				{
					cComPtrNtv_003CIAccountUser_003E.Dispose();
				}
			}
			finally
			{
				cComPtrNtv_003CIPassportIdentity_003E.Dispose();
			}
			return result;
		}

		public unsafe HRESULT SetAccount(PassportIdentity passportIdentity, AccountUser accountUser, out ServiceError serviceError)
        {
            serviceError = null;
            //IL_005c: Expected I, but got I8
            //IL_005c: Expected I, but got I8
            //IL_005c: Expected I, but got I8
            //IL_006b: Expected I, but got I8
            int num = CreateComObject();
			CComPtrNtv<IPassportIdentity> cComPtrNtv_003CIPassportIdentity_003E = new();
			HRESULT result;
			try
			{
				if (num >= 0 && passportIdentity != null)
				{
					num = passportIdentity.GetComPointer(cComPtrNtv_003CIPassportIdentity_003E.GetPtrToPtr());
				}
				CComPtrNtv<IAccountUser> cComPtrNtv_003CIAccountUser_003E = new();
				try
				{
					if (num >= 0 && accountUser != null)
					{
						num = CreateAccountUser(accountUser, cComPtrNtv_003CIAccountUser_003E.GetPtrToPtr());
					}
					CComPtrNtv<IServiceError> cComPtrNtv_003CIServiceError_003E = new();
					try
					{
						if (num >= 0)
						{
							IAccountManagement* p = m_spAccountManagement.p;
							long num2 = *(long*)cComPtrNtv_003CIPassportIdentity_003E.p;
							long num3 = *(long*)cComPtrNtv_003CIAccountUser_003E.p;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPassportIdentity*, IAccountUser*, IServiceError**, int>)(*(ulong*)(*(long*)p + 112)))((nint)p, (IPassportIdentity*)num2, (IAccountUser*)num3, (IServiceError**)cComPtrNtv_003CIServiceError_003E.p);
						}
						if (*(long*)cComPtrNtv_003CIServiceError_003E.p != 0L)
						{
							serviceError = new ServiceError((IServiceError*)*(ulong*)cComPtrNtv_003CIServiceError_003E.p);
						}
						result = new HRESULT(num);
					}
					finally
					{
						cComPtrNtv_003CIServiceError_003E.Dispose();
					}
				}
				finally
				{
					cComPtrNtv_003CIAccountUser_003E.Dispose();
				}
			}
			finally
			{
				cComPtrNtv_003CIPassportIdentity_003E.Dispose();
			}
			return result;
        }

        public unsafe HRESULT SetNewsLetterSettings(AccountSettings accountSettings, out ServiceError serviceError)
        {
            serviceError = null;
            //IL_001e: Expected I, but got I8
            //IL_0046: Expected I, but got I8
            //IL_0046: Expected I, but got I8
            //IL_0055: Expected I, but got I8
            int num = CreateComObject();
			CComPtrNtv<INewsletterSettings> cComPtrNtv_003CINewsletterSettings_003E = new();
			HRESULT result;
			try
			{
				if (num >= 0 && accountSettings != null)
				{
					num = CreateAccountSettings(accountSettings, cComPtrNtv_003CINewsletterSettings_003E.GetPtrToPtr(), null);
				}
				CComPtrNtv<IServiceError> cComPtrNtv_003CIServiceError_003E = new();
				try
				{
					if (num >= 0)
					{
						IAccountManagement* p = m_spAccountManagement.p;
						long num2 = *(long*)cComPtrNtv_003CINewsletterSettings_003E.p;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, INewsletterSettings*, IServiceError**, int>)(*(ulong*)(*(long*)p + 120)))((nint)p, (INewsletterSettings*)num2, (IServiceError**)cComPtrNtv_003CIServiceError_003E.p);
					}
					if (*(long*)cComPtrNtv_003CIServiceError_003E.p != 0L)
					{
						serviceError = new ServiceError((IServiceError*)*(ulong*)cComPtrNtv_003CIServiceError_003E.p);
					}
					result = new HRESULT(num);
				}
				finally
				{
					cComPtrNtv_003CIServiceError_003E.Dispose();
				}
			}
			finally
			{
				cComPtrNtv_003CINewsletterSettings_003E.Dispose();
			}
			return result;
        }

        public unsafe HRESULT SetPrivacySettings(AccountSettings accountSettings, PassportIdentity parentPassportIdentity, out ServiceError serviceError)
        {
            serviceError = null;
            //IL_001e: Expected I, but got I8
            //IL_0061: Expected I, but got I8
            //IL_0061: Expected I, but got I8
            //IL_0061: Expected I, but got I8
            //IL_0070: Expected I, but got I8
            int num = CreateComObject();
			CComPtrNtv<IPrivacySettings> cComPtrNtv_003CIPrivacySettings_003E = new();
			HRESULT result;
			try
			{
				if (num >= 0 && accountSettings != null)
				{
					num = CreateAccountSettings(accountSettings, null, cComPtrNtv_003CIPrivacySettings_003E.GetPtrToPtr());
				}
				CComPtrNtv<IPassportIdentity> cComPtrNtv_003CIPassportIdentity_003E = new();
				try
				{
					if (num >= 0 && parentPassportIdentity != null)
					{
						num = parentPassportIdentity.GetComPointer(cComPtrNtv_003CIPassportIdentity_003E.GetPtrToPtr());
					}
					CComPtrNtv<IServiceError> cComPtrNtv_003CIServiceError_003E = new();
					try
					{
						if (num >= 0)
						{
							IAccountManagement* p = m_spAccountManagement.p;
							long num2 = *(long*)cComPtrNtv_003CIPrivacySettings_003E.p;
							long num3 = *(long*)cComPtrNtv_003CIPassportIdentity_003E.p;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPrivacySettings*, IPassportIdentity*, IServiceError**, int>)(*(ulong*)(*(long*)p + 128)))((nint)p, (IPrivacySettings*)num2, (IPassportIdentity*)num3, (IServiceError**)cComPtrNtv_003CIServiceError_003E.p);
						}
						if (*(long*)cComPtrNtv_003CIServiceError_003E.p != 0L)
						{
							serviceError = new ServiceError((IServiceError*)*(ulong*)cComPtrNtv_003CIServiceError_003E.p);
						}
						result = new HRESULT(num);
					}
					finally
					{
						cComPtrNtv_003CIServiceError_003E.Dispose();
					}
				}
				finally
				{
					cComPtrNtv_003CIPassportIdentity_003E.Dispose();
				}
			}
			finally
			{
				cComPtrNtv_003CIPrivacySettings_003E.Dispose();
			}
			return result;
        }

        public unsafe HRESULT UpgradeAccount(PassportIdentity passportIdentity, AccountSettings accountSettings, PassportIdentity parentPassportIdentity, out ServiceError serviceError)
        {
            serviceError = null;
            //IL_0081: Expected I, but got I8
            //IL_0081: Expected I, but got I8
            //IL_0081: Expected I, but got I8
            //IL_0081: Expected I, but got I8
            //IL_0081: Expected I, but got I8
            //IL_0091: Expected I, but got I8
            int num = CreateComObject();
			CComPtrNtv<INewsletterSettings> cComPtrNtv_003CINewsletterSettings_003E = new();
			HRESULT result;
			try
			{
				CComPtrNtv<IPrivacySettings> cComPtrNtv_003CIPrivacySettings_003E = new();
				try
				{
					if (num >= 0 && accountSettings != null)
					{
						num = CreateAccountSettings(accountSettings, (INewsletterSettings**)cComPtrNtv_003CINewsletterSettings_003E.p, (IPrivacySettings**)cComPtrNtv_003CIPrivacySettings_003E.p);
					}
					CComPtrNtv<IPassportIdentity> cComPtrNtv_003CIPassportIdentity_003E = new();
					try
					{
						if (num >= 0 && passportIdentity != null)
						{
							num = passportIdentity.GetComPointer((IPassportIdentity**)cComPtrNtv_003CIPassportIdentity_003E.p);
						}
						CComPtrNtv<IPassportIdentity> cComPtrNtv_003CIPassportIdentity_003E2 = new();
						try
						{
							if (num >= 0 && parentPassportIdentity != null)
							{
								num = parentPassportIdentity.GetComPointer(cComPtrNtv_003CIPassportIdentity_003E2.GetPtrToPtr());
							}
							CComPtrNtv<IServiceError> cComPtrNtv_003CIServiceError_003E = new();
							try
							{
								if (num >= 0)
								{
									IAccountManagement* p = m_spAccountManagement.p;
									long num2 = *(long*)cComPtrNtv_003CIPassportIdentity_003E.p;
									long num3 = *(long*)cComPtrNtv_003CINewsletterSettings_003E.p;
									long num4 = *(long*)cComPtrNtv_003CIPrivacySettings_003E.p;
									long num5 = *(long*)cComPtrNtv_003CIPassportIdentity_003E2.p;
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPassportIdentity*, INewsletterSettings*, IPrivacySettings*, IPassportIdentity*, IServiceError**, int>)(*(ulong*)(*(long*)p + 136)))((nint)p, (IPassportIdentity*)num2, (INewsletterSettings*)num3, (IPrivacySettings*)num4, (IPassportIdentity*)num5, (IServiceError**)cComPtrNtv_003CIServiceError_003E.p);
								}
								if (*(long*)cComPtrNtv_003CIServiceError_003E.p != 0L)
								{
									serviceError = new ServiceError((IServiceError*)*(ulong*)cComPtrNtv_003CIServiceError_003E.p);
								}
								result = new HRESULT(num);
							}
							finally
							{
								cComPtrNtv_003CIServiceError_003E.Dispose();
							}
						}
						finally
						{
							cComPtrNtv_003CIPassportIdentity_003E2.Dispose();
						}
					}
					finally
					{
						cComPtrNtv_003CIPassportIdentity_003E.Dispose();
					}
				}
				finally
				{
					cComPtrNtv_003CIPrivacySettings_003E.Dispose();
				}
			}
			finally
			{
				cComPtrNtv_003CINewsletterSettings_003E.Dispose();
			}
			return result;
        }

        public unsafe HRESULT GetTermsOfService(string languageCode, string countryCode, out string termsOfService)
        {
            termsOfService = null;
            //IL_0045: Expected I, but got I8
            //IL_0065: Expected I, but got I8
            int num = CreateComObject();
			string wBSTRString = "";
			HRESULT result;
			fixed (char* wBSTRStringPtr = wBSTRString)
			{
				if (num >= 0)
				{
					fixed (char* countryCodePtr = countryCode)
					{
						ushort* ptr2 = (ushort*)countryCodePtr;
						try
						{
							fixed (char* languageCodePtr = languageCode)
							{
								ushort* ptr = (ushort*)languageCodePtr;
								try
								{
									IAccountManagement* p = m_spAccountManagement.p;
									long num2 = *(long*)p + 144;
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, ushort**, int>)(*(ulong*)num2))((nint)p, ptr, ptr2, (ushort**)wBSTRStringPtr);
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
						termsOfService = wBSTRString;
					}
				}
				result = new HRESULT(num);
			}
			return result;
        }

        public unsafe HRESULT GetSubscriptionDetails(string offerId, out ArrayList bulletStrings)
        {
            bulletStrings = null;
            //IL_000a: Expected I, but got I8
            //IL_003d: Expected I, but got I8
            //IL_0086: Expected I, but got I8
            int num = CreateComObject();
			tagSAFEARRAY* ptr = null;
			if (num >= 0)
			{
				fixed (char* offerIdPtr = offerId)
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
			CComPtrNtv<IAddress> cComPtrNtv_003CIAddress_003E = new();
			try
			{
				if (num >= 0)
				{
					IAccountManagement* p = m_spAccountManagement.p;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IAddress**, int>)(*(ulong*)(*(long*)p + 32)))((nint)p, (IAddress**)(cComPtrNtv_003CIAddress_003E.p));
					if (num >= 0)
					{
						fixed (char* addressStreet1Ptr = address.Street1)
						{
							ushort* ptr = (ushort*)addressStreet1Ptr;
							try
							{
								fixed (char* addressStreet2Ptr = address.Street2)
								{
									ushort* ptr2 = (ushort*)addressStreet2Ptr;
									try
									{
										fixed (char* addressCityPtr = address.City)
										{
											ushort* ptr3 = (ushort*)addressCityPtr;
											try
											{
												fixed (char* addressStatePtr = address.State)
												{
													ushort* ptr4 = (ushort*)addressStatePtr;
													try
													{
														fixed (char* addressDistrictPtr = address.District)
														{
															ushort* ptr5 = (ushort*)addressDistrictPtr;
															try
															{
																fixed (char* addressPostalCodePtr = address.PostalCode)
																{
																	ushort* ptr6 = (ushort*)addressPostalCodePtr;
																	try
																	{
																		long num2 = *(long*)(*(ulong*)(cComPtrNtv_003CIAddress_003E.p)) + 24;
																		long num3 = *(long*)(cComPtrNtv_003CIAddress_003E.p);
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
							IAddress* ptr7 = (IAddress*)(*(ulong*)(cComPtrNtv_003CIAddress_003E.p));
							*(long*)(cComPtrNtv_003CIAddress_003E.p) = 0L;
							*(long*)ppAddress = (nint)ptr7;
						}
					}
				}
			}
			finally
			{
				cComPtrNtv_003CIAddress_003E.Dispose();
			}
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
			CComPtrNtv<ICreditCard> cComPtrNtv_003CICreditCard_003E = new();
			try
			{
				if (num >= 0)
				{
					IAccountManagement* p = m_spAccountManagement.p;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ICreditCard**, int>)(*(ulong*)(*(long*)p + 40)))((nint)p, cComPtrNtv_003CICreditCard_003E.GetPtrToPtr());
				}
				CComPtrNtv<IAddress> cComPtrNtv_003CIAddress_003E = new();
				try
				{
					if (num >= 0)
					{
						num = CreateAddress(creditCard.Address, cComPtrNtv_003CIAddress_003E.GetPtrToPtr());
						if (num >= 0)
						{
							_SYSTEMTIME sYSTEMTIME = Module.DateTimeToSystemTime(creditCard.ExpirationDate);
							fixed (char* creditCardAccountHolderNamePtr = creditCard.AccountHolderName)
							{
								ushort* ptr = (ushort*)creditCardAccountHolderNamePtr;
								try
								{
									fixed (char* creditCardAccountNumberPtr = creditCard.AccountNumber)
									{
										ushort* ptr2 = (ushort*)creditCardAccountNumberPtr;
										try
										{
											fixed (char* creditCardCCVNumberPtr = creditCard.CCVNumber)
											{
												ushort* ptr3 = (ushort*)creditCardCCVNumberPtr;
												try
												{
													fixed (char* creditCardLocalePtr = creditCard.Locale)
													{
														ushort* ptr4 = (ushort*)creditCardLocalePtr;
														try
														{
															fixed (char* creditCardPhoneNumberPtr = creditCard.PhoneNumber)
															{
																ushort* ptr5 = (ushort*)creditCardPhoneNumberPtr;
																try
																{
																	fixed (char* creditCardEmailPtr = creditCard.Email)
																	{
																		ushort* ptr6 = (ushort*)creditCardEmailPtr;
																		try
																		{
																			fixed (char* creditCardContactFirstNamePtr = creditCard.ContactFirstName)
																			{
																				ushort* ptr7 = (ushort*)creditCardContactFirstNamePtr;
																				try
																				{
																					fixed (char* creditCardContactLastNamePtr = creditCard.ContactLastName)
																					{
																						ushort* ptr8 = (ushort*)creditCardContactLastNamePtr;
																						try
																						{
																							long num2 = *(long*)(*(ulong*)(cComPtrNtv_003CICreditCard_003E.p)) + 24;
																							long num3 = *(long*)(cComPtrNtv_003CICreditCard_003E.p);
																							long num4 = *(long*)(cComPtrNtv_003CIAddress_003E.p);
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
								long num5 = *(long*)(cComPtrNtv_003CICreditCard_003E.p);
								bool parentCreditCard = creditCard.ParentCreditCard;
								((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte, int>)(*(ulong*)(*(long*)(*(ulong*)(cComPtrNtv_003CICreditCard_003E.p)) + 40)))((nint)num5, parentCreditCard ? ((byte)1) : ((byte)0));
								if (ppCreditCard != null)
								{
									ICreditCard* ptr9 = (ICreditCard*)(*(ulong*)(cComPtrNtv_003CICreditCard_003E.p));
									*(long*)(cComPtrNtv_003CICreditCard_003E.p) = 0L;
									*(long*)ppCreditCard = (nint)ptr9;
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
			finally
			{
				cComPtrNtv_003CICreditCard_003E.Dispose();
			}
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
			CComPtrNtv<INewsletterSettings> cComPtrNtv_003CINewsletterSettings_003E = new();
			try
			{
				if (num >= 0)
				{
					IAccountManagement* p = m_spAccountManagement.p;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, INewsletterSettings**, int>)(*(ulong*)(*(long*)p + 48)))((nint)p, (INewsletterSettings**)(cComPtrNtv_003CINewsletterSettings_003E.p));
					if (num >= 0)
					{
						NewsletterOptions newsletterOptions = new()
						{
							emailFormat = accountSettings.EmailFormat,
							allowZuneEmails = accountSettings.AllowZuneEmails ? 1 : 0,
							allowPartnerEmails = accountSettings.AllowPartnerEmails ? 1 : 0
						};
						long num2 = *(long*)(cComPtrNtv_003CINewsletterSettings_003E.p);
						NewsletterOptions newsletterOptions2 = newsletterOptions;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, NewsletterOptions, int>)(*(ulong*)(*(long*)(*(ulong*)(cComPtrNtv_003CINewsletterSettings_003E.p)) + 24)))((nint)num2, newsletterOptions2);
					}
				}
				CComPtrNtv<IPrivacySettings> cComPtrNtv_003CIPrivacySettings_003E = new();
				try
				{
					if (num >= 0)
					{
						if (accountSettings.PrivacySettings.Count > 0)
						{
							IAccountManagement* p2 = m_spAccountManagement.p;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IPrivacySettings**, int>)(*(ulong*)(*(long*)p2 + 56)))((nint)p2, cComPtrNtv_003CIPrivacySettings_003E.GetPtrToPtr());
						}
						if (num >= 0)
						{
							if (*(long*)(cComPtrNtv_003CIPrivacySettings_003E.p) != 0L)
							{
								foreach (KeyValuePair<PrivacySettingId, PrivacySettingValue> privacySetting in accountSettings.PrivacySettings)
								{
									EPrivacySettingId key = (EPrivacySettingId)privacySetting.Key;
									EPrivacySettingValue value = (EPrivacySettingValue)privacySetting.Value;
									long num3 = *(long*)(cComPtrNtv_003CIPrivacySettings_003E.p);
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EPrivacySettingId, EPrivacySettingValue, int>)(*(ulong*)(*(long*)(*(ulong*)(cComPtrNtv_003CIPrivacySettings_003E.p)) + 24)))((nint)num3, key, value);
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
									INewsletterSettings* ptr = (INewsletterSettings*)(*(ulong*)(cComPtrNtv_003CINewsletterSettings_003E.p));
									*(long*)(cComPtrNtv_003CINewsletterSettings_003E.p) = 0L;
									*(long*)ppNewsletterSettings = (nint)ptr;
								}
								if (ppPrivacySettings != null)
								{
									if (*(long*)(cComPtrNtv_003CIPrivacySettings_003E.p) != 0L)
									{
										IPrivacySettings* ptr2 = (IPrivacySettings*)(*(ulong*)(cComPtrNtv_003CIPrivacySettings_003E.p));
										*(long*)(cComPtrNtv_003CIPrivacySettings_003E.p) = 0L;
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
				finally
				{
					cComPtrNtv_003CIPrivacySettings_003E.Dispose();
				}
			}
			finally
			{
				cComPtrNtv_003CINewsletterSettings_003E.Dispose();
			}
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
			CComPtrNtv<IAccountUser> cComPtrNtv_003CIAccountUser_003E = new();
			try
			{
				if (num >= 0)
				{
					IAccountManagement* p = m_spAccountManagement.p;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IAccountUser**, int>)(*(ulong*)(*(long*)p + 64)))((nint)p, (IAccountUser**)(cComPtrNtv_003CIAccountUser_003E.p));
				}
				CComPtrNtv<IAddress> cComPtrNtv_003CIAddress_003E = new();
				try
				{
					if (num >= 0 && accountUser.Address != null)
					{
						num = CreateAddress(accountUser.Address, cComPtrNtv_003CIAddress_003E.GetPtrToPtr());
					}
					CComPtrNtv<INewsletterSettings> cComPtrNtv_003CINewsletterSettings_003E = new();
					try
					{
						CComPtrNtv<IPrivacySettings> cComPtrNtv_003CIPrivacySettings_003E = new();
						try
						{
							if (num >= 0 && accountUser.AccountSettings != null)
							{
								num = CreateAccountSettings(accountUser.AccountSettings, cComPtrNtv_003CINewsletterSettings_003E.GetPtrToPtr(), (IPrivacySettings**)(cComPtrNtv_003CIPrivacySettings_003E.p));
							}
							CComPtrNtv<ICreditCard> cComPtrNtv_003CICreditCard_003E = new();
							try
							{
								if (num >= 0 && accountUser.ParentCreditCard != null)
								{
									num = CreateCreditCard(accountUser.ParentCreditCard, cComPtrNtv_003CICreditCard_003E.GetPtrToPtr());
								}
								CComPtrNtv<IPassportIdentity> cComPtrNtv_003CIPassportIdentity_003E = new();
								try
								{
									if (num >= 0)
									{
										if (accountUser.ParentPassportIdentity != null)
										{
											num = accountUser.ParentPassportIdentity.GetComPointer(cComPtrNtv_003CIPassportIdentity_003E.GetPtrToPtr());
										}
										if (num >= 0)
										{
											bool flag = accountUser.Birthday != DateTime.MinValue;
											_SYSTEMTIME sYSTEMTIME;
											if (flag)
											{
												sYSTEMTIME = Module.DateTimeToSystemTime(accountUser.Birthday);
											}
											fixed (char* accountUserZuneTagPtr = accountUser.ZuneTag)
											{
												ushort* ptr2 = (ushort*)accountUserZuneTagPtr;
												try
												{
													fixed (char* accountUserLocalePtr = accountUser.Locale)
													{
														ushort* ptr3 = (ushort*)accountUserLocalePtr;
														try
														{
															fixed (char* accountUserFirstNamePtr = accountUser.FirstName)
															{
																ushort* ptr4 = (ushort*)accountUserFirstNamePtr;
																try
																{
																	fixed (char* accountUserLastNamePtr = accountUser.LastName)
																	{
																		ushort* ptr5 = (ushort*)accountUserLastNamePtr;
																		try
																		{
																			fixed (char* accountUserEmailPtr = accountUser.Email)
																			{
																				ushort* ptr6 = (ushort*)accountUserEmailPtr;
																				try
																				{
																					fixed (char* accountUserPhoneNumberPtr = accountUser.PhoneNumber)
																					{
																						ushort* ptr7 = (ushort*)accountUserPhoneNumberPtr;
																						try
																						{
																							fixed (char* accountUserMobilePhoneNumberPtr = accountUser.MobilePhoneNumber)
																							{
																								ushort* ptr8 = (ushort*)accountUserMobilePhoneNumberPtr;
																								try
																								{
																									_SYSTEMTIME* ptr = flag ? &sYSTEMTIME : null;
																									long num2 = *(long*)(*(ulong*)(cComPtrNtv_003CIAccountUser_003E.p)) + 24;
																									long num3 = *(long*)(cComPtrNtv_003CIAccountUser_003E.p);
																									long num4 = *(long*)(cComPtrNtv_003CIAddress_003E.p);
																									long num5 = *(long*)(cComPtrNtv_003CINewsletterSettings_003E.p);
																									long num6 = *(long*)(cComPtrNtv_003CIPrivacySettings_003E.p);
																									long num7 = *(long*)(cComPtrNtv_003CIPassportIdentity_003E.p);
																									long num8 = *(long*)(cComPtrNtv_003CICreditCard_003E.p);
																									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, _SYSTEMTIME*, ushort*, ushort*, ushort*, ushort*, ushort*, IAddress*, INewsletterSettings*, IPrivacySettings*, IPassportIdentity*, ICreditCard*, int>)(*(ulong*)num2))((nint)num3, (ushort*)(nint)ptr2, (ushort*)(nint)ptr3, ptr, (ushort*)(nint)ptr4, (ushort*)(nint)ptr5, (ushort*)(nint)ptr6, (ushort*)(nint)ptr7, (ushort*)(nint)ptr8, (IAddress*)num4, (INewsletterSettings*)num5, (IPrivacySettings*)num6, (IPassportIdentity*)num7, (ICreditCard*)num8);
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
												IAccountUser* ptr9 = (IAccountUser*)(*(ulong*)(cComPtrNtv_003CIAccountUser_003E.p));
												*(long*)(cComPtrNtv_003CIAccountUser_003E.p) = 0L;
												*(long*)ppAccountUser = (nint)ptr9;
											}
										}
									}
								}
								finally
								{
									cComPtrNtv_003CIPassportIdentity_003E.Dispose();
								}
							}
							finally
							{
								cComPtrNtv_003CICreditCard_003E.Dispose();
							}
						}
						finally
						{
							cComPtrNtv_003CIPrivacySettings_003E.Dispose();
						}
					}
					finally
					{
						cComPtrNtv_003CINewsletterSettings_003E.Dispose();
					}
				}
				finally
				{
					cComPtrNtv_003CIAddress_003E.Dispose();
				}
			}
			finally
			{
				cComPtrNtv_003CIAccountUser_003E.Dispose();
			}
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
				CComPtrNtv<IService> cComPtrNtv_003CIService_003E = new();
				try
				{
					num = Module.GetSingleton(Module.GUID_IService, (void**)(cComPtrNtv_003CIService_003E.GetPtrToPtr()));
					CComPtrNtv<IUnknown> cComPtrNtv_003CIUnknown_003E = new();
					try
					{
						if (num >= 0)
						{
							long num2 = *(long*)(cComPtrNtv_003CIService_003E.p);
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IUnknown**, int>)(*(ulong*)(*(long*)(*(ulong*)(cComPtrNtv_003CIService_003E.p)) + 104)))((nint)num2, cComPtrNtv_003CIUnknown_003E.GetPtrToPtr());
						}
						CComPtrNtv<IAccountManagement> cComPtrNtv_003CIAccountManagement_003E = new();
						try
						{
							if (num >= 0)
							{
								num = Module.CComPtrNtv_003CIUnknown_003E_002EQueryInterface_003Cstruct_0020IAccountManagement_003E(cComPtrNtv_003CIUnknown_003E.p, cComPtrNtv_003CIAccountManagement_003E.GetPtrToPtr());
								if (num >= 0)
								{
									long num3 = *(long*)(cComPtrNtv_003CIAccountManagement_003E.p);
									long num4 = *(long*)(cComPtrNtv_003CIService_003E.p);
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IService*, int>)(*(ulong*)(*(long*)(*(ulong*)(cComPtrNtv_003CIAccountManagement_003E.p)) + 24)))((nint)num3, (IService*)num4);
									if (num >= 0)
									{
										m_spAccountManagement.op_Assign((IAccountManagement*)(*(ulong*)(cComPtrNtv_003CIAccountManagement_003E.p)));
									}
								}
							}
						}
						finally
						{
							cComPtrNtv_003CIAccountManagement_003E.Dispose();
						}
					}
					finally
					{
						cComPtrNtv_003CIUnknown_003E.Dispose();
					}
				}
				finally
				{
					cComPtrNtv_003CIService_003E.Dispose();
				}
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
				m_spAccountManagement.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
