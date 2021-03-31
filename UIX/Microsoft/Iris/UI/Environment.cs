// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.UI.Environment
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.OS;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;
using System;

namespace Microsoft.Iris.UI
{
    internal sealed class Environment : NotifyObjectBase
    {
        private bool _mouseActiveFlag;
        private bool _soundEffectsEnabledFlag;
        private ColorScheme _currentColorScheme;
        private static Environment s_instance;
        private static float s_dpiScale = Math.Max(1f, (float)NativeApi.SpGetDpi() / 96f);

        private Environment() => this._soundEffectsEnabledFlag = true;

        public static Environment Instance
        {
            get
            {
                if (Environment.s_instance == null)
                    Environment.s_instance = new Environment();
                return Environment.s_instance;
            }
        }

        public bool IsMouseActive => this._mouseActiveFlag;

        public void SetIsMouseActive(bool value)
        {
            if (this._mouseActiveFlag == value)
                return;
            this._mouseActiveFlag = value;
        }

        public bool IsRightToLeft => UISession.Default.IsRtl;

        public ColorScheme ColorScheme => this._currentColorScheme;

        public void SetColorScheme(ColorScheme value)
        {
            if (this._currentColorScheme == value)
                return;
            this._currentColorScheme = value;
            this.FireNotification(NotificationID.ColorScheme);
        }

        public bool SoundEffectsEnabled => this._soundEffectsEnabledFlag;

        public void SetSoundEffectsEnabled(bool value)
        {
            if (this._soundEffectsEnabledFlag == value)
                return;
            this._soundEffectsEnabledFlag = value;
        }

        public static float DpiScale => Environment.s_dpiScale;

        public float AnimationSpeed
        {
            get => this.AnimationSystem.SpeedAdjustment;
            set => this.AnimationSystem.SpeedAdjustment = value;
        }

        public int AnimationUpdatesPerSecond
        {
            get => this.AnimationSystem.UpdatesPerSecond;
            set => this.AnimationSystem.UpdatesPerSecond = value;
        }

        private IAnimationSystem AnimationSystem => UISession.Default.RenderSession.AnimationSystem;

        public void AnimationAdvance(int milliseconds) => this.AnimationSystem.PulseTimeAdvance(milliseconds);
    }
}
