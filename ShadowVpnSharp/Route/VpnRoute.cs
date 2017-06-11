using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ShadowVpnSharp.Configuration;
using ShadowVpnSharp.Device;
using ShadowVpnSharp.Helper;

namespace ShadowVpnSharp.Route {
    class VpnRoute {
        private readonly NetIntf _origin;
        private readonly v4addr _originGateway;
        private bool _chnRouteOn;
        public bool IsUp { get; private set; }
        public NetIntf Tunnel { get; }


        public VpnRoute() {
            Tunnel = new NetIntf(TapDeviceFunc.GetTapDevice(Program.UserConfig.TapIntfaceName));
            var intf = Diagnostic.GetBestInterface(Program.UserConfig.CurrentProfile.ServerIp);
            _origin = intf == null ? new NetIntf(IPNetHelper.GetBestIntf(Program.UserConfig.CurrentProfile.ServerIp.ToUInt32())) : new NetIntf(intf);
            _chnRouteOn = false;
            _originGateway = GetGateway();
        }

        public void Up() {
            // route add %server% %orig_gw% metric 5 > NUL
            _origin.AddRouteEntryUsingRouteExe(Program.UserConfig.CurrentProfile.ServerIp, v4addr.GetMaskAddr(32), _originGateway, 5);
            // netsh interface ip set interface %orig_intf% ignoredefaultroutes=enabled > NUL
            _origin.SetIgnoreDefaultRoute(true);

            // netsh interface ip set address name="%intf%" static %tunip% 255.255.255.0 > NUL
            Tunnel.SetStaticIP(Program.UserConfig.CurrentProfile.Ip, v4addr.GetMaskAddr(24));
            // netsh interface ipv4 set subinterface "%intf%" mtu=%mtu% > NUL
            Tunnel.SetMtu(Program.UserConfig.CurrentProfile.Mtu);
            // netsh interface ip set dns name="%intf%" static %dns_server% > NUL
            Tunnel.SetDns(Program.UserConfig.CurrentProfile.Dns);

            if (Program.UserConfig.UseChnRoute) {
                Logger.Info("Using CHNRoute");
                _chnRouteOn = false;
                EnableChnroute();
                _chnRouteOn = true;
            } else {
                Logger.Info("Using Global Routing");
            }

            // setting origin interface metric should after adding chnroute
            _origin.DeleteRouteEntryUsingRouteExe(new v4addr("0.0.0.0"), v4addr.GetMaskAddr(0), _originGateway);
            _origin.SetMetric(200);

            // netsh interface ipv4 add route 128.0.0.0/1 "%intf%" %remote_tun_ip% metric=6 > NUL
            Tunnel.AddRouteEntryUsingRouteExe(new v4addr("128.0.0.0"), v4addr.GetMaskAddr(1), Program.UserConfig.CurrentProfile.RemoteTunIp, 6);
            //Tunnel.AddPersistentRoute(new v4addr("128.0.0.0"), v4addr.GetMaskAddr(1), Program.UserConfig.CurrentProfile.RemoteTunIp, 6);
            // netsh interface ipv4 add route 0.0.0.0/1 "%intf%" %remote_tun_ip% metric=6 > NUL
            Tunnel.AddRouteEntryUsingRouteExe(new v4addr("0.0.0.0"), v4addr.GetMaskAddr(1), Program.UserConfig.CurrentProfile.RemoteTunIp, 6);
            //Tunnel.AddPersistentRoute(new v4addr("0.0.0.0"), v4addr.GetMaskAddr(1), Program.UserConfig.CurrentProfile.RemoteTunIp, 6);
            //            _tunnel.SetGateway(new [] {_config.Addresses["remote_tun_ip"] });
            Logger.Info("Vpn Route Up");
            IsUp = true;
        }


        public void Down() {
            // netsh interface ip set interface %orig_intf% ignoredefaultroutes=disabled > NUL
            _origin.SetIgnoreDefaultRoute(false);

            // restore
            _origin.RestoreMetric();
            _origin.AddRouteEntryUsingRouteExe(new v4addr("0.0.0.0"), v4addr.GetMaskAddr(0), _originGateway, 1);


            // netsh interface ip set address name="%intf%" dhcp > NUL
            Tunnel.EnableDhcp();
            // route delete 0.0.0.0 mask 128.0.0.0 %remote_tun_ip% > NUL
            Tunnel.DeleteRouteEntryUsingRouteExe(new v4addr("0.0.0.0"), v4addr.GetMaskAddr(1), Program.UserConfig.CurrentProfile.RemoteTunIp);
            // route delete 128.0.0.0 mask 128.0.0.0 %remote_tun_ip% > NUL
            Tunnel.DeleteRouteEntryUsingRouteExe(new v4addr("128.0.0.0"), v4addr.GetMaskAddr(1), Program.UserConfig.CurrentProfile.RemoteTunIp);
            //_tunnel.SetGateway(new v4addr[] {});
            // route delete %server% > NUL
            _origin.DeleteRouteEntryUsingRouteExe(Program.UserConfig.CurrentProfile.ServerIp, v4addr.GetMaskAddr(32), _originGateway);

            if (Program.UserConfig.UseChnRoute && _chnRouteOn) {
                DisableChnroute();
                _chnRouteOn = false;
            } 
            
            // netsh interface ip set dns name="%intf%" source=dhcp > NUL
            Tunnel.SetDns(new v4addr[] {});
            Logger.Info("Vpn Route Down");
            IsUp = false;
        }

        private void EnableChnroute() {
            foreach (string line in File.ReadAllLines(ComponentPath.AddRouteScriptPath)) {
                var args = line.Split();
                _origin.AddRouteEntry(new v4addr(args[1]), new v4addr(args[3]), _originGateway);
            }
        }

        private void DisableChnroute() {
            foreach (string line in File.ReadAllLines(ComponentPath.AddRouteScriptPath)) {
                var args = line.Split();
                _origin.DeleteRouteEntry(new v4addr(args[1]), new v4addr(args[3]), _originGateway);
            }
        }

        private v4addr GetGateway() {
            var gate = GetGatewayFromRouteTable();
            if (gate != null) {
                Logger.Debug($"Find gateway: {gate} using route table");
                return gate;
            }

            gate = GetGatewayFromTracert();
            if (gate != null) {
                Logger.Debug($"Find gateway: {gate} using traceroute");
                return gate;
            }

            throw new Exception("Cannot find gateway anyway.");
        }

        private v4addr GetGatewayFromRouteTable() {
            return (from row in IPNetHelper.GetCurrentForwardTable()
                    where row.dwForwardDest == 0 
                    select new v4addr(row.dwForwardNextHop)).FirstOrDefault();
        }

        private v4addr GetGatewayFromTracert() {
            // more than one running network interface
            var matches = Regex.Matches(Diagnostic.CallTracert(" -d -h 1 114.114.114.114"), @"(\d{1,3}\.){3}\d{1,3}");
            if (matches.Count != 2) return null;
            var targetGateway = new v4addr(matches[1].Value);
            return targetGateway;
        }

    }
}
