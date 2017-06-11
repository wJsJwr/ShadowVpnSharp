using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ShadowVpnSharp.Device;

namespace ShadowVpnSharp.Helper {
    internal static class Diagnostic {
        public static void CallRouteExe(string args) {
            var r = CallSystemCommand(args, "route", ComponentPath.RouteExePath);
            if(!string.IsNullOrEmpty(r)) Logger.Debug($"[route]{r}");
        }

        public static NetworkInterface GetBestInterface(v4addr addr) {
            var ifs = (from adapter in NetworkInterface.GetAllNetworkInterfaces()
                where adapter.OperationalStatus == OperationalStatus.Up && adapter.Supports(NetworkInterfaceComponent.IPv4)
                select adapter).ToList();

            NetworkInterface candidate = null;
            
            if (ifs.Count == 1) {
                candidate = ifs.First();
            } else if (ifs.Count > 1) {
                // more than one running network interface
                string tr = CallTracert($" -d -h 1 {addr}");
                var matches = Regex.Matches(tr, @"(\d{1,3}\.){3}\d{1,3}");
                if (matches.Count == 2) {
                    var targetGateway = new v4addr(matches[1].Value);
                    Logger.Debug($"Searching for gateway {targetGateway}");
                    uint targetInterfaceIndex = (from row in IPNetHelper.GetCurrentForwardTable()
                        where row.dwForwardNextHop == targetGateway.ToUInt32()
                        select row.dwForwardIfIndex).FirstOrDefault();
                    candidate =
                        ifs.FirstOrDefault(a => a.GetIPProperties().GetIPv4Properties().Index == targetInterfaceIndex);
                    if (candidate != null) {
                        Logger.Debug($"find interface({targetInterfaceIndex}) {candidate.Name}");
                    }
                } else {
                    Logger.Error("Trace route error.");
                    Logger.Debug(tr);
                }
            }
            return candidate;
        }

