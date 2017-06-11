using System;
using System.IO;
using System.Text;

namespace ShadowVpnSharp.Helper {
    static class Logger {
        public static void Init() {
            if (!File.Exists(ComponentPath.LogFilePath) && !Directory.Exists(Path.GetDirectoryName(ComponentPath.LogFilePath))) {
                Directory.CreateDirectory(Path.GetDirectoryName(ComponentPath.LogFilePath));
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
                File.AppendAllText(ComponentPath.LogFilePath, sb.ToString());
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