// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ViewItems.TextFlowRenderingHelper
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Render;

namespace Microsoft.Iris.ViewItems
{
    internal class TextFlowRenderingHelper
    {
        private IGradient _gradientMultiLine;
        private IGradient _gradientClipLeftRight;

        public void InvalidateGradients()
        {
        }

        public void CreateFadeGradients(Text textViewItem, IVisualContainer visContainer)
        {
            if (visContainer == null)
                return;
            this.DisposeFadeGradients(visContainer);
            textViewItem.CreateFadeGradientsHelper(ref this._gradientClipLeftRight, ref this._gradientMultiLine);
            if (this._gradientClipLeftRight != null)
                visContainer.AddGradient(this._gradientClipLeftRight);
            if (this._gradientMultiLine == null)
                return;
            visContainer.AddGradient(this._gradientMultiLine);
        }

        private void DisposeFadeGradients(IVisualContainer visContainer)
        {
            visContainer.RemoveAllGradients();
            if (this._gradientMultiLine != null)
            {
                this._gradientMultiLine.UnregisterUsage(this);
                this._gradientMultiLine = null;
            }
            if (this._gradientClipLeftRight == null)
                return;
            this._gradientClipLeftRight.UnregisterUsage(this);
            this._gradientClipLeftRight = null;
        }
    }
}
