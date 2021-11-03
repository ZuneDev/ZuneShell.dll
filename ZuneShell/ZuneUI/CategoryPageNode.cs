// Decompiled with JetBrains decompiler
// Type: ZuneUI.CategoryPageNode
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class CategoryPageNode : Node
    {
        private IList _categories;
        private bool _allowBackNavigation = true;
        private bool _hideDeviceOnCancel = true;

        public CategoryPageNode(
          Experience owner,
          StringId id,
          IList categories,
          SQMDataId sqmDataID,
          bool allowBackNavigation,
          bool hideDeviceOnCancel)
          : base(owner, id, null, sqmDataID)
        {
            this._allowBackNavigation = allowBackNavigation;
            this._hideDeviceOnCancel = hideDeviceOnCancel;
            bool flag = false;
            foreach (Category category in categories)
            {
                if (category == null)
                    flag = true;
            }
            if (flag)
            {
                List<Category> categoryList = new List<Category>(categories.Count);
                foreach (Category category in categories)
                {
                    if (category != null)
                        categoryList.Add(category);
                }
                categories = categoryList.ToArray();
            }
            this._categories = categories;
        }

        protected override void Execute(Shell shell) => this.Invoke((Category)this._categories[0], null);

        public void Invoke(Category category) => this.Invoke(category, null);

        public void Invoke(Category category, IDictionary commandArgs)
        {
            ZuneShell defaultInstance = ZuneShell.DefaultInstance;
            if (defaultInstance == null)
                throw new InvalidOperationException("No Shell instance has been registered.  Unable to perform navigation.");
            CategoryPage categoryPage = new CategoryPage(this);
            categoryPage.CurrentCategory = category;
            if (commandArgs != null)
                categoryPage.NavigationArguments = commandArgs;
            defaultInstance.NavigateToPage(categoryPage);
        }

        public IList Categories => this._categories;

        public bool AllowBackNavigation => this._allowBackNavigation;

        public bool HideDeviceOnCancel => this._hideDeviceOnCancel;
    }
}
