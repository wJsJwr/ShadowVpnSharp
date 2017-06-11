using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using ShadowVpnSharp.Helper;

namespace ShadowVpnSharp.Device {
    static class TapDeviceFunc {
        public const string DefaultTapDeviceName = "svTun";

        public static bool CheckTapWindowsIntall() {
            string instKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\TAP-Windows";
            RegistryPermission rp = new RegistryPermission(RegistryPermissionAccess.Read, $"HKEY_LOCAL_MACHINE{instKey}");
            rp.Assert();
            RegistryKey rbase = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey rk = rbase.OpenSubKey(instKey, RegistryKeyPermissionCheck.ReadSubTree);
            if (rk == null) {
                Logger.Error("Tap-Windows Not Found!");
                return false;
            }
            Logger.Info($"Find {rk.GetValue("DisplayName")}");
            ComponentPath.TapWindowsPath = Path.GetDirectoryName(rk.GetValue("UninstallString") as string);
            Logger.Debug($"Tap-Windows Path: {ComponentPath.TapWindowsPath}");
            return true;
        }

        /// <summary>
        /// 检查Tap驱动文件是否存在
        /// </summary>
        /// <returns></returns>
        public static bool CheckTapDriver() {
            DirectoryInfo ds =
                new DirectoryInfo(
                    Path.Combine(new string[] {
                        Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                        @"system32\DriverStore\FileRepository"
                    }));
            foreach (DirectoryInfo drv in ds.GetDirectories("oemvista.inf*")) {
                foreach (FileInfo fi in drv.GetFiles()) {
                    if (fi.Name == "tap0901.sys") {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 获取Tap设备，如果不存在则返回null
        /// </summary>
        /// <param name="deviceName">指定设备名，留空表示任意Tap设备</param>
        /// <returns></returns>
        public static NetworkInterface GetTapDevice(string deviceName = "") {
            var ifs = from adapter in NetworkInterface.GetAllNetworkInterfaces()
                where adapter.Description.Contains("TAP-Windows Adapter V9") && (deviceName == "" || adapter.Name == deviceName)
                select adapter;

            return ifs.Any() ? ifs.First() : null;
        }

        /// <summary>
        /// 在日志中输出所有的网络设备
        /// </summary>
        public static void TraceNetworkInterface() {
            Logger.Debug("List all network interface:");
            StringBuilder sb = new StringBuilder();
            foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces()) {
                sb.AppendLine(
                    $"[{adapter.Id}]{adapter.Name}: {adapter.Description}, Type={adapter.NetworkInterfaceType}, MAC={adapter.GetPhysicalAddress()}");
            }
            Logger.Debug(sb.ToString());
        }

        /// <summary>
        /// 重命名Tap设备
        /// </summary>
        /// <param name="name">新名字</param>
        /// <param name="adapter">Tap设备</param>
        public static void RenameTapDevice(string name, NetworkInterface adapter) {
            if (Os.Ver == Os.V.XpOrLower) throw new NotSupportedException("Windows Version Too Low");
            if (Os.Ver == Os.V.VistaOrSeven) {
                // win7 or lower
                ManagementClass nic = new ManagementClass(@"\\.\ROOT\cimv2:Win32_NetworkAdapter");
                ManagementObject target = null;
                foreach (ManagementObject a in nic.GetInstances()) {
                    if ((string) a["GUID"] == adapter.Id) {
                        target = a;
                        break;
                    }
                }
                if (target != null) {
                    target.SetPropertyValue("NetConnectionID", name);
                    PutOptions po = new PutOptions {Type = PutType.UpdateOnly};
                    target.Put(po);
                }
            } else {
                // win8 or higher
                ManagementClass nic = new ManagementClass(@"\\.\ROOT\StandardCimv2:MSFT_NetAdapter");
                ManagementObject target = null;
                foreach (ManagementObject a in nic.GetInstances()) {
                    if ((string) a["DeviceID"] == adapter.Id) {
                        target = a;
                        break;
                    }
                }
                if (target != null) {
                    ManagementBaseObject param = target.GetMethodParameters("Rename");
                    param["NewName"] = name;
                    target.InvokeMethod("Rename", param, null);
                }
            }


            /*
            else {
                // xp or lower，不确定是否有效(win7测试无效)
                string adapterKey = @"SYSTEM\CurrentControlSet\Control\Network\{4D36E972-E325-11CE-BFC1-08002BE10318}\" + adapter.Id + @"\Connection";
                RegistryKey rbase = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default);
                RegistryKey rk = rbase.OpenSubKey(adapterKey, true);
                rk?.SetValue("Name", name);
            }
            */
        }

        /// <summary>
        /// 安装Tap驱动
        /// </summary>
        public static void InstallTapDriver() {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ComponentPath.TapInstallerPath;
            psi.Arguments = "/S";
            Process ps = new Process {StartInfo = psi};
            ps.Start();
            ps.WaitForExit();
            CheckTapWindowsIntall();
        }

        /// <summary>
        /// 添加Tap设备
        /// </summary>
        public static void AddTapDevice() {
            CallTapInstall($"install \"{ComponentPath.TapWindowsPath}\\driver\\OemVista.inf\" tap0901");
        }


        public static void DeleteTapDevice() {
            CallTapInstall("remove tap0901");
        }


        private static void CallTapInstall(string args) {
            Logger.Debug($"Call tapinstall: {args}");
            var psi = new ProcessStartInfo {
                FileName = $"{ComponentPath.TapWindowsPath}\\bin\\tapinstall.exe",
                Arguments = args,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            //"C:\Program Files\TAP-Windows\bin\tapinstall.exe" install "C:\Program Files\TAP-Windows\driver\OemVista.inf" tap0901


            Process ps = new Process {
                StartInfo = psi,
                EnableRaisingEvents = true
            };
            ps.Exited += (sender, e) => {
                Thread.Sleep(500);// wait for async io
                ps.CancelErrorRead();
                ps.CancelOutputRead();
            };
            ps.OutputDataReceived += (sender, e) => {
                try {
                    Logger.Info($"[tapinstall]{e.Data}");
                } catch (NullReferenceException) {
                    //suppress
                } catch (Exception ex) {
                    Logger.Error($"[tapinstall-err]{ex.Message}");
                }
            };
            ps.ErrorDataReceived += (sender, e) => {
                try {
                    Logger.Error($"[tapinstall]{e.Data}");
                } catch (NullReferenceException) {
                    //suppress
                } catch (Exception ex) {
                    Logger.Error($"[tapinstall-err]{ex.Message}");
                }
            };
            ps.Start();
            ps.BeginErrorReadLine();
            ps.BeginOutputReadLine();
            ps.WaitForExit();

        }

        public static void GetSetupLog() {
            List<string> lastlog = new List<string>();
            using (var sr = new StreamReader(File.OpenRead(ComponentPath.SetupApiLogPath))) {
                while (!sr.EndOfStream) {
                    var currentLine = sr.ReadLine();
                    if (currentLine != null && currentLine.Contains(@">>>  [Device Install (UpdateDriverForPlugAndPlayDevices) - tap0901]")) {
                        lastlog.Clear();
                        lastlog.Add(currentLine);
                        while (currentLine != null && !currentLine.Contains(@"<<<  [Exit ") && !sr.EndOfStream) {
                            currentLine = sr.ReadLine();
                            lastlog.Add(currentLine);
                        }
                    }
                }
            }
            if (lastlog.Count > 0) {
                File.WriteAllLines(ComponentPath.LocalSaLogPath, lastlog);
            }
        }
    }
}