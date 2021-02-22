// Decompiled with JetBrains decompiler
// Type: ZuneUI.WirelessTypeGrid
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;
using System;
using System.Collections.Generic;

namespace ZuneUI
{
    internal class WirelessTypeGrid
    {
        private int _iColumn;
        private int _cColumns;
        private List<WirelessNetworkTypeCommand> _items = new List<WirelessNetworkTypeCommand>();

        public WirelessTypeGrid(int columns) => this._cColumns = columns;

        public void NewRow()
        {
            if (this._iColumn > 0)
            {
                while (this._iColumn < this._cColumns)
                    this.AddItem(new WirelessNetworkTypeCommand((IModelItemOwner)null, (string)null, (EventHandler)null, (WlanAuthCipherPair)null));
            }
            this._iColumn = 0;
        }

        public bool AddItem(WirelessNetworkTypeCommand item)
        {
            if (this._iColumn >= this._cColumns)
                return false;
            this._items.Add(item);
            ++this._iColumn;
            return true;
        }

        public List<WirelessNetworkTypeCommand> NetworkList => this._items;
    }
}
