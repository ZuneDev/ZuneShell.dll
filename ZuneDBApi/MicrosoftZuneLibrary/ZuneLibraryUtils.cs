using System.Runtime.CompilerServices;

namespace MicrosoftZuneLibrary
{
	public class ZuneLibraryUtils
	{
		public unsafe static int ConvertPropVariantToString(PROPVARIANT pPropVariant, string* pRetString)
		{
			//IL_000b: Expected I4, but got I8
			//IL_001e: Expected I4, but got I8
			//IL_003d: Expected I, but got I8
			*pRetString = null;
			CComPropVariant cComPropVariant;
            // IL initblk instruction
            Unsafe.InitBlock(ref cComPropVariant, 0, 24);
			int num;
			try
			{
				num = 0;
				PROPVARIANT ptr;
				if (*(ushort*)pPropVariant == 8)
				{
					ptr = pPropVariant;
					goto IL_0032;
				}
                // IL initblk instruction
                Unsafe.InitBlock(ref cComPropVariant, 0, 24);
				num = Module.ZuneLibraryExports_002EZunePropVariantChangeType((PROPVARIANT)(&cComPropVariant), pPropVariant, 1, 8);
				if (num >= 0)
				{
					ptr = (PROPVARIANT)(&cComPropVariant);
					goto IL_0032;
				}
				goto end_IL_000b;
				IL_0032:
				*pRetString = new string((char*)(*(ulong*)((ulong)(nint)ptr + 8uL)));
				end_IL_000b:;
			}
			catch
			{
				//try-fault
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&Module.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
				throw;
			}
			Module.CComPropVariant_002EClear(&cComPropVariant);
			return num;
		}

		public unsafe static int ConvertPropVariantToInt(PROPVARIANT pPropVariant)
		{
			//IL_000a: Expected I4, but got I8
			//IL_001b: Expected I4, but got I8
			int result = 0;
			CComPropVariant cComPropVariant;
            // IL initblk instruction
            Unsafe.InitBlock(ref cComPropVariant, 0, 24);
			try
			{
				PROPVARIANT ptr;
				if (*(ushort*)pPropVariant == 3)
				{
					ptr = pPropVariant;
					goto IL_002d;
				}
                // IL initblk instruction
                Unsafe.InitBlock(ref cComPropVariant, 0, 24);
				if (Module.ZuneLibraryExports_002EZunePropVariantChangeType((PROPVARIANT)(&cComPropVariant), pPropVariant, 1, 3) >= 0)
				{
					ptr = (PROPVARIANT)(&cComPropVariant);
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
				Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CComPropVariant*, void>)(&Module.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
				throw;
			}
			Module.CComPropVariant_002EClear(&cComPropVariant);
			return result;
		}
	}
}
