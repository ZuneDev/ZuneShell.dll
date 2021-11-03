// Decompiled with JetBrains decompiler
// Type: ZuneUI.ArtistChooserWizardPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class ArtistChooserWizardPage : WizardPage
    {
        internal ArtistChooserWizardPage(ArtistChooserWizard wizard)
          : base(wizard)
          => this.Description = Shell.LoadString(StringId.IDS_ArtistChooserTitle);

        protected ArtistChooserWizard Wizard => (ArtistChooserWizard)this._owner;

        public override string UI => "res://ZuneShellResources!ArtistChooser.uix#ArtistChooserWizardPage";
    }
}
