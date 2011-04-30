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
            this.SuspendLayout();
            // 
            // tbxScore
            // 
            this.tbxScore.Location = new System.Drawing.Point(66, 24);
            this.tbxScore.MaxLength = 50;
            this.tbxScore.Name = "tbxScore";
            this.tbxScore.Size = new System.Drawing.Size(461, 20);
            this.tbxScore.TabIndex = 44;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblName.Location = new System.Drawing.Point(24, 27);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 43;
            this.lblName.Text = "Name";
            // 
            // DefaultEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbxScore);
            this.Controls.Add(this.lblName);
            this.Name = "DefaultEditor";
            this.Size = new System.Drawing.Size(632, 150);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxScore;
        private System.Windows.Forms.Label lblName;
    }
}
