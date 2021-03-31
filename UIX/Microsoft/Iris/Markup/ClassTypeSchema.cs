// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.ClassTypeSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup.UIX;
using Microsoft.Iris.UI;
using System;

namespace Microsoft.Iris.Markup
{
    internal class ClassTypeSchema : MarkupTypeSchema
    {
        private bool _isShared;
        private Class _sharedInstance;

        public ClassTypeSchema(MarkupLoadResult owner, string name)
          : base(owner, name)
        {
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            if (this._sharedInstance == null)
                return;
            this._sharedInstance.Dispose((object)this);
            this._sharedInstance = (Class)null;
        }

        public override MarkupType MarkupType => MarkupType.Class;

        protected override TypeSchema DefaultBase => (TypeSchema)ObjectSchema.Type;

        public override Type RuntimeType => typeof(Class);

        public override object ConstructDefault() => this._isShared ? (object)this.SharedInstance : (object)this.ConstructNewInstance();

        public override void InitializeInstance(ref object instance) => this.InitializeInstance((IMarkupTypeBase)instance);

        public override bool HasInitializer => !this._isShared;

        public override bool Disposable => !this._isShared && base.Disposable;

        public bool IsShared => this._isShared;

        public void MarkShareable() => this._isShared = true;

        public Class SharedInstance
        {
            get
            {
                if (!this._isShared)
                    return (Class)null;
                if (this._sharedInstance == null)
                {
                    this._sharedInstance = this.ConstructNewInstance();
                    this._sharedInstance.DeclareOwner((object)this);
                    this.InitializeInstance((IMarkupTypeBase)this._sharedInstance);
                }
                return this._sharedInstance;
            }
        }

        protected virtual Class ConstructNewInstance() => new Class((MarkupTypeSchema)this);
    }
}
