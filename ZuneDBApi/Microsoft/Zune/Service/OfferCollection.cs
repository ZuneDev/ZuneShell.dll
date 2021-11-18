using System;
using System.Collections;

namespace Microsoft.Zune.Service
{
	public abstract class OfferCollection
	{
		protected unsafe string GetRecommendationContext(Guid id, IDictionary mapIdToContext, IContextData* pContextData)
		{
			//IL_0041: Expected I, but got I8
			//IL_0065: Expected I, but got I8
			string result = null;
			if (mapIdToContext != null && mapIdToContext.Contains(id))
			{
				result = (string)mapIdToContext[id];
			}
			else if (pContextData != null)
			{
				WBSTRString wBSTRString;
				Module.WBSTRString_002E_007Bctor_007D(&wBSTRString);
				try
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pContextData + 24)))((nint)pContextData, (ushort**)(&wBSTRString));
					bool flag = *(long*)(&wBSTRString) != 0L && ((*(ushort*)(*(ulong*)(&wBSTRString)) != 0) ? true : false);
					if (flag)
					{
						result = new string((char*)(*(ulong*)(&wBSTRString)));
					}
				}
				catch
				{
					//try-fault
					Module.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<WBSTRString*, void>)(&Module.WBSTRString_002E_007Bdtor_007D), &wBSTRString);
					throw;
				}
				Module.WBSTRString_002E_007Bdtor_007D(&wBSTRString);
			}
			return result;
		}

		public OfferCollection()
		{
		}
	}
}
