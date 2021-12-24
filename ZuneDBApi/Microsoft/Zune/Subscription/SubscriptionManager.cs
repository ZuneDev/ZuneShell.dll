using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.Zune.Subscription
{
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
					lock (sm_lock)
                    {
						if (sm_instance == null)
						{
							SubscriptionManager subscriptionManager = new SubscriptionManager();
							Thread.MemoryBarrier();
							sm_instance = subscriptionManager;
						}
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
				ISubscriptionManager* pSubscriptionManager = m_pSubscriptionManager;
				int lastSignedInUserId = GetLastSignedInUserId();
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, EMediaTypes, int>)(*(ulong*)num2))((nint)pSubscriptionManager, lastSignedInUserId, subscriptionMediaId, eSubscriptionMediaType);
				if (num < 0)
				{
					if (ptr == null)
					{
						goto IL_006d;
					}
					Module.Microsoft_002EZune_002ESubscription_002ECSubscriptionEventProxy_002EUninitialize(ptr);
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
			//IL_00bd: Expected I, but got I8
			//IL_00cb: Expected I4, but got I8
			//IL_00d3: Expected I4, but got I8
			//IL_00db: Expected I4, but got I8
			//IL_0128: Expected I8, but got I
			//IL_016d: Expected I, but got I8
			//IL_018c: Expected I, but got I8
			//IL_01ab: Expected I, but got I8
			//IL_01cf: Expected I, but got I8
			//IL_0245: Expected I, but got I8
			//IL_028f: Expected I, but got I8
			int num = 0;
			if (m_pSubscriptionManager == null || feedUrl == null)
			{
				num = -2147467261;
			}
			int num2 = -1;
			int lastSignedInUserId = GetLastSignedInUserId();
			_GUID gUID = Guid.Empty;
			PROPVARIANT tagPROPVARIANT = new() { vt = VARTYPE.VT_NULL };
			if (serviceId != Guid.Empty)
			{
				gUID = serviceId;
				tagPROPVARIANT = new(gUID, VarEnum.VT_CLSID);
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
						num = Module.CreatePropertySet((_GUID*)Unsafe.AsPointer(ref Module.ID_MS_MEDIA_SCHEMA_SERIES), 3229616385u, &ptr);
						if (num < 0)
						{
							goto IL_027f;
						}
						IMSMediaSchemaPropertySet* intPtr = ptr;
						PROPVARIANT tagPROPVARIANT2 = tagPROPVARIANT;
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, PROPVARIANT, int>)(*(ulong*)(*(long*)ptr + 56)))((nint)intPtr, 67133455u, tagPROPVARIANT2);
					}
				}
				else
				{
                    PROPVARIANT cComPropVariant = new();
					try
					{
                        PROPVARIANT cComPropVariant2 = new();
						try
						{
                            PROPVARIANT cComPropVariant3 = new();
							try
							{
								EPlaylistType nSrc = (isPersonalChannel ? ((EPlaylistType)6) : ((EPlaylistType)5));
								Module.CComPropVariant_002E_003D(&cComPropVariant, (int)nSrc);
								Module.CComPropVariant_002E_003D(&cComPropVariant3, lastSignedInUserId);
								num = Module.CreatePropertySet((_GUID*)Unsafe.AsPointer(ref Module.ID_MS_MEDIA_SCHEMA_PLAYLIST), 3229617665u, &ptr);
								if (num >= 0)
								{
									int num3;
									fixed (char* subscriptionTitlePtr = subscriptionTitle.ToCharArray())
									{
										ushort* ptr2 = (ushort*)subscriptionTitlePtr;
										try
										{
                                            Unsafe.As<PROPVARIANT, long>(ref Unsafe.AddByteOffset(ref cComPropVariant2, 8)) = (nint)Module.SysAllocString(ptr2);
											*(short*)(&cComPropVariant2) = 8;
											num3 = ((Unsafe.As<PROPVARIANT, long>(ref Unsafe.AddByteOffset(ref cComPropVariant2, 8)) != 0) ? num : (-2147024882));
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
										PROPVARIANT tagPROPVARIANT3 = cComPropVariant2;
										IMSMediaSchemaPropertySet* intPtr2 = ptr;
										num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, PROPVARIANT, int>)(*(ulong*)(*(long*)ptr + 56)))((nint)intPtr2, 16777217u, tagPROPVARIANT3);
										if (num >= 0)
										{
											PROPVARIANT tagPROPVARIANT4 = cComPropVariant;
											IMSMediaSchemaPropertySet* intPtr3 = ptr;
											num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, PROPVARIANT, int>)(*(ulong*)(*(long*)ptr + 56)))((nint)intPtr3, 100663307u, tagPROPVARIANT4);
											if (num >= 0)
											{
												PROPVARIANT tagPROPVARIANT5 = cComPropVariant3;
												IMSMediaSchemaPropertySet* intPtr4 = ptr;
												num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, PROPVARIANT, int>)(*(ulong*)(*(long*)ptr + 56)))((nint)intPtr4, 100663309u, tagPROPVARIANT5);
												if (num >= 0 && 72 == *(ushort*)(&tagPROPVARIANT))
												{
													IMSMediaSchemaPropertySet* intPtr5 = ptr;
													PROPVARIANT tagPROPVARIANT6 = tagPROPVARIANT;
													num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, PROPVARIANT, int>)(*(ulong*)(*(long*)ptr + 56)))((nint)intPtr5, 67108873u, tagPROPVARIANT6);
												}
											}
										}
									}
								}
							}
							catch
							{
                                //try-fault
                                Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<PROPVARIANT*, void>)(&Module.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant3);
								throw;
							}
							Module.CComPropVariant_002E_007Bdtor_007D(&cComPropVariant3);
						}
						catch
						{
                            //try-fault
                            Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<PROPVARIANT*, void>)(&Module.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant2);
							throw;
						}
						Module.CComPropVariant_002E_007Bdtor_007D(&cComPropVariant2);
					}
					catch
					{
                        //try-fault
                        Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<PROPVARIANT*, void>)(&Module.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
						throw;
					}
					Module.CComPropVariant_002E_007Bdtor_007D(&cComPropVariant);
				}
				if (num >= 0)
				{
					fixed (char* feedUrlPtr = feedUrl))
					{
						ushort* ptr3 = (ushort*)feedUrlPtr;
						try
						{
							long num4 = *(long*)m_pSubscriptionManager + 32;
							ISubscriptionManager* pSubscriptionManager = m_pSubscriptionManager;
							var val = ptr3;
							IMSMediaSchemaPropertySet* intPtr6 = ptr;
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, ushort*, EMediaTypes, ESubscriptionSource, IMSMediaSchemaPropertySet*, int*, int>)(*(ulong*)num4))((nint)pSubscriptionManager, lastSignedInUserId, (ushort*)(nint)val, eSubscriptionMediaType, subscriptionSource, intPtr6, &num2);
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
				IMSMediaSchemaPropertySet* intPtr7 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr7 + 16)))((nint)intPtr7);
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
				ISubscriptionManager* pSubscriptionManager2 = m_pSubscriptionManager;
				int lastSignedInUserId = GetLastSignedInUserId();
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, EMediaTypes, int, int>)(*(ulong*)num2))((nint)pSubscriptionManager2, lastSignedInUserId, subscriptionMediaId, eSubscriptionMediaType, deleteContent ? 1 : 0);
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
						Module.Microsoft_002EZune_002ESubscription_002ECSubscriptionEventProxy_002EUninitialize(ptr);
					}
					if (ptr2 != null)
					{
						Module.Microsoft_002EZune_002ESubscription_002ECSubscriptionEventProxy_002EUninitialize(ptr2);
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
					fixed (char* feedUrlPtr = feedUrl))
					{
						ushort* ptr = (ushort*)feedUrlPtr;
						try
						{
							long num2 = *(long*)m_pSubscriptionManager + 72;
							ISubscriptionManager* pSubscriptionManager = m_pSubscriptionManager;
							int lastSignedInUserId = GetLastSignedInUserId();
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, EMediaTypes, ushort*, int>)(*(ulong*)num2))((nint)pSubscriptionManager, lastSignedInUserId, subscriptionMediaId, EMediaTypes.eMediaTypePodcastSeries, ptr);
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
				fixed (char* feedUrlPtr = feedUrl.ToCharArray())
				{
					ushort* ptr = (ushort*)feedUrlPtr;
					try
					{
						long num4 = *(long*)m_pSubscriptionManager + 80;
						ISubscriptionManager* pSubscriptionManager = m_pSubscriptionManager;
						int lastSignedInUserId = GetLastSignedInUserId();
						num3 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, ushort*, EMediaTypes, int*, int*, int>)(*(ulong*)num4))((nint)pSubscriptionManager, lastSignedInUserId, ptr, eSubscriptionMediaType, &num2, &num);
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
				int num5 = ((isSubscribed = ((num != 0) ? true : false)) ? 1 : 0);
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
			_GUID gUID = serviceId;
			ISubscriptionManager* pSubscriptionManager = m_pSubscriptionManager;
			int lastSignedInUserId = GetLastSignedInUserId();
			if (0 == ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, _GUID*, EMediaTypes, int*, int*, int>)(*(ulong*)(*(long*)pSubscriptionManager + 88)))((nint)pSubscriptionManager, lastSignedInUserId, &gUID, eSubscriptionMediaType, &num2, &num))
			{
				subscriptionMediaId = num2;
				int num3 = ((isSubscribed = ((num != 0) ? true : false)) ? 1 : 0);
				result = true;
			}
			return result;
		}

		public unsafe int SetCredentialHandler(EMediaTypes eSubscriptionMediaType, SubscriptionCredentialHandler credentialHandler)
		{
			//IL_0029: Expected I, but got I8
			//IL_0062: Expected I, but got I8
			//IL_0077: Expected I, but got I8
			if (null == credentialHandler)
			{
				return -2147467261;
			}
			int num = 0;
			CSubscriptionCredentialProviderProxy* ptr = (CSubscriptionCredentialProviderProxy*)Module.@new(24uL);
			CSubscriptionCredentialProviderProxy* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : Module.Microsoft_002EZune_002ESubscription_002ECSubscriptionCredentialProviderProxy_002E_007Bctor_007D(ptr));
			}
			catch
			{
				//try-fault
				Module.delete(ptr);
				throw;
			}
			CSubscriptionCredentialProviderProxy* ptr3 = ptr2;
			try
			{
				if (ptr2 == null)
				{
					return -2147024882;
				}
				num = Module.Microsoft_002EZune_002ESubscription_002ECSubscriptionCredentialProviderProxy_002EInitialize(ptr2, credentialHandler);
				if (num >= 0)
				{
					ISubscriptionManager* pSubscriptionManager = m_pSubscriptionManager;
					return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaTypes, ISubscriptionManagerCredentialProvider*, int>)(*(ulong*)(*(long*)pSubscriptionManager + 96)))((nint)pSubscriptionManager, eSubscriptionMediaType, (ISubscriptionManagerCredentialProvider*)ptr2);
				}
				return num;
			}
			finally
			{
				if (ptr3 != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr3 + 16)))((nint)ptr3);
				}
			}
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
				ISubscriptionManager* pSubscriptionManager2 = m_pSubscriptionManager;
				int lastSignedInUserId = GetLastSignedInUserId();
				result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, EMediaTypes, uint, ESeriesPlaybackOrder, int>)(*(ulong*)num))((nint)pSubscriptionManager2, lastSignedInUserId, subscriptionMediaId, EMediaTypes.eMediaTypePodcastSeries, keepEpisodes, playbackOrder);
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
				ISubscriptionManager* pSubscriptionManager2 = m_pSubscriptionManager;
				int lastSignedInUserId = GetLastSignedInUserId();
				num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, EMediaTypes, uint*, ESeriesPlaybackOrder*, int>)(*(ulong*)num3))((nint)pSubscriptionManager2, lastSignedInUserId, subscriptionMediaId, EMediaTypes.eMediaTypePodcastSeries, &num2, &eSeriesPlaybackOrder);
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
				ISubscriptionManager* pSubscriptionManager2 = m_pSubscriptionManager;
				int lastSignedInUserId = GetLastSignedInUserId();
				result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, int, EMediaTypes, int>)(*(ulong*)num))((nint)pSubscriptionManager2, lastSignedInUserId, subscriptionMediaId, subscriptionItemMediaId, EMediaTypes.eMediaTypePodcastSeries);
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
				ISubscriptionManager* pSubscriptionManager2 = m_pSubscriptionManager;
				int lastSignedInUserId = GetLastSignedInUserId();
				result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, int, EMediaTypes, int>)(*(ulong*)num))((nint)pSubscriptionManager2, lastSignedInUserId, subscriptionMediaId, subscriptionItemMediaId, EMediaTypes.eMediaTypePodcastSeries);
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
				ISubscriptionManager* pSubscriptionManager2 = m_pSubscriptionManager;
				int lastSignedInUserId = GetLastSignedInUserId();
				result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, int, EMediaTypes, int>)(*(ulong*)num))((nint)pSubscriptionManager2, lastSignedInUserId, subscriptionMediaId, subscriptionItemMediaId, EMediaTypes.eMediaTypePodcastSeries);
			}
			return result;
		}

		public unsafe int SetEpisodeSeriesUrl(IList subscriptionItemMediaIdList, string feedUrl)
		{
			//IL_002d: Expected I, but got I8
			//IL_00c4: Expected I, but got I8
			int num = ((m_pSubscriptionManager != null && !(feedUrl == null)) ? ValidateUrl(ref feedUrl) : (-2147467261));
			fixed (char* feedUrlPtr = feedUrl.ToCharArray())
			{
				ushort* ptr3 = (ushort*)feedUrlPtr;
				int* ptr = null;
				int count = subscriptionItemMediaIdList.Count;
				if (num >= 0)
				{
					try
					{
						ulong num2 = (ulong)count;
						int* ptr2 = (int*)Module.new_005B_005D((num2 > 4611686018427387903L) ? ulong.MaxValue : (num2 * 4));
						ptr = ptr2;
						num = (((long)(nint)ptr2 == 0) ? (-2147024882) : num);
						if (num >= 0)
						{
							for (int i = 0; i < count; i++)
							{
								*(int*)(i * 4L + (nint)ptr) = (int)subscriptionItemMediaIdList[i];
							}
							long num3 = *(long*)m_pSubscriptionManager + 152;
							ISubscriptionManager* pSubscriptionManager = m_pSubscriptionManager;
							int lastSignedInUserId = GetLastSignedInUserId();
							return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, EMediaTypes, int, int*, ushort*, int>)(*(ulong*)num3))((nint)pSubscriptionManager, lastSignedInUserId, EMediaTypes.eMediaTypePodcastSeries, count, ptr, ptr3);
						}
						return num;
					}
					finally
					{
						if (ptr != null)
						{
							Module.delete_005B_005D(ptr);
						}
					}
				}
				return num;
			}
		}

		protected unsafe int SubscribeToEvent(int subscriptionMediaId, EMediaTypes eSubscriptionMediaType, string subscriptionTitle, SubscriptionAction targetAction, [MarshalAs(UnmanagedType.U1)] bool userInitiated, CSubscriptionEventProxy** ppSubscriptionEventProxy)
		{
			//IL_0051: Expected I, but got I8
			//IL_0099: Expected I8, but got I
			//IL_00a8: Expected I, but got I8
			if (ppSubscriptionEventProxy == null)
			{
				Module._ZuneShipAssert(1001u, 1475u);
				return -2147467261;
			}
			CSubscriptionEventProxy* ptr2;
			int num;
			if (null != m_SubscriptionEventHandler && (EMediaTypes.eMediaTypePodcastSeries == eSubscriptionMediaType || EMediaTypes.eMediaTypePlaylist == eSubscriptionMediaType || EMediaTypes.eMediaTypeUser == eSubscriptionMediaType))
			{
				CSubscriptionEventProxy* ptr = (CSubscriptionEventProxy*)Module.@new(72uL);
				try
				{
					ptr2 = ((ptr == null) ? null : Module.Microsoft_002EZune_002ESubscription_002ECSubscriptionEventProxy_002E_007Bctor_007D(ptr));
				}
				catch
				{
					//try-fault
					Module.delete(ptr);
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
					num = Module.Microsoft_002EZune_002ESubscription_002ECSubscriptionEventProxy_002EInitialize(ptr2, m_SubscriptionEventHandler, subscriptionMediaId, eSubscriptionMediaType, subscriptionTitle, targetAction, userInitiated);
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
			//Discarded unreachable code: IL_005a
			//IL_0057: Expected I, but got I8
			//IL_0078: Expected I, but got I8
			//IL_00a6: Expected I, but got I8
			subscriptionTitle = null;
			DBPropertyRequestStruct dBPropertyRequestStruct;
			Module.DBPropertyRequestStruct_002E_007Bctor_007D(&dBPropertyRequestStruct, 344u);
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
					subscriptionTitle = "";
					goto IL_0041;
				case EMediaTypes.eMediaTypePodcastSeries:
					goto IL_004a;
				}
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<DBPropertyRequestStruct*, void>)(&Module.DBPropertyRequestStruct_002E_007Bdtor_007D), &dBPropertyRequestStruct);
				throw;
			}
			try
			{
				num = Module.GetFieldValues(subscriptionMediaId, EListType.ePlaylistList, 1, &dBPropertyRequestStruct, null);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<DBPropertyRequestStruct*, void>)(&Module.DBPropertyRequestStruct_002E_007Bdtor_007D), &dBPropertyRequestStruct);
				throw;
			}
			goto IL_0089;
			IL_0041:
			Module.DBPropertyRequestStruct_002E_007Bdtor_007D(&dBPropertyRequestStruct);
			return 0;
			IL_004a:
			try
			{
				num = Module.GetFieldValues(subscriptionMediaId, EListType.ePodcastList, 1, &dBPropertyRequestStruct, null);
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<DBPropertyRequestStruct*, void>)(&Module.DBPropertyRequestStruct_002E_007Bdtor_007D), &dBPropertyRequestStruct);
				throw;
			}
			goto IL_0089;
			IL_00b7:
			Module.DBPropertyRequestStruct_002E_007Bdtor_007D(&dBPropertyRequestStruct);
			return num;
			IL_0089:
			try
			{
				if (0 == num && Unsafe.As<DBPropertyRequestStruct, int>(ref Unsafe.AddByteOffset(ref dBPropertyRequestStruct, 4)) >= 0)
				{
					subscriptionTitle = new string((char*)Unsafe.As<DBPropertyRequestStruct, ulong>(ref Unsafe.AddByteOffset(ref dBPropertyRequestStruct, 16)));
				}
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<DBPropertyRequestStruct*, void>)(&Module.DBPropertyRequestStruct_002E_007Bdtor_007D), &dBPropertyRequestStruct);
				throw;
			}
			goto IL_00b7;
		}

		private unsafe SubscriptionManager()
		{
			//IL_0009: Expected I, but got I8
			ISubscriptionManager* pSubscriptionManager = null;
			int singleton = Module.GetSingleton(Module.GUID_ISubscriptionManager, (void**)(&pSubscriptionManager));
			if (singleton >= 0)
			{
				m_pSubscriptionManager = pSubscriptionManager;
				return;
			}
			throw new ApplicationException(Module.GetErrorDescription(singleton));
		}

		private int ValidateUrl([In][Out] ref string feedUrl)
		{
			if (feedUrl == null)
			{
				return -1072884976;
			}
			int num = Module.Microsoft_002EZune_002ESubscription_002EIsWellFormedUriString(ref feedUrl);
			if (num == -1072884976)
			{
				feedUrl = "http://" + feedUrl;
				num = Module.Microsoft_002EZune_002ESubscription_002EIsWellFormedUriString(ref feedUrl);
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
			if (Module.GetSingleton(Module.GUID_IService, (void**)(&ptr)) >= 0)
			{
				IService* intPtr = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID*, int*, int>)(*(ulong*)(*(long*)ptr + 384)))((nint)intPtr, null, &result);
			}
			if (0L != (nint)ptr)
			{
				IService* intPtr2 = ptr;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr2 + 16)))((nint)intPtr2);
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
				//base.Finalize();
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		~SubscriptionManager()
		{
			Dispose(false);
		}
	}
}
