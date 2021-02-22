// Decompiled with JetBrains decompiler
// Type: ZuneXml.MessageTypeEnum
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneXml
{
    public enum MessageTypeEnum
    {
        Invalid = -1, // 0xFFFFFFFF
        Album = 0,
        Card = 1,
        Forums = 2,
        FriendRequest = 3,
        Message = 4,
        MusicVideo = 5,
        Notification = 6,
        Photos = 7,
        Playlist = 8,
        Podcast = 9,
        Song = 10, // 0x0000000A
        Video = 11, // 0x0000000B
        Movie = 12, // 0x0000000C
        MovieTrailer = 13, // 0x0000000D
    }
}
