using System;
using System.Windows.Forms;
using ShadowVpnSharp.Configuration;
using ShadowVpnSharp.Helper;

namespace ShadowVpnSharp.View {
    public partial class FromUri : Form {
        private Profile _profile;
        private readonly string _argUri;
        public FromUri() {
            _argUri = null;
            InitializeComponent();
        }

        public FromUri(string uri) {
            _argUri = uri;
            InitializeComponent();
        }

        private void FromUri_Load(object sender, EventArgs e) {
            btnAdd.Enabled = false;
            lbErr.Visible = false;
            try {
                var cdata = Clipboard.GetText();
                if (Profile.CheckUriFormat(cdata)) {
                    tbUri.Text = cdata;
                }
            } catch (Exception exception) {
                Logger.Error(exception.Message);
            }
            if (!string.IsNullOrEmpty(_argUri)) {
                tbUri.Text = _argUri;
                tbUri.Enabled = false;
                tbProfileName.Focus();
                lbFor_tbUri.Visible = false;
            }

        }

        private void tbUri_TextChanged(object sender, EventArgs e) {
            if (Profile.CheckUriFormat(tbUri.Text.Trim())) {
                try {
                    _profile = Profile.FromBase64(tbUri.Text.Trim());
                    tbProfileName.Text = Profile.GetTitleFromString(tbUri.Text.Trim()) ?? Profile.GenerateTitle();
                    lbErr.Visible = false;
                    btnAdd.Enabled = true;
                } catch (Exception) {
                    lbErr.Visible = true;
                    btnAdd.Enabled = false;
                }
            } else {
                lbErr.Visible = true;
                btnAdd.Enabled = false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(tbProfileName.Text)) {
                MessageBox.Show(@"请填写配置名称");
            } else {
                try {
                    Program.UserConfig.AddProfile(tbProfileName.Text, _profile);
                    Program.UserConfig.SaveToFile();
                    Close();
                } catch (Exception exception) {
                    MessageBox.Show(this, exception.Message);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
