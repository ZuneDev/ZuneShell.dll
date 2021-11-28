using System.Runtime.CompilerServices;

namespace MicrosoftZuneLibrary
{
    public class ZuneLibraryUtils
    {
        public unsafe static int ConvertPropVariantToString(PROPVARIANT pPropVariant, ref string pRetString)
        {
            //IL_000b: Expected I4, but got I8
            //IL_001e: Expected I4, but got I8
            //IL_003d: Expected I, but got I8
            pRetString = null;
            PROPVARIANT cComPropVariant = new();
            int num;
            try
            {
                num = 0;
                PROPVARIANT ptr = new();
                if (pPropVariant.vt == VARTYPE.VT_BSTR)
                {
                    ptr = pPropVariant;
                    goto done;
                }
                num = Module.ZunePropVariantChangeType(ref cComPropVariant, ref pPropVariant, 1, 8);
                if (num >= 0)
                {
                    ptr = cComPropVariant;
                    goto done;
                }
                goto skip;

                done:
                pRetString = ptr.bstrVal;

                skip:;
            }
            catch
            {
                //try-fault
                Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<PROPVARIANT*, void>)(&Module.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
                throw;
            }
            cComPropVariant.Clear();
            return num;
        }

        public unsafe static int ConvertPropVariantToInt(PROPVARIANT pPropVariant)
        {
            //IL_000a: Expected I4, but got I8
            //IL_001b: Expected I4, but got I8
            int result = 0;
            PROPVARIANT cComPropVariant = new();
            try
            {
                PROPVARIANT ptr = new();
                if (pPropVariant.vt == VARTYPE.VT_I4)
                {
                    ptr = pPropVariant;
                    goto IL_002d;
                }
                if (Module.ZunePropVariantChangeType(ref cComPropVariant, ref pPropVariant, 1, 3) >= 0)
                {
                    ptr = cComPropVariant;
                    goto IL_002d;
                }
                goto end_IL_000a;
                IL_002d:
                result = ptr.intVal;
                end_IL_000a:;
            }
            catch
            {
                //try-fault
                Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<PROPVARIANT*, void>)(&Module.CComPropVariant_002E_007Bdtor_007D), &cComPropVariant);
                throw;
            }
            cComPropVariant.Clear();
            return result;
        }
    }
}
