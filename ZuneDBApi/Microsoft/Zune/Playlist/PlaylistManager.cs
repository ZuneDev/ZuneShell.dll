using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using ZuneUI;

namespace Microsoft.Zune.Playlist
{
	public class PlaylistManager : IDisposable
	{
		private unsafe IPlaylistManager* m_pPlaylistManager;

		private static PlaylistManager sm_instance = null;

		private static object sm_lock = new object();

		internal unsafe IPlaylistManager* NativePlaylistManager => m_pPlaylistManager;

		public static PlaylistManager Instance
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
							PlaylistManager playlistManager = new PlaylistManager();
							Thread.MemoryBarrier();
							sm_instance = playlistManager;
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

		private void _007EPlaylistManager()
		{
			_0021PlaylistManager();
		}

		private unsafe void _0021PlaylistManager()
		{
			//IL_0017: Expected I, but got I8
			//IL_0020: Expected I, but got I8
			IPlaylistManager* pPlaylistManager = m_pPlaylistManager;
			if (pPlaylistManager != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pPlaylistManager + 16)))((nint)pPlaylistManager);
				m_pPlaylistManager = null;
			}
		}

		public unsafe HRESULT CreatePlaylist(string title, string author, [In] ValueType serviceMediaId, CreatePlaylistOption options, out int playlistId)
		{
			playlistId = -1;
			bool flag = (options & CreatePlaylistOption.PrivatePlaylist) > CreatePlaylistOption.None;
			bool flag2 = (options & CreatePlaylistOption.AutoPlaylist) > CreatePlaylistOption.None;
			bool flag3 = (options & CreatePlaylistOption.SyncRule) > CreatePlaylistOption.None;
			bool flag4 = (options & CreatePlaylistOption.OverwriteOnConflict) > CreatePlaylistOption.None;
			bool flag5 = (options & CreatePlaylistOption.RenameOnConflict) > CreatePlaylistOption.None;
			int hr;
			if ((flag && (flag2 || flag3)) || (flag4 && flag5))
			{
				hr = -2147024809;
			}
			else if (m_pPlaylistManager == null)
			{
				hr = -2147467261;
			}
			else
			{
				fixed (char* titlePtr = title.ToCharArray())
				{
					ushort* ptr2 = (ushort*)titlePtr;
					try
					{
						fixed (char* authorPtr = author.ToCharArray())
						{
							ushort* ptr3 = (ushort*)authorPtr;
							try
							{
								EPlaylistCreateConflictAction ePlaylistCreateConflictAction = 0;
								if (flag5)
								{
									ePlaylistCreateConflictAction = (EPlaylistCreateConflictAction)2;
								}
								else if (flag || flag4)
								{
									ePlaylistCreateConflictAction = (EPlaylistCreateConflictAction)1;
								}
								_GUID gUID = new();
								if (serviceMediaId != null)
								{
									gUID = (Guid)serviceMediaId;
								}
								bool flag6 = ((!(flag3 || flag)) ? true : false);
								EPlaylistType ePlaylistType;
								if (flag)
								{
									ePlaylistType = (EPlaylistType)4;
								}
								else
								{
									EPlaylistType ePlaylistType2;
									if (flag3)
									{
										ePlaylistType2 = (EPlaylistType)3;
									}
									else
									{
										EPlaylistType ePlaylistType3 = (flag2 ? ((EPlaylistType)1) : 0);
										ePlaylistType2 = ePlaylistType3;
									}
									ePlaylistType = ePlaylistType2;
								}
								_GUID* ptr = (_GUID*)Unsafe.AsPointer(ref serviceMediaId != null ? ref gUID : ref *(_GUID*)null);
								long num = *(long*)m_pPlaylistManager + 24;
								IPlaylistManager* pPlaylistManager = m_pPlaylistManager;
								int num2;
								hr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, ushort*, _GUID*, int*, EPlaylistType, EPlaylistCreateConflictAction, int, VARIANT*, int>)(*(ulong*)num))((nint)pPlaylistManager, ptr2, ptr3, ptr, &num2, ePlaylistType, ePlaylistCreateConflictAction, flag6 ? 1 : 0, null);
								playlistId = num2;
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
			return new HRESULT(hr);
		}

		public unsafe HRESULT AddMediaToPlaylist(int playlistId, int cMediaCount, int[] mediaIds, int[] mediaTypeIds, int insertPos, [Out] int[] playlistContentIds)
		{
			//IL_0019: Incompatible stack types: I8 vs Ref
			//IL_002a: Incompatible stack types: I8 vs Ref
			//IL_003b: Incompatible stack types: I8 vs Ref
			//IL_0063: Expected I, but got I8
			int hr;
			if (m_pPlaylistManager == null)
			{
				hr = -2147467261;
			}
			else
			{
				fixed (int* ptr = &(mediaIds != null ? ref mediaIds[0] : ref *(int*)null))
				{
					try
					{
						fixed (int* ptr2 = &(mediaTypeIds != null ? ref mediaTypeIds[0] : ref *(int*)null))
						{
							try
							{
								fixed (int* ptr3 = &(playlistContentIds != null ? ref playlistContentIds[0] : ref *(int*)null))
								{
									try
									{
										long num = *(long*)m_pPlaylistManager + 40;
										IPlaylistManager* pPlaylistManager = m_pPlaylistManager;
										hr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, int*, int*, int, int*, int>)(*(ulong*)num))((nint)pPlaylistManager, playlistId, cMediaCount, ptr, ptr2, insertPos, ptr3);
									}
									catch
									{
										//try-fault
										playlistContentIds = null;
										throw;
									}
								}
							}
							catch
							{
								//try-fault
								mediaTypeIds = null;
								throw;
							}
						}
					}
					catch
					{
						//try-fault
						mediaIds = null;
						throw;
					}
				}
			}
			return new HRESULT(hr);
		}

		public unsafe void RemoveMediaFromPlaylist(int playlistId, int cIdCount, int[] playlistContentIds)
		{
			//IL_0011: Incompatible stack types: I8 vs Ref
			//IL_0033: Expected I, but got I8
			if (m_pPlaylistManager == null)
			{
				return;
			}
			fixed (int* ptr = &(playlistContentIds != null ? ref playlistContentIds[0] : ref *(int*)null))
			{
				try
				{
					long num = *(long*)m_pPlaylistManager + 48;
					IPlaylistManager* pPlaylistManager = m_pPlaylistManager;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, int*, int>)(*(ulong*)num))((nint)pPlaylistManager, playlistId, cIdCount, ptr);
				}
				catch
				{
					//try-fault
					playlistContentIds = null;
					throw;
				}
			}
		}

		public unsafe HRESULT UpdateMediaPositionInPlaylist(int playlistId, int cPlaylistContentCount, int[] playlistContentIds, int newPosition)
		{
			//IL_0019: Incompatible stack types: I8 vs Ref
			//IL_003d: Expected I, but got I8
			int hr;
			if (m_pPlaylistManager == null)
			{
				hr = -2147467261;
			}
			else
			{
				fixed (int* ptr = &(playlistContentIds != null ? ref playlistContentIds[0] : ref *(int*)null))
				{
					try
					{
						long num = *(long*)m_pPlaylistManager + 64;
						IPlaylistManager* pPlaylistManager = m_pPlaylistManager;
						hr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, int*, int, int>)(*(ulong*)num))((nint)pPlaylistManager, playlistId, cPlaylistContentCount, ptr, newPosition);
					}
					catch
					{
						//try-fault
						playlistContentIds = null;
						throw;
					}
				}
			}
			return new HRESULT(hr);
		}

		public unsafe void DedupPlaylist(int playlistId)
		{
			//IL_0018: Expected I, but got I8
			IPlaylistManager* pPlaylistManager = m_pPlaylistManager;
			if (pPlaylistManager != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)pPlaylistManager + 104)))((nint)pPlaylistManager, playlistId);
			}
		}

		public unsafe HRESULT RenamePlaylist(int playlistId, string newTitle)
		{
			//IL_0032: Expected I, but got I8
			int hr;
			if (m_pPlaylistManager == null)
			{
				hr = -2147467261;
			}
			else
			{
				fixed (char* newTitlePtr = newTitle.ToCharArray())
				{
					ushort* ptr = (ushort*)newTitlePtr;
					try
					{
						long num = *(long*)m_pPlaylistManager + 72;
						IPlaylistManager* pPlaylistManager = m_pPlaylistManager;
						hr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, ushort*, int>)(*(ulong*)num))((nint)pPlaylistManager, playlistId, ptr);
					}
					catch
					{
						//try-fault
						ptr = null;
						throw;
					}
				}
			}
			return new HRESULT(hr);
		}

		public unsafe HRESULT SavePlaylistAsStatic(int playlistId, PlaylistAsyncOperationCompleted playlistAsyncOperationCompleted)
		{
			//IL_002b: Expected I, but got I8
			if (m_pPlaylistManager == null)
			{
				return -2147467261;
			}
			PlaylistAsyncOperation* ptr = (PlaylistAsyncOperation*)Module.@new(24uL);
			PlaylistAsyncOperation* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : Module.Microsoft_002EZune_002EPlaylist_002EPlaylistAsyncOperation_002E_007Bctor_007D(ptr));
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
				num2 = Module.Microsoft_002EZune_002EPlaylist_002EPlaylistAsyncOperation_002EAsyncSavePlaylistAsStatic(ptr2, playlistId, m_pPlaylistManager, playlistAsyncOperationCompleted);
				if (num2 >= 0)
				{
					goto done;
				}
			}
			if (ptr2 != null)
			{
				Module.Microsoft_002EZune_002EPlaylist_002EPlaylistAsyncOperation_002E__delDtor(ptr2, 1u);
			}
			goto done;
			done:
			return new HRESULT(num2);
		}

		public unsafe string GetUniquePlaylistTitle(string candidateTitle)
		{
			//IL_0019: Expected I4, but got I8
			//IL_003a: Expected I, but got I8
			//IL_0045: Expected I, but got I8
			string result = candidateTitle;
			if (m_pPlaylistManager != null)
			{
				fixed (char* candidateTitlePtr = candidateTitle.ToCharArray())
				{
					ushort* ptr = (ushort*)candidateTitlePtr;
					try
					{
                        PROPVARIANT cComPropVariant;
                        // IL initblk instruction
                        Unsafe.InitBlock(&cComPropVariant, 0, 24);
						try
						{
							long num = *(long*)m_pPlaylistManager + 136;
							IPlaylistManager* pPlaylistManager = m_pPlaylistManager;
							int num2;
							((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort*, int*, PROPVARIANT, int>)(*(ulong*)num))((nint)pPlaylistManager, ptr, &num2, cComPropVariant);
							result = new string((char*)Unsafe.As<PROPVARIANT, ulong>(ref Unsafe.AddByteOffset(ref cComPropVariant, 8)));
						}
						catch
						{
                            //try-fault
                            Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<PROPVARIANT*, void>)(&Module.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
							throw;
						}
						cComPropVariant.Clear();
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

		public unsafe HRESULT GetPlaylistByServiceMediaId(Guid serviceMediaId, out int playlistId)
		{
			//IL_0034: Expected I, but got I8
			playlistId = -1;
			int hr;
			if (m_pPlaylistManager == null)
			{
				hr = -2147467261;
			}
			else
			{
				int num = -1;
				_GUID gUID = serviceMediaId;
				IPlaylistManager* pPlaylistManager = m_pPlaylistManager;
				hr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, _GUID, int*, int>)(*(ulong*)(*(long*)pPlaylistManager + 168)))((nint)pPlaylistManager, gUID, &num);
				playlistId = num;
			}
			return new HRESULT(hr);
		}

		public unsafe HRESULT GetAutoPlaylistSchema(int playlistId, out EMediaTypes schema)
		{
			schema = EMediaTypes.eMediaTypeInvalid;
			EMediaTypes eMediaTypes = EMediaTypes.eMediaTypeInvalid;
			int num;
			if (playlistId < 0)
			{
				num = -2147418113;
			}
			else
			{
				IPlaylistManager* pPlaylistManager = m_pPlaylistManager;
				if (pPlaylistManager == null)
				{
					num = -2147467261;
				}
				else
				{
					num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, EMediaTypes*, int>)(*(ulong*)(*(long*)pPlaylistManager + 144)))((nint)pPlaylistManager, playlistId, &eMediaTypes);
					if (num >= 0)
					{
						schema = eMediaTypes;
					}
				}
			}
			return new HRESULT(num);
		}

		public unsafe HRESULT EnableAutomaticRefresh(int playlistId)
		{
			//IL_0023: Expected I, but got I8
			IPlaylistManager* pPlaylistManager = m_pPlaylistManager;
			int hr = ((pPlaylistManager != null) ? ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)pPlaylistManager + 152)))((nint)pPlaylistManager, playlistId) : (-2147467261));
			return new HRESULT(hr);
		}

		public unsafe HRESULT DisableAutomaticRefresh(int playlistId)
		{
			//IL_0023: Expected I, but got I8
			IPlaylistManager* pPlaylistManager = m_pPlaylistManager;
			int hr = ((pPlaylistManager != null) ? ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)pPlaylistManager + 160)))((nint)pPlaylistManager, playlistId) : (-2147467261));
			return new HRESULT(hr);
		}

		public unsafe HRESULT RefreshAutoPlaylist(int playlistId)
		{
			//IL_0020: Expected I, but got I8
			IPlaylistManager* pPlaylistManager = m_pPlaylistManager;
			int hr = ((pPlaylistManager != null) ? ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int>)(*(ulong*)(*(long*)pPlaylistManager + 88)))((nint)pPlaylistManager, playlistId) : (-2147467261));
			return new HRESULT(hr);
		}

		private unsafe PlaylistManager()
		{
			//IL_0009: Expected I, but got I8
			IPlaylistManager* pPlaylistManager = null;
			int singleton = Module.GetSingleton(Module.GUID_IPlaylistManager, (void**)(&pPlaylistManager));
			if (singleton >= 0)
			{
				m_pPlaylistManager = pPlaylistManager;
				return;
			}
			throw new ApplicationException(Module.GetErrorDescription(singleton));
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_0021PlaylistManager();
				return;
			}
			try
			{
				_0021PlaylistManager();
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

		~PlaylistManager()
		{
			Dispose(false);
		}
	}
}
