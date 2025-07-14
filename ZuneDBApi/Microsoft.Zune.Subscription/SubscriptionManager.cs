using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.Zune.Subscription;

public class SubscriptionManager : IDisposable
{
	private unsafe ISubscriptionManager* m_pSubscriptionManager;

	private SubscriptionEventHandler m_SubscriptionEventHandler;

	private static SubscriptionManager sm_instance = null;

	private static object sm_lock = new object();

	public static SubscriptionManager Instance
	{
		get
		{
			if (sm_instance == null)
			{
				try
				{
					Monitor.Enter(sm_lock);
					if (sm_instance == null)
					{
						SubscriptionManager subscriptionManager = new SubscriptionManager();
						Thread.MemoryBarrier();
						sm_instance = subscriptionManager;
					}
				}
				finally
				{
					Monitor.Exit(sm_lock);
				}
			}
			return sm_instance;
		}
	}

	[SpecialName]
	public event SubscriptionEventHandler OnForegroundSubscriptionChanged
	{
		add
		{
			m_SubscriptionEventHandler = (SubscriptionEventHandler)Delegate.Combine(m_SubscriptionEventHandler, value);
		}
		remove
		{
			m_SubscriptionEventHandler = (SubscriptionEventHandler)Delegate.Remove(m_SubscriptionEventHandler, value);
		}
	}

	private void _007ESubscriptionManager()
	{
		_0021SubscriptionManager();
	}

