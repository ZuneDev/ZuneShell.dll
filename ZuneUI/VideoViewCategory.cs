// Decompiled with JetBrains decompiler
// Type: ZuneUI.VideoViewCategory
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class VideoViewCategory
    {
        private string _view;
        private string _category;

        public VideoViewCategory(string view, string category)
        {
            this._view = view;
            this._category = category;
        }

        public string View => this._view;

        public string Category => this._category;
    }
}
