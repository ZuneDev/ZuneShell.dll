// Decompiled with JetBrains decompiler
// Type: ZuneUI.SlideshowLand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.ComponentModel;

namespace ZuneUI
{
    public class SlideshowLand : PlaybackPage, ISlideShowStateOwner, INotifyPropertyChanged
    {
        private SlideShowState _state;

        public SlideshowLand()
        {
            this.BackgroundUI = SlideshowLand.PhotoSlideshowTemplate;
            this.TransportControlStyle = TransportControlStyle.Photo;
            this.AutoHideToolbars = true;
            this.ShowAppBackground = true;
            this.NoStackPage = true;
        }

        public SlideShowState SlideShowState
        {
            get => this._state;
            set
            {
                if (this._state == value)
                    return;
                this._state = value;
                this.FirePropertyChanged(nameof(SlideShowState));
            }
        }

        public void MoveToNextSlide() => ++this.SlideShowState.Index;

        public void MoveToPreviousSlide() => --this.SlideShowState.Index;

        public void TogglePlayOrPauseSlideshow() => this.SlideShowState.Play = !this.SlideShowState.Play;

        private static string PhotoSlideshowTemplate => "res://ZuneShellResources!PhotoSlideShow.uix#PhotoSlideShow";
    }
}
