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
            this.numCacheExpiration = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxBackdrop = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelectDir = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnUpdateNow = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ckxOverwriteIcons = new System.Windows.Forms.CheckBox();
            this.ckxRules = new System.Windows.Forms.CheckBox();
            this.ckxUrl = new System.Windows.Forms.CheckBox();
            this.ckxNew = new System.Windows.Forms.CheckBox();
            this.ckxNames = new System.Windows.Forms.CheckBox();
            this.ckxMergeExisting = new System.Windows.Forms.CheckBox();
            this.tbxUrl = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColumnPName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gbxLive = new System.Windows.Forms.GroupBox();
            this.ckxPlaySound = new System.Windows.Forms.CheckBox();
            this.lblCheckUnit = new System.Windows.Forms.Label();
            this.lblNotifUnit = new System.Windows.Forms.Label();
            this.numCheckDelay = new System.Windows.Forms.NumericUpDown();
            this.numNotificationTime = new System.Windows.Forms.NumericUpDown();
            this.lblCheckDelay = new System.Windows.Forms.Label();
            this.lblNotificationTime = new System.Windows.Forms.Label();
            this.gbxGeneral = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.numCacheExpiration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.gbxLive.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCheckDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNotificationTime)).BeginInit();
            this.gbxGeneral.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
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
            // numCacheExpiration
            // 
            resources.ApplyResources(this.numCacheExpiration, "numCacheExpiration");
            this.numCacheExpiration.Name = "numCacheExpiration";
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
            // ckxOverwriteIcons
            // 
            resources.ApplyResources(this.ckxOverwriteIcons, "ckxOverwriteIcons");
            this.ckxOverwriteIcons.Name = "ckxOverwriteIcons";
            this.ckxOverwriteIcons.UseVisualStyleBackColor = true;
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
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnPName,
            this.ColumnPValue});
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.Name = "dataGridView1";
            // 
            // ColumnPName
            // 
            this.ColumnPName.DataPropertyName = "Name";
            resources.ApplyResources(this.ColumnPName, "ColumnPName");
            this.ColumnPName.Name = "ColumnPName";
            // 
            // ColumnPValue
            // 
            this.ColumnPValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnPValue.DataPropertyName = "Value";
            resources.ApplyResources(this.ColumnPValue, "ColumnPValue");
            this.ColumnPValue.Name = "ColumnPValue";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gbxLive);
            this.tabPage1.Controls.Add(this.gbxGeneral);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // gbxLive
            // 
            this.gbxLive.Controls.Add(this.ckxPlaySound);
            this.gbxLive.Controls.Add(this.lblCheckUnit);
            this.gbxLive.Controls.Add(this.lblNotifUnit);
            this.gbxLive.Controls.Add(this.numCheckDelay);
            this.gbxLive.Controls.Add(this.numNotificationTime);
            this.gbxLive.Controls.Add(this.lblCheckDelay);
            this.gbxLive.Controls.Add(this.lblNotificationTime);
            resources.ApplyResources(this.gbxLive, "gbxLive");
            this.gbxLive.Name = "gbxLive";
            this.gbxLive.TabStop = false;
            // 
            // ckxPlaySound
            // 
            resources.ApplyResources(this.ckxPlaySound, "ckxPlaySound");
            this.ckxPlaySound.Name = "ckxPlaySound";
            this.ckxPlaySound.UseVisualStyleBackColor = true;
            // 
            // lblCheckUnit
            // 
            resources.ApplyResources(this.lblCheckUnit, "lblCheckUnit");
            this.lblCheckUnit.Name = "lblCheckUnit";
            // 
            // lblNotifUnit
            // 
            resources.ApplyResources(this.lblNotifUnit, "lblNotifUnit");
            this.lblNotifUnit.Name = "lblNotifUnit";
            // 
            // numCheckDelay
            // 
            resources.ApplyResources(this.numCheckDelay, "numCheckDelay");
            this.numCheckDelay.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numCheckDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCheckDelay.Name = "numCheckDelay";
            this.numCheckDelay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numNotificationTime
            // 
            resources.ApplyResources(this.numNotificationTime, "numNotificationTime");
            this.numNotificationTime.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numNotificationTime.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numNotificationTime.Name = "numNotificationTime";
            this.numNotificationTime.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // lblCheckDelay
            // 
            resources.ApplyResources(this.lblCheckDelay, "lblCheckDelay");
            this.lblCheckDelay.Name = "lblCheckDelay";
            // 
            // lblNotificationTime
            // 
            resources.ApplyResources(this.lblNotificationTime, "lblNotificationTime");
            this.lblNotificationTime.Name = "lblNotificationTime";
            // 
            // gbxGeneral
            // 
            this.gbxGeneral.Controls.Add(this.label5);
            this.gbxGeneral.Controls.Add(this.lblName);
            this.gbxGeneral.Controls.Add(this.tbxName);
            this.gbxGeneral.Controls.Add(this.tbxBackdrop);
            this.gbxGeneral.Controls.Add(this.numCacheExpiration);
            this.gbxGeneral.Controls.Add(this.btnSelectDir);
            this.gbxGeneral.Controls.Add(this.label2);
            this.gbxGeneral.Controls.Add(this.label1);
            resources.ApplyResources(this.gbxGeneral, "gbxGeneral");
            this.gbxGeneral.Name = "gbxGeneral";
            this.gbxGeneral.TabStop = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.comboBox1);
            this.tabPage2.Controls.Add(this.btnUpdateNow);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.tbxUrl);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.ckxMergeExisting);
            this.tabPage2.Controls.Add(this.ckxOverwriteIcons);
            this.tabPage2.Controls.Add(this.ckxNames);
            this.tabPage2.Controls.Add(this.ckxRules);
            this.tabPage2.Controls.Add(this.ckxNew);
            this.tabPage2.Controls.Add(this.ckxUrl);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGridView1);
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // OptionsDialog
            // 
            this.AcceptButton = this.btnOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.tabControl1);
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
            ((System.ComponentModel.ISupportInitialize)(this.numCacheExpiration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.gbxLive.ResumeLayout(false);
            this.gbxLive.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCheckDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNotificationTime)).EndInit();
            this.gbxGeneral.ResumeLayout(false);
            this.gbxGeneral.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbxName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.NumericUpDown numCacheExpiration;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxBackdrop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelectDir;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
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
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPValue;
        private System.Windows.Forms.GroupBox gbxGeneral;
        private System.Windows.Forms.GroupBox gbxLive;
        private System.Windows.Forms.NumericUpDown numCheckDelay;
        private System.Windows.Forms.NumericUpDown numNotificationTime;
        private System.Windows.Forms.Label lblCheckDelay;
        private System.Windows.Forms.Label lblNotificationTime;
        private System.Windows.Forms.Label lblCheckUnit;
        private System.Windows.Forms.Label lblNotifUnit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox ckxPlaySound;
    }
}