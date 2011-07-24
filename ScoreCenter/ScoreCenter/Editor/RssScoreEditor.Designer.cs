namespace MediaPortal.Plugin.ScoreCenter.Editor
{
    partial class RssScoreEditor
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
            this.tbxEncoding = new System.Windows.Forms.TextBox();
            this.btnOpenUrl = new System.Windows.Forms.Button();
            this.tbxScore = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tbxUrl = new System.Windows.Forms.TextBox();
            this.lblEncoding = new System.Windows.Forms.Label();
            this.lblUrl = new System.Windows.Forms.Label();
            this.tbxScoreId = new System.Windows.Forms.TextBox();
            this.lblScoreId = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbxEncoding
            // 
            this.tbxEncoding.Location = new System.Drawing.Point(54, 81);
            this.tbxEncoding.MaxLength = 20;
            this.tbxEncoding.Name = "tbxEncoding";
            this.tbxEncoding.Size = new System.Drawing.Size(119, 20);
            this.tbxEncoding.TabIndex = 8;
            // 
            // btnOpenUrl
            // 
            this.btnOpenUrl.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.Web;
            this.btnOpenUrl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOpenUrl.Location = new System.Drawing.Point(515, 53);
            this.btnOpenUrl.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.btnOpenUrl.Name = "btnOpenUrl";
            this.btnOpenUrl.Size = new System.Drawing.Size(25, 25);
            this.btnOpenUrl.TabIndex = 6;
            this.btnOpenUrl.UseVisualStyleBackColor = true;
            this.btnOpenUrl.Click += new System.EventHandler(this.btnOpenUrl_Click);
            // 
            // tbxScore
            // 
            this.tbxScore.Location = new System.Drawing.Point(54, 29);
            this.tbxScore.MaxLength = 50;
            this.tbxScore.Name = "tbxScore";
            this.tbxScore.Size = new System.Drawing.Size(461, 20);
            this.tbxScore.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblName.Location = new System.Drawing.Point(13, 32);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name";
            // 
            // tbxUrl
            // 
            this.tbxUrl.Location = new System.Drawing.Point(53, 55);
            this.tbxUrl.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.tbxUrl.MaxLength = 200;
            this.tbxUrl.Name = "tbxUrl";
            this.tbxUrl.Size = new System.Drawing.Size(462, 20);
            this.tbxUrl.TabIndex = 5;
            // 
            // lblEncoding
            // 
            this.lblEncoding.AutoSize = true;
            this.lblEncoding.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblEncoding.Location = new System.Drawing.Point(1, 84);
            this.lblEncoding.Name = "lblEncoding";
            this.lblEncoding.Size = new System.Drawing.Size(52, 13);
            this.lblEncoding.TabIndex = 7;
            this.lblEncoding.Text = "Encoding";
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblUrl.Location = new System.Drawing.Point(19, 58);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(29, 13);
            this.lblUrl.TabIndex = 4;
            this.lblUrl.Text = "URL";
            // 
            // tbxScoreId
            // 
            this.tbxScoreId.HideSelection = false;
            this.tbxScoreId.Location = new System.Drawing.Point(54, 3);
            this.tbxScoreId.Name = "tbxScoreId";
            this.tbxScoreId.ReadOnly = true;
            this.tbxScoreId.Size = new System.Drawing.Size(301, 20);
            this.tbxScoreId.TabIndex = 1;
            // 
            // lblScoreId
            // 
            this.lblScoreId.AutoSize = true;
            this.lblScoreId.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblScoreId.Location = new System.Drawing.Point(32, 6);
            this.lblScoreId.Name = "lblScoreId";
            this.lblScoreId.Size = new System.Drawing.Size(16, 13);
            this.lblScoreId.TabIndex = 0;
            this.lblScoreId.Text = "Id";
            // 
            // RssScoreEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblScoreId);
            this.Controls.Add(this.tbxScoreId);
            this.Controls.Add(this.tbxEncoding);
            this.Controls.Add(this.btnOpenUrl);
            this.Controls.Add(this.tbxScore);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.tbxUrl);
            this.Controls.Add(this.lblEncoding);
            this.Controls.Add(this.lblUrl);
            this.Name = "RssScoreEditor";
            this.Size = new System.Drawing.Size(584, 118);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxEncoding;
        private System.Windows.Forms.Button btnOpenUrl;
        private System.Windows.Forms.TextBox tbxScore;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbxUrl;
        private System.Windows.Forms.Label lblEncoding;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.TextBox tbxScoreId;
        private System.Windows.Forms.Label lblScoreId;
    }
}
