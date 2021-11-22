using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MicrosoftZuneLibrary;

namespace Microsoft.Zune.Messaging
{
	public class MessagingService : IDisposable
	{
		private static MessagingService m_singletonInstance = null;

		private unsafe IZuneNetMessaging* m_pMessaging = null;

		public static bool HasInstance
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((m_singletonInstance != null) ? 1u : 0u) != 0;
			}
		}

		public static MessagingService Instance
		{
			get
			{
				if (m_singletonInstance == null)
				{
					m_singletonInstance = new MessagingService();
				}
				return m_singletonInstance;
			}
		}

		private unsafe MessagingService()
		{
			//IL_0008: Expected I, but got I8
			//IL_0011: Expected I, but got I8
			IZuneNetMessaging* pMessaging = null;
			if (Module.GetSingleton(Module.GUID_IZuneNetMessaging, (void**)(&pMessaging)) >= 0)
			{
				m_pMessaging = pMessaging;
			}
		}

		private void _007EMessagingService()
		{
			_0021MessagingService();
		}

		private unsafe void _0021MessagingService()
		{
			//IL_0017: Expected I, but got I8
			//IL_0020: Expected I, but got I8
			IZuneNetMessaging* pMessaging = m_pMessaging;
			if (pMessaging != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pMessaging + 16)))((nint)pMessaging);
				m_pMessaging = null;
			}
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool MessageSetRead(string strMessageUrl)
		{
			//IL_002b: Expected I, but got I8
			bool result = false;
			if (m_pMessaging != null)
			{
				fixed (char* strMessageUrlPtr = strMessageUrl.ToCharArray())
				{
					ushort* ptr = (ushort*)strMessageUrlPtr;
					try
					{
						long num = *(long*)m_pMessaging + 24;
						IZuneNetMessaging* pMessaging = m_pMessaging;
						result = (byte)((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)num))((nint)pMessaging, ptr) >= 0) ? 1u : 0u) != 0;
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
				}
			}
			return result;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool MessageDelete(string strMessageUrl)
		{
			//IL_002b: Expected I, but got I8
			bool result = false;
			if (m_pMessaging != null)
			{
				fixed (char* strMessageUrlPtr = strMessageUrl.ToCharArray())
				{
					ushort* ptr = (ushort*)strMessageUrlPtr;
					try
					{
						long num = *(long*)m_pMessaging + 32;
						IZuneNetMessaging* pMessaging = m_pMessaging;
						result = (byte)((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)num))((nint)pMessaging, ptr) >= 0) ? 1u : 0u) != 0;
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
				}
			}
			return result;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool AcceptFriend(string strPostUrl)
		{
			//IL_002b: Expected I, but got I8
			bool result = false;
			if (m_pMessaging != null)
			{
				fixed (char* strPostUrlPtr = strPostUrl.ToCharArray())
				{
					ushort* ptr = (ushort*)strPostUrlPtr;
					try
					{
						long num = *(long*)m_pMessaging + 40;
						IZuneNetMessaging* pMessaging = m_pMessaging;
						result = (byte)((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)num))((nint)pMessaging, ptr) >= 0) ? 1u : 0u) != 0;
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
				}
			}
			return result;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool RejectFriend(string strPostUrl)
		{
			//IL_002b: Expected I, but got I8
			bool result = false;
			if (m_pMessaging != null)
			{
				fixed (char* strPostUrlPtr = strPostUrl.ToCharArray())
				{
					ushort* ptr = (ushort*)strPostUrlPtr;
					try
					{
						long num = *(long*)m_pMessaging + 48;
						IZuneNetMessaging* pMessaging = m_pMessaging;
						result = (byte)((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)num))((nint)pMessaging, ptr) >= 0) ? 1u : 0u) != 0;
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
				}
			}
			return result;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool ManageFriend(FriendAction eAction, string strPostUrl, string strZuneTag)
		{
			//IL_0034: Expected I, but got I8
			bool result = false;
			if (m_pMessaging != null)
			{
				fixed (char* strPostUrlPtr = strPostUrl.ToCharArray())
				{
					ushort* ptr = (ushort*)strPostUrlPtr;
					try
					{
						fixed (char* strZuneTagPtr = strZuneTag.ToCharArray())
						{
							ushort* ptr2 = (ushort*)strZuneTagPtr;
							try
							{
								long num = *(long*)m_pMessaging + 56;
								IZuneNetMessaging* pMessaging = m_pMessaging;
								result = (byte)((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EFriendAction, ushort*, ushort*, int>)(*(ulong*)num))((nint)pMessaging, (EFriendAction)eAction, ptr, ptr2) >= 0) ? 1u : 0u) != 0;
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
			return result;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool AddComment(string strPostUrl, string strZuneTag, string strMessage, CommentCallback callback)
		{
			//IL_0061: Expected I, but got I8
			//IL_0061: Expected I, but got I8
			bool result = false;
			if (m_pMessaging != null)
			{
				CComPtrNtv_003CIMessagingCallback_003E cComPtrNtv_003CIMessagingCallback_003E;
				*(long*)(&cComPtrNtv_003CIMessagingCallback_003E) = 0L;
				try
				{
					int num;
					if (callback != null)
					{
						num = Module.Microsoft_002EZune_002EMessaging_002EAddCommentCallbackWrapper_002ECreateInstance(callback, (IMessagingCallback**)(&cComPtrNtv_003CIMessagingCallback_003E));
						if (num < 0)
						{
							goto IL_0086;
						}
					}
					fixed (char* strPostUrlPtr = strPostUrl.ToCharArray())
					{
						ushort* ptr = (ushort*)strPostUrlPtr;
						try
						{
							fixed (char* strZuneTagPtr = strZuneTag.ToCharArray())
							{
								ushort* ptr2 = (ushort*)strZuneTagPtr;
								try
								{
									fixed (char* strMessagePtr = strMessage.ToCharArray())
									{
										ushort* ptr3 = (ushort*)strMessagePtr;
										try
										{
											long num2 = *(long*)m_pMessaging + 112;
											IZuneNetMessaging* pMessaging = m_pMessaging;
											_003F val = ptr;
											_003F val2 = ptr2;
											_003F val3 = ptr3;
											long num3 = *(long*)(&cComPtrNtv_003CIMessagingCallback_003E);
											num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, ushort*, IMessagingCallback*, int>)(*(ulong*)num2))((nint)pMessaging, (ushort*)(nint)val, (ushort*)(nint)val2, (ushort*)(nint)val3, (IMessagingCallback*)num3);
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
					if (num < 0)
					{
						goto IL_0086;
					}
					int num4 = 1;
					goto IL_0089;
					IL_0089:
					result = (byte)num4 != 0;
					goto end_IL_0013;
					IL_0086:
					num4 = 0;
					goto IL_0089;
					end_IL_0013:;
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIMessagingCallback_003E*, void>)(&Module.CComPtrNtv_003CIMessagingCallback_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIMessagingCallback_003E);
					throw;
				}
				Module.CComPtrNtv_003CIMessagingCallback_003E_002ERelease(&cComPtrNtv_003CIMessagingCallback_003E);
			}
			return result;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool DeleteComment(string strPostUrl, string strZuneTag, MessagingCallback callback)
		{
			//IL_0058: Expected I, but got I8
			//IL_0058: Expected I, but got I8
			bool result = false;
			if (m_pMessaging != null)
			{
				CComPtrNtv_003CIMessagingCallback_003E cComPtrNtv_003CIMessagingCallback_003E;
				*(long*)(&cComPtrNtv_003CIMessagingCallback_003E) = 0L;
				try
				{
					int num;
					if (callback != null)
					{
						num = Module.Microsoft_002EZune_002EMessaging_002EMessagingCallbackWrapper_002ECreateInstance(callback, null, (IMessagingCallback**)(&cComPtrNtv_003CIMessagingCallback_003E));
						if (num < 0)
						{
							goto IL_0073;
						}
					}
					fixed (char* strPostUrlPtr = strPostUrl.ToCharArray())
					{
						ushort* ptr = (ushort*)strPostUrlPtr;
						try
						{
							fixed (char* strZuneTagPtr = strZuneTag.ToCharArray())
							{
								ushort* ptr2 = (ushort*)strZuneTagPtr;
								try
								{
									long num2 = *(long*)m_pMessaging + 120;
									IZuneNetMessaging* pMessaging = m_pMessaging;
									_003F val = ptr;
									_003F val2 = ptr2;
									long num3 = *(long*)(&cComPtrNtv_003CIMessagingCallback_003E);
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, IMessagingCallback*, int>)(*(ulong*)num2))((nint)pMessaging, (ushort*)(nint)val, (ushort*)(nint)val2, (IMessagingCallback*)num3);
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
					if (num < 0)
					{
						goto IL_0073;
					}
					int num4 = 1;
					goto IL_0075;
					IL_0075:
					result = (byte)num4 != 0;
					goto end_IL_0013;
					IL_0073:
					num4 = 0;
					goto IL_0075;
					end_IL_0013:;
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIMessagingCallback_003E*, void>)(&Module.CComPtrNtv_003CIMessagingCallback_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIMessagingCallback_003E);
					throw;
				}
				Module.CComPtrNtv_003CIMessagingCallback_003E_002ERelease(&cComPtrNtv_003CIMessagingCallback_003E);
			}
			return result;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool Compose(string strPostUrl, string strMessage, string strRecipientZuneTags, string strRequestType, IPropertySetMessageData messageData, MessagingCallback callback, object state)
		{
			//IL_0019: Expected I, but got I8
			//IL_0027: Expected I, but got I8
			//IL_008a: Expected I, but got I8
			//IL_00d4: Expected I, but got I8
			bool result = false;
			IMessagingCallback* ptr2;
			int num3;
			if (messageData != null && m_pMessaging != null)
			{
				IMSMediaSchemaPropertySet* ptr = null;
				int num = messageData.GetPropertySet(&ptr);
				ptr2 = null;
				if (num >= 0)
				{
					if (callback != null)
					{
						num = Module.Microsoft_002EZune_002EMessaging_002EMessagingCallbackWrapper_002ECreateInstance(callback, state, &ptr2);
					}
					if (num >= 0)
					{
						fixed (char* strPostUrlPtr = strPostUrl.ToCharArray())
						{
							ushort* ptr3 = (ushort*)strPostUrlPtr;
							try
							{
								fixed (char* strMessagePtr = strMessage.ToCharArray())
								{
									ushort* ptr4 = (ushort*)strMessagePtr;
									try
									{
										fixed (char* strRecipientZuneTagsPtr = strRecipientZuneTags.ToCharArray())
										{
											ushort* ptr5 = (ushort*)strRecipientZuneTagsPtr;
											try
											{
												fixed (char* strRequestTypePtr = strRequestType.ToCharArray())
												{
													ushort* ptr6 = (ushort*)strRequestTypePtr;
													try
													{
														long num2 = *(long*)m_pMessaging + 64;
														IZuneNetMessaging* pMessaging = m_pMessaging;
														_003F val = ptr3;
														_003F val2 = ptr4;
														_003F val3 = ptr5;
														_003F val4 = ptr6;
														IMSMediaSchemaPropertySet* intPtr = ptr;
														IMessagingCallback* intPtr2 = ptr2;
														num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, ushort*, ushort*, IMSMediaSchemaPropertySet*, IMessagingCallback*, int>)(*(ulong*)num2))((nint)pMessaging, (ushort*)(nint)val, (ushort*)(nint)val2, (ushort*)(nint)val3, (ushort*)(nint)val4, intPtr, intPtr2);
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
						if (num >= 0)
						{
							num3 = 1;
							goto IL_00bd;
						}
					}
				}
				num3 = 0;
				goto IL_00bd;
			}
			goto IL_00d5;
			IL_00bd:
			result = (byte)num3 != 0;
			if (ptr2 != null)
			{
				IMessagingCallback* intPtr3 = ptr2;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr3 + 16)))((nint)intPtr3);
			}
			goto IL_00d5;
			IL_00d5:
			return result;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool Compose(string strPostUrl, string strMessage, MessagingCallback callback, object state)
		{
			//IL_0059: Expected I, but got I8
			//IL_0059: Expected I, but got I8
			bool result = false;
			if (m_pMessaging != null)
			{
				CComPtrNtv_003CIMessagingCallback_003E cComPtrNtv_003CIMessagingCallback_003E;
				*(long*)(&cComPtrNtv_003CIMessagingCallback_003E) = 0L;
				try
				{
					int num;
					if (callback != null)
					{
						num = Module.Microsoft_002EZune_002EMessaging_002EMessagingCallbackWrapper_002ECreateInstance(callback, state, (IMessagingCallback**)(&cComPtrNtv_003CIMessagingCallback_003E));
						if (num < 0)
						{
							goto IL_0074;
						}
					}
					fixed (char* strPostUrlPtr = strPostUrl.ToCharArray())
					{
						ushort* ptr = (ushort*)strPostUrlPtr;
						try
						{
							fixed (char* strMessagePtr = strMessage.ToCharArray())
							{
								ushort* ptr2 = (ushort*)strMessagePtr;
								try
								{
									long num2 = *(long*)m_pMessaging + 72;
									IZuneNetMessaging* pMessaging = m_pMessaging;
									_003F val = ptr;
									_003F val2 = ptr2;
									long num3 = *(long*)(&cComPtrNtv_003CIMessagingCallback_003E);
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, IMessagingCallback*, int>)(*(ulong*)num2))((nint)pMessaging, (ushort*)(nint)val, (ushort*)(nint)val2, (IMessagingCallback*)num3);
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
					if (num < 0)
					{
						goto IL_0074;
					}
					int num4 = 1;
					goto IL_0076;
					IL_0076:
					result = (byte)num4 != 0;
					goto end_IL_0013;
					IL_0074:
					num4 = 0;
					goto IL_0076;
					end_IL_0013:;
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIMessagingCallback_003E*, void>)(&Module.CComPtrNtv_003CIMessagingCallback_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIMessagingCallback_003E);
					throw;
				}
				Module.CComPtrNtv_003CIMessagingCallback_003E_002ERelease(&cComPtrNtv_003CIMessagingCallback_003E);
			}
			return result;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool ManageFavorites(FavoritesAction eAction, string strFavoritesUrl, string strInstructions, MessagingCallback callback, object state)
		{
			//IL_0011: Expected I, but got I8
			//IL_0058: Expected I, but got I8
			//IL_008c: Expected I, but got I8
			bool result = false;
			IMessagingCallback* ptr;
			int num3;
			if (m_pMessaging != null)
			{
				ptr = null;
				int num;
				if (callback != null)
				{
					num = Module.Microsoft_002EZune_002EMessaging_002EMessagingCallbackWrapper_002ECreateInstance(callback, state, &ptr);
					if (num < 0)
					{
						goto IL_0074;
					}
				}
				fixed (char* strFavoritesUrlPtr = strFavoritesUrl.ToCharArray())
				{
					ushort* ptr2 = (ushort*)strFavoritesUrlPtr;
					try
					{
						fixed (char* strInstructionsPtr = strInstructions.ToCharArray())
						{
							ushort* ptr3 = (ushort*)strInstructionsPtr;
							try
							{
								long num2 = *(long*)m_pMessaging + 80;
								IZuneNetMessaging* pMessaging = m_pMessaging;
								_003F val = ptr2;
								_003F val2 = ptr3;
								IMessagingCallback* intPtr = ptr;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EFavoritesAction, ushort*, ushort*, IMessagingCallback*, int>)(*(ulong*)num2))((nint)pMessaging, (EFavoritesAction)eAction, (ushort*)(nint)val, (ushort*)(nint)val2, intPtr);
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
				if (num < 0)
				{
					goto IL_0074;
				}
				num3 = 1;
				goto IL_0077;
			}
			goto IL_008d;
			IL_0077:
			result = (byte)num3 != 0;
			if (ptr != null)
			{
				IMessagingCallback* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
			goto IL_008d;
			IL_0074:
			num3 = 0;
			goto IL_0077;
			IL_008d:
			return result;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool ManageProfile(string strProfileUrl, string strFieldValue, MessagingCallback callback, object state)
		{
			//IL_0011: Expected I, but got I8
			//IL_0055: Expected I, but got I8
			//IL_0089: Expected I, but got I8
			bool result = false;
			IMessagingCallback* ptr;
			int num3;
			if (m_pMessaging != null)
			{
				ptr = null;
				int num;
				if (callback != null)
				{
					num = Module.Microsoft_002EZune_002EMessaging_002EMessagingCallbackWrapper_002ECreateInstance(callback, state, &ptr);
					if (num < 0)
					{
						goto IL_0071;
					}
				}
				fixed (char* strProfileUrlPtr = strProfileUrl.ToCharArray())
				{
					ushort* ptr2 = (ushort*)strProfileUrlPtr;
					try
					{
						fixed (char* strFieldValuePtr = strFieldValue.ToCharArray())
						{
							ushort* ptr3 = (ushort*)strFieldValuePtr;
							try
							{
								long num2 = *(long*)m_pMessaging + 88;
								IZuneNetMessaging* pMessaging = m_pMessaging;
								_003F val = ptr2;
								_003F val2 = ptr3;
								IMessagingCallback* intPtr = ptr;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, IMessagingCallback*, int>)(*(ulong*)num2))((nint)pMessaging, (ushort*)(nint)val, (ushort*)(nint)val2, intPtr);
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
				if (num < 0)
				{
					goto IL_0071;
				}
				num3 = 1;
				goto IL_0074;
			}
			goto IL_008a;
			IL_0074:
			result = (byte)num3 != 0;
			if (ptr != null)
			{
				IMessagingCallback* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
			goto IL_008a;
			IL_0071:
			num3 = 0;
			goto IL_0074;
			IL_008a:
			return result;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool ManageProfileImage(string strProfileImageUrl, string strProfileImageResource, MessagingCallback callback, object state)
		{
			//IL_0071: Expected I, but got I8
			//IL_0071: Expected I, but got I8
			bool result = false;
			if (m_pMessaging != null && strProfileImageResource != null && strProfileImageUrl != null)
			{
				CComPtrNtv_003CIMessagingCallback_003E cComPtrNtv_003CIMessagingCallback_003E;
				*(long*)(&cComPtrNtv_003CIMessagingCallback_003E) = 0L;
				try
				{
					int num;
					if (callback != null)
					{
						num = Module.Microsoft_002EZune_002EMessaging_002EMessagingCallbackWrapper_002ECreateInstance(callback, state, (IMessagingCallback**)(&cComPtrNtv_003CIMessagingCallback_003E));
						if (num < 0)
						{
							goto IL_008c;
						}
					}
					fixed (char* strProfileImageUrlPtr = strProfileImageUrl.ToCharArray())
					{
						ushort* ptr = (ushort*)strProfileImageUrlPtr;
						try
						{
							fixed (char* strProfileImageResourcePtr = strProfileImageResource.ToCharArray())
							{
								ushort* ptr2 = (ushort*)strProfileImageResourcePtr;
								try
								{
									long num2 = *(long*)m_pMessaging + 96;
									IZuneNetMessaging* pMessaging = m_pMessaging;
									_003F val = ptr;
									_003F val2 = ptr2;
									long num3 = *(long*)(&cComPtrNtv_003CIMessagingCallback_003E);
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, IMessagingCallback*, int>)(*(ulong*)num2))((nint)pMessaging, (ushort*)(nint)val, (ushort*)(nint)val2, (IMessagingCallback*)num3);
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
					if (num < 0)
					{
						goto IL_008c;
					}
					int num4 = 1;
					goto IL_008e;
					IL_008e:
					result = (byte)num4 != 0;
					goto end_IL_002b;
					IL_008c:
					num4 = 0;
					goto IL_008e;
					end_IL_002b:;
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIMessagingCallback_003E*, void>)(&Module.CComPtrNtv_003CIMessagingCallback_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIMessagingCallback_003E);
					throw;
				}
				Module.CComPtrNtv_003CIMessagingCallback_003E_002ERelease(&cComPtrNtv_003CIMessagingCallback_003E);
			}
			return result;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool ManageProfileImage(string strProfileImageUrl, SafeBitmap profileImage, MessagingCallback callback, object state)
		{
			//IL_006f: Expected I, but got I8
			//IL_0090: Expected I, but got I8
			//IL_0090: Expected I, but got I8
			bool result = false;
			if (m_pMessaging != null && profileImage != null && strProfileImageUrl != null)
			{
				int num = 0;
				CComPtrNtv_003CIMessagingCallback_003E cComPtrNtv_003CIMessagingCallback_003E;
				*(long*)(&cComPtrNtv_003CIMessagingCallback_003E) = 0L;
				try
				{
					if (callback != null)
					{
						num = Module.Microsoft_002EZune_002EMessaging_002EMessagingCallbackWrapper_002ECreateInstance(callback, state, (IMessagingCallback**)(&cComPtrNtv_003CIMessagingCallback_003E));
					}
					bool success = false;
					int num4;
					if (num >= 0)
					{
						profileImage.DangerousAddRef(ref success);
						num = ((!success) ? (-2147467259) : num);
						if (num >= 0)
						{
							fixed (char* strProfileImageUrlPtr = strProfileImageUrl.ToCharArray())
							{
								ushort* ptr2 = (ushort*)strProfileImageUrlPtr;
								try
								{
									HBITMAP* ptr = (HBITMAP*)(int)profileImage.DangerousGetHandle();
									long num2 = *(long*)m_pMessaging + 104;
									IZuneNetMessaging* pMessaging = m_pMessaging;
									_003F val = ptr2;
									long num3 = *(long*)(&cComPtrNtv_003CIMessagingCallback_003E);
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, HBITMAP*, IMessagingCallback*, int>)(*(ulong*)num2))((nint)pMessaging, (ushort*)(nint)val, ptr, (IMessagingCallback*)num3);
								}
								catch
								{
									//try-fault
									ptr2 = null;
									throw;
								}
							}
						}
						if (success)
						{
							profileImage.DangerousRelease();
						}
						if (num >= 0)
						{
							num4 = 1;
							goto IL_00ad;
						}
					}
					num4 = 0;
					goto IL_00ad;
					IL_00ad:
					result = (byte)num4 != 0;
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIMessagingCallback_003E*, void>)(&Module.CComPtrNtv_003CIMessagingCallback_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIMessagingCallback_003E);
					throw;
				}
				Module.CComPtrNtv_003CIMessagingCallback_003E_002ERelease(&cComPtrNtv_003CIMessagingCallback_003E);
			}
			return result;
		}

		public unsafe string GetInboxPhotoUrl(string title, string collectionName)
		{
			//IL_000d: Expected I, but got I8
			//IL_0042: Expected I, but got I8
			string result = null;
			if (m_pMessaging != null)
			{
				ushort* ptr = null;
				int num = 0;
				fixed (char* titlePtr = title.ToCharArray())
				{
					ushort* ptr2 = (ushort*)titlePtr;
					try
					{
						fixed (char* collectionNamePtr = collectionName.ToCharArray())
						{
							ushort* ptr3 = (ushort*)collectionNamePtr;
							try
							{
								long num2 = *(long*)m_pMessaging + 152;
								IZuneNetMessaging* pMessaging = m_pMessaging;
								if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, ushort**, int*, int>)(*(ulong*)num2))((nint)pMessaging, ptr2, ptr3, &ptr, &num) >= 0 && num != 0)
								{
									result = new string((char*)ptr);
								}
								if (ptr != null)
								{
									Module.SysFreeString(ptr);
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
			return result;
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool AddInboxPhoto(string title, string collectionName, string localFilePath)
		{
			//IL_0040: Expected I, but got I8
			bool result = false;
			if (m_pMessaging != null)
			{
				fixed (char* titlePtr = title.ToCharArray())
				{
					ushort* ptr = (ushort*)titlePtr;
					try
					{
						fixed (char* collectionNamePtr = collectionName.ToCharArray())
						{
							ushort* ptr2 = (ushort*)collectionNamePtr;
							try
							{
								fixed (char* localFilePathPtr = localFilePath.ToCharArray())
								{
									ushort* ptr3 = (ushort*)localFilePathPtr;
									try
									{
										long num = *(long*)m_pMessaging + 144;
										IZuneNetMessaging* pMessaging = m_pMessaging;
										result = (byte)((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, ushort*, int>)(*(ulong*)num))((nint)pMessaging, ptr, ptr2, ptr3) >= 0) ? 1u : 0u) != 0;
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
			return result;
		}

		public unsafe int GetInboxDownloadFolderId(string collectionName)
		{
			//IL_0030: Expected I, but got I8
			int num = 0;
			if (m_pMessaging != null)
			{
				fixed (char* collectionNamePtr = collectionName.ToCharArray())
				{
					ushort* ptr = (ushort*)collectionNamePtr;
					try
					{
						long num2 = *(long*)m_pMessaging + 160;
						IZuneNetMessaging* pMessaging = m_pMessaging;
						num = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int*, int>)(*(ulong*)num2))((nint)pMessaging, ptr, &num) >= 0) ? num : 0);
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
				}
			}
			return num;
		}

		public unsafe void InitiateUploadDeviceCartItems()
		{
			//IL_001c: Expected I, but got I8
			//IL_001c: Expected I, but got I8
			IZuneNetMessaging* pMessaging = m_pMessaging;
			if (pMessaging != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IProgressNotify*, int>)(*(ulong*)(*(long*)pMessaging + 136)))((nint)pMessaging, null);
			}
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_0021MessagingService();
				return;
			}
			try
			{
				_0021MessagingService();
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

		~MessagingService()
		{
			Dispose(false);
		}
	}
}
