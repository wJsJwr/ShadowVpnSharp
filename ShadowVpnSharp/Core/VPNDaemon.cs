using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ShadowVpnSharp.Helper;
using ShadowVpnSharp.Route;

namespace ShadowVpnSharp.Core {
    public class VPNDaemon {
        private readonly VpnRoute _route;

        private readonly ProcessStartInfo _psi = new ProcessStartInfo();
        private Process _sv;

        public delegate void NormalExitHandler(object sender, EventArgs e);

        public delegate void UnexpectedExitHandler(object sender, EventArgs e);

        public delegate void StartHandler(object sender, EventArgs e);

        public event NormalExitHandler OnNormalExit;
        public event UnexpectedExitHandler OnUnexpectedExit;
        public event StartHandler OnStart;

        private Thread _statusThread;
        private UdpClient _ctrlClient;
        private bool _isEmpty = false;

        public int ExitCode => _sv.ExitCode;

        public bool Running {
            get {
                if (_sv == null) return false;
                return !_sv.HasExited;
            }
        }

        public ulong VpnBytesRecvOffset => _bytesRecv.Offset;
        public ulong VpnBytesSentOffset => _bytesSent.Offset;
        private readonly Counter _bytesSent = new Counter(0);
        private readonly Counter _bytesRecv = new Counter(0);

        public VPNDaemon() {
            _route = new VpnRoute();
            _psi.FileName = ComponentPath.VpnBinaryPath;
            _psi.Arguments = $"-c \"{ComponentPath.VpnConfigPath}\" -v";
            _psi.Verb = "runas";
            _psi.WorkingDirectory = Directory.GetParent(ComponentPath.VpnBinaryPath).ToString();
            _psi.CreateNoWindow = true;
            _psi.UseShellExecute = false;
            _psi.RedirectStandardError = true;
            _psi.RedirectStandardOutput = true;
            _psi.WindowStyle = ProcessWindowStyle.Hidden;
        }

        private VPNDaemon(bool empty) {
            _sv = null;
            _isEmpty = true;
        }

        public static VPNDaemon Empty() {
            return new VPNDaemon(true);
        }

        private void CtrlThread() {
            byte[] ctrlCode = {2};
            var tempPoint = new IPEndPoint(IPAddress.Loopback, 55152);
            while (true) {
//                ctrlCode[0] = 1;
                try {
                    var r = _ctrlClient.Send(ctrlCode, 1);
                    if (r > 0) {
                        var buffer = _ctrlClient.Receive(ref tempPoint);
                        if (buffer.Length == 16) {
                            _bytesRecv.Value = BitConverter.ToUInt64(buffer, 0);
                            _bytesSent.Value = BitConverter.ToUInt64(buffer, 8);
                        }
                    }
                } catch (Exception e) {
                    Logger.Error(e.Message);
                    Logger.Debug(e.StackTrace);
                    Exit(true);
                    break;
                }

                Thread.Sleep(500);
            }
        }

        public void Exit(bool fromCtrl = false) {
            if (_isEmpty) return;
            if (_sv.HasExited) return;
            byte[] exitCode = {0};
            var r = _ctrlClient.Send(exitCode, 1);
            if (r > 0)
                if (_sv.WaitForExit(3000) == false) {
                    Logger.Info("shadowvpn didn't exit in 3 seconds, kill it");
                    _sv.Kill();
                }

            _ctrlClient.Close();
            _ctrlClient.Dispose();
            if (fromCtrl) _statusThread.Abort();

            try {
                _route.Down();
            } catch (Exception e) {
                Logger.Error(e.Message);
                Logger.Debug(e.StackTrace);
                // INFO: suppress
            }

            if (fromCtrl) OnUnexpectedExit?.Invoke(this, EventArgs.Empty);
        }

        public void Kill() {
            if (_isEmpty) return;
            if (!_sv.HasExited) {
                try {
                    _sv.Kill();
                } catch (Exception e) {
                    Logger.Error(e.Message);
                }
                try {
                    _ctrlClient.Close();
                } catch (Exception e) {
                    Logger.Error(e.Message);
                }
                try {
                    _ctrlClient.Dispose();
                } catch (Exception e) {
                    Logger.Error(e.Message);
                }
                try {
                    _statusThread.Abort();
                } catch (Exception e) {
                    Logger.Error(e.Message);
                }
            }
            try {
                if(_route.IsUp) _route.Down();
            } catch (Exception e) {
                Logger.Error(e.Message);
            }
        }

        public void Start() {
            if (_isEmpty) return;
            _sv = new Process {
                StartInfo = _psi,
                EnableRaisingEvents = true
            };
            _sv.Exited += (sender, e) => {
                Thread.Sleep(500); // wait for async io
                _sv.CancelErrorRead();
                _sv.CancelOutputRead();
                OnNormalExit?.Invoke(sender, e);
            };
            _sv.OutputDataReceived += Daemon_OutputDataReceived;
            _sv.ErrorDataReceived += Daemon_ErrorDataReceived;
            _sv.Start();
            _sv.BeginErrorReadLine();
            _sv.BeginOutputReadLine();
        }

        private static void Daemon_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
            try {
                Logger.Error($"[sv]{e.Data}");
                if (!e.Data.Contains("warn")) MessageBox.Show("ERR:" + e.Data);
            } catch (NullReferenceException) {
                //suppress
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void Daemon_OutputDataReceived(object sender, DataReceivedEventArgs e) {
            try {
                Logger.Debug($"[sv]{e.Data}");
                if (e.Data.Contains("VPN started")) {
                    _route.Up();

                    _ctrlClient = new UdpClient("127.0.0.1", 55152);
                    _statusThread = new Thread(CtrlThread);
                    _statusThread.Start();

                    OnStart?.Invoke(this, new EventArgs());
                }
            } catch (NullReferenceException) {
                //suppress
            } catch (Exception ex) {
                Logger.Error(ex.Message);
                Logger.Debug(ex.StackTrace);
                MessageBox.Show(ex.Message);
                try {
                    Exit();
                } catch (Exception x) {
                    Logger.Error(x.Message);
                    Logger.Debug(x.StackTrace);
                    MessageBox.Show(x.Message);
                }
            }
        }
    }
}