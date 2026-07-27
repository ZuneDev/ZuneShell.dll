using System.Collections;
using System.Threading;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary;

public class FriendsOfArtistQuery : ModelItem, IDisposable
{
    private Guid m_artistId;

    private bool m_getFriends;

    private int m_userId;

    private bool m_resolveZuneTags;

    private bool m_hasFriends;

    private bool m_queryComplete;

    private IList m_friends;

    private IList m_friendsWithFavs;

    private IList m_friendsWithMostPlayedSongs;

    private IList m_friendsWithMostPlayedArtists;

    private static FriendsOfArtistQuery m_singletonInstance;

    public IList FriendsWithMostPlayedArtists => m_friendsWithMostPlayedArtists;

    public IList FriendsWithMostPlayedSongs => m_friendsWithMostPlayedSongs;

    public IList FriendsWithFavs => m_friendsWithFavs;

    public IList Friends => m_friends;

    public bool HasFriends => m_hasFriends;

    public bool QueryComplete
    {
        get => m_queryComplete;
        set
        {
            if (value != m_queryComplete)
            {
                m_queryComplete = value;
                FirePropertyChanged(nameof(QueryComplete));
            }
        }
    }

    public static FriendsOfArtistQuery Instance => m_singletonInstance ??= new FriendsOfArtistQuery();

    private FriendsOfArtistQuery()
    {
    }

    public void Query(Guid artistId, bool getFriends, int userId) => Query(artistId, getFriends, userId, resolveZuneTags: true);

    public void Query(Guid artistId, bool getFriends, int userId, bool resolveZuneTags)
    {
        try
        {
            Monitor.Enter(m_singletonInstance);

            QueryComplete = false;

            if (artistId == m_artistId && (!getFriends || m_getFriends) && userId == m_userId)
            {
                QueryComplete = true;
                return;
            }

            m_artistId = artistId;
            m_getFriends = getFriends;
            m_userId = userId;
            m_resolveZuneTags = resolveZuneTags;

            SetResults(hasFriends: false, null, null, null, null);

            if (artistId != Guid.Empty)
            {
                AsyncQueryParams state = new(artistId, getFriends, userId, DeferredSetResults, m_resolveZuneTags);
                ThreadPool.QueueUserWorkItem(AsyncQuery, state);
            }
            else
            {
                QueryComplete = true;
            }
        }
        finally
        {
            Monitor.Exit(m_singletonInstance);
        }
    }

    // The original resolves the artist's user cards (and, per card, the ZuneTag) via the native
    // ZuneLibraryExports.UserCardsForMedia/GetFieldValues exports. Those resolve through the same
    // opaque native singleton/factory table documented as unreachable via static analysis in
    // logs/MicrosoftZuneInterop/IQueryPropertyBag.md (no recoverable RTTI/symbol for the concrete
    // implementation, would need dynamic tracing) — see logs/MicrosoftZuneLibrary/FriendsOfArtistQuery.md.
    // Until that native database exists, every query completes with "no friends found," matching
    // the same treatment as MicrosoftZuneLibrary/LibraryDataProviderItemBase.GetFieldValue/SetFieldValue.
    private static void AsyncQuery(object obj)
    {
        AsyncQueryParams asyncQueryParams = (AsyncQueryParams)obj;
        SetResultsParams args = new(hasFriends: false, null, null, null, null);
        Application.DeferredInvoke(asyncQueryParams.SetResultsHandler, args);
    }

    private void SetResults(bool hasFriends, IList friends, IList friendsWithFavs, IList friendsWithMostPlayedSongs, IList friendsWithMostPlayedArtists)
    {
        if (m_friends != friends)
        {
            m_friends = friends;
            FirePropertyChanged(nameof(Friends));
        }
        if (m_friendsWithFavs != friendsWithFavs)
        {
            m_friendsWithFavs = friendsWithFavs;
            FirePropertyChanged(nameof(FriendsWithFavs));
        }
        if (m_friendsWithMostPlayedSongs != friendsWithMostPlayedSongs)
        {
            m_friendsWithMostPlayedSongs = friendsWithMostPlayedSongs;
            FirePropertyChanged(nameof(FriendsWithMostPlayedSongs));
        }
        if (m_friendsWithMostPlayedArtists != friendsWithMostPlayedArtists)
        {
            m_friendsWithMostPlayedArtists = friendsWithMostPlayedArtists;
            FirePropertyChanged(nameof(FriendsWithMostPlayedArtists));
        }
        if (m_hasFriends != hasFriends)
        {
            m_hasFriends = hasFriends;
            FirePropertyChanged(nameof(HasFriends));
        }
    }

    private void DeferredSetResults(object obj)
    {
        QueryComplete = true;
        SetResultsParams setResultsParams = (SetResultsParams)obj;
        SetResults(setResultsParams.HasFriends, setResultsParams.Friends, setResultsParams.FriendsWithFavs, setResultsParams.FriendsWithMostPlayedSongs, setResultsParams.FriendsWithMostPlayedArtists);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            base.Dispose();
    }

    public new void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~FriendsOfArtistQuery()
    {
        Dispose(false);
    }
}
