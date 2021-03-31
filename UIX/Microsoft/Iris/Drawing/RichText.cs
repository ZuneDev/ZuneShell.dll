// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Drawing.RichText
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.OS;
using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI;
using Microsoft.Iris.RenderAPI.Drawing;
using Microsoft.Iris.Session;
using Microsoft.Iris.ViewItems;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Iris.Drawing
{
    internal sealed class RichText : IDisposable
    {
        public const float MaxWidthConstraint = 4095f;
        public const float MaxHeightConstraint = 8191f;
        private Win32Api.HANDLE _rtoHandle;
        private NativeApi.ReportRunCallback _rrcb;
        private string _currentlyMeasuringText;
        private bool _oversampled;
        private bool _hosted;
        private bool _inImeCompositionMode;
        private ArrayList _timers;
        private EventHandler _timerTickHandler;
        private object _lock;

        public RichText(bool richTextMode)
          : this(richTextMode, (IRichTextCallbacks)null)
        {
        }

        public unsafe RichText(bool richTextMode, IRichTextCallbacks callbacks)
        {
            Size sizeMaximumSurface = Size.Zero;
            if (UISession.Default != null)
                sizeMaximumSurface = UIImage.MaximumSurfaceSize(UISession.Default);
            if (callbacks != null)
            {
                this._hosted = true;
                this._timers = new ArrayList(6);
                this._timerTickHandler = new EventHandler(this.OnTimerTick);
            }
            RendererApi.IFC(NativeApi.SpRichTextBuildObject(richTextMode, sizeMaximumSurface, callbacks, out this._rtoHandle));
            this._oversampled = false;
            this._lock = new object();
            this._rrcb = new NativeApi.ReportRunCallback(this.OnReportRun);
        }

        public void Dispose() => this.Dispose(true);

        private void Dispose(bool inDispose)
        {
            if (!inDispose)
                return;
            GC.SuppressFinalize((object)this);
            lock (this._lock)
            {
                NativeApi.SpRichTextDestroyObject(this._rtoHandle);
                this._rtoHandle.h = IntPtr.Zero;
            }
            if (this._timers == null)
                return;
            for (int index = 0; index < this._timers.Count; ++index)
                this.DisposeTimer((DispatcherTimer)this._timers[index]);
            this._timers.Clear();
        }

        ~RichText() => this.Dispose(false);

        public string Content
        {
            set
            {
                lock (this._lock)
                    RendererApi.IFC(NativeApi.SpRichTextSetContent(this._rtoHandle, value));
            }
        }

        public string SimpleContent
        {
            get
            {
                string str = (string)null;
                int textLength = 0;
                lock (this._lock)
                {
                    RendererApi.IFC(NativeApi.SpRichTextGetSimpleContentLength(this._rtoHandle, out textLength));
                    if (textLength != 0)
                    {
                        StringBuilder textBuffer = new StringBuilder(textLength);
                        RendererApi.IFC(NativeApi.SpRichTextGetSimpleContent(this._rtoHandle, textBuffer, textBuffer.Capacity));
                        str = textBuffer.ToString();
                    }
                }
                return str;
            }
        }

        public bool Oversample
        {
            set
            {
                if (this._oversampled == value)
                    return;
                lock (this._lock)
                    RendererApi.IFC(NativeApi.SpRichTextSetOversampleMode(this._rtoHandle, value));
                this._oversampled = value;
            }
            get => this._oversampled;
        }

        public int MaxLength
        {
            set
            {
                lock (this._lock)
                    RendererApi.IFC(NativeApi.SpRichTextSetMaximumLength(this._rtoHandle, value));
            }
        }

        public bool DetectUrls
        {
            set
            {
                lock (this._lock)
                    RendererApi.IFC(NativeApi.SpRichTextSetDetectUrls(this._rtoHandle, value));
            }
        }

        public bool HasCallbacks => this._hosted;

        public bool ReadOnly
        {
            set
            {
                lock (this._lock)
                    RendererApi.IFC(NativeApi.SpRichTextSetReadOnly(this._rtoHandle, value));
            }
        }

        public void SetWordWrap(bool wordWrap)
        {
            lock (this._lock)
                RendererApi.IFC(NativeApi.SpRichTextSetWordWrap(this._rtoHandle, wordWrap));
        }

        public Size GetNaturalBounds()
        {
            int cWidth;
            int cHeight;
            lock (this._lock)
                RendererApi.IFC(NativeApi.SpRichTextGetNaturalBounds(this._rtoHandle, out cWidth, out cHeight));
            return new Size(cWidth, cHeight);
        }

        public void SetSelectionRange(int selectionStart, int selectionEnd)
        {
            lock (this._lock)
                RendererApi.IFC(NativeApi.SpRichTextSetSelectionRange(this._rtoHandle, selectionStart, selectionEnd));
        }

        public unsafe TextFlow Measure(string content, ref TextMeasureParams measureParams)
        {
            TextFlow textFlow = new TextFlow();
            GCHandle gcHandle = GCHandle.Alloc((object)textFlow);
            this._currentlyMeasuringText = content != null ? content : string.Empty;
            fixed (char* content1 = this._currentlyMeasuringText)
            fixed (char* chPtr = measureParams._textStyle.FontFace)
            {
                var style = new TextStyle.MarshalledData(measureParams._textStyle)
                {
                    _fontFace = chPtr
                };
                measureParams.SetContent(content1);
                measureParams._data._pTextStyle = &style;
                fixed (TextMeasureParams.FormattedRange* formattedRangePtr = measureParams._formattedRanges)
                {
                    measureParams._data._pFormattedRanges = formattedRangePtr;
                    fixed (TextStyle.MarshalledData* marshalledDataPtr = measureParams._formattedRangeStyles)
                    {
                        measureParams._data._pFormattedRangeStyles = marshalledDataPtr;
                        lock (this._lock)
                            RendererApi.IFC(NativeApi.SpRichTextMeasure(this._rtoHandle, ref measureParams._data, this._rrcb, GCHandle.ToIntPtr(gcHandle)));
                    }
                }
            }
            gcHandle.Free();
            this._currentlyMeasuringText = (string)null;
            return textFlow;
        }

        public void NotifyOfFocusChange(bool gainingFocus)
        {
            lock (this._lock)
            {
                RendererApi.IFC(NativeApi.SpRichTextSetFocus(this._rtoHandle, gainingFocus));
                if (gainingFocus)
                    return;
                this._inImeCompositionMode = false;
            }
        }

        public void ScrollUp(ScrollbarType whichBar)
        {
            lock (this._lock)
                RendererApi.IFC(NativeApi.SpRichTextScroll(this._rtoHandle, (int)whichBar, 0));
        }

        public void ScrollDown(ScrollbarType whichBar)
        {
            lock (this._lock)
                RendererApi.IFC(NativeApi.SpRichTextScroll(this._rtoHandle, (int)whichBar, 1));
        }

        public void PageUp(ScrollbarType whichBar)
        {
            lock (this._lock)
                RendererApi.IFC(NativeApi.SpRichTextScroll(this._rtoHandle, (int)whichBar, 2));
        }

        public void PageDown(ScrollbarType whichBar)
        {
            lock (this._lock)
                RendererApi.IFC(NativeApi.SpRichTextScroll(this._rtoHandle, (int)whichBar, 3));
        }

        public void ScrollToPosition(ScrollbarType whichBar, int whereTo)
        {
            lock (this._lock)
                RendererApi.IFC(NativeApi.SpRichTextScrollToPosition(this._rtoHandle, (int)whichBar, whereTo));
        }

        public void SetScrollbars(bool horizontalScrollbar, bool verticalScrollbar)
        {
            lock (this._lock)
                RendererApi.IFC(NativeApi.SpRichTextSetScrollbars(this._rtoHandle, horizontalScrollbar, verticalScrollbar));
        }

        public bool ForwardKeyStateNotification(
          uint message,
          int virtualKey,
          int scanCode,
          int repeatCount,
          uint modifierState,
          ushort flags)
        {
            bool handled = true;
            lock (this._lock)
            {
                if (!this._inImeCompositionMode)
                    RendererApi.IFC(NativeApi.SpRichTextForwardKeyState(this._rtoHandle, message, virtualKey, scanCode, repeatCount, modifierState, flags, out handled));
            }
            return handled;
        }

        public bool ForwardKeyCharacterNotification(
          uint message,
          int character,
          int scanCode,
          int repeatCount,
          uint modifierState,
          ushort flags)
        {
            bool handled = true;
            lock (this._lock)
            {
                if (!this._inImeCompositionMode)
                    RendererApi.IFC(NativeApi.SpRichTextForwardKeyCharacter(this._rtoHandle, message, character, scanCode, repeatCount, modifierState, flags, out handled));
            }
            return handled;
        }

        public bool ForwardMouseInput(
          uint message,
          uint modifierState,
          int mouseButton,
          int x,
          int y,
          int mouseWheelDelta)
        {
            bool handled = true;
            lock (this._lock)
            {
                if (!this._inImeCompositionMode)
                    RendererApi.IFC(NativeApi.SpRichTextForwardMouseInput(this._rtoHandle, message, modifierState, mouseButton, x, y, mouseWheelDelta, out handled));
            }
            return handled;
        }

        public HRESULT ForwardImeMessage(uint message, UIntPtr wParam, UIntPtr lParam)
        {
            lock (this._lock)
            {
                switch (message)
                {
                    case 269:
                        this._inImeCompositionMode = true;
                        break;
                    case 270:
                        this._inImeCompositionMode = false;
                        break;
                }
                RendererApi.IFC(NativeApi.SpRichTextForwardImeMessage(this._rtoHandle, message, wParam, lParam));
            }
            return new HRESULT(0);
        }

        public bool CanUndo
        {
            get
            {
                bool canUndo;
                lock (this._lock)
                    RendererApi.IFC(NativeApi.SpRichTextCanUndo(this._rtoHandle, out canUndo));
                return canUndo;
            }
        }

        public void Undo()
        {
            lock (this._lock)
                RendererApi.IFC(NativeApi.SpRichTextUndo(this._rtoHandle));
        }

        public void Cut()
        {
            lock (this._lock)
                RendererApi.IFC(NativeApi.SpRichTextCut(this._rtoHandle));
        }

        public void Copy()
        {
            lock (this._lock)
                RendererApi.IFC(NativeApi.SpRichTextCopy(this._rtoHandle));
        }

        public void Paste()
        {
            lock (this._lock)
                RendererApi.IFC(NativeApi.SpRichTextPaste(this._rtoHandle));
        }

        public void Delete()
        {
            lock (this._lock)
                RendererApi.IFC(NativeApi.SpRichTextDelete(this._rtoHandle));
        }

        public static LineAlignment ReverseAlignment(
          LineAlignment alignment,
          bool condition)
        {
            if (condition)
            {
                switch (alignment)
                {
                    case LineAlignment.Near:
                        alignment = LineAlignment.Far;
                        break;
                    case LineAlignment.Far:
                        alignment = LineAlignment.Near;
                        break;
                }
            }
            return alignment;
        }

        internal static Dib Rasterize(
          IntPtr hGlyphRunInfo,
          bool outlineMode,
          Color textColor,
          bool shadowMode)
        {
            IntPtr phTextBitmap = IntPtr.Zero;
            IntPtr ppvBits;
            Size psizeBitmap;
            RendererApi.IFC(NativeApi.SpRichTextRasterize(hGlyphRunInfo, outlineMode ? 1 : 0, textColor, shadowMode ? 1 : 0, out phTextBitmap, out ppvBits, out psizeBitmap));
            return new Dib(phTextBitmap, ppvBits, psizeBitmap);
        }

        public void SetTimer(uint id, uint timeout)
        {
            DispatcherTimer dispatcherTimer = this.FindTimer(id);
            if (dispatcherTimer == null)
            {
                dispatcherTimer = new DispatcherTimer();
                this._timers.Add((object)dispatcherTimer);
            }
            dispatcherTimer.UserData = (object)id;
            dispatcherTimer.Interval = (int)timeout;
            dispatcherTimer.Tick += this._timerTickHandler;
            dispatcherTimer.Start();
        }

        public void KillTimer(uint id)
        {
            DispatcherTimer timer = this.FindTimer(id);
            if (timer == null)
                return;
            this.DisposeTimer(timer);
            this._timers.Remove((object)timer);
        }

        private DispatcherTimer FindTimer(uint id)
        {
            for (int index = 0; index < this._timers.Count; ++index)
            {
                DispatcherTimer timer = (DispatcherTimer)this._timers[index];
                if ((int)(uint)timer.UserData == (int)id)
                    return timer;
            }
            return (DispatcherTimer)null;
        }

        private void DisposeTimer(DispatcherTimer timer)
        {
            timer.Tick -= this._timerTickHandler;
            timer.Stop();
        }

        private void OnTimerTick(object sender, EventArgs ea)
        {
            DispatcherTimer dispatcherTimer = (DispatcherTimer)sender;
            lock (this._lock)
                RendererApi.IFC(NativeApi.SpRichTextOnTimerTick(this._rtoHandle, (uint)dispatcherTimer.UserData));
        }

        private unsafe HRESULT OnReportRun(
          IntPtr hGlyphRunInfo,
          NativeApi.RasterizeRunPacket* runPacketPtr,
          IntPtr lpString,
          uint nChars,
          IntPtr dataPtr)
        {
            bool flag = false;
            if (this._currentlyMeasuringText != null && (long)this._currentlyMeasuringText.Length == (long)nChars)
            {
                flag = true;
                char* pointer = (char*)lpString.ToPointer();
                for (int index = 0; (long)index < (long)nChars; ++index)
                {
                    if ((int)this._currentlyMeasuringText[index] != (int)pointer[index])
                    {
                        flag = false;
                        break;
                    }
                }
            }
            string content = flag ? this._currentlyMeasuringText : Marshal.PtrToStringUni(lpString, (int)nChars);
            TextRun run = TextRun.FromRunPacket(hGlyphRunInfo, runPacketPtr, content);
            if (run != null)
                ((TextFlow)GCHandle.FromIntPtr(dataPtr).Target).Add(run);
            return new HRESULT(0);
        }
    }
}
