// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.AssemblyMethodSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System.Reflection;

namespace Microsoft.Iris.Markup
{
    internal class AssemblyMethodSchema : MethodSchema
    {
        private MethodInfo _methodInfo;
        private TypeSchema[] _parameterTypes;
        private TypeSchema _returnTypeSchema;
        private FastReflectionInvokeHandler _method;

        public AssemblyMethodSchema(
          AssemblyTypeSchema owner,
          MethodInfo methodInfo,
          TypeSchema[] parameterTypes)
          : base(owner)
        {
            this._methodInfo = methodInfo;
            this._parameterTypes = parameterTypes;
            this._returnTypeSchema = AssemblyLoadResult.MapType(this._methodInfo.ReturnType);
        }

        public override string Name => this._methodInfo.Name;

        public override TypeSchema[] ParameterTypes => this._parameterTypes;

        public override TypeSchema ReturnType => this._returnTypeSchema;

        public override bool IsStatic => this._methodInfo.IsStatic;

        public override object Invoke(object instance, object[] parameters)
        {
            object target = AssemblyLoadResult.UnwrapObject(instance);
            object[] paramters = AssemblyLoadResult.UnwrapObjectList(parameters);
            if (this._method == null)
                this._method = ReflectionHelper.CreateMethodInvoke(_methodInfo);
            return AssemblyLoadResult.WrapObject(this._returnTypeSchema, this._method(target, paramters));
        }
    }
}
