// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ViewItems.TextRunData
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Render;

namespace Microsoft.Iris.ViewItems
{
    internal class TextRunData
    {
        private TextRun _textRun;
        private Text _textViewItem;
        private Point _position;
        private Size _size;
        private bool _isOnLastLine;
        private int _lineNumber;

        public TextRunData(
          TextRun textRun,
          bool isOnLastLine,
          Text textViewItem,
          int lineAlignmentOffset)
        {
            this._textRun = textRun;
            this._size = textRun.Size;
            this._position = textRun.Position;
            this._position.X += lineAlignmentOffset;
            this._lineNumber = textRun.Line;
            this._isOnLastLine = isOnLastLine;
            this._textViewItem = textViewItem;
        }

        public Point Position => this._position;

        public Size Size => this._size;

        public Color Color => this._textRun == null ? new Color(0U) : this._textRun.Color;

        public int LineNumber => this._lineNumber;

        public TextRun Run => this._textRun;

        public Text TextViewItem => this._textViewItem;

        public bool IsOnLastLine => this._isOnLastLine;

        public void NotifyPaintInvalid()
        {
            if (this.PaintInvalid == null)
                return;
            this.PaintInvalid();
        }

        public override string ToString() => string.Format("[ Text = {0}, Position = {1}, Size = {2} ]", (object)this._textRun.Content, (object)this._position, (object)this._size);

        public event PaintInvalidEventHandler PaintInvalid;
    }
}
