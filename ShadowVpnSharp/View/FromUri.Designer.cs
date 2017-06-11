namespace ShadowVpnSharp.View {
    partial class FromUri {
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
            this.lbFor_tbUri = new System.Windows.Forms.Label();
            this.tbUri = new System.Windows.Forms.TextBox();
            this.lbErr = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbProfileName = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbFor_tbUri
            // 
            this.lbFor_tbUri.AutoSize = true;
            this.lbFor_tbUri.Location = new System.Drawing.Point(12, 9);
            this.lbFor_tbUri.Name = "lbFor_tbUri";
            this.lbFor_tbUri.Size = new System.Drawing.Size(155, 12);
            this.lbFor_tbUri.TabIndex = 0;
            this.lbFor_tbUri.Text = "将URI粘贴至下面的文本框中";
            // 
            // tbUri
            // 
            this.tbUri.Location = new System.Drawing.Point(14, 24);
            this.tbUri.Name = "tbUri";
            this.tbUri.Size = new System.Drawing.Size(510, 21);
            this.tbUri.TabIndex = 1;
            this.tbUri.TextChanged += new System.EventHandler(this.tbUri_TextChanged);
            // 
            // lbErr
            // 
            this.lbErr.ForeColor = System.Drawing.Color.Red;
            this.lbErr.Location = new System.Drawing.Point(12, 48);
            this.lbErr.Name = "lbErr";
            this.lbErr.Size = new System.Drawing.Size(512, 14);
            this.lbErr.TabIndex = 2;
            this.lbErr.Text = "URI不合法";
            this.lbErr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbErr.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "配置名称:";
            // 
            // tbProfileName
            // 
            this.tbProfileName.Location = new System.Drawing.Point(77, 65);
            this.tbProfileName.Name = "tbProfileName";
            this.tbProfileName.Size = new System.Drawing.Size(447, 21);
            this.tbProfileName.TabIndex = 4;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(449, 91);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(368, 92);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FromUri
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 126);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.tbProfileName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbErr);
            this.Controls.Add(this.tbUri);
            this.Controls.Add(this.lbFor_tbUri);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FromUri";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "从URI创建配置";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FromUri_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbFor_tbUri;
        private System.Windows.Forms.TextBox tbUri;
        private System.Windows.Forms.Label lbErr;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbProfileName;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCancel;
    }
}