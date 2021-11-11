using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service
{
	public class HttpResponseStream : Stream
	{
		private unsafe IStream* m_pStream;

		private long _length;

		private long _position;

		public unsafe override long Position
		{
			get
			{
				if (m_pStream == null)
				{
					throw new ObjectDisposedException("Already disposed!");
				}
				return _position;
			}
			set
			{
				//Discarded unreachable code: IL_000b
				throw new NotSupportedException("HttpResponseStream.Position.set(...)");
			}
		}

		public unsafe override long Length
		{
			get
			{
				if (m_pStream == null)
				{
					throw new ObjectDisposedException("Already disposed!");
				}
				return _length;
			}
		}

		public override bool CanSeek
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return false;
			}
		}

		public override bool CanWrite
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return false;
			}
		}

		public override bool CanRead
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return true;
			}
		}

		public unsafe HttpResponseStream(IStream* pStream, long length)
		{
			//IL_0028: Expected I, but got I8
			try
			{
				if (pStream == null)
				{
					throw new HttpWebException("Response closed", WebExceptionStatus.ConnectionClosed);
				}
				m_pStream = pStream;
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pStream + 8)))((nint)pStream);
				_length = length;
			}
			catch
			{
				//try-fault
				base.Dispose(disposing: true);
				throw;
			}
		}

		private void _007EHttpResponseStream()
		{
			_0021HttpResponseStream();
		}

		private unsafe void _0021HttpResponseStream()
		{
			//IL_0017: Expected I, but got I8
			//IL_0020: Expected I, but got I8
			IStream* pStream = m_pStream;
			if (pStream != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pStream + 16)))((nint)pStream);
				m_pStream = null;
			}
		}

		public override void Flush()
		{
			//Discarded unreachable code: IL_000b
			throw new NotSupportedException("HttpResponseStream.Flush()");
		}

		public override void SetLength(long value)
		{
			//Discarded unreachable code: IL_000b
			throw new NotSupportedException("HttpResponseStream.SetLength(...)");
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			//Discarded unreachable code: IL_000b
			throw new NotSupportedException("HttpResponseStream.Seek(...)");
		}

		public unsafe override int Read(byte[] buffer, int offset, int count)
		{
			//IL_006b: Expected I, but got I8
			if (m_pStream == null)
			{
				throw new ObjectDisposedException("Already disposed!");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("HttpResponseStream.Read: buffer is null");
			}
			if (offset + count > (nint)buffer.LongLength)
			{
				throw new ArgumentException("HttpResponseStream.Read: offset + count >= buffer.Length");
			}
			if (offset + count < 0)
			{
				throw new ArgumentOutOfRangeException("HttpResponseStream.Read: offset + count < 0");
			}
			fixed (byte* ptr = &buffer[offset])
			{
				uint num = 0u;
				long num2 = *(long*)m_pStream + 24;
				IStream* pStream = m_pStream;
				int num3 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, void*, uint, uint*, int>)(*(ulong*)num2))((nint)pStream, ptr, (uint)count, &num);
				if (num3 < 0)
				{
					throw new IOException($"Read error: 0x{num3:X}");
				}
				_position = (int)num + _position;
				return (int)num;
			}
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			//Discarded unreachable code: IL_000b
			throw new NotSupportedException("HttpResponseStream.Write(...)");
		}

		protected override void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
					_007EHttpResponseStream();
				}
				finally
				{
					base.Dispose(disposing: true);
				}
			}
			else
			{
				try
				{
					_0021HttpResponseStream();
				}
				finally
				{
					base.Dispose(disposing: false);
				}
			}
		}

		~HttpResponseStream()
		{
			Dispose(false);
		}
	}
}
