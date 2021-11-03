// Decompiled with JetBrains decompiler
// Type: ZuneUI.SlideShowState
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections.Generic;

namespace ZuneUI
{
    public class SlideShowState : ModelItem
    {
        private int _index;
        private string _sort;
        private bool _play;
        private bool _canPlay;
        private bool _usePhotoIds;
        private bool _startWithFirstPhotoId;
        private Command _navigate;
        private int _folderId;
        private List<int> _photoIds;

        public SlideShowState(IModelItemOwner owner)
          : base(owner)
        {
            this._navigate = new Command(this);
            this._photoIds = new List<int>();
        }

        public int Index
        {
            get => this._index;
            set
            {
                if (this._index == value)
                    return;
                this._index = value;
                this.FirePropertyChanged(nameof(Index));
            }
        }

        public string Sort
        {
            get => this._sort;
            set
            {
                if (!(this._sort != value))
                    return;
                this._sort = value;
                this.FirePropertyChanged(nameof(Sort));
            }
        }

        public bool Play
        {
            get => this._play;
            set
            {
                if (this._play == value)
                    return;
                this._play = value;
                this.FirePropertyChanged(nameof(Play));
            }
        }

        public bool CanPlay
        {
            get => this._canPlay;
            set
            {
                if (this._canPlay == value)
                    return;
                this._canPlay = value;
                this.FirePropertyChanged(nameof(CanPlay));
            }
        }

        public Command Navigate => this._navigate;

        public int FolderId
        {
            get => this._folderId;
            set
            {
                if (this._folderId == value)
                    return;
                this._folderId = value;
                this.FirePropertyChanged(nameof(FolderId));
            }
        }

        public List<int> PhotoIds
        {
            get => this._photoIds;
            set
            {
                if (this._photoIds == value)
                    return;
                this._photoIds = value;
                this.FirePropertyChanged(nameof(PhotoIds));
            }
        }

        public bool UsePhotoIds
        {
            get => this._usePhotoIds;
            set
            {
                if (this._usePhotoIds == value)
                    return;
                this._usePhotoIds = value;
                this.FirePropertyChanged(nameof(UsePhotoIds));
            }
        }

        public bool StartWithFirstPhotoId
        {
            get => this._startWithFirstPhotoId;
            set
            {
                if (this._startWithFirstPhotoId == value)
                    return;
                this._startWithFirstPhotoId = value;
                this.FirePropertyChanged(nameof(StartWithFirstPhotoId));
            }
        }
    }
}
