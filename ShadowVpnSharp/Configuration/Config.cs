using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ShadowVpnSharp.Device;
using ShadowVpnSharp.Helper;
using fastJSON;

namespace ShadowVpnSharp.Configuration {
    public class Config {
        public bool UseChnRoute;
        public string TapIntfaceName;
        public bool AutoChooseFastestServer = false;
        public bool AutoUpdate = true;

        public Profile CurrentProfile => string.IsNullOrEmpty(CurrentProfileName) ? null : Profiles[CurrentProfileName];

        public string CurrentProfileName { get; private set; }

        public Dictionary<string, Profile> Profiles { get; private set; }

        private Config(ConfigJsonObject o) {
            UseChnRoute = false; //TODO: ADD CHNRoute Support
            TapIntfaceName = o.TapIntfaceName;
            CurrentProfileName = o.CurrentProfileName;
            AutoUpdate = o.AutoUpdate;
            AutoChooseFastestServer = o.AutoChooseFastestServer;
            Profiles = new Dictionary<string, Profile>();
            if(o.Profiles != null) {
                foreach (var profileJsonObject in o.Profiles) {
                    Profiles.Add(profileJsonObject.Key, new Profile(profileJsonObject.Value));
                }
            }

            SaveVpnConfig();
        }

        public void SetCurrentProfile(string name) {
            if (!string.IsNullOrWhiteSpace(name) && Profiles.ContainsKey(name)) CurrentProfileName = name;
            SaveToFile();
            SaveVpnConfig();
        }

        public bool SelfCheck() {
            if (string.IsNullOrEmpty(TapIntfaceName)) return false;
            if (Profiles == null) {
                Profiles = new Dictionary<string, Profile>();
                CurrentProfileName = "";
            } else if (Profiles.Count == 0) {
                if (!string.IsNullOrEmpty(CurrentProfileName)) return false;
            } else {
                if(!Profiles.ContainsKey(CurrentProfileName)) return false;
            }
            if (TapDeviceFunc.GetTapDevice(TapIntfaceName) == null) {
                Logger.Error("vpnConfig.intf not found in system.");
                var adapter = TapDeviceFunc.GetTapDevice();
                if (adapter != null) {
                    if (Regex.IsMatch(adapter.Name, @"^[a-zA-Z0-9]+$")) {
                        Logger.Info($"Changing vpnConfig.intf to {adapter.Name}");
                        TapIntfaceName = adapter.Name;
                    }
                } else {
                    return false;
                }
            }
            return Profiles.Values.All(profile => profile.SelfCheck());
        }

        public void AddProfile(string name, Profile p) {
            if(string.IsNullOrEmpty(name)) throw new Exception("配置名不能为空");
            if (Profiles.ContainsKey(name)) {
                Profiles.Add(GenerateNewName(name), p);
            } else {
                Profiles.Add(name, p);
            }
            
        }

        private string GenerateNewName(string oldname) {
            int i = 1;
            string name = $"{oldname} {i}";
            while (Profiles.ContainsKey(name)) {
                i += 1;
                name = $"{oldname} {i}";
            }
            return name;
        }

        public void RemoveProfile(string name) {
            if (!string.IsNullOrWhiteSpace(name) && Profiles.ContainsKey(name)) Profiles.Remove(name);
        }

        private void SaveVpnConfig() {
            if (CurrentProfile == null) return;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"server={CurrentProfile.ServerIp}");
            sb.AppendLine($"port={CurrentProfile.Port}");
            sb.AppendLine($"password={CurrentProfile.Password}");
            sb.AppendLine("mode=client");
            sb.AppendLine($"tunip={CurrentProfile.Ip}");
            sb.AppendLine($"mtu={CurrentProfile.Mtu}");
            sb.AppendLine($"intf={TapIntfaceName}");
            if (CurrentProfile.HasUserToken) {
                sb.AppendLine($"user_token={CurrentProfile.UserToken}");
            }
            File.WriteAllText(ComponentPath.VpnConfigPath, sb.ToString());
        }

        private ConfigJsonObject ConvertToConfigJsonObject() {
            var o = new ConfigJsonObject {
                UseChnRoute = UseChnRoute,
                TapIntfaceName = TapIntfaceName,
                CurrentProfileName = CurrentProfileName,
                AutoChooseFastestServer = AutoChooseFastestServer,
                AutoUpdate = AutoUpdate,
                Profiles = new Dictionary<string, Profile.ProfileJsonObject>()
            };
            foreach (var profile in Profiles) {
                o.Profiles.Add(profile.Key, profile.Value.ConverToJsonObject());
            }
            return o;
        }

        public void SaveToFile() {
            var towrite = JSON.ToNiceJSON(ConvertToConfigJsonObject());
            File.WriteAllText(ComponentPath.UserConfigPath, towrite);
        }

        public static Config FromFile() {
            if (File.Exists(ComponentPath.UserConfigPath)) {
                var json = File.ReadAllText(ComponentPath.UserConfigPath);
                var o = JSON.ToObject<ConfigJsonObject>(json);
                return new Config(o);
            } else {
                if (!File.Exists(ComponentPath.UserConfigPath) && !Directory.Exists(Path.GetDirectoryName(ComponentPath.UserConfigPath))) {
                    Directory.CreateDirectory(Path.GetDirectoryName(ComponentPath.UserConfigPath));
                }
                var o = new ConfigJsonObject {TapIntfaceName = "svTun"};
                return new Config(o);
            }

        }

        public class ConfigJsonObject {
            public bool UseChnRoute = false;
            public string TapIntfaceName = null;
            public Dictionary<string, Profile.ProfileJsonObject> Profiles;
            public string CurrentProfileName;
            public bool AutoChooseFastestServer = false;
            public bool AutoUpdate = true;
        }


    }
}
