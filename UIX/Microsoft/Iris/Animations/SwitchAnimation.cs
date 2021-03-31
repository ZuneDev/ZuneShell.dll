// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Animations.SwitchAnimation
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.ModelItems;
using System.Collections.Generic;

namespace Microsoft.Iris.Animations
{
    internal class SwitchAnimation : IAnimationProvider
    {
        private IUIValueRange _expressionObject;
        private Dictionary<string, IAnimationProvider> _optionsList;
        private AnimationEventType _type;

        public IUIValueRange Expression
        {
            get => this._expressionObject;
            set => this._expressionObject = value;
        }

        public Dictionary<string, IAnimationProvider> Options
        {
            get
            {
                if (this._optionsList == null)
                    this._optionsList = new Dictionary<string, IAnimationProvider>();
                return this._optionsList;
            }
            set => this._optionsList = value;
        }

        public AnimationEventType Type
        {
            get => this._type;
            set => this._type = value;
        }

        public AnimationTemplate Build(ref AnimationArgs args)
        {
            AnimationTemplate animationTemplate = (AnimationTemplate)null;
            if (this._optionsList != null)
            {
                object obj = (object)null;
                if (this._expressionObject != null)
                    obj = this._expressionObject.ObjectValue;
                string key = (string)null;
                if (obj != null)
                    key = obj.ToString();
                IAnimationProvider animationProvider = (IAnimationProvider)null;
                if (key != null && this._optionsList.ContainsKey(key))
                    animationProvider = this._optionsList[key];
                if (animationProvider != null)
                    animationTemplate = animationProvider.Build(ref args);
            }
            return animationTemplate;
        }

        public bool CanCache => false;
    }
}
