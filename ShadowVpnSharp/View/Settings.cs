using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShadowVpnSharp.View {
    public partial class Settings : Form {
        public Settings() {
            InitializeComponent();
        }

        private void bCancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void Settings_Load(object sender, EventArgs e) {
            cbAllowAutoChooseFastestServer.Checked = Program.UserConfig.AutoChooseFastestServer;
            cbAllowAutoUpdate.Checked = Program.UserConfig.AutoUpdate;
        }

        private void bOK_Click(object sender, EventArgs e) {
            Program.UserConfig.AutoUpdate = cbAllowAutoUpdate.Checked;
            Program.UserConfig.AutoChooseFastestServer = cbAllowAutoChooseFastestServer.Checked;
            Program.UserConfig.SaveToFile();
            Close();
        }
    }
}
