// Decompiled with JetBrains decompiler
// Type: ZuneUI.MixStackEntry
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneUI
{
    public class MixStackEntry
    {
        private MixResult _result;
        private object _layout;
        private IList _dataList;

        public MixStackEntry(MixResult result, object layout, IList dataList)
        {
            this._result = result;
            this._layout = layout;
            this._dataList = dataList;
        }

        public MixResult Result => this._result;

        public object Layout => this._layout;

        public IList DataList => this._dataList;
    }
}
