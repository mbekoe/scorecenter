namespace MediaPortal.Plugin.ScoreCenter.Editor
{
    partial class FussballdeScoreEditor
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbxUrl = new System.Windows.Forms.TextBox();
            this.lblUrl = new System.Windows.Forms.Label();
            this.tbxScoreId = new System.Windows.Forms.TextBox();
            this.lblScoreId = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.tbxName = new System.Windows.Forms.TextBox();
            this.cbxDetailsHelper = new System.Windows.Forms.ComboBox();
            this.lblDetails = new System.Windows.Forms.Label();
            this.tbxDetails = new System.Windows.Forms.TextBox();
            this.lblLevels = new System.Windows.Forms.Label();
            this.tbxLevels = new System.Windows.Forms.TextBox();
            this.btnOpenUrl = new System.Windows.Forms.Button();
            this.btnIcon = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblHighlights = new System.Windows.Forms.Label();
            this.tbxHighlights = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbxUrl
            // 
            this.tbxUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxUrl.Location = new System.Drawing.Point(57, 67);
            this.tbxUrl.MaxLength = 300;
            this.tbxUrl.Name = "tbxUrl";
            this.tbxUrl.Size = new System.Drawing.Size(405, 20);
            this.tbxUrl.TabIndex = 5;
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(22, 70);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(29, 13);
            this.lblUrl.TabIndex = 4;
            this.lblUrl.Text = "URL";
            // 
            // tbxScoreId
            // 
            this.tbxScoreId.HideSelection = false;
            this.tbxScoreId.Location = new System.Drawing.Point(57, 15);
            this.tbxScoreId.Name = "tbxScoreId";
            this.tbxScoreId.ReadOnly = true;
            this.tbxScoreId.Size = new System.Drawing.Size(301, 20);
            this.tbxScoreId.TabIndex = 1;
            // 
            // lblScoreId
            // 
            this.lblScoreId.AutoSize = true;
            this.lblScoreId.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblScoreId.Location = new System.Drawing.Point(35, 18);
            this.lblScoreId.Name = "lblScoreId";
            this.lblScoreId.Size = new System.Drawing.Size(16, 13);
            this.lblScoreId.TabIndex = 0;
            this.lblScoreId.Text = "Id";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(16, 44);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name";
            // 
            // tbxName
            // 
            this.tbxName.Location = new System.Drawing.Point(57, 41);
            this.tbxName.MaxLength = 30;
            this.tbxName.Name = "tbxName";
            this.tbxName.Size = new System.Drawing.Size(229, 20);
            this.tbxName.TabIndex = 3;
            // 
            // cbxDetailsHelper
            // 
            this.cbxDetailsHelper.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDetailsHelper.FormattingEnabled = true;
            this.cbxDetailsHelper.Items.AddRange(new object[] {
            "runde",
            "fairness",
            "torjaeger"});
            this.cbxDetailsHelper.Location = new System.Drawing.Point(394, 92);
            this.cbxDetailsHelper.Name = "cbxDetailsHelper";
            this.cbxDetailsHelper.Size = new System.Drawing.Size(112, 21);
            this.cbxDetailsHelper.TabIndex = 9;
            this.cbxDetailsHelper.SelectedIndexChanged += new System.EventHandler(this.cbxDetailsHelper_SelectedIndexChanged);
            // 
            // lblDetails
            // 
            this.lblDetails.AutoSize = true;
            this.lblDetails.Location = new System.Drawing.Point(12, 96);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(39, 13);
            this.lblDetails.TabIndex = 7;
            this.lblDetails.Text = "Details";
            // 
            // tbxDetails
            // 
            this.tbxDetails.Location = new System.Drawing.Point(57, 93);
            this.tbxDetails.MaxLength = 100;
            this.tbxDetails.Name = "tbxDetails";
            this.tbxDetails.Size = new System.Drawing.Size(331, 20);
            this.tbxDetails.TabIndex = 8;
            // 
            // lblLevels
            // 
            this.lblLevels.AutoSize = true;
            this.lblLevels.Location = new System.Drawing.Point(13, 122);
            this.lblLevels.Name = "lblLevels";
            this.lblLevels.Size = new System.Drawing.Size(38, 13);
            this.lblLevels.TabIndex = 10;
            this.lblLevels.Text = "Levels";
            // 
            // tbxLevels
            // 
            this.tbxLevels.Location = new System.Drawing.Point(57, 119);
            this.tbxLevels.MaxLength = 100;
            this.tbxLevels.Name = "tbxLevels";
            this.tbxLevels.Size = new System.Drawing.Size(331, 20);
            this.tbxLevels.TabIndex = 11;
            // 
            // btnOpenUrl
            // 
            this.btnOpenUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenUrl.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.Web;
            this.btnOpenUrl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOpenUrl.Location = new System.Drawing.Point(481, 64);
            this.btnOpenUrl.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.btnOpenUrl.Name = "btnOpenUrl";
            this.btnOpenUrl.Size = new System.Drawing.Size(25, 25);
            this.btnOpenUrl.TabIndex = 6;
            this.btnOpenUrl.UseVisualStyleBackColor = true;
            this.btnOpenUrl.Click += new System.EventHandler(this.btnOpenUrl_Click);
            // 
            // btnIcon
            // 
            this.btnIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIcon.Location = new System.Drawing.Point(429, 182);
            this.btnIcon.Name = "btnIcon";
            this.btnIcon.Size = new System.Drawing.Size(75, 23);
            this.btnIcon.TabIndex = 12;
            this.btnIcon.Text = "Icon";
            this.btnIcon.UseVisualStyleBackColor = true;
            this.btnIcon.Click += new System.EventHandler(this.btnIcon_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(429, 122);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(75, 62);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // lblHighlights
            // 
            this.lblHighlights.AutoSize = true;
            this.lblHighlights.Location = new System.Drawing.Point(-2, 148);
            this.lblHighlights.Name = "lblHighlights";
            this.lblHighlights.Size = new System.Drawing.Size(53, 13);
            this.lblHighlights.TabIndex = 24;
            this.lblHighlights.Text = "Highlights";
            // 
            // tbxHighlights
            // 
            this.tbxHighlights.Location = new System.Drawing.Point(57, 145);
            this.tbxHighlights.MaxLength = 100;
            this.tbxHighlights.Name = "tbxHighlights";
            this.tbxHighlights.Size = new System.Drawing.Size(331, 20);
            this.tbxHighlights.TabIndex = 25;
            // 
            // FussballdeScoreEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblHighlights);
            this.Controls.Add(this.tbxHighlights);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnIcon);
            this.Controls.Add(this.btnOpenUrl);
            this.Controls.Add(this.lblLevels);
            this.Controls.Add(this.tbxLevels);
            this.Controls.Add(this.cbxDetailsHelper);
            this.Controls.Add(this.lblDetails);
            this.Controls.Add(this.tbxDetails);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.tbxName);
            this.Controls.Add(this.tbxScoreId);
            this.Controls.Add(this.lblScoreId);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.tbxUrl);
            this.Name = "FussballdeScoreEditor";
            this.Size = new System.Drawing.Size(524, 205);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxUrl;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.TextBox tbxScoreId;
        private System.Windows.Forms.Label lblScoreId;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbxName;
        private System.Windows.Forms.ComboBox cbxDetailsHelper;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.TextBox tbxDetails;
        private System.Windows.Forms.Label lblLevels;
        private System.Windows.Forms.TextBox tbxLevels;
        private System.Windows.Forms.Button btnOpenUrl;
        private System.Windows.Forms.Button btnIcon;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblHighlights;
        private System.Windows.Forms.TextBox tbxHighlights;
    }
}
