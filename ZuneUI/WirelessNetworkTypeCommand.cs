// Decompiled with JetBrains decompiler
// Type: ZuneUI.WirelessNetworkTypeCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;
using System;

namespace ZuneUI
{
    public class WirelessNetworkTypeCommand : Command
    {
        private WlanAuthCipherPair _networkType;

        public WirelessNetworkTypeCommand(
          IModelItemOwner owner,
          string description,
          EventHandler handler,
          WlanAuthCipherPair pair)
          : base(owner, description, handler)
        {
            this._networkType = pair;
        }

        public WlanAuthCipherPair NetworkType => this._networkType;
    }
}
