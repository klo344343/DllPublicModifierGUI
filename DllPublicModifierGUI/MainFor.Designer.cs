namespace DllPublicModifierGUI
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtDllPath = new System.Windows.Forms.TextBox();
            this.btnSelectDll = new System.Windows.Forms.Button();
            this.lstFields = new System.Windows.Forms.ListBox();
            this.btnModify = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtDllPath
            // 
            this.txtDllPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDllPath.Location = new System.Drawing.Point(12, 12);
            this.txtDllPath.Name = "txtDllPath";
            this.txtDllPath.ReadOnly = true;
            this.txtDllPath.Size = new System.Drawing.Size(400, 20);
            this.txtDllPath.TabIndex = 0;
            // 
            // btnSelectDll
            // 
            this.btnSelectDll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectDll.Location = new System.Drawing.Point(418, 10);
            this.btnSelectDll.Name = "btnSelectDll";
            this.btnSelectDll.Size = new System.Drawing.Size(100, 23);
            this.btnSelectDll.TabIndex = 1;
            this.btnSelectDll.Text = "Выбрать DLL";
            this.btnSelectDll.UseVisualStyleBackColor = true;
            this.btnSelectDll.Click += new System.EventHandler(this.btnSelectDll_Click);
            // 
            // lstFields
            // 
            this.lstFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFields.FormattingEnabled = true;
            this.lstFields.Location = new System.Drawing.Point(12, 38);
            this.lstFields.Name = "lstFields";
            this.lstFields.Size = new System.Drawing.Size(506, 199);
            this.lstFields.TabIndex = 2;
            // 
            // btnModify
            // 
            this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModify.Enabled = false;
            this.btnModify.Location = new System.Drawing.Point(418, 243);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(100, 23);
            this.btnModify.TabIndex = 3;
            this.btnModify.Text = "Сделать public";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 248);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(89, 13);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Выберите DLL...";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 278);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnModify);
            this.Controls.Add(this.lstFields);
            this.Controls.Add(this.btnSelectDll);
            this.Controls.Add(this.txtDllPath);
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "MainForm";
            this.Text = "DLL Public Fields Modifier";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}