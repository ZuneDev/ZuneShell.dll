// Decompiled with JetBrains decompiler
// Type: ZuneUI.MixResult
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;
using System.ComponentModel;

namespace ZuneUI
{
    public class MixResult : INotifyPropertyChanged
    {
        private MixResultType _resultType;
        private string _reason;
        private string _primaryText;
        private string _secondaryText;
        private string _id;
        private Guid _imageId;
        private IList _tracks;
        private DataProviderQueryStatus _tracksQueryStatus;
        private string _imageUri;
        private DataProviderObject _baseObject;

        protected MixResult()
        {
        }

        protected void Initialize(
          MixResultType resultType,
          string reason,
          string primaryText,
          string secondaryText,
          string id,
          string imageUri,
          Guid imageId,
          DataProviderObject baseObject)
        {
            this._resultType = resultType;
            this._reason = reason;
            this._primaryText = primaryText;
            this._secondaryText = secondaryText;
            this._id = id;
            this._imageId = imageId;
            this._imageUri = imageUri;
            this._baseObject = baseObject;
        }

        public static MixResult CreateInstance(
          MixResultType resultType,
          string reason,
          string primaryText,
          string secondaryText,
          string id,
          string imageUri,
          Guid imageId,
          DataProviderObject baseObject)
        {
            MixResult mixResult = new MixResult();
            mixResult.Initialize(resultType, reason, primaryText, secondaryText, id, imageUri, imageId, baseObject);
            return mixResult;
        }

        public string Reason => this._reason;

        public string PrimaryText => this._primaryText;

        public string SecondaryText => this._secondaryText;

        public string Id => this._id;

        public MixResultType ResultType => this._resultType;

        public Guid ImageId => this._imageId;

        public string ImageUri => this._imageUri;

        public DataProviderObject BaseObject => this._baseObject;

        internal virtual bool IsDuplicate(MixResult compareTo) => this.ResultType == compareTo.ResultType && this.Id == compareTo.Id;

        public IList Tracks
        {
            get => this._tracks;
            set
            {
                if (value == this._tracks)
                    return;
                this._tracks = value;
                this.NotifyPropertyChanged(nameof(Tracks));
            }
        }

        public DataProviderQueryStatus TracksQueryStatus
        {
            get => this._tracksQueryStatus;
            set
            {
                if (value == this._tracksQueryStatus)
                    return;
                this._tracksQueryStatus = value;
                this.NotifyPropertyChanged(nameof(TracksQueryStatus));
            }
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged == null)
                return;
            this.PropertyChanged((object)this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
