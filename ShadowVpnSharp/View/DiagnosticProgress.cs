using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;
using ShadowVpnSharp.Helper;

namespace ShadowVpnSharp.View {
    public partial class DiagnosticProgress : Form {
        private readonly bool _vpnConnected;
        private readonly string _reportPath;
        public DiagnosticProgress(bool vpnConnected=false) {
            _vpnConnected = vpnConnected;
            InitializeComponent();
            _reportPath = Path.Combine(ComponentPath.RootPath,
                $"log\\diagnostic_report_{DateTime.Now:yyyy_MM_dd_HH_mm_ss_ff}.txt");

            var parent = new FileInfo(_reportPath).Directory;
            if (parent != null && !parent.Exists) parent.Create();

            _vpnConnected = vpnConnected;
        }

        private void DiagnosticProgress_Load(object sender, EventArgs e) {
            theProgress.Minimum = 0;
            theProgress.Maximum = 100;
            BackgroundWorker bw = new BackgroundWorker {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = false
            };
            bw.DoWork += Bw_DoWork;
            bw.ProgressChanged += (o, args) => {
                theProgress.Value = args.ProgressPercentage;
            };
            bw.RunWorkerCompleted += (o, args) => {
                MessageBox.Show(@"报告已生成，"+'\n'+$@"保存于{Path.GetFileName(_reportPath)}");
                Close();
            };
            bw.RunWorkerAsync();
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e) {
            int current = 0;
            const int all = 100;
            var worker = sender as BackgroundWorker;
            using (var file = File.OpenWrite(_reportPath)) {
                using (var sw = new StreamWriter(file)) {
                    sw.WriteLine("===Configuration Information===");
                    try {
                        sw.WriteLine("SECTION: app.user.conf");
                        sw.WriteLine(File.Exists(ComponentPath.UserConfigPath)
                            ? File.ReadAllText(ComponentPath.UserConfigPath)
                            : "\t!!!Not Exist!!!");
                    } catch (Exception exception) {
                        sw.WriteLine(exception);
                    } finally {
                        current += 5;
                        worker?.ReportProgress(current * 100 / all);
                    }

                    try {
                        sw.WriteLine("SECTION: vpn.user.conf");
                        sw.WriteLine(File.Exists(ComponentPath.VpnConfigPath)
                            ? File.ReadAllText(ComponentPath.VpnConfigPath)
                            : "\t!!!Not Exist!!!");
                    } catch (Exception exception) {
                        sw.WriteLine(exception);
                    } finally {
                        current += 5;
                        worker?.ReportProgress(current * 100 / all);
                    }
                    sw.WriteLine("===============================\n\n\n");


                    try {
                        sw.WriteLine("===File List===");
                        foreach (var entry in Directory.EnumerateFileSystemEntries(ComponentPath.RootPath,"*",SearchOption.AllDirectories)) {
                            if(entry == _reportPath) continue;
                            sw.WriteLine($"{Diagnostic.FileSha1Hash(entry).PadRight(40)}\t{entry}");
                        }
                        sw.WriteLine("===============\n\n\n");
                    } catch (Exception exception) {
                        sw.WriteLine(exception);
                    } finally {
                        current += 10;
                        worker?.ReportProgress(current * 100 / all);
                    }

                    try {
                        sw.WriteLine("===Process Information===");
                        sw.WriteLine(Diagnostic.ListAllProcess());
                        sw.WriteLine("==========================\n\n\n");
                    } catch (Exception exception) {
                        sw.WriteLine(exception);
                    } finally {
                        current += 10;
                        worker?.ReportProgress(current * 100 / all);
                    }

                    try {
                        sw.WriteLine("===Network Interface Information===");
                        var list = Diagnostic.ListAllInterfaces();
                        foreach (var kv in list) {
                            sw.WriteLine($"InterfaceId: {kv.Key}");
                            foreach (var s in kv.Value.ToString().Split("\r\n".ToCharArray(),StringSplitOptions.RemoveEmptyEntries)) {
                                sw.WriteLine($"\t{s}");
                            }
                            sw.WriteLine();
                        }
                        sw.WriteLine("===================================\n\n\n");
                    } catch (Exception exception) {
                        sw.WriteLine(exception);
                    } finally {
                        current += 10;
                        worker?.ReportProgress(current * 100 / all);
                    }

                    try {
                        sw.WriteLine("===VPN Status===");
                        sw.WriteLine("VPN is " + (_vpnConnected ? "On" : "Off"));
                        sw.WriteLine("================\n\n\n");
                    } catch (Exception exception) {
                        sw.WriteLine(exception);
                    } finally {
                        current += 10;
                        worker?.ReportProgress(current * 100 / all);
                    }

                    try {
                        sw.WriteLine("===Route Status===");
                        sw.WriteLine(Diagnostic.ListAllRoutes());
                        sw.WriteLine("==================\n\n\n");
                    } catch (Exception exception) {
                        sw.WriteLine(exception);
                    } finally {
                        current += 10;
                        worker?.ReportProgress(current * 100 / all);
                    }


                    sw.WriteLine("===Ping Test===");
                    const int times = 10;
                    foreach (var address in new[]{IPAddress.Parse("8.8.8.8"),IPAddress.Parse("114.114.114.114"),IPAddress.Parse("107.191.52.20") }) {
                        sw.WriteLine($"Begin to ping {address} with {times} packages");
                        for (uint i = 0; i < times; i++) {
                            sw.WriteLine($"\t{Diagnostic.PingIt(address)}");
                            current++;
                            worker?.ReportProgress(current * 100 / all);
                        }
                    }
                    sw.WriteLine("===============\n\n\n");

                    try {
                        sw.WriteLine("===Tracert Status===");
                        sw.WriteLine(Diagnostic.CallTracert(" -d 114.114.114.114"));
                        sw.WriteLine("==================\n\n\n");
                    } catch (Exception exception) {
                        sw.WriteLine(exception);
                    } finally {
                        current += 10;
                        worker?.ReportProgress(current * 100 / all);
                    }


                }
            }
        }
    }
}
