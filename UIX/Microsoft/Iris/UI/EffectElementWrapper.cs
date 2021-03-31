// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.UI.EffectElementWrapper
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Drawing;
using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI.VideoPlayback;

namespace Microsoft.Iris.UI
{
    internal class EffectElementWrapper
    {
        private EffectClass _class;
        private string _elementName;
        private static Map<int, string> s_propertyMap;

        public EffectElementWrapper(EffectClass cls, string elementName)
        {
            this._class = cls;
            this._elementName = elementName;
        }

        public void SetProperty(string propertyName, int value) => this._class.SetRenderEffectProperty(this.MakeEffectPropertyName(propertyName), new EffectValue((object)value, EffectValueType.Int));

        public void SetProperty(string propertyName, float value) => this._class.SetRenderEffectProperty(this.MakeEffectPropertyName(propertyName), new EffectValue((object)value, EffectValueType.Float));

        public void SetProperty(string propertyName, UIImage value) => this._class.SetRenderEffectProperty(this.MakeEffectPropertyName(propertyName), new EffectValue((object)value, EffectValueType.UIImage));

        public void SetProperty(string propertyName, IUIVideoStream value) => this._class.SetRenderEffectProperty(this.MakeEffectPropertyName(propertyName), new EffectValue((object)value, EffectValueType.IUIVideoStream));

        public void SetProperty(string propertyName, Color value) => this._class.SetRenderEffectProperty(this.MakeEffectPropertyName(propertyName), new EffectValue((object)value, EffectValueType.Color));

        public void SetProperty(string propertyName, Vector2 value) => this._class.SetRenderEffectProperty(this.MakeEffectPropertyName(propertyName), new EffectValue((object)value, EffectValueType.Vector2));

        public void SetProperty(string propertyName, Vector3 value) => this._class.SetRenderEffectProperty(this.MakeEffectPropertyName(propertyName), new EffectValue((object)value, EffectValueType.Vector3));

        public void PlayAnimation(EffectProperty property, EffectAnimation animation) => this._class.PlayAnimation(this.MakeEffectPropertyName(property), animation);

        private string MakeEffectPropertyName(string propertyName) => EffectElementWrapper.MakeEffectPropertyName(this._elementName, propertyName);

        private string MakeEffectPropertyName(EffectProperty property) => EffectElementWrapper.MakeEffectPropertyName(this._elementName, property);

        public static string MakeEffectPropertyName(string elementName, EffectProperty property)
        {
            EffectElementWrapper.EnsurePropertyMap();
            return EffectElementWrapper.MakeEffectPropertyName(elementName, EffectElementWrapper.s_propertyMap[(int)property]);
        }

        public static string MakeEffectPropertyName(string elementName, string propertyName) => elementName + "." + propertyName;

        private static void EnsurePropertyMap()
        {
            if (EffectElementWrapper.s_propertyMap != null)
                return;
            EffectElementWrapper.s_propertyMap = new Map<int, string>();
            EffectElementWrapper.s_propertyMap[2] = "Attenuation";
            EffectElementWrapper.s_propertyMap[3] = "Brightness";
            EffectElementWrapper.s_propertyMap[4] = "Color";
            EffectElementWrapper.s_propertyMap[14] = "InnerConeAngle";
            EffectElementWrapper.s_propertyMap[18] = "OuterConeAngle";
            EffectElementWrapper.s_propertyMap[5] = "Contrast";
            EffectElementWrapper.s_propertyMap[6] = "DarkColor";
            EffectElementWrapper.s_propertyMap[7] = "Decay";
            EffectElementWrapper.s_propertyMap[8] = "Density";
            EffectElementWrapper.s_propertyMap[9] = "Desaturate";
            EffectElementWrapper.s_propertyMap[10] = "DirectionAngle";
            EffectElementWrapper.s_propertyMap[11] = "EdgeLimit";
            EffectElementWrapper.s_propertyMap[12] = "FallOff";
            EffectElementWrapper.s_propertyMap[13] = "Hue";
            EffectElementWrapper.s_propertyMap[15] = "Intensity";
            EffectElementWrapper.s_propertyMap[16] = "LightColor";
            EffectElementWrapper.s_propertyMap[1] = "AmbientColor";
            EffectElementWrapper.s_propertyMap[17] = "Lightness";
            EffectElementWrapper.s_propertyMap[19] = "Position";
            EffectElementWrapper.s_propertyMap[20] = "Radius";
            EffectElementWrapper.s_propertyMap[21] = "Saturation";
            EffectElementWrapper.s_propertyMap[22] = "Tone";
            EffectElementWrapper.s_propertyMap[23] = "Weight";
            EffectElementWrapper.s_propertyMap[24] = "Value";
            EffectElementWrapper.s_propertyMap[25] = "Downsample";
        }
    }
}
