using StrixMusic.Sdk.CoreModels;
using StrixMusic.Sdk.MediaPlayback;
using System;
using System.IO;

namespace Microsoft.Zune.Playback;

public class ZuneMediaSourceConfig : IMediaSourceConfig
{
    public ZuneMediaSourceConfig(string uri, int playbackId)
    {
        MediaSourceUri = new(uri);
        Id = uri;
        PlaybackId = playbackId;
    }

    public ICoreTrack Track => null;

    public string Id { get; private set; }

    public int PlaybackId { get; private set; }

    public Uri MediaSourceUri { get; private set; }

    public Stream FileStreamSource => null;

    public string FileStreamContentType => null;

    public DateTime? ExpirationDate => null;

    public static PlaybackItem CreatePlaybackItem(string uri, int playbackId)
    {
        return new()
        {
            MediaConfig = new ZuneMediaSourceConfig(uri, playbackId)
        };
    }
}
