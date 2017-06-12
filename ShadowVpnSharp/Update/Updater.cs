using System.IO.Compression;
using System.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using ShadowVpnSharp.Helper;

namespace ShadowVpnSharp.Update {
    class Updater {
        public static void Extract(string zipfile) {
            ZipFile.ExtractToDirectory(zipfile, Environment.CurrentDirectory);
        }

        public static void Copy(string source, string dest) {
            foreach (var dir in Directory.GetDirectories(source)) {
                string newDir = dir.Replace(source, dest);
                if (!Directory.Exists(newDir)) {
                    Directory.CreateDirectory(newDir);
                    Logger.Info($"Create Directory: {newDir}");
                }
            }
            foreach (var file in Directory.GetFiles(source, "*",SearchOption.AllDirectories)) {
                string newFile = file.Replace(source, dest);
                File.Copy(file, newFile, true);
                Logger.Info($"File Copied: {file} to {newFile}");
            }
        }

        public static void RemoveDir(string target) {
            Directory.Delete(target, true);
        }

        public static bool CheckFile(string filename, string checksum) {
            FileInfo fi = new FileInfo(filename);
            var sha512 = SHA512.Create();
            string sum;
            using (var stream = fi.OpenRead()) {
                sum = FormatHash(sha512.ComputeHash(stream));
            }
            return string.Equals(sum, checksum, StringComparison.CurrentCultureIgnoreCase);
        }

        public static void StartUpdate(string filename) {
            ProcessStartInfo psi = new ProcessStartInfo {
                FileName = filename,
                Arguments = "-u"
            };
            new Process {StartInfo = psi}.Start();
        }

        public static void Updating() {
            Copy(Path.GetDirectoryName(Application.ExecutablePath),
                Path.GetDirectoryName(Path.GetDirectoryName(Application.ExecutablePath)));
            Process.Start(
                Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Application.ExecutablePath)),
                    "ShadowVpnSharp.exe"), $"-u --from={Path.GetDirectoryName(Application.ExecutablePath)}");
        }


        private static string FormatHash(IEnumerable<byte> array) {
            StringBuilder sb = new StringBuilder();
            foreach (byte t in array) sb.AppendFormat("{0:X2}", t);
            return sb.ToString();
        }



    }
}
