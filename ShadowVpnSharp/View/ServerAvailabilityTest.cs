using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShadowVpnSharp.Helper;

namespace ShadowVpnSharp.View {
    public partial class ServerAvailabilityTest : Form {
        private readonly EntryWindow _ew;
        private const int PingTestCount = 5;
        private double _minimumDropRate = double.MaxValue;
        private double _minimumRtt = double.MaxValue;
        private string _fastestProfileName;
        public ServerAvailabilityTest(EntryWindow e) {
            _ew = e;
            InitializeComponent();
        }

        private void ServerAvailabilityTest_Load(object sender, EventArgs e) {
            progressBar.Minimum = 0;
            progressBar.Maximum = Program.UserConfig.Profiles.Count * PingTestCount;
            BackgroundWorker bw = new BackgroundWorker {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            bw.DoWork += BwOnDoWork;
            bw.ProgressChanged += (o, args) => {
                progressBar.Value = args.ProgressPercentage;
                lbProgress.Text = args.UserState as string;
            };
            bw.RunWorkerCompleted += (o, args) => {
                if (args.Cancelled) {
                    Close();
                } else {
                    _ew.ProfileMenuItem_Click(null, _fastestProfileName);
                    MessageBox.Show(this,
                        $@"测试完成，已选择服务器配置：{_fastestProfileName}{Environment.NewLine}丢包率：{_minimumDropRate * 100}%{Environment
                            .NewLine}往返时延：{_minimumRtt}ms", "成功");
                    Close();
                }
            };
            bw.RunWorkerAsync();
        }

        private void BwOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs) {
            var worker = sender as BackgroundWorker;
            int tick = 0;
            foreach (var profile in Program.UserConfig.Profiles) {
                int dropped = 0;
                int sumRTT = 0;
                for (int i = 0; i < PingTestCount; i++) {
                    var result = Diagnostic.PingIt(profile.Value.ServerIp.Addr);
                    if (result.StartsWith("Reply from")) {
                        var fi = result.IndexOf("time=", StringComparison.CurrentCultureIgnoreCase) + 5;
                        var li = result.IndexOf("TTL", StringComparison.CurrentCultureIgnoreCase) - 1;
                        sumRTT += int.Parse(result.Substring(fi, li - fi));
                    } else {
                        dropped += 1;
                    }
                    worker?.ReportProgress(++tick, $@"正在测试配置文件：{profile.Key}");
                }
                if (_minimumDropRate > (double) dropped / PingTestCount) {
                    _fastestProfileName = profile.Key;
                    _minimumDropRate = (double) dropped / PingTestCount;
                    _minimumRtt = (double) sumRTT / (PingTestCount - dropped);
                } else {
                    if (_minimumRtt > (double) sumRTT / (PingTestCount - dropped)) {
                        _fastestProfileName = profile.Key;
                        _minimumRtt = (double)sumRTT / (PingTestCount - dropped);
                    }
                }

            }
        }
    }
}
