// Decompiled with JetBrains decompiler
// Type: ZuneUI.PageNode
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using System;
using System.Collections;

namespace ZuneUI
{
    public class PageNode : Node
    {
        private GetPageCallback _handler;

        public PageNode(Experience owner, StringId id, GetPageCallback handler, SQMDataId sqmDataID)
          : base(owner, id, (string)null, sqmDataID)
          => this._handler = handler;

        protected override void Execute(Shell shell) => this.Invoke((IDictionary)null);

        public void Invoke(IDictionary commandArgs)
        {
            ZuneShell defaultInstance = ZuneShell.DefaultInstance;
            if (defaultInstance == null)
                throw new InvalidOperationException("No Shell instance has been registered.  Unable to perform navigation.");
            ZunePage page = this._handler();
            if (commandArgs != null)
                page.NavigationArguments = commandArgs;
            defaultInstance.NavigateToPage(page);
        }
    }
}
