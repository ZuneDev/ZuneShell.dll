// Decompiled with JetBrains decompiler
// Type: UIXControls.MenuItemCommand
// Assembly: UIXControls, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: 78800EA5-2757-404C-BA30-C33FCFC2852A
// Assembly location: C:\Program Files\Zune\UIXControls.dll

using Microsoft.Iris;
using System;

namespace UIXControls
{
    public class MenuItemCommand : Command
    {
        private bool _hidden;

        public MenuItemCommand()
        {
        }

        public MenuItemCommand(string description)
          : base((IModelItemOwner)null, description, (EventHandler)null)
        {
        }

        public bool Hidden
        {
            get => this._hidden;
            set
            {
                if (this._hidden == value)
                    return;
                this._hidden = value;
                this.FirePropertyChanged(nameof(Hidden));
            }
        }

        public virtual bool ShouldHide() => this.Hidden;

        public override string ToString() => string.Format("{0}:\"{1}\", Available = {2}, Hidden = {3}", (object)this.GetType().Name, (object)this.Description, (object)this.Available, (object)this.Hidden);
    }
}
