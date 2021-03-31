// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.CodeModel.Cpp.DllEventSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup;
using Microsoft.Iris.OS;
using System;
using System.Diagnostics;

namespace Microsoft.Iris.CodeModel.Cpp
{
    internal class DllEventSchema : EventSchema
    {
        private string _name;
        private bool _isStatic;

        public DllEventSchema(DllTypeSchema owner, uint ID)
          : base(owner)
        {
        }

        public bool Load(IntPtr eventSchema) => this.QueryEventName(eventSchema) && this.QueryIsStatic(eventSchema);

        [Conditional("DEBUG")]
        public void DEBUG_Dump()
        {
        }

        public override string Name => this._name;

        public bool IsStatic => this._isStatic;

        private unsafe bool QueryEventName(IntPtr eventSchema)
        {
            bool flag = false;
            char* name;
            if (this.CheckNativeReturn(NativeApi.SpQueryEventName(eventSchema, out name)))
            {
                this._name = NotifyService.CanonicalizeString(new string(name));
                flag = true;
            }
            return flag;
        }

        private bool QueryIsStatic(IntPtr eventSchema) => this.CheckNativeReturn(NativeApi.SpQueryEventIsStatic(eventSchema, out this._isStatic));

        private bool CheckNativeReturn(uint hr) => DllLoadResult.CheckNativeReturn(hr, "IUIXEvent");
    }
}
