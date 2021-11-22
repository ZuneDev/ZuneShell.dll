using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service
{
	public class AlbumOfferCollection : OfferCollection, IDisposable
	{
		private IList m_items;

		private unsafe IMusicAlbumCollection* m_pCollection;

		public IList Items => m_items;

		internal unsafe AlbumOfferCollection()
		{
			//IL_0015: Expected I, but got I8
			m_items = null;
			m_pCollection = null;
		}

		private void _007EAlbumOfferCollection()
		{
			_0021AlbumOfferCollection();
		}

		private unsafe void _0021AlbumOfferCollection()
		{
			//IL_0017: Expected I, but got I8
			//IL_0020: Expected I, but got I8
			IMusicAlbumCollection* pCollection = m_pCollection;
			if (pCollection != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pCollection + 16)))((nint)pCollection);
				m_pCollection = null;
			}
			m_items = null;
		}

		internal unsafe int Init(IMusicAlbumCollection* pCollection, IDictionary mapIdToContext)
		{
			//IL_000f: Expected I, but got I8
			//IL_0044: Expected I, but got I8
			//IL_0073: Expected I, but got I8
			//IL_0073: Expected I, but got I8
			//IL_0091: Expected I, but got I8
			//IL_0091: Expected I, but got I8
			//IL_00ac: Expected I, but got I8
			//IL_00ac: Expected I, but got I8
			//IL_00c0: Expected I, but got I8
			//IL_00e7: Expected I, but got I8
			//IL_00e7: Expected I, but got I8
			//IL_0115: Expected I, but got I8
			//IL_0115: Expected I, but got I8
			//IL_017f: Expected I, but got I8
			//IL_018a: Expected I, but got I8
			//IL_0195: Expected I, but got I8
			//IL_01a2: Expected I, but got I8
			//IL_01aa: Expected I, but got I8
			//IL_022e: Expected I, but got I8
			int num = 0;
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pCollection + 24)))((nint)pCollection);
			IList list = new ArrayList(num2);
			int num3 = 0;
			if (0 < num2)
			{
				do
				{
					MusicAlbumMetadata musicAlbumMetadata;
					Module.MusicAlbumMetadata_002E_007Bctor_007D(&musicAlbumMetadata);
					try
					{
						CComPtrNtv_003CIContextData_003E cComPtrNtv_003CIContextData_003E;
						*(long*)(&cComPtrNtv_003CIContextData_003E) = 0L;
						try
						{
							if (num >= 0)
							{
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, MusicAlbumMetadata*, IContextData**, int>)(*(ulong*)(*(long*)pCollection + 32)))((nint)pCollection, num3, &musicAlbumMetadata, (IContextData**)(&cComPtrNtv_003CIContextData_003E));
							}
							CComPtrNtv_003CIPriceInfo_003E cComPtrNtv_003CIPriceInfo_003E;
							Module.CComPtrNtv_003CIPriceInfo_003E_002E_007Bctor_007D(&cComPtrNtv_003CIPriceInfo_003E);
							try
							{
								int releaseYear = 0;
								if (num >= 0)
								{
									long num4 = Unsafe.As<MusicAlbumMetadata, long>(ref Unsafe.AddByteOffset(ref musicAlbumMetadata, 88));
									int num5 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)Unsafe.As<MusicAlbumMetadata, ulong>(ref Unsafe.AddByteOffset(ref musicAlbumMetadata, 88)) + 144)))((nint)num4);
									long num6 = Unsafe.As<MusicAlbumMetadata, long>(ref Unsafe.AddByteOffset(ref musicAlbumMetadata, 88));
									int num7 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, int>)(*(ulong*)(*(long*)Unsafe.As<MusicAlbumMetadata, ulong>(ref Unsafe.AddByteOffset(ref musicAlbumMetadata, 88)) + 176)))((nint)num6, 1, 1);
									long num8 = Unsafe.As<MusicAlbumMetadata, long>(ref Unsafe.AddByteOffset(ref musicAlbumMetadata, 88));
									int num9 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, int>)(*(ulong*)(*(long*)Unsafe.As<MusicAlbumMetadata, ulong>(ref Unsafe.AddByteOffset(ref musicAlbumMetadata, 88)) + 72)))((nint)num8, (EMediaRights)4, 0);
									if (Unsafe.As<MusicAlbumMetadata, long>(ref Unsafe.AddByteOffset(ref musicAlbumMetadata, 64)) != 0L)
									{
										releaseYear = Module._wtoi((ushort*)Unsafe.As<MusicAlbumMetadata, ulong>(ref Unsafe.AddByteOffset(ref musicAlbumMetadata, 64)));
									}
									EMediaFormat eMediaFormat = ((num9 == 0) ? ((EMediaFormat)1) : 0);
									long num10 = Unsafe.As<MusicAlbumMetadata, long>(ref Unsafe.AddByteOffset(ref musicAlbumMetadata, 88));
									if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, int>)(*(ulong*)(*(long*)Unsafe.As<MusicAlbumMetadata, ulong>(ref Unsafe.AddByteOffset(ref musicAlbumMetadata, 88)) + 72)))((nint)num10, (EMediaRights)4, eMediaFormat) != 0)
									{
										EMediaFormat eMediaFormat2 = ((num9 == 0) ? ((EMediaFormat)1) : 0);
										long num11 = Unsafe.As<MusicAlbumMetadata, long>(ref Unsafe.AddByteOffset(ref musicAlbumMetadata, 88));
										IPriceInfo** intPtr = Module.CComPtrNtv_003CIPriceInfo_003E_002E_0026(&cComPtrNtv_003CIPriceInfo_003E);
										num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, IPriceInfo**, int>)(*(ulong*)(*(long*)Unsafe.As<MusicAlbumMetadata, ulong>(ref Unsafe.AddByteOffset(ref musicAlbumMetadata, 88)) + 104)))((nint)num11, (EMediaRights)4, eMediaFormat2, intPtr);
									}
									if (num >= 0)
									{
										Guid id = Unsafe.As<MusicAlbumMetadata, _GUID>(ref Unsafe.AddByteOffset(ref musicAlbumMetadata, 8));
										string recommendationContext = GetRecommendationContext(id, mapIdToContext, Module.CComPtrNtv_003CIContextData_003E_002E_002EPEAUIContextData_0040_0040(&cComPtrNtv_003CIContextData_003E));
										bool inCollection = ((num5 != 0) ? true : false);
										bool previouslyPurchased = ((num7 != 0) ? true : false);
										bool premium = ((Unsafe.As<MusicAlbumMetadata, int>(ref Unsafe.AddByteOffset(ref musicAlbumMetadata, 84)) != 0) ? true : false);
										bool isMP = ((num9 != 0) ? true : false);
										list.Add(new AlbumOffer(id, new string((char*)Unsafe.As<MusicAlbumMetadata, ulong>(ref Unsafe.AddByteOffset(ref musicAlbumMetadata, 40))), new string((char*)Unsafe.As<MusicAlbumMetadata, ulong>(ref Unsafe.AddByteOffset(ref musicAlbumMetadata, 48))), new string((char*)Unsafe.As<MusicAlbumMetadata, ulong>(ref Unsafe.AddByteOffset(ref musicAlbumMetadata, 56))), releaseYear, new string((char*)Unsafe.As<MusicAlbumMetadata, ulong>(ref Unsafe.AddByteOffset(ref musicAlbumMetadata, 72))), new PriceInfo((IPriceInfo*)(*(ulong*)(&cComPtrNtv_003CIPriceInfo_003E))), isMP, premium, previouslyPurchased, inCollection, recommendationContext));
									}
								}
							}
							catch
							{
								//try-fault
								Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPriceInfo_003E*, void>)(&Module.CComPtrNtv_003CIPriceInfo_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPriceInfo_003E);
								throw;
							}
							Module.CComPtrNtv_003CIPriceInfo_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIPriceInfo_003E);
						}
						catch
						{
							//try-fault
							Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIContextData_003E*, void>)(&Module.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIContextData_003E);
							throw;
						}
						Module.CComPtrNtv_003CIContextData_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIContextData_003E);
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<MusicAlbumMetadata*, void>)(&Module.MusicAlbumMetadata_002E_007Bdtor_007D), &musicAlbumMetadata);
						throw;
					}
					Module.MusicAlbumMetadata_002E_007Bdtor_007D(&musicAlbumMetadata);
					num3++;
				}
				while (num3 < num2);
				if (num < 0)
				{
					goto IL_022f;
				}
			}
			m_items = list;
			m_pCollection = pCollection;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pCollection + 8)))((nint)pCollection);
			goto IL_022f;
			IL_022f:
			return num;
		}

		internal unsafe IMusicAlbumCollection* GetCollection()
		{
			//IL_0016: Expected I, but got I8
			IMusicAlbumCollection* pCollection = m_pCollection;
			if (pCollection != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pCollection + 8)))((nint)pCollection);
			}
			return m_pCollection;
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				_0021AlbumOfferCollection();
				return;
			}
			try
			{
				_0021AlbumOfferCollection();
			}
			finally
			{
				base.Finalize();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~AlbumOfferCollection()
		{
			Dispose(false);
		}
	}
}
