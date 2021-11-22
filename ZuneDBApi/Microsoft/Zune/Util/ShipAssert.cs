using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Util
{
	public class ShipAssert
	{
		public unsafe static void AssertId([MarshalAs(UnmanagedType.U1)] bool condition, uint id, uint param)
		{
			//IL_000c: Expected I, but got I8
			if (!condition)
			{
				Module.ShipAssert(id, param, null);
			}
		}

		public unsafe static void AssertId_NoBreak([MarshalAs(UnmanagedType.U1)] bool condition, uint id, uint param)
		{
			//IL_000c: Expected I, but got I8
			if (!condition)
			{
				Module.ShipAssert(id, param, null);
			}
		}

		public unsafe static void AssertIdMsg([MarshalAs(UnmanagedType.U1)] bool condition, uint id, uint param, string msg)
		{
			if (condition)
			{
				return;
			}
			fixed (char* msgPtr = msg.ToCharArray())
			{
				ushort* ptr = (ushort*)msgPtr;
				try
				{
					Module.ShipAssert(id, param, ptr);
				}
				catch
				{
					//try-fault
					ptr = null;
					throw;
				}
			}
		}

		public unsafe static void AssertIdMsg_NoBreak([MarshalAs(UnmanagedType.U1)] bool condition, uint id, uint param, string msg)
		{
			if (condition)
			{
				return;
			}
			fixed (char* msgPtr = msg.ToCharArray())
			{
				ushort* ptr = (ushort*)msgPtr;
				try
				{
					Module.ShipAssert(id, param, ptr);
				}
				catch
				{
					//try-fault
					ptr = null;
					throw;
				}
			}
		}

		public unsafe static void Assert([MarshalAs(UnmanagedType.U1)] bool condition)
		{
			//IL_0069: Expected I, but got I8
			if (!condition)
			{
				StackFrame frame = new StackTrace(1, fNeedFileInfo: false).GetFrame(0);
				string name = frame.GetMethod().Name;
				uint num = 0u;
				if (name != null && name.Length >= 2)
				{
					char[] array = name.ToCharArray(0, 2);
					num = ((uint)(array[1] & 0xFF) | ((uint)array[0] << 8)) << 16;
				}
				num |= (uint)frame.GetILOffset() & 0xFFFFu;
				Module.ShipAssert(1006u, num, null);
			}
		}
	}
}