// Decompiled with JetBrains decompiler
// Type: ZuneUI.WizardPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneUI
{
    public abstract class WizardPage : ModelItem
    {
        protected Wizard _owner;
        private string _breadcrumbTitle;
        private string _detailDescription;
        private string _loadStatus;
        protected bool _showValidationState;
        private string _statusMessage;
        private bool _showStatusMessageIcon;
        private bool _enableVerticalScrolling;
        private bool _canNavigateInto;

        protected WizardPage(Wizard owner)
        {
            this._canNavigateInto = true;
            this._showStatusMessageIcon = true;
            this._owner = owner;
        }

        public abstract string UI { get; }

        internal virtual void Activate()
        {
        }

        internal virtual void Deactivate() => this.ShowValidationState = false;

        public virtual bool IsEnabled => !this._owner.ErrorPageIsEnabled;

        public virtual bool IsValid => true;

        public virtual bool ShowNavigation => true;

        public virtual bool ShowClose => false;

        public virtual bool CanCancel => true;

        public string BreadcrumbTitle
        {
            get => this._breadcrumbTitle;
            set
            {
                if (!(this._breadcrumbTitle != value))
                    return;
                this._breadcrumbTitle = value;
                this.FirePropertyChanged(nameof(BreadcrumbTitle));
            }
        }

        public string DetailDescription
        {
            get => this._detailDescription;
            set
            {
                if (!(this._detailDescription != value))
                    return;
                this._detailDescription = value;
                this.FirePropertyChanged(nameof(DetailDescription));
            }
        }

        public string LoadStatus
        {
            get => this._loadStatus;
            set
            {
                if (!(this._loadStatus != value))
                    return;
                this._loadStatus = value;
                this.FirePropertyChanged(nameof(LoadStatus));
            }
        }

        public bool ShowValidationState
        {
            get => this._showValidationState;
            private set
            {
                if (this._showValidationState == value)
                    return;
                this._showValidationState = value;
                this.FirePropertyChanged(nameof(ShowValidationState));
            }
        }

        public string StatusMessage
        {
            get => this._statusMessage;
            set
            {
                if (!(this._statusMessage != value))
                    return;
                this._statusMessage = value;
                this.FirePropertyChanged(nameof(StatusMessage));
            }
        }

        public virtual bool ShowPrivacyStatement => false;

        public bool CanShowStatusMessageIcon
        {
            get => this._showStatusMessageIcon;
            set
            {
                if (this._showStatusMessageIcon == value)
                    return;
                this._showStatusMessageIcon = value;
                this.FirePropertyChanged(nameof(CanShowStatusMessageIcon));
            }
        }

        public bool CanNavigateInto
        {
            get => this._canNavigateInto;
            set
            {
                if (this._canNavigateInto == value)
                    return;
                this._canNavigateInto = value;
                this.FirePropertyChanged(nameof(CanNavigateInto));
            }
        }

        public bool EnableVerticalScrolling
        {
            get => this._enableVerticalScrolling;
            set
            {
                if (this._enableVerticalScrolling == value)
                    return;
                this._enableVerticalScrolling = value;
                this.FirePropertyChanged(nameof(EnableVerticalScrolling));
            }
        }

        public void ShowValidation()
        {
            this.ShowValidationState = true;
            this.FirePropertyChanged("ShowValidationState");
        }

        public virtual void RefreshValidationState() => this.ShowValidation();

        public void ShowGenericErrorStatus()
        {
            if (!string.IsNullOrEmpty(this.StatusMessage) || this.IsValid)
                return;
            this.StatusMessage = Shell.LoadString(StringId.IDS_WIZARD_GENERIC_ERROR);
        }

        public bool CommitChanges() => this.OnCommitChanges();

        protected virtual bool OnCommitChanges() => true;

        internal virtual bool OnMovingNext() => true;

        internal virtual bool OnMovingBack() => true;
    }
}
