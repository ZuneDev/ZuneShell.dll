// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Animations.ValueTransformer
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System;

namespace Microsoft.Iris.Animations
{
    internal class ValueTransformer
    {
        private float _add;
        private float _subtract;
        private float _multiply;
        private float _divide;
        private float _mod;
        private bool _absolute;

        public ValueTransformer()
        {
            this._multiply = 1f;
            this._divide = 1f;
            this._mod = float.MaxValue;
        }

        public float Transform(float value)
        {
            value *= this._multiply;
            value /= this._divide;
            value += this._add;
            value -= this._subtract;
            if ((double)this._mod != 3.40282346638529E+38)
                value %= this._mod;
            if (this._absolute)
                value = Math.Abs(value);
            return value;
        }

        public float Add
        {
            get => this._add;
            set => this._add = value;
        }

        public float Subtract
        {
            get => this._subtract;
            set => this._subtract = value;
        }

        public float Multiply
        {
            get => this._multiply;
            set => this._multiply = value;
        }

        public float Divide
        {
            get => this._divide;
            set => this._divide = value;
        }

        public float Mod
        {
            get => this._mod;
            set => this._mod = value;
        }

        public bool Absolute
        {
            get => this._absolute;
            set => this._absolute = value;
        }
    }
}
