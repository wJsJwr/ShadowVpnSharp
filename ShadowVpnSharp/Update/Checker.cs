using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShadowVpnSharp.Helper;
using fastJSON;

namespace ShadowVpnSharp.Update {
    class Checker {
        private const string UpdateURL = "https://api.github.com/repos/wJsJwr/ShadowVpnSharp/releases/latest";
        private ReleaseInfo _release;

        public async Task<ReleaseInfo> GetLatestVersion() {
            try {
                Downloader d = new Downloader();
                string latest = await d.DownloadStringTaskAsync(UpdateURL);
                dynamic json = fastJSON.JSON.ToDynamic(latest);
                ReleaseInfo info = new ReleaseInfo {
                    DownloadUrl = json.assets[0].browser_download_url,
                    VersionNumber = json.tag_name,
                    FileName = $"{Environment.CurrentDirectory}\\{json.assets[0].name}"
                };
                int checksumIndex = json.body.IndexOf("SHA-512: `") + 10;
                info.Checksum = json.body.Substring(checksumIndex, 128);
                info.ReleaseVersion = new Version(json.name);
                _release = info;
                return info;
            } catch (Exception ex) {
                Logger.Error(ex.Message);
                Logger.Debug(ex.StackTrace);
                throw;
            } 
        }

        public bool IsNewer => _release.ReleaseVersion > Assembly.GetExecutingAssembly().GetName().Version;

        public bool ShowConfirm(IWin32Window owner) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($@"发现新版本：{_release.ReleaseVersion}");
            sb.AppendLine($@"当前版本：{Assembly.GetExecutingAssembly().GetName().Version}");
            sb.Append(@"是否更新？");
            return
                MessageBox.Show(owner, sb.ToString(), @"请确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == DialogResult.Yes;
        }
    }
}
