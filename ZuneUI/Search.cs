// Decompiled with JetBrains decompiler
// Type: ZuneUI.Search
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security;
using System.Text;

namespace ZuneUI
{
    public class Search : ModelItem
    {
        private string _keywords = "";
        private string[] _processedKeywords;
        private static Search _singletonInstance;
        private bool _ignoreAccented;
        private Dictionary<char, char> _accentTable;
        private Command _executeCommand;
        private Hashtable _keywordLinkTable;
        private Command _searchFocusHotkey;
        private Choice _filterList;
        private SearchResultFilterType _selectedFilter;
        private SearchResultContextType _usersContextType = SearchResultContextType.Undefined;
        private SearchHintHelper _hintHelper;
        private static List<SearchResultFilterCommand> _searchResultFilterCommandTypes;

        public static Search Instance
        {
            get
            {
                if (Search._singletonInstance == null)
                    Search._singletonInstance = new Search();
                return Search._singletonInstance;
            }
        }

        internal static bool HasInstance => Search._singletonInstance != null;

        private Search()
        {
            this._executeCommand = new Command();
            this._ignoreAccented = string.Compare(CultureInfo.CurrentCulture.TwoLetterISOLanguageName, "en", true) == 0;
            this._accentTable = new Dictionary<char, char>();
            this._accentTable.Add('À', 'A');
            this._accentTable.Add('Á', 'A');
            this._accentTable.Add('Â', 'A');
            this._accentTable.Add('Ã', 'A');
            this._accentTable.Add('Ä', 'A');
            this._accentTable.Add('Å', 'A');
            this._accentTable.Add('à', 'a');
            this._accentTable.Add('á', 'a');
            this._accentTable.Add('â', 'a');
            this._accentTable.Add('ã', 'a');
            this._accentTable.Add('ä', 'a');
            this._accentTable.Add('å', 'a');
            this._accentTable.Add('È', 'E');
            this._accentTable.Add('É', 'E');
            this._accentTable.Add('Ê', 'E');
            this._accentTable.Add('Ë', 'E');
            this._accentTable.Add('è', 'e');
            this._accentTable.Add('é', 'e');
            this._accentTable.Add('ê', 'e');
            this._accentTable.Add('ë', 'e');
            this._accentTable.Add('Ì', 'I');
            this._accentTable.Add('Í', 'I');
            this._accentTable.Add('Î', 'I');
            this._accentTable.Add('Ï', 'I');
            this._accentTable.Add('ì', 'i');
            this._accentTable.Add('í', 'i');
            this._accentTable.Add('î', 'i');
            this._accentTable.Add('ï', 'i');
            this._accentTable.Add('Ò', 'O');
            this._accentTable.Add('Ó', 'O');
            this._accentTable.Add('Ô', 'O');
            this._accentTable.Add('Õ', 'O');
            this._accentTable.Add('Ö', 'O');
            this._accentTable.Add('ò', 'o');
            this._accentTable.Add('ó', 'o');
            this._accentTable.Add('ô', 'o');
            this._accentTable.Add('õ', 'o');
            this._accentTable.Add('ö', 'o');
            this._accentTable.Add('Ù', 'U');
            this._accentTable.Add('Ú', 'U');
            this._accentTable.Add('Û', 'U');
            this._accentTable.Add('Ü', 'U');
            this._accentTable.Add('ù', 'u');
            this._accentTable.Add('ú', 'u');
            this._accentTable.Add('û', 'u');
            this._accentTable.Add('ü', 'u');
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing && this._filterList != null)
                this._filterList.ChosenChanged -= new EventHandler(this.FilterListChosenChanged);
            base.OnDispose(disposing);
        }

        public Choice FilterList
        {
            get
            {
                if (this._filterList == null)
                {
                    this._filterList = new Choice((IModelItemOwner)this);
                    this._filterList.Options = (IList)new ArrayListDataSet();
                    this._filterList.ChosenChanged += new EventHandler(this.FilterListChosenChanged);
                    this.UpdateFiltersList();
                }
                this._filterList.ChosenIndex = 0;
                return this._filterList;
            }
        }

        private void FilterListChosenChanged(object sender, EventArgs args)
        {
            if (this._filterList == null)
                return;
            SearchResultFilterCommand chosenValue = (SearchResultFilterCommand)this._filterList.ChosenValue;
            if (chosenValue == null)
                return;
            this.SelectedFilterType = chosenValue.Type;
        }

