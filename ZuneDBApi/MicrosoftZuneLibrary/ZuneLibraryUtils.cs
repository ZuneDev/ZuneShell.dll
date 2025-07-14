using System.Runtime.CompilerServices;
using ZuneDBApi.Interop;

namespace MicrosoftZuneLibrary;

public class ZuneLibraryUtils
{
	public unsafe static int ConvertPropVariantToString(tagPROPVARIANT* pPropVariant, string* pRetString)
	{
		//IL_000b: Expected I4, but got I8
		//IL_001e: Expected I4, but got I8
		//IL_003d: Expected I, but got I8
		System.Runtime.CompilerServices.Unsafe.Write(pRetString, null);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPropVariant cComPropVariant);
		// IL initblk instruction
		System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant, 0, 24);
		int num;
		try
		{
			num = 0;
			tagPROPVARIANT* ptr;
			if (*(ushort*)pPropVariant == 8)
			{
				ptr = pPropVariant;
				goto IL_0032;
			}
			// IL initblk instruction
			System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant, 0, 24);
			num = ZuneLibraryExports.ZunePropVariantChangeType((tagPROPVARIANT*)(&cComPropVariant), pPropVariant, 1, 8);
			if (num >= 0)
			{
				ptr = (tagPROPVARIANT*)(&cComPropVariant);
				goto IL_0032;
			}
			goto end_IL_000b;
			IL_0032:
			System.Runtime.CompilerServices.Unsafe.Write(pRetString, new string((char*)(*(ulong*)((ulong)(nint)ptr + 8uL))));
			end_IL_000b:;
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&global::_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
			throw;
		}
		global::_003CModule_003E.CComPropVariant_002EClear(&cComPropVariant);
		return num;
	}

	public unsafe static int ConvertPropVariantToInt(tagPROPVARIANT* pPropVariant)
	{
		//IL_000a: Expected I4, but got I8
		//IL_001b: Expected I4, but got I8
		int result = 0;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CComPropVariant cComPropVariant);
		// IL initblk instruction
		System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant, 0, 24);
		try
		{
			tagPROPVARIANT* ptr;
			if (*(ushort*)pPropVariant == 3)
			{
				ptr = pPropVariant;
				goto IL_002d;
			}
			// IL initblk instruction
			System.Runtime.CompilerServices.Unsafe.InitBlock(ref cComPropVariant, 0, 24);
			if (ZuneLibraryExports.ZunePropVariantChangeType((tagPROPVARIANT*)(&cComPropVariant), pPropVariant, 1, 3) >= 0)
			{
				ptr = (tagPROPVARIANT*)(&cComPropVariant);
				goto IL_002d;
			}
			goto end_IL_000a;
			IL_002d:
			result = *(int*)((ulong)(nint)ptr + 8uL);
			end_IL_000a:;
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&global::_003CModule_003E.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
			throw;
		}
		global::_003CModule_003E.CComPropVariant_002EClear(&cComPropVariant);
		return result;
	}
}
