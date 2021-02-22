// Decompiled with JetBrains decompiler
// Type: ZuneUI.MediaType
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public enum MediaType
    {
        Undefined = -1, // 0xFFFFFFFF
        Track = 3,
        Video = 4,
        Photo = 5,
        Playlist = 9,
        Album = 11, // 0x0000000B
        PodcastEpisode = 17, // 0x00000011
        Podcast = 18, // 0x00000012
        MediaFolder = 20, // 0x00000014
        Genre = 21, // 0x00000015
        AudioMP4 = 32, // 0x00000020
        AudioMP3 = 33, // 0x00000021
        AudioWMA = 34, // 0x00000022
        AudioWAV = 35, // 0x00000023
        ImageJPEG = 36, // 0x00000024
        VideoMP4 = 37, // 0x00000025
        VideoMPG = 38, // 0x00000026
        VideoWMV = 39, // 0x00000027
        VideoQT = 40, // 0x00000028
        AudioQT = 41, // 0x00000029
        VideoDVRMS = 42, // 0x0000002A
        VideoMBR = 43, // 0x0000002B
        VideoAVI = 44, // 0x0000002C
        PlaylistChannel = 50, // 0x00000032
        PlaylistContentItem = 56, // 0x00000038
        Artist = 65, // 0x00000041
        UserCard = 96, // 0x00000060
        Application = 110, // 0x0000006E
    }
}
