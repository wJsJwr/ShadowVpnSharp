using System;
using System.IO;
using System.Text;

namespace ShadowVpnSharp.Helper {
    static class Logger {
        private static bool _isUpdate;

        public static string Logfile { get; private set; }

        public static void Init(bool isUpdate = false) {
            if (isUpdate) {
                _isUpdate = isUpdate;
                Logfile = Path.GetTempFileName();
            } else {
                if (!File.Exists(ComponentPath.LogFilePath) && !Directory.Exists(Path.GetDirectoryName(ComponentPath.LogFilePath))) {
                    Directory.CreateDirectory(Path.GetDirectoryName(ComponentPath.LogFilePath));
                }
                Logfile = ComponentPath.LogFilePath;
            }

        }
        public static void Close() {}

        private static void LogWrite(string msg, string category) {
            if (string.IsNullOrEmpty(msg) || string.IsNullOrEmpty(category)) return;
            string prefix = $"[{DateTime.Now:yyyy/MM/dd_HH:mm:ss.ff}][{category}] ";
            string space = " ".PadLeft(prefix.Length);
            string[] lines = msg.Split("\r\n".ToCharArray());
            StringBuilder sb = new StringBuilder();
            sb.Append(prefix);
            sb.AppendLine(lines[0]);
            if (lines.Length > 1) {
                for (var i = 1; i < lines.Length; i++) {
                    if (string.IsNullOrEmpty(lines[i])) continue;
                    sb.AppendLine(space + lines[i]);
                }
            }
            try {
                File.AppendAllText(Logfile, sb.ToString());
            } catch {
                // drop it;
            }
        }

        public static void Info(string msg) {
            LogWrite(msg, "INFO");
        }

        public static void Error(string msg) {
            LogWrite(msg, "Error");
        }

        public static void Debug(string msg) {
            LogWrite(msg, "Debug");
        }
    }
}