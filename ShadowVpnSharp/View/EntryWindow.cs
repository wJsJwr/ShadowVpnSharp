using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShadowVpnSharp.Configuration;
using ShadowVpnSharp.Core;
using ShadowVpnSharp.Device;
using ShadowVpnSharp.Helper;
using ShadowVpnSharp.Update;

namespace ShadowVpnSharp.View {
    public partial class EntryWindow : Form {
        private static VPNDaemon daemon = null;
        private readonly Color Warn = Color.LightCoral;
        private readonly Color Primary = Color.DodgerBlue;
        private readonly Color Success = Color.ForestGreen;
        private List<ToolStripMenuItem> profileList;
        private List<ulong> statRecvQueue;
        private List<ulong> statSentQueue;
        private const int histCount = 71;
        private const int histWidth = 4;
        private const int histMaxHeight = 118;
        private const int centerY = 143;

        private readonly Font _statusFont = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
        private readonly Font _titleFont = new Font("微软雅黑", 16.25F, FontStyle.Regular, GraphicsUnit.Point, 134);
        private readonly StringFormat _rightAlignFormat = new StringFormat { Alignment = StringAlignment.Far };
        private readonly StringFormat _centerFormat = new StringFormat {Alignment = StringAlignment.Center};
        private readonly Brush _histogramBrush = new SolidBrush(Color.FromArgb(128, 255, 255, 255));
        private readonly Brush _statusBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
        private readonly Brush _bgBrush = new SolidBrush(Color.FromArgb(64, 0, 0, 0));

        private bool _picHover = false;

        public static VPNDaemon Daemon => daemon;

        public EntryWindow() {
            InitializeComponent();
            lbCurrentProfileName.Parent = PicStat;
            statRecvQueue = new List<ulong>(histCount + 1);
            statSentQueue = new List<ulong>(histCount + 1);
            daemon = VPNDaemon.Empty();
        }

        private void MenuItemHardwareCheck_Click(object sender, EventArgs e) {
            ProcessStartInfo psi = new ProcessStartInfo {
                FileName = Application.ExecutablePath,
                Arguments = "-d",
                Verb = "runas"
            };

            Process ps = new Process { StartInfo = psi };
            try {
                ps.Start();
                ps.WaitForExit();
            } catch (Exception ex) {
                MessageBox.Show(this, ex.Message);
            } finally {
                Program.UserConfig = Config.FromFile();
            }
        }

