using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShadowVpnSharp.Configuration;

namespace ShadowVpnSharp.View {
    public partial class GenerateUri : Form {
        public GenerateUri() {
            InitializeComponent();
        }

        private void GenerateUri_Load(object sender, EventArgs e) {
            foreach (var profilesKey in Program.UserConfig.Profiles.Keys) {
                cbProfiles.Items.Add(profilesKey);
            }
        }

        private void cbProfiles_SelectedIndexChanged(object sender, EventArgs e) {
            var s = cbProfiles.SelectedItem as string;

            if (s != null && Program.UserConfig.Profiles.ContainsKey(s)) {
                tbResult.Text = Program.UserConfig.Profiles[s].ToSchemaString(s);
            }

        }

        private void btnCopy_Click(object sender, EventArgs e) {
            if (!string.IsNullOrWhiteSpace(tbResult.Text)) {
                try {
                    Clipboard.SetText(tbResult.Text);
                    MessageBox.Show(this, @"复制成功");
                } catch (Exception) {
                    MessageBox.Show(this, @"复制失败，请再试一次。", @"出错啦", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
