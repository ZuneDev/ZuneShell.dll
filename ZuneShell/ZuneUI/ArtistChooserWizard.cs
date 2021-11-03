// Decompiled with JetBrains decompiler
// Type: ZuneUI.ArtistChooserWizard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class ArtistChooserWizard : Wizard
    {
        private bool _errorOccurred;

        public ArtistChooserWizard() => this.AddPage(new ArtistChooserWizardPage(this));

        public bool ErrorOccurred
        {
            get => this._errorOccurred;
            set
            {
                if (this._errorOccurred == value)
                    return;
                this._errorOccurred = value;
                this.FirePropertyChanged(nameof(ErrorOccurred));
            }
        }
    }
}
