using System;
using System.Linq;
using System.Management;
using System.Management.Instrumentation;
using System.Net.NetworkInformation;
using ShadowVpnSharp.Helper;

namespace ShadowVpnSharp.Device {
    class NetIntf {
        private uint _defaultInterfaceMetric;

        public uint IntfIndex { get; }

        public NetIntf(uint index) {
            IntfIndex = index;
            var nic = GetNic(index);
            if (nic == null) {
                throw new Exception($"NetworkInterface Not Found with Index({index})");
            }
            GetMetric();
        }

        private static NetworkInterface GetNic(uint index) {
            return
                NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(
                        adapter =>
                            adapter.Supports(NetworkInterfaceComponent.IPv4) &&
                            adapter.GetIPProperties().GetIPv4Properties().Index == index);
        }

        public string GetGuid() {
            var obj = GetNetworkAdapter();
            if (obj == null) throw new InstanceNotFoundException();
            if (Os.Ver == Os.V.VistaOrSeven) {
                return obj["GUID"] as string;
            } else {
                // win8 or higher
                return obj["InterfaceGuid"] as string;
            }
        }

        private void GetMetric() {
            var r = IPNetHelper.GetInterface(IntfIndex);
            _defaultInterfaceMetric = r.Metric;
            Logger.Debug($"Get Metric on Interface({IntfIndex}): Metric={_defaultInterfaceMetric}");
        }

        public NetIntf(NetworkInterface ni) {
            var nic = ni;
            if(!ni.Supports(NetworkInterfaceComponent.IPv4)) throw new Exception("Specified interface does not support IPv4");
            IntfIndex = (uint) ni.GetIPProperties().GetIPv4Properties().Index;
            Logger.Debug($"Get InterfaceIndex: {IntfIndex}");
            GetMetric();
        }


        internal void SetDns(v4addr[] dns) {
            var target = GetManagementObject();
            if (target != null) {
                ManagementBaseObject param = target.GetMethodParameters("SetDNSServerSearchOrder");
                string[] dnsstr = new string[dns.Length];
                for (int i = 0; i < dns.Length; i++) dnsstr[i] = dns[i].ToString();
                param["DNSServerSearchOrder"] = dnsstr;
                target.InvokeMethod("SetDNSServerSearchOrder", param, null);
            }
        }

        public void AddRouteEntry(v4addr dest, v4addr mask, v4addr next, uint metric = 0) {
            IPNetHelper.CreateIPForwardEntry(dest.ToUInt32(), mask.ToUInt32(), next.ToUInt32(), IntfIndex,
                (int) (metric == 0 ? _defaultInterfaceMetric : _defaultInterfaceMetric + metric));
        }

        public void DeleteRouteEntry(v4addr dest, v4addr mask, v4addr next) {
            IPNetHelper.DeleteIPForwardEntry(dest.ToUInt32(), mask.ToUInt32(), next.ToUInt32(), IntfIndex);
        }

        public void AddRouteEntryUsingRouteExe(v4addr dest, v4addr mask, v4addr next, int metric) {
            Diagnostic.CallRouteExe($"ADD {dest} MASK {mask} {next} METRIC {metric} IF {IntfIndex}");
        }

        public void AddPersistentRoute(v4addr dest, v4addr mask, v4addr next, int metric) {
            Diagnostic.CallRouteExe($"-p ADD {dest} MASK {mask} {next} METRIC {metric} IF {IntfIndex}");
        }
        public void DeleteRouteEntryUsingRouteExe(v4addr dest, v4addr mask, v4addr next) {
            Diagnostic.CallRouteExe($"DELETE {dest} MASK {mask} {next}");
        }

        public void DeletePersistentRoute(v4addr dest, v4addr mask, v4addr next) {
            Diagnostic.CallRouteExe($"-p DELETE {dest} MASK {mask} {next}");
        }

        public void SetStaticIP(v4addr addr, v4addr mask) {
            var target = GetManagementObject();
            if (target != null) {
                ManagementBaseObject param = target.GetMethodParameters("EnableStatic");
                param["IPAddress"] = new[] {addr.ToString()};
                param["SubnetMask"] = new[] {mask.ToString()};
                target.InvokeMethod("EnableStatic", param, null);
            }
        }

