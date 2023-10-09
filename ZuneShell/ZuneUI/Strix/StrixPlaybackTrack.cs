using Microsoft.Zune;
using StrixMusic.Sdk.AppModels;
using System;
using System.Linq;

namespace ZuneUI.Strix;

public class StrixPlaybackTrack : PlaybackTrack
{
    public StrixPlaybackTrack(ITrack stTrack, Guid zuneId)
    {
        Track = stTrack;
        ZuneMediaId = zuneId;
    }

    public ITrack Track { get; }

    public override string Title => Track.Name;

    public override TimeSpan Duration => Track.Duration;

    public override Guid ZuneMediaId { get; }

    public override HRESULT GetURI(out string uri)
    {
        uri = null;
        if (Track.TotalUrlCount <= 0)
            return HRESULT._DB_E_NOTFOUND;

        var url = AsyncHelper.Run(
            () => Track.GetUrlsAsync(1, 0).FirstAsync().AsTask()
        );

        uri = url.Url.ToString();
        return HRESULT._S_OK;
    }
}
