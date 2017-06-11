using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using ShadowVpnSharp.Device;
using ShadowVpnSharp.Helper;
using ShadowVpnSharp.View;
using ShadowVpnSharp.Configuration;
using fastJSON;
using ShadowVpnSharp.Core;
using ShadowVpnSharp.Update;

namespace ShadowVpnSharp {
    static class Program {

        public static Config UserConfig = null;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main(string[] args) {
            Logger.Init();

            JSON.Parameters.SerializeNullValues = false;
            JSON.Parameters.UseExtensions = false;



            Application.ApplicationExit += (sender, e) => {
                Logger.Close();
            };
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Os.Init();
            bool hasRun = false;
            if (args.Length > 0) {
                if (args[0] == "-d") {
                    WindowsIdentity identity = WindowsIdentity.GetCurrent();
                    WindowsPrincipal principal = new WindowsPrincipal(identity);
                    if(principal.IsInRole(WindowsBuiltInRole.Administrator)) {
                        if (args.Contains("-r")) {
                            Application.ApplicationExit += (sender, e) => {
                                ProcessStartInfo psi = new ProcessStartInfo {
                                    FileName = Application.ExecutablePath,
                                    Verb = "runas"
                                };
                                new Process {StartInfo = psi}.Start();
                            };
                        }
                        Application.Run(new TapChecker());
                        hasRun = true;
                    }
                } else if(args[0] == "-r") {
                    TapDeviceFunc.GetSetupLog();
                    hasRun = true;
                } else if (args[0] == "--test") {
                    
                } else if (args[0] == "--diagnostic") {
                    Application.Run(new DiagnosticProgress());
                    hasRun = true;
                } else if (args[0] == "-u") {
                    GetRunningProc()?.WaitForExit();
                    if (args.Length == 2 && args[1].StartsWith("--from")) {
                        Updater.RemoveDir(args[1].Substring(7));
                    } else {
                        Updater.Updating();
                        hasRun = true;
                    }
                }
            }
            if (!hasRun) {
                SelfCheck();
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                // handle UI exceptions
                Application.ThreadException += Application_ThreadException;
                // handle non-UI exceptions
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                Application.ApplicationExit += Application_ApplicationExit;
                Application.Run(new EntryWindow());
            }
        }

        private static void Application_ApplicationExit(object sender, EventArgs e) {
            // detach static event handlers
            Application.ApplicationExit -= Application_ApplicationExit;
            Application.ThreadException -= Application_ThreadException;
        }
        private static int exited = 0;
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            if (Interlocked.Increment(ref exited) == 1) {
                string msg = e.ExceptionObject.ToString();
                RecoverEnv();
                Logger.Error(msg);
                MessageBox.Show($"程序意外退出{Environment.NewLine}{msg}", "SVS non-UI Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e) {
            if (Interlocked.Increment(ref exited) == 1) {
                string msg = $"Exception Detail: {Environment.NewLine}{e.Exception}";
                Logger.Error(msg);
                RecoverEnv();
                MessageBox.Show($@"程序意外退出{Environment.NewLine}{msg}", "SVS UI Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private static void RecoverEnv() {
            EntryWindow.Daemon?.Kill();
        }

        private static Process GetRunningProc() {
            return Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).FirstOrDefault(process => process.Id != Process.GetCurrentProcess().Id);
        }


        private static void SelfCheck() {
            if (!CheckdotNetVersion()) {
                string msg = @"您尚未安装最新版本的Microsoft .Net Framework，请使用程序目录下的安装包安装。";
                Logger.Error(msg);
                MessageBox.Show(msg);
                Environment.Exit(1);
            }
            if (!NetworkInterface.GetIsNetworkAvailable()) {
                string msg = @"无可用网络连接，请确保电脑已连接至互联网。";
                Logger.Error(msg);
                MessageBox.Show(msg);
                Environment.Exit(-2);
            }
            if (!File.Exists(ComponentPath.VpnBinaryPath)) {
                string msg = $@"缺少关键文件{ComponentPath.VpnBinaryPath}";
                Logger.Error(msg);
                MessageBox.Show(msg);
                Environment.Exit(-3);
            }
            if (GetRunningProc() != null) {
                string msg = @"ShadowVpnSharp正在运行中。";
                Logger.Error(msg);
                MessageBox.Show(msg);
                Environment.Exit(-4);
            }
            try {
                UserConfig = Config.FromFile();
                if (!UserConfig.SelfCheck() || !TapDeviceFunc.CheckTapWindowsIntall()) {
                    if (!File.Exists(ComponentPath.TapInstallerPath)) {
                        string _msg = $@"缺少关键文件{ComponentPath.TapInstallerPath}";
                        Logger.Error(_msg);
                        MessageBox.Show(_msg);
                        Environment.Exit(-5);
                    }

                    string msg = "没有找到合适的网络设备，即将执行硬件检测。";
                    Logger.Error(msg);
                    MessageBox.Show(msg);
                    ProcessStartInfo psi = new ProcessStartInfo {
                        FileName = Application.ExecutablePath,
                        Arguments = "-d -r",
                        Verb = "runas"
                    };

                    Process ps = new Process { StartInfo = psi };
                    try {
                        ps.Start();
                        Environment.Exit(2);
                    } catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                    }
                }
            } catch (Exception ex) {
                Logger.Error(ex.Message);
                Logger.Error(ex.StackTrace);
                MessageBox.Show(ex.Message);
                Environment.Exit(-1);
            }
        }

        private static bool CheckdotNetVersion() {
            var v = Environment.Version;
            if (v.Major < 4) return false;
            if (v.Build < 30319) return false;
            return v.Revision >= 42000;
        }
    }
}
