using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Zune.Playlist;
using ZuneUI;

namespace Microsoft.Zune.QuickMix
{
	public class QuickMixSession : IDisposable
	{
		private readonly CComPtrMgd_003CIQuickMixSession_003E m_spSession;

		public unsafe QuickMixSession(IQuickMixSession* pSession)
		{
			CComPtrMgd_003CIQuickMixSession_003E spSession = new CComPtrMgd_003CIQuickMixSession_003E();
			try
			{
				m_spSession = spSession;
				base._002Ector();
				m_spSession.op_Assign(pSession);
			}
			catch
			{
				//try-fault
				((IDisposable)m_spSession).Dispose();
				throw;
			}
		}

		private void _007EQuickMixSession()
		{
			_0021QuickMixSession();
		}

		private unsafe void _0021QuickMixSession()
		{
			//IL_001c: Expected I, but got I8
			//IL_002a: Expected I, but got I8
			IQuickMixSession* p = m_spSession.p;
			if (p != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)p + 80)))((nint)p);
				m_spSession.op_Assign(null);
			}
		}

		public unsafe EQuickMixType GetQuickMixType()
		{
			//IL_0017: Expected I, but got I8
			IQuickMixSession* p = m_spSession.p;
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQuickMixType>)(*(ulong*)(*(long*)p + 24)))((nint)p);
		}

		public unsafe HRESULT SetQuickMixType(EQuickMixType eQuickMixType)
		{
			//IL_001a: Expected I, but got I8
			IQuickMixSession* p = m_spSession.p;
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQuickMixType, int>)(*(ulong*)(*(long*)p + 32)))((nint)p, eQuickMixType);
		}

		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool GetQuickMixTypeAvailable(EQuickMixType eQuickMixType)
		{
			//IL_001a: Expected I, but got I8
			IQuickMixSession* p = m_spSession.p;
			return ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EQuickMixType, byte>)(*(ulong*)(*(long*)p + 40)))((nint)p, eQuickMixType) != 0;
		}

		public unsafe HRESULT GetSimilarMedia(uint maxBatchTracks, TimeSpan maxBatchTimeout, SimilarMediaBatchHandler similarBatchHandler, BatchEndHandler batchEndHandler)
		{
			//IL_0030: Expected I, but got I8
			//IL_0075: Expected I, but got I8
			//IL_0085: Expected I, but got I8
			if (null == batchEndHandler)
			{
				return -2147467261;
			}
			QuickMixCallbackProxy* ptr = (QuickMixCallbackProxy*)Module.@new(48uL);
			QuickMixCallbackProxy* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : Module.Microsoft_002EZune_002EQuickMix_002EQuickMixCallbackProxy_002E_007Bctor_007D(ptr, similarBatchHandler, batchEndHandler));
			}
			catch
			{
				//try-fault
				Module.delete(ptr);
				throw;
			}
			int num = (((long)(nint)ptr2 == 0) ? (-2147024882) : 0);
			int num2 = num;
			if (num >= 0)
			{
				IQuickMixSession* p = m_spSession.p;
				long num3 = *(long*)p + 48;
				double totalMilliseconds = maxBatchTimeout.TotalMilliseconds;
				num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EPlaylistLimitType, uint, uint, IQuickMixSessionCallback*, int>)(*(ulong*)num3))((nint)p, (EPlaylistLimitType)2, maxBatchTracks, (uint)totalMilliseconds, (IQuickMixSessionCallback*)ptr2);
			}
			if (ptr2 != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
			}
			return num2;
		}

		public unsafe HRESULT GetSimilarMedia(TimeSpan maxBatchDuration, TimeSpan maxBatchTimeout, SimilarMediaBatchHandler similarBatchHandler, BatchEndHandler batchEndHandler)
		{
			//IL_0030: Expected I, but got I8
			//IL_007c: Expected I, but got I8
			//IL_008c: Expected I, but got I8
			if (null == batchEndHandler)
			{
				return -2147467261;
			}
			QuickMixCallbackProxy* ptr = (QuickMixCallbackProxy*)Module.@new(48uL);
			QuickMixCallbackProxy* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : Module.Microsoft_002EZune_002EQuickMix_002EQuickMixCallbackProxy_002E_007Bctor_007D(ptr, similarBatchHandler, batchEndHandler));
			}
			catch
			{
				//try-fault
				Module.delete(ptr);
				throw;
			}
			int num = (((long)(nint)ptr2 == 0) ? (-2147024882) : 0);
			int num2 = num;
			if (num >= 0)
			{
				IQuickMixSession* p = m_spSession.p;
				long num3 = *(long*)p + 48;
				uint num4 = (uint)maxBatchDuration.TotalSeconds;
				double totalMilliseconds = maxBatchTimeout.TotalMilliseconds;
				num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EPlaylistLimitType, uint, uint, IQuickMixSessionCallback*, int>)(*(ulong*)num3))((nint)p, (EPlaylistLimitType)1, num4, (uint)totalMilliseconds, (IQuickMixSessionCallback*)ptr2);
			}
			if (ptr2 != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
			}
			return num2;
		}

		public unsafe HRESULT Refresh(TimeSpan maxBatchTimeout, [MarshalAs(UnmanagedType.U1)] bool reloadSettings, SimilarMediaBatchHandler similarBatchHandler, BatchEndHandler batchEndHandler)
		{
			//IL_0030: Expected I, but got I8
			//IL_0074: Expected I, but got I8
			//IL_0084: Expected I, but got I8
			if (null == batchEndHandler)
			{
				return -2147467261;
			}
			QuickMixCallbackProxy* ptr = (QuickMixCallbackProxy*)Module.@new(48uL);
			QuickMixCallbackProxy* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : Module.Microsoft_002EZune_002EQuickMix_002EQuickMixCallbackProxy_002E_007Bctor_007D(ptr, similarBatchHandler, batchEndHandler));
			}
			catch
			{
				//try-fault
				Module.delete(ptr);
				throw;
			}
			int num = (((long)(nint)ptr2 == 0) ? (-2147024882) : 0);
			int num2 = num;
			if (num >= 0)
			{
				IQuickMixSession* p = m_spSession.p;
				long num3 = *(long*)p + 56;
				double totalMilliseconds = maxBatchTimeout.TotalMilliseconds;
				num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, byte, IQuickMixSessionCallback*, int>)(*(ulong*)num3))((nint)p, (uint)totalMilliseconds, reloadSettings ? ((byte)1) : ((byte)0), (IQuickMixSessionCallback*)ptr2);
			}
			if (ptr2 != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)ptr2 + 16)))((nint)ptr2);
			}
			return num2;
		}

		public unsafe HRESULT GetPlaylistTitle(out string playlistTitle)
		{
			//IL_0023: Expected I, but got I8
			//IL_0031: Expected I, but got I8
			WBSTRString wBSTRString;
			Module.WBSTRString_002E_007Bctor_007D(&wBSTRString);
			HRESULT result;
			try
			{
				IQuickMixSession* p = m_spSession.p;
				int num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)p + 64)))((nint)p, (ushort**)(&wBSTRString));
				if (num >= 0)
				{
					playlistTitle = new string((char*)(*(ulong*)(&wBSTRString)));
				}
				result = num;
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

		public unsafe HRESULT SaveAsPlaylist(string playlistTitle, CreatePlaylistOption createOption, out int playlistId)
		{
			//IL_0048: Expected I, but got I8
			int num = -1;
			fixed (char* playlistTitlePtr = playlistTitle.ToCharArray())
			{
				ushort* ptr = (ushort*)playlistTitlePtr;
				int num2;
				EPlaylistCreateConflictAction ePlaylistCreateConflictAction;
				if (createOption != 0)
				{
					if (createOption != CreatePlaylistOption.RenameOnConflict)
					{
						if (createOption != CreatePlaylistOption.OverwriteOnConflict)
						{
							num2 = -2147418113;
							goto IL_0051;
						}
						ePlaylistCreateConflictAction = (EPlaylistCreateConflictAction)1;
					}
					else
					{
						ePlaylistCreateConflictAction = (EPlaylistCreateConflictAction)2;
					}
				}
				else
				{
					ePlaylistCreateConflictAction = (EPlaylistCreateConflictAction)0;
				}
				IQuickMixSession* p = m_spSession.p;
				long num3 = *(long*)p + 72;
				num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, EPlaylistCreateConflictAction, int*, int>)(*(ulong*)num3))((nint)p, ptr, ePlaylistCreateConflictAction, &num);
				if (num2 >= 0)
				{
					playlistId = num;
				}
				goto IL_0051;
				IL_0051:
				return num2;
			}
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
					_0021QuickMixSession();
				}
				finally
				{
					((IDisposable)m_spSession).Dispose();
				}
			}
			else
			{
				try
				{
					_0021QuickMixSession();
				}
				finally
				{
					base.Finalize();
				}
			}
		}

		public sealed override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~QuickMixSession()
		{
			Dispose(false);
		}
	}
}
