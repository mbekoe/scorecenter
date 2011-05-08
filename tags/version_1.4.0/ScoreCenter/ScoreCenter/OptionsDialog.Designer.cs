namespace MediaPortal.Plugin.ScoreCenter
{
    partial class OptionsDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsDialog));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbxName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxBackdrop = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelectDir = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnUpdateNow = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ckxRules = new System.Windows.Forms.CheckBox();
            this.ckxUrl = new System.Windows.Forms.CheckBox();
            this.ckxNew = new System.Windows.Forms.CheckBox();
            this.ckxNames = new System.Windows.Forms.CheckBox();
            this.ckxMergeExisting = new System.Windows.Forms.CheckBox();
            this.tbxUrl = new System.Windows.Forms.TextBox();
            this.ckxOverwriteIcons = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // tbxName
            // 
            resources.ApplyResources(this.tbxName, "tbxName");
            this.tbxName.Name = "tbxName";
            // 
            // lblName
            // 
            resources.ApplyResources(this.lblName, "lblName");
            this.lblName.Name = "lblName";
            // 
            // numericUpDown1
            // 
            resources.ApplyResources(this.numericUpDown1, "numericUpDown1");
            this.numericUpDown1.Name = "numericUpDown1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbxBackdrop
            // 
            resources.ApplyResources(this.tbxBackdrop, "tbxBackdrop");
            this.tbxBackdrop.Name = "tbxBackdrop";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btnSelectDir
            // 
            this.btnSelectDir.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.openfolderHS;
            resources.ApplyResources(this.btnSelectDir, "btnSelectDir");
            this.btnSelectDir.Name = "btnSelectDir";
            this.btnSelectDir.UseVisualStyleBackColor = true;
            this.btnSelectDir.Click += new System.EventHandler(this.btnSelectDir_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.tbxName);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnSelectDir);
            this.groupBox1.Controls.Add(this.lblName);
            this.groupBox1.Controls.Add(this.tbxBackdrop);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.btnUpdateNow);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.ckxOverwriteIcons);
            this.groupBox2.Controls.Add(this.ckxRules);
            this.groupBox2.Controls.Add(this.ckxUrl);
            this.groupBox2.Controls.Add(this.ckxNew);
            this.groupBox2.Controls.Add(this.ckxNames);
            this.groupBox2.Controls.Add(this.ckxMergeExisting);
            this.groupBox2.Controls.Add(this.tbxUrl);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.Name = "comboBox1";
            // 
            // btnUpdateNow
            // 
            resources.ApplyResources(this.btnUpdateNow, "btnUpdateNow");
            this.btnUpdateNow.Name = "btnUpdateNow";
            this.btnUpdateNow.UseVisualStyleBackColor = true;
            this.btnUpdateNow.Click += new System.EventHandler(this.btnUpdateNow_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // ckxRules
            // 
            resources.ApplyResources(this.ckxRules, "ckxRules");
            this.ckxRules.Name = "ckxRules";
            this.ckxRules.UseVisualStyleBackColor = true;
            // 
            // ckxUrl
            // 
            resources.ApplyResources(this.ckxUrl, "ckxUrl");
            this.ckxUrl.Name = "ckxUrl";
            this.ckxUrl.UseVisualStyleBackColor = true;
            // 
            // ckxNew
            // 
            resources.ApplyResources(this.ckxNew, "ckxNew");
            this.ckxNew.Name = "ckxNew";
            this.ckxNew.UseVisualStyleBackColor = true;
            // 
            // ckxNames
            // 
            resources.ApplyResources(this.ckxNames, "ckxNames");
            this.ckxNames.Name = "ckxNames";
            this.ckxNames.UseVisualStyleBackColor = true;
            // 
            // ckxMergeExisting
            // 
            resources.ApplyResources(this.ckxMergeExisting, "ckxMergeExisting");
            this.ckxMergeExisting.Name = "ckxMergeExisting";
            this.ckxMergeExisting.UseVisualStyleBackColor = true;
            // 
            // tbxUrl
            // 
            resources.ApplyResources(this.tbxUrl, "tbxUrl");
            this.tbxUrl.Name = "tbxUrl";
            // 
            // ckxOverwriteIcons
            // 
            resources.ApplyResources(this.ckxOverwriteIcons, "ckxOverwriteIcons");
            this.ckxOverwriteIcons.Name = "ckxOverwriteIcons";
            this.ckxOverwriteIcons.UseVisualStyleBackColor = true;
            // 
            // OptionsDialog
            // 
            this.AcceptButton = this.btnOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.OptionsDialog_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsDialog_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbxName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxBackdrop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelectDir;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox ckxRules;
        private System.Windows.Forms.CheckBox ckxUrl;
        private System.Windows.Forms.CheckBox ckxNew;
        private System.Windows.Forms.CheckBox ckxNames;
        private System.Windows.Forms.CheckBox ckxMergeExisting;
        private System.Windows.Forms.TextBox tbxUrl;
        private System.Windows.Forms.Button btnUpdateNow;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox ckxOverwriteIcons;
    }
}