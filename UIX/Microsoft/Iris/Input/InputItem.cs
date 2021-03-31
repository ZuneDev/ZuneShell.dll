// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Input.InputItem
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Debug;
using Microsoft.Iris.Queues;

namespace Microsoft.Iris.Input
{
    internal class InputItem : QueueItem
    {
        private const int c_maxCacheCount = 5;
        private static InputItem s_cache;
        private static int s_cachedCount;
        private InputManager _manager;
        private ICookedInputSite _target;
        private InputInfo _info;

        private InputItem()
        {
        }

        public static InputItem Create(
          InputManager manager,
          ICookedInputSite target,
          InputInfo info)
        {
            InputItem inputItem = InputItem.AllocateFromPool();
            inputItem._manager = manager;
            inputItem._target = target;
            inputItem._info = info;
            return inputItem;
        }

        private static InputItem AllocateFromPool()
        {
            InputItem inputItem = (InputItem)null;
            if (InputItem.s_cache != null)
            {
                inputItem = InputItem.s_cache;
                InputItem.s_cache = (InputItem)inputItem._next;
                inputItem._next = (QueueItem)null;
                --InputItem.s_cachedCount;
            }
            if (inputItem == null)
                inputItem = new InputItem();
            return inputItem;
        }

        public void ReturnToPool() => this.ReturnToPool(true);

        private void ReturnToPool(bool returnInfoToo)
        {
            if (returnInfoToo)
                this._info.ReturnToPool();
            this._manager = (InputManager)null;
            this._target = (ICookedInputSite)null;
            this._info = (InputInfo)null;
            this._prev = (QueueItem)null;
            this._next = (QueueItem)null;
            this._owner = (QueueItem.Chain)null;
            if (InputItem.s_cachedCount >= 5)
                return;
            this._next = (QueueItem)InputItem.s_cache;
            InputItem.s_cache = this;
            ++InputItem.s_cachedCount;
        }

        public InputManager Manager => this._manager;

        public InputInfo Info => this._info;

        public override string ToString() => base.ToString() + " -> " + (object)this._info + " -> " + DebugHelpers.DEBUG_ObjectToString((object)this._target);

        public override void Dispatch()
        {
            if (this._info.Target == null)
                this._info.UpdateTarget(this._target);
            this._info.Lock();
            this._manager.DeliverInput(this._target, this._info);
            this._info.Unlock();
            this.ReturnToPool(false);
        }

        internal void UpdateInputSite(ICookedInputSite target) => this._target = target;
    }
}
