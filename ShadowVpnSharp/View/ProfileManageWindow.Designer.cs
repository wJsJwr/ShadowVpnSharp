namespace ShadowVpnSharp.View {
    partial class ProfileManageWindow {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfileManageWindow));
            this.lsProfiles = new System.Windows.Forms.ListBox();
            this.gbProfile = new System.Windows.Forms.GroupBox();
            this.tbUsertoken = new System.Windows.Forms.TextBox();
            this.lbUsertoken = new System.Windows.Forms.Label();
            this.cbNeedUsertoken = new System.Windows.Forms.CheckBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lbPassword = new System.Windows.Forms.Label();
            this.tbMtu = new System.Windows.Forms.TextBox();
            this.lbMtu = new System.Windows.Forms.Label();
            this.tbDns = new System.Windows.Forms.TextBox();
            this.lbDns = new System.Windows.Forms.Label();
            this.tbRemoteTunIp = new System.Windows.Forms.TextBox();
            this.lbRemoteTunIp = new System.Windows.Forms.Label();
            this.tbNetmask = new System.Windows.Forms.TextBox();
            this.lbNetmask = new System.Windows.Forms.Label();
            this.tbTunIp = new System.Windows.Forms.TextBox();
            this.lbTunIp = new System.Windows.Forms.Label();
            this.tbServerPort = new System.Windows.Forms.TextBox();
            this.lbServerPort = new System.Windows.Forms.Label();
            this.tbServerIp = new System.Windows.Forms.TextBox();
            this.lbServerIp = new System.Windows.Forms.Label();
            this.tbProfileName = new System.Windows.Forms.TextBox();
            this.lbProfileName = new System.Windows.Forms.Label();
            this.bAddProfile = new System.Windows.Forms.Button();
            this.bRemoveProfile = new System.Windows.Forms.Button();
            this.bDuplicateProfile = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOk = new System.Windows.Forms.Button();
            this.gbProfile.SuspendLayout();
            this.SuspendLayout();
            // 
            // lsProfiles
            // 
            this.lsProfiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsProfiles.FormattingEnabled = true;
            this.lsProfiles.ItemHeight = 17;
            this.lsProfiles.Location = new System.Drawing.Point(14, 17);
            this.lsProfiles.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lsProfiles.Name = "lsProfiles";
            this.lsProfiles.Size = new System.Drawing.Size(244, 395);
            this.lsProfiles.TabIndex = 0;
            this.lsProfiles.SelectedIndexChanged += new System.EventHandler(this.lsProfiles_SelectedIndexChanged);
            // 
            // gbProfile
            // 
            this.gbProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbProfile.Controls.Add(this.tbUsertoken);
            this.gbProfile.Controls.Add(this.lbUsertoken);
            this.gbProfile.Controls.Add(this.cbNeedUsertoken);
            this.gbProfile.Controls.Add(this.tbPassword);
            this.gbProfile.Controls.Add(this.lbPassword);
            this.gbProfile.Controls.Add(this.tbMtu);
            this.gbProfile.Controls.Add(this.lbMtu);
            this.gbProfile.Controls.Add(this.tbDns);
            this.gbProfile.Controls.Add(this.lbDns);
            this.gbProfile.Controls.Add(this.tbRemoteTunIp);
            this.gbProfile.Controls.Add(this.lbRemoteTunIp);
            this.gbProfile.Controls.Add(this.tbNetmask);
            this.gbProfile.Controls.Add(this.lbNetmask);
            this.gbProfile.Controls.Add(this.tbTunIp);
            this.gbProfile.Controls.Add(this.lbTunIp);
            this.gbProfile.Controls.Add(this.tbServerPort);
            this.gbProfile.Controls.Add(this.lbServerPort);
            this.gbProfile.Controls.Add(this.tbServerIp);
            this.gbProfile.Controls.Add(this.lbServerIp);
            this.gbProfile.Controls.Add(this.tbProfileName);
            this.gbProfile.Controls.Add(this.lbProfileName);
            this.gbProfile.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbProfile.Location = new System.Drawing.Point(266, 17);
            this.gbProfile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbProfile.Name = "gbProfile";
            this.gbProfile.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbProfile.Size = new System.Drawing.Size(279, 396);
            this.gbProfile.TabIndex = 1;
            this.gbProfile.TabStop = false;
            this.gbProfile.Text = "服务器配置";
            // 
            // tbUsertoken
            // 
            this.tbUsertoken.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUsertoken.Location = new System.Drawing.Point(107, 344);
            this.tbUsertoken.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbUsertoken.Name = "tbUsertoken";
            this.tbUsertoken.Size = new System.Drawing.Size(150, 23);
            this.tbUsertoken.TabIndex = 20;
            // 
            // lbUsertoken
            // 
            this.lbUsertoken.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbUsertoken.AutoSize = true;
            this.lbUsertoken.Location = new System.Drawing.Point(33, 347);
            this.lbUsertoken.Name = "lbUsertoken";
            this.lbUsertoken.Size = new System.Drawing.Size(68, 17);
            this.lbUsertoken.TabIndex = 19;
            this.lbUsertoken.Text = "Usertoken";
            this.lbUsertoken.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbNeedUsertoken
            // 
            this.cbNeedUsertoken.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbNeedUsertoken.AutoSize = true;
            this.cbNeedUsertoken.Location = new System.Drawing.Point(107, 316);
            this.cbNeedUsertoken.Name = "cbNeedUsertoken";
            this.cbNeedUsertoken.Size = new System.Drawing.Size(111, 21);
            this.cbNeedUsertoken.TabIndex = 18;
            this.cbNeedUsertoken.Text = "需要Usertoken";
            this.cbNeedUsertoken.UseVisualStyleBackColor = true;
            this.cbNeedUsertoken.CheckedChanged += new System.EventHandler(this.cbNeedUsertoken_CheckedChanged);
            // 
            // tbPassword
            // 
            this.tbPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPassword.Location = new System.Drawing.Point(107, 285);
            this.tbPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '·';
            this.tbPassword.Size = new System.Drawing.Size(150, 23);
            this.tbPassword.TabIndex = 17;
            // 
            // lbPassword
            // 
            this.lbPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPassword.AutoSize = true;
            this.lbPassword.Location = new System.Drawing.Point(69, 288);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(32, 17);
            this.lbPassword.TabIndex = 16;
            this.lbPassword.Text = "密码";
            this.lbPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbMtu
            // 
            this.tbMtu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMtu.Location = new System.Drawing.Point(107, 254);
            this.tbMtu.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbMtu.Name = "tbMtu";
            this.tbMtu.Size = new System.Drawing.Size(150, 23);
            this.tbMtu.TabIndex = 15;
            // 
            // lbMtu
            // 
            this.lbMtu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbMtu.AutoSize = true;
            this.lbMtu.Location = new System.Drawing.Point(65, 257);
            this.lbMtu.Name = "lbMtu";
            this.lbMtu.Size = new System.Drawing.Size(36, 17);
            this.lbMtu.TabIndex = 14;
            this.lbMtu.Text = "MTU";
            this.lbMtu.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDns
            // 
            this.tbDns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDns.Location = new System.Drawing.Point(107, 223);
            this.tbDns.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbDns.Name = "tbDns";
            this.tbDns.Size = new System.Drawing.Size(150, 23);
            this.tbDns.TabIndex = 13;
            // 
            // lbDns
            // 
            this.lbDns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbDns.AutoSize = true;
            this.lbDns.Location = new System.Drawing.Point(67, 226);
            this.lbDns.Name = "lbDns";
            this.lbDns.Size = new System.Drawing.Size(34, 17);
            this.lbDns.TabIndex = 12;
            this.lbDns.Text = "DNS";
            this.lbDns.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbRemoteTunIp
            // 
            this.tbRemoteTunIp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRemoteTunIp.Location = new System.Drawing.Point(107, 192);
            this.tbRemoteTunIp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbRemoteTunIp.Name = "tbRemoteTunIp";
            this.tbRemoteTunIp.Size = new System.Drawing.Size(150, 23);
            this.tbRemoteTunIp.TabIndex = 11;
            // 
            // lbRemoteTunIp
            // 
            this.lbRemoteTunIp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbRemoteTunIp.AutoSize = true;
            this.lbRemoteTunIp.Location = new System.Drawing.Point(58, 195);
            this.lbRemoteTunIp.Name = "lbRemoteTunIp";
            this.lbRemoteTunIp.Size = new System.Drawing.Size(43, 17);
            this.lbRemoteTunIp.TabIndex = 10;
            this.lbRemoteTunIp.Text = "远程IP";
            this.lbRemoteTunIp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbNetmask
            // 
            this.tbNetmask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNetmask.Location = new System.Drawing.Point(107, 161);
            this.tbNetmask.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbNetmask.Name = "tbNetmask";
            this.tbNetmask.Size = new System.Drawing.Size(150, 23);
            this.tbNetmask.TabIndex = 9;
            // 
            // lbNetmask
            // 
            this.lbNetmask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbNetmask.AutoSize = true;
            this.lbNetmask.Location = new System.Drawing.Point(45, 164);
            this.lbNetmask.Name = "lbNetmask";
            this.lbNetmask.Size = new System.Drawing.Size(56, 17);
            this.lbNetmask.TabIndex = 8;
            this.lbNetmask.Text = "本地掩码";
            this.lbNetmask.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbTunIp
            // 
            this.tbTunIp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTunIp.Location = new System.Drawing.Point(107, 130);
            this.tbTunIp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbTunIp.Name = "tbTunIp";
            this.tbTunIp.Size = new System.Drawing.Size(150, 23);
            this.tbTunIp.TabIndex = 7;
            // 
            // lbTunIp
            // 
            this.lbTunIp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTunIp.AutoSize = true;
            this.lbTunIp.Location = new System.Drawing.Point(58, 133);
            this.lbTunIp.Name = "lbTunIp";
            this.lbTunIp.Size = new System.Drawing.Size(43, 17);
            this.lbTunIp.TabIndex = 6;
            this.lbTunIp.Text = "本地IP";
            this.lbTunIp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbServerPort
            // 
            this.tbServerPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbServerPort.Location = new System.Drawing.Point(107, 99);
            this.tbServerPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbServerPort.Name = "tbServerPort";
            this.tbServerPort.Size = new System.Drawing.Size(150, 23);
            this.tbServerPort.TabIndex = 5;
            // 
            // lbServerPort
            // 
            this.lbServerPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbServerPort.AutoSize = true;
            this.lbServerPort.Location = new System.Drawing.Point(69, 102);
            this.lbServerPort.Name = "lbServerPort";
            this.lbServerPort.Size = new System.Drawing.Size(32, 17);
            this.lbServerPort.TabIndex = 4;
            this.lbServerPort.Text = "端口";
            this.lbServerPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbServerIp
            // 
            this.tbServerIp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbServerIp.Location = new System.Drawing.Point(107, 68);
            this.tbServerIp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbServerIp.Name = "tbServerIp";
            this.tbServerIp.Size = new System.Drawing.Size(150, 23);
            this.tbServerIp.TabIndex = 3;
            // 
            // lbServerIp
            // 
            this.lbServerIp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbServerIp.AutoSize = true;
            this.lbServerIp.Location = new System.Drawing.Point(46, 71);
            this.lbServerIp.Name = "lbServerIp";
            this.lbServerIp.Size = new System.Drawing.Size(55, 17);
            this.lbServerIp.TabIndex = 2;
            this.lbServerIp.Text = "服务器IP";
            this.lbServerIp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbProfileName
            // 
            this.tbProfileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbProfileName.Location = new System.Drawing.Point(107, 37);
            this.tbProfileName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbProfileName.Name = "tbProfileName";
            this.tbProfileName.Size = new System.Drawing.Size(150, 23);
            this.tbProfileName.TabIndex = 1;
            // 
            // lbProfileName
            // 
            this.lbProfileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbProfileName.AutoSize = true;
            this.lbProfileName.Location = new System.Drawing.Point(57, 40);
            this.lbProfileName.Name = "lbProfileName";
            this.lbProfileName.Size = new System.Drawing.Size(44, 17);
            this.lbProfileName.TabIndex = 0;
            this.lbProfileName.Text = "配置名";
            this.lbProfileName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bAddProfile
            // 
            this.bAddProfile.Location = new System.Drawing.Point(14, 419);
            this.bAddProfile.Name = "bAddProfile";
            this.bAddProfile.Size = new System.Drawing.Size(75, 25);
            this.bAddProfile.TabIndex = 2;
            this.bAddProfile.Text = "添加";
            this.bAddProfile.UseVisualStyleBackColor = true;
            this.bAddProfile.Click += new System.EventHandler(this.bAddProfile_Click);
            // 
            // bRemoveProfile
            // 
            this.bRemoveProfile.Location = new System.Drawing.Point(98, 419);
            this.bRemoveProfile.Name = "bRemoveProfile";
            this.bRemoveProfile.Size = new System.Drawing.Size(75, 25);
            this.bRemoveProfile.TabIndex = 3;
            this.bRemoveProfile.Text = "删除";
            this.bRemoveProfile.UseVisualStyleBackColor = true;
            this.bRemoveProfile.Click += new System.EventHandler(this.bRemoveProfile_Click);
            // 
            // bDuplicateProfile
            // 
            this.bDuplicateProfile.Location = new System.Drawing.Point(182, 419);
            this.bDuplicateProfile.Name = "bDuplicateProfile";
            this.bDuplicateProfile.Size = new System.Drawing.Size(75, 25);
            this.bDuplicateProfile.TabIndex = 4;
            this.bDuplicateProfile.Text = "复制";
            this.bDuplicateProfile.UseVisualStyleBackColor = true;
            this.bDuplicateProfile.Click += new System.EventHandler(this.bDuplicateProfile_Click);
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(470, 420);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 25);
            this.bCancel.TabIndex = 6;
            this.bCancel.Text = "关闭";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(386, 420);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 25);
            this.bOk.TabIndex = 5;
            this.bOk.Text = "保存";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // ProfileManageWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 456);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.bDuplicateProfile);
            this.Controls.Add(this.bRemoveProfile);
            this.Controls.Add(this.bAddProfile);
            this.Controls.Add(this.gbProfile);
            this.Controls.Add(this.lsProfiles);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProfileManageWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "配置管理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProfileManageWindow_FormClosing);
            this.Load += new System.EventHandler(this.ProfileManageWindow_Load);
            this.gbProfile.ResumeLayout(false);
            this.gbProfile.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lsProfiles;
        private System.Windows.Forms.GroupBox gbProfile;
        private System.Windows.Forms.TextBox tbNetmask;
        private System.Windows.Forms.Label lbNetmask;
        private System.Windows.Forms.TextBox tbTunIp;
        private System.Windows.Forms.Label lbTunIp;
        private System.Windows.Forms.TextBox tbServerPort;
        private System.Windows.Forms.Label lbServerPort;
        private System.Windows.Forms.TextBox tbServerIp;
        private System.Windows.Forms.Label lbServerIp;
        private System.Windows.Forms.TextBox tbProfileName;
        private System.Windows.Forms.Label lbProfileName;
        private System.Windows.Forms.TextBox tbRemoteTunIp;
        private System.Windows.Forms.Label lbRemoteTunIp;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lbPassword;
        private System.Windows.Forms.TextBox tbMtu;
        private System.Windows.Forms.Label lbMtu;
        private System.Windows.Forms.TextBox tbDns;
        private System.Windows.Forms.Label lbDns;
        private System.Windows.Forms.CheckBox cbNeedUsertoken;
        private System.Windows.Forms.TextBox tbUsertoken;
        private System.Windows.Forms.Label lbUsertoken;
        private System.Windows.Forms.Button bAddProfile;
        private System.Windows.Forms.Button bRemoveProfile;
        private System.Windows.Forms.Button bDuplicateProfile;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOk;
    }
}