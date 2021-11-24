using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using ZuneUI;

namespace Microsoft.Zune.QuickMix
{
	public class QuickMix
	{
		private static object sm_lock = new object();

		private static QuickMix sm_instance;

		private QuickMixProgressHandler m_onProgressHandler;

		public unsafe bool IsReady
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				//IL_0018: Expected I4, but got I8
				//IL_003e: Expected I, but got I8
				//IL_003e: Expected I, but got I8
				bool flag = false;
				CComPtrNtv<IQuickMixManager> cComPtrNtv_003CIQuickMixManager_003E = new();
				try
				{
					QUICK_MIX_STATUS_INFO qUICK_MIX_STATUS_INFO;
					*(sbyte*)(&qUICK_MIX_STATUS_INFO) = 0;
                    // IL initblk instruction
                    Unsafe.InitBlockUnaligned(ref Unsafe.AddByteOffset(ref qUICK_MIX_STATUS_INFO, 1), 0, 15);
					int singleton = Module.GetSingleton(Module.GUID_IQuickMixManager, (void**)(cComPtrNtv_003CIQuickMixManager_003E.p));
					if (singleton >= 0)
					{
						long num = *(long*)(cComPtrNtv_003CIQuickMixManager_003E.p);
						singleton = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, QUICK_MIX_STATUS_INFO*, int>)(*(ulong*)(*(long*)(*(ulong*)(cComPtrNtv_003CIQuickMixManager_003E.p)) + 56)))((nint)num, &qUICK_MIX_STATUS_INFO);
						flag = ((singleton >= 0) ? (*(bool*)(&qUICK_MIX_STATUS_INFO)) : flag);
					}
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<IQuickMixManager*, void>)(&Module.CComPtrNtv_003CIQuickMixManager_003E_002E_007Bdtor_007D), cComPtrNtv_003CIQuickMixManager_003E.p);
					throw;
				}
				cComPtrNtv_003CIQuickMixManager_003E.Dispose();
				return flag;
			}
		}

		public static QuickMix Instance
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
							QuickMix quickMix = new QuickMix();
							int num = quickMix.Initialize();
							Thread.MemoryBarrier();
							if (num >= 0)
							{
								sm_instance = quickMix;
							}
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
		public unsafe event QuickMixProgressHandler OnProgress
		{
			add
			{
				//IL_002d: Expected I4, but got I8
				//IL_004f: Expected I, but got I8
				//IL_004f: Expected I, but got I8
				m_onProgressHandler = (QuickMixProgressHandler)Delegate.Combine(m_onProgressHandler, value);
				CComPtrNtv<IQuickMixManager> cComPtrNtv_003CIQuickMixManager_003E = new();
				try
				{
					QUICK_MIX_STATUS_INFO qUICK_MIX_STATUS_INFO;
					*(sbyte*)(&qUICK_MIX_STATUS_INFO) = 0;
                    // IL initblk instruction
                    Unsafe.InitBlockUnaligned(ref Unsafe.AddByteOffset(ref qUICK_MIX_STATUS_INFO, 1), 0, 15);
					if (Module.GetSingleton(Module.GUID_IQuickMixManager, (void**)(cComPtrNtv_003CIQuickMixManager_003E.p)) >= 0)
					{
						long num = *(long*)(cComPtrNtv_003CIQuickMixManager_003E.p);
						((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, QUICK_MIX_STATUS_INFO*, int>)(*(ulong*)(*(long*)(*(ulong*)(cComPtrNtv_003CIQuickMixManager_003E.p)) + 56)))((nint)num, &qUICK_MIX_STATUS_INFO);
					}
					value(Unsafe.As<QUICK_MIX_STATUS_INFO, float>(ref Unsafe.AddByteOffset(ref qUICK_MIX_STATUS_INFO, 4)), Unsafe.As<QUICK_MIX_STATUS_INFO, int>(ref Unsafe.AddByteOffset(ref qUICK_MIX_STATUS_INFO, 12)));
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<IQuickMixManager*, void>)(&Module.CComPtrNtv_003CIQuickMixManager_003E_002E_007Bdtor_007D), cComPtrNtv_003CIQuickMixManager_003E.p);
					throw;
				}
				cComPtrNtv_003CIQuickMixManager_003E.Dispose();
			}
			remove
			{
				m_onProgressHandler = (QuickMixProgressHandler)Delegate.Remove(m_onProgressHandler, value);
			}
		}

		public unsafe HRESULT CreateSession(EQuickMixMode eQuickMixMode, Guid serviceMediaId, EMediaTypes eMediaType, string mediaTitle, out QuickMixSession quickMixSession)
		{
			//IL_0045: Expected I, but got I8
			//IL_0045: Expected I, but got I8
			//IL_005d: Expected I, but got I8
			CComPtrNtv<IQuickMixManager> cComPtrNtv_003CIQuickMixManager_003E = new();
			HRESULT result;
			try
			{
				CComPtrNtv<IQuickMixSession> cComPtrNtv_003CIQuickMixSession_003E = new();
				try
				{
					int num = Module.GetSingleton(Module.GUID_IQuickMixManager, (void**)(cComPtrNtv_003CIQuickMixManager_003E.p));
					if (num >= 0)
					{
						_GUID gUID = serviceMediaId;
						fixed (char* mediaTitlePtr = mediaTitle.ToCharArray())
						{
							ushort* ptr = (ushort*)mediaTitlePtr;
							try
							{
								long num2 = *(long*)(*(ulong*)(cComPtrNtv_003CIQuickMixManager_003E.p)) + 24;
								long num3 = *(long*)(cComPtrNtv_003CIQuickMixManager_003E.p);
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQuickMixMode, _GUID*, EMediaTypes, ushort*, IQuickMixSession**, int>)(*(ulong*)num2))((nint)num3, eQuickMixMode, &gUID, eMediaType, ptr, (IQuickMixSession**)(cComPtrNtv_003CIQuickMixSession_003E.p));
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
							quickMixSession = new QuickMixSession((IQuickMixSession*)(*(ulong*)(cComPtrNtv_003CIQuickMixSession_003E.p)));
						}
					}
					result = num;
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<IQuickMixSession*, void>)(&Module.CComPtrNtv_003CIQuickMixSession_003E_002E_007Bdtor_007D), cComPtrNtv_003CIQuickMixSession_003E.p);
					throw;
				}
				cComPtrNtv_003CIQuickMixSession_003E.Dispose();
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<IQuickMixManager*, void>)(&Module.CComPtrNtv_003CIQuickMixManager_003E_002E_007Bdtor_007D), cComPtrNtv_003CIQuickMixManager_003E.p);
				throw;
			}
			cComPtrNtv_003CIQuickMixManager_003E.Dispose();
			return result;
		}

		public unsafe HRESULT CreateSession(EQuickMixMode eQuickMixMode, int[] seedMediaIds, EMediaTypes eMediaType, out QuickMixSession quickMixSession)
		{
			//IL_003d: Expected I, but got I8
			//IL_003d: Expected I, but got I8
			//IL_0055: Expected I, but got I8
			CComPtrNtv<IQuickMixManager> cComPtrNtv_003CIQuickMixManager_003E = new();
			HRESULT result;
			try
			{
				CComPtrNtv<IQuickMixSession> cComPtrNtv_003CIQuickMixSession_003E = new();
				try
				{
					int num = Module.GetSingleton(Module.GUID_IQuickMixManager, (void**)(cComPtrNtv_003CIQuickMixManager_003E.p));
					if (num >= 0)
					{
						fixed (int* ptr = &seedMediaIds[0])
						{
							try
							{
								long num2 = *(long*)(*(ulong*)(cComPtrNtv_003CIQuickMixManager_003E.p)) + 32;
								long num3 = *(long*)(cComPtrNtv_003CIQuickMixManager_003E.p);
								IntPtr intPtr = (nint)seedMediaIds.LongLength;
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQuickMixMode, int, int*, EMediaTypes, IQuickMixSession**, int>)(*(ulong*)num2))((nint)num3, eQuickMixMode, (int)(nint)intPtr, ptr, eMediaType, (IQuickMixSession**)(cComPtrNtv_003CIQuickMixSession_003E.p));
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
							quickMixSession = new QuickMixSession((IQuickMixSession*)(*(ulong*)(cComPtrNtv_003CIQuickMixSession_003E.p)));
						}
					}
					result = num;
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<IQuickMixSession*, void>)(&Module.CComPtrNtv_003CIQuickMixSession_003E_002E_007Bdtor_007D), cComPtrNtv_003CIQuickMixSession_003E.p);
					throw;
				}
				cComPtrNtv_003CIQuickMixSession_003E.Dispose();
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<IQuickMixManager*, void>)(&Module.CComPtrNtv_003CIQuickMixManager_003E_002E_007Bdtor_007D), cComPtrNtv_003CIQuickMixManager_003E.p);
				throw;
			}
			cComPtrNtv_003CIQuickMixManager_003E.Dispose();
			return result;
		}

		private QuickMix()
		{
		}

		private unsafe int Initialize()
		{
			//IL_002d: Expected I, but got I8
			//IL_0066: Expected I, but got I8
			//IL_006b: Expected I, but got I8
			//IL_007d: Expected I, but got I8
			//IL_007d: Expected I, but got I8
			//IL_0090: Expected I, but got I8
			CComPtrNtv<IQuickMixManager> cComPtrNtv_003CIQuickMixManager_003E = new();
			int num2;
			try
			{
				QuickMixProgressHandler progressHandler = QuickMixProgressHandlerInternal;
				QuickMixCallbackProxy* ptr = (QuickMixCallbackProxy*)Module.@new(48uL);
				QuickMixCallbackProxy* ptr2;
				try
				{
					ptr2 = ((ptr == null) ? null : Module.Microsoft_002EZune_002EQuickMix_002EQuickMixCallbackProxy_002E_007Bctor_007D(ptr, progressHandler));
				}
				catch
				{
					//try-fault
					Module.delete(ptr);
					throw;
				}
				int num = (((long)(nint)ptr2 == 0) ? (-2147024882) : 0);
				num2 = num;
				if (num >= 0)
				{
					num2 = Module.GetSingleton(Module.GUID_IQuickMixManager, (void**)(cComPtrNtv_003CIQuickMixManager_003E.p));
					if (num2 >= 0)
					{
						QuickMixCallbackProxy* ptr3 = (QuickMixCallbackProxy*)((ptr2 == null) ? 0 : ((ulong)(nint)ptr2 + 8uL));
						long num3 = *(long*)(cComPtrNtv_003CIQuickMixManager_003E.p);
						num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, IQuickMixStatusCallback*, int>)(*(ulong*)(*(long*)(*(ulong*)(cComPtrNtv_003CIQuickMixManager_003E.p)) + 72)))((nint)num3, (IQuickMixStatusCallback*)ptr3);
					}
				}
				if (ptr2 != null)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
				}
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<IQuickMixManager*, void>)(&Module.CComPtrNtv_003CIQuickMixManager_003E_002E_007Bdtor_007D), cComPtrNtv_003CIQuickMixManager_003E.p);
				throw;
			}
			cComPtrNtv_003CIQuickMixManager_003E.Dispose();
			return num2;
		}

		private void QuickMixProgressHandlerInternal(float progress, int secondsRemaining)
		{
			m_onProgressHandler?.Invoke(progress, secondsRemaining);
		}
	}
}
