using System;
using System.IO;
using System.Windows.Forms;

namespace ShadowVpnSharp.Helper {
    static class ComponentPath {
        public static string RootPath => Directory.GetParent(Application.ExecutablePath).ToString();

        private static string AbsolutePath(string rPath) {
            return Path.Combine(new[] {RootPath, rPath});
        }

        public static string TapInstallerPath => AbsolutePath(@"vendor\tap\tap-windows-9.21.2.exe");

        public static string VpnConfigPath => AbsolutePath(@"config\client.user.conf");

        public static string VpnBinaryPath => AbsolutePath(@"lib\shadowvpn.exe");

        public static string UserConfigPath => AbsolutePath(@"config\user.conf.json");

        public static string LogFilePath => AbsolutePath(@"log\svgui.log");

        public static string AddRouteScriptPath => AbsolutePath(@"vendor\shadowvpn-win-master\addchnroutes.txt");

        public static string TapWindowsPath { get; set; }

        public static string SetupApiLogPath => Path.Combine(new[]
            {Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"inf\setupapi.dev.log"});

        public static string LocalSaLogPath => AbsolutePath(@"log\salog.log");

        public static string RouteExePath => Path.Combine(new[]
            {Environment.GetFolderPath(Environment.SpecialFolder.System), @"route.exe"});

        public static string TracertPath => Path.Combine(new[]
            {Environment.GetFolderPath(Environment.SpecialFolder.System), @"tracert.exe"});
    }
}