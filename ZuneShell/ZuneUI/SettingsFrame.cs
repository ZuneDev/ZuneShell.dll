// Decompiled with JetBrains decompiler
// Type: ZuneUI.SettingsFrame
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public class SettingsFrame : Frame
    {
        private Experience[] _experiences;
        private SettingsExperience _settings;
        private WizardExperience _wizard;

        public SettingsFrame(IModelItemOwner owner)
          : base(owner)
        {
        }

        public override IList ExperiencesList
        {
            get
            {
                if (this._experiences == null)
                    this._experiences = new Experience[1]
                    {
             Settings
                    };
                return _experiences;
            }
        }

        public SettingsExperience Settings
        {
            get
            {
                if (this._settings == null)
                    this._settings = new SettingsExperience(this);
                return this._settings;
            }
        }

        public WizardExperience Wizard
        {
            get
            {
                if (this._wizard == null)
                    this._wizard = new WizardExperience(this);
                return this._wizard;
            }
        }

        protected override void OnIsCurrentChanged() => SingletonModelItem<UIDeviceList>.Instance.AllowUnreadyDevices = !this.IsCurrent || ZuneShell.DefaultInstance.CurrentPage is FirstLaunchLandPage || ZuneShell.DefaultInstance.CurrentPage is GDILandPage;
    }
}
