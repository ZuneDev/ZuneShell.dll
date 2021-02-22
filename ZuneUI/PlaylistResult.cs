// Decompiled with JetBrains decompiler
// Type: ZuneUI.PlaylistResult
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class PlaylistResult
    {
        private PlaylistError _error;
        private int _playlistId;
        private HRESULT _hr;

        internal PlaylistResult(int playlistId, HRESULT hr)
        {
            this._hr = hr;
            if (hr.IsSuccess && playlistId > 0)
            {
                this._playlistId = playlistId;
            }
            else
            {
                this._playlistId = PlaylistManager.InvalidPlaylistId;
                if ((HRESULT)hr.Int == HRESULT._DB_E_RESOURCEEXISTS)
                    this._error = PlaylistError.NameExists;
                else if ((HRESULT)hr.Int == HRESULT._DB_E_BADPARAMETERNAME)
                    this._error = PlaylistError.InvalidName;
                else
                    this._error = PlaylistError.Other;
            }
        }

        public PlaylistResult(int playlistId, PlaylistError error)
        {
            this._playlistId = playlistId;
            this._error = error;
        }

        public PlaylistError Error => this._error;

        public int PlaylistId => this._playlistId;

        public HRESULT HR => this._hr;
    }
}