        public void UpdateFiltersList()
        {
            if (this._filterList == null)
                return;
            if (Search._searchResultFilterCommandTypes == null)
            {
                Search._searchResultFilterCommandTypes = new List<SearchResultFilterCommand>();
                Search._searchResultFilterCommandTypes.Add(SearchResultFilter.All);
                Search._searchResultFilterCommandTypes.Add(SearchResultFilter.Artists);
                Search._searchResultFilterCommandTypes.Add(SearchResultFilter.Albums);
                Search._searchResultFilterCommandTypes.Add(SearchResultFilter.Tracks);
                Search._searchResultFilterCommandTypes.Add(SearchResultFilter.MusicVideos);
                Search._searchResultFilterCommandTypes.Add(SearchResultFilter.TVShows);
                Search._searchResultFilterCommandTypes.Add(SearchResultFilter.Movies);
                Search._searchResultFilterCommandTypes.Add(SearchResultFilter.OtherVideo);
                Search._searchResultFilterCommandTypes.Add(SearchResultFilter.Podcasts);
                Search._searchResultFilterCommandTypes.Add(SearchResultFilter.Playlists);
                Search._searchResultFilterCommandTypes.Add(SearchResultFilter.Channels);
                Search._searchResultFilterCommandTypes.Add(SearchResultFilter.WindowsPhoneApps);
                Search._searchResultFilterCommandTypes.Add(SearchResultFilter.Profile);
            }
            this._filterList.Options.Clear();
            for (int index = 0; index < Search._searchResultFilterCommandTypes.Count; ++index)
            {
                SearchResultFilterCommand filterCommandType = Search._searchResultFilterCommandTypes[index];
                if (filterCommandType.HasResults)
                    this._filterList.Options.Add((object)filterCommandType);
            }
        }

        public void Navigate(string command, IDictionary commandArguments)
        {
            if (string.IsNullOrEmpty(command))
                return;
            if (command.Contains("Marketplace\\"))
                SQMLog.Log(SQMDataId.SearchMarketplaceResultsCount, 1);
            else if (command.Contains("Collection\\"))
                SQMLog.Log(SQMDataId.SearchCollectionResultsCount, 1);
            ZuneShell.DefaultInstance.Execute(command, commandArguments);
        }

        public void RecordContextMenuType(SearchResultContextMenuType type) => SQMLog.LogToStream(SQMDataId.SearchContextMenuAction, (uint)type);

        public SearchResultFilterType SelectedFilterType
        {
            get => this._selectedFilter;
            set
            {
                if (this._selectedFilter == value)
                    return;
                this._selectedFilter = value;
                this.FirePropertyChanged(nameof(SelectedFilterType));
                SQMLog.Log(SQMDataId.SearchFiltersCount, 1);
                SQMLog.LogToStream(SQMDataId.SearchFiltersSelection, (uint)this._selectedFilter);
            }
        }

        public SearchResultContextType UsersContextType
        {
            get => this._usersContextType;
            set
            {
                if (this._usersContextType == value)
                    return;
                this._usersContextType = value;
                this.FirePropertyChanged(nameof(UsersContextType));
            }
        }

        public SearchHintHelper HintHelper
        {
            get
            {
                if (this._hintHelper == null)
                    this._hintHelper = new SearchHintHelper();
                return this._hintHelper;
            }
        }

        public Command SearchFocusHotkey
        {
            get
            {
                if (this._searchFocusHotkey == null)
                    this._searchFocusHotkey = new Command((IModelItemOwner)this);
                return this._searchFocusHotkey;
            }
        }

        public string Keywords
        {
            get => this._keywords.Trim();
            private set
            {
                if (!(this._keywords != value) || string.IsNullOrEmpty(value))
                    return;
                this._keywords = value;
                this._processedKeywords = (string[])null;
                this.FirePropertyChanged(nameof(Keywords));
            }
        }

        public void Execute(string keywords)
        {
            this.Keywords = keywords;
            if (this._executeCommand == null)
                return;
            this._executeCommand.Invoke();
        }

        public Command Executed => this._executeCommand;

