// Decompiled with JetBrains decompiler
// Type: ZuneUI.ProfileTrack
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneUI
{
    public class ProfileTrack
    {
        private Category _category;
        private DataProviderObject _track;

        public ProfileTrack(Category category, DataProviderObject track)
        {
            this._category = category;
            this._track = track;
        }

        public Category Category => this._category;

        public DataProviderObject Track => this._track;
    }
}
