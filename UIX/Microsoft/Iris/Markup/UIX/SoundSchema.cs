// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.SoundSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Audio;
using Microsoft.Iris.Data;
using Microsoft.Iris.Library;
using Microsoft.Iris.RenderAPI.Audio;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class SoundSchema
    {
        public static UIXTypeSchema Type;

        private static object GetSource(object instanceObj) => (object)((Sound)instanceObj).Source;

        private static void SetSource(ref object instanceObj, object valueObj) => ((Sound)instanceObj).Source = (string)valueObj;

        private static object GetSystemSoundEvent(object instanceObj) => (object)((Sound)instanceObj).SystemSoundEvent;

        private static void SetSystemSoundEvent(ref object instanceObj, object valueObj) => ((Sound)instanceObj).SystemSoundEvent = (SystemSoundEvent)valueObj;

        private static object Construct() => (object)new Sound();

        private static object ConstructSource(object[] parameters)
        {
            object instanceObj = SoundSchema.Construct();
            SoundSchema.SetSource(ref instanceObj, parameters[0]);
            return instanceObj;
        }

        private static Result ConvertFromStringSource(string[] splitString, out object instance)
        {
            instance = SoundSchema.Construct();
            object valueObj;
            Result result = UIXLoadResult.ValidateStringAsValue(splitString[0], (TypeSchema)StringSchema.Type, (RangeValidator)null, out valueObj);
            if (result.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Sound", (object)result.Error);
            SoundSchema.SetSource(ref instance, valueObj);
            return result;
        }

        private static object CallPlay(object instanceObj, object[] parameters)
        {
            ((Sound)instanceObj).Play();
            return (object)null;
        }

        private static bool IsConversionSupported(TypeSchema fromType) => StringSchema.Type.IsAssignableFrom(fromType);

        private static Result TryConvertFrom(
          object from,
          TypeSchema fromType,
          out object instance)
        {
            Result result = Result.Fail("Unsupported");
            instance = (object)null;
            if (StringSchema.Type.IsAssignableFrom(fromType))
            {
                string[] splitString = StringUtility.SplitAndTrim(',', (string)from);
                if (splitString.Length == 1)
                {
                    result = SoundSchema.ConvertFromStringSource(splitString, out instance);
                    if (!result.Failed)
                        return result;
                }
                else
                    result = Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)from.ToString(), (object)"Sound");
            }
            return result;
        }

        public static void Pass1Initialize() => SoundSchema.Type = new UIXTypeSchema((short)201, "Sound", (string)null, (short)153, typeof(Sound), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)201, "Source", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(SoundSchema.GetSource), new SetValueHandler(SoundSchema.SetSource), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)201, "SystemSoundEvent", (short)211, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(SoundSchema.GetSystemSoundEvent), new SetValueHandler(SoundSchema.SetSystemSoundEvent), false);
            UIXConstructorSchema constructorSchema = new UIXConstructorSchema((short)201, new short[1]
            {
        (short) 208
            }, new ConstructHandler(SoundSchema.ConstructSource));
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)201, "Play", (short[])null, (short)240, new InvokeHandler(SoundSchema.CallPlay), false);
            SoundSchema.Type.Initialize(new DefaultConstructHandler(SoundSchema.Construct), new ConstructorSchema[1]
            {
        (ConstructorSchema) constructorSchema
            }, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, new TypeConverterHandler(SoundSchema.TryConvertFrom), new SupportsTypeConversionHandler(SoundSchema.IsConversionSupported), (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
