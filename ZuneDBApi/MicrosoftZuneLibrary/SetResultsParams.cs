using System.Collections;
using System.Runtime.InteropServices;

namespace MicrosoftZuneLibrary;

public class SetResultsParams
{
	private bool _hasFriends;

	private IList _friends;

	private IList _friendsWithFavs;

	private IList _friendsWithMostPlayedSongs;

	private IList _friendsWithMostPlayedArtists;

	public IList FriendsWithMostPlayedArtists => _friendsWithMostPlayedArtists;

	public IList FriendsWithMostPlayedSongs => _friendsWithMostPlayedSongs;

	public IList FriendsWithFavs => _friendsWithFavs;

	public IList Friends => _friends;

	public bool HasFriends
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return _hasFriends;
		}
	}

	public SetResultsParams([MarshalAs(UnmanagedType.U1)] bool hasFriends, IList friends, IList friendsWithFavs, IList friendsWithMostPlayedSongs, IList friendsWithMostPlayedArtists)
	{
		_hasFriends = hasFriends;
		_friends = friends;
		_friendsWithFavs = friendsWithFavs;
		_friendsWithMostPlayedSongs = friendsWithMostPlayedSongs;
		_friendsWithMostPlayedArtists = friendsWithMostPlayedArtists;
	}
}
