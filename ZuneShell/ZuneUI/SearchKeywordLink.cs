// Decompiled with JetBrains decompiler
// Type: ZuneUI.SearchKeywordLink
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class SearchKeywordLink : ShellCommand
    {
        private string _header;
        private string[] _keywords;
        private StringId _idsKeywords;

        public SearchKeywordLink(
          StringId idsHeader,
          StringId idsDescription,
          StringId idsKeywords,
          string command)
        {
            this._header = Shell.LoadString(idsHeader);
            this.Description = Shell.LoadString(idsDescription);
            this.Command = command;
            this._idsKeywords = idsKeywords;
        }

        public string Header => this._header;

        public string[] Keywords
        {
            get
            {
                if (this._keywords == null)
                    this._keywords = Shell.LoadString(this._idsKeywords).Split(',');
                return this._keywords;
            }
        }
    }
}
