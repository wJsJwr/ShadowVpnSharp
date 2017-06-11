using System;
using System.Drawing;
using System.Windows.Forms;
using ShadowVpnSharp.Configuration;
using ZXing;

namespace ShadowVpnSharp.View {
    public partial class CaptureQrCode : Form {
        public CaptureQrCode() {
            InitializeComponent();
        }

        private void btnCap_Click(object sender, EventArgs e) {
            var psb = Screen.PrimaryScreen.Bounds;
            Bitmap scr = new Bitmap(psb.Width, psb.Height);
            Graphics g = Graphics.FromImage(scr);
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(psb.Width, psb.Height));
            IBarcodeReader reader = new BarcodeReader();
            var result = reader.Decode(scr);
            if (result?.BarcodeFormat == BarcodeFormat.QR_CODE) {
                Console.WriteLine(result.Text);
                if (Profile.CheckUriFormat(result.Text) && Profile.ValidateString(result.Text)) {
                    new FromUri(result.Text).ShowDialog(this);
                    Close();
                } else {
                    MessageBox.Show(this, "屏幕上的二维码不是合法的ShadowVPNSharp二维码。");
                }
            } else {
                MessageBox.Show(this, "未能识别二维码，请尝试移动或者放大二维码。");
            }
        }
    }
}
