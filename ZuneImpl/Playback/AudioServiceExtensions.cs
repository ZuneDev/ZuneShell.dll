using StrixMusic.Sdk.MediaPlayback;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Zune.Playback;

public static class AudioServiceExtensions
{
    public static async Task StopAsync(this IAudioPlayerService player, CancellationToken cancellationToken = default)
    {
        await player.PauseAsync(cancellationToken);
        await player.SeekAsync(TimeSpan.Zero, cancellationToken);
    }

    public static void Stop(this IAudioPlayerService player) => AsyncHelper.Run(player.StopAsync());

    public static Task MuteAsync(this IAudioPlayerService player, CancellationToken cancellationToken = default)
        => player.ChangeVolumeAsync(0.0, cancellationToken);

    public static void Mute(this IAudioPlayerService player) => AsyncHelper.Run(player.MuteAsync());

    public static void PlaySync(this IAudioPlayerService player, PlaybackItem item) => AsyncHelper.Run(player.Play(item));

    public static void PreloadSync(this IAudioPlayerService player, PlaybackItem item) => AsyncHelper.Run(player.Preload(item));

    public static void Resume(this IAudioPlayerService player) => AsyncHelper.Run(player.ResumeAsync());
}