        public SearchKeywordLink KeywordLink => (SearchKeywordLink)this.KeywordLinkTable[(object)this.Keywords.ToLower()];

        private Hashtable KeywordLinkTable
        {
            get
            {
                if (this._keywordLinkTable == null)
                {
                    this._keywordLinkTable = new Hashtable();
                    foreach (SearchKeywordLink searchKeywordLink in new SearchKeywordLink[1]
                    {
            new SearchKeywordLink(StringId.IDS_APPLICATIONS, StringId.IDS_SEARCH_MARKETPLACE_GAMES_LINK, StringId.IDS_SEARCH_MARKETPLACE_GAMES_KEYWORDS, "Marketplace\\Apps\\Home")
                    })
                    {
                        for (int index = 0; index < searchKeywordLink.Keywords.Length; ++index)
                            this._keywordLinkTable[(object)searchKeywordLink.Keywords[index].ToLower().Trim()] = (object)searchKeywordLink;
                    }
                }
                return this._keywordLinkTable;
            }
        }

        public bool IsValidKeyword(string keywords)
        {
            if (string.IsNullOrEmpty(keywords))
                return false;
            string str = keywords.Trim();
            return !string.IsNullOrEmpty(str) && (str.Length > 1 || this.IsValidCjkCharacter(str[0]));
        }

        private bool IsValidCjkCharacter(char c)
        {
            ushort num = (ushort)c;
            return num >= (ushort)12352 && (num > (ushort)12351 && num < (ushort)12544 || num > (ushort)13311 && num < (ushort)19904 || (num > (ushort)19967 && num < (ushort)40960 || num > (ushort)44031 && num < (ushort)55216));
        }

        public string FormatResult(string keywordStyle, string item0, string item1)
        {
            List<string> list = new List<string>();
            this.AddResult(keywordStyle, list, item0);
            this.AddResult(keywordStyle, list, item1);
            return this.FormatResult(list);
        }

        public string FormatResult(string keywordStyle, string item0, string item1, string item2)
        {
            List<string> list = new List<string>();
            this.AddResult(keywordStyle, list, item0);
            this.AddResult(keywordStyle, list, item1);
            this.AddResult(keywordStyle, list, item2);
            return this.FormatResult(list);
        }

        private void AddResult(string keywordStyle, List<string> list, string item)
        {
            if (string.IsNullOrEmpty(item))
                return;
            if (keywordStyle != null)
                list.Add(this.HighlightKeywords(item, keywordStyle));
            else
                list.Add(item);
        }

        private string FormatResult(List<string> list)
        {
            switch (list.Count)
            {
                case 1:
                    return list[0];
                case 2:
                    return string.Format(Shell.LoadString(StringId.IDS_FOUND_ITEM_BUTTON1), (object)list[0], (object)list[1]);
                case 3:
                    return string.Format(Shell.LoadString(StringId.IDS_FOUND_ITEM_BUTTON2), (object)list[0], (object)list[1], (object)list[2]);
                default:
                    return "";
            }
        }

        public string HighlightKeywords(string target, string styleName)
        {
            int startIndex = 0;
            if (target == null)
                return "";
            StringBuilder stringBuilder = new StringBuilder(target.Length + styleName.Length * 4 + 10);
            List<Search.IndexRange> keywords = this.FindKeywords(this.ProcessKeywords(), target);
            for (int index = 0; index < keywords.Count; ++index)
            {
                Search.IndexRange indexRange = keywords[index];
                if (indexRange.indexStart > startIndex)
                {
                    string str = target.Substring(startIndex, indexRange.indexStart - startIndex);
                    stringBuilder.Append(SecurityElement.Escape(str));
                }
                string str1 = target.Substring(indexRange.indexStart, indexRange.indexEnd - indexRange.indexStart);
                stringBuilder.Append(string.Format("<{0}>{1}</{0}>", (object)styleName, (object)SecurityElement.Escape(str1)));
                startIndex = indexRange.indexEnd;
            }
            if (startIndex < target.Length)
            {
                string str = target.Substring(startIndex);
                stringBuilder.Append(SecurityElement.Escape(str));
            }
            return stringBuilder.ToString();
        }

