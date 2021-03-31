// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ClassStateSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ClassStateSchema
    {
        public static UIXTypeSchema Type;

        private static object CallDisposeOwnedObjectObject(object instanceObj, object[] parameters)
        {
            Class @class = (Class)instanceObj;
            object parameter = parameters[0];
            if (parameter == null)
                return (object)null;
            if (!(parameter is IDisposableObject disposable))
            {
                ErrorManager.ReportError("Attempt to dispose an object '{0}' that isn't disposable", (object)TypeSchema.NameFromInstance(parameter));
                return (object)null;
            }
            if (!@class.UnregisterDisposable(ref disposable))
            {
                ErrorManager.ReportError("Attempt to dispose an object '{0}' that '{1}' doesn't own", (object)TypeSchema.NameFromInstance((object)disposable), (object)@class.TypeSchema.Name);
                return (object)null;
            }
            disposable.Dispose((object)@class);
            return (object)null;
        }

        public static void Pass1Initialize() => ClassStateSchema.Type = new UIXTypeSchema((short)30, "ClassState", (string)null, (short)-1, typeof(Class), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)30, "DisposeOwnedObject", new short[1]
            {
        (short) 153
            }, (short)240, new InvokeHandler(ClassStateSchema.CallDisposeOwnedObjectObject), false);
            ClassStateSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, (PropertySchema[])null, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
