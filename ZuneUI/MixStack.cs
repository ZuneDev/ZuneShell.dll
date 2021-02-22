// Decompiled with JetBrains decompiler
// Type: ZuneUI.MixStack
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class MixStack : Stack<MixStackEntry>
    {
        public void Push(MixResult seedResult, object layout, IList dataList) => this.Push(new MixStackEntry(seedResult, layout, dataList));

        public static bool IsNullOrEmpty(MixStack mixStack) => mixStack == null || mixStack.Count == 0;
    }
}
