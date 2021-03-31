﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.RenderAPI.VideoPlayback.VideoPresentationBuilder
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.RenderAPI.Drawing;
using System;

namespace Microsoft.Iris.RenderAPI.VideoPlayback
{
    internal sealed class VideoPresentationBuilder
    {
        private SizeF m_sizefOriginalSource;
        private RectangleF m_rcfInputDestViewPxl;
        private SizeF m_sizefInputDestAspect;
        private float m_flInputDisplayOverscanPer;
        private float m_flInputContentOverscanPer;
        private VideoZoomHandler m_handlerZoomMode;
        private VideoDisplayMode m_nDisplayMode;
        private VideoOverscanMode m_nInputOverscanMode;
        private SizeF m_sizefInputContentAspect;
        private bool m_fInputFullDisplay;

        public VideoPresentationBuilder()
        {
            this.m_handlerZoomMode = LinearVideoStretch.ShrinkToFit;
            this.m_sizefInputContentAspect = new SizeF(1f, 1f);
            this.m_sizefInputDestAspect = new SizeF(1f, 1f);
        }

        public SizeF SourceDimensions
        {
            get => this.m_sizefOriginalSource;
            set => this.m_sizefOriginalSource = value;
        }

        public RectangleF CompleteDestination
        {
            get => this.m_rcfInputDestViewPxl;
            set => this.m_rcfInputDestViewPxl = value;
        }

        public SizeF DestinationAspectRatio
        {
            get => this.m_sizefInputDestAspect;
            set => this.m_sizefInputDestAspect = value;
        }

        public float DisplayOverscanFactor
        {
            get => this.m_flInputDisplayOverscanPer;
            set => this.m_flInputDisplayOverscanPer = value;
        }

        public float ContentOverscanFactor
        {
            get => this.m_flInputContentOverscanPer;
            set => this.m_flInputContentOverscanPer = value;
        }

        public VideoOverscanMode ContentOverscanMode
        {
            get => this.m_nInputOverscanMode;
            set => this.m_nInputOverscanMode = value;
        }

        public SizeF ContentAspectRatio
        {
            get => this.m_sizefInputContentAspect;
            set => this.m_sizefInputContentAspect = value;
        }

        public VideoZoomHandler ZoomMode
        {
            get => this.m_handlerZoomMode;
            set => this.m_handlerZoomMode = value;
        }

        public bool IsFullDisplay
        {
            get => this.m_fInputFullDisplay;
            set => this.m_fInputFullDisplay = value;
        }

        public VideoDisplayMode DisplayMode
        {
            get => this.m_nDisplayMode;
            set => this.m_nDisplayMode = value;
        }

