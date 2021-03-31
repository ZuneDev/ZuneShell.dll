// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Animations.RendererProperty
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Animations
{
    internal struct RendererProperty
    {
        private string _property;
        private string _sourceMask;
        private string _targetMask;

        public RendererProperty(string property)
        {
            this._property = property;
            this._sourceMask = null;
            this._targetMask = null;
        }

        public RendererProperty(string property, string sourceMask, string targetMask)
        {
            this._property = property;
            this._sourceMask = sourceMask;
            this._targetMask = targetMask;
        }

        public string Property => this._property;

        public string SourceMask => this._sourceMask;

        public string TargetMask => this._targetMask;
    }
}
