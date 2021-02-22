// Decompiled with JetBrains decompiler
// Type: ZuneUI.FriendsPanel
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public class FriendsPanel : ListPanel
    {
        internal FriendsPanel(FriendsPage page)
          : base((IModelItemOwner)page)
        {
        }

        public int GetIndexFromZuneTag(string tagToFind)
        {
            int num1 = -1;
            int num2 = 0;
            if (this.Content != null && !string.IsNullOrEmpty(tagToFind))
            {
                foreach (object data in (IEnumerable)this.Content)
                {
                    if (ProfileCardData.GetDataProviderObject(data) is DataProviderObject dataProviderObject && SignIn.TagsMatch(dataProviderObject.GetProperty("ZuneTag") as string, tagToFind))
                    {
                        num1 = num2;
                        break;
                    }
                    ++num2;
                }
            }
            return num1;
        }
    }
}
