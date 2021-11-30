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
				string wBSTRString = "";
				fixed (char* wBSTRStringPtr = wBSTRString)
				{
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ushort**, int>)(*(ulong*)(*(long*)pContextData + 24)))((nint)pContextData, (ushort**)(wBSTRStringPtr));
					bool flag = *(long*)(wBSTRStringPtr) != 0L && (*(ushort*)(*(ulong*)(wBSTRStringPtr)) != 0);
					if (flag)
					{
						result = wBSTRString;
					}
				}
			}
			return result;
		}

		public OfferCollection()
		{
		}
	}
}