        public static Dictionary<long, InterfaceInformation> ListAllInterfaces() {
            if (Os.Ver == Os.V.XpOrLower) throw new NotSupportedException("Windows Version Too Low");

            var transId = new Dictionary<string, uint>();
            long accu = -1;
            var result = new Dictionary<long, InterfaceInformation>();
            if (Os.Ver == Os.V.VistaOrSeven) {
                ManagementClass nic = new ManagementClass(@"\\.\ROOT\cimv2:Win32_NetworkAdapter");
                foreach (var a in nic.GetInstances()) {
                    var key = (uint) a["InterfaceIndex"];
                    var value = new InterfaceInformation {
                        WMI_Win32_NetworkAdapter_Exist = true,
                        WMI_WNA_AdapterType = a["AdapterType"] as string,
                        WMI_WNA_Availability = Convert.ToUInt16(a["Availability"]),
                        WMI_WNA_Caption = a["Caption"] as string,
                        WMI_WNA_Description = a["Description"] as string,
                        WMI_WNA_DeviceID = a["DeviceID"] as string,
                        WMI_WNA_GUID = a["GUID"] as string,
                        WMI_WNA_Index = Convert.ToUInt32(a["Index"]),
                        WMI_WNA_InterfaceIndex = Convert.ToUInt32(a["InterfaceIndex"]),
                        WMI_WNA_Name = a["Name"] as string,
                        WMI_WNA_NetConnectionID = a["NetConnectionID"] as string,
                        WMI_WNA_NetConnectionStatus = Convert.ToUInt16(a["NetConnectionStatus"]),
                        WMI_WNA_PhysicalAdapter = Convert.ToBoolean(a["PhysicalAdapter"]),
                        WMI_WNA_Status = a["Status"] as string
                    };
                    result.Add(key, value);
                    if (!string.IsNullOrEmpty(value.WMI_WNA_GUID)) transId.Add(value.WMI_WNA_GUID, key);
                }
            } else {
                // win8 or higher
                ManagementClass nic = new ManagementClass(@"\\.\ROOT\StandardCimv2:MSFT_NetAdapter");
                foreach (var a in nic.GetInstances()) {
                    var key = (uint)a["InterfaceIndex"];
                    var value = new InterfaceInformation {
                        WMI_MSFT_NetAdapter_Exist = true,
                        WMI_MSFT_NA_Availability = Convert.ToUInt16(a["Availability"]),
                        WMI_MSFT_NA_Caption = a["Caption"] as string,
                        WMI_MSFT_NA_ConnectorPresent = Convert.ToBoolean(a["ConnectorPresent"]),
                        WMI_MSFT_NA_Description = a["Description"] as string,
                        WMI_MSFT_NA_DeviceID = a["DeviceID"] as string,
                        WMI_MSFT_NA_DeviceName = a["DeviceName"] as string,
                        WMI_MSFT_NA_HardwareInterface = Convert.ToBoolean(a["HardwareInterface"]),
                        WMI_MSFT_NA_InterfaceDescription = a["InterfaceDescription"] as string,
                        WMI_MSFT_NA_InterfaceGuid = a["InterfaceGuid"] as string,
                        WMI_MSFT_NA_InterfaceIndex = Convert.ToUInt32(a["InterfaceIndex"]),
                        WMI_MSFT_NA_InterfaceName = a["InterfaceName"] as string,
                        WMI_MSFT_NA_MediaConnectState = Convert.ToUInt32(a["MediaConnectState"]),
                        WMI_MSFT_NA_MtuSize = Convert.ToUInt32(a["MtuSize"]),
                        WMI_MSFT_NA_Name = a["Name"] as string,
                        WMI_MSFT_NA_NetLuid = Convert.ToUInt64(a["NetLuid"]),
                        WMI_MSFT_NA_PNPDeviceID = a["PNPDeviceID"] as string,
                        WMI_MSFT_NA_Status = a["Status"] as string,
                        WMI_MSFT_NA_Virtual = Convert.ToBoolean(a["Virtual"])
                    };

                    result.Add(key, value);
                    if (!string.IsNullOrEmpty(value.WMI_MSFT_NA_DeviceID)) transId.Add(value.WMI_MSFT_NA_DeviceID, key);
                }
            }

            foreach (var row in IPNetHelper.GetInterfaceTable()) {
                if (!result.ContainsKey(row.InterfaceIndex)) {
                    result.Add(row.InterfaceIndex, new InterfaceInformation());
                }
                result[row.InterfaceIndex].IPHLPAPI_IFINTERFACE_ROW_Exist = true;
                result[row.InterfaceIndex].IP_IFR_InterfaceLuid = row.InterfaceLuid;
                result[row.InterfaceIndex].IP_IFR_InterfaceIndex = row.InterfaceIndex;
                result[row.InterfaceIndex].IP_IFR_UseAutomaticMetric = row.UseAutomaticMetric == 1;
                result[row.InterfaceIndex].IP_IFR_Metric = row.Metric;
                result[row.InterfaceIndex].IP_IFR_NlMtu = row.NlMtu;
                result[row.InterfaceIndex].IP_IFR_Connected = row.Connected == 1;
                result[row.InterfaceIndex].IP_IFR_DisableDefaultRoutes = row.DisableDefaultRoutes == 1;
            }

            ManagementClass mc = new ManagementClass(@"\\.\ROOT\cimv2:Win32_NetworkAdapterConfiguration");
            foreach (var a in mc.GetInstances()) {
                var key = (uint) a["InterfaceIndex"];
                if (!result.ContainsKey(key)) {
                    result.Add(key, new InterfaceInformation());
                }
                result[key].WMI_Win32_NetworkAdapterConfiguration_Exist = true;
                result[key].WMI_NAC_Caption = a["Caption"] as string;
                result[key].WMI_NAC_Description = a["Description"] as string;
                result[key].WMI_NAC_DefaultIPGateway = a["DefaultIPGateway"] == null
                    ? ""
                    : string.Join(", ", (string[]) a["DefaultIPGateway"]);
                result[key].WMI_NAC_Index = Convert.ToUInt32(a["Index"]);
                result[key].WMI_NAC_InterfaceIndex = Convert.ToUInt32(a["InterfaceIndex"]);
                result[key].WMI_NAC_DNSServerSearchOrder = a["DNSServerSearchOrder"] == null
                    ? ""
                    : string.Join(", ", (string[]) a["DNSServerSearchOrder"]);
                result[key].WMI_NAC_DHCPEnabled = Convert.ToBoolean(a["DHCPEnabled"]);
                result[key].WMI_NAC_IPEnabled = Convert.ToBoolean(a["IPEnabled"]);
                result[key].WMI_NAC_IPConnectionMetric = Convert.ToUInt32(a["IPConnectionMetric"]);
                result[key].WMI_NAC_IPAddress = a["IPAddress"] == null
                    ? ""
                    : string.Join(", ", (string[]) a["IPAddress"]);
                result[key].WMI_NAC_MTU = Convert.ToUInt32(a["MTU"]);
                result[key].WMI_NAC_MACAddress = a["MACAddress"] as string;
            }

            foreach (var adaptor in NetworkInterface.GetAllNetworkInterfaces()) {
                long key;
                if (transId.ContainsKey(adaptor.Id)) {
                    key = transId[adaptor.Id];
                } else {
                    if (adaptor.Supports(NetworkInterfaceComponent.IPv4) && result.ContainsKey(adaptor.GetIPProperties().GetIPv4Properties().Index)) {
                        key = adaptor.GetIPProperties().GetIPv4Properties().Index;
                    } else {
                        key = accu;
                        accu -= 1;
                        result.Add(key, new InterfaceInformation());
                    }
                }
                result[key].Net_NetworkInterface_Exist = true;
                result[key].Net_ID = adaptor.Id;
                result[key].Net_Description = adaptor.Description;
                result[key].Net_Name = adaptor.Name;
                result[key].Net_OperationalStatus = adaptor.OperationalStatus.ToString();
                result[key].Net_SupportIPv4 = adaptor.Supports(NetworkInterfaceComponent.IPv4);
                result[key].Net_NetworkInterfaceType = adaptor.NetworkInterfaceType.ToString();
                if (adaptor.Supports(NetworkInterfaceComponent.IPv4)) {
                    result[key].Net_IPv4Index = adaptor.GetIPProperties().GetIPv4Properties().Index;
                    result[key].Net_IPv4MTU = adaptor.GetIPProperties().GetIPv4Properties().Mtu;
                }
                result[key].Net_DnsAddresses = string.Join(", ", adaptor.GetIPProperties().DnsAddresses);
                result[key].Net_GatewayAddresses = string.Join(", ",
                    adaptor.GetIPProperties().GatewayAddresses.ToList().ConvertAll(gi => gi.Address.ToString()));
            }

            return result;
        }

