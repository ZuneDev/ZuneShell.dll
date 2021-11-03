// Decompiled with JetBrains decompiler
// Type: ZuneUI.DataObjectStringExtractorList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public class DataObjectStringExtractorList : StringExtractorList
    {
        private string _property;

        public DataObjectStringExtractorList() => this.CanSearchForString = true;

        public DataObjectStringExtractorList(IList source, string property)
          : this()
        {
            this.Source = source;
            this.Property = property;
        }

        public string Property
        {
            get => this._property;
            set
            {
                if (!(this._property != value))
                    return;
                this._property = value;
                this.Reset();
            }
        }

        protected override string ExtractString(object item)
        {
            if (this._property == null)
                return base.ExtractString(item);
            return ((DataProviderObject)item).GetProperty(this._property)?.ToString();
        }
    }
}