        public BasicVideoPresentation BuildPresentation()
        {
            BasicVideoGeometry geometry = new BasicVideoGeometry();
            if ((double)this.m_rcfInputDestViewPxl.Width > 0.0 && (double)this.m_rcfInputDestViewPxl.Height > 0.0)
            {
                RectangleF rcfBoundSrcVideoPxl;
                RectangleF rcfBoundDestViewPxl;
                float flSrcWidthMultiplier;
                float flDstWidthMultiplier;
                this.ComputeLocations(out rcfBoundSrcVideoPxl, out rcfBoundDestViewPxl, out flSrcWidthMultiplier, out flDstWidthMultiplier);
                this.m_handlerZoomMode(VideoPresentationBuilder.ConvertToSquare(rcfBoundSrcVideoPxl, flSrcWidthMultiplier), VideoPresentationBuilder.ConvertToSquare(rcfBoundDestViewPxl, flDstWidthMultiplier), out geometry.arrcfSrcVideo, out geometry.arrcfDestView);
                if (geometry.arrcfDestView != null)
                {
                    int length = geometry.arrcfDestView.Length;
                }
                int num1 = geometry.arrcfDestView != null ? geometry.arrcfDestView.Length : 0;
                VideoPresentationBuilder.ConvertFromSquare(geometry.arrcfSrcVideo, flSrcWidthMultiplier);
                VideoPresentationBuilder.ConvertFromSquare(geometry.arrcfDestView, flDstWidthMultiplier);
                geometry.rcfSrcVideoBounds = VideoPresentationBuilder.ComputeBounds(geometry.arrcfSrcVideo);
                geometry.rcfDestViewBounds = VideoPresentationBuilder.ComputeBounds(geometry.arrcfDestView);
                float num2 = 0.0001f;
                RectangleF rcfDestViewBounds = geometry.rcfDestViewBounds;
                for (int index = 0; index < num1; ++index)
                {
                    float left = geometry.arrcfDestView[index].Left;
                    float right = geometry.arrcfDestView[index].Right;
                    float top = geometry.arrcfDestView[index].Top;
                    float bottom = geometry.arrcfDestView[index].Bottom;
                    if ((double)Math.Abs(geometry.arrcfDestView[index].Top - geometry.rcfDestViewBounds.Top) < (double)num2)
                        top = rcfDestViewBounds.Top;
                    if ((double)Math.Abs(geometry.arrcfDestView[index].Bottom - geometry.rcfDestViewBounds.Bottom) < (double)num2)
                        bottom = rcfDestViewBounds.Bottom;
                    if ((double)Math.Abs(geometry.arrcfDestView[index].Left - geometry.rcfDestViewBounds.Left) < (double)num2)
                        left = rcfDestViewBounds.Left;
                    if ((double)Math.Abs(geometry.arrcfDestView[index].Right - geometry.rcfDestViewBounds.Right) < (double)num2)
                        right = rcfDestViewBounds.Right;
                    geometry.arrcfDestView[index] = new RectangleF(left, top, right - left, bottom - top);
                }
                geometry.rcfDestViewBounds = rcfDestViewBounds;
                if ((double)this.m_sizefOriginalSource.Width > 0.0 && (double)this.m_sizefOriginalSource.Height > 0.0)
                {
                    float num3 = this.m_sizefOriginalSource.Width / geometry.rcfSrcVideoBounds.Width;
                    float num4 = this.m_sizefOriginalSource.Height / geometry.rcfSrcVideoBounds.Height;
                    RectangleF square1 = VideoPresentationBuilder.ConvertToSquare(geometry.rcfSrcVideoBounds, flSrcWidthMultiplier);
                    RectangleF square2 = VideoPresentationBuilder.ConvertToSquare(geometry.rcfDestViewBounds, flDstWidthMultiplier);
                    RectangleF rectangleF = new RectangleF()
                    {
                        X = square2.X - num3 * square1.X,
                        Y = square2.Y - num4 * square1.Y,
                        Width = num3 * square2.Width,
                        Height = num4 * square2.Height
                    };
                }
            }
            else
                geometry.arrcfSrcVideo = geometry.arrcfDestView = new RectangleF[0];
            geometry.arrcfBorders = this.ComputeBorders(geometry.rcfDestViewBounds);
            return new BasicVideoPresentation(geometry);
        }

        private static RectangleF ConvertToSquare(RectangleF rcfSrc, float flWidthAdjust)
        {
            if ((double)flWidthAdjust == 1.0)
                return rcfSrc;
            float x = rcfSrc.Left * flWidthAdjust;
            float num = rcfSrc.Right * flWidthAdjust;
            return new RectangleF(x, rcfSrc.Y, num - x, rcfSrc.Height);
        }

        private static RectangleF ConvertFromSquare(RectangleF rcfSrc, float flWidthAdjust)
        {
            if ((double)flWidthAdjust == 1.0)
                return rcfSrc;
            float x = rcfSrc.Left / flWidthAdjust;
            float num = rcfSrc.Right / flWidthAdjust;
            return new RectangleF(x, rcfSrc.Y, num - x, rcfSrc.Height);
        }

        private static void ConvertFromSquare(RectangleF[] arrcf, float flWidthAdjust)
        {
            if (arrcf == null || (double)flWidthAdjust == 1.0)
                return;
            for (int index = 0; index < arrcf.Length; ++index)
            {
                float num1 = arrcf[index].Left / flWidthAdjust;
                float num2 = arrcf[index].Right / flWidthAdjust;
                arrcf[index].X = num1;
                arrcf[index].Width = num2 - num1;
            }
        }

