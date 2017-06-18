namespace ShadowVpnSharp.View {
    partial class EntryWindow {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntryWindow));
            this.EntryWindowMenuStrip = new System.Windows.Forms.MenuStrip();
            this.MenuItemProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemChooseProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.EmptyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemManageProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemQRCode = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemGenerateQRCode = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemRecognizeQRCode = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemUri = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemGenerateUri = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemImportFromUri = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemTools = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemChooseFastest = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemSystemTools = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemHardwareCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemResetTapDevice = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDiagnostic = new System.Windows.Forms.ToolStripMenuItem();
            this.lbCurrentProfileName = new System.Windows.Forms.Label();
            this.bVpnSwitch = new System.Windows.Forms.Button();
            this.StatTimer = new System.Windows.Forms.Timer(this.components);
            this.PicStat = new System.Windows.Forms.PictureBox();
            this.EntryWindowMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicStat)).BeginInit();
            this.SuspendLayout();
            // 
            // EntryWindowMenuStrip
            // 
            this.EntryWindowMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemProfile,
            this.MenuItemTools,
            this.MenuItemHelp});
            this.EntryWindowMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.EntryWindowMenuStrip.Name = "EntryWindowMenuStrip";
            this.EntryWindowMenuStrip.Size = new System.Drawing.Size(284, 25);
            this.EntryWindowMenuStrip.TabIndex = 0;
            this.EntryWindowMenuStrip.Text = "menuStrip1";
            // 
            // MenuItemProfile
            // 
            this.MenuItemProfile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemChooseProfile,
            this.MenuItemManageProfile,
            this.toolStripMenuItem1,
            this.MenuItemQRCode,
            this.MenuItemUri});
            this.MenuItemProfile.Name = "MenuItemProfile";
            this.MenuItemProfile.Size = new System.Drawing.Size(44, 21);
            this.MenuItemProfile.Text = "配置";
            // 
            // MenuItemChooseProfile
            // 
            this.MenuItemChooseProfile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EmptyToolStripMenuItem});
            this.MenuItemChooseProfile.Name = "MenuItemChooseProfile";
            this.MenuItemChooseProfile.Size = new System.Drawing.Size(124, 22);
            this.MenuItemChooseProfile.Text = "选择配置";
            // 
            // EmptyToolStripMenuItem
            // 
            this.EmptyToolStripMenuItem.Enabled = false;
            this.EmptyToolStripMenuItem.Name = "EmptyToolStripMenuItem";
            this.EmptyToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.EmptyToolStripMenuItem.Text = "（空）";
            // 
            // MenuItemManageProfile
            // 
            this.MenuItemManageProfile.Name = "MenuItemManageProfile";
            this.MenuItemManageProfile.Size = new System.Drawing.Size(124, 22);
            this.MenuItemManageProfile.Text = "编辑配置";
            this.MenuItemManageProfile.Click += new System.EventHandler(this.MenuItemManageProfile_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(121, 6);
            // 
            // MenuItemQRCode
            // 
            this.MenuItemQRCode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemGenerateQRCode,
            this.MenuItemRecognizeQRCode});
            this.MenuItemQRCode.Name = "MenuItemQRCode";
            this.MenuItemQRCode.Size = new System.Drawing.Size(124, 22);
            this.MenuItemQRCode.Text = "二维码";
            // 
            // MenuItemGenerateQRCode
            // 
            this.MenuItemGenerateQRCode.Name = "MenuItemGenerateQRCode";
            this.MenuItemGenerateQRCode.Size = new System.Drawing.Size(184, 22);
            this.MenuItemGenerateQRCode.Text = "生成二维码";
            this.MenuItemGenerateQRCode.Click += new System.EventHandler(this.MenuItemGenerateQRCode_Click);
            // 
            // MenuItemRecognizeQRCode
            // 
            this.MenuItemRecognizeQRCode.Name = "MenuItemRecognizeQRCode";
            this.MenuItemRecognizeQRCode.Size = new System.Drawing.Size(184, 22);
            this.MenuItemRecognizeQRCode.Text = "识别屏幕上的二维码";
            this.MenuItemRecognizeQRCode.Click += new System.EventHandler(this.MenuItemRecognizeQRCode_Click);
            // 
            // MenuItemUri
            // 
            this.MenuItemUri.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemGenerateUri,
            this.MenuItemImportFromUri});
            this.MenuItemUri.Name = "MenuItemUri";
            this.MenuItemUri.Size = new System.Drawing.Size(124, 22);
            this.MenuItemUri.Text = "URI";
            // 
            // MenuItemGenerateUri
            // 
            this.MenuItemGenerateUri.Name = "MenuItemGenerateUri";
            this.MenuItemGenerateUri.Size = new System.Drawing.Size(157, 22);
            this.MenuItemGenerateUri.Text = "生成URI";
            this.MenuItemGenerateUri.Click += new System.EventHandler(this.MenuItemGenerateUri_Click);
            // 
            // MenuItemImportFromUri
            // 
            this.MenuItemImportFromUri.Name = "MenuItemImportFromUri";
            this.MenuItemImportFromUri.Size = new System.Drawing.Size(157, 22);
            this.MenuItemImportFromUri.Text = "从URI导入配置";
            this.MenuItemImportFromUri.Click += new System.EventHandler(this.MenuItemImportFromUri_Click);
            // 
            // MenuItemTools
            // 
            this.MenuItemTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemChooseFastest,
            this.MenuItemSystemTools,
            this.toolStripMenuItem2});
            this.MenuItemTools.Name = "MenuItemTools";
            this.MenuItemTools.Size = new System.Drawing.Size(44, 21);
            this.MenuItemTools.Text = "工具";
            // 
            // MenuItemChooseFastest
            // 
            this.MenuItemChooseFastest.Name = "MenuItemChooseFastest";
            this.MenuItemChooseFastest.Size = new System.Drawing.Size(152, 22);
            this.MenuItemChooseFastest.Text = "选择最优路线";
            this.MenuItemChooseFastest.Click += new System.EventHandler(this.MenuItemChooseFastest_Click);
            // 
            // MenuItemSystemTools
            // 
            this.MenuItemSystemTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemHardwareCheck,
            this.MenuItemResetTapDevice});
            this.MenuItemSystemTools.Name = "MenuItemSystemTools";
            this.MenuItemSystemTools.Size = new System.Drawing.Size(152, 22);
            this.MenuItemSystemTools.Text = "系统工具";
            // 
            // MenuItemHardwareCheck
            // 
            this.MenuItemHardwareCheck.Name = "MenuItemHardwareCheck";
            this.MenuItemHardwareCheck.Size = new System.Drawing.Size(148, 22);
            this.MenuItemHardwareCheck.Text = "硬件检测";
            this.MenuItemHardwareCheck.Click += new System.EventHandler(this.MenuItemHardwareCheck_Click);
            // 
            // MenuItemResetTapDevice
            // 
            this.MenuItemResetTapDevice.Name = "MenuItemResetTapDevice";
            this.MenuItemResetTapDevice.Size = new System.Drawing.Size(148, 22);
            this.MenuItemResetTapDevice.Text = "重置硬件设备";
            this.MenuItemResetTapDevice.Click += new System.EventHandler(this.MenuItemResetTapDevice_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(149, 6);
            // 
            // MenuItemHelp
            // 
            this.MenuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemUpdate,
            this.MenuItemAbout,
            this.MenuItemDiagnostic});
            this.MenuItemHelp.Name = "MenuItemHelp";
            this.MenuItemHelp.Size = new System.Drawing.Size(44, 21);
            this.MenuItemHelp.Text = "帮助";
            // 
            // MenuItemUpdate
            // 
            this.MenuItemUpdate.Name = "MenuItemUpdate";
            this.MenuItemUpdate.Size = new System.Drawing.Size(124, 22);
            this.MenuItemUpdate.Text = "检查更新";
            this.MenuItemUpdate.Click += new System.EventHandler(this.MenuItemUpdate_Click);
            // 
            // MenuItemAbout
            // 
            this.MenuItemAbout.Name = "MenuItemAbout";
            this.MenuItemAbout.Size = new System.Drawing.Size(124, 22);
            this.MenuItemAbout.Text = "关于";
            this.MenuItemAbout.Click += new System.EventHandler(this.MenuItemAbout_Click);
            // 
            // MenuItemDiagnostic
            // 
            this.MenuItemDiagnostic.Name = "MenuItemDiagnostic";
            this.MenuItemDiagnostic.Size = new System.Drawing.Size(124, 22);
            this.MenuItemDiagnostic.Text = "系统诊断";
            this.MenuItemDiagnostic.Click += new System.EventHandler(this.MenuItemDiagnostic_Click);
            // 
            // lbCurrentProfileName
            // 
            this.lbCurrentProfileName.BackColor = System.Drawing.SystemColors.Control;
            this.lbCurrentProfileName.Cursor = System.Windows.Forms.Cursors.Default;
            this.lbCurrentProfileName.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbCurrentProfileName.ForeColor = System.Drawing.Color.White;
            this.lbCurrentProfileName.Location = new System.Drawing.Point(0, 77);
            this.lbCurrentProfileName.Name = "lbCurrentProfileName";
            this.lbCurrentProfileName.Size = new System.Drawing.Size(284, 28);
            this.lbCurrentProfileName.TabIndex = 2;
            this.lbCurrentProfileName.Text = "当前配置名";
            this.lbCurrentProfileName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bVpnSwitch
            // 
            this.bVpnSwitch.BackColor = System.Drawing.Color.Transparent;
            this.bVpnSwitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bVpnSwitch.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bVpnSwitch.ForeColor = System.Drawing.Color.White;
            this.bVpnSwitch.Location = new System.Drawing.Point(82, 180);
            this.bVpnSwitch.Name = "bVpnSwitch";
            this.bVpnSwitch.Size = new System.Drawing.Size(120, 53);
            this.bVpnSwitch.TabIndex = 3;
            this.bVpnSwitch.Text = "启动";
            this.bVpnSwitch.UseVisualStyleBackColor = false;
            this.bVpnSwitch.Click += new System.EventHandler(this.bVpnSwitch_Click);
            // 
            // StatTimer
            // 
            this.StatTimer.Interval = 1000;
            this.StatTimer.Tick += new System.EventHandler(this.StatTimer_Tick);
            // 
            // PicStat
            // 
            this.PicStat.BackColor = System.Drawing.Color.DodgerBlue;
            this.PicStat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PicStat.Location = new System.Drawing.Point(0, 0);
            this.PicStat.Name = "PicStat";
            this.PicStat.Size = new System.Drawing.Size(284, 261);
            this.PicStat.TabIndex = 1;
            this.PicStat.TabStop = false;
            this.PicStat.Click += new System.EventHandler(this.PicStat_Click);
            this.PicStat.MouseEnter += new System.EventHandler(this.PicStat_MouseEnter);
            this.PicStat.MouseLeave += new System.EventHandler(this.PicStat_MouseLeave);
            // 
            // EntryWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.bVpnSwitch);
            this.Controls.Add(this.lbCurrentProfileName);
            this.Controls.Add(this.EntryWindowMenuStrip);
            this.Controls.Add(this.PicStat);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.EntryWindowMenuStrip;
            this.MaximizeBox = false;
            this.Name = "EntryWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShadowVPNSharp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EntryWindow_FormClosing);
            this.Load += new System.EventHandler(this.EntryWindow_Load);
            this.EntryWindowMenuStrip.ResumeLayout(false);
            this.EntryWindowMenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicStat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip EntryWindowMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem MenuItemProfile;
        private System.Windows.Forms.ToolStripMenuItem MenuItemChooseProfile;
        private System.Windows.Forms.ToolStripMenuItem MenuItemManageProfile;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem MenuItemQRCode;
        private System.Windows.Forms.ToolStripMenuItem MenuItemGenerateQRCode;
        private System.Windows.Forms.ToolStripMenuItem MenuItemRecognizeQRCode;
        private System.Windows.Forms.ToolStripMenuItem MenuItemUri;
        private System.Windows.Forms.ToolStripMenuItem MenuItemTools;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSystemTools;
        private System.Windows.Forms.ToolStripMenuItem MenuItemHardwareCheck;
        private System.Windows.Forms.ToolStripMenuItem MenuItemResetTapDevice;
        private System.Windows.Forms.ToolStripMenuItem MenuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem MenuItemUpdate;
        private System.Windows.Forms.ToolStripMenuItem MenuItemAbout;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDiagnostic;
        private System.Windows.Forms.PictureBox PicStat;
        private System.Windows.Forms.Label lbCurrentProfileName;
        private System.Windows.Forms.Button bVpnSwitch;
        private System.Windows.Forms.ToolStripMenuItem EmptyToolStripMenuItem;
        private System.Windows.Forms.Timer StatTimer;
        private System.Windows.Forms.ToolStripMenuItem MenuItemGenerateUri;
        private System.Windows.Forms.ToolStripMenuItem MenuItemImportFromUri;
        private System.Windows.Forms.ToolStripMenuItem MenuItemChooseFastest;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    }
}