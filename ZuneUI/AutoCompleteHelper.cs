// Decompiled with JetBrains decompiler
// Type: ZuneUI.AutoCompleteHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class AutoCompleteHelper : ModelItem
    {
        private IList _options;
        private IList _filteredOptions;
        private string _entry;
        private int _cursorPosition;
        private bool _useEntrySeparator = true;
        private bool _applyfilter = true;
        protected static char[] s_entrySeparators = new char[2]
        {
      ';',
      ','
        };
        private static char[] s_entryTrimmers = new char[3]
        {
      ';',
      ',',
      ' '
        };

        public AutoCompleteHelper()
        {
            this._entry = string.Empty;
            this._cursorPosition = 0;
        }

        public string InsertAtCursor(string insert)
        {
            insert = insert ?? string.Empty;
            string filterString = this.GetFilterString();
            if (this._cursorPosition > 0 && filterString.Length > 0)
            {
                insert = this._entry.Substring(0, this._cursorPosition - filterString.Length) + insert;
                if (this._cursorPosition < this._entry.Length)
                    insert += this._entry.Substring(this._cursorPosition - filterString.Length);
                else if (this._useEntrySeparator)
                    insert = string.Format("{0}{1} ", insert, s_entrySeparators[0]);
                this.Entry = insert;
            }
            else
                this.InsertAtEnd(insert);
            return this.Entry;
        }

        public string InsertAtEnd(string insert)
        {
            if (this.Entry == null || !this._useEntrySeparator)
                this.Entry = insert;
            else if (!string.IsNullOrEmpty(insert))
            {
                string str = this.Entry.TrimEnd(s_entryTrimmers);
                if (str.Length > 0)
                    insert = string.Format("{0} {1}{2} ", s_entrySeparators[0], insert, s_entrySeparators[0]);
                else if (this._useEntrySeparator)
                    insert = string.Format("{0}{1} ", insert, s_entrySeparators[0]);
                this.Entry = str + insert;
            }
            return this.Entry;
        }

        public IList Options
        {
            get => this._options;
            set
            {
                if (this._options == value)
                    return;
                this._options = value;
                this.FirePropertyChanged(nameof(Options));
            }
        }

        public string Entry
        {
            get => this._entry;
            set
            {
                if (!(this._entry != value))
                    return;
                this._cursorPosition = this._entry == null || value == null || !value.StartsWith(this._entry, StringComparison.CurrentCultureIgnoreCase) ? -1 : value.Length;
                this._entry = value;
                if (this.Applyfilter)
                    this.Filter();
                else
                    this.FilteredOptions = this._options;
                this.FirePropertyChanged(nameof(Entry));
            }
        }

        public IList FilteredOptions
        {
            get => this._filteredOptions;
            set
            {
                if (this._filteredOptions == value)
                    return;
                this._filteredOptions = value;
                this.FirePropertyChanged(nameof(FilteredOptions));
            }
        }

        public bool UseEntrySeparator
        {
            get => this._useEntrySeparator;
            set
            {
                if (this._useEntrySeparator == value)
                    return;
                this._useEntrySeparator = value;
                this.FirePropertyChanged(nameof(UseEntrySeparator));
            }
        }

        public bool Applyfilter
        {
            get => this._applyfilter;
            set
            {
                if (this._applyfilter == value)
                    return;
                this._applyfilter = value;
                this.FirePropertyChanged(nameof(Applyfilter));
            }
        }

        private void Filter()
        {
            List<string> stringList = new List<string>();
            string filterString = this.GetFilterString();
            if (this._options != null && !string.IsNullOrEmpty(filterString))
            {
                int index;
                if (this._options is ISearchableList)
                    index = ((ISearchableList)this._options).SearchForString(filterString);
                else if (this._options is List<string>)
                {
                    index = ((List<string>)this._options).BinarySearch(filterString, StringComparer.CurrentCultureIgnoreCase);
                    if (index < 0)
                        index = ~index;
                }
                else
                    index = -1;
                if (index >= 0)
                {
                    for (; index < this._options.Count; ++index)
                    {
                        string option = (string)this._options[index];
                        if (ZuneLibrary.CompareWithoutArticles(filterString, option) == 0)
                            stringList.Add(option);
                        else
                            break;
                    }
                }
            }
            this.FilteredOptions = stringList;
        }

        private string GetFilterString()
        {
            string str = string.Empty;
            if (this._cursorPosition > 0 && this._entry != null)
            {
                int num1 = this._entry.LastIndexOfAny(s_entrySeparators, this._cursorPosition - 1) + 1;
                while (num1 < this._entry.Length && char.IsWhiteSpace(this._entry, num1))
                    ++num1;
                int num2 = this._entry.IndexOfAny(s_entrySeparators, num1);
                int length = num2 <= 0 ? this._entry.Length - num1 : num2 - num1;
                str = this._entry.Substring(num1, length);
            }
            return str.Normalize();
        }
    }
}