        private void MenuItemResetTapDevice_Click(object sender, EventArgs e) {
            if (MessageBox.Show(this, @"将会清除现有的Tap设备，是否确定？", @"Caution", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                TapDeviceFunc.DeleteTapDevice();
                MenuItemHardwareCheck_Click(sender, e);
            }
        }

        private void MenuItemDiagnostic_Click(object sender, EventArgs e) {
            var d = new DiagnosticProgress(daemon?.Running ?? false);
            d.ShowDialog(this);
        }

        private void EntryWindow_Load(object sender, EventArgs e) {
            BackColor = Primary;
            bVpnSwitch.FlatAppearance.MouseOverBackColor = Color.FromArgb(128, 0, 0, 0);
            bVpnSwitch.FlatAppearance.MouseDownBackColor = Color.FromArgb(128, 255, 255, 255);
            lbCurrentProfileName.BackColor = Color.FromArgb(0, 0, 0, 0);

            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            Text = $"ShadowVpnSharp {v.Major}.{v.Minor}.{v.Build}";

            DisplayConfig();
            BackgroundChecker();
        }


        private async void BackgroundChecker() {
            try {
                ShadowVpnSharp.Update.Checker checker = new Checker();
                var ri = await checker.GetLatestVersion();
                if (checker.IsNewer && checker.ShowConfirm(this)) {
                    new UpdateDownload(this, ri).ShowDialog(this);
                }
            } catch (Exception ex) {
                Logger.Error(ex.Message);
            }
        }


        private void DisplayConfig() {
            profileList = new List<ToolStripMenuItem>();
            MenuItemChooseProfile.DropDownItems.Clear();
            foreach (var profile in Program.UserConfig.Profiles) {
                ToolStripMenuItem item = new ToolStripMenuItem(profile.Key);
                item.Click += (sender, args) => {
                    var s = sender as ToolStripMenuItem;
                    Program.UserConfig.SetCurrentProfile(s?.Text);
                    foreach (var menuItem in profileList) {
                        if (menuItem.Checked && menuItem != s) menuItem.Checked = false;
                        if (menuItem == s) menuItem.Checked = true;
                    }
                    lbCurrentProfileName.Text = Program.UserConfig.CurrentProfileName;
                    bVpnSwitch.Enabled = true;
                };
                if (profile.Key == Program.UserConfig.CurrentProfileName) item.Checked = true;
                profileList.Add(item);
                MenuItemChooseProfile.DropDownItems.Add(item);
            }
            if (MenuItemChooseProfile.DropDownItems.Count == 0) {
                MenuItemChooseProfile.DropDownItems.Add(EmptyToolStripMenuItem);
            }

            if (Program.UserConfig.CurrentProfileName == null) {
                lbCurrentProfileName.Text = @"没有配置文件";
                bVpnSwitch.Enabled = false;
            } else {
                lbCurrentProfileName.Text = Program.UserConfig.CurrentProfileName;
                bVpnSwitch.Enabled = true;
            }
        }

        private void bVpnSwitch_Click(object sender, EventArgs e) {
            if (daemon!=null && !daemon.Running) {
                daemon = new VPNDaemon();
                daemon.OnUnexpectedExit += Reset;
                daemon.Start();
                lbCurrentProfileName.Hide();
                bVpnSwitch.Hide();
                StatTimer.Start();
                MenuItemManageProfile.Enabled = false;
                MenuItemChooseProfile.Enabled = false;
            }
        }

        private void Reset(object sender, EventArgs ea) {
            Invoke(new Action<object, EventArgs>((s, e) => {
                bVpnSwitch.Show();
                StatTimer.Stop();
                PicStat.CreateGraphics().Clear(Primary);
                lbCurrentProfileName.Show();
                MenuItemManageProfile.Enabled = true;
                MenuItemChooseProfile.Enabled = true;
            }), sender, ea);
        }

        private void StatTimer_Tick(object sender, EventArgs ea) {
            Invoke(new Action<object, EventArgs>((s, e) => {
                statRecvQueue.Add(daemon.VpnBytesRecvOffset);
                if (statRecvQueue.Count > histCount) statRecvQueue.RemoveAt(0);
                statSentQueue.Add(daemon.VpnBytesSentOffset);
                if (statSentQueue.Count > histCount) statSentQueue.RemoveAt(0);
                DrawPicBox();
            }), sender, ea);
        }

        private void DrawPicBox() {
            var g = PicStat.CreateGraphics();
            g.Clear(Primary);
            if (statSentQueue.Count > 0) {
                var bpp = Math.Max(statSentQueue.Max(), statRecvQueue.Max()) / (histMaxHeight * 0.9);
                g.DrawLine(Pens.White, 0, centerY, 284, centerY);
                for (var i = 1; i <= statSentQueue.Count; i++) {
                    var height = statSentQueue[statSentQueue.Count - i] / bpp;
                    g.FillRectangle(_histogramBrush, 284 - histWidth * i, (float)(centerY - height), histWidth, (float)height);
                    height = statRecvQueue[statRecvQueue.Count - i] / bpp;
                    g.FillRectangle(_histogramBrush, 284 - histWidth * i, centerY, histWidth, (float)height);
                }
                g.DrawString($"Out: {SpeedString(statSentQueue.Last())}", _statusFont, _statusBrush, new PointF(284, 25), _rightAlignFormat);
                g.DrawString($"In: {SpeedString(statRecvQueue.Last())}", _statusFont, _statusBrush, new PointF(284, 246), _rightAlignFormat);
            }


            if (_picHover) {
                DrawHover(g);
            }
        }

        private void DrawHover() {
            DrawHover(PicStat.CreateGraphics());
        }
        private void DrawHover(Graphics g) {
            g.FillRectangle(_bgBrush, 0, 0, 284, 261);
            g.DrawString("停止", _titleFont, _statusBrush, 142, 161, _centerFormat);
        }

        private void MenuItemManageProfile_Click(object sender, EventArgs e) {
            new ProfileManageWindow().ShowDialog(this);
            DisplayConfig();
        }

        private const ulong K = 0x400;
        private const ulong M = 0x100000;


        private static string SpeedString(ulong speed) {
            if (speed < K) {
                return $@"{speed} byte/s";
            }
            ulong first, second;
            if (speed < M) {
                first = speed / K;
                second = speed % K * 10 / K;
                return $@"{first}.{second} kb/s";
            } else {
                first = speed / M;
                second = speed % M * 10 / M;
                return $@"{first}.{second} mb/s";
            }
        }

        private void PicStat_Click(object sender, EventArgs e) {
            if (_picHover && daemon.Running) {
                daemon.Exit();
                Reset(sender, e);
            }
        }

        public void StopVpnAndExit() {
            if (daemon.Running) {
                daemon.Exit();
                Reset(null, null);
            }
            Application.Exit();
        }

        private void PicStat_MouseEnter(object sender, EventArgs e) {
            if (daemon.Running) {
                PicStat.Cursor = Cursors.Hand;
                _picHover = true;
                DrawHover();
            }
        }

        private void PicStat_MouseLeave(object sender, EventArgs e) {
           _picHover = false;
            if (daemon.Running) {
                PicStat.Cursor = Cursors.Default;
                DrawPicBox();
            }
        }



        private void MenuItemGenerateUri_Click(object sender, EventArgs e) {
            new GenerateUri().Show(this);
        }

        private void MenuItemImportFromUri_Click(object sender, EventArgs e) {
            new FromUri().ShowDialog(this);
            DisplayConfig();
        }

        private void MenuItemGenerateQRCode_Click(object sender, EventArgs e) {
            new GenerateQrCode().Show(this);
        }

        private void MenuItemRecognizeQRCode_Click(object sender, EventArgs e) {
            new CaptureQrCode().ShowDialog(this);
            DisplayConfig();
        }

        private void MenuItemAbout_Click(object sender, EventArgs e) {
            new About().ShowDialog(this);
        }

        private void MenuItemUpdate_Click(object sender, EventArgs e) {
            new UpdateDownload(this).ShowDialog(this);
        }

        private void EntryWindow_FormClosing(object sender, FormClosingEventArgs e) {
            daemon?.Kill();
        }
    }
}
