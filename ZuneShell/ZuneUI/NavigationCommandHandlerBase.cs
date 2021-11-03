// Decompiled with JetBrains decompiler
// Type: ZuneUI.NavigationCommandHandlerBase
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Collections;

namespace ZuneUI
{
    public abstract class NavigationCommandHandlerBase : ICommandHandler
    {
        protected string _pageUIPath;

        public string UIPath
        {
            get => this._pageUIPath;
            set => this._pageUIPath = value;
        }

        protected abstract ZunePage GetPage(IDictionary args);

        public void Execute(string command, IDictionary commandArgs)
        {
            ZuneShell defaultInstance = ZuneShell.DefaultInstance;
            if (defaultInstance == null)
                throw new InvalidOperationException("No Shell instance has been registered.  Unable to perform navigation.");
            if (!Shell.IsUIPathEnabled(this.UIPath))
                return;
            ZunePage page = this.GetPage(commandArgs);
            if (commandArgs != null)
                page.NavigationArguments = commandArgs;
            page.NavigationCommand = command;
            if (this.UIPath != null)
                page.UIPath = this.UIPath;
            defaultInstance.NavigateToPage(page);
        }
    }
}
