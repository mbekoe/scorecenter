﻿namespace MediaPortal.Plugin.ScoreCenter.Editor
{
    partial class WorldFootballScoreEditor
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
            this.tbxName = new System.Windows.Forms.TextBox();
            this.tbxCountry = new System.Windows.Forms.TextBox();
            this.tbxLeague = new System.Windows.Forms.TextBox();
            this.tbxSeason = new System.Windows.Forms.TextBox();
            this.tbxLevels = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblCountry = new System.Windows.Forms.Label();
            this.lblLeague = new System.Windows.Forms.Label();
            this.lblSeason = new System.Windows.Forms.Label();
            this.btnOpenUrl = new System.Windows.Forms.Button();
            this.numNbTeams = new System.Windows.Forms.NumericUpDown();
            this.lblNbTeams = new System.Windows.Forms.Label();
            this.lblLevels = new System.Windows.Forms.Label();
            this.cbxKind = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbxHighlights = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnIcon = new System.Windows.Forms.Button();
            this.tbxDetails = new System.Windows.Forms.TextBox();
            this.lblDetails = new System.Windows.Forms.Label();
            this.lblScoreId = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNbTeams)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbxName
            // 
            this.tbxName.Location = new System.Drawing.Point(95, 21);
            this.tbxName.MaxLength = 30;
            this.tbxName.Name = "tbxName";
            this.tbxName.Size = new System.Drawing.Size(229, 20);
            this.tbxName.TabIndex = 1;
            // 
            // tbxCountry
            // 
            this.tbxCountry.AutoCompleteCustomSource.AddRange(new string[] {
            "alb",
            "alg",
            "and",
            "arg",
            "arm",
            "aus",
            "aut",
            "aze",
            "bhr",
            "blr",
            "bel",
            "bol",
            "bih",
            "bra",
            "bul",
            "can",
            "chin",
            "chn",
            "col",
            "crc",
            "cro",
            "cyp",
            "cze",
            "den",
            "ecu",
            "egy",
            "eng",
            "est",
            "fro",
            "fin",
            "fra",
            "gdr",
            "geo",
            "ger",
            "gha",
            "gre",
            "gua",
            "hon",
            "hkg",
            "hun",
            "isl",
            "ind",
            "idn",
            "irn",
            "irq",
            "irl",
            "isr",
            "ita",
            "jam",
            "jpn",
            "kaz",
            "lat",
            "lby",
            "ltu",
            "lux",
            "mkd",
            "mas",
            "mlt",
            "mex",
            "mda",
            "mne",
            "mar",
            "ned",
            "nzl",
            "nga",
            "nir",
            "nor",
            "par",
            "per",
            "pol",
            "por",
            "qat",
            "rom",
            "rus",
            "smr",
            "ksa",
            "sco",
            "srb",
            "sin",
            "svk",
            "svn",
            "rsa",
            "kor",
            "esp",
            "sud",
            "swe",
            "sui",
            "syr",
            "tha",
            "tun",
            "tur",
            "uae",
            "ukr",
            "uru",
            "usa",
            "uzb",
            "ven",
            "vie",
            "wal"});
            this.tbxCountry.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbxCountry.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbxCountry.Location = new System.Drawing.Point(95, 47);
            this.tbxCountry.MaxLength = 100;
            this.tbxCountry.Name = "tbxCountry";
            this.tbxCountry.Size = new System.Drawing.Size(141, 20);
            this.tbxCountry.TabIndex = 3;
            // 
            // tbxLeague
            // 
            this.tbxLeague.Location = new System.Drawing.Point(95, 73);
            this.tbxLeague.MaxLength = 100;
            this.tbxLeague.Name = "tbxLeague";
            this.tbxLeague.Size = new System.Drawing.Size(141, 20);
            this.tbxLeague.TabIndex = 5;
            // 
            // tbxSeason
            // 
            this.tbxSeason.AutoCompleteCustomSource.AddRange(new string[] {
            "{yyyy}",
            "{Y-Y+1}"});
            this.tbxSeason.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbxSeason.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbxSeason.Location = new System.Drawing.Point(95, 99);
            this.tbxSeason.MaxLength = 20;
            this.tbxSeason.Name = "tbxSeason";
            this.tbxSeason.Size = new System.Drawing.Size(141, 20);
            this.tbxSeason.TabIndex = 8;
            // 
            // tbxLevels
            // 
            this.tbxLevels.Location = new System.Drawing.Point(95, 204);
            this.tbxLevels.MaxLength = 100;
            this.tbxLevels.Name = "tbxLevels";
            this.tbxLevels.Size = new System.Drawing.Size(331, 20);
            this.tbxLevels.TabIndex = 16;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(54, 24);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // lblCountry
            // 
            this.lblCountry.AutoSize = true;
            this.lblCountry.Location = new System.Drawing.Point(46, 50);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(43, 13);
            this.lblCountry.TabIndex = 2;
            this.lblCountry.Text = "Country";
            // 
            // lblLeague
            // 
            this.lblLeague.AutoSize = true;
            this.lblLeague.Location = new System.Drawing.Point(46, 76);
            this.lblLeague.Name = "lblLeague";
            this.lblLeague.Size = new System.Drawing.Size(43, 13);
            this.lblLeague.TabIndex = 4;
            this.lblLeague.Text = "League";
            // 
            // lblSeason
            // 
            this.lblSeason.AutoSize = true;
            this.lblSeason.Location = new System.Drawing.Point(46, 102);
            this.lblSeason.Name = "lblSeason";
            this.lblSeason.Size = new System.Drawing.Size(43, 13);
            this.lblSeason.TabIndex = 7;
            this.lblSeason.Text = "Season";
            // 
            // btnOpenUrl
            // 
            this.btnOpenUrl.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.Web;
            this.btnOpenUrl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOpenUrl.Location = new System.Drawing.Point(261, 70);
            this.btnOpenUrl.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.btnOpenUrl.Name = "btnOpenUrl";
            this.btnOpenUrl.Size = new System.Drawing.Size(25, 25);
            this.btnOpenUrl.TabIndex = 6;
            this.btnOpenUrl.UseVisualStyleBackColor = true;
            this.btnOpenUrl.Click += new System.EventHandler(this.btnOpenUrl_Click);
            // 
            // numNbTeams
            // 
            this.numNbTeams.Location = new System.Drawing.Point(95, 178);
            this.numNbTeams.Name = "numNbTeams";
            this.numNbTeams.Size = new System.Drawing.Size(64, 20);
            this.numNbTeams.TabIndex = 14;
            this.numNbTeams.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // lblNbTeams
            // 
            this.lblNbTeams.AutoSize = true;
            this.lblNbTeams.Location = new System.Drawing.Point(33, 180);
            this.lblNbTeams.Name = "lblNbTeams";
            this.lblNbTeams.Size = new System.Drawing.Size(56, 13);
            this.lblNbTeams.TabIndex = 13;
            this.lblNbTeams.Text = "Nb Teams";
            // 
            // lblLevels
            // 
            this.lblLevels.AutoSize = true;
            this.lblLevels.Location = new System.Drawing.Point(51, 207);
            this.lblLevels.Name = "lblLevels";
            this.lblLevels.Size = new System.Drawing.Size(38, 13);
            this.lblLevels.TabIndex = 15;
            this.lblLevels.Text = "Levels";
            // 
            // cbxKind
            // 
            this.cbxKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxKind.FormattingEnabled = true;
            this.cbxKind.Location = new System.Drawing.Point(95, 125);
            this.cbxKind.Name = "cbxKind";
            this.cbxKind.Size = new System.Drawing.Size(141, 21);
            this.cbxKind.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Type";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.wfb_logo;
            this.pictureBox1.Location = new System.Drawing.Point(347, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(80, 83);
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // tbxHighlights
            // 
            this.tbxHighlights.Location = new System.Drawing.Point(95, 230);
            this.tbxHighlights.MaxLength = 100;
            this.tbxHighlights.Name = "tbxHighlights";
            this.tbxHighlights.Size = new System.Drawing.Size(331, 20);
            this.tbxHighlights.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 233);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Highlights";
            // 
            // btnIcon
            // 
            this.btnIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIcon.Location = new System.Drawing.Point(347, 92);
            this.btnIcon.Name = "btnIcon";
            this.btnIcon.Size = new System.Drawing.Size(80, 23);
            this.btnIcon.TabIndex = 19;
            this.btnIcon.Text = "Icon";
            this.btnIcon.UseVisualStyleBackColor = true;
            this.btnIcon.Click += new System.EventHandler(this.btnIcon_Click);
            // 
            // tbxDetails
            // 
            this.tbxDetails.Location = new System.Drawing.Point(95, 152);
            this.tbxDetails.MaxLength = 100;
            this.tbxDetails.Name = "tbxDetails";
            this.tbxDetails.Size = new System.Drawing.Size(331, 20);
            this.tbxDetails.TabIndex = 12;
            // 
            // lblDetails
            // 
            this.lblDetails.AutoSize = true;
            this.lblDetails.Location = new System.Drawing.Point(36, 155);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(39, 13);
            this.lblDetails.TabIndex = 11;
            this.lblDetails.Text = "Details";
            // 
            // lblScoreId
            // 
            this.lblScoreId.AutoSize = true;
            this.lblScoreId.Location = new System.Drawing.Point(3, 4);
            this.lblScoreId.Name = "lblScoreId";
            this.lblScoreId.Size = new System.Drawing.Size(56, 13);
            this.lblScoreId.TabIndex = 60;
            this.lblScoreId.Text = "<score id>";
            this.lblScoreId.Visible = false;
            // 
            // WorldFootballScoreEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblScoreId);
            this.Controls.Add(this.btnIcon);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.cbxKind);
            this.Controls.Add(this.numNbTeams);
            this.Controls.Add(this.btnOpenUrl);
            this.Controls.Add(this.lblDetails);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblLevels);
            this.Controls.Add(this.lblNbTeams);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblSeason);
            this.Controls.Add(this.lblLeague);
            this.Controls.Add(this.lblCountry);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.tbxDetails);
            this.Controls.Add(this.tbxHighlights);
            this.Controls.Add(this.tbxLevels);
            this.Controls.Add(this.tbxSeason);
            this.Controls.Add(this.tbxLeague);
            this.Controls.Add(this.tbxCountry);
            this.Controls.Add(this.tbxName);
            this.MinimumSize = new System.Drawing.Size(430, 259);
            this.Name = "WorldFootballScoreEditor";
            this.Size = new System.Drawing.Size(430, 272);
            this.Load += new System.EventHandler(this.WorldFootballScoreEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNbTeams)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxName;
        private System.Windows.Forms.TextBox tbxCountry;
        private System.Windows.Forms.TextBox tbxLeague;
        private System.Windows.Forms.TextBox tbxSeason;
        private System.Windows.Forms.TextBox tbxLevels;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblCountry;
        private System.Windows.Forms.Label lblLeague;
        private System.Windows.Forms.Label lblSeason;
        private System.Windows.Forms.Button btnOpenUrl;
        private System.Windows.Forms.NumericUpDown numNbTeams;
        private System.Windows.Forms.Label lblNbTeams;
        private System.Windows.Forms.Label lblLevels;
        private System.Windows.Forms.ComboBox cbxKind;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox tbxHighlights;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnIcon;
        private System.Windows.Forms.TextBox tbxDetails;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.Label lblScoreId;
    }
}
