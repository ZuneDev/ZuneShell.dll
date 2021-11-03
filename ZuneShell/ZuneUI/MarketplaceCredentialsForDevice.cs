// Decompiled with JetBrains decompiler
// Type: ZuneUI.MarketplaceCredentialsForDevice
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneUI
{
    public class MarketplaceCredentialsForDevice : ModelItem
    {
        private string _email;
        private string _password;
        private string _zuneTag;
        private Guid _userGuid;
        private bool _purchaseEnabled;
        private bool _newTagSet;
        private Choice _enableMarketplaceChoice;
        private int _hr = HRESULT._E_PENDING.Int;

        public MarketplaceCredentialsForDevice(
          string email,
          string password,
          bool purchaseEnabled,
          bool alreadyAssociated,
          string zuneTag,
          Choice enableMarketplaceChoice)
        {
            this._email = email;
            this._password = password;
            this._purchaseEnabled = purchaseEnabled;
            this._zuneTag = zuneTag;
            this._enableMarketplaceChoice = enableMarketplaceChoice;
            this._newTagSet = alreadyAssociated;
        }

        public string Email
        {
            get => this._email;
            set => this._email = value;
        }

        public string Password
        {
            get => this._password;
            set => this._password = value;
        }

        public string ZuneTag
        {
            get => this._zuneTag;
            set
            {
                if (!(this._zuneTag != value))
                    return;
                this._zuneTag = value;
                this._newTagSet = !string.IsNullOrEmpty(this.ZuneTag);
                this.FirePropertyChanged("IsAssociated");
            }
        }

        public Guid UserGuid
        {
            get => this._userGuid;
            set => this._userGuid = value;
        }

        public bool PurchaseEnabled
        {
            get => this._purchaseEnabled;
            set
            {
                if (this._purchaseEnabled == value)
                    return;
                this._purchaseEnabled = value;
                this.FirePropertyChanged(nameof(PurchaseEnabled));
            }
        }

        public bool IsAssociated => this._enableMarketplaceChoice.ChosenIndex != 0 && this._newTagSet;

        public int hr
        {
            get => this._hr;
            set => this._hr = value;
        }
    }
}
