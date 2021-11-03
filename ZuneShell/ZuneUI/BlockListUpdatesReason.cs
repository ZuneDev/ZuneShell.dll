// Decompiled with JetBrains decompiler
// Type: ZuneUI.BlockListUpdatesReason
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    [Flags]
    public enum BlockListUpdatesReason
    {
        None = 0,
        ModalUI = 1,
        JumpInList = 2,
        DragSelect = 4,
        DragDrop = 8,
        Edit = 16, // 0x00000010
        Focus = 32, // 0x00000020
    }
}
