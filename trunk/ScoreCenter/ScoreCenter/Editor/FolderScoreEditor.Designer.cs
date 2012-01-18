namespace MediaPortal.Plugin.ScoreCenter.Editor
{
    partial class FolderScoreEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbxScore = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblId = new System.Windows.Forms.Label();
            this.tbxScoreId = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbxScore
            // 
            this.tbxScore.Location = new System.Drawing.Point(64, 38);
            this.tbxScore.MaxLength = 50;
            this.tbxScore.Name = "tbxScore";
            this.tbxScore.Size = new System.Drawing.Size(461, 20);
            this.tbxScore.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblName.Location = new System.Drawing.Point(22, 41);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name";
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblId.Location = new System.Drawing.Point(41, 15);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(16, 13);
            this.lblId.TabIndex = 0;
            this.lblId.Text = "Id";
            // 
            // tbxScoreId
            // 
            this.tbxScoreId.HideSelection = false;
            this.tbxScoreId.Location = new System.Drawing.Point(64, 12);
            this.tbxScoreId.Name = "tbxScoreId";
            this.tbxScoreId.ReadOnly = true;
            this.tbxScoreId.Size = new System.Drawing.Size(301, 20);
            this.tbxScoreId.TabIndex = 1;
            // 
            // FolderScoreEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbxScoreId);
            this.Controls.Add(this.lblId);
            this.Controls.Add(this.tbxScore);
            this.Controls.Add(this.lblName);
            this.Name = "FolderScoreEditor";
            this.Size = new System.Drawing.Size(632, 150);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxScore;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.TextBox tbxScoreId;
    }
}
