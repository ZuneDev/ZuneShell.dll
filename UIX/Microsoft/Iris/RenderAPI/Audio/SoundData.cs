// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.RenderAPI.Audio.SoundData
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.Render.Extensions;
using System;

namespace Microsoft.Iris.RenderAPI.Audio
{
    internal class SoundData : ISoundData, IDisposable
    {
        private string _stSource;
        private Resource _soundResource;
        private uint _openCount;
        private bool _fStreamLoading;
        private bool _fStreamAvailable;
        private ExtensionsApi.HSpSound _soundHandle;
        private ExtensionsApi.SoundInformation _soundInfo;

        internal SoundData(string stSource, Resource soundResource)
        {
            this._stSource = stSource;
            this._soundResource = soundResource;
            this._fStreamAvailable = false;
            this._fStreamLoading = false;
        }

        ~SoundData() => this.Dispose(false);

        public void Dispose()
        {
            GC.SuppressFinalize((object)this);
            this.Dispose(true);
        }

        protected virtual void Dispose(bool fInDispose)
        {
            int num = fInDispose ? 1 : 0;
            if (!(this._soundHandle != ExtensionsApi.HSpSound.NULL))
                return;
            SoundLoader.DisposeData(this._soundHandle, this._soundInfo);
            this._soundHandle = ExtensionsApi.HSpSound.NULL;
        }

        internal bool IsAvailable => this._soundHandle != ExtensionsApi.HSpSound.NULL;

        public bool Load()
        {
            bool flag = false;
            if (this._openCount == 0U && !this._fStreamAvailable && !this._fStreamLoading)
            {
                this._fStreamLoading = true;
                this._soundResource.Acquire(new ResourceAcquisitionCompleteHandler(this.OnContentLoadComplete));
            }
            if (this._soundHandle != ExtensionsApi.HSpSound.NULL)
                flag = true;
            else if (this._fStreamAvailable)
            {
                if (this._soundResource.Status == ResourceStatus.Available)
                {
                    ExtensionsApi.HSpSound soundDataHandle;
                    ExtensionsApi.SoundInformation soundDataInfo;
                    SoundLoader.FromMemory(this._soundResource.Buffer, (int)this._soundResource.Length, out soundDataHandle, out soundDataInfo);
                    this._soundHandle = soundDataHandle;
                    this._soundInfo = soundDataInfo;
                    flag = true;
                }
            }
            else
                flag = false;
            ++this._openCount;
            return flag;
        }

        public void Unload()
        {
            --this._openCount;
            if (this._openCount != 0U)
                return;
            if (this._fStreamAvailable)
            {
                this._soundResource.Free(new ResourceAcquisitionCompleteHandler(this.OnContentLoadComplete));
                this._fStreamAvailable = false;
            }
            if (!(this._soundHandle != ExtensionsApi.HSpSound.NULL))
                return;
            SoundLoader.DisposeData(this._soundHandle, this._soundInfo);
            this._soundHandle = ExtensionsApi.HSpSound.NULL;
        }

        public static string GetCacheKey(string stSource) => InvariantString.Format("SND|{0}", (object)stSource);

        SoundDataFormat ISoundData.Format => (SoundDataFormat)this._soundInfo.Header.wFormatTag;

        uint ISoundData.ChannelCount => (uint)this._soundInfo.Header.nChannels;

        uint ISoundData.SampleRate => this._soundInfo.Header.nSamplesPerSec;

        uint ISoundData.SampleSize => (uint)this._soundInfo.Header.wBitsPerSample;

        uint ISoundData.SampleCount => this._soundInfo.Header.cbDataSize * 8U / (uint)this._soundInfo.Header.wBitsPerSample;

        IntPtr ISoundData.AcquireContent()
        {
            if (this.Load())
                return this._soundInfo.Data.rgData;
            throw new InvalidOperationException("Sound data isn't available");
        }

        void ISoundData.ReleaseContent() => this.Unload();

        private void OnContentLoadComplete(Resource resource)
        {
            this._fStreamLoading = false;
            if (this._soundResource.Status == ResourceStatus.Error)
                this._fStreamAvailable = false;
            else
                this._fStreamAvailable = true;
        }
    }
}
