// Decompiled with JetBrains decompiler
// Type: ZuneUI.Node
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;
using System;
using System.Collections;
using System.ComponentModel;

namespace ZuneUI
{
    public class Node : Command
    {
        private bool _isCurrent;
        private bool _pendingNavigation;
        private string _command;
        private SQMDataId _sqmCountID;

        public Node(Experience owner, string command, SQMDataId sqmCountID)
          : base((IModelItemOwner)owner, (string)null, (EventHandler)null)
        {
            this._command = command;
            this._sqmCountID = sqmCountID;
        }

        public Node(Experience owner, StringId id, string command, SQMDataId sqmCountID)
          : base((IModelItemOwner)owner, Shell.LoadString(id), (EventHandler)null)
        {
            this._command = command;
            this._sqmCountID = sqmCountID;
        }

        public Node(Experience owner, string name, string command)
          : base((IModelItemOwner)owner, name, (EventHandler)null)
        {
            this._command = command;
            this._sqmCountID = SQMDataId.Invalid;
        }

        public Experience Experience => (Experience)this.Owner;

        public bool IsCurrent
        {
            get => this._isCurrent;
            set
            {
                if (this._isCurrent == value)
                    return;
                this._isCurrent = value;
                this.OnIsCurrentChanged();
                this.FirePropertyChanged(nameof(IsCurrent));
            }
        }

        protected virtual void OnIsCurrentChanged()
        {
        }

        protected override void OnInvoked()
        {
            Shell defaultInstance = (Shell)ZuneShell.DefaultInstance;
            if (defaultInstance.NavigationLocked)
            {
                defaultInstance.BlockedByNavigationLock = true;
                defaultInstance.DeferredNavigateNode = this;
            }
            else
            {
                bool flag = defaultInstance.CurrentNode == this;
                bool isRootPage = defaultInstance.CurrentPage.IsRootPage;
                if (!this._pendingNavigation)
                {
                    if (!flag || !isRootPage)
                    {
                        defaultInstance.PropertyChanged += new PropertyChangedEventHandler(this.ShellPropertyChanged);
                        this.Execute(defaultInstance);
                        this._pendingNavigation = true;
                        if (this._sqmCountID != SQMDataId.Invalid)
                            SQMLog.Log(this._sqmCountID, 1);
                    }
                    else
                        defaultInstance.CurrentPage.RefreshPage();
                }
                base.OnInvoked();
            }
        }

        protected virtual void Execute(Shell shell) => shell.Execute(this._command, (IDictionary)null);

        private void ShellPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (!(args.PropertyName == "CurrentPage"))
                return;
            this._pendingNavigation = false;
            ZuneShell.DefaultInstance.PropertyChanged -= new PropertyChangedEventHandler(this.ShellPropertyChanged);
        }
    }
}
