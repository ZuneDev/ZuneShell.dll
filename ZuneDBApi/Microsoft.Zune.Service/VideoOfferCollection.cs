using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using _003CCppImplementationDetails_003E;

namespace Microsoft.Zune.Service;

public class VideoOfferCollection : OfferCollection, IDisposable
{
	private IList m_items;

	private unsafe IVideoCollection* m_pCollection;

	public IList Items => m_items;

	internal unsafe VideoOfferCollection()
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
		//IL_0679: Expected I, but got I8
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
		//IL_062a: Expected I, but got I8
		//IL_02e5: Expected I, but got I8
		//IL_02e5: Expected I, but got I8
		//IL_02f6: Expected I, but got I8
		//IL_0378: Expected I, but got I8
		//IL_0378: Expected I, but got I8
		//IL_0347: Expected I, but got I8
		//IL_0347: Expected I, but got I8
		//IL_03f3: Expected I, but got I8
		//IL_03f3: Expected I, but got I8
		//IL_046b: Expected I, but got I8
		//IL_046b: Expected I, but got I8
		//IL_050e: Expected I, but got I8
		//IL_050e: Expected I, but got I8
		//IL_0547: Expected I, but got I8
		//IL_0547: Expected I, but got I8
		//IL_0595: Expected I, but got I8
		//IL_0595: Expected I, but got I8
		//IL_0487->IL0487: Incompatible stack types: I vs Ref
		int num = 0;
		int num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pCollection + 24)))((nint)pCollection);
		IList list = new ArrayList(num2);
		int num3 = 0;
		if (0 < num2)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out VideoMetadata videoMetadata);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040 _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPtrNtv_003CIPriceInfo_003E cComPtrNtv_003CIPriceInfo_003E);
			do
			{
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				global::_003CModule_003E.VideoMetadata_002E_007Bctor_007D(&videoMetadata);
				try
				{
					if (num >= 0)
					{
						num = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int, VideoMetadata*, IContextData**, int>)(*(ulong*)(*(long*)pCollection + 32)))((nint)pCollection, num3, &videoMetadata, null);
						if (num >= 0)
						{
							Guid id = global::_003CModule_003E.GUIDToGuid(System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, _GUID>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 8)));
							Guid albumId = global::_003CModule_003E.GUIDToGuid(System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, _GUID>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 80)));
							string title = new string((char*)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 32)));
							string seriesTitle = new string((char*)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 120)));
							string artist = new string((char*)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 48)));
							string genre = new string((char*)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 96)));
							string previewImageUrl = new string((char*)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 160)));
							string productionCompany = new string((char*)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 112)));
							bool flag = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 192)) + 144)))((nint)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 192))) != 0) ? true : false);
							int releaseYear = 0;
							if (System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 144)) != 0L)
							{
								releaseYear = global::_003CModule_003E._wtoi((ushort*)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 144)));
							}
							*(int*)(&_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040) = 3;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 4)) = 3;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 8)) = 4;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 12)) = 3;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 16)) = 2;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 20)) = 4;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 24)) = 3;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 28)) = 4;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 32)) = -1;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 36)) = 7;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 40)) = 2;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 44)) = -1;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 48)) = 7;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 52)) = 3;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 56)) = -1;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 60)) = 8;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 64)) = 2;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 68)) = -1;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 72)) = 8;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 76)) = 3;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 80)) = -1;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 84)) = 9;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 88)) = 3;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 92)) = -1;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 96)) = 9;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 100)) = 2;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 104)) = -1;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 108)) = 12;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 112)) = 2;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 116)) = 4;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 120)) = 12;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 124)) = 3;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 128)) = 4;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 132)) = 13;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 136)) = 2;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 140)) = -1;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 144)) = 13;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 148)) = 3;
							System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 152)) = -1;
							int num8 = 0;
							VideoOfferParams* ptr = (VideoOfferParams*)(&_0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040);
							long num9 = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 4));
							long num10 = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY0N_0040UVideoOfferParams_0040Service_0040Zune_0040Microsoft_0040_0040, 8));
							while (num6 == 0)
							{
								int num11 = *(int*)ptr;
								if ((num11 != 8 && num11 != 13) || *(int*)num9 != 3 || *(int*)num10 != -1 || num7 == 0)
								{
									if (num11 == 3)
									{
										if (*(int*)num9 == 4 && *(int*)num10 == -1 && num4 != 0)
										{
											goto IL_0610;
										}
									}
									else if (num11 != 12)
									{
										goto IL_02a8;
									}
									if (*(int*)num9 != 3 || *(int*)num10 != 4 || num5 == 0)
									{
										goto IL_02a8;
									}
								}
								goto IL_0610;
								IL_0430:
								bool flag2;
								bool flag3;
								bool flag4;
								int num12;
								bool flag6;
								ushort* ptr2;
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
										bool flag5 = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, int, int, int>)(*(ulong*)(*(long*)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 192)) + 168)))((nint)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 192)), eMediaRights, (EMediaFormat)num12, 1, 1) != 0) ? true : false);
										flag6 = flag5;
									}
									string expirationDate = new string((char*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref (long)(nint)ptr2 == 0 ? ref System.Runtime.CompilerServices.Unsafe.As<_0024ArrayType_0024_0024_0024BY00_0024_0024CBG, _003F>(ref global::_003CModule_003E._003F_003F_C_0040_11LOCGONAA_0040_003F_0024AA_003F_0024AA_0040) : ref *(_003F*)ptr2));
									VideoOffer value = new VideoOffer(id, title, seriesTitle, System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 128)), System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 132)), artist, albumId, genre, releaseYear, previewImageUrl, productionCompany, new PriceInfo(global::_003CModule_003E.CComPtrNtv_003CIPriceInfo_003E_002E_002EPEAUIPriceInfo_0040_0040(&cComPtrNtv_003CIPriceInfo_003E)), flag7, isRental, flag4, isMusicVideo, flag3, flag6, flag, expirationDate);
									list.Add(value);
									num4 = 1;
									if (flag8 && flag9 && flag6 && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, EMediaFormat, int>)(*(ulong*)(*(long*)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 192)) + 64)))((nint)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 192)), (EMediaRights)num11, (EMediaFormat)3, (EMediaFormat)4) != 0)
									{
										num6 = (flag ? 1 : 0);
										num5 = 1;
									}
									else
									{
										if (flag7 && flag4 && flag6)
										{
											num7 = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, int>)(*(ulong*)(*(long*)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 192)) + 72)))((nint)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 192)), (EMediaRights)num11, (EMediaFormat)2) != 0) ? 1 : num7);
										}
										if (num5 == 0 && flag7 && flag9 && flag6 && ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, EMediaFormat, int>)(*(ulong*)(*(long*)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 192)) + 64)))((nint)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 192)), (EMediaRights)num11, (EMediaFormat)2, (EMediaFormat)3) != 0)
										{
											VideoOffer value2 = new VideoOffer(id, title, seriesTitle, System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 128)), System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 132)), artist, albumId, genre, releaseYear, previewImageUrl, productionCompany, PriceInfo.FreeWithPoints(), isHD: false, isRental: false, isStream: false, isMusicVideo: false, flag3, previouslyPurchased: true, flag, expirationDate);
											list.Add(value2);
											num5 = 1;
										}
									}
								}
								catch
								{
									//try-fault
									global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPriceInfo_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIPriceInfo_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPriceInfo_003E);
									throw;
								}
								goto IL_05f2;
								IL_0423:
								global::_003CModule_003E.CComPtrNtv_003CIPriceInfo_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIPriceInfo_003E);
								goto IL_0610;
								IL_0610:
								num8++;
								num10 += 12;
								num9 += 12;
								ptr = (VideoOfferParams*)((ulong)(nint)ptr + 12uL);
								if ((uint)num8 >= 13u)
								{
									break;
								}
								continue;
								IL_02a8:
								num12 = *(int*)num9;
								flag7 = num12 == 2;
								flag8 = num12 == 3;
								ESeasonPurchaseFlags eSeasonPurchaseFlags = ((!flag7) ? ((ESeasonPurchaseFlags)1) : ((ESeasonPurchaseFlags)2));
								flag3 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ESeasonPurchaseFlags, int>)(*(ulong*)(*(long*)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 192)) + 224)))((nint)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 192)), eSeasonPurchaseFlags) == 1;
								global::_003CModule_003E.CComPtrNtv_003CIPriceInfo_003E_002E_007Bctor_007D(&cComPtrNtv_003CIPriceInfo_003E);
								try
								{
									ptr2 = null;
									_GUID gUID_NULL = global::_003CModule_003E.GUID_NULL;
									_GUID gUID_NULL2 = global::_003CModule_003E.GUID_NULL;
									_GUID gUID_NULL3 = global::_003CModule_003E.GUID_NULL;
									int num13 = *(int*)num10;
									if (((num13 == -1) ? ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, _GUID*, _GUID*, IPriceInfo**, ushort**, int>)(*(ulong*)(*(long*)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 192)) + 136)))((nint)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 192)), (EMediaRights)num11, (EMediaFormat)num12, &gUID_NULL2, &gUID_NULL, global::_003CModule_003E.CComPtrNtv_003CIPriceInfo_003E_002E_0026(&cComPtrNtv_003CIPriceInfo_003E), &ptr2) : ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, EMediaFormat, _GUID*, _GUID*, _GUID*, IPriceInfo**, ushort**, int>)(*(ulong*)(*(long*)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 192)) + 128)))((nint)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 192)), (EMediaRights)num11, (EMediaFormat)num12, (EMediaFormat)num13, &gUID_NULL2, &gUID_NULL3, &gUID_NULL, global::_003CModule_003E.CComPtrNtv_003CIPriceInfo_003E_002E_0026(&cComPtrNtv_003CIPriceInfo_003E), &ptr2)) >= 0)
									{
										flag9 = ((num11 == 3 || num11 == 12) ? true : false);
										isRental = ((num11 == 7 || num11 == 9) ? true : false);
										flag4 = ((num11 == 8 || num11 == 13 || num11 == 9) ? true : false);
										int num14 = ((num11 == 12 || num11 == 13) ? 1 : 0);
										bool flag10 = (byte)num14 != 0;
										flag2 = ((((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, EMediaRights, EMediaFormat, int, int, int>)(*(ulong*)(*(long*)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 192)) + 168)))((nint)System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 192)), (EMediaRights)num11, (EMediaFormat)num12, 1, 1) != 0) ? true : false);
										flag6 = flag2;
										isMusicVideo = System.Runtime.CompilerServices.Unsafe.As<VideoMetadata, int>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref videoMetadata, 24)) == 0;
										if (flag3 == flag10)
										{
											goto IL_0430;
										}
										goto IL_0423;
									}
								}
								catch
								{
									//try-fault
									global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPriceInfo_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIPriceInfo_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPriceInfo_003E);
									throw;
								}
								goto IL_05f2;
								IL_05f2:
								try
								{
									global::_003CModule_003E.SysFreeString(ptr2);
								}
								catch
								{
									//try-fault
									global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPtrNtv_003CIPriceInfo_003E*, void>)(&global::_003CModule_003E.CComPtrNtv_003CIPriceInfo_003E_002E_007Bdtor_007D), &cComPtrNtv_003CIPriceInfo_003E);
									throw;
								}
								global::_003CModule_003E.CComPtrNtv_003CIPriceInfo_003E_002E_007Bdtor_007D(&cComPtrNtv_003CIPriceInfo_003E);
								goto IL_0610;
							}
						}
					}
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<VideoMetadata*, void>)(&global::_003CModule_003E.VideoMetadata_002E_007Bdtor_007D), &videoMetadata);
					throw;
				}
				global::_003CModule_003E.VideoMetadata_002E_007Bdtor_007D(&videoMetadata);
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
			base.Finalize();
		}
	}

	public virtual sealed void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	~VideoOfferCollection()
	{
		Dispose(false);
	}
}
