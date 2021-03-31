// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Animations.RelativeTo
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Render;

namespace Microsoft.Iris.Animations
{
    internal class RelativeTo
    {
        private IAnimatable _sourceObject;
        private int _sourceId;
        private string _sourceProperty;
        private SnapshotPolicy _snapshot;
        private int _power = 1;
        private float _multiply = 1f;
        private float _add;
        private static RelativeTo s_absolute = new RelativeTo();
        private static RelativeTo s_current = new RelativeTo(SnapshotPolicy.Once);
        private static RelativeTo s_currentSnapshotOnLoop = new RelativeTo(SnapshotPolicy.OnLoop);
        private static RelativeTo s_final = new RelativeTo();

        public RelativeTo() => this._snapshot = SnapshotPolicy.Continuous;

        public RelativeTo(SnapshotPolicy snapshot) => this._snapshot = snapshot;

        public bool IsRelativeToObject => this._sourceObject != null || this._sourceId != 0 || this == RelativeTo.s_current || this == RelativeTo.s_currentSnapshotOnLoop;

        public IAnimatable Source
        {
            get => this._sourceObject;
            set => this._sourceObject = value;
        }

        public int SourceId
        {
            get => this._sourceId;
            set => this._sourceId = value;
        }

        public string Property
        {
            get => this._sourceProperty;
            set => this._sourceProperty = value;
        }

        public SnapshotPolicy Snapshot
        {
            get => this._snapshot;
            set => this._snapshot = value;
        }

        public int Power
        {
            get => this._power;
            set => this._power = value;
        }

        public float Multiply
        {
            get => this._multiply;
            set => this._multiply = value;
        }

        public float Add
        {
            get => this._add;
            set => this._add = value;
        }

        public static RelativeTo Absolute => RelativeTo.s_absolute;

        public static RelativeTo Current => RelativeTo.s_current;

        public static RelativeTo CurrentSnapshotOnLoop => RelativeTo.s_currentSnapshotOnLoop;

        public static RelativeTo Final => RelativeTo.s_final;

        public AnimationInput CreateAnimationInput(
          IAnimatable defaultSource,
          string defaultSourceProperty,
          string defaultSourcePropertyMask)
        {
            IAnimatable sourceObject = null;
            string sourcePropertyName = null;
            string sourceMaskSpec = null;
            bool flag = true;
            if (this._sourceProperty != null)
            {
                sourceObject = this._sourceId == 0 ? this._sourceObject : Application.MapExternalAnimationInput(this._sourceId);
                if (sourceObject != null)
                {
                    sourcePropertyName = this._sourceProperty;
                    flag = false;
                }
            }
            if (flag)
            {
                sourceObject = defaultSource;
                sourcePropertyName = defaultSourceProperty;
                sourceMaskSpec = defaultSourcePropertyMask;
            }
            AnimationInput animationInput1;
            if (this.Snapshot == SnapshotPolicy.Continuous)
            {
                animationInput1 = new ContinuousAnimationInput(sourceObject, sourcePropertyName, sourceMaskSpec);
            }
            else
            {
                CapturedAnimationInput capturedAnimationInput = new CapturedAnimationInput(sourceObject, sourcePropertyName, sourceMaskSpec);
                if (this.Snapshot == SnapshotPolicy.OnLoop)
                    capturedAnimationInput.RefreshOnRepeat = true;
                animationInput1 = capturedAnimationInput;
            }
            AnimationInput animationInput2 = animationInput1;
            for (int index = 1; index < this._power; ++index)
                animationInput2 *= animationInput1;
            if (_multiply != 1.0)
                animationInput2 *= new ConstantAnimationInput(this._multiply);
            if (_add != 0.0)
                animationInput2 += new ConstantAnimationInput(this._add);
            return animationInput2;
        }

        public override string ToString()
        {
            if (this == RelativeTo.s_absolute)
                return "Absolute";
            if (this == RelativeTo.s_current)
                return "Current";
            if (this == RelativeTo.s_currentSnapshotOnLoop)
                return "CurrentSnapshotOnLoop";
            return this == RelativeTo.s_final ? "Final" : string.Format("[Object = {0}, Property = {1}]", this._sourceObject != null ? _sourceObject : (object)this._sourceId, _sourceProperty);
        }
    }
}
