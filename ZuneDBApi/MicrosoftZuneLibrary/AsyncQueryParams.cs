using System;
using System.Runtime.InteropServices;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary;

public class AsyncQueryParams
{
	private Guid _artistId;

	private bool _getFriends;

	private bool _resolveZuneTags;

	private int _userId;

	private DeferredInvokeHandler _setResultsHandler;

	public DeferredInvokeHandler SetResultsHandler => _setResultsHandler;

	public bool ResolveZuneTags
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return _resolveZuneTags;
		}
	}

	public int UserId => _userId;

	public bool GetFriends
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return _getFriends;
		}
	}

	public Guid ArtistId => _artistId;

	public AsyncQueryParams(Guid artistId, [MarshalAs(UnmanagedType.U1)] bool getFriends, int userId, DeferredInvokeHandler setResultsHandler, [MarshalAs(UnmanagedType.U1)] bool resolveZuneTags)
	{
		_userId = userId;
		_artistId = artistId;
		_getFriends = getFriends;
		_setResultsHandler = setResultsHandler;
		_resolveZuneTags = resolveZuneTags;
	}
}
