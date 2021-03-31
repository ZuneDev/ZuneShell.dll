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
            InputItem inputItem = AllocateFromPool();
            inputItem._manager = manager;
            inputItem._target = target;
            inputItem._info = info;
            return inputItem;
        }

        private static InputItem AllocateFromPool()
        {
            InputItem inputItem = null;
            if (s_cache != null)
            {
                inputItem = s_cache;
                s_cache = (InputItem)inputItem._next;
                inputItem._next = null;
                --s_cachedCount;
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
            this._manager = null;
            this._target = null;
            this._info = null;
            this._prev = null;
            this._next = null;
            this._owner = null;
            if (s_cachedCount >= 5)
                return;
            this._next = s_cache;
            s_cache = this;
            ++s_cachedCount;
        }

        public InputManager Manager => this._manager;

        public InputInfo Info => this._info;

        public override string ToString() => base.ToString() + " -> " + _info + " -> " + DebugHelpers.DEBUG_ObjectToString(_target);

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