        private static RectangleF ComputeBounds(RectangleF[] arrcfStrips)
        {
            if (arrcfStrips == null || arrcfStrips.Length <= 0)
                return RectangleF.Zero;
            float left = arrcfStrips[0].Left;
            float top = arrcfStrips[0].Top;
            float right = arrcfStrips[0].Right;
            float bottom = arrcfStrips[0].Bottom;
            for (int index = 1; index < arrcfStrips.Length; ++index)
            {
                RectangleF arrcfStrip = arrcfStrips[index];
                if ((double)left > (double)arrcfStrip.Left)
                    left = arrcfStrip.Left;
                if ((double)top > (double)arrcfStrip.Top)
                    top = arrcfStrip.Top;
                if ((double)right < (double)arrcfStrip.Right)
                    right = arrcfStrip.Right;
                if ((double)bottom < (double)arrcfStrip.Bottom)
                    bottom = arrcfStrip.Bottom;
            }
            return new RectangleF(left, top, right - left, bottom - top);
        }

        private static void ApplyOverscanFactor(ref RectangleF rcf, float flOverscanPer)
        {
            if ((double)flOverscanPer > 50.0)
                flOverscanPer = 50f;
            SizeF sizeF = new SizeF((float)((double)rcf.Width * (double)flOverscanPer / 100.0), (float)((double)rcf.Height * (double)flOverscanPer / 100.0));
            rcf.X += sizeF.Width / 2f;
            rcf.Y += sizeF.Height / 2f;
            rcf.Width -= sizeF.Width;
            rcf.Height -= sizeF.Height;
        }

        private void ComputeLocations(
          out RectangleF rcfBoundSrcVideoPxl,
          out RectangleF rcfBoundDestViewPxl,
          out float flSrcWidthMultiplier,
          out float flDstWidthMultiplier)
        {
            //ref RectangleF rcfBoundSrcVideoPxl = ref rcfBoundSrcVideoPxl;
            //ref RectangleF rcfBoundDestViewPxl = ref rcfBoundDestViewPxl;
            RectangleF rectangleF1 = new RectangleF();
            RectangleF rectangleF2 = rectangleF1;
            rcfBoundDestViewPxl = rectangleF2;
            RectangleF rectangleF3 = rectangleF1;
            rcfBoundSrcVideoPxl = rectangleF3;
            flSrcWidthMultiplier = flDstWidthMultiplier = 1f;
            float flOverscanPer1 = 0.0f;
            float flOverscanPer2 = 0.0f;
            if ((double)this.m_flInputDisplayOverscanPer > 0.0 || (double)this.m_flInputContentOverscanPer > 0.0)
            {
                switch (this.m_nDisplayMode)
                {
                    case VideoDisplayMode.Inset:
                        if (this.m_nInputOverscanMode == VideoOverscanMode.InvalidContent)
                        {
                            flOverscanPer1 = this.m_flInputContentOverscanPer;
                            break;
                        }
                        break;
                    case VideoDisplayMode.FullPreScene:
                    case VideoDisplayMode.FullInScene:
                        switch (this.m_nInputOverscanMode)
                        {
                            case VideoOverscanMode.NoOverscan:
                                flOverscanPer2 = this.m_flInputDisplayOverscanPer;
                                break;
                            case VideoOverscanMode.ValidContent:
                                break;
                            case VideoOverscanMode.InvalidContent:
                                flOverscanPer1 = !this.m_fInputFullDisplay ? this.m_flInputContentOverscanPer : this.m_flInputContentOverscanPer - this.m_flInputDisplayOverscanPer;
                                break;
                            default:
                                return;
                        }
                        break;
                    case VideoDisplayMode.Animating:
                        switch (this.m_nInputOverscanMode)
                        {
                            case VideoOverscanMode.NoOverscan:
                                flOverscanPer2 = this.m_flInputDisplayOverscanPer;
                                break;
                            case VideoOverscanMode.ValidContent:
                                break;
                            case VideoOverscanMode.InvalidContent:
                                flOverscanPer1 = this.m_flInputContentOverscanPer;
                                flOverscanPer2 = this.m_flInputDisplayOverscanPer;
                                break;
                            default:
                                return;
                        }
                        break;
                    default:
                        return;
                }
            }
            rcfBoundSrcVideoPxl.Size = this.m_sizefOriginalSource;
            if ((double)rcfBoundSrcVideoPxl.Width > 0.0 && (double)rcfBoundSrcVideoPxl.Height > 0.0)
            {
                float num = rcfBoundSrcVideoPxl.Height * this.m_sizefInputContentAspect.Width / this.m_sizefInputContentAspect.Height;
                flSrcWidthMultiplier = num / rcfBoundSrcVideoPxl.Width;
                VideoPresentationBuilder.ApplyOverscanFactor(ref rcfBoundSrcVideoPxl, flOverscanPer1);
            }
            else
            {
                rcfBoundSrcVideoPxl.Width = 1f;
                rcfBoundSrcVideoPxl.Height = 1f;
            }
            rcfBoundDestViewPxl = this.m_rcfInputDestViewPxl;
            if ((double)rcfBoundDestViewPxl.Width > 0.0 && (double)rcfBoundDestViewPxl.Height > 0.0)
            {
                float num = rcfBoundDestViewPxl.Height * this.m_sizefInputDestAspect.Width / this.m_sizefInputDestAspect.Height;
                flDstWidthMultiplier = num / rcfBoundDestViewPxl.Width;
                VideoPresentationBuilder.ApplyOverscanFactor(ref rcfBoundDestViewPxl, flOverscanPer2);
            }
            else
            {
                rcfBoundDestViewPxl.Width = 0.0f;
                rcfBoundDestViewPxl.Height = 0.0f;
            }
        }

