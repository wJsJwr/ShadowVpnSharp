using System;

namespace ShadowVpnSharp.Helper {
    internal static class Os {
        public enum V {
            EightOrHigher,
            VistaOrSeven,
            XpOrLower
        }

        public static V Ver { get; private set; }

        public static void Init() {
            var ver = Environment.OSVersion;
            Logger.Debug(ver.ToString());
            if (ver.Version.Major >= 10) {
                Ver = V.EightOrHigher;
            } else if (ver.Version.Major == 6) {
                Ver = ver.Version.Minor >= 2 ? V.EightOrHigher : V.VistaOrSeven;
            } else {
                Ver = V.XpOrLower;
                Logger.Error("OS version not supported");
            }
        }
    }
}
