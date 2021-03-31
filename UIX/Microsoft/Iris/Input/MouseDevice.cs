// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Input.MouseDevice
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Render;

namespace Microsoft.Iris.Input
{
    internal class MouseDevice : InputDevice
    {
        public const uint LButtonDown = 513;
        public const uint LButtonUp = 514;
        public const uint LButtonDblClk = 515;
        public const uint RButtonDown = 516;
        public const uint RButtonUp = 517;
        public const uint RButtonDblClk = 518;
        public const uint MButtonDown = 519;
        public const uint MButtonUp = 520;
        public const uint MButtonDblClk = 521;
        public const uint XButtonDown = 523;
        public const uint XButtonUp = 524;
        public const uint XButtonDblClk = 525;
        public const uint MouseMove = 512;
        public const uint MouseWheel = 522;
        public const uint MouseLeave = 675;

        internal MouseDevice(InputManager manager)
          : base(manager)
        {
        }

        public InputModifiers MouseModifiers => this.Manager.HACK_SystemModifiers & InputModifiers.AllMouse;

        public bool OnRawInput(uint message, InputModifiers modifiers, ref RawMouseData args)
        {
            IRawInputSite visCapture = args._visCapture;
            IRawInputSite visNatural = args._visNatural;
            this.Manager.HACK_UpdateSystemModifiers(modifiers);
            switch (message)
            {
                case 512:
                case 513:
                case 514:
                case 515:
                case 516:
                case 517:
                case 518:
                case 519:
                case 520:
                case 521:
                case 523:
                case 524:
                case 525:
                    this.Manager.MostRecentPhysicalMousePos = new Point(args._physicalX, args._physicalY);
                    this.Manager.Queue.RawMouseMove(visCapture, visNatural, this.Manager.Modifiers, ref args);
                    break;
                case 675:
                    this.Manager.Queue.RawMouseLeave();
                    break;
            }
            bool state = false;
            switch (message)
            {
                case 513:
                case 515:
                case 516:
                case 518:
                case 519:
                case 521:
                case 523:
                case 525:
                    state = true;
                    goto case 514;
                case 514:
                case 517:
                case 520:
                case 524:
                    this.Manager.Queue.RawMouseButtonState(message, visCapture, visNatural, this.Manager.Modifiers, args._button, state);
                    break;
                case 522:
                    this.Manager.Queue.RawMouseWheel(this.Manager.Modifiers, ref args);
                    break;
            }
            return true;
        }

        public bool OnRawInput(
          uint message,
          InputModifiers modifiers,
          ref RawDragData args,
          object data)
        {
            this.Manager.HACK_UpdateSystemModifiers(modifiers);
            this.Manager.MostRecentPhysicalMousePos = new Point(args._positionX, args._positionY);
            IRawInputSite visCapture = args._visCapture;
            DragOperation formOperation = (DragOperation)message;
            this.Manager.Queue.RawDragDrop(visCapture, data, args._positionX, args._positionY, modifiers, formOperation, args);
            return true;
        }
    }
}
