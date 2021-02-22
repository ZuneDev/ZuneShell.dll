// Decompiled with JetBrains decompiler
// Type: ZuneUI.Category
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;

namespace ZuneUI
{
    public class Category : ModelItem
    {
        private string _target;
        private bool _allowScrolling;
        private SQMDataId _sqmCountID;

        public Category(StringId titleID, string target)
          : this(titleID, target, true, SQMDataId.Invalid)
        {
        }

        public Category(StringId titleID, string target, bool allowScrolling)
          : this(titleID, target, allowScrolling, SQMDataId.Invalid)
        {
            this._target = target;
            this._allowScrolling = allowScrolling;
        }

        public Category(StringId titleID, string target, bool allowScrolling, SQMDataId sqmCountID)
          : base((IModelItemOwner)null, Shell.LoadString(titleID))
        {
            this._target = target;
            this._allowScrolling = allowScrolling;
            this._sqmCountID = sqmCountID;
        }

        public string Target => this._target;

        public bool AllowScrolling => this._allowScrolling;

        public void LogCategoryView()
        {
            if (this._sqmCountID == SQMDataId.Invalid)
                return;
            SQMLog.Log(this._sqmCountID, 1);
        }
    }
}
