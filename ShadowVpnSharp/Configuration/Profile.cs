using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ShadowVpnSharp.Helper;
using fastJSON;
using ShadowVpnSharp.View;

namespace ShadowVpnSharp.Configuration {
    public class Profile {
        /**
         * shadowvpn://BASE64_DATA
            BASE64_DATA = base64({
                "server": "server_ip",
                "port": "1xxx",
                "usertoken": "hex",
                "ip": "10.7.0.2",
                "subnet": "255.255.0.0",
                "dns": "8.8.8.8"
            })
        */

        private const string SchemaRegex = "^(shadowvpn://)?(?<body>[a-zA-Z0-9+/\\-_=]+)(#(?<title>.+))?$";

        public string Server = null;
        public int Port = 0;
        public string UserToken = null;
        public v4addr Ip = null;
        public v4addr Subnet = null;
        public v4addr Dns = null;
        public string Password = null;
        public uint Mtu = 0;
        public v4addr RemoteTunIp = null;
        public v4addr ServerIp = null;

        public Profile(ProfileJsonObject o) {
            Server = o.server;
            Port = o.port;
            UserToken = o.usertoken;
            Ip = new v4addr(o.ip);
            Subnet = new v4addr(o.subnet);
            Dns = new v4addr(o.dns);
            Password = o.password;
            Mtu = o.mtu;
            RemoteTunIp = new v4addr(o.remote_tun_ip);
            ServerIp = new v4addr(o.server);
        }

        public ProfileJsonObject ConverToJsonObject() {
            return new ProfileJsonObject {
                server =  Server,
                port =  Port,
                usertoken = UserToken,
                ip = Ip,
                subnet = Subnet,
                dns = Dns,
                password = Password,
                mtu = Mtu,
                remote_tun_ip = RemoteTunIp
            };
        }

        public bool HasUserToken => !string.IsNullOrEmpty(UserToken);

        public bool SelfCheck() {
            if (Server == null) return false;
            if (Port <= 0 || Port > 65535) return false;
            if (!string.IsNullOrEmpty(UserToken)) if(!Regex.IsMatch(UserToken, @"^[a-z0-9]{16}$")) return false;
            if (string.IsNullOrEmpty(Password)) return false;
            if (Mtu <= 0) return false;
            return Ip != null && Subnet != null && Dns != null && RemoteTunIp != null;
        }

        public Profile Clone() {
            return new Profile(this.ConverToJsonObject());
        }

        public string ToSchemaString(string title) {
            StringBuilder sb = new StringBuilder();
            sb.Append("shadowvpn://");
            sb.Append(Convert.ToBase64String(Encoding.UTF8.GetBytes(JSON.ToJSON(this.ConverToJsonObject()))));
            if (!string.IsNullOrEmpty(title)) {
                sb.Append($"#{title}");
            }
            return sb.ToString();
        }

        public static Profile FromBase64(string b64) {
            try {
                return
                    new Profile(JSON.ToObject<ProfileJsonObject>(Encoding.UTF8.GetString(Convert.FromBase64String(_removeProtocol(b64)))));
            } catch (Exception e) {
                Logger.Debug(e.Message);
                Logger.Debug(e.StackTrace);
                throw;
            }
        }

        public static bool ValidateString(string b64) {
            try {
                FromBase64(b64);
                return true;
            } catch (Exception) {
                return false;
            }
        }

        public static string GetTitleFromString(string s) {
            var match = Regex.Match(s, SchemaRegex);
            var group = match.Groups;
            return group["title"].Success ? group["title"].Value : null;
        }

        public static string GenerateTitle() {
            return $"VpnProfile#{(ulong)DateTime.Now.ToBinary()}";
        }

        private static string _removeProtocol(string s) {

            var match = Regex.Match(s, SchemaRegex, RegexOptions.Multiline);
            var group = match.Groups;
            Console.WriteLine(group["body"]);
            return group["body"].Value;
        }

        public static bool CheckUriFormat(string s) {
            return Regex.IsMatch(s.ToLower(), SchemaRegex);
        }

        public class ProfileJsonObject {
            public string server = null;
            public int port = 0;
            public string usertoken = null;
            public string ip = null;
            public string subnet = null;
            public string dns = null;
            public string password = null;
            public uint mtu = 0;
            public string remote_tun_ip = null;
        }
    }
}
