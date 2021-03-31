// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Animations.TransformAnimation
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System.Collections;

namespace Microsoft.Iris.Animations
{
    internal class TransformAnimation : ReferenceAnimation
    {
        private float _timeScaleValue;
        private float _timeOffsetValue;
        private float _magnitudeValue;
        private KeyframeFilter _filter;
        private AnimationTemplate _cacheAnimation;

        public TransformAnimation()
        {
            this._timeScaleValue = 1f;
            this._magnitudeValue = 1f;
            this._filter = KeyframeFilter.All;
        }

        public float TimeScale
        {
            get => this._timeScaleValue;
            set
            {
                if ((double)this._timeScaleValue == (double)value)
                    return;
                this._timeScaleValue = value;
                this.ClearCache();
            }
        }

        public float Delay
        {
            get => this._timeOffsetValue;
            set
            {
                if ((double)this._timeOffsetValue == (double)value)
                    return;
                this._timeOffsetValue = value;
                this.ClearCache();
            }
        }

        public float Magnitude
        {
            get => this._magnitudeValue;
            set
            {
                if ((double)this._magnitudeValue == (double)value)
                    return;
                this._magnitudeValue = value;
                this.ClearCache();
            }
        }

        public KeyframeFilter Filter
        {
            get => this._filter;
            set
            {
                if (this._filter == value)
                    return;
                this._filter = value;
                this.ClearCache();
            }
        }

        protected override AnimationTemplate BuildWorker(ref AnimationArgs args)
        {
            if (this._cacheAnimation != null)
                return this._cacheAnimation;
            float timeScale = this.GetTimeScale(ref args);
            float delayTime = this.GetDelayTime(ref args);
            float magnitude = this.GetMagnitude(ref args);
            bool flag1 = (double)timeScale != 1.0;
            bool flag2 = (double)delayTime != 0.0;
            bool flag3 = (double)magnitude != 1.0;
            int filter = (int)this._filter;
            AnimationTemplate anim1 = base.BuildWorker(ref args);
            TransformAnimation.DumpAnimation(anim1, "Source");
            if (!flag1 && !flag2 && !flag3)
                return anim1;
            AnimationTemplate anim2 = (AnimationTemplate)anim1.Clone();
            anim2.DebugID += "'";
            if (flag1)
                this.ApplyTimeScale(anim2, timeScale);
            if (flag2)
                this.ApplyTimeOffset(anim2, delayTime);
            if (flag3)
                this.ApplyMagnitude(anim2, magnitude);
            if (this.CanCache)
                this._cacheAnimation = anim2;
            return anim2;
        }

        internal static void DumpAnimation(AnimationTemplate anim, string descriptionName)
        {
            foreach (BaseKeyframe keyframe in anim.Keyframes)
                ;
        }

        protected virtual float GetTimeScale(ref AnimationArgs args) => this._timeScaleValue;

        protected virtual float GetDelayTime(ref AnimationArgs args) => this._timeOffsetValue;

        protected virtual float GetMagnitude(ref AnimationArgs args) => this._magnitudeValue;

        private void ApplyTimeScale(AnimationTemplate anim, float timeScaleValue)
        {
            foreach (BaseKeyframe keyframe in anim.Keyframes)
            {
                if (this.ShouldApplyTransform(keyframe))
                    keyframe.Time *= timeScaleValue;
            }
            TransformAnimation.DumpAnimation(anim, "Result");
        }

        private void ApplyTimeOffset(AnimationTemplate anim, float timeOffsetValue)
        {
            ArrayList arrayList = new ArrayList();
            foreach (BaseKeyframe keyframe in anim.Keyframes)
            {
                if (this.ShouldApplyTransform(keyframe))
                {
                    if ((double)keyframe.Time == 0.0)
                        arrayList.Add((object)keyframe.Clone());
                    keyframe.Time += timeOffsetValue;
                }
            }
            foreach (BaseKeyframe key in arrayList)
                anim.AddKeyframe(key);
            TransformAnimation.DumpAnimation(anim, "Result");
        }

        private void ApplyMagnitude(AnimationTemplate anim, float magnitudeValue)
        {
            foreach (BaseKeyframe keyframe in anim.Keyframes)
            {
                if (this.ShouldApplyTransform(keyframe))
                    keyframe.MagnifyValue(magnitudeValue);
            }
            TransformAnimation.DumpAnimation(anim, "Result");
        }

        private KeyframeFilter GetKeyframeFilter(BaseKeyframe key)
        {
            switch (key.Type)
            {
                case AnimationType.Position:
                    return KeyframeFilter.Position;
                case AnimationType.Size:
                    return KeyframeFilter.Size;
                case AnimationType.Alpha:
                    return KeyframeFilter.Alpha;
                case AnimationType.Scale:
                    return KeyframeFilter.Scale;
                case AnimationType.Rotate:
                    return KeyframeFilter.Rotate;
                default:
                    return KeyframeFilter.All;
            }
        }

        private bool ShouldApplyTransform(BaseKeyframe key) => this._filter == KeyframeFilter.All || this.GetKeyframeFilter(key) == this._filter;

        protected override void OnSourceChanged() => this.ClearCache();

        protected void ClearCache() => this._cacheAnimation = (AnimationTemplate)null;
    }
}
