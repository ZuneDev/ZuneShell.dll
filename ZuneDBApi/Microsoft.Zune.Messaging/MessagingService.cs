using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MicrosoftZuneLibrary;

namespace Microsoft.Zune.Messaging;

public class MessagingService : IDisposable
{
	private static MessagingService m_singletonInstance = null;

	private unsafe IZuneNetMessaging* m_pMessaging = null;

	public static bool HasInstance
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return m_singletonInstance != null;
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
		if (global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_bf368f0d_4743_439c_9142_e487c9534104, (void**)(&pMessaging)) >= 0)
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
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strMessageUrl)))
			{
				try
				{
					long num = *(long*)m_pMessaging + 24;
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)num))((nint)m_pMessaging, ptr) >= 0;
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
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strMessageUrl)))
			{
				try
				{
					long num = *(long*)m_pMessaging + 32;
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)num))((nint)m_pMessaging, ptr) >= 0;
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
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strPostUrl)))
			{
				try
				{
					long num = *(long*)m_pMessaging + 40;
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)num))((nint)m_pMessaging, ptr) >= 0;
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
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strPostUrl)))
			{
				try
				{
					long num = *(long*)m_pMessaging + 48;
					result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int>)(*(ulong*)num))((nint)m_pMessaging, ptr) >= 0;
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
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strPostUrl)))
			{
				try
				{
					fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strZuneTag)))
					{
						try
						{
							long num = *(long*)m_pMessaging + 56;
							result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EFriendAction, ushort*, ushort*, int>)(*(ulong*)num))((nint)m_pMessaging, (EFriendAction)eAction, ptr, ptr2) >= 0;
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
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIMessagingCallback_003E cComPtrNtv_003CIMessagingCallback_003E);
			*(long*)(&cComPtrNtv_003CIMessagingCallback_003E) = 0L;
			try
			{
				int num;
				if (callback != null)
				{
					num = global::_003CModule_003E.Microsoft_002EZune_002EMessaging_002EAddCommentCallbackWrapper_002ECreateInstance(callback, (IMessagingCallback**)(&cComPtrNtv_003CIMessagingCallback_003E));
					if (num < 0)
					{
						goto IL_0086;
					}
				}
				fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strPostUrl)))
				{
					try
					{
						fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strZuneTag)))
						{
							try
							{
								fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strMessage)))
								{
									try
									{
										long num2 = *(long*)m_pMessaging + 112;
										num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, ushort*, IMessagingCallback*, int>)(*(ulong*)num2))((nint)m_pMessaging, ptr, ptr2, ptr3, (IMessagingCallback*)(*(ulong*)(&cComPtrNtv_003CIMessagingCallback_003E)));
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
				int num3 = 1;
				goto IL_0089;
				IL_0086:
				num3 = 0;
				goto IL_0089;
				IL_0089:
				result = (byte)num3 != 0;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIMessagingCallback_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIMessagingCallback_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIMessagingCallback_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CIMessagingCallback_003E_002ERelease(&cComPtrNtv_003CIMessagingCallback_003E);
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
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIMessagingCallback_003E cComPtrNtv_003CIMessagingCallback_003E);
			*(long*)(&cComPtrNtv_003CIMessagingCallback_003E) = 0L;
			try
			{
				int num;
				if (callback != null)
				{
					num = global::_003CModule_003E.Microsoft_002EZune_002EMessaging_002EMessagingCallbackWrapper_002ECreateInstance(callback, null, (IMessagingCallback**)(&cComPtrNtv_003CIMessagingCallback_003E));
					if (num < 0)
					{
						goto IL_0073;
					}
				}
				fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strPostUrl)))
				{
					try
					{
						fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strZuneTag)))
						{
							try
							{
								long num2 = *(long*)m_pMessaging + 120;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, IMessagingCallback*, int>)(*(ulong*)num2))((nint)m_pMessaging, ptr, ptr2, (IMessagingCallback*)(*(ulong*)(&cComPtrNtv_003CIMessagingCallback_003E)));
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
				int num3 = 1;
				goto IL_0075;
				IL_0073:
				num3 = 0;
				goto IL_0075;
				IL_0075:
				result = (byte)num3 != 0;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIMessagingCallback_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIMessagingCallback_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIMessagingCallback_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CIMessagingCallback_003E_002ERelease(&cComPtrNtv_003CIMessagingCallback_003E);
		}
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool Compose(string strPostUrl, string strMessage, string strRecipientZuneTags, string strRequestType, IPropertySetMessageData messageData, MessagingCallback callback, object state)
	{
		//IL_0019: Expected I, but got I8
		//IL_0027: Expected I, but got I8
		//IL_00d4: Expected I, but got I8
		//IL_008a: Expected I, but got I8
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
					num = global::_003CModule_003E.Microsoft_002EZune_002EMessaging_002EMessagingCallbackWrapper_002ECreateInstance(callback, state, &ptr2);
				}
				if (num >= 0)
				{
					fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strPostUrl)))
					{
						try
						{
							fixed (ushort* ptr4 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strMessage)))
							{
								try
								{
									fixed (ushort* ptr5 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strRecipientZuneTags)))
									{
										try
										{
											fixed (ushort* ptr6 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strRequestType)))
											{
												try
												{
													long num2 = *(long*)m_pMessaging + 64;
													num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, ushort*, ushort*, IMSMediaSchemaPropertySet*, IMessagingCallback*, int>)(*(ulong*)num2))((nint)m_pMessaging, ptr3, ptr4, ptr5, ptr6, ptr, ptr2);
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
		IL_00d5:
		return result;
		IL_00bd:
		result = (byte)num3 != 0;
		if (ptr2 != null)
		{
			IMessagingCallback* intPtr = ptr2;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
		}
		goto IL_00d5;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool Compose(string strPostUrl, string strMessage, MessagingCallback callback, object state)
	{
		//IL_0059: Expected I, but got I8
		//IL_0059: Expected I, but got I8
		bool result = false;
		if (m_pMessaging != null)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIMessagingCallback_003E cComPtrNtv_003CIMessagingCallback_003E);
			*(long*)(&cComPtrNtv_003CIMessagingCallback_003E) = 0L;
			try
			{
				int num;
				if (callback != null)
				{
					num = global::_003CModule_003E.Microsoft_002EZune_002EMessaging_002EMessagingCallbackWrapper_002ECreateInstance(callback, state, (IMessagingCallback**)(&cComPtrNtv_003CIMessagingCallback_003E));
					if (num < 0)
					{
						goto IL_0074;
					}
				}
				fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strPostUrl)))
				{
					try
					{
						fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strMessage)))
						{
							try
							{
								long num2 = *(long*)m_pMessaging + 72;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, IMessagingCallback*, int>)(*(ulong*)num2))((nint)m_pMessaging, ptr, ptr2, (IMessagingCallback*)(*(ulong*)(&cComPtrNtv_003CIMessagingCallback_003E)));
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
				int num3 = 1;
				goto IL_0076;
				IL_0074:
				num3 = 0;
				goto IL_0076;
				IL_0076:
				result = (byte)num3 != 0;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIMessagingCallback_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIMessagingCallback_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIMessagingCallback_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CIMessagingCallback_003E_002ERelease(&cComPtrNtv_003CIMessagingCallback_003E);
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
				num = global::_003CModule_003E.Microsoft_002EZune_002EMessaging_002EMessagingCallbackWrapper_002ECreateInstance(callback, state, &ptr);
				if (num < 0)
				{
					goto IL_0074;
				}
			}
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strFavoritesUrl)))
			{
				try
				{
					fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strInstructions)))
					{
						try
						{
							long num2 = *(long*)m_pMessaging + 80;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EFavoritesAction, ushort*, ushort*, IMessagingCallback*, int>)(*(ulong*)num2))((nint)m_pMessaging, (EFavoritesAction)eAction, ptr2, ptr3, ptr);
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
			IMessagingCallback* intPtr = ptr;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
		}
		goto IL_008d;
		IL_008d:
		return result;
		IL_0074:
		num3 = 0;
		goto IL_0077;
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
				num = global::_003CModule_003E.Microsoft_002EZune_002EMessaging_002EMessagingCallbackWrapper_002ECreateInstance(callback, state, &ptr);
				if (num < 0)
				{
					goto IL_0071;
				}
			}
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strProfileUrl)))
			{
				try
				{
					fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strFieldValue)))
					{
						try
						{
							long num2 = *(long*)m_pMessaging + 88;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, IMessagingCallback*, int>)(*(ulong*)num2))((nint)m_pMessaging, ptr2, ptr3, ptr);
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
			IMessagingCallback* intPtr = ptr;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
		}
		goto IL_008a;
		IL_008a:
		return result;
		IL_0071:
		num3 = 0;
		goto IL_0074;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool ManageProfileImage(string strProfileImageUrl, string strProfileImageResource, MessagingCallback callback, object state)
	{
		//IL_0071: Expected I, but got I8
		//IL_0071: Expected I, but got I8
		bool result = false;
		if (m_pMessaging != null && strProfileImageResource != null && strProfileImageUrl != null)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIMessagingCallback_003E cComPtrNtv_003CIMessagingCallback_003E);
			*(long*)(&cComPtrNtv_003CIMessagingCallback_003E) = 0L;
			try
			{
				int num;
				if (callback != null)
				{
					num = global::_003CModule_003E.Microsoft_002EZune_002EMessaging_002EMessagingCallbackWrapper_002ECreateInstance(callback, state, (IMessagingCallback**)(&cComPtrNtv_003CIMessagingCallback_003E));
					if (num < 0)
					{
						goto IL_008c;
					}
				}
				fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strProfileImageUrl)))
				{
					try
					{
						fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strProfileImageResource)))
						{
							try
							{
								long num2 = *(long*)m_pMessaging + 96;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, IMessagingCallback*, int>)(*(ulong*)num2))((nint)m_pMessaging, ptr, ptr2, (IMessagingCallback*)(*(ulong*)(&cComPtrNtv_003CIMessagingCallback_003E)));
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
				int num3 = 1;
				goto IL_008e;
				IL_008c:
				num3 = 0;
				goto IL_008e;
				IL_008e:
				result = (byte)num3 != 0;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIMessagingCallback_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIMessagingCallback_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIMessagingCallback_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CIMessagingCallback_003E_002ERelease(&cComPtrNtv_003CIMessagingCallback_003E);
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
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIMessagingCallback_003E cComPtrNtv_003CIMessagingCallback_003E);
			*(long*)(&cComPtrNtv_003CIMessagingCallback_003E) = 0L;
			try
			{
				if (callback != null)
				{
					num = global::_003CModule_003E.Microsoft_002EZune_002EMessaging_002EMessagingCallbackWrapper_002ECreateInstance(callback, state, (IMessagingCallback**)(&cComPtrNtv_003CIMessagingCallback_003E));
				}
				bool success = false;
				int num3;
				if (num >= 0)
				{
					profileImage.DangerousAddRef(ref success);
					num = ((!success) ? (-2147467259) : num);
					if (num >= 0)
					{
						fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(strProfileImageUrl)))
						{
							try
							{
								HBITMAP__* ptr2 = (HBITMAP__*)(int)profileImage.DangerousGetHandle();
								long num2 = *(long*)m_pMessaging + 104;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, HBITMAP__*, IMessagingCallback*, int>)(*(ulong*)num2))((nint)m_pMessaging, ptr, ptr2, (IMessagingCallback*)(*(ulong*)(&cComPtrNtv_003CIMessagingCallback_003E)));
							}
							catch
							{
								//try-fault
								ptr = null;
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
						num3 = 1;
						goto IL_00ad;
					}
				}
				num3 = 0;
				goto IL_00ad;
				IL_00ad:
				result = (byte)num3 != 0;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIMessagingCallback_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIMessagingCallback_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIMessagingCallback_003E);
				throw;
			}
			global::_003CModule_003E.CComPtrNtv_003CIMessagingCallback_003E_002ERelease(&cComPtrNtv_003CIMessagingCallback_003E);
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
			fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(title)))
			{
				try
				{
					fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(collectionName)))
					{
						try
						{
							long num2 = *(long*)m_pMessaging + 152;
							if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, ushort**, int*, int>)(*(ulong*)num2))((nint)m_pMessaging, ptr2, ptr3, &ptr, &num) >= 0 && num != 0)
							{
								result = new string((char*)ptr);
							}
							if (ptr != null)
							{
								global::_003CModule_003E.SysFreeString(ptr);
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
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(title)))
			{
				try
				{
					fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(collectionName)))
					{
						try
						{
							fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(localFilePath)))
							{
								try
								{
									long num = *(long*)m_pMessaging + 144;
									result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, ushort*, int>)(*(ulong*)num))((nint)m_pMessaging, ptr, ptr2, ptr3) >= 0;
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
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(collectionName)))
			{
				try
				{
					long num2 = *(long*)m_pMessaging + 160;
					num = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int*, int>)(*(ulong*)num2))((nint)m_pMessaging, ptr, &num) >= 0) ? num : 0);
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
			base.Finalize();
		}
	}

	public virtual sealed void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	~MessagingService()
	{
		Dispose(false);
	}
}
