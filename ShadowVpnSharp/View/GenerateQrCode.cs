using System;
using System.Drawing;
using System.Windows.Forms;
using ZXing.QrCode.Internal;

namespace ShadowVpnSharp.View {
    public partial class GenerateQrCode : Form {
        public GenerateQrCode() {
            InitializeComponent();
        }

        private void GenerateQrCode_Load(object sender, EventArgs e) {
            foreach (var profilesKey in Program.UserConfig.Profiles.Keys) {
                cbProfiles.Items.Add(profilesKey);
            }
        }

        private void cbProfiles_SelectedIndexChanged(object sender, EventArgs e) {
            var s = cbProfiles.SelectedItem as string;

            if (s != null && Program.UserConfig.Profiles.ContainsKey(s)) {
                GenerateQrCodeFromString(Program.UserConfig.Profiles[s].ToSchemaString(s));
            }
        }

        private void GenerateQrCodeFromString(string str) {
            QRCode qr = ZXing.QrCode.Internal.Encoder.encode(str, ErrorCorrectionLevel.M);
            ByteMatrix m = qr.Matrix;

            int blockSize = Math.Max(picQrCode.Height / m.Height, 1);

            picQrCode.SizeMode = blockSize == 1 ? PictureBoxSizeMode.Zoom : PictureBoxSizeMode.CenterImage;

            Bitmap drawArea = new Bitmap(m.Width * blockSize, m.Height * blockSize);
            using (Graphics g = Graphics.FromImage(drawArea)) {
                g.Clear(Color.White);
                using (Brush b = new SolidBrush(Color.Black)) {
                    for (int row = 0; row < m.Width; row++) {
                        for (int col = 0; col < m.Height; col++) {
                            if (m[row, col] != 0) {
                                g.FillRectangle(b, blockSize * row, blockSize * col, blockSize, blockSize);
                            }
                        }
                    }
                }
            }
            picQrCode.Image = drawArea;
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(cbProfiles.Text)) return;
            SaveFileDialog sfd = new SaveFileDialog { AddExtension = true, DefaultExt = ".bmp",FileName = "share.bmp",Filter = "Bitmap|*.bmp"};
            if (sfd.ShowDialog(this) == DialogResult.OK) {
                try {
                    picQrCode.Image.Save(sfd.FileName);
                    MessageBox.Show(this, "保存成功");
                } catch (Exception ex) {
                    MessageBox.Show(this, $"保存失败, {ex.Message}");

                }
            }
        }

        private void btnCopy_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(cbProfiles.Text)) return;

            try {
                Clipboard.SetImage(picQrCode.Image);
                MessageBox.Show(this, "复制成功");
            } catch (Exception ex) {
                MessageBox.Show(this, $"复制失败, {ex.Message}");
            }
        }
    }
}
