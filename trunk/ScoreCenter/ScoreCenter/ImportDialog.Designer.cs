namespace MediaPortal.Plugin.ScoreCenter
{
    partial class ImportDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportDialog));
            this.rbnNewOnly = new System.Windows.Forms.RadioButton();
            this.rbnMerge = new System.Windows.Forms.RadioButton();
            this.ckxNames = new System.Windows.Forms.CheckBox();
            this.ckxUrl = new System.Windows.Forms.CheckBox();
            this.ckxRules = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rbnNewOnly
            // 
            resources.ApplyResources(this.rbnNewOnly, "rbnNewOnly");
            this.rbnNewOnly.Name = "rbnNewOnly";
            this.rbnNewOnly.TabStop = true;
            this.rbnNewOnly.UseVisualStyleBackColor = true;
            this.rbnNewOnly.CheckedChanged += new System.EventHandler(this.rbn_CheckedChanged);
            // 
            // rbnMerge
            // 
            resources.ApplyResources(this.rbnMerge, "rbnMerge");
            this.rbnMerge.Name = "rbnMerge";
            this.rbnMerge.TabStop = true;
            this.rbnMerge.UseVisualStyleBackColor = true;
            this.rbnMerge.CheckedChanged += new System.EventHandler(this.rbn_CheckedChanged);
            // 
            // ckxNames
            // 
            resources.ApplyResources(this.ckxNames, "ckxNames");
            this.ckxNames.Name = "ckxNames";
            this.ckxNames.UseVisualStyleBackColor = true;
            // 
            // ckxUrl
            // 
            resources.ApplyResources(this.ckxUrl, "ckxUrl");
            this.ckxUrl.Name = "ckxUrl";
            this.ckxUrl.UseVisualStyleBackColor = true;
            // 
            // ckxRules
            // 
            resources.ApplyResources(this.ckxRules, "ckxRules");
            this.ckxRules.Name = "ckxRules";
            this.ckxRules.UseVisualStyleBackColor = true;
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
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ImportDialog
            // 
            this.AcceptButton = this.btnOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.ckxRules);
            this.Controls.Add(this.ckxUrl);
            this.Controls.Add(this.ckxNames);
            this.Controls.Add(this.rbnMerge);
            this.Controls.Add(this.rbnNewOnly);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ImportDialog_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbnNewOnly;
        private System.Windows.Forms.RadioButton rbnMerge;
        private System.Windows.Forms.CheckBox ckxNames;
        private System.Windows.Forms.CheckBox ckxUrl;
        private System.Windows.Forms.CheckBox ckxRules;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
    }
}