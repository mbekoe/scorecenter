namespace MediaPortal.Plugin.ScoreCenter
{
    partial class AboutDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblVersionLabel = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lnkSite = new System.Windows.Forms.LinkLabel();
            this.lvwSummary = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Name = "lblTitle";
            // 
            // lblVersionLabel
            // 
            resources.ApplyResources(this.lblVersionLabel, "lblVersionLabel");
            this.lblVersionLabel.Name = "lblVersionLabel";
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblVersion
            // 
            resources.ApplyResources(this.lblVersion, "lblVersion");
            this.lblVersion.Name = "lblVersion";
            // 
            // lnkSite
            // 
            resources.ApplyResources(this.lnkSite, "lnkSite");
            this.lnkSite.Name = "lnkSite";
            this.lnkSite.TabStop = true;
            this.lnkSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSite_LinkClicked);
            // 
            // lvwSummary
            // 
            resources.ApplyResources(this.lvwSummary, "lvwSummary");
            this.lvwSummary.BackColor = System.Drawing.SystemColors.Control;
            this.lvwSummary.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvwSummary.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvwSummary.Name = "lvwSummary";
            this.lvwSummary.UseCompatibleStateImageBehavior = false;
            this.lvwSummary.View = System.Windows.Forms.View.Details;
            // 
            // AboutDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOk;
            this.Controls.Add(this.lvwSummary);
            this.Controls.Add(this.lnkSite);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblVersionLabel);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblVersionLabel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.LinkLabel lnkSite;
        private System.Windows.Forms.ListView lvwSummary;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}