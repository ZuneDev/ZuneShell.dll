// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Input.InputInfo
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System.Diagnostics;

namespace Microsoft.Iris.Input
{
    internal abstract class InputInfo
    {
        private static InputInfo.InfoPool[] s_pools;
        protected ICookedInputSite _target;
        protected InputEventType _eventType;
        private byte _lockCount;
        private bool _routeTruncated;
        private bool _handled;

        protected void Initialize(InputEventType eventType)
        {
            this._eventType = eventType;
            this._lockCount = (byte)0;
            this._routeTruncated = false;
            this._handled = false;
        }

        [Conditional("DEBUG")]
        protected void DEBUG_AssertInitialized()
        {
        }

        [Conditional("DEBUG")]
        private void DEBUG_AssertZombied()
        {
        }

        protected static void SetPoolLimitMode(InputInfo.InfoType type, bool keepSingle)
        {
            if (InputInfo.s_pools == null)
                InputInfo.s_pools = new InputInfo.InfoPool[9];
            InputInfo.s_pools[(int)type].SetLimitMode(keepSingle);
        }

        protected abstract InputInfo.InfoType PoolType { get; }

        protected virtual void Zombie()
        {
            this._target = (ICookedInputSite)null;
            this._eventType = InputEventType.Invalid;
        }

        protected static InputInfo GetFromPool(InputInfo.InfoType poolType) => InputInfo.s_pools[(int)poolType].GetPooledInfo();

        public void ReturnToPool()
        {
            if (!this.Poolable)
                return;
            InputInfo.s_pools[(int)this.PoolType].RecycleInfo(this);
        }

        private bool Poolable => this._lockCount == (byte)0;

        public void Lock() => ++this._lockCount;

        public void Unlock()
        {
            --this._lockCount;
            if (this._lockCount != (byte)0)
                return;
            this.ReturnToPool();
        }

        public bool Handled => this._handled;

        public void MarkHandled() => this._handled = true;

        public bool RouteTruncated => this._routeTruncated;

        public ICookedInputSite Target => this._target;

        public virtual InputEventType EventType => this._eventType;

        public void TruncateRoute() => this._routeTruncated = true;

        public virtual void UpdateTarget(ICookedInputSite target) => this._target = target;

        public override string ToString() => this.GetType().Name;

        private struct InfoPool
        {
            private int _numEntries;
            private int _maxEntries;
            private object _storage;

            public void SetLimitMode(bool keepSingle) => this._maxEntries = keepSingle ? 1 : 10;

            public InputInfo GetPooledInfo()
            {
                InputInfo inputInfo = (InputInfo)null;
                if (this._numEntries > 0)
                {
                    inputInfo = this._maxEntries != 1 ? ((InputInfo[])this._storage)[this._numEntries - 1] : (InputInfo)this._storage;
                    --this._numEntries;
                }
                return inputInfo;
            }

            public bool RecycleInfo(InputInfo info)
            {
                bool flag = false;
                if (this._numEntries < this._maxEntries)
                {
                    info.Zombie();
                    if (this._maxEntries == 1)
                    {
                        this._storage = (object)info;
                    }
                    else
                    {
                        if (this._storage == null)
                            this._storage = (object)new InputInfo[this._maxEntries];
                        ((InputInfo[])this._storage)[this._numEntries] = info;
                    }
                    ++this._numEntries;
                    flag = true;
                }
                return flag;
            }
        }

        public enum InfoType
        {
            KeyCommand,
            KeyFocus,
            KeyState,
            KeyCharacter,
            MouseFocus,
            MouseMove,
            MouseButton,
            MouseWheel,
            DragDrop,
            NumTypes,
        }
    }
}
