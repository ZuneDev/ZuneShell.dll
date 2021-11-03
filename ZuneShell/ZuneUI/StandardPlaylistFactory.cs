// Decompiled with JetBrains decompiler
// Type: ZuneUI.StandardPlaylistFactory
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Playlist;

namespace ZuneUI
{
    public class StandardPlaylistFactory : PlaylistFactory
    {
        private readonly string _baseUniqueTitle;

        private StandardPlaylistFactory(string baseUniqueTitle)
          : base(false)
        {
            this.Ready = true;
            this._baseUniqueTitle = baseUniqueTitle;
        }

        public static StandardPlaylistFactory CreateInstance(
          string baseUniqueTitle)
        {
            return new StandardPlaylistFactory(baseUniqueTitle);
        }

        public override string GetUniqueTitle() => Microsoft.Zune.Playlist.PlaylistManager.Instance.GetUniquePlaylistTitle(this._baseUniqueTitle);

        public override PlaylistResult CreatePlaylist(
          string title,
          CreatePlaylistOption option)
        {
            return PlaylistManager.Instance.CreatePlaylist(title, option);
        }
    }
}
