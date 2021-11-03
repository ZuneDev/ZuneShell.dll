// Decompiled with JetBrains decompiler
// Type: ZuneUI.FeaturesChanged
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using System;
using UIXControls;

namespace ZuneUI
{
    public class FeaturesChanged : IDisposable
    {
        private bool m_featuresHaveChanged;
        private static FeaturesChanged m_instance;

        public static FeaturesChanged Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new FeaturesChanged();
                return m_instance;
            }
        }

        public void StartUp()
        {
            this.m_featuresHaveChanged = false;
            FeaturesChangedApi.Instance.OnFeaturesChangedEvent += new FeaturesChangedHandler(this.OnFeaturesChangedCallback);
        }

        private void OnFeaturesChangedCallback(bool featuresHaveChanged)
        {
            this.FeaturesHaveChanged = featuresHaveChanged;
            if (!Shell.MainFrame.Marketplace.IsCurrent)
                return;
            CultureHelper.CheckMarketplaceCulture();
        }

        public bool FeaturesHaveChanged
        {
            get => this.m_featuresHaveChanged;
            private set
            {
                if (this.m_featuresHaveChanged || !value)
                    return;
                MessageBox.Show(Shell.LoadString(StringId.IDS_FEATURESCHANGED_TITLE), Shell.LoadString(StringId.IDS_FEATURESCHANGED_CONTENT), null);
                this.m_featuresHaveChanged = value;
            }
        }

        public void Dispose() => FeaturesChangedApi.Instance.OnFeaturesChangedEvent -= new FeaturesChangedHandler(this.OnFeaturesChangedCallback);
    }
}
