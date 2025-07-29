// Decompiled with JetBrains decompiler
// Type: Microsoft.Zune.Shell.StandAlone
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Iris.Debug;
using Microsoft.Win32;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Util;
using System;
using System.Collections;

namespace Microsoft.Zune.Shell
{
    internal class StandAlone
    {
        public static Hashtable Startup(string[] args, string defaultCommandLineSwitch) => Startup(args, defaultCommandLineSwitch, null);

        public static Hashtable Startup(string[] args, string defaultCommandLineSwitch, Func<IDebuggerServer> debuggerFactory)
        {
            WindowSize windowSize = new WindowSize(1012, 693);
            string textDirectionOption = null;
            bool useNativeFrame = false;
            bool minimized = false;
            Hashtable hashtable = new Hashtable();
            bool overrideRenderSettings = false;
            if (args != null)
            {
                foreach (CommandLineArgument commandLineArgument in CommandLineArgument.ParseArgs(args, defaultCommandLineSwitch))
                {
                    switch (commandLineArgument.Name)
                    {
                        case "gdi":
                            Application.RenderingType = RenderingType.GDI;
                            overrideRenderSettings = true;
                            break;
                        case "switchtogdi":
                            ClientConfiguration.GeneralSettings.RenderingType = 0;
                            break;
                        case "dx9":
                            Application.RenderingType = RenderingType.DX9;
                            overrideRenderSettings = true;
                            break;
                        case "size":
                            try
                            {
                                windowSize = ParseSize(commandLineArgument.Value);
                            }
                            catch (FormatException) { }
                            catch (ArgumentException) { }
                            break;
                        case "minimized":
                            minimized = true;
                            break;
                        case "nativeframe":
                            useNativeFrame = true;
                            break;
                        case "animations":
                            try
                            {
                                Application.AnimationsEnabled = bool.Parse(commandLineArgument.Value);
                            }
                            catch (FormatException) { }
                            break;
                        case "uixdebuguri":
                            var debugConnectionUri = commandLineArgument.Value ?? DebugRemoting.DEFAULT_TCP_URI.OriginalString;
                            debuggerFactory = () => new Iris.Debug.SystemNet.NetDebuggerServer(debugConnectionUri);
                            break;
                        case "uixtrace":
                            try
                            {
                                int idx = commandLineArgument.Value.IndexOf(':');
                                byte level;
                                TraceCategory cat;
                                if (idx != -1)
                                {
                                    level = byte.Parse(commandLineArgument.Value.Substring(idx + 1));
                                    cat = (TraceCategory)Enum.Parse(typeof(TraceCategory), commandLineArgument.Value.Substring(0, idx));
                                }
                                else
                                {
                                    level = 1;
                                    cat = (TraceCategory)Enum.Parse(typeof(TraceCategory), commandLineArgument.Value);
                                }
                                Application.DebugSettings.TraceSettings.SetCategoryLevel(cat, level);
                            }
                            catch (FormatException) { }
                            break;

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

            if (!overrideRenderSettings)
            {
                Application.RenderingType = (RenderingType)ClientConfiguration.GeneralSettings.RenderingType;
                Application.RenderingQuality = (RenderingQuality)ClientConfiguration.GeneralSettings.RenderingQuality;
                Application.AnimationsEnabled = ClientConfiguration.GeneralSettings.AnimationsEnabled;

#if WINDOWS
                unsafe
                {
                    Vanara.BOOL systemAnimationsEnabled;
                    IntPtr pSystemAnimationsEnabled = new(&systemAnimationsEnabled);
                    if (Vanara.PInvoke.User32.SystemParametersInfo(Vanara.PInvoke.User32.SPI.SPI_GETCLIENTAREAANIMATION, pvParam: pSystemAnimationsEnabled)
                        && !systemAnimationsEnabled)
                    {
                        Application.AnimationsEnabled = systemAnimationsEnabled;
                    }
                }
#endif
            }

            if (textDirectionOption != null)
            {
                if (textDirectionOption == "ltr")
                    Application.IsRTL = false;
                else if (textDirectionOption == "rtl")
                    Application.IsRTL = true;
            }

            Application.Initialize(debuggerFactory);
            Application.Window.InitialClientSize = windowSize;

#if WINDOWS
            object regWindowPosition = Registry.GetValue(ZuneUI.Shell.SettingsRegistryPath, "WindowPosition", null);
            if (regWindowPosition != null)
            {
                if (regWindowPosition is string initialPos)
                {
                    try
                    {
                        if (!minimized)
                            Application.Window.SetSavedInitialPosition(initialPos);
                        else
                            Application.Window.SetSavedInitialPosition(initialPos, WindowState.Minimized);
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }
            Application.Window.RespectsStartupSettings = true;
#endif

            Application.Window.InitialPositionPolicy = WindowPositionPolicy.CenterOnWorkArea | WindowPositionPolicy.ConstrainToWorkArea;
            Application.Window.ShowWindowFrame = useNativeFrame;
            Application.Window.SetBackgroundColor(ZuneUI.Shell.WindowColorFromRGB(ClientConfiguration.Shell.BackgroundColor));
            if (!minimized)
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

#if WINDOWS
            Registry.SetValue(ZuneUI.Shell.SettingsRegistryPath, "WindowPosition", Application.Window.GetSavedPosition(true));
#endif
        }

        public static void Shutdown() => Application.Shutdown();
    }
}