        public void SetGateway(v4addr[] gws) {
            var target = GetManagementObject();
            if (target != null) {
                ManagementBaseObject param = target.GetMethodParameters("SetGateways");
                string[] gwsswr = new string[gws.Length];
                for (int i = 0; i < gws.Length; i++) gwsswr[i] = gws[i].ToString();
                param["DefaultIPGateway"] = gwsswr;
                target.InvokeMethod("SetGateways", param, null);
            }
        }

        public void EnableDhcp() {
            var target = GetManagementObject();
            target?.InvokeMethod("EnableDHCP", null, null);
        }

        public void SetIgnoreDefaultRoute(bool setting) {
            Logger.Debug(setting
                ? $"Ignoring Default Route on Interface({IntfIndex})"
                : $"Reverting Default Route on Interface({IntfIndex})");
            var r = IPNetHelper.InitInterface(IntfIndex);
            r.DisableDefaultRoutes = (byte)(setting ? 1 : 0);
            IPNetHelper.SetInterface(r);
        }

        public void SetMtu(uint mtu) {
            Logger.Debug($"Setting MTU on Interface({IntfIndex})");
            var r = IPNetHelper.InitInterface(IntfIndex);
            r.NlMtu = mtu;
            IPNetHelper.SetInterface(r);
        }

        public void SetMetric(uint metric) {
            Logger.Debug($"Setting Metric on Interface({IntfIndex})");
            var r = IPNetHelper.InitInterface(IntfIndex);
            r.Metric = metric;
            r.UseAutomaticMetric = 0;
            IPNetHelper.SetInterface(r);
        }

        public void RestoreMetric() {
            Logger.Debug($"Restoring Metric on Interface({IntfIndex})");
            var r = IPNetHelper.InitInterface(IntfIndex);
            r.Metric = _defaultInterfaceMetric;
            r.UseAutomaticMetric = 1;
            IPNetHelper.SetInterface(r);
        }

        public void SetDns(v4addr dns) {
            var target = GetManagementObject();
            if (target != null) {
                ManagementBaseObject param = target.GetMethodParameters("SetDNSServerSearchOrder");
                param["DNSServerSearchOrder"] = new[] {dns.ToString()};
                target.InvokeMethod("SetDNSServerSearchOrder", param, null);
            }
        }

        private ManagementObject GetManagementObject() {
            ManagementClass nic = new ManagementClass(@"\\.\ROOT\cimv2:Win32_NetworkAdapterConfiguration");
            ManagementObject target = null;
            foreach (var a in nic.GetInstances()) {
                if ((uint) a["InterfaceIndex"] == IntfIndex) {
                    target = a as ManagementObject;
                    break;
                }
            }
            if (target == null) {
                Logger.Error($"Win32_NetworkAdapterConfiguration Not Found with index={IntfIndex}");
            }
            return target;
        }

        private ManagementBaseObject GetNetworkAdapter() {
            if (Os.Ver == Os.V.XpOrLower) throw new NotSupportedException("Windows Version Too Low");
            if (Os.Ver == Os.V.VistaOrSeven) {
                ManagementClass nic = new ManagementClass(@"\\.\ROOT\cimv2:Win32_NetworkAdapter");
                foreach (var a in nic.GetInstances()) {
                    if ((uint)a["InterfaceIndex"] == IntfIndex) {
                        return a;
                    }
                }
            } else {
                // win8 or higher
                ManagementClass nic = new ManagementClass(@"\\.\ROOT\StandardCimv2:MSFT_NetAdapter");
                foreach (var a in nic.GetInstances()) {
                    if ((uint)a["InterfaceIndex"] == IntfIndex) {
                        return a;
                    }
                }
            }
            return null;
        }

        protected bool CheckIsWorkingHardware() {
            var obj = GetNetworkAdapter();
            if (obj == null) return false;
            bool result = false;
            if (Os.Ver == Os.V.VistaOrSeven) {
                Logger.Debug($"Found {obj["NetConnectionID"]}");
                result = (ushort)obj["NetConnectionStatus"] == 2 && (bool)obj["PhysicalAdapter"];
            } else {
                // win8 or higher
                Logger.Debug($"Found {obj["Name"]}");
                result = (uint)obj["MediaConnectState"] == 1 && (bool)obj["ConnectorPresent"] &&
                         !(bool)obj["Virtual"];
            }
            return result;
        }
    }
}