        public static string ListAllRoutes() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SECTION: ROUTE PRINT");
            sb.AppendLine(CallSystemCommand("PRINT -4", "route", ComponentPath.RouteExePath));
            sb.AppendLine("\nSECTION: IPHLPAPI");
            sb.AppendLine("\tDEST             NEXT             MASK             INTF METRIC");
            foreach (var row in IPNetHelper.GetCurrentForwardTable()) {
                sb.Append($"\t{new IPAddress(row.dwForwardDest).ToString().PadRight(17)}");
                sb.Append(new IPAddress(row.dwForwardNextHop).ToString().PadRight(17));
                sb.Append(new IPAddress(row.dwForwardMask).ToString().PadRight(17));
                sb.Append(row.dwForwardIfIndex.ToString().PadRight(5));
                sb.AppendLine(row.dwForwardMetric1.ToString());
            }
            return sb.ToString();
        }

        public static string ListAllProcess() {
            StringBuilder sb = new StringBuilder();
            foreach (var process in Process.GetProcesses()) {
                sb.AppendLine($"{process.Id.ToString().PadRight(5)}\t{process.ProcessName}\t{process.MainWindowTitle}");
            }
            return sb.ToString();
        }

        public static string PingIt(IPAddress address) {
            Ping p = new Ping();
            byte[] buffer = new byte[32];
            PingOptions po = new PingOptions {
                DontFragment = true
            };
            try {
                var reply = p.Send(address, 10000, buffer, po);
                if (reply == null) {
                    throw new NullReferenceException("reply is null");
                }
                return reply.Status == IPStatus.Success
                    ? $"Reply from {reply.Address}: bytes={reply.Buffer.Length} time={reply.RoundtripTime} TTL={reply.Options.Ttl}"
                    : $"Ping failed: {reply.Status}";
            } catch (PingException e) {
                var extra = string.Join(", ", (from DictionaryEntry kv in e.Data select $"{kv.Key}:{kv.Value}").ToList());
                return $"PingException: {e.Message}, Inner={e.InnerException}, Extra={extra}";
            } catch (Exception e) {
                return $"{e.GetType()}: {e.Message}";
            }
        }

        public static string FileSha1Hash(string filepath) {
            if (!File.Exists(filepath)) {
                return File.GetAttributes(filepath).HasFlag(FileAttributes.Directory) ? "Directory" : "???";
            }
            
            try {
                var result = new SHA1CryptoServiceProvider().ComputeHash(File.ReadAllBytes(filepath));
                var sb = new StringBuilder();
                foreach (var t in result) {
                    sb.Append(t.ToString("x2"));
                }
                return sb.ToString();
            } catch (Exception e) {
                Logger.Error($"FileSHA1Hash: {e.Message}");
                Logger.Debug(e.StackTrace);
                return "ERROR";
            }

        }

        public static string CallTracert(string args) {
            return CallSystemCommand(args, "tracert", ComponentPath.TracertPath);
        }

        private static string CallSystemCommand(string args, string name, string path) {
            StringBuilder sb = new StringBuilder();
            Logger.Debug($"[{name}] {args}");
            var psi = new ProcessStartInfo {
                FileName = path,
                Arguments = args,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            Process ps = new Process {
                StartInfo = psi,
                EnableRaisingEvents = true
            };
            ps.OutputDataReceived += (sender, e) => {
                try {
                    if(string.IsNullOrEmpty(e.Data)) return;
                    sb.AppendLine(e.Data);
                } catch (NullReferenceException) {
                    //suppress
                } catch (Exception ex) {
                    Logger.Error($"[{name}-err]{ex.Message}");
                }
            };
            ps.ErrorDataReceived += (sender, e) => {
                try {
                    if(string.IsNullOrEmpty(e.Data)) return;
                    Logger.Error($"[{name}]{e.Data}");
                } catch (NullReferenceException) {
                    //suppress
                } catch (Exception ex) {
                    Logger.Error($"[{name}-err]{ex.Message}");
                }
            };
            ps.Start();
            ps.BeginErrorReadLine();
            ps.BeginOutputReadLine();
            ps.WaitForExit();
            ps.Close();
            return sb.ToString();
        }
    }
}
