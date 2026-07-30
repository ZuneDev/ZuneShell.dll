// Decompiled with JetBrains decompiler
// Type: ZuneUI.MousePosition
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using Microsoft.Iris;
using Timer = System.Threading.Timer;

namespace ZuneUI
{
    [Serializable]
    public class MousePosition : ModelItem
    {
        private int _x;
        private int _y;
        private Timer _checkPosition;
        private bool _invokeOutstanding;

        public MousePosition()
        {
            _invokeOutstanding = false;
            _checkPosition = new Timer(CheckPosition, null, 0, 16);
        }

        private void CheckPosition(object state)
        {
            if (_invokeOutstanding)
                return;
            _invokeOutstanding = true;
            GetCursorScreenPosition(out var x, out var y);
            Application.DeferredInvoke(DeferredUpdatePosition, new object[] { x, y });
        }

        private void DeferredUpdatePosition(object arg)
        {
            var objArray = (object[])arg;
            var num1 = (int)objArray[0];
            var num2 = (int)objArray[1];
            GetWindowRect(out var left, out var top, out var right, out var bottom);
            var num3 = num1 < left ? left : num1;
            var num4 = num3 >= right ? right - 1 : num3;
            var num5 = num2 < top ? top : num2;
            var num6 = num5 >= bottom ? bottom - 1 : num5;
            X = num4 - left;
            Y = num6 - top;
            _invokeOutstanding = false;
        }

        public int X
        {
            get => _x;
            private set
            {
                if (value == _x)
                    return;
                _x = value;
                FirePropertyChanged(nameof(X));
            }
        }

        public int Y
        {
            get => _y;
            private set
            {
                if (value == _y)
                    return;
                _y = value;
                FirePropertyChanged(nameof(Y));
            }
        }

#if WINDOWS
        public static void GetCursorScreenPosition(out int x, out int y)
        {
            POINT lpPoint;
            if (GetCursorPos(out lpPoint))
            {
                x = lpPoint.X;
                y = lpPoint.Y;
            }
            else
                x = y = 0;
        }

        private static void GetWindowRect(out int left, out int top, out int right, out int bottom)
        {
            if (GetWindowRect(ZuneApplication.GetRenderWindow(), out Vanara.PInvoke.RECT lpRect))
            {
                left = lpRect.Left;
                top = lpRect.Top;
                right = lpRect.Right;
                bottom = lpRect.Bottom;
            }
            else
            {
                left = top = 0;
                right = bottom = 1;
            }
        }

        [DllImport("User32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("User32.dll")]
        private static extern bool GetWindowRect(IntPtr hwnd, out Vanara.PInvoke.RECT lpRect);

        private struct POINT
        {
            public int X;
            public int Y;
        }
#else
        // TODO(stage 3): no cross-platform cursor-position/window-rect abstraction exists
        // yet (Iris's IRenderWindow has no screen-space cursor query). Stubbed to a no-op
        // so MousePosition compiles and no-ops on non-Windows rather than needing a raw,
        // ungated User32 P/Invoke (which never worked outside Windows in the first place --
        // this file was never touched since the original decompile). Logged in
        // logs/ZuneUI/BuildFixes.md.
        public static void GetCursorScreenPosition(out int x, out int y) => x = y = 0;

        private static void GetWindowRect(out int left, out int top, out int right, out int bottom)
        {
            left = top = 0;
            right = bottom = 1;
        }
#endif

        public override string ToString() => $"{{MousePosition ({X}, {Y})}}";
    }
}
