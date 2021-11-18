using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service
{
	public class TrackOfferCollection : OfferCollection, IDisposable
	{
		private IList m_items;

		private unsafe IMusicTrackCollection* m_pCollection;

		public IList Items => m_items;

		internal unsafe TrackOfferCollection()
			: this()
		{
			//IL_0015: Expected I, but got I8
			m_items = null;
			m_pCollection = null;
		}

		private void _007ETrackOfferCollection()
		{
			_0021TrackOfferCollection();
		}

		private unsafe void _0021TrackOfferCollection()
		{
			//IL_0017: Expected I, but got I8
			//IL_0020: Expected I, but got I8
			IMusicTrackCollection* pCollection = m_pCollection;
			if (pCollection != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pCollection + 16)))((nint)pCollection);
				m_pCollection = null;
			}
			m_items = null;
		}

		internal unsafe int Init(IMusicTrackCollection* pCollection, IDictionary mapIdToContext)
		{
			//IL_000f: Expected I, but got I8
			//IL_0044: Expected I, but got I8
			//IL_0076: Expected I, but got I8
			//IL_0076: Expected I, but got I8
			//IL_009f: Expected I, but got I8
			//IL_009f: Expected I, but got I8
			//IL_00c3: Expected I, but got I8
			//IL_00c3: Expected I, but got I8
			//IL_00e6: Expected I, but got I8
			//IL_00e6: Expected I, but got I8
			//IL_011a: Expected I, but got I8
			//IL_011a: Expected I, but got I8
			//IL_014b: Expected I, but got I8
			//IL_014b: Expected I, but got I8
			//IL_0171: Expected I, but got I8
			//IL_0193: Expected I, but got I8
			//IL_0193: Expected I, but got I8
			//IL_01d1: Expected I, but got I8
			//IL_01dc: Expected I, but got I8
			//IL_01e7: Expected I, but got I8
			//IL_01f1: Expected I, but got I8
			//IL_0273: Expected I, but got I8
			int num = 0;
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pCollection + 24)))((nint)pCollection);
			IList list = new ArrayList(num2);
			int num3 = 0;
			if (0 < num2)
			{
				do
				{
					MusicTrackMetadata musicTrackMetadata;
					Module.MusicTrackMetadata_002E_007Bctor_007D(&musicTrackMetadata);
					try
					{
						CComPtrNtv_003CIContextData_003E cComPtrNtv_003CIContextData_003E;
						*(long*)(&cComPtrNtv_003CIContextData_003E) = 0L;
						try
						{
							if (num >= 0)
							{
								num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, MusicTrackMetadata*, IContextData**, int>)(*(ulong*)(*(long*)pCollection + 32)))((nint)pCollection, num3, &musicTrackMetadata, (IContextData**)(&cComPtrNtv_003CIContextData_003E));
							}
							CComPtrNtv_003CIPriceInfo_003E cComPtrNtv_003CIPriceInfo_003E;
							Module.CComPtrNtv_003CIPriceInfo_003E_002E_007Bctor_007D(&cComPtrNtv_003CIPriceInfo_003E);
							try
							{
								if (num >= 0)
								{
									long num4 = Unsafe.As<MusicTrackMetadata, long>(ref Unsafe.AddByteOffset(ref musicTrackMetadata, 152));
									EMediaRights eMediaRights = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)Unsafe.As<MusicTrackMetadata, ulong>(ref Unsafe.AddByteOffset(ref musicTrackMetadata, 152)) + 208)))((nint)num4) != 0) ? ((EMediaRights)5) : ((EMediaRights)3));
									long num5 = Unsafe.As<MusicTrackMetadata, long>(ref Unsafe.AddByteOffset(ref musicTrackMetadata, 152));
									int num6 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)Unsafe.As<MusicTrackMetadata, ulong>(ref Unsafe.AddByteOffset(ref musicTrackMetadata, 152)) + 144)))((nint)num5);
									long num7 = Unsafe.As<MusicTrackMetadata, long>(ref Unsafe.AddByteOffset(ref musicTrackMetadata, 152));
									int num8 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, int, int>)(*(ulong*)(*(long*)Unsafe.As<MusicTrackMetadata, ulong>(ref Unsafe.AddByteOffset(ref musicTrackMetadata, 152)) + 176)))((nint)num7, 1, 1);
									long num9 = Unsafe.As<MusicTrackMetadata, long>(ref Unsafe.AddByteOffset(ref musicTrackMetadata, 152));
									int num10 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, int>)(*(ulong*)(*(long*)Unsafe.As<MusicTrackMetadata, ulong>(ref Unsafe.AddByteOffset(ref musicTrackMetadata, 152)) + 72)))((nint)num9, eMediaRights, (EMediaFormat)0);
									if (eMediaRights != (EMediaRights)5)
									{
										EMediaFormat eMediaFormat = ((num10 == 0) ? ((EMediaFormat)1) : ((EMediaFormat)0));
										long num11 = Unsafe.As<MusicTrackMetadata, long>(ref Unsafe.AddByteOffset(ref musicTrackMetadata, 152));
										if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, int>)(*(ulong*)(*(long*)Unsafe.As<MusicTrackMetadata, ulong>(ref Unsafe.AddByteOffset(ref musicTrackMetadata, 152)) + 72)))((nint)num11, eMediaRights, eMediaFormat) != 0)
										{
											EMediaFormat eMediaFormat2 = ((num10 == 0) ? ((EMediaFormat)1) : ((EMediaFormat)0));
											long num12 = Unsafe.As<MusicTrackMetadata, long>(ref Unsafe.AddByteOffset(ref musicTrackMetadata, 152));
											num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, IPriceInfo**, int>)(*(ulong*)(*(long*)Unsafe.As<MusicTrackMetadata, ulong>(ref Unsafe.AddByteOffset(ref musicTrackMetadata, 152)) + 104)))((nint)num12, eMediaRights, eMediaFormat2, (IPriceInfo**)(&cComPtrNtv_003CIPriceInfo_003E));
										}
									}
									if (num >= 0)
									{
										Guid id = Module.GUIDToGuid(Unsafe.As<MusicTrackMetadata, _GUID>(ref Unsafe.AddByteOffset(ref musicTrackMetadata, 8)));
										string recommendationContext = GetRecommendationContext(id, mapIdToContext, (IContextData*)(*(ulong*)(&cComPtrNtv_003CIContextData_003E)));
										long num13 = Unsafe.As<MusicTrackMetadata, long>(ref Unsafe.AddByteOffset(ref musicTrackMetadata, 152));
										bool subscriptionFree = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)Unsafe.As<MusicTrackMetadata, ulong>(ref Unsafe.AddByteOffset(ref musicTrackMetadata, 152)) + 208)))((nint)num13) != 0) ? true : false);
										bool inCollection = ((num6 != 0) ? true : false);
										bool previouslyPurchased = ((num8 != 0) ? true : false);
										bool isMP = ((num10 != 0) ? true : false);
										list.Add(new TrackOffer(id, Unsafe.As<MusicTrackMetadata, int>(ref Unsafe.AddByteOffset(ref musicTrackMetadata, 72)), new string((char*)Unsafe.As<MusicTrackMetadata, ulong>(ref Unsafe.AddByteOffset(ref musicTrackMetadata, 80))), new string((char*)Unsafe.As<MusicTrackMetadata, ulong>(ref Unsafe.AddByteOffset(ref musicTrackMetadata, 88))), new string((char*)Unsafe.As<MusicTrackMetadata, ulong>(ref Unsafe.AddByteOffset(ref musicTrackMetadata, 96))), recommendationContext, new PriceInfo((IPriceInfo*)(*(ulong*)(&cComPtrNtv_003CIPriceInfo_003E))), isMP, previouslyPurchased, inCollection, subscriptionFree));
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
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<MusicTrackMetadata*, void>)(&Module.MusicTrackMetadata_002E_007Bdtor_007D), &musicTrackMetadata);
						throw;
					}
					Module.MusicTrackMetadata_002E_007Bdtor_007D(&musicTrackMetadata);
					num3++;
				}
				while (num3 < num2);
				if (num < 0)
				{
					goto IL_0274;
				}
			}
			m_items = list;
			m_pCollection = pCollection;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pCollection + 8)))((nint)pCollection);
			goto IL_0274;
			IL_0274:
			return num;
		}

		internal unsafe IMusicTrackCollection* GetCollection()
		{
			//IL_0016: Expected I, but got I8
			IMusicTrackCollection* pCollection = m_pCollection;
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
				_0021TrackOfferCollection();
				return;
			}
			try
			{
				_0021TrackOfferCollection();
			}
			finally
			{
				base.Finalize();
			}
		}

		public sealed override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~TrackOfferCollection()
		{
			Dispose(false);
		}
	}
}
