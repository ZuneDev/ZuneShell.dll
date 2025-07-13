using System;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary;

public class ZunePlaylist : IDisposable
{
	private unsafe IPlaylist* m_pPlaylist;

	private bool _disposed;

	internal unsafe IPlaylist* Playlist => m_pPlaylist;

	private void _007EZunePlaylist()
	{
		_0021ZunePlaylist();
	}

	private unsafe void _0021ZunePlaylist()
	{
		//IL_001f: Expected I, but got I8
		//IL_0028: Expected I, but got I8
		if (!_disposed)
		{
			IPlaylist* pPlaylist = m_pPlaylist;
			if (pPlaylist != null)
			{
				((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint>)(*(ulong*)(*(long*)pPlaylist + 16)))((nint)pPlaylist);
				m_pPlaylist = null;
			}
		}
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
	{
		if (P_0)
		{
			_0021ZunePlaylist();
			return;
		}
		try
		{
			_0021ZunePlaylist();
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

	~ZunePlaylist()
	{
		Dispose(false);
	}
}
