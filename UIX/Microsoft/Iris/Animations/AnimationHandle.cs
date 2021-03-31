// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Animations.AnimationHandle
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using System;

namespace Microsoft.Iris.Animations
{
    internal class AnimationHandle : NotifyObjectBase
    {
        private int _playing;

        public event EventHandler Completed;

        public bool Playing => this._playing > 0;

        internal void AssociateWithAnimationInstance(ActiveSequence anim)
        {
            anim.AnimationCompleted += new EventHandler(this.OnAnimationCompleted);
            ++this._playing;
            if (this._playing != 1)
                return;
            this.FireNotification(NotificationID.Playing);
        }

        internal void FireCompleted()
        {
            if (this.Completed != null)
                this.Completed(this, EventArgs.Empty);
            this.FireNotification(NotificationID.Completed);
        }

        private void OnAnimationCompleted(object sender, EventArgs args)
        {
            ((ActiveSequence)sender).AnimationCompleted -= new EventHandler(this.OnAnimationCompleted);
            --this._playing;
            if (this._playing != 0)
                return;
            this.FireNotification(NotificationID.Playing);
            this.FireCompleted();
        }
    }
}
