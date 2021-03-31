// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ViewItems.Text
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.InputHandlers;
using Microsoft.Iris.Layout;
using Microsoft.Iris.Layouts;
using Microsoft.Iris.Markup;
using Microsoft.Iris.OS;
using Microsoft.Iris.Render;
using Microsoft.Iris.Render.Extensions;
using Microsoft.Iris.RenderAPI.Drawing;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Iris.ViewItems
{
    internal class Text : ViewItem, ILayout
    {
        private const float c_scaleEpsilon = 0.01f;
        private const float c_lineSpacingDefault = 0.0f;
        private const float c_characterSpacingDefault = 0.0f;
        private const bool c_enableKerningDefault = false;
        private uint _bits;
        private TextFlow _flow;
        private float _lastLineExtentLeft;
        private float _lastLineExtentRight;
        private Size _textSize;
        private Size _slotSize;
        private int _lineAlignmentOffset;
        private string _content;
        private Font _font;
        private Color _textColor;
        private Color _textHighlightColor;
        private Color _backHighlightColor;
        private char _passwordChar;
        private float _scale;
        private LineAlignment _lineAlignment;
        private float _fadeSize;
        private int _maxLines;
        private int _lastVisibleRun;
        private TextBounds _boundsType;
        private bool _disableIme;
        private TextStyle _textStyle;
        private IDictionary _namedStyles;
        private RichText _richTextRasterizer;
        private TextEditingHandler _externalEditingHandler;
        private string _parsedContent;
        private ArrayList _parsedContentMarkedRanges;
        private ArrayList _fragments;
        private TextFlowRenderingHelper _renderingHelper;
        private static Font s_defaultFont = new Font("Arial", 24f);
        private static RichText s_sharedOversampledRasterizer;
        private static RichText s_sharedNonOversampledRasterizer;
        private static SimpleText s_sharedSimpleTextRasterizer;
        private static bool s_simpleTextMeasureAvailable;
        private Vector<float> _recentScaleChanges;
        private float _lineSpacing;
        private float _characterSpacing;
        private bool _enableKerning;
        private static char[] s_whitespaceChars = new char[3]
        {
      ' ',
      '\r',
      '\n'
        };

        public Text()
        {
            this.Layout = this;
            this._font = Microsoft.Iris.ViewItems.Text.s_defaultFont;
            this._textColor = Color.Black;
            this._textHighlightColor = Color.White;
            this._backHighlightColor = Color.Black;
            this._scale = 1f;
            this._fadeSize = 32f;
            this._maxLines = int.MaxValue;
            this._passwordChar = '•';
            this._lineAlignment = LineAlignment.Near;
            this._richTextRasterizer = Microsoft.Iris.ViewItems.Text.SharedNonOversampledRasterizer;
            this._renderingHelper = new TextFlowRenderingHelper();
            this.ContributesToWidth = true;
            this.TextFitsWidth = true;
            this.TextFitsHeight = true;
            this.SetClipped(false);
            this.MarkScaleDirty();
            if (Microsoft.Iris.ViewItems.Text.s_simpleTextMeasureAvailable)
                return;
            this.ClearBit(Microsoft.Iris.ViewItems.Text.Bits.FastMeasurePossible);
            this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.FastMeasureValid);
        }

        public Text(UIClass ownerUI)
          : this()
        {
        }

        protected override void OnDispose()
        {
            this.DisposeFlow();
            this.UnregisterFragmentUsage();
            base.OnDispose();
        }

        private void DisposeFlow()
        {
            if (this._flow == null)
                return;
            this._flow.Dispose(this);
            this._flow = null;
        }

        public static void Initialize()
        {
        }

        public static void Uninitialize()
        {
            if (Microsoft.Iris.ViewItems.Text.s_sharedOversampledRasterizer != null)
            {
                Microsoft.Iris.ViewItems.Text.s_sharedOversampledRasterizer.Dispose();
                Microsoft.Iris.ViewItems.Text.s_sharedOversampledRasterizer = null;
            }
            if (Microsoft.Iris.ViewItems.Text.s_sharedNonOversampledRasterizer != null)
            {
                Microsoft.Iris.ViewItems.Text.s_sharedNonOversampledRasterizer.Dispose();
                Microsoft.Iris.ViewItems.Text.s_sharedNonOversampledRasterizer = null;
            }
            if (Microsoft.Iris.ViewItems.Text.s_sharedSimpleTextRasterizer == null)
                return;
            Microsoft.Iris.ViewItems.Text.s_sharedSimpleTextRasterizer.Dispose();
            Microsoft.Iris.ViewItems.Text.s_sharedSimpleTextRasterizer = null;
        }

        private static SimpleText SharedSimpleTextRasterizer
        {
            get
            {
                if (Microsoft.Iris.ViewItems.Text.s_sharedSimpleTextRasterizer == null)
                    Microsoft.Iris.ViewItems.Text.s_sharedSimpleTextRasterizer = new SimpleText();
                return Microsoft.Iris.ViewItems.Text.s_sharedSimpleTextRasterizer;
            }
        }

        private static RichText SharedOversampledRasterizer
        {
            get
            {
                if (Microsoft.Iris.ViewItems.Text.s_sharedOversampledRasterizer == null)
                {
                    Microsoft.Iris.ViewItems.Text.s_sharedOversampledRasterizer = new RichText(true);
                    Microsoft.Iris.ViewItems.Text.s_sharedOversampledRasterizer.Oversample = true;
                    Microsoft.Iris.ViewItems.Text.s_simpleTextMeasureAvailable = NativeApi.SpSimpleTextIsAvailable();
                }
                return Microsoft.Iris.ViewItems.Text.s_sharedOversampledRasterizer;
            }
        }

        private static RichText SharedNonOversampledRasterizer
        {
            get
            {
                if (Microsoft.Iris.ViewItems.Text.s_sharedNonOversampledRasterizer == null)
                {
                    Microsoft.Iris.ViewItems.Text.s_sharedNonOversampledRasterizer = new RichText(true);
                    Microsoft.Iris.ViewItems.Text.s_simpleTextMeasureAvailable = NativeApi.SpSimpleTextIsAvailable();
                }
                return Microsoft.Iris.ViewItems.Text.s_sharedNonOversampledRasterizer;
            }
        }

        public string Content
        {
            get => this._content;
            set
            {
                if (!(this._content != value))
                    return;
                if (value != null && value.Length > 3)
                {
                    this._content = value.TrimEnd(Microsoft.Iris.ViewItems.Text.s_whitespaceChars);
                    char ch1 = value[value.Length - 1];
                    char ch2 = value[value.Length - 2];
                    if (ch1 == ' ' && ch2 != ' ')
                        this._content += (string)(object)ch1;
                }
                else
                    this._content = value;
                this._parsedContent = null;
                this._parsedContentMarkedRanges = null;
                this.OnDisplayedContentChange();
                this.FireNotification(NotificationID.Content);
            }
        }

        public void MarkScaleDirty() => this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.ScaleDirty);

        private bool InMeasure
        {
            get => this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.InMeasure);
            set => this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.InMeasure, value);
        }

        public void OnDisplayedContentChange()
        {
            if (this.InMeasure)
                return;
            this.MarkTextLayoutInvalid();
            this.MarkPaintInvalid();
            this.ForceContentChange();
        }

        public Font Font
        {
            get => this._font;
            set
            {
                if (this._font == value)
                    return;
                this._font = value;
                this.MarkPaintInvalid();
                this.MarkTextLayoutInvalid();
                this.ForceContentChange();
                this.FireNotification(NotificationID.Font);
            }
        }

        public Color Color
        {
            get => this._textColor;
            set
            {
                if (!(this._textColor != value))
                    return;
                this._textColor = value;
                if (this.KeepFlowAlive)
                {
                    this.MarkPaintInvalid();
                }
                else
                {
                    this.MarkLayoutInvalid();
                    if (this.HasEverPainted)
                        this.KeepFlowAlive = true;
                }
                this.FireNotification(NotificationID.Color);
            }
        }

        public Color HighlightColor
        {
            get => this._backHighlightColor;
            set
            {
                if (!(this._backHighlightColor != value))
                    return;
                this._backHighlightColor = value;
                this.MarkPaintInvalid();
                this.FireNotification(NotificationID.HighlightColor);
            }
        }

        public Color TextHighlightColor
        {
            get => this._textHighlightColor;
            set
            {
                if (!(this.TextHighlightColor != value))
                    return;
                this._textHighlightColor = value;
                this.MarkPaintInvalid();
                this.FireNotification(NotificationID.TextHighlightColor);
            }
        }

        public TextSharpness TextSharpness
        {
            get => !this._richTextRasterizer.Oversample ? TextSharpness.Sharp : TextSharpness.Soft;
            set
            {
                if (this.TextSharpness == value)
                    return;
                bool flag = false;
                switch (value)
                {
                    case TextSharpness.Sharp:
                        flag = false;
                        break;
                    case TextSharpness.Soft:
                        flag = true;
                        break;
                }
                if (this._richTextRasterizer == Microsoft.Iris.ViewItems.Text.s_sharedNonOversampledRasterizer && flag)
                    this._richTextRasterizer = Microsoft.Iris.ViewItems.Text.SharedOversampledRasterizer;
                else if (this._richTextRasterizer == Microsoft.Iris.ViewItems.Text.s_sharedOversampledRasterizer && !flag)
                    this._richTextRasterizer = Microsoft.Iris.ViewItems.Text.SharedNonOversampledRasterizer;
                else
                    this._richTextRasterizer.Oversample = flag;
                this.MarkTextLayoutInvalid();
                this.ForceContentChange();
                this.FireNotification(NotificationID.TextSharpness);
            }
        }

        public bool WordWrap
        {
            get => this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.WordWrap);
            set
            {
                if (this.WordWrap == value)
                    return;
                this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.WordWrap, value);
                if (value)
                    this.KeepFlowAlive = true;
                this.MarkPaintInvalid();
                this.MarkTextLayoutInvalid();
                this.UpdateWordWrapForExternalRasterizer();
                this.FireNotification(NotificationID.WordWrap);
            }
        }

        public bool UsePasswordMask
        {
            get => this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.PasswordMasked);
            set
            {
                if (this.UsePasswordMask == value)
                    return;
                this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.PasswordMasked, value);
                this.MarkPaintInvalid();
                this.MarkTextLayoutInvalid();
                this.FireNotification(NotificationID.UsePasswordMask);
            }
        }

        public char PasswordMask
        {
            get => this._passwordChar;
            set
            {
                if (_passwordChar == value)
                    return;
                this._passwordChar = value;
                if (this.UsePasswordMask)
                {
                    this.MarkPaintInvalid();
                    this.MarkTextLayoutInvalid();
                }
                this.FireNotification(NotificationID.PasswordMask);
            }
        }

        public int MaximumLines
        {
            get => this._maxLines;
            set
            {
                if (this._maxLines == value)
                    return;
                this._maxLines = value;
                this.MarkPaintInvalid();
                this.MarkTextLayoutInvalid();
                this.FireNotification(NotificationID.MaximumLines);
            }
        }

        public LineAlignment LineAlignment
        {
            get => this._lineAlignment;
            set
            {
                if (this._lineAlignment == value)
                    return;
                this._lineAlignment = value;
                this.MarkPaintInvalid();
                this.MarkTextLayoutInvalid();
                this.FireNotification(NotificationID.LineAlignment);
            }
        }

        public float LineSpacing
        {
            get => !this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.LineSpacingSet) ? 0.0f : this._lineSpacing;
            set
            {
                if (LineSpacing == (double)value)
                    return;
                this._lineSpacing = value;
                this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.LineSpacingSet);
                this.MarkPaintInvalid();
                this.MarkTextLayoutInvalid();
                this.FireNotification(NotificationID.LineSpacing);
            }
        }

        public float CharacterSpacing
        {
            get => !this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.CharacterSpacingSet) ? 0.0f : this._characterSpacing;
            set
            {
                if (CharacterSpacing == (double)value)
                    return;
                this._characterSpacing = value;
                this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.CharacterSpacingSet);
                this.MarkPaintInvalid();
                this.MarkTextLayoutInvalid();
                this.FireNotification(NotificationID.CharacterSpacing);
            }
        }

        public bool EnableKerning
        {
            get => this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.EnableKerningSet) && this._enableKerning;
            set
            {
                if (this.EnableKerning == value)
                    return;
                this._enableKerning = value;
                this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.EnableKerningSet);
                this.MarkPaintInvalid();
                this.MarkTextLayoutInvalid();
                this.FireNotification(NotificationID.EnableKerning);
            }
        }

        public float FadeSize
        {
            get => this._fadeSize;
            set
            {
                if (_fadeSize == (double)value)
                    return;
                this._fadeSize = value;
                this.InvalidateGradients();
                this.FireNotification(NotificationID.FadeSize);
            }
        }

        public TextStyle Style
        {
            get => this._textStyle;
            set
            {
                if (this._textStyle == value)
                    return;
                this._textStyle = value;
                this.MarkPaintInvalid();
                this.MarkTextLayoutInvalid();
                this.ForceContentChange();
                this.FireNotification(NotificationID.Style);
            }
        }

        public IDictionary NamedStyles
        {
            get => this._namedStyles;
            set
            {
                if (this._namedStyles == value)
                    return;
                this._namedStyles = value;
                this.MarkPaintInvalid();
                this.MarkTextLayoutInvalid();
                this.ForceContentChange();
                this.FireNotification(NotificationID.NamedStyles);
            }
        }

        public IList Fragments => _fragments;

        public bool DisableIme
        {
            get => this._disableIme;
            set
            {
                if (this._disableIme == value)
                    return;
                this._disableIme = value;
                this.FireNotification(NotificationID.DisableIme);
            }
        }

        public Rectangle LastLineBounds => this._flow != null ? this._flow.GetLastLineBounds(this._lineAlignmentOffset) : new Rectangle(this._textSize);

        public int NumberOfLines
        {
            get
            {
                if (this._flow == null)
                    return 1;
                return this._flow.Count < 1 ? 0 : this._flow[this._flow.Count - 1].Line;
            }
        }

        public int NumberOfVisibleLines
        {
            get
            {
                if (this._flow == null)
                    return 1;
                return this._flow.FirstFitRunOnFinalLine == null ? 0 : this._flow.FirstFitRunOnFinalLine.Line;
            }
        }

        public bool ContributesToWidth
        {
            get => this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.ContributesToWidth);
            set
            {
                if (this.ContributesToWidth == value)
                    return;
                this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.ContributesToWidth, value);
                this.FireNotification(NotificationID.ContributesToWidth);
                this.MarkLayoutInvalid();
            }
        }

        public TextBounds BoundsType
        {
            get => this._boundsType;
            set
            {
                if (this._boundsType == value)
                    return;
                this._boundsType = value;
                this.MarkLayoutInvalid();
                this.FireNotification(NotificationID.BoundsType);
            }
        }

        public bool Clipped => this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.Clipped);

        private void SetClipped(bool value)
        {
            if (this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.Clipped) == value)
                return;
            this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.Clipped, value);
            this.FireNotification(NotificationID.Clipped);
        }

        public RichText ExternalRasterizer
        {
            set
            {
                if (this._richTextRasterizer == value)
                    return;
                bool oversample = this._richTextRasterizer.Oversample;
                this._richTextRasterizer = value;
                if (value != null)
                {
                    this._richTextRasterizer.Oversample = oversample;
                    this.MarkScaleDirty();
                }
                else
                    this._richTextRasterizer = !oversample ? Microsoft.Iris.ViewItems.Text.SharedNonOversampledRasterizer : Microsoft.Iris.ViewItems.Text.SharedOversampledRasterizer;
                this.MarkTextLayoutInvalid();
            }
        }

        public TextEditingHandler ExternalEditingHandler
        {
            set
            {
                if (this._externalEditingHandler == value)
                    return;
                this._externalEditingHandler = value;
                this.UpdateWordWrapForExternalRasterizer();
            }
        }

        private void UpdateWordWrapForExternalRasterizer()
        {
            if (this._externalEditingHandler == null)
                return;
            this._externalEditingHandler.WordWrap = this.WordWrap;
        }

        public ItemAlignment DefaultChildAlignment => ItemAlignment.Default;

        public bool IsViewDependent(ViewItem node) => this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.ViewDependent);

        public void GetInitialChildrenRequests(out int more) => more = 0;

        Size ILayout.Measure(ILayoutNode layoutNode, Size constraint)
        {
            Size zero = Size.Zero;
            this.InMeasure = true;
            LineAlignment alignment = RichText.ReverseAlignment(this._lineAlignment, this.UISession.IsRtl);
            if (!this.TextLayoutInvalid && this._flow != null && (!this.WordWrap && alignment == LineAlignment.Near) && (this.TextFitsWidth && constraint.Width >= this._textSize.Width && (this.TextFitsHeight && constraint.Height >= this._textSize.Height)))
            {
                Size size = Size.Min(this._textSize, constraint);
                this.InMeasure = false;
                return size;
            }
            int boundingWidth = constraint.Width;
            if (!this.ContributesToWidth)
            {
                boundingWidth = 16777215;
                layoutNode.LayoutContributesToWidth = false;
            }
            this.MeasureText(boundingWidth, constraint.Height, alignment);
            if (alignment != LineAlignment.Near && this._flow.Bounds.Width < boundingWidth && this._flow.Bounds.Height >= 0)
                this.MeasureText(this._flow.Bounds.Width, this._flow.Bounds.Height, alignment);
            bool flag1 = false;
            bool flag2 = false;
            if (this._flow.Count > 0)
            {
                Size naturalSize = this.GetNaturalSize();
                flag1 = naturalSize.Width > boundingWidth;
                flag2 = naturalSize.Height > constraint.Height;
            }
            int x = 0;
            int y = 0;
            if (this._flow.Bounds.Left < 0)
                x = -this._flow.Bounds.Left;
            if (this._flow.Bounds.Top < 0)
                y = -this._flow.Bounds.Top;
            Point point1 = new Point(x, y);
            this.ClipToHeight = false;
            int num1 = -1;
            int num2 = 0;
            int val1_1 = 0;
            int val1_2 = 0;
            int num3 = this.MaximumLines;
            if (num3 == 0)
                num3 = int.MaxValue;
            int index;
            for (index = 0; index < this._flow.Count; ++index)
            {
                TextRun run = this._flow[index];
                if (run.Line > num3)
                {
                    flag2 = true;
                    break;
                }
                if (this.BoundsType != TextBounds.Full)
                {
                    if (run.Line != num1)
                    {
                        num2 += val1_1 + val1_2;
                        val1_1 = 0;
                        val1_2 = 0;
                        num1 = run.Line;
                    }
                    Point offsetPoint = point1;
                    offsetPoint.Y -= num2;
                    if ((this.BoundsType & TextBounds.AlignToAscender) != TextBounds.Full)
                    {
                        val1_1 = Math.Max(val1_1, run.AscenderInset);
                        offsetPoint.Y -= run.AscenderInset;
                    }
                    if ((this.BoundsType & TextBounds.AlignToBaseline) != TextBounds.Full)
                        val1_2 = Math.Max(val1_2, run.BaselineInset);
                    run.ApplyOffset(offsetPoint);
                }
                int num4 = (this.BoundsType & TextBounds.AlignToBaseline) != TextBounds.Full ? run.BaselineInset : 0;
                if (run.LayoutBounds.Bottom - num4 > constraint.Height)
                {
                    if (run.Line == 1)
                        this.ClipToHeight = true;
                    else
                        break;
                }
                this._flow.AddFit(run);
            }
            this._lastVisibleRun = index - 1;
            this.TextFitsWidth = this.WordWrap || !flag1;
            this.TextFitsHeight = !flag2;
            if (!this.TextFitsHeight && this._flow.HasVisibleRuns)
            {
                this._lastLineExtentLeft = _flow.FirstFitRunOnFinalLine.LayoutBounds.Left;
                this._lastLineExtentRight = _flow.LastFitRun.LayoutBounds.Right;
            }
            else
                this._lastLineExtentLeft = this._lastLineExtentRight = 0.0f;
            this.InvalidateGradients();
            if (this.UpdateFragmentsAfterLayout)
                ((ViewItem)layoutNode).MarkLayoutOutputDirty(true);
            Point point2 = new Point(this._flow.Bounds.Location.X + point1.X, this._flow.Bounds.Location.Y + point1.Y);
            this._textSize = new Size(this._flow.Bounds.Width + point2.X, this._flow.FitBounds.Height + point2.Y - (val1_1 + val1_2));
            Size constraint1 = Size.Min(this._textSize, constraint);
            DefaultLayout.Measure(layoutNode, constraint1);
            this.FireNotification(NotificationID.LastLineBounds);
            this.InMeasure = false;
            return constraint1;
        }

        void ILayout.Arrange(ILayoutNode layoutNode, LayoutSlot slot)
        {
            this._slotSize = slot.Bounds;
            if (!this.ContributesToWidth)
                this.TextFitsWidth = this._textSize.Width <= slot.Bounds.Width;
            this._lineAlignmentOffset = 0;
            if (this.TextFitsWidth)
            {
                if (this.LineAlignment == LineAlignment.Center)
                    this._lineAlignmentOffset = (this._slotSize.Width - this._textSize.Width) / 2;
                else if (this.LineAlignment == LineAlignment.Far)
                    this._lineAlignmentOffset = this._slotSize.Width - this._textSize.Width;
            }
            bool flag1 = false;
            if (this._flow != null)
            {
                Rectangle view = slot.View;
                Size size = view.Size;
                bool flag2 = this._textSize.Width > size.Width || this._textSize.Height > size.Height;
                flag1 = ((flag1 ? 1 : 0) | (!this.KeepFlowAlive ? 0 : (flag2 ? 1 : 0))) != 0;
                this._flow.ResetVisibleTracking();
                if (flag1)
                {
                    bool flag3 = false;
                    for (int index = 0; index <= this._lastVisibleRun; ++index)
                    {
                        TextRun textRun = this._flow[index];
                        bool flag4 = !textRun.LayoutBounds.IsEmpty && textRun.LayoutBounds.IntersectsWith(view);
                        if (!flag3 && textRun.Visible != flag4)
                        {
                            this.MarkPaintInvalid();
                            this.UpdateFragmentsAfterLayout = true;
                            flag3 = true;
                        }
                        if (flag4)
                            this._flow.AddVisible(index);
                        else
                            textRun.Visible = false;
                    }
                }
                else
                {
                    for (int index = 0; index <= this._lastVisibleRun; ++index)
                    {
                        TextRun textRun = this._flow[index];
                        if (!textRun.LayoutBounds.IsEmpty)
                        {
                            if (!textRun.Visible)
                                this.MarkPaintInvalid();
                            this._flow.AddVisible(index);
                        }
                    }
                }
            }
            this.KeepFlowAlive |= flag1;
            this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.ViewDependent, flag1);
            DefaultLayout.Arrange(layoutNode, slot);
        }

        private Size GetNaturalSize()
        {
            if (!this.FastMeasurePossible)
                return this._richTextRasterizer.GetNaturalBounds();
            return this._flow.Count == 1 ? this._flow[0].NaturalExtent : Size.Zero;
        }

        private void MeasureText(int boundingWidth, int boundingHeight, LineAlignment alignment)
        {
            if (this.FastMeasurePossible)
                this.DoFastMeasure(boundingWidth, boundingHeight, alignment);
            else
                this.DoRichEditMeasure(boundingWidth, boundingHeight, alignment);
        }

        private void DoFastMeasure(int boundingWidth, int boundingHeight, LineAlignment alignment)
        {
            TextStyle effectiveTextStyle = this.GetEffectiveTextStyle();
            Size constraint = new Size(boundingWidth, boundingHeight);
            this.DisposeFlow();
            this._flow = Microsoft.Iris.ViewItems.Text.SharedSimpleTextRasterizer.Measure(this._content, alignment, effectiveTextStyle, constraint);
            this._flow.DeclareOwner(this);
        }

        private void DoRichEditMeasure(int boundingWidth, int boundingHeight, LineAlignment alignment)
        {
            TextMeasureParams measureParams = new TextMeasureParams();
            measureParams.Initialize();
            float width = Math.Min(boundingWidth, 4095f);
            float height = Math.Min(boundingHeight, 8191f);
            measureParams.SetConstraint(new SizeF(width, height));
            TextStyle effectiveTextStyle = this.GetEffectiveTextStyle();
            string empty = string.Empty;
            string content;
            if (!this.UsedForEditing)
            {
                content = this.Content;
                if (this._namedStyles != null)
                {
                    if (this._parsedContent == null && content != null)
                    {
                        this._parsedContentMarkedRanges = new ArrayList();
                        this._parsedContent = Microsoft.Iris.ViewItems.Text.ParseMarkedUpText(content, this._parsedContentMarkedRanges);
                    }
                    content = this._parsedContent;
                }
                measureParams.SetWordWrap(this.WordWrap);
            }
            else
                content = this._richTextRasterizer.SimpleContent;
            measureParams.SetEditMode(this.UsedForEditing);
            if (this.UsePasswordMask)
                measureParams.SetPasswordChar(this._passwordChar);
            measureParams.SetFormat(alignment, effectiveTextStyle);
            if ((this._boundsType & TextBounds.TrimLeftSideBearing) != TextBounds.Full && this._lineAlignment == LineAlignment.Near)
                measureParams.TrimLeftSideBearing();
            if (!this.UsedForEditing || this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.ScaleDirty))
            {
                this.ClearBit(Microsoft.Iris.ViewItems.Text.Bits.ScaleDirty);
                measureParams.SetScale(this._scale);
            }
            if (this._parsedContent != null)
                this.ApplyContentFormatting(ref measureParams);
            this.DisposeFlow();
            this._flow = this._richTextRasterizer.Measure(content, ref measureParams);
            measureParams.Dispose();
            this._flow.DeclareOwner(this);
            this.UpdateFragmentsAfterLayout = true;
        }

        private bool FastMeasurePossible
        {
            get
            {
                if (!this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.FastMeasureValid))
                {
                    bool flag = this.UsingSharedRasterizer && !this.WordWrap && (this._namedStyles == null && _scale == 1.0) && (this.TextSharpness == TextSharpness.Sharp && !this.UsePasswordMask) && !this.Zone.Session.IsRtl;
                    if (flag)
                        flag = Microsoft.Iris.ViewItems.Text.SharedSimpleTextRasterizer.CanMeasure(this.Content, this.GetEffectiveTextStyle());
                    this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.FastMeasurePossible, flag);
                    this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.FastMeasureValid);
                }
                return this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.FastMeasurePossible);
            }
        }

        internal TextStyle GetEffectiveTextStyle()
        {
            TextStyle textStyle = new TextStyle();
            Font font = this._font ?? Microsoft.Iris.ViewItems.Text.s_defaultFont;
            textStyle.FontFace = font.FontName;
            textStyle.FontSize = font.FontSize;
            if (font.AltFontSize != (double)font.FontSize)
                textStyle.AltFontSize = font.AltFontSize;
            if ((font.FontStyle & FontStyles.Bold) != FontStyles.None)
                textStyle.Bold = true;
            if ((font.FontStyle & FontStyles.Italic) != FontStyles.None)
                textStyle.Italic = true;
            if ((font.FontStyle & FontStyles.Underline) != FontStyles.None)
                textStyle.Underline = true;
            textStyle.Color = this._textColor;
            if (this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.LineSpacingSet))
                textStyle.LineSpacing = this.LineSpacing;
            if (this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.CharacterSpacingSet))
                textStyle.CharacterSpacing = this.CharacterSpacing;
            if (this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.EnableKerningSet))
                textStyle.EnableKerning = this.EnableKerning;
            if (this._textStyle != null)
                textStyle.Add(this._textStyle);
            return textStyle;
        }

        protected override void OnLayoutComplete(ViewItem sender)
        {
            ArrayList arrayList = null;
            if (this.UpdateFragmentsAfterLayout)
            {
                if (this._namedStyles != null)
                    arrayList = this.AnnotateFragments();
                bool flag = false;
                if (this._fragments != null || arrayList != null)
                    flag = this.TextLayoutInvalid || !Microsoft.Iris.ViewItems.Text.AreFragmentListsEquivalent(_fragments, arrayList);
                if (flag)
                {
                    this.UnregisterFragmentUsage();
                    this._fragments = arrayList;
                    this.RegisterFragmentUsage();
                    this.FireNotification(NotificationID.Fragments);
                }
                else if (this._fragments != null)
                {
                    for (int index = 0; index < this._fragments.Count; ++index)
                        ((TextFragment)this._fragments[index]).NotifyPaintInvalid();
                }
                this.ResetMarkedRanges();
                this.UpdateFragmentsAfterLayout = false;
            }
            this.SetClipped(!this.TextFitsWidth || !this.TextFitsHeight);
            this.TextLayoutInvalid = false;
            base.OnLayoutComplete(sender);
        }

        private static bool AreFragmentListsEquivalent(IList lhsFragments, IList rhsFragments)
        {
            int num1 = lhsFragments != null ? lhsFragments.Count : 0;
            int num2 = rhsFragments != null ? rhsFragments.Count : 0;
            if (num1 != num2)
                return false;
            for (int index = 0; index < num1; ++index)
            {
                if (!((TextFragment)lhsFragments[index]).IsLayoutEquivalentTo((TextFragment)rhsFragments[index]))
                    return false;
            }
            return true;
        }

        private void ApplyContentFormatting(ref TextMeasureParams measureParams)
        {
            if (this._parsedContentMarkedRanges == null || this._namedStyles == null)
                return;
            if (!this.UsingSharedRasterizer)
            {
                ErrorManager.ReportError("Text: Complex formatting unsupported on text that is editable");
            }
            else
            {
                measureParams.AllocateFormattedRanges(this._parsedContentMarkedRanges.Count, this._namedStyles.Count);
                TextStyle[] array = new TextStyle[this._namedStyles.Count];
                int index1 = 0;
                foreach (TextStyle style in _namedStyles.Values)
                {
                    measureParams.SetFormattedRangeStyle(index1, style);
                    array[index1] = style;
                    ++index1;
                }
                TextMeasureParams.FormattedRange[] formattedRanges = measureParams.FormattedRanges;
                for (int index2 = 0; index2 < this._parsedContentMarkedRanges.Count; ++index2)
                {
                    Microsoft.Iris.ViewItems.Text.MarkedRange contentMarkedRange = (Microsoft.Iris.ViewItems.Text.MarkedRange)this._parsedContentMarkedRanges[index2];
                    if (this._namedStyles.Contains(contentMarkedRange.tagName))
                    {
                        TextStyle namedStyle = this._namedStyles[contentMarkedRange.tagName] as TextStyle;
                        contentMarkedRange.cachedStyle = namedStyle;
                        if (namedStyle != null)
                        {
                            formattedRanges[index2].FirstCharacter = contentMarkedRange.firstCharacter;
                            formattedRanges[index2].LastCharacter = contentMarkedRange.lastCharacter;
                            formattedRanges[index2].Color = contentMarkedRange.RangeIDAsColor;
                            formattedRanges[index2].StyleIndex = Array.IndexOf<TextStyle>(array, namedStyle);
                        }
                    }
                    else
                        contentMarkedRange.cachedStyle = null;
                }
            }
        }

        private ArrayList AnnotateFragments()
        {
            ArrayList arrayList = null;
            if (this._parsedContentMarkedRanges != null && this._flow != null)
            {
                for (int firstVisibleIndex = this._flow.FirstVisibleIndex; firstVisibleIndex <= this._flow.LastVisibleIndex; ++firstVisibleIndex)
                {
                    TextRun textRun = this._flow[firstVisibleIndex];
                    textRun.IsFragment = false;
                    if (textRun.RunColor.A != byte.MaxValue)
                    {
                        Microsoft.Iris.ViewItems.Text.MarkedRange markedRange = null;
                        for (int index = 0; index < this._parsedContentMarkedRanges.Count; ++index)
                        {
                            Microsoft.Iris.ViewItems.Text.MarkedRange contentMarkedRange = (Microsoft.Iris.ViewItems.Text.MarkedRange)this._parsedContentMarkedRanges[index];
                            if (contentMarkedRange.RangeIDAsColor == textRun.RunColor)
                            {
                                textRun.OverrideColor = contentMarkedRange.GetEffectiveColor(Color.Transparent);
                                markedRange = contentMarkedRange;
                                break;
                            }
                        }
                        if (markedRange != null && markedRange.IsInFragment)
                        {
                            textRun.IsFragment = true;
                            if (markedRange.fragment == null)
                            {
                                markedRange.fragment = new TextFragment(markedRange.tagName, markedRange.attributes, this);
                                if (arrayList == null)
                                    arrayList = new ArrayList();
                                arrayList.Add(markedRange.fragment);
                            }
                            markedRange.fragment.InternalRuns.Add(new TextRunData(textRun, this.IsOnLastLine(textRun), this, this._lineAlignmentOffset));
                        }
                    }
                }
            }
            return arrayList;
        }

        private void RegisterFragmentUsage()
        {
            if (this._fragments == null)
                return;
            foreach (TextFragment fragment in this._fragments)
            {
                if (fragment.Runs != null)
                {
                    foreach (TextRunData run in fragment.Runs)
                        run.Run.RegisterUsage(this);
                }
            }
        }

        private void UnregisterFragmentUsage()
        {
            if (this._fragments == null)
                return;
            foreach (TextFragment fragment in this._fragments)
            {
                if (fragment.Runs != null)
                {
                    foreach (TextRunData run in fragment.Runs)
                        run.Run.UnregisterUsage(this);
                }
            }
            this._fragments = null;
        }

        private void ResetMarkedRanges()
        {
            if (this._parsedContentMarkedRanges == null)
                return;
            for (int index = 0; index < this._parsedContentMarkedRanges.Count; ++index)
                ((Microsoft.Iris.ViewItems.Text.MarkedRange)this._parsedContentMarkedRanges[index]).fragment = null;
        }

        private static string ParseMarkedUpText(string content, ArrayList markedRanges)
        {
            StringBuilder stringBuilder = new StringBuilder();
            ArrayList arrayList = new ArrayList();
            uint num = 0;
            try
            {
                using (NativeXmlReader nativeXmlReader = new NativeXmlReader(content, true))
                {
                    Microsoft.Iris.ViewItems.Text.MarkedRange markedRange1 = null;
                    NativeXmlNodeType nodeType;
                    while (nativeXmlReader.Read(out nodeType))
                    {
                        switch (nodeType)
                        {
                            case NativeXmlNodeType.Element:
                                if (!nativeXmlReader.IsEmptyElement)
                                {
                                    string name = nativeXmlReader.Name;
                                    Microsoft.Iris.ViewItems.Text.MarkedRange markedRange2 = new Microsoft.Iris.ViewItems.Text.MarkedRange();
                                    markedRange2.tagName = name;
                                    markedRange2.firstCharacter = stringBuilder.Length;
                                    markedRange2.lastCharacter = int.MaxValue;
                                    markedRange2.rangeID = ++num;
                                    arrayList.Add(markedRange2);
                                    markedRanges.Add(markedRange2);
                                    markedRange2.parentRange = markedRange1;
                                    markedRange1 = markedRange2;
                                    while (nativeXmlReader.ReadAttribute())
                                    {
                                        if (markedRange1.attributes == null)
                                            markedRange1.attributes = new Dictionary<object, object>();
                                        markedRange1.attributes[nativeXmlReader.Name] = nativeXmlReader.Value;
                                    }
                                    continue;
                                }
                                continue;
                            case NativeXmlNodeType.Text:
                            case NativeXmlNodeType.CDATA:
                            case NativeXmlNodeType.Whitespace:
                                string str = nativeXmlReader.Value;
                                if (str.IndexOf("\r\n", StringComparison.Ordinal) >= 0)
                                    str = str.Replace("\r\n", "\r");
                                stringBuilder.Append(str);
                                continue;
                            case NativeXmlNodeType.EndElement:
                                string name1 = nativeXmlReader.Name;
                                for (int index = arrayList.Count - 1; index >= 0; --index)
                                {
                                    Microsoft.Iris.ViewItems.Text.MarkedRange markedRange2 = (Microsoft.Iris.ViewItems.Text.MarkedRange)arrayList[index];
                                    markedRange2.lastCharacter = stringBuilder.Length;
                                    arrayList.RemoveAt(index);
                                    if (markedRange2.tagName == name1)
                                        break;
                                }
                                if (markedRange1 != null)
                                {
                                    markedRange1 = markedRange1.parentRange;
                                    continue;
                                }
                                continue;
                            default:
                                continue;
                        }
                    }
                }
            }
            catch (NativeXmlException ex)
            {
                markedRanges.Clear();
                stringBuilder = null;
            }
            if (stringBuilder == null)
                return content;
            return stringBuilder.ToString();
        }

        public void CreateFadeGradientsHelper(
          ref IGradient gradientClipLeftRight,
          ref IGradient gradientMultiLine)
        {
            bool flag1 = true;
            if (this.TextFitsWidth && this.TextFitsHeight)
                flag1 = false;
            float fadeSize = this.FadeSize;
            if (!flag1 || fadeSize <= 0.0)
                return;
            if (!this.WordWrap)
            {
                if (this.TextFitsWidth)
                    return;
                float num = 0.0f;
                float flPosition = 0.0f;
                LineAlignment lineAlignment = this.LineAlignment;
                if (lineAlignment == LineAlignment.Center)
                {
                    flPosition = num = fadeSize;
                }
                else
                {
                    bool flag2 = lineAlignment == LineAlignment.Near;
                    if (this.UISession.IsRtl)
                        flag2 = !flag2;
                    if (flag2)
                        num = fadeSize;
                    else
                        flPosition = fadeSize;
                }
                IGradient gradient = this.UISession.RenderSession.CreateGradient(this);
                gradient.Orientation = Orientation.Horizontal;
                if (flPosition > 0.0)
                {
                    gradient.AddValue(-1f, 0.0f, RelativeSpace.Min);
                    gradient.AddValue(flPosition, 1f, RelativeSpace.Min);
                }
                if (num > 0.0)
                {
                    gradient.AddValue(_slotSize.Width - num, 1f, RelativeSpace.Min);
                    gradient.AddValue(this._slotSize.Width + 1, 0.0f, RelativeSpace.Min);
                }
                gradientClipLeftRight = gradient;
            }
            else
            {
                if (this.TextFitsHeight)
                    return;
                float flPosition1 = 0.0f;
                float flPosition2 = 0.0f;
                if (!this.UISession.IsRtl)
                {
                    if (this._flow.LastFitRun != null)
                    {
                        flPosition2 = _flow.LastFitRun.LayoutBounds.Width;
                        flPosition1 = flPosition2 - fadeSize;
                    }
                }
                else
                {
                    flPosition2 = 0.0f;
                    flPosition1 = flPosition2 + fadeSize;
                }
                IGradient gradient = this.UISession.RenderSession.CreateGradient(this);
                gradient.ColorMask = new ColorF(byte.MaxValue, 0, 0, 0);
                gradient.Orientation = Orientation.Horizontal;
                gradient.AddValue(flPosition1, 1f, RelativeSpace.Min);
                gradient.AddValue(flPosition2, 0.0f, RelativeSpace.Min);
                gradientMultiLine = gradient;
            }
        }

        private bool UsingSharedRasterizer => !this._richTextRasterizer.HasCallbacks;

        private bool UsedForEditing => this._externalEditingHandler != null;

        private void ResetCachedScaleState()
        {
            this.IgnoreEffectiveScaleChanges = false;
            this._recentScaleChanges = null;
        }

        private void CreateVisuals(IVisualContainer topVisual, IRenderSession renderSession)
        {
            VisualOrder nOrder = VisualOrder.First;
            IGradient gradientClipLeftRight = null;
            IGradient gradientMultiLine = null;
            this.CreateFadeGradientsHelper(ref gradientClipLeftRight, ref gradientMultiLine);
            TextRun textRun = this.UISession.IsRtl ? this._flow.FirstFitRunOnFinalLine : this._flow.LastFitRun;
            for (int firstVisibleIndex = this._flow.FirstVisibleIndex; firstVisibleIndex <= this._flow.LastVisibleIndex; ++firstVisibleIndex)
            {
                TextRun run = this._flow[firstVisibleIndex];
                if (run.Visible && !run.IsFragment)
                {
                    Color effectiveColor = this.GetEffectiveColor(run);
                    IImage imageForRun = Microsoft.Iris.ViewItems.Text.GetImageForRun(this.UISession, run, effectiveColor);
                    if (imageForRun != null)
                    {
                        float x = run.RenderBounds.Left + _lineAlignmentOffset;
                        if (run.Highlighted)
                        {
                            RectangleF lineBound = (RectangleF)this._flow.LineBounds[run.Line - 1];
                            ISprite sprite = renderSession.CreateSprite(this, this);
                            sprite.Effect = EffectManager.CreateColorFillEffect(this, this._backHighlightColor);
                            sprite.Effect.UnregisterUsage(this);
                            sprite.Position = new Vector3(x, lineBound.Top, 0.0f);
                            sprite.Size = new Vector2(run.RenderBounds.Width, lineBound.Height);
                            topVisual.AddChild(sprite, null, nOrder);
                            sprite.UnregisterUsage(this);
                            run.HighlightSprite = sprite;
                        }
                        ISprite sprite1 = renderSession.CreateSprite(this, this);
                        sprite1.Effect = EffectClass.CreateImageRenderEffectWithFallback(this.Effect, this, imageForRun);
                        sprite1.Effect.UnregisterUsage(this);
                        sprite1.Position = new Vector3(x, run.RenderBounds.Top, 0.0f);
                        sprite1.Size = new Vector2(run.RenderBounds.Width, run.RenderBounds.Height);
                        if (gradientMultiLine != null && run == textRun)
                            sprite1.AddGradient(gradientMultiLine);
                        topVisual.AddChild(sprite1, null, nOrder);
                        sprite1.UnregisterUsage(this);
                        run.TextSprite = sprite1;
                    }
                }
            }
            topVisual.RemoveAllGradients();
            if (gradientClipLeftRight != null)
                topVisual.AddGradient(gradientClipLeftRight);
            gradientClipLeftRight?.UnregisterUsage(this);
            gradientMultiLine?.UnregisterUsage(this);
        }

        private Color GetEffectiveColor(TextRun run)
        {
            Color color = this._textColor;
            if (run.Highlighted)
                color = this._textHighlightColor;
            else if (run.OverrideColor != Color.Transparent)
                color = run.OverrideColor;
            else if (run.Link && this._externalEditingHandler != null && this._externalEditingHandler.LinkColor != Color.Transparent)
                color = this._externalEditingHandler.LinkColor;
            else if (this._textStyle != null && this._textStyle.HasColor)
                color = this._textStyle.Color;
            return color;
        }

        protected override void DisposeAllContent()
        {
            base.DisposeAllContent();
            this.DisposeContent(true);
        }

        protected virtual void DisposeContent(bool removeFromTree)
        {
            if (removeFromTree)
                this.VisualContainer.RemoveAllChildren();
            this.Effect?.DoneWithRenderEffects(this);
            if (this._flow == null)
                return;
            this._flow.ClearSprites();
        }

        protected override void OnPaint(bool visible)
        {
            this.DisposeAllContent();
            base.OnPaint(visible);
            this.ResetCachedScaleState();
            if (this._flow == null)
            {
                if (this._content == null && this.UsingSharedRasterizer)
                    return;
                this.MarkTextLayoutInvalid();
            }
            else
            {
                this.CreateVisuals(this.VisualContainer, this.UISession.RenderSession);
                if (!this.KeepFlowAlive)
                    this.DisposeFlow();
                this.HasEverPainted = true;
            }
        }

        private bool IsOnLastLine(TextRun run) => this._flow.IsOnLastLine(run);

        private void InvalidateGradients()
        {
            this._renderingHelper.InvalidateGradients();
            this.MarkPaintInvalid();
        }

        protected override void OnEffectiveScaleChange()
        {
            float y = this.ComputeEffectiveScale().Y;
            if (!this.ScaleDifferenceIsGreaterThanThreshold(y, this._scale))
                return;
            Vector<float> vector = this._recentScaleChanges;
            if (!this.IgnoreEffectiveScaleChanges && vector != null)
            {
                foreach (float newScale in vector)
                {
                    if (!this.ScaleDifferenceIsGreaterThanThreshold(y, newScale))
                    {
                        this.IgnoreEffectiveScaleChanges = true;
                        break;
                    }
                }
            }
            if (this.IgnoreEffectiveScaleChanges)
                return;
            if (vector == null)
                vector = new Vector<float>(4);
            vector.Add(y);
            this._recentScaleChanges = vector;
            this.MarkTextLayoutInvalid();
            this._scale = y;
            this.MarkScaleDirty();
        }

        private bool ScaleDifferenceIsGreaterThanThreshold(float oldScale, float newScale) => Math.Abs(oldScale - newScale) > 0.00999999977648258;

        private void MarkTextLayoutInvalid()
        {
            this.TextLayoutInvalid = true;
            if (Microsoft.Iris.ViewItems.Text.s_simpleTextMeasureAvailable)
                this.ClearBit(Microsoft.Iris.ViewItems.Text.Bits.FastMeasureValid);
            this.DisposeFlow();
            this.MarkLayoutInvalid();
        }

        internal static IImage GetImageForRun(UISession session, TextRun run, Color textColor)
        {
            if (string.IsNullOrEmpty(run.Content))
                return null;
            string str = "aa";
            bool flag = false;
            RichTextInfoKey richTextInfoKey = new RichTextInfoKey(run, str, flag, textColor);
            ImageCache instance = TextImageCache.Instance;
            ImageCacheItem imageCacheItem = instance.Lookup(richTextInfoKey);
            if (imageCacheItem == null)
            {
                imageCacheItem = new TextImageItem(session.RenderSession, run, str, flag, textColor);
                instance.Add(richTextInfoKey, imageCacheItem);
            }
            return imageCacheItem.RenderImage;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.ToString());
            if (this._content != null)
            {
                stringBuilder.Append(" (Content=\"");
                stringBuilder.Append(this._content);
                stringBuilder.Append("\")");
            }
            return stringBuilder.ToString();
        }

        private bool TextFitsWidth
        {
            get => this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.TextFitsWidth);
            set => this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.TextFitsWidth, value);
        }

        private bool TextFitsHeight
        {
            get => this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.TextFitsHeight);
            set => this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.TextFitsHeight, value);
        }

        private bool ClipToHeight
        {
            get => this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.ClipToHeight);
            set => this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.ClipToHeight, value);
        }

        private bool KeepFlowAlive
        {
            get => this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.KeepFlowAlive);
            set => this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.KeepFlowAlive, value);
        }

        private bool HasEverPainted
        {
            get => this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.HasEverPainted);
            set => this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.HasEverPainted, value);
        }

        private bool TextLayoutInvalid
        {
            get => this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.TextLayoutInvalid);
            set => this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.TextLayoutInvalid, value);
        }

        private bool UpdateFragmentsAfterLayout
        {
            get => this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.UpdateFragmentsAfterLayout);
            set => this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.UpdateFragmentsAfterLayout, value);
        }

        private bool IgnoreEffectiveScaleChanges
        {
            get => this.GetBit(Microsoft.Iris.ViewItems.Text.Bits.IgnoreEffectiveScaleChanges);
            set => this.SetBit(Microsoft.Iris.ViewItems.Text.Bits.IgnoreEffectiveScaleChanges, value);
        }

        private bool GetBit(Microsoft.Iris.ViewItems.Text.Bits lookupBit) => ((Microsoft.Iris.ViewItems.Text.Bits)this._bits & lookupBit) != 0;

        private void SetBit(Microsoft.Iris.ViewItems.Text.Bits changeBit, bool value) => this._bits = value ? (uint)((Microsoft.Iris.ViewItems.Text.Bits)this._bits | changeBit) : (uint)((Microsoft.Iris.ViewItems.Text.Bits)this._bits & ~changeBit);

        private void SetBit(Microsoft.Iris.ViewItems.Text.Bits changeBit)
        {
            Microsoft.Iris.ViewItems.Text text = this;
            text._bits = (uint)((Microsoft.Iris.ViewItems.Text.Bits)text._bits | changeBit);
        }

        private void ClearBit(Microsoft.Iris.ViewItems.Text.Bits changeBit)
        {
            Microsoft.Iris.ViewItems.Text text = this;
            text._bits = (uint)((Microsoft.Iris.ViewItems.Text.Bits)text._bits & ~changeBit);
        }

        private class MarkedRange
        {
            public string tagName;
            public int firstCharacter;
            public int lastCharacter;
            public uint rangeID;
            public Dictionary<object, object> attributes;
            public Microsoft.Iris.ViewItems.Text.MarkedRange parentRange;
            public TextStyle cachedStyle;
            public TextFragment fragment;
            private static uint s_rangeIDIndicator = 1073741824;

            public Color RangeIDAsColor => new Color(Microsoft.Iris.ViewItems.Text.MarkedRange.s_rangeIDIndicator | this.rangeID);

            public Color GetEffectiveColor(Color defaultColor)
            {
                if (this.cachedStyle != null && this.cachedStyle.HasColor)
                    return this.cachedStyle.Color;
                return this.parentRange != null ? this.parentRange.GetEffectiveColor(defaultColor) : defaultColor;
            }

            public bool IsInFragment
            {
                get
                {
                    bool flag = false;
                    for (Microsoft.Iris.ViewItems.Text.MarkedRange markedRange = this; markedRange != null; markedRange = markedRange.parentRange)
                    {
                        TextStyle cachedStyle = markedRange.cachedStyle;
                        if (cachedStyle != null && cachedStyle.Fragment)
                        {
                            flag = true;
                            break;
                        }
                    }
                    return flag;
                }
            }
        }

        private new enum Bits : uint
        {
            TextFitsWidth = 1,
            TextFitsHeight = 2,
            ClipToHeight = 4,
            WordWrap = 8,
            PasswordMasked = 16, // 0x00000010
            KeepFlowAlive = 32, // 0x00000020
            TextLayoutInvalid = 64, // 0x00000040
            UpdateFragmentsAfterLayout = 128, // 0x00000080
            IgnoreEffectiveScaleChanges = 256, // 0x00000100
            Clipped = 512, // 0x00000200
            InMeasure = 1024, // 0x00000400
            FastMeasureValid = 2048, // 0x00000800
            FastMeasurePossible = 4096, // 0x00001000
            ContributesToWidth = 8192, // 0x00002000
            ScaleDirty = 16384, // 0x00004000
            HasEverPainted = 32768, // 0x00008000
            LineSpacingSet = 65536, // 0x00010000
            CharacterSpacingSet = 131072, // 0x00020000
            EnableKerningSet = 262144, // 0x00040000
            ViewDependent = 524288, // 0x00080000
        }
    }
}
