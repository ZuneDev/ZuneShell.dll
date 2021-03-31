// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Drawing.TextFlow
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.OS;
using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI.Drawing;

namespace Microsoft.Iris.Drawing
{
    internal sealed class TextFlow : DisposableObject
    {
        private object _runsList;
        private Vector _lineBounds;
        private Rectangle _bounds;
        private Rectangle _fitBounds;
        private TextRun _lastFitRun;
        private TextRun _firstFitOnFinalLineRun;
        private int _firstVisibleIndex;
        private int _lastVisibleIndex;

        internal TextFlow()
        {
            this._bounds = new Rectangle(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
            this.ResetFitTracking();
            this.ResetVisibleTracking();
        }

        protected override void OnDispose()
        {
            if (this._runsList != null)
            {
                if (this._runsList is TextRun runsList)
                {
                    runsList.UnregisterUsage(this);
                }
                else
                {
                    foreach (SharedDisposableObject runs in (Vector)this._runsList)
                        runs.UnregisterUsage(this);
                }
            }
            base.OnDispose();
        }

        public TextRun this[int index] => this._runsList is TextRun runsList ? runsList : (TextRun)((Vector)this._runsList)[index];

        public int Count
        {
            get
            {
                if (this._runsList is Vector runsList)
                    return runsList.Count;
                return this._runsList == null ? 0 : 1;
            }
        }

        public Vector LineBounds => this._lineBounds;

        internal Rectangle GetLastLineBounds(int lineAlignmentOffset)
        {
            Rectangle left = Rectangle.Zero;
            for (int index = this.Count - 1; index >= 0; --index)
            {
                TextRun run = this[index];
                if (this.IsOnLastLine(run))
                {
                    Point position = run.Position;
                    Size size = run.Size;
                    int x = position.X + lineAlignmentOffset;
                    int y = position.Y;
                    int num1 = x + size.Width;
                    int num2 = y + size.Height;
                    Rectangle right = new Rectangle(x, y, num1 - x, num2 - y);
                    left = !left.IsZero ? Rectangle.Union(left, right) : right;
                }
                else
                    break;
            }
            return left;
        }

        internal bool IsOnLastLine(TextRun run) => run.Line == this.LastFitRun.Line;

        public Rectangle Bounds => this._bounds;

        public Rectangle FitBounds => this._fitBounds;

        public TextRun LastFitRun => this._lastFitRun;

        public TextRun FirstFitRunOnFinalLine => this._firstFitOnFinalLineRun;

        public int FirstVisibleIndex => this._firstVisibleIndex;

        public int LastVisibleIndex => this._lastVisibleIndex;

        public bool HasVisibleRuns => this._firstVisibleIndex != -1;

        public void Add(TextRun run)
        {
            this._bounds = Rectangle.Union(this._bounds, run.LayoutBounds);
            if (this._runsList == null)
            {
                this._runsList = run;
                this._lineBounds = new Vector();
                this._lineBounds.Add(run.RenderBounds);
            }
            else
            {
                if (!(this._runsList is Vector vector))
                {
                    object runsList = this._runsList;
                    vector = new Vector();
                    vector.Add(runsList);
                    this._runsList = vector;
                }
                if (run.UnderlineStyle != NativeApi.UnderlineStyle.None)
                {
                    Rectangle underlineBounds = run.UnderlineBounds;
                    int line1 = run.Line;
                    for (int index = vector.Count - 1; index > 0; --index)
                    {
                        Point position = ((TextRun)vector[index]).Position;
                        int line2 = ((TextRun)vector[index]).Line;
                        if (position.X >= underlineBounds.Left && line1 == line2)
                            ((TextRun)vector[index]).UnderlineStyle = run.UnderlineStyle;
                        else
                            break;
                    }
                }
                if (this._lineBounds.Count < run.Line)
                {
                    this._lineBounds.Add(run.RenderBounds);
                }
                else
                {
                    RectangleF lineBound = (RectangleF)this._lineBounds[run.Line - 1];
                    if ((int)lineBound.Y > (int)run.RenderBounds.Y || lineBound.IsEmpty)
                        this._lineBounds[run.Line - 1] = run.RenderBounds;
                }
                vector.Add(run);
            }
            run.RegisterUsage(this);
        }

        public void AddFit(TextRun run)
        {
            this._fitBounds = Rectangle.Union(this._fitBounds, run.LayoutBounds);
            if (run.LayoutBounds.IsEmpty)
                return;
            if (this._firstFitOnFinalLineRun == null || run.Line > this._firstFitOnFinalLineRun.Line)
                this._firstFitOnFinalLineRun = run;
            this._lastFitRun = run;
        }

        public void AddVisible(int runIndex)
        {
            this[runIndex].Visible = true;
            if (this._firstVisibleIndex == -1)
                this._firstVisibleIndex = runIndex;
            this._lastVisibleIndex = runIndex;
        }

        public void ResetFitTracking()
        {
            this._lastFitRun = null;
            this._fitBounds = new Rectangle(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
        }

        public void ResetVisibleTracking()
        {
            this._firstVisibleIndex = -1;
            this._lastVisibleIndex = int.MinValue;
        }

        public void ClearSprites()
        {
            if (this._runsList == null)
                return;
            if (this._runsList is TextRun runsList)
            {
                runsList.TextSprite = null;
                runsList.HighlightSprite = null;
            }
            else
            {
                foreach (TextRun runs in (Vector)this._runsList)
                {
                    runs.TextSprite = null;
                    runs.HighlightSprite = null;
                }
            }
        }
    }
}
