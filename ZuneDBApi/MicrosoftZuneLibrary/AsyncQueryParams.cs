using Microsoft.Iris;

namespace MicrosoftZuneLibrary;

public class AsyncQueryParams
{
    public Guid ArtistId { get; }

    public bool GetFriends { get; }

    public int UserId { get; }

    public DeferredInvokeHandler SetResultsHandler { get; }

    public bool ResolveZuneTags { get; }

    public AsyncQueryParams(Guid artistId, bool getFriends, int userId, DeferredInvokeHandler setResultsHandler, bool resolveZuneTags)
    {
        ArtistId = artistId;
        GetFriends = getFriends;
        UserId = userId;
        SetResultsHandler = setResultsHandler;
        ResolveZuneTags = resolveZuneTags;
    }
}
