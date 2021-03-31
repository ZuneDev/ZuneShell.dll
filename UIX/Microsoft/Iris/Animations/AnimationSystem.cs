// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Animations.AnimationSystem
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Iris.Animations
{
    internal static class AnimationSystem
    {
        private static bool _enabledFlag = true;
        private static bool _overrideToFalseFlag;
        private static int _disableAnimationCount;
        private static Dictionary<string, AnimationTemplate> _sequences = new Dictionary<string, AnimationTemplate>();

        public static void ValidateAnimationType(AnimationType paramType)
        {
        }

        public static bool SequenceExists(string id) => AnimationSystem._sequences.ContainsKey(id);

        public static AnimationTemplate GetSequenceByID(string id)
        {
            if (!AnimationSystem.Enabled)
                return (AnimationTemplate)null;
            return AnimationSystem.SequenceExists(id) ? (AnimationTemplate)AnimationSystem._sequences[id].Clone() : (AnimationTemplate)null;
        }

        public static AnimationTemplate GetSequenceByIDAlways(string id) => AnimationSystem.SequenceExists(id) ? (AnimationTemplate)AnimationSystem._sequences[id].Clone() : (AnimationTemplate)null;

        public static void AddSequenceByID(string id, AnimationTemplate seq)
        {
            if (AnimationSystem.SequenceExists(id))
                return;
            AnimationSystem._sequences.Add(id, seq);
        }

        public static void ClearSequences() => AnimationSystem._sequences = new Dictionary<string, AnimationTemplate>();

        public static ICollection GetAllSequences() => (ICollection)AnimationSystem._sequences.Values;

        public static bool Enabled => AnimationSystem._enabledFlag;

        public static void SetEnableState(bool value) => AnimationSystem._enabledFlag = value;

        public static void OverrideAnimationState(bool overrideToFalseFlag)
        {
            if (AnimationSystem._overrideToFalseFlag == overrideToFalseFlag)
                return;
            AnimationSystem._overrideToFalseFlag = overrideToFalseFlag;
            AnimationSystem.UpdateAnimationState();
        }

        public static void UpdateAnimationState()
        {
            bool flag = true;
            if (AnimationSystem._disableAnimationCount > 0 || AnimationSystem._overrideToFalseFlag)
                flag = false;
            AnimationSystem.SetEnableState(flag);
        }

        public static void PushDisableAnimations()
        {
            ++AnimationSystem._disableAnimationCount;
            if (AnimationSystem._disableAnimationCount != 1)
                return;
            AnimationSystem.UpdateAnimationState();
        }

        public static void PopDisableAnimations()
        {
            --AnimationSystem._disableAnimationCount;
            if (AnimationSystem._disableAnimationCount != 0)
                return;
            AnimationSystem.UpdateAnimationState();
        }
    }
}
