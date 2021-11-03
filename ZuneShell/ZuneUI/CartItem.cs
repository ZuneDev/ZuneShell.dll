// Decompiled with JetBrains decompiler
// Type: ZuneUI.CartItem
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Service;
using System;

namespace ZuneUI
{
    public class CartItem : ModelItem
    {
        private string _messagingId;
        private string _type;
        private string _detailsLink;
        private Guid _mediaId;
        private EContentType _mediaType;
        private string _displayType;
        private DataProviderObject _marketplaceItem;
        private string _title;
        private string _sortTitle;
        private string _artistName;
        private string _albumTitle;
        private bool _availableInMarketplace;
        private Command _executeCommand;
        private object _extraData;

        public CartItem()
        {
            this._mediaId = Guid.Empty;
            this._albumTitle = string.Empty;
            this._title = string.Empty;
            this._artistName = string.Empty;
        }

        public string MessagingId
        {
            get => this._messagingId;
            set
            {
                if (!(this._messagingId != value))
                    return;
                this._messagingId = value;
                this.FirePropertyChanged(nameof(MessagingId));
            }
        }

        public string Type
        {
            get => this._type;
            set
            {
                if (!(this._type != value))
                    return;
                this._type = value;
                this.FirePropertyChanged(nameof(Type));
            }
        }

        public string DetailsLink
        {
            get => this._detailsLink;
            set
            {
                if (!(this._detailsLink != value))
                    return;
                this._detailsLink = value;
                this.FirePropertyChanged(nameof(DetailsLink));
            }
        }

        public Guid MediaId
        {
            get => this._mediaId;
            set
            {
                if (!(this._mediaId != value))
                    return;
                this._mediaId = value;
                this.FirePropertyChanged(nameof(MediaId));
            }
        }

        public EContentType MediaType
        {
            get => this._mediaType;
            set
            {
                if (this._mediaType == value)
                    return;
                this._mediaType = value;
                this.FirePropertyChanged(nameof(MediaType));
            }
        }

        public string DisplayType
        {
            get => this._displayType;
            set
            {
                if (!(this._displayType != value))
                    return;
                this._displayType = value;
                this.FirePropertyChanged(nameof(DisplayType));
            }
        }

        public DataProviderObject MarketplaceItem
        {
            get => this._marketplaceItem;
            set
            {
                if (this._marketplaceItem == value)
                    return;
                this._marketplaceItem = value;
                this.FirePropertyChanged(nameof(MarketplaceItem));
            }
        }

        public string Title
        {
            get => this._title;
            set
            {
                if (!(this._title != value))
                    return;
                this._title = value;
                this.FirePropertyChanged(nameof(Title));
            }
        }

        public string SortTitle
        {
            get => this._sortTitle;
            set
            {
                if (!(this._sortTitle != value))
                    return;
                this._sortTitle = value;
                this.FirePropertyChanged(nameof(SortTitle));
            }
        }

        public string ArtistName
        {
            get => this._artistName;
            set
            {
                if (!(this._artistName != value))
                    return;
                this._artistName = value;
                this.FirePropertyChanged(nameof(ArtistName));
            }
        }

        public string AlbumTitle
        {
            get => this._albumTitle;
            set
            {
                if (!(this._albumTitle != value))
                    return;
                this._albumTitle = value;
                this.FirePropertyChanged(nameof(AlbumTitle));
            }
        }

        public bool AvailableInMarketplace
        {
            get => this._availableInMarketplace;
            set
            {
                if (this._availableInMarketplace == value)
                    return;
                this._availableInMarketplace = value;
                this.FirePropertyChanged(nameof(AvailableInMarketplace));
            }
        }

        public Command ExecuteCommand
        {
            get => this._executeCommand;
            set
            {
                if (this._executeCommand == value)
                    return;
                this._executeCommand = value;
                this.FirePropertyChanged(nameof(ExecuteCommand));
            }
        }

        public object ExtraData
        {
            get => this._extraData;
            set
            {
                if (this._extraData == value)
                    return;
                this._extraData = value;
                this.FirePropertyChanged(nameof(ExtraData));
            }
        }
    }
}
