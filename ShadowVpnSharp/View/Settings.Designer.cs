namespace ShadowVpnSharp.View {
    partial class Settings {
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
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.cbAllowAutoChooseFastestServer = new System.Windows.Forms.CheckBox();
            this.cbAllowAutoUpdate = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(13, 74);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 0;
            this.bCancel.Text = "取消";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.Location = new System.Drawing.Point(161, 73);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 1;
            this.bOK.Text = "确定";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // cbAllowAutoChooseFastestServer
            // 
            this.cbAllowAutoChooseFastestServer.AutoSize = true;
            this.cbAllowAutoChooseFastestServer.Location = new System.Drawing.Point(13, 13);
            this.cbAllowAutoChooseFastestServer.Name = "cbAllowAutoChooseFastestServer";
            this.cbAllowAutoChooseFastestServer.Size = new System.Drawing.Size(156, 16);
            this.cbAllowAutoChooseFastestServer.TabIndex = 2;
            this.cbAllowAutoChooseFastestServer.Text = "启动时自动选择最优线路";
            this.cbAllowAutoChooseFastestServer.UseVisualStyleBackColor = true;
            // 
            // cbAllowAutoUpdate
            // 
            this.cbAllowAutoUpdate.AutoSize = true;
            this.cbAllowAutoUpdate.Location = new System.Drawing.Point(13, 36);
            this.cbAllowAutoUpdate.Name = "cbAllowAutoUpdate";
            this.cbAllowAutoUpdate.Size = new System.Drawing.Size(96, 16);
            this.cbAllowAutoUpdate.TabIndex = 3;
            this.cbAllowAutoUpdate.Text = "自动检查更新";
            this.cbAllowAutoUpdate.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(248, 109);
            this.Controls.Add(this.cbAllowAutoUpdate);
            this.Controls.Add(this.cbAllowAutoChooseFastestServer);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "系统设置";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.CheckBox cbAllowAutoChooseFastestServer;
        private System.Windows.Forms.CheckBox cbAllowAutoUpdate;
    }
}