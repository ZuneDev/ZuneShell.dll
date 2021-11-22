using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ZuneUI;

namespace Microsoft.Zune.Service
{
	public class WinLiveSignup : IDisposable
	{
		private readonly CComPtrMgd_003CIWinLiveSignup_003E m_spWinLiveSignup;

		public WinLiveSignup()
		{
			CComPtrMgd_003CIWinLiveSignup_003E spWinLiveSignup = new CComPtrMgd_003CIWinLiveSignup_003E();
			try
			{
				m_spWinLiveSignup = spWinLiveSignup;
			}
			catch
			{
				//try-fault
				((IDisposable)m_spWinLiveSignup).Dispose();
				throw;
			}
		}

		public unsafe HRESULT GetInformation(string locale, EHipType hipType, out WinLiveInformation information, out ServiceError serviceError)
		{
			//IL_004c: Expected I, but got I8
			//IL_0074: Expected I, but got I8
			//IL_0086: Expected I, but got I8
			HRESULT result = CreateComObject();
			CComPtrNtv_003CIWinLiveInformation_003E cComPtrNtv_003CIWinLiveInformation_003E;
			*(long*)(&cComPtrNtv_003CIWinLiveInformation_003E) = 0L;
			try
			{
				CComPtrNtv_003CIServiceError_003E cComPtrNtv_003CIServiceError_003E;
				*(long*)(&cComPtrNtv_003CIServiceError_003E) = 0L;
				try
				{
					if ((byte)((result.hr >= 0) ? 1u : 0u) != 0)
					{
						fixed (char* localePtr = locale.ToCharArray())
						{
							ushort* ptr = (ushort*)localePtr;
							try
							{
								IWinLiveSignup* p = m_spWinLiveSignup.p;
								long num = *(long*)p + 32;
								result.hr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, global::EHipType, IWinLiveInformation**, IServiceError**, int>)(*(ulong*)num))((nint)p, ptr, (global::EHipType)hipType, (IWinLiveInformation**)(&cComPtrNtv_003CIWinLiveInformation_003E), (IServiceError**)(&cComPtrNtv_003CIServiceError_003E));
							}
							catch
							{
								//try-fault
								ptr = null;
								throw;
							}
						}
					}
					if ((byte)((result.hr >= 0) ? 1u : 0u) != 0)
					{
						information = new WinLiveInformation((IWinLiveInformation*)(*(ulong*)(&cComPtrNtv_003CIWinLiveInformation_003E)));
					}
					if (*(long*)(&cComPtrNtv_003CIServiceError_003E) != 0L)
					{
						serviceError = new ServiceError((IServiceError*)(*(ulong*)(&cComPtrNtv_003CIServiceError_003E)));
					}
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
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIWinLiveInformation_003E*, void>)(&Module.CComPtrNtv_003CIWinLiveInformation_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIWinLiveInformation_003E);
				throw;
			}
			Module.CComPtrNtv_003CIWinLiveInformation_003E_002ERelease(&cComPtrNtv_003CIWinLiveInformation_003E);
			return result;
		}

		public unsafe HRESULT CheckAvailableSigninName(string signinName, [MarshalAs(UnmanagedType.U1)] bool needSuggestedNames, string firstName, string lastName, out WinLiveAvailableInformation information, out ServiceError serviceError)
		{
			//IL_0060: Expected I, but got I8
			//IL_009b: Expected I, but got I8
			//IL_00ad: Expected I, but got I8
			HRESULT result = CreateComObject();
			CComPtrNtv_003CIWinLiveAvailableInformation_003E cComPtrNtv_003CIWinLiveAvailableInformation_003E;
			*(long*)(&cComPtrNtv_003CIWinLiveAvailableInformation_003E) = 0L;
			try
			{
				CComPtrNtv_003CIServiceError_003E cComPtrNtv_003CIServiceError_003E;
				*(long*)(&cComPtrNtv_003CIServiceError_003E) = 0L;
				try
				{
					if ((byte)((result.hr >= 0) ? 1u : 0u) != 0)
					{
						fixed (char* signinNamePtr = signinName.ToCharArray())
						{
							ushort* ptr = (ushort*)signinNamePtr;
							try
							{
								fixed (char* firstNamePtr = firstName.ToCharArray())
								{
									ushort* ptr2 = (ushort*)firstNamePtr;
									try
									{
										fixed (char* lastNamePtr = lastName.ToCharArray())
										{
											ushort* ptr3 = (ushort*)lastNamePtr;
											try
											{
												IWinLiveSignup* p = m_spWinLiveSignup.p;
												long num = *(long*)p + 40;
												result.hr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, byte, ushort*, ushort*, IWinLiveAvailableInformation**, IServiceError**, int>)(*(ulong*)num))((nint)p, ptr, needSuggestedNames ? ((byte)1) : ((byte)0), ptr2, ptr3, (IWinLiveAvailableInformation**)(&cComPtrNtv_003CIWinLiveAvailableInformation_003E), (IServiceError**)(&cComPtrNtv_003CIServiceError_003E));
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
					if ((byte)((result.hr >= 0) ? 1u : 0u) != 0)
					{
						information = new WinLiveAvailableInformation((IWinLiveAvailableInformation*)(*(ulong*)(&cComPtrNtv_003CIWinLiveAvailableInformation_003E)));
					}
					if (*(long*)(&cComPtrNtv_003CIServiceError_003E) != 0L)
					{
						serviceError = new ServiceError((IServiceError*)(*(ulong*)(&cComPtrNtv_003CIServiceError_003E)));
					}
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
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIWinLiveAvailableInformation_003E*, void>)(&Module.CComPtrNtv_003CIWinLiveAvailableInformation_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIWinLiveAvailableInformation_003E);
				throw;
			}
			Module.CComPtrNtv_003CIWinLiveAvailableInformation_003E_002ERelease(&cComPtrNtv_003CIWinLiveAvailableInformation_003E);
			return result;
		}

		public unsafe HRESULT CreateAccount(string signinName, string signinPassword, string secretQuestion, string secretAnswer, string countryCode, string hipChallenge, string hipSolution, DateTime birthday, int termsOfServiceVersion, int languagePreference, out ServiceError serviceError)
		{
			//IL_0091: Expected I, but got I8
			//IL_00ec: Expected I, but got I8
			HRESULT result = CreateComObject();
			CComPtrNtv_003CIServiceError_003E cComPtrNtv_003CIServiceError_003E;
			Module.CComPtrNtv_003CIServiceError_003E_002E_007Bctor_007D(&cComPtrNtv_003CIServiceError_003E);
			try
			{
				if (result.IsSuccess)
				{
					_SYSTEMTIME sYSTEMTIME = Module.DateTimeToSystemTime(birthday);
					fixed (char* signinNamePtr = signinName.ToCharArray())
					{
						ushort* ptr2 = (ushort*)signinNamePtr;
						try
						{
							fixed (char* signinPasswordPtr = signinPassword.ToCharArray())
							{
								ushort* ptr3 = (ushort*)signinPasswordPtr;
								try
								{
									fixed (char* secretQuestionPtr = secretQuestion.ToCharArray())
									{
										ushort* ptr4 = (ushort*)secretQuestionPtr;
										try
										{
											fixed (char* secretAnswerPtr = secretAnswer.ToCharArray())
											{
												ushort* ptr5 = (ushort*)secretAnswerPtr;
												try
												{
													fixed (char* countryCodePtr = countryCode.ToCharArray())
													{
														ushort* ptr6 = (ushort*)countryCodePtr;
														try
														{
															fixed (char* hipChallengePtr = hipChallenge.ToCharArray())
															{
																ushort* ptr7 = (ushort*)hipChallengePtr;
																try
																{
																	fixed (char* hipSolutionPtr = hipSolution.ToCharArray())
																	{
																		ushort* ptr8 = (ushort*)hipSolutionPtr;
																		try
																		{
																			IWinLiveSignup* ptr = m_spWinLiveSignup.op_MemberSelection();
																			long num = *(long*)ptr + 48;
																			result.hr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, ushort*, ushort*, ushort*, ushort*, ushort*, _SYSTEMTIME*, int, int, IServiceError**, int>)(*(ulong*)num))((nint)ptr, ptr2, ptr3, ptr4, ptr5, ptr6, ptr7, ptr8, &sYSTEMTIME, termsOfServiceVersion, languagePreference, (IServiceError**)(&cComPtrNtv_003CIServiceError_003E));
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
				if (*(long*)(&cComPtrNtv_003CIServiceError_003E) != 0L)
				{
					serviceError = new ServiceError((IServiceError*)(*(ulong*)(&cComPtrNtv_003CIServiceError_003E)));
				}
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIServiceError_003E*, void>)(&Module.CComPtrNtv_003CIServiceError_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIServiceError_003E);
				throw;
			}
			Module.CComPtrNtv_003CIServiceError_003E_002ERelease(&cComPtrNtv_003CIServiceError_003E);
			return result;
		}

		private unsafe HRESULT CreateComObject()
		{
			//IL_0045: Expected I, but got I8
			//IL_0045: Expected I, but got I8
			//IL_005b: Expected I, but got I8
			//IL_0073: Expected I, but got I8
			//IL_0073: Expected I, but got I8
			//IL_0088: Expected I, but got I8
			int num = 0;
			if ((long)(nint)m_spWinLiveSignup.p == 0)
			{
				CComPtrNtv_003CIService_003E cComPtrNtv_003CIService_003E;
				*(long*)(&cComPtrNtv_003CIService_003E) = 0L;
				try
				{
					num = Module.GetSingleton(Module.GUID_IService, (void**)(&cComPtrNtv_003CIService_003E));
					CComPtrNtv_003CIUnknown_003E cComPtrNtv_003CIUnknown_003E;
					*(long*)(&cComPtrNtv_003CIUnknown_003E) = 0L;
					try
					{
						if (num >= 0)
						{
							long num2 = *(long*)(&cComPtrNtv_003CIService_003E);
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IUnknown**, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIService_003E)) + 96)))((nint)num2, (IUnknown**)(&cComPtrNtv_003CIUnknown_003E));
						}
						CComPtrNtv_003CIWinLiveSignup_003E cComPtrNtv_003CIWinLiveSignup_003E;
						*(long*)(&cComPtrNtv_003CIWinLiveSignup_003E) = 0L;
						try
						{
							if (num >= 0)
							{
								num = Module.IUnknown_002EQueryInterface_003Cstruct_0020IWinLiveSignup_003E((IUnknown*)(*(ulong*)(&cComPtrNtv_003CIUnknown_003E)), (IWinLiveSignup**)(&cComPtrNtv_003CIWinLiveSignup_003E));
								if (num >= 0)
								{
									long num3 = *(long*)(&cComPtrNtv_003CIWinLiveSignup_003E);
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)(*(ulong*)(&cComPtrNtv_003CIWinLiveSignup_003E)) + 24)))((nint)num3);
									if (num >= 0)
									{
										m_spWinLiveSignup.op_Assign((IWinLiveSignup*)(*(ulong*)(&cComPtrNtv_003CIWinLiveSignup_003E)));
									}
								}
							}
						}
						catch
						{
							//try-fault
							Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIWinLiveSignup_003E*, void>)(&Module.CComPtrNtv_003CIWinLiveSignup_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIWinLiveSignup_003E);
							throw;
						}
						Module.CComPtrNtv_003CIWinLiveSignup_003E_002ERelease(&cComPtrNtv_003CIWinLiveSignup_003E);
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
			return new HRESULT(num);
		}

		public void _007EWinLiveSignup()
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
					((IDisposable)m_spWinLiveSignup).Dispose();
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