        private List<Search.IndexRange> FindKeywords(string[] keywords, string text)
        {
            StringBuilder stringBuilder = new StringBuilder(text.Length);
            for (int index = 0; index < text.Length; ++index)
                stringBuilder.Append(this.MapAccentedChar(text[index]));
            string str = stringBuilder.ToString();
            List<Search.IndexRange> rangeList = new List<Search.IndexRange>();
            foreach (string keyword in keywords)
            {
                int startIndex = 0;
                while (true)
                {
                    int num = str.IndexOf(keyword, startIndex, StringComparison.OrdinalIgnoreCase);
                    if (num >= 0)
                    {
                        char c1 = num > 0 ? str[num - 1] : ' ';
                        char c2 = num + keyword.Length < str.Length ? str[num + keyword.Length] : ' ';
                        if (!char.IsLetterOrDigit(c1) && (keyword.Length >= 2 || !char.IsLetterOrDigit(c2)))
                        {
                            Search.IndexRange keyRange;
                            keyRange.indexStart = num;
                            keyRange.indexEnd = num + keyword.Length;
                            this.AddRange(keyRange, rangeList);
                        }
                        startIndex = num + keyword.Length;
                    }
                    else
                        break;
                }
            }
            this.CollapseRanges(rangeList);
            return rangeList;
        }

        private void CollapseRanges(List<Search.IndexRange> rangeList)
        {
            for (int index1 = 0; index1 < rangeList.Count; ++index1)
            {
                int index2 = index1 + 1;
                while (index2 < rangeList.Count)
                {
                    Search.IndexRange range1 = rangeList[index1];
                    Search.IndexRange range2 = rangeList[index2];
                    if (range1.indexEnd > range2.indexStart)
                    {
                        if (range1.indexEnd >= range2.indexEnd)
                        {
                            rangeList.RemoveAt(index2);
                        }
                        else
                        {
                            range1.indexEnd = range2.indexEnd;
                            rangeList[index1] = range1;
                            rangeList.RemoveAt(index2);
                        }
                    }
                    else
                        break;
                }
            }
        }

        private void AddRange(Search.IndexRange keyRange, List<Search.IndexRange> rangeList)
        {
            int index;
            for (index = 0; index < rangeList.Count; ++index)
            {
                Search.IndexRange range = rangeList[index];
                if (keyRange.indexStart <= range.indexStart)
                    break;
            }
            rangeList.Insert(index, keyRange);
        }

        private string[] ProcessKeywords()
        {
            if (this._processedKeywords == null)
            {
                StringBuilder stringBuilder = new StringBuilder(this._keywords.Length);
                bool flag = false;
                for (int index = 0; index < this._keywords.Length; ++index)
                {
                    char keyword = this._keywords[index];
                    switch (keyword)
                    {
                        case '\t':
                        case '\n':
                        case '\r':
                        case '!':
                        case '"':
                        case '%':
                        case '&':
                        case '(':
                        case ')':
                        case '+':
                        case '-':
                        case ';':
                        case '<':
                        case '>':
                        case '{':
                        case '}':
                            stringBuilder.Append(' ');
                            flag = true;
                            break;
                        case '\'':
                            char c1 = index > 0 ? this._keywords[index - 1] : ' ';
                            char c2 = index + 1 < this._keywords.Length ? this._keywords[index + 1] : ' ';
                            if (char.IsLetterOrDigit(c1) && char.IsLetterOrDigit(c2))
                            {
                                stringBuilder.Append(keyword);
                                break;
                            }
                            stringBuilder.Append(' ');
                            flag = true;
                            break;
                        default:
                            stringBuilder.Append(this.MapAccentedChar(keyword));
                            break;
                    }
                }
                this._processedKeywords = stringBuilder.ToString().Split(new char[1]
                {
          ' '
                }, StringSplitOptions.RemoveEmptyEntries);
                if (flag)
                {
                    Array.Resize<string>(ref this._processedKeywords, this._processedKeywords.Length + 1);
                    this._processedKeywords[this._processedKeywords.Length - 1] = this._keywords;
                }
            }
            return this._processedKeywords;
        }

        private char MapAccentedChar(char original)
        {
            char key = original;
            char ch;
            if (this._ignoreAccented && this._accentTable.TryGetValue(key, out ch))
                key = ch;
            return key;
        }

        private struct IndexRange
        {
            public int indexStart;
            public int indexEnd;
        }
    }
}
