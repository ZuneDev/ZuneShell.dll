// Decompiled with JetBrains decompiler
// Type: ZuneUI.RadioStation
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class RadioStation
    {
        private string m_title;
        private string m_sourceUrl;
        private string m_image;

        public RadioStation(string Title, string SourceURL, string ImagePath)
        {
            this.m_title = Title;
            this.m_sourceUrl = SourceURL;
            this.m_image = ImagePath;
        }

        public string Title
        {
            get => this.m_title;
            private set
            {
                if (!(value != this.m_title))
                    return;
                this.m_title = value;
            }
        }

        public string SourceURL
        {
            get => this.m_sourceUrl;
            private set
            {
                if (!(value != this.m_sourceUrl))
                    return;
                this.m_sourceUrl = value;
            }
        }

        public string ImagePath
        {
            get => this.m_image;
            private set
            {
                if (!(value != this.m_image))
                    return;
                this.m_image = value;
            }
        }
    }
}
