using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShadowVpnSharp.Device;
using ShadowVpnSharp.Helper;

namespace ShadowVpnSharp.View {
    public partial class TapChecker : Form {
        public TapChecker() {
            InitializeComponent();
        }

        private async void fTapChecker_Load(object sender, EventArgs e) {
            bool overall = true;
            try {
                lbMsg.Text = @"检查Tap-Windows是否安装……";
                if (TapDeviceFunc.CheckTapWindowsIntall()) {
                    lbMsg.Text += "OK\n";
                } else {
                    lbMsg.Text += "Fail\n";
                    overall = false;
                    lbMsg.Text += @"正在安装Tap设备……";
                    Task t = new Task(TapDeviceFunc.InstallTapDriver);
                    t.Start();
                    await t;
                    lbMsg.Text += "OK\n";
                    overall = true;
                }
                lbMsg.Text += @"获取Tap设备……";
                var adapter = TapDeviceFunc.GetTapDevice();
                if (adapter != null) {
                    lbMsg.Text += "OK\n";
                    lbMsg.Text += @"检查Tap设备名……";
                    if (Regex.IsMatch(adapter.Name, @"^[a-zA-Z0-9]+$")) {
                        lbMsg.Text += "OK\n";
                    } else {
                        lbMsg.Text += $"Fail({adapter.Name})\n";
                        overall = false;
                        lbMsg.Text += @"更改Tap设备名……";
                        Task t = new Task(() => { TapDeviceFunc.RenameTapDevice(TapDeviceFunc.DefaultTapDeviceName, adapter); });
                        t.Start();
                        await t;
                        lbMsg.Text += "OK\n";
                        overall = true;
                    }
                } else {
                    lbMsg.Text += "Fail\n";
                    overall = false;
                    lbMsg.Text += @"添加Tap设备……";
                    Task t = new Task(TapDeviceFunc.AddTapDevice);
                    t.Start();
                    await t;
                    adapter = TapDeviceFunc.GetTapDevice();
                    if (adapter == null) {
                        lbMsg.Text += "Fail\n";
                        lbMsg.Text += @"正在安装Tap设备……";
                        t = new Task(TapDeviceFunc.InstallTapDriver);
                        t.Start();
                        await t;
                        lbMsg.Text += "OK\n";
                    }
                    lbMsg.Text += @"获取Tap设备……";
                    adapter = TapDeviceFunc.GetTapDevice();
                    if (adapter == null) {
                        lbMsg.Text += "Fail\n";
                        overall = false;
                        Logger.Error("Can not get TapDevice anyway");
                        TapDeviceFunc.TraceNetworkInterface();
                    } else {
                        TapDeviceFunc.RenameTapDevice(TapDeviceFunc.DefaultTapDeviceName, adapter);

                        lbMsg.Text += "OK\n";
                        overall = true;
                    }
                    
                }
            } catch (Exception ex) {
                Logger.Error(ex.Message);
                Logger.Debug(ex.StackTrace);
            } finally {
                TapDeviceFunc.GetSetupLog();
                if(overall) {
                    lbMsg.Text += @"硬件检测成功，一切就绪，点击此处退出程序。";
                } else {
                    lbMsg.Text += "\n";
                    lbMsg.Text += @"硬件检测出错误，修复失败，正在生成诊断报告……";
                    new DiagnosticProgress().ShowDialog(this);
                    lbMsg.Text += @"生成完毕，点击此处退出程序。";
                }
                lbMsg.Click += (_sender, _e) => {
                    Application.Exit();
                };
            }
            
        }
        
    }
}
