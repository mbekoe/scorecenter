namespace MediaPortal.Plugin.ScoreCenter.Editor
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
            this.components = new System.ComponentModel.Container();
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
            this.lblType = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbxHighlights = new System.Windows.Forms.TextBox();
            this.lblHighlights = new System.Windows.Forms.Label();
            this.btnIcon = new System.Windows.Forms.Button();
            this.tbxDetails = new System.Windows.Forms.TextBox();
            this.lblDetails = new System.Windows.Forms.Label();
            this.cbxDetailsHelper = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblScoreId = new System.Windows.Forms.Label();
            this.tbxScoreId = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNbTeams)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbxName
            // 
            this.tbxName.Location = new System.Drawing.Point(95, 29);
            this.tbxName.MaxLength = 30;
            this.tbxName.Name = "tbxName";
            this.tbxName.Size = new System.Drawing.Size(229, 20);
            this.tbxName.TabIndex = 3;
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
            this.tbxCountry.Location = new System.Drawing.Point(95, 55);
            this.tbxCountry.MaxLength = 100;
            this.tbxCountry.Name = "tbxCountry";
            this.tbxCountry.Size = new System.Drawing.Size(141, 20);
            this.tbxCountry.TabIndex = 5;
            // 
            // tbxLeague
            // 
            this.tbxLeague.Location = new System.Drawing.Point(95, 81);
            this.tbxLeague.MaxLength = 100;
            this.tbxLeague.Name = "tbxLeague";
            this.tbxLeague.Size = new System.Drawing.Size(141, 20);
            this.tbxLeague.TabIndex = 7;
            // 
            // tbxSeason
            // 
            this.tbxSeason.AutoCompleteCustomSource.AddRange(new string[] {
            "{yyyy}",
            "{YY+1}",
            "{YYYY+1}",
            "{YY-YY+1}",
            "{YYYY-YY+1}",
            "{YYYY-YYYY+1}"});
            this.tbxSeason.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbxSeason.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbxSeason.Location = new System.Drawing.Point(95, 107);
            this.tbxSeason.MaxLength = 20;
            this.tbxSeason.Name = "tbxSeason";
            this.tbxSeason.Size = new System.Drawing.Size(141, 20);
            this.tbxSeason.TabIndex = 10;
            // 
            // tbxLevels
            // 
            this.tbxLevels.Location = new System.Drawing.Point(95, 212);
            this.tbxLevels.MaxLength = 100;
            this.tbxLevels.Name = "tbxLevels";
            this.tbxLevels.Size = new System.Drawing.Size(331, 20);
            this.tbxLevels.TabIndex = 19;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(54, 32);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name";
            // 
            // lblCountry
            // 
            this.lblCountry.AutoSize = true;
            this.lblCountry.Location = new System.Drawing.Point(46, 58);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(43, 13);
            this.lblCountry.TabIndex = 4;
            this.lblCountry.Text = "Country";
            // 
            // lblLeague
            // 
            this.lblLeague.AutoSize = true;
            this.lblLeague.Location = new System.Drawing.Point(46, 84);
            this.lblLeague.Name = "lblLeague";
            this.lblLeague.Size = new System.Drawing.Size(43, 13);
            this.lblLeague.TabIndex = 6;
            this.lblLeague.Text = "League";
            // 
            // lblSeason
            // 
            this.lblSeason.AutoSize = true;
            this.lblSeason.Location = new System.Drawing.Point(46, 110);
            this.lblSeason.Name = "lblSeason";
            this.lblSeason.Size = new System.Drawing.Size(43, 13);
            this.lblSeason.TabIndex = 9;
            this.lblSeason.Text = "Season";
            // 
            // btnOpenUrl
            // 
            this.btnOpenUrl.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.Web;
            this.btnOpenUrl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOpenUrl.Location = new System.Drawing.Point(261, 78);
            this.btnOpenUrl.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.btnOpenUrl.Name = "btnOpenUrl";
            this.btnOpenUrl.Size = new System.Drawing.Size(25, 25);
            this.btnOpenUrl.TabIndex = 8;
            this.toolTip1.SetToolTip(this.btnOpenUrl, "Open the page");
            this.btnOpenUrl.UseVisualStyleBackColor = true;
            this.btnOpenUrl.Click += new System.EventHandler(this.btnOpenUrl_Click);
            // 
            // numNbTeams
            // 
            this.numNbTeams.Location = new System.Drawing.Point(95, 186);
            this.numNbTeams.Name = "numNbTeams";
            this.numNbTeams.Size = new System.Drawing.Size(64, 20);
            this.numNbTeams.TabIndex = 17;
            this.numNbTeams.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // lblNbTeams
            // 
            this.lblNbTeams.AutoSize = true;
            this.lblNbTeams.Location = new System.Drawing.Point(33, 188);
            this.lblNbTeams.Name = "lblNbTeams";
            this.lblNbTeams.Size = new System.Drawing.Size(56, 13);
            this.lblNbTeams.TabIndex = 16;
            this.lblNbTeams.Text = "Nb Teams";
            // 
            // lblLevels
            // 
            this.lblLevels.AutoSize = true;
            this.lblLevels.Location = new System.Drawing.Point(51, 215);
            this.lblLevels.Name = "lblLevels";
            this.lblLevels.Size = new System.Drawing.Size(38, 13);
            this.lblLevels.TabIndex = 18;
            this.lblLevels.Text = "Levels";
            // 
            // cbxKind
            // 
            this.cbxKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxKind.FormattingEnabled = true;
            this.cbxKind.Location = new System.Drawing.Point(95, 133);
            this.cbxKind.Name = "cbxKind";
            this.cbxKind.Size = new System.Drawing.Size(141, 21);
            this.cbxKind.TabIndex = 12;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(58, 136);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(31, 13);
            this.lblType.TabIndex = 11;
            this.lblType.Text = "Type";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.wfb_logo;
            this.pictureBox1.Location = new System.Drawing.Point(464, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(80, 83);
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // tbxHighlights
            // 
            this.tbxHighlights.Location = new System.Drawing.Point(95, 238);
            this.tbxHighlights.MaxLength = 100;
            this.tbxHighlights.Name = "tbxHighlights";
            this.tbxHighlights.Size = new System.Drawing.Size(331, 20);
            this.tbxHighlights.TabIndex = 21;
            // 
            // lblHighlights
            // 
            this.lblHighlights.AutoSize = true;
            this.lblHighlights.Location = new System.Drawing.Point(36, 241);
            this.lblHighlights.Name = "lblHighlights";
            this.lblHighlights.Size = new System.Drawing.Size(53, 13);
            this.lblHighlights.TabIndex = 20;
            this.lblHighlights.Text = "Highlights";
            // 
            // btnIcon
            // 
            this.btnIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIcon.Location = new System.Drawing.Point(464, 92);
            this.btnIcon.Name = "btnIcon";
            this.btnIcon.Size = new System.Drawing.Size(80, 23);
            this.btnIcon.TabIndex = 22;
            this.btnIcon.Text = "Icon";
            this.toolTip1.SetToolTip(this.btnIcon, "Get the icon");
            this.btnIcon.UseVisualStyleBackColor = true;
            this.btnIcon.Click += new System.EventHandler(this.btnIcon_Click);
            // 
            // tbxDetails
            // 
            this.tbxDetails.Location = new System.Drawing.Point(95, 160);
            this.tbxDetails.MaxLength = 100;
            this.tbxDetails.Name = "tbxDetails";
            this.tbxDetails.Size = new System.Drawing.Size(331, 20);
            this.tbxDetails.TabIndex = 14;
            this.toolTip1.SetToolTip(this.tbxDetails, "Details for the score (separate by a ,)");
            // 
            // lblDetails
            // 
            this.lblDetails.AutoSize = true;
            this.lblDetails.Location = new System.Drawing.Point(36, 163);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(39, 13);
            this.lblDetails.TabIndex = 13;
            this.lblDetails.Text = "Details";
            // 
            // cbxDetailsHelper
            // 
            this.cbxDetailsHelper.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDetailsHelper.FormattingEnabled = true;
            this.cbxDetailsHelper.Items.AddRange(new object[] {
            "1-runde",
            "2-runde",
            "3-runde",
            "achtelfinale",
            "viertelfinale",
            "halbfinale",
            "3-platz",
            "finale"});
            this.cbxDetailsHelper.Location = new System.Drawing.Point(432, 159);
            this.cbxDetailsHelper.Name = "cbxDetailsHelper";
            this.cbxDetailsHelper.Size = new System.Drawing.Size(112, 21);
            this.cbxDetailsHelper.TabIndex = 15;
            this.toolTip1.SetToolTip(this.cbxDetailsHelper, "Select a detail");
            this.cbxDetailsHelper.SelectedIndexChanged += new System.EventHandler(this.cbxDetailsHelper_SelectedIndexChanged);
            // 
            // lblScoreId
            // 
            this.lblScoreId.AutoSize = true;
            this.lblScoreId.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblScoreId.Location = new System.Drawing.Point(73, 6);
            this.lblScoreId.Name = "lblScoreId";
            this.lblScoreId.Size = new System.Drawing.Size(16, 13);
            this.lblScoreId.TabIndex = 0;
            this.lblScoreId.Text = "Id";
            // 
            // tbxScoreId
            // 
            this.tbxScoreId.HideSelection = false;
            this.tbxScoreId.Location = new System.Drawing.Point(95, 3);
            this.tbxScoreId.Name = "tbxScoreId";
            this.tbxScoreId.ReadOnly = true;
            this.tbxScoreId.Size = new System.Drawing.Size(301, 20);
            this.tbxScoreId.TabIndex = 1;
            // 
            // WorldFootballScoreEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbxScoreId);
            this.Controls.Add(this.lblScoreId);
            this.Controls.Add(this.cbxDetailsHelper);
            this.Controls.Add(this.btnIcon);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.cbxKind);
            this.Controls.Add(this.numNbTeams);
            this.Controls.Add(this.btnOpenUrl);
            this.Controls.Add(this.lblDetails);
            this.Controls.Add(this.lblHighlights);
            this.Controls.Add(this.lblLevels);
            this.Controls.Add(this.lblNbTeams);
            this.Controls.Add(this.lblType);
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
            this.Size = new System.Drawing.Size(547, 272);
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
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox tbxHighlights;
        private System.Windows.Forms.Label lblHighlights;
        private System.Windows.Forms.Button btnIcon;
        private System.Windows.Forms.TextBox tbxDetails;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.ComboBox cbxDetailsHelper;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblScoreId;
        private System.Windows.Forms.TextBox tbxScoreId;
    }
}
