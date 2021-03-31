// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.InputHandlers.TextEditingHandler
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Input;
using Microsoft.Iris.Layout;
using Microsoft.Iris.Markup;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.OS;
using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using Microsoft.Iris.ViewItems;
using System;

namespace Microsoft.Iris.InputHandlers
{
    internal class TextEditingHandler :
      InputHandler,
      IRichTextCallbacks,
      ITextScrollModelCallback,
      IImeCallbacks
    {
        private const uint WM_LBUTTONDOWN = 513;
        private const uint WM_LBUTTONUP = 514;
        private TextEditingHandler.TextEditingCommand _copyCommand;
        private TextEditingHandler.TextEditingCommand _cutCommand;
        private TextEditingHandler.TextEditingCommand _deleteCommand;
        private TextEditingHandler.TextPasteCommand _pasteCommand;
        private TextEditingHandler.TextEditingCommand _selectAllCommand;
        private TextEditingHandler.TextEditingCommand _undoCommand;
        private uint _bits;
        private Range _selection;
        private Point _inputOffset;
        private int _savedMouseYPositionToWorkAroundRichEditBug;
        private EditableTextData _editData;
        private EventHandler _maxLengthChangedHandler;
        private EventHandler _readOnlyChangedHandler;
        private EventHandler _valueChangedHandler;
        private EventHandler _activationStateHandler;
        private Form _activationStateNotifier;
        private Text _textDisplay;
        private CaretInfo _caretInfo;
        private RichText _editControl;
        private CursorID _cursor;
        private TextScrollModel.State _pendingHorizontalScrollState;
        private TextScrollModel.State _pendingVerticalScrollState;
        private TextScrollModel _horizontalScrollModel;
        private TextScrollModel _verticalScrollModel;
        private SimpleCallback _updateScrollbars;
        private InputInfo _pendingPointerDown;
        private Color _linkColor;
        private string _linkClickedParameter;
        private bool _InImeMode;
        private uint _ImeCallbackToken;

        public TextEditingHandler()
        {
            this._caretInfo = new CaretInfo();
            this._editControl = new RichText(true, (IRichTextCallbacks)this);
            this._maxLengthChangedHandler = new EventHandler(this.OnEditableTextMaxLengthChanged);
            this._readOnlyChangedHandler = new EventHandler(this.OnEditableTextReadOnlyChanged);
            this._valueChangedHandler = new EventHandler(this.OnEditableTextValueChanged);
            this._activationStateHandler = new EventHandler(this.OnActivationChanged);
            this.SetBit(TextEditingHandler.Bits.AcceptsEnter);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            this.RemoveEditableTextEventHandlers();
            this.UnregisterImeMessageHandler();
            if (this._activationStateNotifier != null)
                this.UpdateActivationStateHandler(false);
            this.HorizontalScrollModel = (TextScrollModel)null;
            this.VerticalScrollModel = (TextScrollModel)null;
            this._editControl.Dispose();
        }

        protected override void ConfigureInteractivity()
        {
            base.ConfigureInteractivity();
            if (!this.HandleDirect)
                return;
            this.UI.MouseInteractive = true;
            this.UI.KeyInteractive = true;
        }

        public override void OnZoneAttached()
        {
            if (!this.UI.DirectKeyFocus)
                return;
            this.UpdateActivationStateHandler(true);
        }

        private void OnActivationChanged(object sender, EventArgs ea)
        {
            this.WindowIsActivated = this._activationStateNotifier.ActivationState;
            this.UpdateCaretVisibility();
        }

        public EditableTextData EditableTextData
        {
            get => this._editData;
            set
            {
                if (this._editData == value)
                    return;
                this.RemoveEditableTextEventHandlers();
                this._editData = value;
                if (this._editData != null)
                {
                    this.AddEditableTextEventHandlers();
                    this.UpdateMaxLengthOnRichEdit();
                    this.UpdateReadOnlyOnRichEdit();
                    this.UpdateContentOnRichEdit();
                }
                this.FireThreadSafeNotification(NotificationID.EditableTextData);
            }
        }

        private void RemoveEditableTextEventHandlers()
        {
            if (this._editData == null)
                return;
            this._editData.MaxLengthChanged -= this._maxLengthChangedHandler;
            this._editData.ReadOnlyChanged -= this._readOnlyChangedHandler;
            this._editData.ValueChanged -= this._valueChangedHandler;
        }

        private void AddEditableTextEventHandlers()
        {
            this._editData.MaxLengthChanged += this._maxLengthChangedHandler;
            this._editData.ReadOnlyChanged += this._readOnlyChangedHandler;
            this._editData.ValueChanged += this._valueChangedHandler;
        }

        private void UnregisterImeMessageHandler() => RendererApi.IFC(NativeApi.SpUnregisterImeCallbacks(this._ImeCallbackToken));

        private void OnEditableTextMaxLengthChanged(object sender, EventArgs unused) => this.UpdateMaxLengthOnRichEdit();

        private void OnEditableTextReadOnlyChanged(object sender, EventArgs unused) => this.UpdateReadOnlyOnRichEdit();

        private void OnEditableTextValueChanged(object sender, EventArgs unused)
        {
            if (this.InsideValueChangeOnEditableTextData)
                return;
            this.UpdateContentOnRichEdit();
        }

        private void UpdateContentOnRichEdit()
        {
            if (this._editData == null)
                return;
            this.InsideContentChangeOnRichEdit = true;
            this._editControl.Content = this._editData.Value;
            if (this._textDisplay != null)
                this._textDisplay.MarkScaleDirty();
            this.InsideContentChangeOnRichEdit = false;
        }

        private void UpdateReadOnlyOnRichEdit()
        {
            this._editControl.ReadOnly = this._editData.ReadOnly;
            this.UpdateSelectionAndReadOnlyCommands();
        }

        private void UpdateMaxLengthOnRichEdit() => this._editControl.MaxLength = this._editData.MaxLength;

        public Text TextDisplay
        {
            get => this._textDisplay;
            set
            {
                if (this._textDisplay == value)
                    return;
                LayoutCompleteEventHandler completeEventHandler = new LayoutCompleteEventHandler(this.TextLayoutComplete);
                if (this._textDisplay != null)
                {
                    this._textDisplay.ExternalRasterizer = (RichText)null;
                    this._textDisplay.ExternalEditingHandler = (TextEditingHandler)null;
                    this._textDisplay.LayoutComplete -= completeEventHandler;
                }
                this._textDisplay = value;
                if (this._textDisplay != null)
                {
                    this._textDisplay.ExternalRasterizer = this._editControl;
                    this._textDisplay.ExternalEditingHandler = this;
                    this._textDisplay.OnDisplayedContentChange();
                    this._textDisplay.LayoutComplete += completeEventHandler;
                    this.InputOffsetDirty = true;
                }
                this.FireThreadSafeNotification(NotificationID.TextDisplay);
            }
        }

        private void TextLayoutComplete(object sender) => this.InputOffsetDirty = true;

        private void FireTypingInputRejectedEvent() => this.FireThreadSafeNotification(NotificationID.TypingInputRejected);

        public bool DetectUrls
        {
            get => this.GetBit(TextEditingHandler.Bits.DetectUrls);
            set
            {
                if (!this.ChangeBit(TextEditingHandler.Bits.DetectUrls, value))
                    return;
                this._editControl.DetectUrls = value;
                this.FireThreadSafeNotification(NotificationID.DetectUrls);
            }
        }

        public Color LinkColor
        {
            get => this._linkColor;
            set
            {
                if (!(this._linkColor != value))
                    return;
                this._linkColor = value;
                this.FireThreadSafeNotification(NotificationID.LinkColor);
            }
        }

        private void FireLinkClicked() => this.FireThreadSafeNotification(NotificationID.LinkClicked);

        public string LinkClickedParameter
        {
            get => this._linkClickedParameter;
            private set
            {
                if (!(this._linkClickedParameter != value))
                    return;
                this._linkClickedParameter = value;
                this.FireThreadSafeNotification(NotificationID.LinkClickedParameter);
            }
        }

        HRESULT IRichTextCallbacks.SetCursor(EditCursorType type)
        {
            CursorID cursorId = CursorID.NotSpecified;
            switch (type)
            {
                case EditCursorType.IBeam:
                    cursorId = CursorID.IBeam;
                    break;
            }
            if (cursorId != this._cursor)
            {
                this._cursor = cursorId;
                this.UpdateCursor();
            }
            return new HRESULT(0);
        }

        internal override CursorID GetCursor() => this._cursor;

        private bool IgnoreReadOnlyChar(char character) => character == '\r' || character == '\t';

        protected override void OnKeyDown(UIClass ui, KeyStateInfo info)
        {
            bool flag = this.ForwardKeyStateChange(info);
            switch (info.Key)
            {
                case Keys.Back:
                    if (this._editData.ReadOnly)
                        this.FireTypingInputRejectedEvent();
                    info.MarkHandled();
                    break;
                case Keys.Tab:
                    if (flag)
                        info.MarkHandled();
                    this.HandledTabKeyDown = info.Handled;
                    break;
                case Keys.Enter:
                    if (!info.Handled && this.AcceptsEnter && (info.Modifiers == InputModifiers.None && !this._editData.ReadOnly))
                    {
                        this._editData.Submit();
                        info.MarkHandled();
                    }
                    this.HandledEnterKeyDown = info.Handled;
                    break;
            }
        }

        protected override void OnKeyCharacter(UIClass ui, KeyCharacterInfo info)
        {
            if (this.ForwardKeyCharacter(info) && !info.Handled && (this._editData.ReadOnly && !this.IgnoreReadOnlyChar(info.Character)))
                this.FireTypingInputRejectedEvent();
            switch (info.Character)
            {
                case '\b':
                    info.MarkHandled();
                    break;
                case '\t':
                    if (!this.HandledTabKeyDown)
                        break;
                    info.MarkHandled();
                    break;
                case '\r':
                    if (!this.HandledEnterKeyDown)
                        break;
                    info.MarkHandled();
                    break;
            }
        }

        protected override void OnKeyUp(UIClass ui, KeyStateInfo info)
        {
            this.ForwardKeyStateChange(info);
            switch (info.Key)
            {
                case Keys.Back:
                    info.MarkHandled();
                    break;
                case Keys.Tab:
                    if (!this.HandledTabKeyDown)
                        break;
                    info.MarkHandled();
                    this.HandledTabKeyDown = false;
                    break;
                case Keys.Enter:
                    if (!this.HandledEnterKeyDown)
                        break;
                    info.MarkHandled();
                    this.HandledEnterKeyDown = false;
                    break;
            }
        }

        private bool ForwardKeyToRichEdit(KeyStateInfo info)
        {
            bool flag = true;
            switch (info.Key)
            {
                case Keys.Tab:
                    flag = info.Action != KeyAction.Down ? this.HandledTabKeyDown : this.AcceptsTab;
                    break;
                case Keys.Enter:
                    flag = info.Action != KeyAction.Down ? this.HandledEnterKeyDown : this.AcceptsEnter;
                    break;
            }
            return flag;
        }

        private bool ForwardKeyCharacterToRichEdit(KeyCharacterInfo info)
        {
            bool flag = true;
            switch (info.Character)
            {
                case '\t':
                    flag = this.HandledTabKeyDown;
                    break;
                case '\r':
                    flag = this.HandledEnterKeyDown;
                    break;
            }
            return flag;
        }

        private bool ForwardKeyStateChange(KeyStateInfo info)
        {
            if (!this.ForwardKeyToRichEdit(info))
                return false;
            if (this._editControl.ForwardKeyStateNotification(info.NativeMessageID, (int)info.Key, info.ScanCode, (int)info.RepeatCount, (uint)info.Modifiers, info.KeyboardFlags))
                info.MarkHandled();
            return true;
        }

        private bool ForwardKeyCharacter(KeyCharacterInfo info)
        {
            if (!this.ForwardKeyCharacterToRichEdit(info))
                return false;
            if (this._editControl.ForwardKeyCharacterNotification(info.NativeMessageID, (int)info.Character, info.ScanCode, (int)info.RepeatCount, (uint)info.Modifiers, info.KeyboardFlags))
                info.MarkHandled();
            return true;
        }

        protected override void OnGainKeyFocus(UIClass ui, KeyFocusInfo info)
        {
            RendererApi.IFC(NativeApi.SpRegisterImeCallbacks((IImeCallbacks)this, out this._ImeCallbackToken));
            this._editControl.NotifyOfFocusChange(true);
            if (this.Overtype)
                this.SelectAll();
            this.DeliverPendingPointerDown();
            if (this.UI.IsZoned)
                this.UpdateActivationStateHandler(true);
            if (!this._textDisplay.UsePasswordMask && !this._textDisplay.DisableIme)
                return;
            RendererApi.IFC(NativeApi.SpPostDeferredImeMessage(1032U, new UIntPtr(this._ImeCallbackToken), UIntPtr.Zero));
        }

        protected override void OnLoseKeyFocus(UIClass ui, KeyFocusInfo info)
        {
            this._editControl.NotifyOfFocusChange(false);
            this.HandledEnterKeyDown = false;
            this.HandledTabKeyDown = false;
            if (this.UI.IsZoned)
                this.UpdateActivationStateHandler(false);
            this.ClearPendingPointerDown();
            RendererApi.IFC(NativeApi.SpUnregisterImeCallbacks(this._ImeCallbackToken));
        }

        protected override void OnLoseMouseFocus(UIClass ui, MouseFocusInfo info)
        {
        }

        private void UpdateActivationStateHandler(bool add)
        {
            if (add)
            {
                this._activationStateNotifier = (Form)this.UI.Zone.Form;
                this._activationStateNotifier.ActivationChange += this._activationStateHandler;
                this.OnActivationChanged((object)null, EventArgs.Empty);
            }
            else
            {
                this._activationStateNotifier.ActivationChange -= this._activationStateHandler;
                this._activationStateNotifier = (Form)null;
            }
        }

        protected override void OnMouseDoubleClick(UIClass ui, MouseButtonInfo info) => this.ForwardMouseInput((MouseActionInfo)info);

        protected override void OnMouseMove(UIClass ui, MouseMoveInfo info) => this.ForwardMouseInput((MouseActionInfo)info);

        protected override void OnMousePrimaryDown(UIClass ui, MouseButtonInfo info)
        {
            this._pendingPointerDown = (InputInfo)info;
            info.Lock();
            if (!this.UI.DirectKeyFocus && this.HandlerStage == InputHandlerStage.Direct && this.UI.KeyFocusOnMouseDown)
                return;
            this.DeliverPendingPointerDown();
        }

        private void DeliverPendingPointerDown()
        {
            if (this._pendingPointerDown == null)
                return;
            this.MousePrimaryDown = true;
            MouseButtonInfo pendingPointerDown = (MouseButtonInfo)this._pendingPointerDown;
            this._savedMouseYPositionToWorkAroundRichEditBug = pendingPointerDown.Y;
            this.ForwardMouseInput((MouseActionInfo)pendingPointerDown);
            this.ClearPendingPointerDown();
        }

        private void ClearPendingPointerDown()
        {
            if (this._pendingPointerDown == null)
                return;
            this._pendingPointerDown.Unlock();
            this._pendingPointerDown = (InputInfo)null;
        }

        protected override void OnMousePrimaryUp(UIClass ui, MouseButtonInfo info)
        {
            this.MousePrimaryDown = false;
            this.ForwardMouseInput((MouseActionInfo)info);
        }

        protected override void OnMouseSecondaryDown(UIClass ui, MouseButtonInfo info) => this.ForwardMouseInput((MouseActionInfo)info);

        protected override void OnMouseSecondaryUp(UIClass ui, MouseButtonInfo info) => this.ForwardMouseInput((MouseActionInfo)info);

        protected override void OnMouseWheel(UIClass ui, MouseWheelInfo info)
        {
            if (!this._textDisplay.WordWrap)
                return;
            this.ForwardMouseInput((MouseActionInfo)info);
        }

        private bool ForwardMouseInput(
          uint nativeMessageID,
          InputModifiers modifiers,
          MouseButtons button,
          int inputX,
          int inputY,
          int wheelDelta)
        {
            if (this.InputOffsetDirty)
            {
                Vector3 parentOffsetPxlVector;
                ViewItem.GetAccumulatedOffsetAndScale((IZoneDisplayChild)this._textDisplay, (IZoneDisplayChild)this.UI.RootItem, out parentOffsetPxlVector, out Vector3 _);
                this._inputOffset = new Point((int)Math.Floor((double)parentOffsetPxlVector.X), (int)Math.Floor((double)parentOffsetPxlVector.Y));
                this.InputOffsetDirty = false;
            }
            int x = inputX - this._inputOffset.X;
            int y = inputY - this._inputOffset.Y;
            if (this.MousePrimaryDown && this._textDisplay != null && !this._textDisplay.WordWrap)
                y = this._savedMouseYPositionToWorkAroundRichEditBug - this._inputOffset.Y;
            return this._editControl.ForwardMouseInput(nativeMessageID, (uint)modifiers, (int)button, x, y, wheelDelta);
        }

        private void ForwardMouseInput(MouseActionInfo info)
        {
            if (!this.ForwardMouseInput(info.NativeMessageID, info.Modifiers, info.Button, info.X, info.Y, info.WheelDelta))
                return;
            info.MarkHandled();
        }

        public bool Overtype
        {
            get => this.GetBit(TextEditingHandler.Bits.Overtype);
            set
            {
                if (!this.ChangeBit(TextEditingHandler.Bits.Overtype, value))
                    return;
                this.FireThreadSafeNotification(NotificationID.Overtype);
            }
        }

        public bool AcceptsTab
        {
            get => this.GetBit(TextEditingHandler.Bits.AcceptsTab);
            set
            {
                if (!this.ChangeBit(TextEditingHandler.Bits.AcceptsTab, value))
                    return;
                this.FireThreadSafeNotification(NotificationID.AcceptsTab);
            }
        }

        public bool AcceptsEnter
        {
            get => this.GetBit(TextEditingHandler.Bits.AcceptsEnter);
            set
            {
                if (!this.ChangeBit(TextEditingHandler.Bits.AcceptsEnter, value))
                    return;
                this.FireThreadSafeNotification(NotificationID.AcceptsEnter);
            }
        }

        public bool WordWrap
        {
            set
            {
                if (!this.ChangeBit(TextEditingHandler.Bits.WordWrap, value))
                    return;
                this._editControl.SetWordWrap(value);
                if (!value)
                    return;
                this.UpdateContentOnRichEdit();
            }
        }

        public CaretInfo CaretInfo => this._caretInfo;

        private void CreateCommands()
        {
            if (this.GetBit(TextEditingHandler.Bits.CommandsCreated))
                return;
            this.SetBit(TextEditingHandler.Bits.CommandsCreated);
            this._undoCommand = new TextEditingHandler.TextEditingCommand(new SimpleCallback(this._editControl.Undo));
            this._cutCommand = new TextEditingHandler.TextEditingCommand(new SimpleCallback(this._editControl.Cut));
            this._copyCommand = new TextEditingHandler.TextEditingCommand(new SimpleCallback(this._editControl.Copy));
            this._pasteCommand = new TextEditingHandler.TextPasteCommand(new SimpleCallback(this._editControl.Paste));
            this._deleteCommand = new TextEditingHandler.TextEditingCommand(new SimpleCallback(this._editControl.Delete));
            this._selectAllCommand = new TextEditingHandler.TextEditingCommand(new SimpleCallback(this.SelectAll));
            this.UpdateSelectionAndReadOnlyCommands();
            this.UpdateTextBasedCommandAvailability();
        }

        public IUICommand UndoCommand
        {
            get
            {
                this.CreateCommands();
                return (IUICommand)this._undoCommand;
            }
        }

        public void Undo() => this._editControl.Undo();

        public IUICommand CutCommand
        {
            get
            {
                this.CreateCommands();
                return (IUICommand)this._cutCommand;
            }
        }

        public void Cut() => this._editControl.Cut();

        public IUICommand CopyCommand
        {
            get
            {
                this.CreateCommands();
                return (IUICommand)this._copyCommand;
            }
        }

        public void Copy() => this._editControl.Copy();

        public IUICommand PasteCommand
        {
            get
            {
                this.CreateCommands();
                return (IUICommand)this._pasteCommand;
            }
        }

        public void Paste() => this._editControl.Paste();

        public IUICommand DeleteCommand
        {
            get
            {
                this.CreateCommands();
                return (IUICommand)this._deleteCommand;
            }
        }

        public void Delete() => this._editControl.Delete();

        public IUICommand SelectAllCommand
        {
            get
            {
                this.CreateCommands();
                return (IUICommand)this._selectAllCommand;
            }
        }

        public void SelectAll()
        {
            string str = this._editData != null ? this._editData.Value : (string)null;
            if (string.IsNullOrEmpty(str))
                return;
            this.SelectionRange = new Range(0, str.Length);
        }

        public Range SelectionRange
        {
            get => this._selection;
            set
            {
                if (this._selection.IsEqual(value))
                    return;
                this._editControl.SetSelectionRange(value.Begin, value.End);
            }
        }

        private void UpdateTextBasedCommandAvailability()
        {
            this._undoCommand.Available = this._editControl.CanUndo;
            this._selectAllCommand.Available = this._editData != null && !string.IsNullOrEmpty(this._editData.Value);
        }

        private void UpdateSelectionAndReadOnlyCommands()
        {
            if (!this.GetBit(TextEditingHandler.Bits.CommandsCreated))
                return;
            bool flag1 = !this._selection.IsEmpty;
            bool flag2 = this._editData == null || this._editData.ReadOnly;
            this._cutCommand.Available = flag1 && !flag2;
            this._copyCommand.Available = flag1;
            this._deleteCommand.Available = flag1 && !flag2;
            this._pasteCommand.TextIsReadOnly = flag2;
        }

        HRESULT IRichTextCallbacks.TextChanged()
        {
            if (!Application.IsApplicationThread)
            {
                Application.DeferredInvoke((DeferredInvokeHandler)(args => ((IRichTextCallbacks)args).TextChanged()), (object)this, Microsoft.Iris.DeferredInvokePriority.Normal);
                return new HRESULT(0);
            }
            if (this.GetBit(TextEditingHandler.Bits.CommandsCreated))
                this.UpdateTextBasedCommandAvailability();
            if (this._editData != null && !this.InsideContentChangeOnRichEdit)
            {
                this.InsideValueChangeOnEditableTextData = true;
                this._editData.Value = this._editControl.SimpleContent;
                this.InsideValueChangeOnEditableTextData = false;
            }
            return new HRESULT(0);
        }

        HRESULT IRichTextCallbacks.InvalidateContent()
        {
            if (!Application.IsApplicationThread)
            {
                Application.DeferredInvoke((DeferredInvokeHandler)(args => ((IRichTextCallbacks)args).InvalidateContent()), (object)this, Microsoft.Iris.DeferredInvokePriority.Normal);
                return new HRESULT(0);
            }
            if (this._textDisplay != null)
                this._textDisplay.OnDisplayedContentChange();
            this.RefreshCaretPosition();
            return new HRESULT(0);
        }

        HRESULT IRichTextCallbacks.SelectionChanged(
          int selectionStart,
          int selectionEnd)
        {
            this._selection = new Range(selectionStart, selectionEnd);
            this.FireThreadSafeNotification(NotificationID.SelectionRange);
            this.UpdateSelectionAndReadOnlyCommands();
            return new HRESULT(0);
        }

        HRESULT IRichTextCallbacks.MaxLengthExceeded()
        {
            this.FireTypingInputRejectedEvent();
            return new HRESULT(0);
        }

        HRESULT IRichTextCallbacks.SetTimer(uint id, uint timeout)
        {
            this._editControl.SetTimer(id, timeout);
            return new HRESULT(0);
        }

        HRESULT IRichTextCallbacks.KillTimer(uint id)
        {
            this._editControl.KillTimer(id);
            return new HRESULT(0);
        }

        HRESULT IRichTextCallbacks.CreateCaret(int width, int height)
        {
            this._caretInfo.CreateCaret(new Size(width, height));
            return new HRESULT(0);
        }

        HRESULT IRichTextCallbacks.SetCaretPos(int x, int y)
        {
            this._caretInfo.SetCaretPosition(new Point(x, y));
            return new HRESULT(0);
        }

        HRESULT IRichTextCallbacks.ShowCaret(bool visible)
        {
            this.RichEditCaretVisible = visible;
            this.UpdateCaretVisibility();
            return new HRESULT(0);
        }

        HRESULT IRichTextCallbacks.ClientToWindow(Point pt, out Point ppt)
        {
            ppt = this._textDisplay == null ? new Point(0, 0) : this._textDisplay.ClientToWindow(pt);
            return new HRESULT(0);
        }

        HRESULT IRichTextCallbacks.ClientToScreen(Point pt, out Point ppt)
        {
            ppt = this._textDisplay.ClientToScreen(pt);
            return new HRESULT(0);
        }

        HRESULT IRichTextCallbacks.LinkClicked(int start, int end)
        {
            string str = this._editData != null ? this._editData.Value : string.Empty;
            if (string.IsNullOrEmpty(str))
            {
                this.LinkClickedParameter = string.Empty;
            }
            else
            {
                end = end >= 0 ? Math.Min(str.Length, end) : str.Length;
                start = Math.Min(start, end);
                this.LinkClickedParameter = str.Substring(start, end - start);
            }
            this.FireLinkClicked();
            return new HRESULT(0);
        }

        HRESULT IImeCallbacks.OnImeMessageReceived(
          uint message,
          UIntPtr wParam,
          UIntPtr lParam)
        {
            switch (message)
            {
                case 269:
                    this.InImeCompositionMode = true;
                    break;
                case 270:
                    this.InImeCompositionMode = false;
                    break;
            }
            return this._editControl.ForwardImeMessage(message, wParam, lParam);
        }

        public bool InImeCompositionMode
        {
            get => this._InImeMode;
            set
            {
                if (this._InImeMode == value)
                    return;
                this.CaretInfo.IgnoreIdealWidth = value;
                this._InImeMode = value;
                this.FireThreadSafeNotification(NotificationID.InImeCompositionMode);
            }
        }

        HRESULT IRichTextCallbacks.SetScrollRange(
          int whichBarInt,
          int minPosition,
          int extent,
          int viewExtent,
          int scrollPosition)
        {
            if (whichBarInt == 1 && this._verticalScrollModel != null)
            {
                this.SetScrollRange(ref this._pendingVerticalScrollState, minPosition, extent, viewExtent, scrollPosition);
                this.ScheduleScrollbarUpdate(true);
            }
            else if (this._horizontalScrollModel != null)
            {
                this.SetScrollRange(ref this._pendingHorizontalScrollState, minPosition, extent, viewExtent, scrollPosition);
                this.ScheduleScrollbarUpdate(false);
            }
            return new HRESULT(0);
        }

        private void SetScrollRange(
          ref TextScrollModel.State state,
          int minPosition,
          int extent,
          int viewExtent,
          int scrollAmount)
        {
            state.Min = minPosition;
            state.Extent = extent;
            state.ViewExtent = viewExtent;
            state.ScrollAmount = scrollAmount;
        }

        private bool IsVerticalScrollbar(ScrollbarType whichBar) => whichBar == ScrollbarType.Vertical || whichBar == ScrollbarType.Both;

        private bool IsHorizontalScrollbar(ScrollbarType whichBar) => whichBar == ScrollbarType.Horizontal || whichBar == ScrollbarType.Both;

        HRESULT IRichTextCallbacks.EnableScrollbar(
          int whichBarInt,
          ScrollbarEnableFlags flags)
        {
            ScrollbarType whichBar = (ScrollbarType)whichBarInt;
            if (this.IsVerticalScrollbar(whichBar) && this._verticalScrollModel != null)
            {
                this.EnableScrollbar(ref this._pendingVerticalScrollState, flags);
                this.ScheduleScrollbarUpdate(true);
            }
            if (this.IsHorizontalScrollbar(whichBar) && this._horizontalScrollModel != null)
            {
                this.EnableScrollbar(ref this._pendingHorizontalScrollState, flags);
                this.ScheduleScrollbarUpdate(false);
            }
            return new HRESULT(0);
        }

        private void EnableScrollbar(ref TextScrollModel.State state, ScrollbarEnableFlags flags)
        {
            switch (flags)
            {
                case ScrollbarEnableFlags.EnableBoth:
                    state.CanScrollUp = true;
                    state.CanScrollDown = true;
                    break;
                case ScrollbarEnableFlags.DisableNear:
                    state.CanScrollUp = false;
                    state.CanScrollDown = true;
                    break;
                case ScrollbarEnableFlags.DisableFar:
                    state.CanScrollUp = true;
                    state.CanScrollDown = false;
                    break;
                case ScrollbarEnableFlags.DisableBoth:
                    state.CanScrollUp = false;
                    state.CanScrollDown = false;
                    break;
            }
        }

        private void ScheduleScrollbarUpdate(bool vertical)
        {
            bool flag = !this.GetBit(TextEditingHandler.Bits.PendingVerticalScrollbarUpdate) && !this.GetBit(TextEditingHandler.Bits.PendingHorizontalScrollbarUpdate);
            if (vertical)
                this.SetBit(TextEditingHandler.Bits.PendingVerticalScrollbarUpdate);
            else
                this.SetBit(TextEditingHandler.Bits.PendingHorizontalScrollbarUpdate);
            if (!flag)
                return;
            if (this._updateScrollbars == null)
                this._updateScrollbars = new SimpleCallback(this.UpdateScrollbars);
            DeferredCall.Post(DispatchPriority.Normal, this._updateScrollbars);
        }

        private void UpdateScrollbars()
        {
            if (this.GetBit(TextEditingHandler.Bits.PendingVerticalScrollbarUpdate))
            {
                this.ClearBit(TextEditingHandler.Bits.PendingVerticalScrollbarUpdate);
                this._verticalScrollModel.UpdateState(this._pendingVerticalScrollState);
                this._pendingVerticalScrollState = new TextScrollModel.State();
            }
            if (!this.GetBit(TextEditingHandler.Bits.PendingHorizontalScrollbarUpdate))
                return;
            this.ClearBit(TextEditingHandler.Bits.PendingHorizontalScrollbarUpdate);
            this._horizontalScrollModel.UpdateState(this._pendingHorizontalScrollState);
            this._pendingHorizontalScrollState = new TextScrollModel.State();
        }

        public TextScrollModel HorizontalScrollModel
        {
            get
            {
                if (this._horizontalScrollModel == null)
                    this.StoreScrollModel(ref this._horizontalScrollModel, new TextScrollModel());
                return this._horizontalScrollModel;
            }
            set
            {
                if (this._horizontalScrollModel == value)
                    return;
                this.StoreScrollModel(ref this._horizontalScrollModel, value);
                this.FireThreadSafeNotification(NotificationID.HorizontalScrollModel);
            }
        }

        public TextScrollModel VerticalScrollModel
        {
            get
            {
                if (this._verticalScrollModel == null)
                    this.StoreScrollModel(ref this._verticalScrollModel, new TextScrollModel());
                return this._verticalScrollModel;
            }
            set
            {
                if (this._verticalScrollModel == value)
                    return;
                this.StoreScrollModel(ref this._verticalScrollModel, value);
                this.FireThreadSafeNotification(NotificationID.VerticalScrollModel);
            }
        }

        private void StoreScrollModel(ref TextScrollModel storage, TextScrollModel newDude)
        {
            if (storage != null)
                storage.DetachCallbacks();
            storage = newDude;
            if (storage != null)
                storage.AttachCallbacks((ITextScrollModelCallback)this);
            this._editControl.SetScrollbars(this._horizontalScrollModel != null, this._verticalScrollModel != null);
        }

        private ScrollbarType WhichScrollbar(TextScrollModel who) => who != this._verticalScrollModel ? ScrollbarType.Horizontal : ScrollbarType.Vertical;

        void ITextScrollModelCallback.ScrollUp(TextScrollModel who) => this._editControl.ScrollUp(this.WhichScrollbar(who));

        void ITextScrollModelCallback.ScrollDown(TextScrollModel who) => this._editControl.ScrollDown(this.WhichScrollbar(who));

        void ITextScrollModelCallback.PageUp(TextScrollModel who) => this._editControl.PageUp(this.WhichScrollbar(who));

        void ITextScrollModelCallback.PageDown(TextScrollModel who) => this._editControl.PageDown(this.WhichScrollbar(who));

        void ITextScrollModelCallback.ScrollToPosition(
          TextScrollModel who,
          int whereTo)
        {
            this._editControl.ScrollToPosition(this.WhichScrollbar(who), whereTo);
        }

        private void UpdateCaretVisibility() => this.CaretInfo.SetVisible(this.RichEditCaretVisible && this.WindowIsActivated);

        private void RefreshCaretPosition()
        {
            if (!this.CaretInfo.Visible || this.MousePrimaryDown || this._editControl == null)
                return;
            this._editControl.NotifyOfFocusChange(true);
        }

        private bool HandledTabKeyDown
        {
            get => this.GetBit(TextEditingHandler.Bits.HandledTabKeyDown);
            set => this.SetBit(TextEditingHandler.Bits.HandledTabKeyDown, value);
        }

        private bool HandledEnterKeyDown
        {
            get => this.GetBit(TextEditingHandler.Bits.HandledEnterKeyDown);
            set => this.SetBit(TextEditingHandler.Bits.HandledEnterKeyDown, value);
        }

        private bool InputOffsetDirty
        {
            get => this.GetBit(TextEditingHandler.Bits.InputOffsetDirty);
            set => this.SetBit(TextEditingHandler.Bits.InputOffsetDirty, value);
        }

        private bool MousePrimaryDown
        {
            get => this.GetBit(TextEditingHandler.Bits.MousePrimaryDown);
            set => this.SetBit(TextEditingHandler.Bits.MousePrimaryDown, value);
        }

        private bool InsideValueChangeOnEditableTextData
        {
            get => this.GetBit(TextEditingHandler.Bits.InsideValueChangeOnEditableTextData);
            set => this.SetBit(TextEditingHandler.Bits.InsideValueChangeOnEditableTextData, value);
        }

        private bool InsideContentChangeOnRichEdit
        {
            get => this.GetBit(TextEditingHandler.Bits.InsideContentChangeOnRichEdit);
            set => this.SetBit(TextEditingHandler.Bits.InsideContentChangeOnRichEdit, value);
        }

        private bool RichEditCaretVisible
        {
            get => this.GetBit(TextEditingHandler.Bits.RichEditCaretVisible);
            set => this.SetBit(TextEditingHandler.Bits.RichEditCaretVisible, value);
        }

        private bool WindowIsActivated
        {
            get => this.GetBit(TextEditingHandler.Bits.WindowIsActivated);
            set => this.SetBit(TextEditingHandler.Bits.WindowIsActivated, value);
        }

        private bool GetBit(TextEditingHandler.Bits lookupBit) => ((TextEditingHandler.Bits)this._bits & lookupBit) != (TextEditingHandler.Bits)0;

        private void SetBit(TextEditingHandler.Bits changeBit)
        {
            TextEditingHandler textEditingHandler = this;
            textEditingHandler._bits = (uint)((TextEditingHandler.Bits)textEditingHandler._bits | changeBit);
        }

        private void SetBit(TextEditingHandler.Bits changeBit, bool value) => this._bits = value ? (uint)((TextEditingHandler.Bits)this._bits | changeBit) : (uint)((TextEditingHandler.Bits)this._bits & ~changeBit);

        private void ClearBit(TextEditingHandler.Bits changeBit)
        {
            TextEditingHandler textEditingHandler = this;
            textEditingHandler._bits = (uint)((TextEditingHandler.Bits)textEditingHandler._bits & ~changeBit);
        }

        private bool ChangeBit(TextEditingHandler.Bits changeBit, bool value)
        {
            uint num = value ? (uint)((TextEditingHandler.Bits)this._bits | changeBit) : (uint)((TextEditingHandler.Bits)this._bits & ~changeBit);
            bool flag = (int)num != (int)this._bits;
            if (flag)
                this._bits = num;
            return flag;
        }

        private enum Bits
        {
            AcceptsTab = 1,
            AcceptsEnter = 2,
            HandledTabKeyDown = 4,
            HandledEnterKeyDown = 16, // 0x00000010
            InputOffsetDirty = 32, // 0x00000020
            MousePrimaryDown = 64, // 0x00000040
            Overtype = 128, // 0x00000080
            InsideValueChangeOnEditableTextData = 256, // 0x00000100
            InsideContentChangeOnRichEdit = 512, // 0x00000200
            RichEditCaretVisible = 1024, // 0x00000400
            WindowIsActivated = 2048, // 0x00000800
            WordWrap = 4096, // 0x00001000
            CommandsCreated = 8192, // 0x00002000
            PendingHorizontalScrollbarUpdate = 16384, // 0x00004000
            PendingVerticalScrollbarUpdate = 32768, // 0x00008000
            DetectUrls = 65536, // 0x00010000
        }

        private class TextEditingCommand : UICommand
        {
            private SimpleCallback _onInvoked;

            public TextEditingCommand(SimpleCallback onInvoked) => this._onInvoked = onInvoked;

            protected override void OnInvoked()
            {
                base.OnInvoked();
                this._onInvoked();
            }
        }

        private class TextPasteCommand : TextEditingHandler.TextEditingCommand, IUICommand
        {
            private bool _isReadOnly;

            public TextPasteCommand(SimpleCallback onInvoked)
              : base(onInvoked)
            {
            }

            bool IUICommand.Available
            {
                get => Clipboard.ContainsText() && !this._isReadOnly;
                set
                {
                }
            }

            public bool TextIsReadOnly
            {
                get => this._isReadOnly;
                set => this._isReadOnly = value;
            }
        }
    }
}
