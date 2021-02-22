// Decompiled with JetBrains decompiler
// Type: ZuneUI.FamilySetting
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class FamilySetting
    {
        private int _ratingId;
        private string _ratingSystem;
        private int _ratingLevel;
        private bool _blockUnrated;
        private bool _changed;

        public FamilySetting(int ratingId, string ratingSystem, int ratingLevel, bool blockUnrated)
        {
            this._ratingId = ratingId;
            this._ratingSystem = ratingSystem;
            this._ratingLevel = ratingLevel;
            this._blockUnrated = blockUnrated;
            this._changed = false;
        }

        public FamilySetting(string ratingSystem, int ratingLevel, bool blockUnrated)
        {
            this._ratingId = -1;
            this._ratingSystem = ratingSystem;
            this._ratingLevel = ratingLevel;
            this._blockUnrated = blockUnrated;
            this._changed = true;
        }

        public int RatingId
        {
            get => this._ratingId;
            set => this._ratingId = value;
        }

        public string RatingSystem
        {
            get => this._ratingSystem;
            set
            {
                this._ratingSystem = value;
                this._changed = true;
            }
        }

        public int RatingLevel
        {
            get => this._ratingLevel;
            set
            {
                this._ratingLevel = value;
                this._changed = true;
            }
        }

        public bool BlockUnrated
        {
            get => this._blockUnrated;
            set
            {
                this._blockUnrated = value;
                this._changed = true;
            }
        }

        public bool HasChanged => this._changed;
    }
}
