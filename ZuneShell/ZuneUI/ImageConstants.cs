// Decompiled with JetBrains decompiler
// Type: ZuneUI.ImageConstants
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;

namespace ZuneUI
{
    public static class ImageConstants
    {
        private static Size _nowPlayingSize = new Size(1012, 693);
        private static Size _smallArtistTileSize = new Size(160, 120);
        private static Size _largeArtistTileSize = new Size(420, 320);
        private static Size _channelTileSize = new Size(160, 120);
        private static Size _smallMusicVideoTileSize = new Size(160, 120);
        private static Size _largeMusicVideoTileSize = new Size(420, 320);
        private static Size _smallAlbumSize = new Size(86, 86);
        private static Size _mediumAlbumSize = new Size(100, 100);
        private static Size _largeAlbumSize = new Size(320, 320);
        private static Size _mediumAppsTileSize = new Size(100, 100);
        private static Size _largeAppsTileSize = new Size(160, 160);
        private static Size _playlistTileSize = new Size(160, 120);
        private static Size _feature1x1Size = new Size(240, 240);
        private static Size _feature4x3SmallSize = new Size(258, 194);
        private static Size _feature4x3Size = new Size(420, 320);
        private static Size _feature16x9Size = new Size(853, 480);
        private static Size _defaultSize = new Size(320, 320);

        public static Size Default => _defaultSize;

        public static Size NowPlaying => _nowPlayingSize;

        public static Size SmallAlbum => _smallAlbumSize;

        public static Size MediumAlbum => _mediumAlbumSize;

        public static Size LargeAlbum => _largeAlbumSize;

        public static Size ChannelTile => _channelTileSize;

        public static Size SmallMusicVideoTile => _smallMusicVideoTileSize;

        public static Size LargeMusicVideoTile => _largeMusicVideoTileSize;

        public static Size SmallArtistTile => _smallArtistTileSize;

        public static Size LargeArtistTile => _largeArtistTileSize;

        public static Size MediumAppsTile => _mediumAppsTileSize;

        public static Size LargeAppsTile => _largeAppsTileSize;

        public static Size PlaylistTile => _playlistTileSize;

        public static Size Feature1x1 => _feature1x1Size;

        public static Size Feature4x3Small => _feature4x3SmallSize;

        public static Size Feature4x3 => _feature4x3Size;

        public static Size Feature16x9 => _feature16x9Size;
    }
}
