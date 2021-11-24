using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace Microsoft.Zune.Service
{
	public class VideoOfferCollection : OfferCollection, IDisposable
	{
		private IList m_items;

		private unsafe IVideoCollection* m_pCollection;

		public IList Items => m_items;

		internal unsafe VideoOfferCollection() : base()
		{
			//IL_0015: Expected I, but got I8
			m_items = null;
			m_pCollection = null;
		}

		private void _007EVideoOfferCollection()
		{
			_0021VideoOfferCollection();
		}

		private unsafe void _0021VideoOfferCollection()
		{
			//IL_0017: Expected I, but got I8
			//IL_0020: Expected I, but got I8
			IVideoCollection* pCollection = m_pCollection;
			if (pCollection != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pCollection + 16)))((nint)pCollection);
				m_pCollection = null;
			}
			m_items = null;
		}

		internal unsafe int Init(IVideoCollection* pCollection)
		{
			//IL_0010: Expected I, but got I8
			//IL_0057: Expected I, but got I8
			//IL_0057: Expected I, but got I8
			//IL_008f: Expected I, but got I8
			//IL_009c: Expected I, but got I8
			//IL_00a9: Expected I, but got I8
			//IL_00b6: Expected I, but got I8
			//IL_00c6: Expected I, but got I8
			//IL_00d3: Expected I, but got I8
			//IL_00f5: Expected I, but got I8
			//IL_00f5: Expected I, but got I8
			//IL_011a: Expected I, but got I8
			//IL_02e5: Expected I, but got I8
			//IL_02e5: Expected I, but got I8
			//IL_02f6: Expected I, but got I8
			//IL_0347: Expected I, but got I8
			//IL_0347: Expected I, but got I8
			//IL_0378: Expected I, but got I8
			//IL_0378: Expected I, but got I8
			//IL_03f3: Expected I, but got I8
			//IL_03f3: Expected I, but got I8
			//IL_046b: Expected I, but got I8
			//IL_046b: Expected I, but got I8
			//IL_0485: Incompatible stack types: Ref vs I
			//IL_050e: Expected I, but got I8
			//IL_050e: Expected I, but got I8
			//IL_0547: Expected I, but got I8
			//IL_0547: Expected I, but got I8
			//IL_0595: Expected I, but got I8
			//IL_0595: Expected I, but got I8
			//IL_062a: Expected I, but got I8
			//IL_0679: Expected I, but got I8
			int num = 0;
			int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pCollection + 24)))((nint)pCollection);
			IList list = new ArrayList(num2);
			int num3 = 0;
			if (0 < num2)
			{
				do
				{
					int num4 = 0;
					int num5 = 0;
					int num6 = 0;
					int num7 = 0;
					VideoMetadata videoMetadata;
					Module.VideoMetadata_002E_007Bctor_007D(&videoMetadata);
					try
					{
						if (num >= 0)
						{
							num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, VideoMetadata*, IContextData**, int>)(*(ulong*)(*(long*)pCollection + 32)))((nint)pCollection, num3, &videoMetadata, null);
							if (num >= 0)
							{
								Guid id = Unsafe.As<VideoMetadata, _GUID>(ref Unsafe.AddByteOffset(ref videoMetadata, 8));
								Guid albumId = Unsafe.As<VideoMetadata, _GUID>(ref Unsafe.AddByteOffset(ref videoMetadata, 80));
								string title = new string((char*)Unsafe.As<VideoMetadata, ulong>(ref Unsafe.AddByteOffset(ref videoMetadata, 32)));
								string seriesTitle = new string((char*)Unsafe.As<VideoMetadata, ulong>(ref Unsafe.AddByteOffset(ref videoMetadata, 120)));
								string artist = new string((char*)Unsafe.As<VideoMetadata, ulong>(ref Unsafe.AddByteOffset(ref videoMetadata, 48)));
								string genre = new string((char*)Unsafe.As<VideoMetadata, ulong>(ref Unsafe.AddByteOffset(ref videoMetadata, 96)));
								string previewImageUrl = new string((char*)Unsafe.As<VideoMetadata, ulong>(ref Unsafe.AddByteOffset(ref videoMetadata, 160)));
								string productionCompany = new string((char*)Unsafe.As<VideoMetadata, ulong>(ref Unsafe.AddByteOffset(ref videoMetadata, 112)));
								long num8 = Unsafe.As<VideoMetadata, long>(ref Unsafe.AddByteOffset(ref videoMetadata, 192));
								bool flag = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)Unsafe.As<VideoMetadata, ulong>(ref Unsafe.AddByteOffset(ref videoMetadata, 192)) + 144)))((nint)num8) != 0) ? true : false);
								int releaseYear = 0;
								if (Unsafe.As<VideoMetadata, long>(ref Unsafe.AddByteOffset(ref videoMetadata, 144)) != 0L)
								{
									releaseYear = Module._wtoi((ushort*)Unsafe.As<VideoMetadata, ulong>(ref Unsafe.AddByteOffset(ref videoMetadata, 144)));
								}
								_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040 _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040;
								*(int*)(&_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040) = 3;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 4)) = 3;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 8)) = 4;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 12)) = 3;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 16)) = 2;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 20)) = 4;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 24)) = 3;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 28)) = 4;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 32)) = -1;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 36)) = 7;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 40)) = 2;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 44)) = -1;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 48)) = 7;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 52)) = 3;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 56)) = -1;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 60)) = 8;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 64)) = 2;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 68)) = -1;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 72)) = 8;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 76)) = 3;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 80)) = -1;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 84)) = 9;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 88)) = 3;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 92)) = -1;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 96)) = 9;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 100)) = 2;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 104)) = -1;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 108)) = 12;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 112)) = 2;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 116)) = 4;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 120)) = 12;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 124)) = 3;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 128)) = 4;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 132)) = 13;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 136)) = 2;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 140)) = -1;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 144)) = 13;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 148)) = 3;
                                Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 152)) = -1;
								int num9 = 0;
								VideoOfferParams* ptr = (VideoOfferParams*)(&_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040);
								long num10 = (nint)Unsafe.AsPointer(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 4));
								long num11 = (nint)Unsafe.AsPointer(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 8));
								while (num6 == 0)
								{
									int num12 = *(int*)ptr;
									if ((num12 != 8 && num12 != 13) || *(int*)num10 != 3 || *(int*)num11 != -1 || num7 == 0)
									{
										if (num12 == 3)
										{
											if (*(int*)num10 == 4 && *(int*)num11 == -1 && num4 != 0)
											{
												goto IL_0610;
											}
										}
										else if (num12 != 12)
										{
											goto IL_02a8;
										}
										if (*(int*)num10 != 3 || *(int*)num11 != 4 || num5 == 0)
										{
											goto IL_02a8;
										}
									}
									goto IL_0610;
									IL_042f:
									bool flag2;
									bool flag3;
									bool flag4;
									int num14;
									bool flag6;
									ushort* ptr2;
									CComPtrNtv<IPriceInfo> cComPtrNtv_003CIPriceInfo_003E;
									bool flag7;
									bool isRental;
									bool isMusicVideo;
									bool flag8;
									bool flag9;
									try
									{
										if (!flag2 && !flag3)
										{
											EMediaRights eMediaRights = (flag4 ? ((EMediaRights)13) : ((EMediaRights)12));
											long num13 = Unsafe.As<VideoMetadata, long>(ref Unsafe.AddByteOffset(ref videoMetadata, 192));
											bool flag5 = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, int, int, int>)(*(ulong*)(*(long*)Unsafe.As<VideoMetadata, ulong>(ref Unsafe.AddByteOffset(ref videoMetadata, 192)) + 168)))((nint)num13, eMediaRights, (EMediaFormat)num14, 1, 1) != 0) ? true : false);
											flag6 = flag5;
										}
										string expirationDate = new string((char*)Unsafe.AsPointer(ref (long)(nint)ptr2 != 0 ? ref *(_0024ArrayType_0024_0024_0024BY00_0024_0024CBG*)ptr2 : ref Module._003F_003F_C_0040_11LOCGONAA_0040_003F_0024AA_003F_0024AA_0040));
										VideoOffer value = new VideoOffer(id, title, seriesTitle, Unsafe.As<VideoMetadata, int>(ref Unsafe.AddByteOffset(ref videoMetadata, 128)), Unsafe.As<VideoMetadata, int>(ref Unsafe.AddByteOffset(ref videoMetadata, 132)), artist, albumId, genre, releaseYear, previewImageUrl, productionCompany, new PriceInfo(Module.CComPtrNtv_003CIPriceInfo_003E_002E_002EPEAUIPriceInfo_0040_0040(cComPtrNtv_003CIPriceInfo_003E.p)), flag7, isRental, flag4, isMusicVideo, flag3, flag6, flag, expirationDate);
										list.Add(value);
										num4 = 1;
										if (!flag8 || !flag9 || !flag6)
										{
											goto IL_051c;
										}
										long num15 = Unsafe.As<VideoMetadata, long>(ref Unsafe.AddByteOffset(ref videoMetadata, 192));
										if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, EMediaFormat, int>)(*(ulong*)(*(long*)Unsafe.As<VideoMetadata, ulong>(ref Unsafe.AddByteOffset(ref videoMetadata, 192)) + 64)))((nint)num15, (EMediaRights)num12, (EMediaFormat)3, (EMediaFormat)4) == 0)
										{
											goto IL_051c;
										}
										num6 = (flag ? 1 : 0);
										num5 = 1;
										goto end_IL_0430;
										IL_051c:
										if (flag7 && flag4 && flag6)
										{
											long num16 = Unsafe.As<VideoMetadata, long>(ref Unsafe.AddByteOffset(ref videoMetadata, 192));
											num7 = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, int>)(*(ulong*)(*(long*)Unsafe.As<VideoMetadata, ulong>(ref Unsafe.AddByteOffset(ref videoMetadata, 192)) + 72)))((nint)num16, (EMediaRights)num12, (EMediaFormat)2) != 0) ? 1 : num7);
										}
										if (num5 == 0 && flag7 && flag9 && flag6)
										{
											long num17 = Unsafe.As<VideoMetadata, long>(ref Unsafe.AddByteOffset(ref videoMetadata, 192));
											if (((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, EMediaFormat, int>)(*(ulong*)(*(long*)Unsafe.As<VideoMetadata, ulong>(ref Unsafe.AddByteOffset(ref videoMetadata, 192)) + 64)))((nint)num17, (EMediaRights)num12, (EMediaFormat)2, (EMediaFormat)3) != 0)
											{
												VideoOffer value2 = new VideoOffer(id, title, seriesTitle, Unsafe.As<VideoMetadata, int>(ref Unsafe.AddByteOffset(ref videoMetadata, 128)), Unsafe.As<VideoMetadata, int>(ref Unsafe.AddByteOffset(ref videoMetadata, 132)), artist, albumId, genre, releaseYear, previewImageUrl, productionCompany, PriceInfo.FreeWithPoints(), isHD: false, isRental: false, isStream: false, isMusicVideo: false, flag3, previouslyPurchased: true, flag, expirationDate);
												list.Add(value2);
												num5 = 1;
											}
										}
										end_IL_0430:;
									}
									catch
									{
										//try-fault
										Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<IPriceInfo*, void>)(&Module.CComPtrNtv_003CIPriceInfo_003E_002E_007Bdtor_007D), cComPtrNtv_003CIPriceInfo_003E.p);
										throw;
									}
									goto IL_05f1;
									IL_0423:
									Module.CComPtrNtv_003CIPriceInfo_003E_002E_007Bdtor_007D(cComPtrNtv_003CIPriceInfo_003E.p);
									goto IL_0610;
									IL_0610:
									num9++;
									num11 += 12;
									num10 += 12;
									ptr = (VideoOfferParams*)((ulong)(nint)ptr + 12uL);
									if ((uint)num9 >= 13u)
									{
										break;
									}
									continue;
									IL_02a8:
									num14 = *(int*)num10;
									flag7 = num14 == 2;
									flag8 = num14 == 3;
									ESeasonPurchaseFlags eSeasonPurchaseFlags = ((!flag7) ? ((ESeasonPurchaseFlags)1) : ((ESeasonPurchaseFlags)2));
									long num18 = Unsafe.As<VideoMetadata, long>(ref Unsafe.AddByteOffset(ref videoMetadata, 192));
									flag3 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ESeasonPurchaseFlags, int>)(*(ulong*)(*(long*)Unsafe.As<VideoMetadata, ulong>(ref Unsafe.AddByteOffset(ref videoMetadata, 192)) + 224)))((nint)num18, eSeasonPurchaseFlags) == 1;
									Module.CComPtrNtv_003CIPriceInfo_003E_002E_007Bctor_007D(cComPtrNtv_003CIPriceInfo_003E.p);
									try
									{
										ptr2 = null;
										_GUID gUID_NULL = Module.GUID_NULL;
										_GUID gUID_NULL2 = Module.GUID_NULL;
										_GUID gUID_NULL3 = Module.GUID_NULL;
										int num19 = *(int*)num11;
										int num21;
										if (num19 != -1)
										{
											long num20 = Unsafe.As<VideoMetadata, long>(ref Unsafe.AddByteOffset(ref videoMetadata, 192));
											IPriceInfo** intPtr = Module.CComPtrNtv_003CIPriceInfo_003E_002E_0026(cComPtrNtv_003CIPriceInfo_003E.p);
											num21 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, EMediaFormat, _GUID*, _GUID*, _GUID*, IPriceInfo**, ushort**, int>)(*(ulong*)(*(long*)Unsafe.As<VideoMetadata, ulong>(ref Unsafe.AddByteOffset(ref videoMetadata, 192)) + 128)))((nint)num20, (EMediaRights)num12, (EMediaFormat)num14, (EMediaFormat)num19, &gUID_NULL2, &gUID_NULL3, &gUID_NULL, intPtr, &ptr2);
										}
										else
										{
											long num22 = Unsafe.As<VideoMetadata, long>(ref Unsafe.AddByteOffset(ref videoMetadata, 192));
											IPriceInfo** intPtr2 = Module.CComPtrNtv_003CIPriceInfo_003E_002E_0026(cComPtrNtv_003CIPriceInfo_003E.p);
											num21 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, _GUID*, _GUID*, IPriceInfo**, ushort**, int>)(*(ulong*)(*(long*)Unsafe.As<VideoMetadata, ulong>(ref Unsafe.AddByteOffset(ref videoMetadata, 192)) + 136)))((nint)num22, (EMediaRights)num12, (EMediaFormat)num14, &gUID_NULL2, &gUID_NULL, intPtr2, &ptr2);
										}
										if (num21 >= 0)
										{
											flag9 = ((num12 == 3 || num12 == 12) ? true : false);
											isRental = ((num12 == 7 || num12 == 9) ? true : false);
											flag4 = ((num12 == 8 || num12 == 13 || num12 == 9) ? true : false);
											int num23 = ((num12 == 12 || num12 == 13) ? 1 : 0);
											bool flag10 = (byte)num23 != 0;
											long num24 = Unsafe.As<VideoMetadata, long>(ref Unsafe.AddByteOffset(ref videoMetadata, 192));
											flag2 = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, int, int, int>)(*(ulong*)(*(long*)Unsafe.As<VideoMetadata, ulong>(ref Unsafe.AddByteOffset(ref videoMetadata, 192)) + 168)))((nint)num24, (EMediaRights)num12, (EMediaFormat)num14, 1, 1) != 0) ? true : false);
											flag6 = flag2;
											isMusicVideo = Unsafe.As<VideoMetadata, int>(ref Unsafe.AddByteOffset(ref videoMetadata, 24)) == 0;
											if (flag3 == flag10)
											{
												goto IL_042f;
											}
											goto IL_0423;
										}
									}
									catch
									{
										//try-fault
										Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<IPriceInfo*, void>)(&Module.CComPtrNtv_003CIPriceInfo_003E_002E_007Bdtor_007D), cComPtrNtv_003CIPriceInfo_003E.p);
										throw;
									}
									goto IL_05f1;
									IL_05f1:
									try
									{
										Module.SysFreeString(ptr2);
									}
									catch
									{
										//try-fault
										Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<IPriceInfo*, void>)(&Module.CComPtrNtv_003CIPriceInfo_003E_002E_007Bdtor_007D), cComPtrNtv_003CIPriceInfo_003E.p);
										throw;
									}
									Module.CComPtrNtv_003CIPriceInfo_003E_002E_007Bdtor_007D(cComPtrNtv_003CIPriceInfo_003E.p);
									goto IL_0610;
								}
							}
						}
					}
					catch
					{
						//try-fault
						Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<VideoMetadata*, void>)(&Module.VideoMetadata_002E_007Bdtor_007D), &videoMetadata);
						throw;
					}
					Module.VideoMetadata_002E_007Bdtor_007D(&videoMetadata);
					num3++;
				}
				while (num3 < num2);
				if (num < 0)
				{
					goto IL_067a;
				}
			}
			m_items = list;
			m_pCollection = pCollection;
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pCollection + 8)))((nint)pCollection);
			goto IL_067a;
			IL_067a:
			return num;
		}

		internal unsafe IVideoCollection* GetCollection()
		{
			//IL_0016: Expected I, but got I8
			IVideoCollection* pCollection = m_pCollection;
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
				_0021VideoOfferCollection();
				return;
			}
			try
			{
				_0021VideoOfferCollection();
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

		~VideoOfferCollection()
		{
			Dispose(false);
		}
	}
}
