// Decompiled with JetBrains decompiler
// Type: Microsoft.Zune.Shell.StandAlone
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Win32;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Util;
using System;
using System.Collections;

namespace Microsoft.Zune.Shell
{
    internal class StandAlone
    {
        public static Hashtable Startup(string[] args, string defaultCommandLineSwitch)
        {
            WindowSize windowSize = new WindowSize(1012, 693);
            string str = null;
            bool flag1 = false;
            bool flag2 = false;
            Hashtable hashtable = new Hashtable();
            bool flag3 = false;
            if (args != null)
            {
                foreach (CommandLineArgument commandLineArgument in CommandLineArgument.ParseArgs(args, defaultCommandLineSwitch))
                {
                    switch (commandLineArgument.Name)
                    {
                        case "gdi":
                            Application.RenderingType = RenderingType.GDI;
                            flag3 = true;
                            break;
                        case "switchtogdi":
                            ClientConfiguration.GeneralSettings.RenderingType = 0;
                            break;
                        case "dx9":
                            Application.RenderingType = RenderingType.DX9;
                            flag3 = true;
                            break;
                        case "size":
                            try
                            {
                                windowSize = ParseSize(commandLineArgument.Value);
                                break;
                            }
                            catch (FormatException ex)
                            {
                                break;
                            }
                            catch (ArgumentException ex)
                            {
                                break;
                            }
                        case "minimized":
                            flag2 = true;
                            break;
                        case "nativeframe":
                            flag1 = true;
                            break;
                        case "animations":
                            try
                            {
                                Application.AnimationsEnabled = bool.Parse(commandLineArgument.Value);
                                break;
                            }
                            catch (FormatException ex)
                            {
                                break;
                            }
                        default:
                            hashtable[commandLineArgument.Name] = commandLineArgument.Value;
                            break;
                    }
                }
            }
            if (ClientConfiguration.GeneralSettings.UseGDI)
            {
                ClientConfiguration.GeneralSettings.RenderingType = 0;
                ClientConfiguration.GeneralSettings.UseGDI = false;
            }
            if (!flag3)
            {
                Application.RenderingType = (RenderingType)ClientConfiguration.GeneralSettings.RenderingType;
                Application.RenderingQuality = (RenderingQuality)ClientConfiguration.GeneralSettings.RenderingQuality;
            }
            if (str != null)
            {
                if (str == "ltr")
                    Application.IsRTL = false;
                else if (str == "rtl")
                    Application.IsRTL = true;
            }
            Application.AnimationsEnabled = ClientConfiguration.GeneralSettings.AnimationsEnabled;
            Application.Initialize();
            Application.Window.InitialClientSize = windowSize;
            object obj = Registry.GetValue(ZuneUI.Shell.SettingsRegistryPath, "WindowPosition", null);
            if (obj != null)
            {
                if (obj is string)
                {
                    try
                    {
                        if (!flag2)
                            Application.Window.SetSavedInitialPosition((string)obj);
                        else
                            Application.Window.SetSavedInitialPosition((string)obj, WindowState.Minimized);
                    }
                    catch (ArgumentException ex)
                    {
                    }
                }
            }
            Application.Window.RespectsStartupSettings = true;
            Application.Window.InitialPositionPolicy = WindowPositionPolicy.CenterOnWorkArea | WindowPositionPolicy.ConstrainToWorkArea;
            Application.Window.ShowWindowFrame = flag1;
            Application.Window.SetBackgroundColor(ZuneUI.Shell.WindowColorFromRGB(ClientConfiguration.Shell.BackgroundColor));
            if (!flag2)
                Application.DeferredInvoke(delegate
               {
                   Windowing.ForceSetForegroundWindow(Application.Window.Handle);
               }, DeferredInvokePriority.Low);
            return hashtable;
        }

        private static WindowSize ParseSize(string argument)
        {
            int width = 0;
            int height = 0;
            string[] strArray = argument.Split(',');
            if (strArray.Length == 2)
            {
                width = int.Parse(strArray[0]);
                height = int.Parse(strArray[1]);
            }
            return new WindowSize(width, height);
        }

        public static void Run(DeferredInvokeHandler initialLoadComplete)
        {
            Application.Run(initialLoadComplete);
            if (TaskbarPlayer.Instance.ToolbarVisible)
                return;
            Registry.SetValue(ZuneUI.Shell.SettingsRegistryPath, "WindowPosition", Application.Window.GetSavedPosition(true));
        }

        public static void Shutdown() => Application.Shutdown();
    }
}
