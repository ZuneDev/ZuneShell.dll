// Decompiled with JetBrains decompiler
// Type: ZuneUI.KinBrandingStringMap
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    internal class KinBrandingStringMap : PhoneBrandingStringMap
    {
        private static readonly KinBrandingStringMap _instance = new KinBrandingStringMap();

        private KinBrandingStringMap()
        {
        }

        static KinBrandingStringMap() => KinBrandingStringMap._instance.Initialize();

        public static KinBrandingStringMap Instance => KinBrandingStringMap._instance;

        public override void Initialize()
        {
            base.Initialize();
            if (!this._stringMap.ContainsKey(StringId.IDS_EULA_DIALOG_TEXTAREA_TITLE))
                return;
            this._stringMap.Remove(StringId.IDS_EULA_DIALOG_TEXTAREA_TITLE);
        }
    }
}
