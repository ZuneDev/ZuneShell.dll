// Decompiled with JetBrains decompiler
// Type: ZuneUI.Win32MessageBox
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

#if WINDOWS
using System.Runtime.InteropServices;
using System.Threading;
#else
using System.Collections.Generic;
using NativeMessageBox;
#endif

namespace ZuneUI
{
    public static class Win32MessageBox
    {
        public static void Show(string text, string caption, Win32MessageBoxType type, DeferredInvokeHandler callback)
        {
#if WINDOWS
            var winHandle = Application.Window.Handle;
            new Thread(args =>
            {
                var num = MessageBox(winHandle, text, caption, type);
                InvokeCallback(callback, num);
            }).Start();
#else
            var onlyType = GetMsgBoxType(type);
            List<MessageBoxButton> buttons = onlyType switch
            {
                Win32MessageBoxType.MB_ABORTRETRYIGNORE => [ButtonAbort, ButtonRetry, ButtonIgnore],
                Win32MessageBoxType.MB_CANCELTRYCONTINUE => [ButtonCancel, ButtonTryAgain, ButtonContinue],
                Win32MessageBoxType.MB_OK => [ButtonOk],
                Win32MessageBoxType.MB_OKCANCEL => [ButtonOk, ButtonCancel],
                Win32MessageBoxType.MB_RETRYCANCEL => [ButtonRetry, ButtonCancel],
                Win32MessageBoxType.MB_YESNO => [ButtonYes, ButtonNo],
                Win32MessageBoxType.MB_YESNOCANCEL => [ButtonYes, ButtonNo, ButtonCancel],
                _ => throw new ArgumentOutOfRangeException(nameof(onlyType), onlyType, null)
            };

            if (type.HasFlag(Win32MessageBoxType.MB_HELP))
                buttons.Add(ButtonHelp);

            var onlyIcon = GetMsgBoxIcon(type);
            var icon = onlyIcon switch
            {
                Win32MessageBoxType.MB_ICONERROR => MessageBoxIcon.Error,
                Win32MessageBoxType.MB_ICONEXCLAMATION => MessageBoxIcon.Warning,
                Win32MessageBoxType.MB_ICONINFORMATION => MessageBoxIcon.Information,
                Win32MessageBoxType.MB_ICONQUESTION => MessageBoxIcon.Question,
                _ => MessageBoxIcon.None
            };

            MessageBoxOptions options = new(message: text, buttons: buttons, title: caption, icon: icon);
            var num = NativeMessageBoxClient.TryShow(options, out var result)
                ? (int)result.ButtonId
                : 0;

            InvokeCallback(callback, num);
#endif
        }

        private static void InvokeCallback(DeferredInvokeHandler callback, int num)
        {
            if (callback == null)
                return;

            Application.DeferredInvoke(callback, num);
        }
    
#if WINDOWS
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int MessageBox(
            IntPtr hWnd,
            string text,
            string caption,
            Win32MessageBoxType type);
#else
        private static Win32MessageBoxType GetMsgBoxType(Win32MessageBoxType options)
        {
            var typeComponent = (uint)options & (uint)Win32MessageBoxType.MB_TYPEMASK;
            return (Win32MessageBoxType)typeComponent;
        }
        
        private static Win32MessageBoxType GetMsgBoxIcon(Win32MessageBoxType options)
        {
            var iconComponent = (uint)options & (uint)Win32MessageBoxType.MB_ICONMASK;
            return (Win32MessageBoxType)iconComponent;
        }
        
        private static readonly MessageBoxButton ButtonOk = new(1u, "OK", MessageBoxButtonKind.Primary);
        private static readonly MessageBoxButton ButtonCancel = new(2u, "Cancel");
        private static readonly MessageBoxButton ButtonAbort = new(3u, "Abort");
        private static readonly MessageBoxButton ButtonRetry = new(4u, "Retry");
        private static readonly MessageBoxButton ButtonIgnore = new(5u, "Ignore");
        private static readonly MessageBoxButton ButtonYes = new(6u, "Yes");
        private static readonly MessageBoxButton ButtonNo = new(7u, "No");
        private static readonly MessageBoxButton ButtonTryAgain = new(10u, "Try Again");
        private static readonly MessageBoxButton ButtonContinue = new(11u, "Continue");
        private static readonly MessageBoxButton ButtonHelp = new(int.MaxValue, "Help");
#endif
    }
}