	private unsafe void _0021SubscriptionManager()
	{
		//IL_0017: Expected I, but got I8
		//IL_0020: Expected I, but got I8
		ISubscriptionManager* pSubscriptionManager = m_pSubscriptionManager;
		if (pSubscriptionManager != null)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pSubscriptionManager + 16)))((nint)pSubscriptionManager);
			m_pSubscriptionManager = null;
		}
	}

	public unsafe int Subscribe(int subscriptionMediaId, EMediaTypes eSubscriptionMediaType)
	{
		//IL_0018: Expected I, but got I8
		//IL_004b: Expected I, but got I8
		//IL_006c: Expected I, but got I8
		int num = 0;
		num = (((long)(nint)m_pSubscriptionManager == 0) ? (-2147467261) : num);
		CSubscriptionEventProxy* ptr = null;
		if (num >= 0)
		{
			SubscribeToEvent(subscriptionMediaId, eSubscriptionMediaType, null, SubscriptionAction.RefreshFinished, userInitiated: true, &ptr);
			long num2 = *(long*)m_pSubscriptionManager + 24;
			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, EMediaTypes, int>)(*(ulong*)num2))((nint)m_pSubscriptionManager, GetLastSignedInUserId(), subscriptionMediaId, eSubscriptionMediaType);
			if (num < 0)
			{
				if (ptr == null)
				{
					goto IL_006d;
				}
				global::_003CModule_003E.Microsoft_002EZune_002ESubscription_002ECSubscriptionEventProxy_002EUninitialize(ptr);
			}
			if (0L != (nint)ptr)
			{
				CSubscriptionEventProxy* intPtr = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
			}
		}
		goto IL_006d;
		IL_006d:
		return num;
	}

	public unsafe int Subscribe(string feedUrl, string subscriptionTitle, Guid serviceId, [MarshalAs(UnmanagedType.U1)] bool isPersonalChannel, EMediaTypes eSubscriptionMediaType, ESubscriptionSource subscriptionSource, out int subscriptionMediaId)
	{
		//IL_0035: Expected I4, but got I8
		//IL_0070: Expected I, but got I8
		//IL_00cb: Expected I4, but got I8
		//IL_00d3: Expected I4, but got I8
		//IL_00db: Expected I4, but got I8
		//IL_028f: Expected I, but got I8
		//IL_0245: Expected I, but got I8
		//IL_00bd: Expected I, but got I8
		//IL_0128: Expected I8, but got I
		//IL_016d: Expected I, but got I8
		//IL_018c: Expected I, but got I8
		//IL_01ab: Expected I, but got I8
		//IL_01cf: Expected I, but got I8
		int num = 0;
		if (m_pSubscriptionManager == null || feedUrl == null)
		{
			num = -2147467261;
		}
		int num2 = -1;
		int lastSignedInUserId = GetLastSignedInUserId();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out _GUID gUID);
		*(int*)(&gUID) = 0;
		// IL initblk instruction
		System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref gUID, 4), 0, 12);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out tagPROPVARIANT tagPROPVARIANT);
		*(short*)(&tagPROPVARIANT) = 1;
		if (serviceId != Guid.Empty)
		{
			gUID = global::_003CModule_003E.GuidToGUID(serviceId);
			*(short*)(&tagPROPVARIANT) = 72;
			System.Runtime.CompilerServices.Unsafe.As<tagPROPVARIANT, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref tagPROPVARIANT, 8)) = (nint)(&gUID);
		}
		if (num >= 0 && EMediaTypes.eMediaTypePlaylist != eSubscriptionMediaType)
		{
			num = ValidateUrl(ref feedUrl);
		}
		IMSMediaSchemaPropertySet* ptr = null;
		if (num >= 0)
		{
			if (eSubscriptionMediaType != EMediaTypes.eMediaTypePlaylist)
			{
				if (eSubscriptionMediaType == EMediaTypes.eMediaTypePodcastSeries && 72 == *(ushort*)(&tagPROPVARIANT))
				{
					num = ZuneLibraryExports.CreatePropertySet((_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E.ID_MS_MEDIA_SCHEMA_SERIES), 3229616385u, &ptr);
					if (num < 0)
					{
						goto IL_027f;
					}
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, tagPROPVARIANT, int>)(*(ulong*)(*(long*)ptr + 56)))((nint)ptr, 67133455u, tagPROPVARIANT);
				}
			}
			else
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPropVariant cComPropVariant);
				// IL initblk instruction
				System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant, 0, 24);
				try
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPropVariant cComPropVariant2);
					// IL initblk instruction
					System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant2, 0, 24);
					try
					{
						System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPropVariant cComPropVariant3);
						// IL initblk instruction
						System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant3, 0, 24);
						try
						{
							EPlaylistType nSrc = (isPersonalChannel ? ((EPlaylistType)6) : ((EPlaylistType)5));
							global::_003CModule_003E.CComPropVariant_002E_003D(&cComPropVariant, (int)nSrc);
							global::_003CModule_003E.CComPropVariant_002E_003D(&cComPropVariant3, lastSignedInUserId);
							num = ZuneLibraryExports.CreatePropertySet((_GUID*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E.ID_MS_MEDIA_SCHEMA_PLAYLIST), 3229617665u, &ptr);
							if (num >= 0)
							{
								int num3;
								fixed (ushort* ptr2 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(subscriptionTitle)))
								{
									try
									{
										System.Runtime.CompilerServices.Unsafe.As<CComPropVariant, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cComPropVariant2, 8)) = (nint)global::_003CModule_003E.SysAllocString(ptr2);
										*(short*)(&cComPropVariant2) = 8;
										num3 = ((System.Runtime.CompilerServices.Unsafe.As<CComPropVariant, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cComPropVariant2, 8)) != 0) ? num : (-2147024882));
										num = num3;
									}
									catch
									{
										//try-fault
										ptr2 = null;
										throw;
									}
								}
								if (num3 >= 0)
								{
									tagPROPVARIANT tagPROPVARIANT2 = (tagPROPVARIANT)cComPropVariant2;
									num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, tagPROPVARIANT, int>)(*(ulong*)(*(long*)ptr + 56)))((nint)ptr, 16777217u, tagPROPVARIANT2);
									if (num >= 0)
									{
										tagPROPVARIANT tagPROPVARIANT3 = (tagPROPVARIANT)cComPropVariant;
										num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, tagPROPVARIANT, int>)(*(ulong*)(*(long*)ptr + 56)))((nint)ptr, 100663307u, tagPROPVARIANT3);
										if (num >= 0)
										{
											tagPROPVARIANT tagPROPVARIANT4 = (tagPROPVARIANT)cComPropVariant3;
											num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, tagPROPVARIANT, int>)(*(ulong*)(*(long*)ptr + 56)))((nint)ptr, 100663309u, tagPROPVARIANT4);
											if (num >= 0 && 72 == *(ushort*)(&tagPROPVARIANT))
											{
												num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, tagPROPVARIANT, int>)(*(ulong*)(*(long*)ptr + 56)))((nint)ptr, 67108873u, tagPROPVARIANT);
											}
										}
									}
								}
							}
						}
						catch
						{
							//try-fault
							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&global::_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant3);
							throw;
						}
						global::_003CModule_003E.CComPropVariant_002E_007Bdtor_007D(&cComPropVariant3);
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&global::_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant2);
						throw;
					}
					global::_003CModule_003E.CComPropVariant_002E_007Bdtor_007D(&cComPropVariant2);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&global::_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
					throw;
				}
				global::_003CModule_003E.CComPropVariant_002E_007Bdtor_007D(&cComPropVariant);
			}
			if (num >= 0)
			{
				fixed (ushort* ptr3 = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(feedUrl)))
				{
					try
					{
						long num4 = *(long*)m_pSubscriptionManager + 32;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, ushort*, EMediaTypes, ESubscriptionSource, IMSMediaSchemaPropertySet*, int*, int>)(*(ulong*)num4))((nint)m_pSubscriptionManager, lastSignedInUserId, ptr3, eSubscriptionMediaType, subscriptionSource, ptr, &num2);
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
					subscriptionMediaId = num2;
					if (null != m_SubscriptionEventHandler)
					{
						SubscriptonEventArguments args = new SubscriptonEventArguments(SubscriptionAction.Subscribed, subscriptionTitle, eSubscriptionMediaType, userInitiated: true);
						m_SubscriptionEventHandler(args);
					}
				}
			}
			goto IL_027f;
		}
		goto IL_0290;
		IL_0290:
		return num;
		IL_027f:
		if (ptr != null)
		{
			IMSMediaSchemaPropertySet* intPtr = ptr;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
		}
		goto IL_0290;
	}

	public unsafe int Unsubscribe(int subscriptionMediaId, EMediaTypes eSubscriptionMediaType, [MarshalAs(UnmanagedType.U1)] bool deleteContent)
	{
		//IL_002f: Expected I, but got I8
		ISubscriptionManager* pSubscriptionManager = m_pSubscriptionManager;
		int num;
		if (pSubscriptionManager == null)
		{
			num = -2147467261;
		}
		else
		{
			long num2 = *(long*)pSubscriptionManager + 40;
			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, EMediaTypes, int, int>)(*(ulong*)num2))((nint)m_pSubscriptionManager, GetLastSignedInUserId(), subscriptionMediaId, eSubscriptionMediaType, deleteContent ? 1 : 0);
			if (num >= 0 && null != m_SubscriptionEventHandler)
			{
				SubscriptonEventArguments args = new SubscriptonEventArguments(SubscriptionAction.Unsubscribed, null, eSubscriptionMediaType, userInitiated: true);
				m_SubscriptionEventHandler(args);
			}
		}
		return num;
	}

	public unsafe int Refresh(int subscriptionMediaId, EMediaTypes eSubscriptionMediaType, [MarshalAs(UnmanagedType.U1)] bool refreshCache)
	{
		//IL_0003: Expected I, but got I8
		//IL_0006: Expected I, but got I8
		//IL_0049: Expected I, but got I8
		//IL_0074: Expected I, but got I8
		//IL_0087: Expected I, but got I8
		CSubscriptionEventProxy* ptr = null;
		CSubscriptionEventProxy* ptr2 = null;
		int num;
		if (m_pSubscriptionManager == null)
		{
			num = -2147467261;
		}
		else
		{
			SubscribeToEvent(subscriptionMediaId, eSubscriptionMediaType, null, SubscriptionAction.RefreshStarted, refreshCache, &ptr);
			SubscribeToEvent(subscriptionMediaId, eSubscriptionMediaType, null, SubscriptionAction.RefreshFinished, refreshCache, &ptr2);
			ISubscriptionManager* pSubscriptionManager = m_pSubscriptionManager;
			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, EMediaTypes, int, int>)(*(ulong*)(*(long*)pSubscriptionManager + 48)))((nint)pSubscriptionManager, subscriptionMediaId, eSubscriptionMediaType, refreshCache ? 1 : 0);
			if (num < 0)
			{
				if (ptr != null)
				{
					global::_003CModule_003E.Microsoft_002EZune_002ESubscription_002ECSubscriptionEventProxy_002EUninitialize(ptr);
				}
				if (ptr2 != null)
				{
					global::_003CModule_003E.Microsoft_002EZune_002ESubscription_002ECSubscriptionEventProxy_002EUninitialize(ptr2);
				}
			}
			if (0L != (nint)ptr)
			{
				CSubscriptionEventProxy* intPtr = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
			}
			if (0L != (nint)ptr2)
			{
				CSubscriptionEventProxy* intPtr2 = ptr2;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
			}
		}
		return num;
	}

	public unsafe int SetSeriesUrl(int subscriptionMediaId, string feedUrl)
	{
		//IL_0050: Expected I, but got I8
		int num;
		if (m_pSubscriptionManager == null || feedUrl == null)
		{
			num = -2147467261;
		}
		else
		{
			num = ValidateUrl(ref feedUrl);
			if (num >= 0)
			{
				fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(feedUrl)))
				{
					try
					{
						long num2 = *(long*)m_pSubscriptionManager + 72;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, EMediaTypes, ushort*, int>)(*(ulong*)num2))((nint)m_pSubscriptionManager, GetLastSignedInUserId(), subscriptionMediaId, EMediaTypes.eMediaTypePodcastSeries, ptr);
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
				}
			}
		}
		return num;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool FindByUrl(string feedUrl, EMediaTypes eSubscriptionMediaType, out int subscriptionMediaId, out bool isSubscribed)
	{
		//IL_005c: Expected I, but got I8
		bool result = false;
		int num = 0;
		int num2 = -1;
		int num3;
		if (!(feedUrl == null) && m_pSubscriptionManager != null)
		{
			subscriptionMediaId = -1;
			if (EMediaTypes.eMediaTypePlaylist != eSubscriptionMediaType)
			{
				num3 = ValidateUrl(ref feedUrl);
				if (num3 < 0)
				{
					goto IL_0066;
				}
			}
			fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(feedUrl)))
			{
				try
				{
					long num4 = *(long*)m_pSubscriptionManager + 80;
					num3 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, ushort*, EMediaTypes, int*, int*, int>)(*(ulong*)num4))((nint)m_pSubscriptionManager, GetLastSignedInUserId(), ptr, eSubscriptionMediaType, &num2, &num);
				}
				catch
				{
					//try-fault
					ptr = null;
					throw;
				}
			}
			goto IL_0066;
		}
		goto IL_007e;
		IL_007e:
		return result;
		IL_0066:
		if (0 == num3)
		{
			subscriptionMediaId = num2;
			int num5 = ((num != 0) ? 1 : 0);
			isSubscribed = (byte)num5 != 0;
			result = true;
		}
		goto IL_007e;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool FindByServiceId(Guid serviceId, EMediaTypes eSubscriptionMediaType, out int subscriptionMediaId, out bool isSubscribed)
	{
		//IL_0038: Expected I, but got I8
		subscriptionMediaId = -1;
		isSubscribed = false;
		bool result = false;
		int num = 0;
		int num2 = -1;
		_GUID gUID = global::_003CModule_003E.GuidToGUID(serviceId);
		ISubscriptionManager* pSubscriptionManager = m_pSubscriptionManager;
		if (0 == ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, _GUID*, EMediaTypes, int*, int*, int>)(*(ulong*)(*(long*)pSubscriptionManager + 88)))((nint)pSubscriptionManager, GetLastSignedInUserId(), &gUID, eSubscriptionMediaType, &num2, &num))
		{
			subscriptionMediaId = num2;
			int num3 = ((num != 0) ? 1 : 0);
			isSubscribed = (byte)num3 != 0;
			result = true;
		}
		return result;
	}

	public unsafe int SetCredentialHandler(EMediaTypes eSubscriptionMediaType, SubscriptionCredentialHandler credentialHandler)
	{
		//IL_0077: Expected I, but got I8
		//IL_0029: Expected I, but got I8
		//IL_0062: Expected I, but got I8
		if (null == credentialHandler)
		{
			return -2147467261;
		}
		int num = 0;
		CSubscriptionCredentialProviderProxy* ptr = (CSubscriptionCredentialProviderProxy*)global::_003CModule_003E.@new(24uL);
		CSubscriptionCredentialProviderProxy* ptr2;
		try
		{
			ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002ESubscription_002ECSubscriptionCredentialProviderProxy_002E_007Bctor_007D(ptr));
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.delete(ptr);
			throw;
		}
		CSubscriptionCredentialProviderProxy* ptr3 = ptr2;
		try
		{
			if (ptr2 == null)
			{
				num = -2147024882;
			}
			else
			{
				num = global::_003CModule_003E.Microsoft_002EZune_002ESubscription_002ECSubscriptionCredentialProviderProxy_002EInitialize(ptr2, credentialHandler);
				if (num >= 0)
				{
					ISubscriptionManager* pSubscriptionManager = m_pSubscriptionManager;
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaTypes, ISubscriptionManagerCredentialProvider*, int>)(*(ulong*)(*(long*)pSubscriptionManager + 96)))((nint)pSubscriptionManager, eSubscriptionMediaType, (ISubscriptionManagerCredentialProvider*)ptr2);
				}
			}
		}
		finally
		{
			if (ptr3 != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr3 + 16)))((nint)ptr3);
			}
		}
		return num;
	}

	public unsafe int SetManagementSettings(int subscriptionMediaId, uint keepEpisodes, ESeriesPlaybackOrder playbackOrder)
	{
		//IL_0030: Expected I, but got I8
		ISubscriptionManager* pSubscriptionManager = m_pSubscriptionManager;
		int result;
		if (pSubscriptionManager == null)
		{
			result = -2147467261;
		}
		else
		{
			long num = *(long*)pSubscriptionManager + 104;
			result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, EMediaTypes, uint, ESeriesPlaybackOrder, int>)(*(ulong*)num))((nint)m_pSubscriptionManager, GetLastSignedInUserId(), subscriptionMediaId, EMediaTypes.eMediaTypePodcastSeries, keepEpisodes, playbackOrder);
		}
		return result;
	}

	public unsafe int GetManagementSettings(int subscriptionMediaId, out uint keepEpisodes, out ESeriesPlaybackOrder playbackOrder)
	{
		//IL_0041: Expected I, but got I8
		int num = 0;
		ISubscriptionManager* pSubscriptionManager = m_pSubscriptionManager;
		num = (((long)(nint)pSubscriptionManager == 0) ? (-2147467261) : num);
		uint num2 = 0u;
		ESeriesPlaybackOrder eSeriesPlaybackOrder = ESeriesPlaybackOrder.eSeriesPlaybackOrderNewestFirst;
		if (num >= 0)
		{
			long num3 = *(long*)pSubscriptionManager + 112;
			num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, EMediaTypes, uint*, ESeriesPlaybackOrder*, int>)(*(ulong*)num3))((nint)m_pSubscriptionManager, GetLastSignedInUserId(), subscriptionMediaId, EMediaTypes.eMediaTypePodcastSeries, &num2, &eSeriesPlaybackOrder);
			if (num >= 0)
			{
				keepEpisodes = num2;
				playbackOrder = eSeriesPlaybackOrder;
			}
		}
		return num;
	}

	public unsafe int DownloadEpisode(int subscriptionMediaId, int subscriptionItemMediaId)
	{
		//IL_002f: Expected I, but got I8
		ISubscriptionManager* pSubscriptionManager = m_pSubscriptionManager;
		int result;
		if (pSubscriptionManager == null)
		{
			result = -2147467261;
		}
		else
		{
			long num = *(long*)pSubscriptionManager + 120;
			result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, int, EMediaTypes, int>)(*(ulong*)num))((nint)m_pSubscriptionManager, GetLastSignedInUserId(), subscriptionMediaId, subscriptionItemMediaId, EMediaTypes.eMediaTypePodcastSeries);
		}
		return result;
	}

	public unsafe int DeleteEpisode(int subscriptionMediaId, int subscriptionItemMediaId)
	{
		//IL_0032: Expected I, but got I8
		ISubscriptionManager* pSubscriptionManager = m_pSubscriptionManager;
		int result;
		if (pSubscriptionManager == null)
		{
			result = -2147467261;
		}
		else
		{
			long num = *(long*)pSubscriptionManager + 128;
			result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, int, EMediaTypes, int>)(*(ulong*)num))((nint)m_pSubscriptionManager, GetLastSignedInUserId(), subscriptionMediaId, subscriptionItemMediaId, EMediaTypes.eMediaTypePodcastSeries);
		}
		return result;
	}

	public unsafe int SaveEpisodeToCollection(int subscriptionMediaId, int subscriptionItemMediaId)
	{
		//IL_0032: Expected I, but got I8
		ISubscriptionManager* pSubscriptionManager = m_pSubscriptionManager;
		int result;
		if (pSubscriptionManager == null)
		{
			result = -2147467261;
		}
		else
		{
			long num = *(long*)pSubscriptionManager + 144;
			result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, int, EMediaTypes, int>)(*(ulong*)num))((nint)m_pSubscriptionManager, GetLastSignedInUserId(), subscriptionMediaId, subscriptionItemMediaId, EMediaTypes.eMediaTypePodcastSeries);
		}
		return result;
	}

	public unsafe int SetEpisodeSeriesUrl(IList subscriptionItemMediaIdList, string feedUrl)
	{
		//IL_002d: Expected I, but got I8
		//IL_00c4: Expected I, but got I8
		int num = ((m_pSubscriptionManager != null && !(feedUrl == null)) ? ValidateUrl(ref feedUrl) : (-2147467261));
		fixed (ushort* ptr = &System.Runtime.CompilerServices.Unsafe.As<char, ushort>(ref global::_003CModule_003E.PtrToStringChars(feedUrl)))
		{
			int* ptr2 = null;
			int count = subscriptionItemMediaIdList.Count;
			if (num >= 0)
			{
				try
				{
					ulong num2 = (ulong)count;
					int* ptr3 = (int*)global::_003CModule_003E.new_005B_005D((num2 > 4611686018427387903L) ? ulong.MaxValue : (num2 * 4));
					ptr2 = ptr3;
					num = (((long)(nint)ptr3 == 0) ? (-2147024882) : num);
					if (num >= 0)
					{
						for (int i = 0; i < count; i++)
						{
							*(int*)((long)i * 4L + (nint)ptr2) = (int)subscriptionItemMediaIdList[i];
						}
						long num3 = *(long*)m_pSubscriptionManager + 152;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, EMediaTypes, int, int*, ushort*, int>)(*(ulong*)num3))((nint)m_pSubscriptionManager, GetLastSignedInUserId(), EMediaTypes.eMediaTypePodcastSeries, count, ptr2, ptr);
					}
				}
				finally
				{
					if (ptr2 != null)
					{
						global::_003CModule_003E.delete_005B_005D(ptr2);
					}
				}
			}
			return num;
		}
	}

	protected unsafe int SubscribeToEvent(int subscriptionMediaId, EMediaTypes eSubscriptionMediaType, string subscriptionTitle, SubscriptionAction targetAction, [MarshalAs(UnmanagedType.U1)] bool userInitiated, CSubscriptionEventProxy** ppSubscriptionEventProxy)
	{
		//IL_0051: Expected I, but got I8
		//IL_00a8: Expected I, but got I8
		//IL_0099: Expected I8, but got I
		if (ppSubscriptionEventProxy == null)
		{
			global::_003CModule_003E._ZuneShipAssert(1001u, 1475u);
			return -2147467261;
		}
		CSubscriptionEventProxy* ptr2;
		int num;
		if (null != m_SubscriptionEventHandler && (EMediaTypes.eMediaTypePodcastSeries == eSubscriptionMediaType || EMediaTypes.eMediaTypePlaylist == eSubscriptionMediaType || EMediaTypes.eMediaTypeUser == eSubscriptionMediaType))
		{
			CSubscriptionEventProxy* ptr = (CSubscriptionEventProxy*)global::_003CModule_003E.@new(72uL);
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.Microsoft_002EZune_002ESubscription_002ECSubscriptionEventProxy_002E_007Bctor_007D(ptr));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr);
				throw;
			}
			if (ptr2 == null)
			{
				num = -2147024882;
			}
			else
			{
				if (null == subscriptionTitle)
				{
					num = GetSubscriptionTitle(subscriptionMediaId, eSubscriptionMediaType, out subscriptionTitle);
					if (num < 0)
					{
						goto IL_009b;
					}
				}
				num = global::_003CModule_003E.Microsoft_002EZune_002ESubscription_002ECSubscriptionEventProxy_002EInitialize(ptr2, m_SubscriptionEventHandler, subscriptionMediaId, eSubscriptionMediaType, subscriptionTitle, targetAction, userInitiated);
				if (num < 0)
				{
					goto IL_009b;
				}
				*(long*)ppSubscriptionEventProxy = (nint)ptr2;
			}
		}
		else
		{
			num = 1;
		}
		goto IL_00ad;
		IL_00ad:
		return num;
		IL_009b:
		((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
		goto IL_00ad;
	}

	protected unsafe int GetSubscriptionTitle(int subscriptionMediaId, EMediaTypes subscriptionMediaType, out string subscriptionTitle)
	{
		//IL_0078: Expected I, but got I8
		//IL_0057: Expected I, but got I8
		//IL_00a6: Expected I, but got I8
		subscriptionTitle = null;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out DBPropertyRequestStruct dBPropertyRequestStruct);
		global::_003CModule_003E.DBPropertyRequestStruct_002E_007Bctor_007D(&dBPropertyRequestStruct, 344u);
		int num;
		try
		{
			switch (subscriptionMediaType)
			{
			case EMediaTypes.eMediaTypePlaylist:
				break;
			default:
				num = 1;
				goto IL_00b7;
			case EMediaTypes.eMediaTypeUser:
				subscriptionTitle = string.Empty;
				goto IL_0041;
			case EMediaTypes.eMediaTypePodcastSeries:
				goto IL_004b;
			}
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<DBPropertyRequestStruct*, void>)(&global::_003CModule_003E.DBPropertyRequestStruct_002E_007Bdtor_007D), &dBPropertyRequestStruct);
			throw;
		}
		try
		{
			num = ZuneLibraryExports.GetFieldValues(subscriptionMediaId, EListType.ePlaylistList, 1, &dBPropertyRequestStruct, null);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<DBPropertyRequestStruct*, void>)(&global::_003CModule_003E.DBPropertyRequestStruct_002E_007Bdtor_007D), &dBPropertyRequestStruct);
			throw;
		}
		goto IL_008a;
		IL_0041:
		global::_003CModule_003E.DBPropertyRequestStruct_002E_007Bdtor_007D(&dBPropertyRequestStruct);
		return 0;
		IL_004b:
		try
		{
			num = ZuneLibraryExports.GetFieldValues(subscriptionMediaId, EListType.ePodcastList, 1, &dBPropertyRequestStruct, null);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<DBPropertyRequestStruct*, void>)(&global::_003CModule_003E.DBPropertyRequestStruct_002E_007Bdtor_007D), &dBPropertyRequestStruct);
			throw;
		}
		goto IL_008a;
		IL_00b7:
		global::_003CModule_003E.DBPropertyRequestStruct_002E_007Bdtor_007D(&dBPropertyRequestStruct);
		return num;
		IL_008a:
		try
		{
			if (0 == num && System.Runtime.CompilerServices.Unsafe.As<DBPropertyRequestStruct, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref dBPropertyRequestStruct, 4)) >= 0)
			{
				subscriptionTitle = new string((char*)System.Runtime.CompilerServices.Unsafe.As<DBPropertyRequestStruct, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref dBPropertyRequestStruct, 16)));
			}
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<DBPropertyRequestStruct*, void>)(&global::_003CModule_003E.DBPropertyRequestStruct_002E_007Bdtor_007D), &dBPropertyRequestStruct);
			throw;
		}
		goto IL_00b7;
	}

	private unsafe SubscriptionManager()
	{
		//IL_0009: Expected I, but got I8
		ISubscriptionManager* pSubscriptionManager = null;
		int singleton = global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_9dc7c984_41d5_4130_a5ac_46d0825cd29d, (void**)(&pSubscriptionManager));
		if (singleton >= 0)
		{
			m_pSubscriptionManager = pSubscriptionManager;
			return;
		}
		throw new ApplicationException(global::_003CModule_003E.GetErrorDescription(singleton));
	}

	private int ValidateUrl([In][Out] ref string feedUrl)
	{
		if (feedUrl == null)
		{
			return -1072884976;
		}
		int num = global::_003CModule_003E.Microsoft_002EZune_002ESubscription_002EIsWellFormedUriString(ref feedUrl);
		if (num == -1072884976)
		{
			feedUrl = "http://" + feedUrl;
			num = global::_003CModule_003E.Microsoft_002EZune_002ESubscription_002EIsWellFormedUriString(ref feedUrl);
		}
		return num;
	}

	private unsafe int GetLastSignedInUserId()
	{
		//IL_0005: Expected I, but got I8
		//IL_0028: Expected I, but got I8
		//IL_0028: Expected I, but got I8
		//IL_003b: Expected I, but got I8
		int result = 1;
		IService* ptr = null;
		if (global::_003CModule_003E.GetSingleton((_GUID)global::_003CModule_003E._GUID_bb2d1edd_1bd5_4be1_8d38_36d4f0849911, (void**)(&ptr)) >= 0)
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int*, int>)(*(ulong*)(*(long*)ptr + 384)))((nint)ptr, null, &result);
		}
		if (0L != (nint)ptr)
		{
			IService* intPtr = ptr;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
		}
		return result;
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
	{
		if (P_0)
		{
			_0021SubscriptionManager();
			return;
		}
		try
		{
			_0021SubscriptionManager();
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

	~SubscriptionManager()
	{
		Dispose(false);
	}
}
