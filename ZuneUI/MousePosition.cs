// Decompiled with JetBrains decompiler
// Type: ZuneUI.MousePosition
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ZuneUI
{
    public class MousePosition : ModelItem
    {
        private int _x;
        private int _y;
        private System.Threading.Timer _checkPostion;
        private bool _invokeOutstanding;

        public MousePosition()
        {
            this._invokeOutstanding = false;
            this._checkPostion = new System.Threading.Timer(new TimerCallback(this.CheckPosition), (object)null, 0, 16);
        }

        private void CheckPosition(object state)
        {
            if (this._invokeOutstanding)
                return;
            this._invokeOutstanding = true;
            int x;
            int y;
            MousePosition.GetCursorScreenPosition(out x, out y);
            Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredUpdatePosition), (object)new object[2]
            {
        (object) x,
        (object) y
            });
        }

        private void DeferredUpdatePosition(object arg)
        {
            object[] objArray = (object[])arg;
            int num1 = (int)objArray[0];
            int num2 = (int)objArray[1];
            RECT lpRect;
            if (!MousePosition.GetWindowRect(ZuneApplication.GetRenderWindow(), out lpRect))
            {
                lpRect.Left = 0;
                lpRect.Top = 0;
                lpRect.Right = 1;
                lpRect.Bottom = 1;
            }
            int num3 = num1 < lpRect.Left ? lpRect.Left : num1;
            int num4 = num3 >= lpRect.Right ? lpRect.Right - 1 : num3;
            int num5 = num2 < lpRect.Top ? lpRect.Top : num2;
            int num6 = num5 >= lpRect.Bottom ? lpRect.Bottom - 1 : num5;
            this.X = num4 - lpRect.Left;
            this.Y = num6 - lpRect.Top;
            this._invokeOutstanding = false;
        }

        public int X
        {
            get => this._x;
            private set
            {
                if (value == this._x)
                    return;
                this._x = value;
                this.FirePropertyChanged(nameof(X));
            }
        }

        public int Y
        {
            get => this._y;
            private set
            {
                if (value == this._y)
                    return;
                this._y = value;
                this.FirePropertyChanged(nameof(Y));
            }
        }

        public static void GetCursorScreenPosition(out int x, out int y)
        {
            MousePosition.POINT lpPoint;
            if (MousePosition.GetCursorPos(out lpPoint))
            {
                x = lpPoint.X;
                y = lpPoint.Y;
            }
            else
                x = y = 0;
        }

        [DllImport("User32.dll")]
        private static extern bool GetCursorPos(out MousePosition.POINT lpPoint);

        [DllImport("User32.dll")]
        private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        private struct POINT
        {
            public int X;
            public int Y;
        }
    }
}