        private RectangleF[] ComputeBorders(RectangleF rcfContent)
        {
            RectangleF inputDestViewPxl = this.m_rcfInputDestViewPxl;
            RectangleF[] rectangleFArray1 = new RectangleF[4];
            int length = 0;
            rcfContent.Intersect(inputDestViewPxl);
            if (!rcfContent.IsEmpty)
            {
                if ((double)inputDestViewPxl.Left < (double)rcfContent.Left)
                {
                    RectangleF rectangleF = new RectangleF(inputDestViewPxl.Left, inputDestViewPxl.Top, rcfContent.Left - inputDestViewPxl.Left, inputDestViewPxl.Height);
                    inputDestViewPxl.Width -= rcfContent.Left - inputDestViewPxl.Left;
                    inputDestViewPxl.X = rcfContent.Left;
                    if (!rectangleF.IsEmpty)
                    {
                        rectangleFArray1[length] = rectangleF;
                        ++length;
                    }
                }
                if ((double)inputDestViewPxl.Right > (double)rcfContent.Right)
                {
                    RectangleF rectangleF = new RectangleF(rcfContent.Right, inputDestViewPxl.Top, inputDestViewPxl.Right - rcfContent.Right, inputDestViewPxl.Height);
                    inputDestViewPxl.Width = rcfContent.Right - inputDestViewPxl.X;
                    if (!rectangleF.IsEmpty)
                    {
                        rectangleFArray1[length] = rectangleF;
                        ++length;
                    }
                }
                if ((double)inputDestViewPxl.Top < (double)rcfContent.Top)
                {
                    RectangleF rectangleF = new RectangleF(inputDestViewPxl.Left, inputDestViewPxl.Top, inputDestViewPxl.Width, rcfContent.Top - inputDestViewPxl.Top);
                    inputDestViewPxl.Height -= rcfContent.Top - inputDestViewPxl.Top;
                    inputDestViewPxl.Y = rcfContent.Top;
                    if (!rectangleF.IsEmpty)
                    {
                        rectangleFArray1[length] = rectangleF;
                        ++length;
                    }
                }
                if ((double)inputDestViewPxl.Bottom > (double)rcfContent.Bottom)
                {
                    RectangleF rectangleF = new RectangleF(inputDestViewPxl.Left, rcfContent.Bottom, inputDestViewPxl.Width, inputDestViewPxl.Bottom - rcfContent.Bottom);
                    inputDestViewPxl.Height = rcfContent.Bottom - inputDestViewPxl.Y;
                    if (!rectangleF.IsEmpty)
                    {
                        rectangleFArray1[length] = rectangleF;
                        ++length;
                    }
                }
            }
            else if (!inputDestViewPxl.IsEmpty)
            {
                rectangleFArray1[length] = inputDestViewPxl;
                ++length;
            }
            RectangleF[] rectangleFArray2 = new RectangleF[length];
            if (length > 0)
                Array.Copy((Array)rectangleFArray1, (Array)rectangleFArray2, length);
            return rectangleFArray2;
        }
    }
}