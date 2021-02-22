// Decompiled with JetBrains decompiler
// Type: ZuneUI.MixTileSaveData
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class MixTileSaveData
    {
        private MixResult _result;
        private object _size;
        private object _position;
        private object _seedSize;

        public MixTileSaveData(MixResult result, object size, object position, object seedSize)
        {
            this._result = result;
            this._size = size;
            this._position = position;
            this._seedSize = seedSize;
        }

        public MixResult Result => this._result;

        public object Size => this._size;

        public object Position => this._position;

        public object SeedSize => this._seedSize;
    }
}
