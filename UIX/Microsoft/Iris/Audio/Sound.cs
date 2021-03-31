// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Audio.Sound
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI.Audio;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Audio
{
    internal class Sound
    {
        private string _source;
        private SystemSoundEvent _systemSoundEvent;

        internal Sound()
        {
        }

        internal string Source
        {
            get => this._source;
            set
            {
                this._source = value;
                this._systemSoundEvent = SystemSoundEvent.None;
            }
        }

        internal SystemSoundEvent SystemSoundEvent
        {
            get => this._systemSoundEvent;
            set
            {
                this._systemSoundEvent = value;
                this._source = null;
            }
        }

        public void Play()
        {
            if (!Environment.Instance.SoundEffectsEnabled)
                return;
            SoundManager soundManager = UISession.Default.SoundManager;
            ISoundBuffer soundBuffer = null;
            string source = null;
            if (this._source != null)
                source = this._source;
            else if (this._systemSoundEvent != SystemSoundEvent.None)
                source = soundManager.GetSystemSoundEventSource(this._systemSoundEvent);
            if (source != null)
                soundBuffer = soundManager.GetSoundBuffer(source);
            if (soundBuffer == null)
                return;
            ISound sound = soundBuffer.CreateSound(this);
            sound.Play();
            sound.UnregisterUsage(this);
        }
    }
}
