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
using ShadowVpnSharp.Helper;

namespace ShadowVpnSharp.View {
    public partial class ProfileManageWindow : Form {
        public ProfileManageWindow() {
            InitializeComponent();
        }

        private void ProfileManageWindow_Load(object sender, EventArgs e) {
            RefreshList();
        }


        private void RefreshList() {
            lsProfiles.Items.Clear();
            foreach (var key in Program.UserConfig.Profiles.Keys) {
                lsProfiles.Items.Add(key);
            }
            if (Program.UserConfig.Profiles.Count > 0) {
                lsProfiles.SelectedIndex = 0;
            }
        }

        private void lsProfiles_SelectedIndexChanged(object sender, EventArgs e) {
            var key = lsProfiles.SelectedItem as string;
            ReadProfile(key);
        }

        private void cbNeedUsertoken_CheckedChanged(object sender, EventArgs e) {
            tbUsertoken.Enabled = cbNeedUsertoken.Checked;
            lbUsertoken.Enabled = cbNeedUsertoken.Checked;
        }



        private void bAddProfile_Click(object sender, EventArgs e) {
            string key = GenerateNewProfileName();
            try {
                Program.UserConfig.AddProfile(key, new Profile(new Profile.ProfileJsonObject()));
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
            lsProfiles.Items.Add(key);
            lsProfiles.SelectedIndex = lsProfiles.Items.Count - 1;
        }

        private string GenerateNewProfileName() {
            const string basename = "New Profile";
            if (!Program.UserConfig.Profiles.ContainsKey(basename)) return basename;
            int i = 1;
            string newname = $"{basename} {i}";
            while (Program.UserConfig.Profiles.ContainsKey(newname)) {
                i += 1;
                newname = $"{basename} {i}";
            }
            return newname;
        }


        private void ReadProfile(string key) {
            if (key == null || !Program.UserConfig.Profiles.ContainsKey(key)) return;
            var profile = Program.UserConfig.Profiles[key];
            tbProfileName.Text = key;
            tbServerIp.Text = profile.Server;
            tbServerPort.Text = profile.Port.ToString();
            tbTunIp.Text = profile.Ip;
            tbNetmask.Text = profile.Subnet;
            tbRemoteTunIp.Text = profile.RemoteTunIp;
            tbDns.Text = profile.Dns;
            tbMtu.Text = profile.Mtu.ToString();
            tbPassword.Text = profile.Password;
            cbNeedUsertoken.Checked = profile.HasUserToken;
            if (!profile.HasUserToken) {
                tbUsertoken.Enabled = false;
                lbUsertoken.Enabled = false;
            } else {
                tbUsertoken.Text = profile.UserToken;
            }
        }

        private void bOk_Click(object sender, EventArgs e) {
            try {
                var po = new Profile.ProfileJsonObject {
                    server = tbServerIp.Text,
                    port = int.Parse(tbServerPort.Text),
                    ip = tbTunIp.Text,
                    subnet = tbNetmask.Text,
                    remote_tun_ip = tbRemoteTunIp.Text,
                    dns = tbDns.Text,
                    mtu = uint.Parse(tbMtu.Text),
                    password = tbPassword.Text,
                    usertoken = !string.IsNullOrEmpty(tbUsertoken.Text) && cbNeedUsertoken.Checked ? tbUsertoken.Text : null
                };

                var oldkey = lsProfiles.SelectedItem as string;
                if (tbProfileName.Text != oldkey) {
                    if (string.IsNullOrWhiteSpace(tbProfileName.Text)) {
                        throw new Exception(@"配置名不能为空");
                    }
                    if (Program.UserConfig.Profiles.ContainsKey(tbProfileName.Text)) {
                        throw new Exception(@"配置列表中存在新的配置名");
                    }

                    Program.UserConfig.AddProfile(tbProfileName.Text, new Profile(po));
                    Program.UserConfig.RemoveProfile(oldkey);
                    RefreshList();
                } else {
                    if (oldkey != null && Program.UserConfig.Profiles.ContainsKey(oldkey))
                        Program.UserConfig.Profiles[oldkey] = new Profile(po);
                }
                Program.UserConfig.SaveToFile();
            } catch (Exception ex) {
                MessageBox.Show(this, ex.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProfileManageWindow_FormClosing(object sender, FormClosingEventArgs e) {
            foreach (var profile in Program.UserConfig.Profiles) {
                if (!profile.Value.SelfCheck()) {
                    MessageBox.Show(this, $@"配置{profile.Key}有错误，请检查。", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void bRemoveProfile_Click(object sender, EventArgs e) {
            if (lsProfiles.SelectedItem != null) {
                if (MessageBox.Show(this, $@"是否删除配置{lsProfiles.SelectedItem}？", @"确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.Yes) {
                    Program.UserConfig.RemoveProfile(lsProfiles.SelectedItem as string);
                    RefreshList();
                    Program.UserConfig.SaveToFile();
                }
            }
        }

        private void bDuplicateProfile_Click(object sender, EventArgs e) {
            var target = lsProfiles.SelectedItem as string;
            if (!string.IsNullOrEmpty(target)) {
                string key = GenerateNewProfileName();
                Program.UserConfig.AddProfile(key, Program.UserConfig.Profiles[target].Clone());
                RefreshList();
            }
        }

        private void bCancel_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
