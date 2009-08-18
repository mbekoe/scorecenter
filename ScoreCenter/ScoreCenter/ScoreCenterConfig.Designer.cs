namespace MediaPortal.Plugin.ScoreCenter
{
    partial class ScoreCenterConfig
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScoreCenterConfig));
            this.gbxScore = new System.Windows.Forms.GroupBox();
            this.btnAuto = new System.Windows.Forms.Button();
            this.tbxScore = new System.Windows.Forms.TextBox();
            this.tbxLeague = new System.Windows.Forms.TextBox();
            this.tbxCategory = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.ckxReload = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.lblMaxLines = new System.Windows.Forms.Label();
            this.lblSkip = new System.Windows.Forms.Label();
            this.tbxSkip = new System.Windows.Forms.TextBox();
            this.tbxMaxLines = new System.Windows.Forms.TextBox();
            this.tbxSizes = new System.Windows.Forms.TextBox();
            this.tbxHeaders = new System.Windows.Forms.TextBox();
            this.tbxXpath = new System.Windows.Forms.TextBox();
            this.tbxUrl = new System.Windows.Forms.TextBox();
            this.lblSizes = new System.Windows.Forms.Label();
            this.lblHeaders = new System.Windows.Forms.Label();
            this.lblXPath = new System.Windows.Forms.Label();
            this.lblUrl = new System.Windows.Forms.Label();
            this.btnClearIcon = new System.Windows.Forms.Button();
            this.btnSetIcon = new System.Windows.Forms.Button();
            this.pbxIcon = new System.Windows.Forms.PictureBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNewLigue = new System.Windows.Forms.ToolStripButton();
            this.tsbAbout = new System.Windows.Forms.ToolStripButton();
            this.tsbCopyScore = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.tsbEditStyles = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbExport = new System.Windows.Forms.ToolStripButton();
            this.tsbImport = new System.Windows.Forms.ToolStripButton();
            this.tsbOptions = new System.Windows.Forms.ToolStripButton();
            this.grdTest = new System.Windows.Forms.DataGridView();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ofdSelectIcon = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabScore = new System.Windows.Forms.TabControl();
            this.tpgRules = new System.Windows.Forms.TabPage();
            this.grdRule = new System.Windows.Forms.DataGridView();
            this.colColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOperator = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAction = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colStyle = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.tpgTest = new System.Windows.Forms.TabPage();
            this.ofdImport = new System.Windows.Forms.OpenFileDialog();
            this.btnOpenUrl = new System.Windows.Forms.Button();
            this.tvwScores = new MediaPortal.Plugin.ScoreCenter.ThreeStateTreeView();
            this.gbxScore.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTest)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabScore.SuspendLayout();
            this.tpgRules.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRule)).BeginInit();
            this.tpgTest.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxScore
            // 
            resources.ApplyResources(this.gbxScore, "gbxScore");
            this.gbxScore.Controls.Add(this.btnOpenUrl);
            this.gbxScore.Controls.Add(this.btnAuto);
            this.gbxScore.Controls.Add(this.tbxScore);
            this.gbxScore.Controls.Add(this.tbxLeague);
            this.gbxScore.Controls.Add(this.tbxCategory);
            this.gbxScore.Controls.Add(this.lblName);
            this.gbxScore.Controls.Add(this.ckxReload);
            this.gbxScore.Controls.Add(this.btnSave);
            this.gbxScore.Controls.Add(this.btnTest);
            this.gbxScore.Controls.Add(this.lblMaxLines);
            this.gbxScore.Controls.Add(this.lblSkip);
            this.gbxScore.Controls.Add(this.tbxSkip);
            this.gbxScore.Controls.Add(this.tbxMaxLines);
            this.gbxScore.Controls.Add(this.tbxSizes);
            this.gbxScore.Controls.Add(this.tbxHeaders);
            this.gbxScore.Controls.Add(this.tbxXpath);
            this.gbxScore.Controls.Add(this.tbxUrl);
            this.gbxScore.Controls.Add(this.lblSizes);
            this.gbxScore.Controls.Add(this.lblHeaders);
            this.gbxScore.Controls.Add(this.lblXPath);
            this.gbxScore.Controls.Add(this.lblUrl);
            this.gbxScore.Name = "gbxScore";
            this.gbxScore.TabStop = false;
            // 
            // btnAuto
            // 
            resources.ApplyResources(this.btnAuto, "btnAuto");
            this.btnAuto.Name = "btnAuto";
            this.toolTip1.SetToolTip(this.btnAuto, resources.GetString("btnAuto.ToolTip"));
            this.btnAuto.UseVisualStyleBackColor = true;
            this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
            // 
            // tbxScore
            // 
            resources.ApplyResources(this.tbxScore, "tbxScore");
            this.tbxScore.Name = "tbxScore";
            this.toolTip1.SetToolTip(this.tbxScore, resources.GetString("tbxScore.ToolTip"));
            // 
            // tbxLeague
            // 
            resources.ApplyResources(this.tbxLeague, "tbxLeague");
            this.tbxLeague.Name = "tbxLeague";
            this.toolTip1.SetToolTip(this.tbxLeague, resources.GetString("tbxLeague.ToolTip"));
            // 
            // tbxCategory
            // 
            resources.ApplyResources(this.tbxCategory, "tbxCategory");
            this.tbxCategory.Name = "tbxCategory";
            this.toolTip1.SetToolTip(this.tbxCategory, resources.GetString("tbxCategory.ToolTip"));
            // 
            // lblName
            // 
            resources.ApplyResources(this.lblName, "lblName");
            this.lblName.Name = "lblName";
            // 
            // ckxReload
            // 
            resources.ApplyResources(this.ckxReload, "ckxReload");
            this.ckxReload.Name = "ckxReload";
            this.toolTip1.SetToolTip(this.ckxReload, resources.GetString("ckxReload.ToolTip"));
            this.ckxReload.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.toolTip1.SetToolTip(this.btnSave, resources.GetString("btnSave.ToolTip"));
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnTest
            // 
            resources.ApplyResources(this.btnTest, "btnTest");
            this.btnTest.Name = "btnTest";
            this.toolTip1.SetToolTip(this.btnTest, resources.GetString("btnTest.ToolTip"));
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // lblMaxLines
            // 
            resources.ApplyResources(this.lblMaxLines, "lblMaxLines");
            this.lblMaxLines.Name = "lblMaxLines";
            // 
            // lblSkip
            // 
            resources.ApplyResources(this.lblSkip, "lblSkip");
            this.lblSkip.Name = "lblSkip";
            // 
            // tbxSkip
            // 
            resources.ApplyResources(this.tbxSkip, "tbxSkip");
            this.tbxSkip.Name = "tbxSkip";
            this.toolTip1.SetToolTip(this.tbxSkip, resources.GetString("tbxSkip.ToolTip"));
            // 
            // tbxMaxLines
            // 
            resources.ApplyResources(this.tbxMaxLines, "tbxMaxLines");
            this.tbxMaxLines.Name = "tbxMaxLines";
            this.toolTip1.SetToolTip(this.tbxMaxLines, resources.GetString("tbxMaxLines.ToolTip"));
            // 
            // tbxSizes
            // 
            resources.ApplyResources(this.tbxSizes, "tbxSizes");
            this.tbxSizes.Name = "tbxSizes";
            // 
            // tbxHeaders
            // 
            resources.ApplyResources(this.tbxHeaders, "tbxHeaders");
            this.tbxHeaders.Name = "tbxHeaders";
            this.toolTip1.SetToolTip(this.tbxHeaders, resources.GetString("tbxHeaders.ToolTip"));
            // 
            // tbxXpath
            // 
            resources.ApplyResources(this.tbxXpath, "tbxXpath");
            this.tbxXpath.Name = "tbxXpath";
            this.toolTip1.SetToolTip(this.tbxXpath, resources.GetString("tbxXpath.ToolTip"));
            // 
            // tbxUrl
            // 
            resources.ApplyResources(this.tbxUrl, "tbxUrl");
            this.tbxUrl.Name = "tbxUrl";
            this.toolTip1.SetToolTip(this.tbxUrl, resources.GetString("tbxUrl.ToolTip"));
            // 
            // lblSizes
            // 
            resources.ApplyResources(this.lblSizes, "lblSizes");
            this.lblSizes.Name = "lblSizes";
            // 
            // lblHeaders
            // 
            resources.ApplyResources(this.lblHeaders, "lblHeaders");
            this.lblHeaders.Name = "lblHeaders";
            // 
            // lblXPath
            // 
            resources.ApplyResources(this.lblXPath, "lblXPath");
            this.lblXPath.Name = "lblXPath";
            // 
            // lblUrl
            // 
            resources.ApplyResources(this.lblUrl, "lblUrl");
            this.lblUrl.Name = "lblUrl";
            // 
            // btnClearIcon
            // 
            this.btnClearIcon.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.DeleteHS;
            resources.ApplyResources(this.btnClearIcon, "btnClearIcon");
            this.btnClearIcon.Name = "btnClearIcon";
            this.btnClearIcon.UseVisualStyleBackColor = true;
            this.btnClearIcon.Click += new System.EventHandler(this.btnClearIcon_Click);
            // 
            // btnSetIcon
            // 
            this.btnSetIcon.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.openfolderHS;
            resources.ApplyResources(this.btnSetIcon, "btnSetIcon");
            this.btnSetIcon.Name = "btnSetIcon";
            this.btnSetIcon.UseVisualStyleBackColor = true;
            this.btnSetIcon.Click += new System.EventHandler(this.btnSetIcon_Click);
            // 
            // pbxIcon
            // 
            this.pbxIcon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.pbxIcon, "pbxIcon");
            this.pbxIcon.Name = "pbxIcon";
            this.pbxIcon.TabStop = false;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNewLigue,
            this.tsbAbout,
            this.tsbCopyScore,
            this.tsbDelete,
            this.tsbEditStyles,
            this.toolStripSeparator1,
            this.tsbExport,
            this.tsbImport,
            this.tsbOptions});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // tsbNewLigue
            // 
            this.tsbNewLigue.AutoToolTip = false;
            this.tsbNewLigue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNewLigue.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.NewDocumentHS;
            resources.ApplyResources(this.tsbNewLigue, "tsbNewLigue");
            this.tsbNewLigue.Name = "tsbNewLigue";
            this.tsbNewLigue.Click += new System.EventHandler(this.tsbNewLigue_Click);
            // 
            // tsbAbout
            // 
            this.tsbAbout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAbout.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.Help;
            resources.ApplyResources(this.tsbAbout, "tsbAbout");
            this.tsbAbout.Name = "tsbAbout";
            this.tsbAbout.Click += new System.EventHandler(this.tsbAbout_Click);
            // 
            // tsbCopyScore
            // 
            this.tsbCopyScore.AutoToolTip = false;
            this.tsbCopyScore.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCopyScore.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.CopyHS;
            resources.ApplyResources(this.tsbCopyScore, "tsbCopyScore");
            this.tsbCopyScore.Name = "tsbCopyScore";
            this.tsbCopyScore.Click += new System.EventHandler(this.tsbCopyScore_Click);
            // 
            // tsbDelete
            // 
            this.tsbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDelete.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.DeleteHS;
            resources.ApplyResources(this.tsbDelete, "tsbDelete");
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // tsbEditStyles
            // 
            this.tsbEditStyles.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbEditStyles.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.ChooseColor;
            resources.ApplyResources(this.tsbEditStyles, "tsbEditStyles");
            this.tsbEditStyles.Name = "tsbEditStyles";
            this.tsbEditStyles.Click += new System.EventHandler(this.tsbEditStyles_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // tsbExport
            // 
            this.tsbExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExport.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.DownloadDocument;
            resources.ApplyResources(this.tsbExport, "tsbExport");
            this.tsbExport.Name = "tsbExport";
            this.tsbExport.Click += new System.EventHandler(this.tsbExport_Click);
            // 
            // tsbImport
            // 
            this.tsbImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbImport.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.Webcontrol_Fileupload;
            resources.ApplyResources(this.tsbImport, "tsbImport");
            this.tsbImport.Name = "tsbImport";
            this.tsbImport.Click += new System.EventHandler(this.tsbImport_Click);
            // 
            // tsbOptions
            // 
            this.tsbOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOptions.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.gear_32;
            resources.ApplyResources(this.tsbOptions, "tsbOptions");
            this.tsbOptions.Name = "tsbOptions";
            this.tsbOptions.Click += new System.EventHandler(this.tsbOptions_Click);
            // 
            // grdTest
            // 
            this.grdTest.AllowUserToAddRows = false;
            this.grdTest.AllowUserToDeleteRows = false;
            this.grdTest.AllowUserToResizeRows = false;
            this.grdTest.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.grdTest.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdTest.ColumnHeadersVisible = false;
            resources.ApplyResources(this.grdTest, "grdTest");
            this.grdTest.Name = "grdTest";
            this.grdTest.ReadOnly = true;
            this.grdTest.RowHeadersVisible = false;
            this.grdTest.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            // 
            // ofdSelectIcon
            // 
            resources.ApplyResources(this.ofdSelectIcon, "ofdSelectIcon");
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.btnClearIcon);
            this.panel1.Controls.Add(this.pbxIcon);
            this.panel1.Controls.Add(this.btnSetIcon);
            this.panel1.Name = "panel1";
            // 
            // tabScore
            // 
            resources.ApplyResources(this.tabScore, "tabScore");
            this.tabScore.Controls.Add(this.tpgRules);
            this.tabScore.Controls.Add(this.tpgTest);
            this.tabScore.Name = "tabScore";
            this.tabScore.SelectedIndex = 0;
            // 
            // tpgRules
            // 
            this.tpgRules.Controls.Add(this.grdRule);
            resources.ApplyResources(this.tpgRules, "tpgRules");
            this.tpgRules.Name = "tpgRules";
            this.tpgRules.UseVisualStyleBackColor = true;
            // 
            // grdRule
            // 
            this.grdRule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdRule.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colColumn,
            this.colOperator,
            this.colValue,
            this.colAction,
            this.colStyle});
            resources.ApplyResources(this.grdRule, "grdRule");
            this.grdRule.Name = "grdRule";
            this.grdRule.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grdRule_DataError);
            // 
            // colColumn
            // 
            resources.ApplyResources(this.colColumn, "colColumn");
            this.colColumn.MaxInputLength = 5;
            this.colColumn.Name = "colColumn";
            // 
            // colOperator
            // 
            this.colOperator.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            resources.ApplyResources(this.colOperator, "colOperator");
            this.colOperator.Name = "colOperator";
            // 
            // colValue
            // 
            resources.ApplyResources(this.colValue, "colValue");
            this.colValue.MaxInputLength = 100;
            this.colValue.Name = "colValue";
            // 
            // colAction
            // 
            this.colAction.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            resources.ApplyResources(this.colAction, "colAction");
            this.colAction.Name = "colAction";
            // 
            // colStyle
            // 
            this.colStyle.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            resources.ApplyResources(this.colStyle, "colStyle");
            this.colStyle.Name = "colStyle";
            this.colStyle.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // tpgTest
            // 
            this.tpgTest.Controls.Add(this.grdTest);
            resources.ApplyResources(this.tpgTest, "tpgTest");
            this.tpgTest.Name = "tpgTest";
            this.tpgTest.UseVisualStyleBackColor = true;
            // 
            // ofdImport
            // 
            resources.ApplyResources(this.ofdImport, "ofdImport");
            // 
            // btnOpenUrl
            // 
            this.btnOpenUrl.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.Web;
            resources.ApplyResources(this.btnOpenUrl, "btnOpenUrl");
            this.btnOpenUrl.Name = "btnOpenUrl";
            this.toolTip1.SetToolTip(this.btnOpenUrl, resources.GetString("btnOpenUrl.ToolTip"));
            this.btnOpenUrl.UseVisualStyleBackColor = true;
            this.btnOpenUrl.Click += new System.EventHandler(this.btnOpenUrl_Click);
            // 
            // tvwScores
            // 
            resources.ApplyResources(this.tvwScores, "tvwScores");
            this.tvwScores.CheckBoxes = true;
            this.tvwScores.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.tvwScores.FullRowSelect = true;
            this.tvwScores.HideSelection = false;
            this.tvwScores.LabelEdit = true;
            this.tvwScores.Name = "tvwScores";
            this.tvwScores.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvwScores_AfterCheck);
            this.tvwScores.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvwScores_AfterLabelEdit);
            this.tvwScores.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwScores_AfterSelect);
            // 
            // ScoreCenterConfig
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabScore);
            this.Controls.Add(this.gbxScore);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tvwScores);
            this.Name = "ScoreCenterConfig";
            this.Load += new System.EventHandler(this.ScoreCenterConfig_Load);
            this.gbxScore.ResumeLayout(false);
            this.gbxScore.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTest)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tabScore.ResumeLayout(false);
            this.tpgRules.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdRule)).EndInit();
            this.tpgTest.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ThreeStateTreeView tvwScores;
        private System.Windows.Forms.GroupBox gbxScore;
        private System.Windows.Forms.TextBox tbxSkip;
        private System.Windows.Forms.TextBox tbxMaxLines;
        private System.Windows.Forms.TextBox tbxSizes;
        private System.Windows.Forms.TextBox tbxHeaders;
        private System.Windows.Forms.TextBox tbxXpath;
        private System.Windows.Forms.TextBox tbxUrl;
        private System.Windows.Forms.Label lblSizes;
        private System.Windows.Forms.Label lblHeaders;
        private System.Windows.Forms.Label lblXPath;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.Label lblMaxLines;
        private System.Windows.Forms.Label lblSkip;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbNewLigue;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.ToolStripButton tsbAbout;
        private System.Windows.Forms.CheckBox ckxReload;
        private System.Windows.Forms.TextBox tbxScore;
        private System.Windows.Forms.TextBox tbxLeague;
        private System.Windows.Forms.TextBox tbxCategory;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.DataGridView grdTest;
        private System.Windows.Forms.Button btnAuto;
        private System.Windows.Forms.ToolStripButton tsbCopyScore;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbExport;
        private System.Windows.Forms.ToolStripButton tsbImport;
        private System.Windows.Forms.PictureBox pbxIcon;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnClearIcon;
        private System.Windows.Forms.Button btnSetIcon;
        private System.Windows.Forms.OpenFileDialog ofdSelectIcon;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabScore;
        private System.Windows.Forms.TabPage tpgTest;
        private System.Windows.Forms.TabPage tpgRules;
        private System.Windows.Forms.DataGridView grdRule;
        private System.Windows.Forms.ToolStripButton tsbEditStyles;
        private System.Windows.Forms.OpenFileDialog ofdImport;
        private System.Windows.Forms.ToolStripButton tsbOptions;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn colOperator;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.DataGridViewComboBoxColumn colAction;
        private System.Windows.Forms.DataGridViewComboBoxColumn colStyle;
        private System.Windows.Forms.Button btnOpenUrl;
    }
}