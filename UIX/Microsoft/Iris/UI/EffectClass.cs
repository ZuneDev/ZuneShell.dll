// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.UI.EffectClass
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;
using System;

namespace Microsoft.Iris.UI
{
    internal class EffectClass : Class
    {
        private IEffectTemplate _effectTemplate;
        private Map<string, EffectValue> _properties;
        private Vector<EffectClass.EffectAndOwner> _effectsInUse = new Vector<EffectClass.EffectAndOwner>();
        private Vector<ActiveSequence> _activeAnimations;

        public EffectClass(MarkupTypeSchema type, IEffectTemplate effectTemplate)
          : base(type)
          => this._effectTemplate = effectTemplate;

        protected override void OnDispose()
        {
            base.OnDispose();
            if (this._activeAnimations != null)
            {
                foreach (DisposableObject activeAnimation in this._activeAnimations)
                    activeAnimation.Dispose(this);
                this._activeAnimations.Clear();
            }
            foreach (EffectClass.EffectAndOwner effectAndOwner in this._effectsInUse)
                effectAndOwner.Effect.UnregisterUsage(this);
            this._effectsInUse.Clear();
        }

        public string DefaultImageElement => ((EffectClassTypeSchema)this.TypeSchema).DefaultElementSymbol;

        public IEffect CreateRenderEffect(object owner)
        {
            IEffect effect = null;
            if (this._effectTemplate != null && this._effectTemplate.IsBuilt)
            {
                effect = this._effectTemplate.CreateInstance(this);
                this._effectsInUse.Add(new EffectClass.EffectAndOwner(effect, owner));
                effect.RegisterUsage(owner);
            }
            if (effect != null && this._properties != null)
            {
                foreach (KeyValueEntry<string, EffectValue> property in this._properties)
                    property.Value.SetValueOnEffect(effect, property.Key);
            }
            return effect;
        }

        public static IEffect CreateImageRenderEffectWithFallback(
          EffectClass effect,
          object owner,
          IImage initialImage)
        {
            IEffect effect1 = null;
            if (effect != null)
            {
                effect1 = effect.CreateRenderEffect(owner);
                if (effect1 != null && effect.DefaultImageElement != null)
                {
                    string stPropertyName = EffectElementWrapper.MakeEffectPropertyName(effect.DefaultImageElement, "Image");
                    effect1.SetProperty(stPropertyName, initialImage);
                }
            }
            if (effect1 == null)
                effect1 = EffectManager.CreateBasicImageEffect(owner, initialImage);
            return effect1;
        }

        public void DoneWithRenderEffects(object owner)
        {
            if (this.IsDisposed)
                return;
            for (int index = this._effectsInUse.Count - 1; index >= 0; --index)
            {
                if (this._effectsInUse[index].Owner == owner)
                {
                    this._effectsInUse[index].Effect.UnregisterUsage(this);
                    this._effectsInUse.RemoveAt(index);
                }
            }
        }

        public static void SetDefaultEffectProperty(
          EffectClass effect,
          IEffect effectInstance,
          IImage image)
        {
            string stPropertyName = null;
            if (effect != null && effect._effectTemplate == effectInstance.Template)
            {
                if (effect.DefaultImageElement != null)
                    stPropertyName = EffectElementWrapper.MakeEffectPropertyName(effect.DefaultImageElement, "Image");
            }
            else
                stPropertyName = "ImageElem.Image";
            if (stPropertyName == null)
                return;
            effectInstance.SetProperty(stPropertyName, image);
        }

        public void SetRenderEffectProperty(string property, EffectValue value)
        {
            if (this._properties == null)
                this._properties = new Map<string, EffectValue>();
            this._properties[property] = value;
            foreach (EffectClass.EffectAndOwner effectAndOwner in this._effectsInUse)
                value.SetValueOnEffect(effectAndOwner.Effect, property);
        }

        public void PlayAnimation(string property, EffectAnimation animation)
        {
            if (this._activeAnimations == null)
                this._activeAnimations = new Vector<ActiveSequence>();
            foreach (EffectClass.EffectAndOwner effectAndOwner in this._effectsInUse)
            {
                AnimationArgs args = new AnimationArgs();
                ActiveSequence instance = animation.CreateInstance(effectAndOwner.Effect, property, ref args);
                instance?.Play();
                instance.DeclareOwner(this);
                instance.AnimationCompleted += new EventHandler(this.OnAnimationComplete);
                this._activeAnimations.Add(instance);
            }
        }

        private void OnAnimationComplete(object sender, EventArgs args)
        {
            ActiveSequence activeSequence = (ActiveSequence)sender;
            activeSequence.AnimationCompleted -= new EventHandler(this.OnAnimationComplete);
            this._activeAnimations.Remove(activeSequence);
            activeSequence.Dispose(this);
        }

        public override object ReadSymbol(SymbolReference symbolRef) => symbolRef.Origin == SymbolOrigin.Techniques ? new EffectElementWrapper(this, symbolRef.Symbol) : base.ReadSymbol(symbolRef);

        private struct EffectAndOwner
        {
            public IEffect Effect;
            public object Owner;

            public EffectAndOwner(IEffect effect, object owner)
            {
                this.Effect = effect;
                this.Owner = owner;
            }
        }
    }
}
