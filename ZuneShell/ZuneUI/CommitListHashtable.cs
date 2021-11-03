// Decompiled with JetBrains decompiler
// Type: ZuneUI.CommitListHashtable
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneUI
{
    public class CommitListHashtable
    {
        private Hashtable _hashtable;

        public CommitListHashtable() => this._hashtable = new Hashtable();

        public void Clear()
        {
            this._hashtable.Clear();
            ZuneShell.DefaultInstance.Management.HasPendingCommits = false;
        }

        public object this[object key]
        {
            get => this._hashtable[key];
            set
            {
                this._hashtable[key] = value;
                ZuneShell.DefaultInstance.Management.HasPendingCommits = true;
            }
        }

        public int Count => this._hashtable.Count;

        public bool ContainsKey(object key) => this._hashtable.ContainsKey(key);

        public bool ContainsValue(object value) => this._hashtable.ContainsValue(value);

        public bool ContainsIntValue(int searchValue)
        {
            foreach (DictionaryEntry dictionaryEntry in (Hashtable)this._hashtable.Clone())
            {
                if (dictionaryEntry.Value is int && (int)dictionaryEntry.Value == searchValue)
                    return true;
            }
            return false;
        }

        public void RemoveByIntValue(int removeValue)
        {
            foreach (DictionaryEntry dictionaryEntry in (Hashtable)this._hashtable.Clone())
            {
                if (dictionaryEntry.Value is int && (int)dictionaryEntry.Value == removeValue)
                    this._hashtable.Remove(dictionaryEntry.Key);
            }
            this.CheckForPendingCommits();
        }

        public void RemoveByStringValue(string removeValue)
        {
            if (removeValue == null)
                return;
            foreach (DictionaryEntry dictionaryEntry in (Hashtable)this._hashtable.Clone())
            {
                if (dictionaryEntry.Value is string && (string)dictionaryEntry.Value == removeValue)
                    this._hashtable.Remove(dictionaryEntry.Key);
            }
            this.CheckForPendingCommits();
        }

        public void Remove(object key)
        {
            if (key == null)
                return;
            if (key != null)
                this._hashtable.Remove(key);
            this.CheckForPendingCommits();
        }

        public void Save()
        {
            Hashtable hashtable = (Hashtable)this._hashtable.Clone();
            this.Clear();
            foreach (DictionaryEntry dictionaryEntry in hashtable)
                ((ProxySettingDelegate)dictionaryEntry.Key)(dictionaryEntry.Value);
        }

        private void CheckForPendingCommits()
        {
            if (this._hashtable == null || this._hashtable.Count != 0)
                return;
            this.Clear();
        }
    }
}
