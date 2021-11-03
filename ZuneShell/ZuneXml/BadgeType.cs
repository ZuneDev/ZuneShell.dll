// Decompiled with JetBrains decompiler
// Type: ZuneXml.BadgeType
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneXml
{
    public enum BadgeType
    {
        Invalid = -1, // 0xFFFFFFFF
        GoldArtist = 0,
        SilverArtist = 1,
        BronzeArtist = 2,
        GoldAlbum = 3,
        SilverAlbum = 4,
        BronzeAlbum = 5,
        GoldForums = 6,
        SilverForums = 7,
        BronzeForums = 8,
        GoldReview = 9,
        SilverReview = 10, // 0x0000000A
        BronzeReview = 11, // 0x0000000B
    }
}
