// Decompiled with JetBrains decompiler
// Type: ZuneUI.FirstLaunchForPhoneWelcomePage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;

namespace ZuneUI
{
    public class FirstLaunchForPhoneWelcomePage : WizardPage
    {
        private Choice _welcomeOptionsChoice;

        internal FirstLaunchForPhoneWelcomePage(Wizard wizard)
          : base(wizard)
        {
            this.Description = Shell.LoadString(StringId.IDS_PHONE_WELCOME_TITLE);
            this.EnableVerticalScrolling = true;
            this.CanNavigateInto = false;
        }

        public override string UI => "res://ZuneShellResources!FirstLaunch.uix#FirstLaunchForPhoneWelcomePage";

        public Choice WelcomeOptionsChoice
        {
            get
            {
                if (this._welcomeOptionsChoice == null)
                {
                    this._welcomeOptionsChoice = new Choice(this);
                    this._welcomeOptionsChoice.Options = (new Command[2]
                    {
             new RadioOptionWithSecondaryText( this, Shell.LoadString(StringId.IDS_PHONE_WELCOME_DEFAULT_HEADER), Shell.LoadString(StringId.IDS_PHONE_WELCOME_DEFAULT_TEXT)),
             new RadioOptionWithSecondaryText( this, Shell.LoadString(StringId.IDS_PHONE_WELCOME_CONFIG_HEADER), Shell.LoadString(StringId.IDS_PHONE_WELCOME_CONFIG_TEXT))
                    });
                    this._welcomeOptionsChoice.Clear();
                    this._welcomeOptionsChoice.ChosenChanged += (sender, args) => ((FirstLaunchForPhoneWizard)this._owner).IsSoftwareSettingsEnabled = this._welcomeOptionsChoice.ChosenIndex == 1;
                }
                return this._welcomeOptionsChoice;
            }
        }

        internal override bool OnMovingNext()
        {
            if (this._welcomeOptionsChoice.ChosenIndex == 1)
                Fue.Instance.ProxyDefaultPaths();
            return base.OnMovingNext();
        }
    }
}
