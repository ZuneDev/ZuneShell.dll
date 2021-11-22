using System;

namespace Microsoft.Zune.Configuration
{
	public class FileAssociationHandlerFactory
	{
		public unsafe static IFileAssociationHandler CreateFileAssociationHandler()
		{
			//IL_0005: Expected I, but got I8
			//IL_001b: Expected I, but got I8
			//IL_003b: Expected I, but got I8
			FileAssociationHandlerWrapper fileAssociationHandlerWrapper = null;
			global::IFileAssociationHandler* ptr = null;
			try
			{
				int num = Module.CreateNativeFileAssociationHandler((void**)(&ptr));
				if (num >= 0)
				{
					fileAssociationHandlerWrapper = new FileAssociationHandlerWrapper(ptr);
					ptr = null;
					return fileAssociationHandlerWrapper;
				}
				throw new ApplicationException(Module.GetErrorDescription(num));
			}
			finally
			{
				if (ptr != null)
				{
					global::IFileAssociationHandler* intPtr = ptr;
					((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)intPtr + 16)))((nint)intPtr);
				}
			}
		}
	}
}
