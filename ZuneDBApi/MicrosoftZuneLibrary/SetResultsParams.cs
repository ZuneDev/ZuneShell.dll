using System.Collections;

namespace MicrosoftZuneLibrary;

public class SetResultsParams
{
    public bool HasFriends { get; }

    public IList Friends { get; }

    public IList FriendsWithFavs { get; }

    public IList FriendsWithMostPlayedSongs { get; }

    public IList FriendsWithMostPlayedArtists { get; }

    public SetResultsParams(bool hasFriends, IList friends, IList friendsWithFavs, IList friendsWithMostPlayedSongs, IList friendsWithMostPlayedArtists)
    {
        HasFriends = hasFriends;
        Friends = friends;
        FriendsWithFavs = friendsWithFavs;
        FriendsWithMostPlayedSongs = friendsWithMostPlayedSongs;
        FriendsWithMostPlayedArtists = friendsWithMostPlayedArtists;
    }
}
