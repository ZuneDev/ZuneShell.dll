// Decompiled with JetBrains decompiler
// Type: ZuneUI.MarketplaceAppActionCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using System;
using ZuneXml;

namespace ZuneUI
{
    public class MarketplaceAppActionCommand : MarketplaceActionCommand
    {
        private string _serviceVersionString;
        private bool _allowPurchaseFull = true;
        private bool _allowPurchaseTrial = true;

        public override void FindInCollection()
        {
            if (!this.CanFindInCollection)
                return;
            ApplicationLibraryPage.FindInCollection(this.CollectionId);
        }

        internal AppData AppData => (AppData)this.Model;

        public bool AllowPurchaseFull
        {
            get => this._allowPurchaseFull;
            set
            {
                if (this._allowPurchaseFull == value)
                    return;
                this._allowPurchaseFull = value;
                this.FirePropertyChanged(nameof(AllowPurchaseFull));
                this.FirePropertyChanged("CanPurchaseFull");
                this.UpdateState();
            }
        }

        public bool AllowPurchaseTrial
        {
            get => this._allowPurchaseTrial;
            set
            {
                if (this._allowPurchaseTrial == value)
                    return;
                this._allowPurchaseTrial = value;
                this.FirePropertyChanged(nameof(AllowPurchaseTrial));
                this.FirePropertyChanged("CanPurchaseTrial");
                this.UpdateState();
            }
        }

        private string ServiceVersionString
        {
            get
            {
                if (this._serviceVersionString == null)
                    this._serviceVersionString = this.Model != null ? (string)this.Model.GetProperty("Version") : (string)null;
                return this._serviceVersionString;
            }
        }

        public bool NeedsUpdate
        {
            get
            {
                bool flag = false;
                return this.CanFindInCollection ? ApplicationLibraryPage.DoesApplicationNeedUpdate(this.CollectionId, this.ServiceVersionString) : flag;
            }
        }

        public bool CanDownload => this.AppData != null && this.AppData.CanDownload && this.AllowDownload;

        public bool CanPurchaseFull => this.AppData != null && this.AppData.CanPurchaseFull && this.AllowPurchaseFull;

        public bool CanPurchaseTrial => this.AppData != null && this.AppData.CanPurchaseTrial && this.AllowPurchaseTrial;

        protected override EContentType ContentType => EContentType.App;

        protected override void OnPropertyChanged(string property)
        {
            base.OnPropertyChanged(property);
            if ("Model" == property)
            {
                this.Id = this.Model != null ? (Guid)this.Model.GetProperty("Id") : Guid.Empty;
                this._serviceVersionString = (string)null;
                this.FirePropertyChanged("CanDownload");
                this.FirePropertyChanged("CanPurchaseFull");
                this.FirePropertyChanged("CanPurchaseTrial");
            }
            else
            {
                if (!("AllowDownload" == property))
                    return;
                this.FirePropertyChanged("CanDownload");
            }
        }

        public override void UpdateState()
        {
            base.UpdateState();
            if (this.Downloading)
                return;
            if (this.NeedsUpdate)
            {
                this.Description = Shell.LoadString(StringId.IDS_UPDATE);
                this.Available = true;
            }
            else
            {
                if (this.CanFindInCollection)
                    return;
                if (this.CanDownload)
                {
                    this.Description = Shell.LoadString(StringId.IDS_DOWNLOAD);
                    this.Available = true;
                }
                else if (this.CanPurchaseFull)
                {
                    if (this.AppData.Price == 0.0)
                        this.Description = Shell.LoadString(StringId.IDS_FREE);
                    else
                        this.Description = string.Format(Shell.LoadString(StringId.IDS_BUY_CURRENCY), (object)this.AppData.DisplayPriceFull);
                    this.Available = true;
                }
                else
                {
                    if (!this.CanPurchaseTrial)
                        return;
                    this.Description = Shell.LoadString(StringId.IDS_TRY);
                    this.Available = true;
                }
            }
        }
    }
}
