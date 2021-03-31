// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ViewItems.Index
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.ViewItems
{
    internal class Index : NotifyObjectBase
    {
        private int _virtualIndex;
        private int _dataIndex;
        private Repeater _repeater;

        public Index(int virtualIndex, int dataIndex, Repeater repeater)
        {
            this._virtualIndex = virtualIndex;
            this._dataIndex = dataIndex;
            this._repeater = repeater;
        }

        public int Value => this._virtualIndex;

        public int SourceValue => this._dataIndex;

        public void SetValue(int virtualIndex, int dataIndex)
        {
            if (this._virtualIndex != virtualIndex)
            {
                this._virtualIndex = virtualIndex;
                this.FireNotification(NotificationID.Value);
            }
            if (this._dataIndex == dataIndex)
                return;
            this._dataIndex = dataIndex;
            this.FireNotification(NotificationID.SourceValue);
        }

        public Index GetContainerIndex()
        {
            ViewItem viewItem = _repeater;
            while (!(viewItem.Parent is Repeater))
            {
                viewItem = viewItem.Parent;
                if (viewItem == null)
                    return null;
            }
            return ((IndexLayoutInput)viewItem.GetLayoutInput(IndexLayoutInput.Data)).Index;
        }
    }
}
