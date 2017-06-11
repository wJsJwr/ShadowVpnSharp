using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShadowVpnSharp.Update;

namespace ShadowVpnSharp.View {
    public partial class UpdateDownload : Form {
        private ReleaseInfo _info;
        private EntryWindow _caller;
        public UpdateDownload(EntryWindow caller) {
            _caller = caller;
            InitializeComponent();
        }

        public UpdateDownload(EntryWindow caller, ReleaseInfo r) {
            _caller = caller;
            _info = r;
            InitializeComponent();
        }

        private async void UpdateDownload_Load(object sender, EventArgs e) {
            if (_info == null) {
                lbStatus.Text = @"正在检查更新";
                pb.Style = ProgressBarStyle.Marquee;
                Checker checker = new Checker();
                try {
                    _info = await checker.GetLatestVersion();
                    if (checker.IsNewer) {
                        if (checker.ShowConfirm(this)) {
                            StartDownload();
                            return;
                        }
                    } else {
                        MessageBox.Show(this, @"当前版本已是最新版本");
                    }
                } catch (Exception exception) {
                    MessageBox.Show(this, exception.Message + Environment.NewLine + @"请稍后再试。", @"出错啦", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            } else {
                StartDownload();
                return;
            }
            Close();
        }

        private void StartDownload() {
            lbStatus.Text = @"正在下载";
            Console.WriteLine(_info.DownloadUrl);
            pb.Style = ProgressBarStyle.Continuous;
            pb.Maximum = 100;
            pb.Minimum = 0;
            pb.Value = 0;
            Downloader d = new Downloader();
            d.DownloadFileCompleted += D_DownloadFileCompleted;
            d.DownloadProgressChanged += D_DownloadProgressChanged;
            try {
                d.DownloadFileAsync(new Uri(_info.DownloadUrl), _info.FileName);
            } catch (Exception e) {
                MessageBox.Show(this, e.Message + Environment.NewLine + @"请稍后再试。", @"出错啦", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if(File.Exists(_info.FileName)) File.Delete(_info.FileName);
                Close();
            }

        }

        private void D_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            lbStatus.Text = $@"正在下载({e.BytesReceived}/{e.TotalBytesToReceive})";
            pb.Value = e.ProgressPercentage;

        }

        private void D_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e) {
            lbStatus.Text = @"正在检查文件";
            pb.Style = ProgressBarStyle.Marquee;
            if (Updater.CheckFile(_info.FileName, _info.Checksum)) {
                lbStatus.Text = @"文件校验成功，即将开始更新";
                Updater.Extract(_info.FileName);
                Updater.StartUpdate(_info.FileName.Substring(0, _info.FileName.Length - 4) + "\\ShadowVpnSharp.exe");
                _caller.StopVpnAndExit();
            } else {
                lbStatus.Text = @"文件校验失败，请稍后再试";
                MessageBox.Show(this, $@"文件校验失败{Environment.NewLine}建议连接上VPN后再试。", @"出错啦", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
