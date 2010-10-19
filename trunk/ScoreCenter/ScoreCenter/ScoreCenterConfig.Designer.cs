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
            this.cbxBetweenElements = new System.Windows.Forms.ComboBox();
            this.ckxReverseOrder = new System.Windows.Forms.CheckBox();
            this.ckxAllowWrapping = new System.Windows.Forms.CheckBox();
            this.ckxNewLine = new System.Windows.Forms.CheckBox();
            this.lblTotalSize = new System.Windows.Forms.Label();
            this.tbxEncoding = new System.Windows.Forms.TextBox();
            this.tbxElement = new System.Windows.Forms.TextBox();
            this.btnOpenUrl = new System.Windows.Forms.Button();
            this.btnAuto = new System.Windows.Forms.Button();
            this.tbxScore = new System.Windows.Forms.TextBox();
            this.tbxLeague = new System.Windows.Forms.TextBox();
            this.tbxCategory = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.ckxUseCaption = new System.Windows.Forms.CheckBox();
            this.ckxUseTheader = new System.Windows.Forms.CheckBox();
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
            this.lblXpathElement = new System.Windows.Forms.Label();
            this.lblEncoding = new System.Windows.Forms.Label();
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
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbMoveUp = new System.Windows.Forms.ToolStripButton();
            this.tsbMoveDown = new System.Windows.Forms.ToolStripButton();
            this.tsbEditStyles = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbExport = new System.Windows.Forms.ToolStripButton();
            this.tsbImport = new System.Windows.Forms.ToolStripButton();
            this.tsbOptions = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.alignementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.pnlTest = new System.Windows.Forms.Panel();
            this.ofdImport = new System.Windows.Forms.OpenFileDialog();
            this.tvwScores = new MediaPortal.Plugin.ScoreCenter.ThreeStateTreeView();
            this.gbxScore.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabScore.SuspendLayout();
            this.tpgRules.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRule)).BeginInit();
            this.tpgTest.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxScore
            // 
            this.gbxScore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxScore.Controls.Add(this.cbxBetweenElements);
            this.gbxScore.Controls.Add(this.ckxReverseOrder);
            this.gbxScore.Controls.Add(this.ckxAllowWrapping);
            this.gbxScore.Controls.Add(this.ckxNewLine);
            this.gbxScore.Controls.Add(this.lblTotalSize);
            this.gbxScore.Controls.Add(this.tbxEncoding);
            this.gbxScore.Controls.Add(this.tbxElement);
            this.gbxScore.Controls.Add(this.btnOpenUrl);
            this.gbxScore.Controls.Add(this.btnAuto);
            this.gbxScore.Controls.Add(this.tbxScore);
            this.gbxScore.Controls.Add(this.tbxLeague);
            this.gbxScore.Controls.Add(this.tbxCategory);
            this.gbxScore.Controls.Add(this.lblName);
            this.gbxScore.Controls.Add(this.ckxUseCaption);
            this.gbxScore.Controls.Add(this.ckxUseTheader);
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
            this.gbxScore.Controls.Add(this.lblXpathElement);
            this.gbxScore.Controls.Add(this.lblEncoding);
            this.gbxScore.Controls.Add(this.lblXPath);
            this.gbxScore.Controls.Add(this.lblUrl);
            this.gbxScore.Location = new System.Drawing.Point(218, 28);
            this.gbxScore.Name = "gbxScore";
            this.gbxScore.Size = new System.Drawing.Size(667, 262);
            this.gbxScore.TabIndex = 2;
            this.gbxScore.TabStop = false;
            this.gbxScore.Text = "Details";
            // 
            // cbxBetweenElements
            // 
            this.cbxBetweenElements.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBetweenElements.FormattingEnabled = true;
            this.cbxBetweenElements.Location = new System.Drawing.Point(378, 98);
            this.cbxBetweenElements.Name = "cbxBetweenElements";
            this.cbxBetweenElements.Size = new System.Drawing.Size(142, 21);
            this.cbxBetweenElements.TabIndex = 31;
            // 
            // ckxReverseOrder
            // 
            this.ckxReverseOrder.AutoSize = true;
            this.ckxReverseOrder.Location = new System.Drawing.Point(359, 197);
            this.ckxReverseOrder.Name = "ckxReverseOrder";
            this.ckxReverseOrder.Size = new System.Drawing.Size(140, 17);
            this.ckxReverseOrder.TabIndex = 23;
            this.ckxReverseOrder.Text = "Reverse Columns Order";
            this.toolTip1.SetToolTip(this.ckxReverseOrder, "Reverse the columns order (for RTL languages)");
            this.ckxReverseOrder.UseVisualStyleBackColor = true;
            // 
            // ckxAllowWrapping
            // 
            this.ckxAllowWrapping.AutoSize = true;
            this.ckxAllowWrapping.Location = new System.Drawing.Point(359, 174);
            this.ckxAllowWrapping.Name = "ckxAllowWrapping";
            this.ckxAllowWrapping.Size = new System.Drawing.Size(81, 17);
            this.ckxAllowWrapping.TabIndex = 21;
            this.ckxAllowWrapping.Text = "Word Wrap";
            this.ckxAllowWrapping.UseVisualStyleBackColor = true;
            // 
            // ckxNewLine
            // 
            this.ckxNewLine.AutoSize = true;
            this.ckxNewLine.Location = new System.Drawing.Point(245, 174);
            this.ckxNewLine.Name = "ckxNewLine";
            this.ckxNewLine.Size = new System.Drawing.Size(97, 17);
            this.ckxNewLine.TabIndex = 20;
            this.ckxNewLine.Text = "Allow New Line";
            this.ckxNewLine.UseVisualStyleBackColor = true;
            // 
            // lblTotalSize
            // 
            this.lblTotalSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblTotalSize.Location = new System.Drawing.Point(370, 175);
            this.lblTotalSize.Name = "lblTotalSize";
            this.lblTotalSize.Size = new System.Drawing.Size(151, 23);
            this.lblTotalSize.TabIndex = 22;
            this.lblTotalSize.Text = "Total";
            this.lblTotalSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbxEncoding
            // 
            this.tbxEncoding.Location = new System.Drawing.Point(59, 98);
            this.tbxEncoding.MaxLength = 20;
            this.tbxEncoding.Name = "tbxEncoding";
            this.tbxEncoding.Size = new System.Drawing.Size(119, 21);
            this.tbxEncoding.TabIndex = 10;
            // 
            // tbxElement
            // 
            this.tbxElement.Location = new System.Drawing.Point(271, 98);
            this.tbxElement.Name = "tbxElement";
            this.tbxElement.Size = new System.Drawing.Size(100, 21);
            this.tbxElement.TabIndex = 12;
            this.toolTip1.SetToolTip(this.tbxElement, "0 based indices of the elements separeted with \';\' (all if empty)");
            // 
            // btnOpenUrl
            // 
            this.btnOpenUrl.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.Web;
            this.btnOpenUrl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOpenUrl.Location = new System.Drawing.Point(520, 43);
            this.btnOpenUrl.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.btnOpenUrl.Name = "btnOpenUrl";
            this.btnOpenUrl.Size = new System.Drawing.Size(25, 25);
            this.btnOpenUrl.TabIndex = 6;
            this.toolTip1.SetToolTip(this.btnOpenUrl, "Open URL");
            this.btnOpenUrl.UseVisualStyleBackColor = true;
            this.btnOpenUrl.Click += new System.EventHandler(this.btnOpenUrl_Click);
            // 
            // btnAuto
            // 
            this.btnAuto.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAuto.Location = new System.Drawing.Point(527, 149);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(75, 23);
            this.btnAuto.TabIndex = 17;
            this.btnAuto.Text = "Auto";
            this.toolTip1.SetToolTip(this.btnAuto, "Calculate columns sizes with the test results");
            this.btnAuto.UseVisualStyleBackColor = true;
            this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
            // 
            // tbxScore
            // 
            this.tbxScore.Location = new System.Drawing.Point(370, 19);
            this.tbxScore.MaxLength = 50;
            this.tbxScore.Name = "tbxScore";
            this.tbxScore.Size = new System.Drawing.Size(150, 21);
            this.tbxScore.TabIndex = 3;
            this.toolTip1.SetToolTip(this.tbxScore, "Name");
            // 
            // tbxLeague
            // 
            this.tbxLeague.Location = new System.Drawing.Point(214, 19);
            this.tbxLeague.MaxLength = 50;
            this.tbxLeague.Name = "tbxLeague";
            this.tbxLeague.Size = new System.Drawing.Size(150, 21);
            this.tbxLeague.TabIndex = 2;
            this.toolTip1.SetToolTip(this.tbxLeague, "League");
            // 
            // tbxCategory
            // 
            this.tbxCategory.Location = new System.Drawing.Point(58, 19);
            this.tbxCategory.MaxLength = 50;
            this.tbxCategory.Name = "tbxCategory";
            this.tbxCategory.Size = new System.Drawing.Size(150, 21);
            this.tbxCategory.TabIndex = 1;
            this.toolTip1.SetToolTip(this.tbxCategory, "Category");
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblName.Location = new System.Drawing.Point(17, 22);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(34, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // ckxUseCaption
            // 
            this.ckxUseCaption.AutoSize = true;
            this.ckxUseCaption.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ckxUseCaption.Location = new System.Drawing.Point(59, 197);
            this.ckxUseCaption.Name = "ckxUseCaption";
            this.ckxUseCaption.Size = new System.Drawing.Size(101, 17);
            this.ckxUseCaption.TabIndex = 19;
            this.ckxUseCaption.Text = "Include Caption";
            this.ckxUseCaption.UseVisualStyleBackColor = true;
            // 
            // ckxUseTheader
            // 
            this.ckxUseTheader.AutoSize = true;
            this.ckxUseTheader.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ckxUseTheader.Location = new System.Drawing.Point(59, 175);
            this.ckxUseTheader.Name = "ckxUseTheader";
            this.ckxUseTheader.Size = new System.Drawing.Size(164, 17);
            this.ckxUseTheader.TabIndex = 18;
            this.ckxUseTheader.Text = "Include Header/Footer Rows";
            this.toolTip1.SetToolTip(this.ckxUseTheader, "Includes or Excludes THeader amd TFooter from the HML table");
            this.ckxUseTheader.UseVisualStyleBackColor = true;
            // 
            // ckxReload
            // 
            this.ckxReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ckxReload.AutoSize = true;
            this.ckxReload.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ckxReload.Location = new System.Drawing.Point(168, 235);
            this.ckxReload.Name = "ckxReload";
            this.ckxReload.Size = new System.Drawing.Size(59, 17);
            this.ckxReload.TabIndex = 30;
            this.ckxReload.Text = "Reload";
            this.toolTip1.SetToolTip(this.ckxReload, "Reload the page before testing");
            this.ckxReload.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSave.Location = new System.Drawing.Point(6, 231);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 28;
            this.btnSave.Text = "&Save";
            this.toolTip1.SetToolTip(this.btnSave, "Save the current settings");
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTest.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnTest.Location = new System.Drawing.Point(87, 231);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 29;
            this.btnTest.Text = "&Test";
            this.toolTip1.SetToolTip(this.btnTest, "Test your settings");
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // lblMaxLines
            // 
            this.lblMaxLines.AutoSize = true;
            this.lblMaxLines.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMaxLines.Location = new System.Drawing.Point(554, 74);
            this.lblMaxLines.Name = "lblMaxLines";
            this.lblMaxLines.Size = new System.Drawing.Size(54, 13);
            this.lblMaxLines.TabIndex = 26;
            this.lblMaxLines.Text = "Max Lines";
            // 
            // lblSkip
            // 
            this.lblSkip.AutoSize = true;
            this.lblSkip.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSkip.Location = new System.Drawing.Point(574, 48);
            this.lblSkip.Name = "lblSkip";
            this.lblSkip.Size = new System.Drawing.Size(26, 13);
            this.lblSkip.TabIndex = 24;
            this.lblSkip.Text = "Skip";
            // 
            // tbxSkip
            // 
            this.tbxSkip.Location = new System.Drawing.Point(615, 45);
            this.tbxSkip.MaxLength = 3;
            this.tbxSkip.Name = "tbxSkip";
            this.tbxSkip.Size = new System.Drawing.Size(45, 21);
            this.tbxSkip.TabIndex = 25;
            this.toolTip1.SetToolTip(this.tbxSkip, "Number of lines to skip in the table");
            // 
            // tbxMaxLines
            // 
            this.tbxMaxLines.Location = new System.Drawing.Point(615, 71);
            this.tbxMaxLines.MaxLength = 3;
            this.tbxMaxLines.Name = "tbxMaxLines";
            this.tbxMaxLines.Size = new System.Drawing.Size(45, 21);
            this.tbxMaxLines.TabIndex = 27;
            this.toolTip1.SetToolTip(this.tbxMaxLines, "Number of lines to display");
            // 
            // tbxSizes
            // 
            this.tbxSizes.Location = new System.Drawing.Point(59, 151);
            this.tbxSizes.MaxLength = 100;
            this.tbxSizes.Name = "tbxSizes";
            this.tbxSizes.Size = new System.Drawing.Size(462, 21);
            this.tbxSizes.TabIndex = 16;
            this.tbxSizes.TextChanged += new System.EventHandler(this.tbxSizes_TextChanged);
            // 
            // tbxHeaders
            // 
            this.tbxHeaders.Location = new System.Drawing.Point(59, 125);
            this.tbxHeaders.MaxLength = 200;
            this.tbxHeaders.Name = "tbxHeaders";
            this.tbxHeaders.Size = new System.Drawing.Size(462, 21);
            this.tbxHeaders.TabIndex = 14;
            this.toolTip1.SetToolTip(this.tbxHeaders, "Custom Headers");
            // 
            // tbxXpath
            // 
            this.tbxXpath.Location = new System.Drawing.Point(58, 71);
            this.tbxXpath.MaxLength = 200;
            this.tbxXpath.Name = "tbxXpath";
            this.tbxXpath.Size = new System.Drawing.Size(462, 21);
            this.tbxXpath.TabIndex = 8;
            this.toolTip1.SetToolTip(this.tbxXpath, "XPath expression to use to find the table(s)");
            // 
            // tbxUrl
            // 
            this.tbxUrl.Location = new System.Drawing.Point(58, 45);
            this.tbxUrl.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.tbxUrl.MaxLength = 200;
            this.tbxUrl.Name = "tbxUrl";
            this.tbxUrl.Size = new System.Drawing.Size(462, 21);
            this.tbxUrl.TabIndex = 5;
            this.toolTip1.SetToolTip(this.tbxUrl, "Score URL");
            // 
            // lblSizes
            // 
            this.lblSizes.AutoSize = true;
            this.lblSizes.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSizes.Location = new System.Drawing.Point(21, 154);
            this.lblSizes.Name = "lblSizes";
            this.lblSizes.Size = new System.Drawing.Size(31, 13);
            this.lblSizes.TabIndex = 15;
            this.lblSizes.Text = "Sizes";
            // 
            // lblHeaders
            // 
            this.lblHeaders.AutoSize = true;
            this.lblHeaders.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblHeaders.Location = new System.Drawing.Point(6, 128);
            this.lblHeaders.Name = "lblHeaders";
            this.lblHeaders.Size = new System.Drawing.Size(47, 13);
            this.lblHeaders.TabIndex = 13;
            this.lblHeaders.Text = "Headers";
            // 
            // lblXpathElement
            // 
            this.lblXpathElement.AutoSize = true;
            this.lblXpathElement.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblXpathElement.Location = new System.Drawing.Point(184, 101);
            this.lblXpathElement.Name = "lblXpathElement";
            this.lblXpathElement.Size = new System.Drawing.Size(81, 13);
            this.lblXpathElement.TabIndex = 11;
            this.lblXpathElement.Text = "XPath Elements";
            // 
            // lblEncoding
            // 
            this.lblEncoding.AutoSize = true;
            this.lblEncoding.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblEncoding.Location = new System.Drawing.Point(6, 101);
            this.lblEncoding.Name = "lblEncoding";
            this.lblEncoding.Size = new System.Drawing.Size(50, 13);
            this.lblEncoding.TabIndex = 9;
            this.lblEncoding.Text = "Encoding";
            // 
            // lblXPath
            // 
            this.lblXPath.AutoSize = true;
            this.lblXPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblXPath.Location = new System.Drawing.Point(17, 74);
            this.lblXPath.Name = "lblXPath";
            this.lblXPath.Size = new System.Drawing.Size(35, 13);
            this.lblXPath.TabIndex = 7;
            this.lblXPath.Text = "XPath";
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblUrl.Location = new System.Drawing.Point(24, 48);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(26, 13);
            this.lblUrl.TabIndex = 4;
            this.lblUrl.Text = "URL";
            // 
            // btnClearIcon
            // 
            this.btnClearIcon.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.DeleteHS;
            this.btnClearIcon.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClearIcon.Location = new System.Drawing.Point(182, 26);
            this.btnClearIcon.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.btnClearIcon.Name = "btnClearIcon";
            this.btnClearIcon.Size = new System.Drawing.Size(27, 23);
            this.btnClearIcon.TabIndex = 1;
            this.btnClearIcon.UseVisualStyleBackColor = true;
            this.btnClearIcon.Click += new System.EventHandler(this.btnClearIcon_Click);
            // 
            // btnSetIcon
            // 
            this.btnSetIcon.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.openfolderHS;
            this.btnSetIcon.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSetIcon.Location = new System.Drawing.Point(182, 3);
            this.btnSetIcon.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.btnSetIcon.Name = "btnSetIcon";
            this.btnSetIcon.Size = new System.Drawing.Size(27, 23);
            this.btnSetIcon.TabIndex = 0;
            this.btnSetIcon.UseVisualStyleBackColor = true;
            this.btnSetIcon.Click += new System.EventHandler(this.btnSetIcon_Click);
            // 
            // pbxIcon
            // 
            this.pbxIcon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbxIcon.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pbxIcon.Location = new System.Drawing.Point(0, 3);
            this.pbxIcon.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.pbxIcon.Name = "pbxIcon";
            this.pbxIcon.Size = new System.Drawing.Size(182, 75);
            this.pbxIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbxIcon.TabIndex = 21;
            this.pbxIcon.TabStop = false;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(810, 613);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOk.Location = new System.Drawing.Point(729, 613);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "Sa&ve";
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
            this.toolStripSeparator2,
            this.tsbMoveUp,
            this.tsbMoveDown,
            this.tsbEditStyles,
            this.toolStripSeparator1,
            this.tsbExport,
            this.tsbImport,
            this.tsbOptions});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(897, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbNewLigue
            // 
            this.tsbNewLigue.AutoToolTip = false;
            this.tsbNewLigue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNewLigue.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.NewDocumentHS;
            this.tsbNewLigue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNewLigue.Name = "tsbNewLigue";
            this.tsbNewLigue.Size = new System.Drawing.Size(23, 22);
            this.tsbNewLigue.Text = "Add";
            this.tsbNewLigue.ToolTipText = "Add a new score";
            this.tsbNewLigue.Click += new System.EventHandler(this.tsbNewLigue_Click);
            // 
            // tsbAbout
            // 
            this.tsbAbout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAbout.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.Help;
            this.tsbAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAbout.Name = "tsbAbout";
            this.tsbAbout.Size = new System.Drawing.Size(23, 22);
            this.tsbAbout.Text = "About";
            this.tsbAbout.Click += new System.EventHandler(this.tsbAbout_Click);
            // 
            // tsbCopyScore
            // 
            this.tsbCopyScore.AutoToolTip = false;
            this.tsbCopyScore.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCopyScore.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.CopyHS;
            this.tsbCopyScore.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCopyScore.Name = "tsbCopyScore";
            this.tsbCopyScore.Size = new System.Drawing.Size(23, 22);
            this.tsbCopyScore.Text = "Copy";
            this.tsbCopyScore.ToolTipText = "Copy a score";
            this.tsbCopyScore.Click += new System.EventHandler(this.tsbCopyScore_Click);
            // 
            // tsbDelete
            // 
            this.tsbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDelete.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.DeleteHS;
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(23, 22);
            this.tsbDelete.Text = "Delete";
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbMoveUp
            // 
            this.tsbMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMoveUp.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.BuilderDialog_moveup;
            this.tsbMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMoveUp.Name = "tsbMoveUp";
            this.tsbMoveUp.Size = new System.Drawing.Size(23, 22);
            this.tsbMoveUp.Text = "Move Up";
            this.tsbMoveUp.Click += new System.EventHandler(this.tsbMoveUp_Click);
            // 
            // tsbMoveDown
            // 
            this.tsbMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMoveDown.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.BuilderDialog_movedown;
            this.tsbMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMoveDown.Name = "tsbMoveDown";
            this.tsbMoveDown.Size = new System.Drawing.Size(23, 22);
            this.tsbMoveDown.Text = "Move Down";
            this.tsbMoveDown.Click += new System.EventHandler(this.tsbMoveDown_Click);
            // 
            // tsbEditStyles
            // 
            this.tsbEditStyles.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbEditStyles.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.ChooseColor;
            this.tsbEditStyles.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEditStyles.Name = "tsbEditStyles";
            this.tsbEditStyles.Size = new System.Drawing.Size(23, 22);
            this.tsbEditStyles.Text = "Styles";
            this.tsbEditStyles.Click += new System.EventHandler(this.tsbEditStyles_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbExport
            // 
            this.tsbExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExport.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.DownloadDocument;
            this.tsbExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExport.Name = "tsbExport";
            this.tsbExport.Size = new System.Drawing.Size(23, 22);
            this.tsbExport.Text = "Export";
            this.tsbExport.Click += new System.EventHandler(this.tsbExport_Click);
            // 
            // tsbImport
            // 
            this.tsbImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbImport.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.Webcontrol_Fileupload;
            this.tsbImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImport.Name = "tsbImport";
            this.tsbImport.Size = new System.Drawing.Size(23, 22);
            this.tsbImport.Text = "Import";
            this.tsbImport.Click += new System.EventHandler(this.tsbImport_Click);
            // 
            // tsbOptions
            // 
            this.tsbOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOptions.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.gear_32;
            this.tsbOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOptions.Name = "tsbOptions";
            this.tsbOptions.Size = new System.Drawing.Size(23, 22);
            this.tsbOptions.Text = "Options";
            this.tsbOptions.Click += new System.EventHandler(this.tsbOptions_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alignementToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 26);
            // 
            // alignementToolStripMenuItem
            // 
            this.alignementToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.leftToolStripMenuItem,
            this.centerToolStripMenuItem,
            this.rightToolStripMenuItem});
            this.alignementToolStripMenuItem.Name = "alignementToolStripMenuItem";
            this.alignementToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.alignementToolStripMenuItem.Text = "Alignement";
            // 
            // leftToolStripMenuItem
            // 
            this.leftToolStripMenuItem.Name = "leftToolStripMenuItem";
            this.leftToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.leftToolStripMenuItem.Text = "Left";
            this.leftToolStripMenuItem.Click += new System.EventHandler(this.leftToolStripMenuItem_Click);
            // 
            // centerToolStripMenuItem
            // 
            this.centerToolStripMenuItem.Name = "centerToolStripMenuItem";
            this.centerToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.centerToolStripMenuItem.Text = "Center";
            this.centerToolStripMenuItem.Click += new System.EventHandler(this.centerToolStripMenuItem_Click);
            // 
            // rightToolStripMenuItem
            // 
            this.rightToolStripMenuItem.Name = "rightToolStripMenuItem";
            this.rightToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.rightToolStripMenuItem.Text = "Right";
            this.rightToolStripMenuItem.Click += new System.EventHandler(this.rightToolStripMenuItem_Click);
            // 
            // ofdSelectIcon
            // 
            this.ofdSelectIcon.Filter = "Images (*.png)|*.png|All Files (*.*)|*.*";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.Controls.Add(this.btnClearIcon);
            this.panel1.Controls.Add(this.pbxIcon);
            this.panel1.Controls.Add(this.btnSetIcon);
            this.panel1.Location = new System.Drawing.Point(0, 567);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(212, 81);
            this.panel1.TabIndex = 4;
            // 
            // tabScore
            // 
            this.tabScore.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabScore.Controls.Add(this.tpgRules);
            this.tabScore.Controls.Add(this.tpgTest);
            this.tabScore.Location = new System.Drawing.Point(218, 296);
            this.tabScore.Name = "tabScore";
            this.tabScore.SelectedIndex = 0;
            this.tabScore.Size = new System.Drawing.Size(667, 311);
            this.tabScore.TabIndex = 3;
            // 
            // tpgRules
            // 
            this.tpgRules.Controls.Add(this.grdRule);
            this.tpgRules.Location = new System.Drawing.Point(4, 22);
            this.tpgRules.Name = "tpgRules";
            this.tpgRules.Padding = new System.Windows.Forms.Padding(3);
            this.tpgRules.Size = new System.Drawing.Size(659, 285);
            this.tpgRules.TabIndex = 1;
            this.tpgRules.Text = "Rules";
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
            this.grdRule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdRule.Location = new System.Drawing.Point(3, 3);
            this.grdRule.Name = "grdRule";
            this.grdRule.Size = new System.Drawing.Size(653, 279);
            this.grdRule.TabIndex = 0;
            this.grdRule.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grdRule_DataError);
            // 
            // colColumn
            // 
            this.colColumn.HeaderText = "Column";
            this.colColumn.MaxInputLength = 5;
            this.colColumn.Name = "colColumn";
            this.colColumn.Width = 120;
            // 
            // colOperator
            // 
            this.colOperator.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.colOperator.HeaderText = "Operator";
            this.colOperator.Name = "colOperator";
            this.colOperator.Width = 120;
            // 
            // colValue
            // 
            this.colValue.HeaderText = "Value";
            this.colValue.MaxInputLength = 100;
            this.colValue.Name = "colValue";
            this.colValue.Width = 120;
            // 
            // colAction
            // 
            this.colAction.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.colAction.HeaderText = "Action";
            this.colAction.Name = "colAction";
            this.colAction.Width = 120;
            // 
            // colStyle
            // 
            this.colStyle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colStyle.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.colStyle.HeaderText = "Style";
            this.colStyle.Name = "colStyle";
            this.colStyle.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // tpgTest
            // 
            this.tpgTest.Controls.Add(this.pnlTest);
            this.tpgTest.Location = new System.Drawing.Point(4, 22);
            this.tpgTest.Name = "tpgTest";
            this.tpgTest.Padding = new System.Windows.Forms.Padding(10);
            this.tpgTest.Size = new System.Drawing.Size(659, 285);
            this.tpgTest.TabIndex = 0;
            this.tpgTest.Text = "Test";
            // 
            // pnlTest
            // 
            this.pnlTest.AutoScroll = true;
            this.pnlTest.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTest.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlTest.Location = new System.Drawing.Point(10, 10);
            this.pnlTest.Name = "pnlTest";
            this.pnlTest.Size = new System.Drawing.Size(639, 265);
            this.pnlTest.TabIndex = 7;
            // 
            // ofdImport
            // 
            this.ofdImport.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            // 
            // tvwScores
            // 
            this.tvwScores.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.tvwScores.CheckBoxes = true;
            this.tvwScores.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.tvwScores.FullRowSelect = true;
            this.tvwScores.HideSelection = false;
            this.tvwScores.LabelEdit = true;
            this.tvwScores.Location = new System.Drawing.Point(0, 28);
            this.tvwScores.Name = "tvwScores";
            this.tvwScores.Size = new System.Drawing.Size(212, 533);
            this.tvwScores.TabIndex = 1;
            this.tvwScores.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvwScores_AfterCheck);
            this.tvwScores.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvwScores_AfterLabelEdit);
            this.tvwScores.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwScores_AfterSelect);
            // 
            // ScoreCenterConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 648);
            this.Controls.Add(this.tabScore);
            this.Controls.Add(this.gbxScore);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tvwScores);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ScoreCenterConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "My Score Center Configuration";
            this.Load += new System.EventHandler(this.ScoreCenterConfig_Load);
            this.gbxScore.ResumeLayout(false);
            this.gbxScore.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnOpenUrl;
        private System.Windows.Forms.Label lblEncoding;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn colOperator;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.DataGridViewComboBoxColumn colAction;
        private System.Windows.Forms.DataGridViewComboBoxColumn colStyle;
        private System.Windows.Forms.TextBox tbxElement;
        private System.Windows.Forms.Label lblXpathElement;
        private System.Windows.Forms.TextBox tbxEncoding;
        private System.Windows.Forms.ToolStripButton tsbMoveUp;
        private System.Windows.Forms.ToolStripButton tsbMoveDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Label lblTotalSize;
        private System.Windows.Forms.CheckBox ckxUseTheader;
        private System.Windows.Forms.CheckBox ckxNewLine;
        private System.Windows.Forms.CheckBox ckxAllowWrapping;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem alignementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leftToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem centerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rightToolStripMenuItem;
        private System.Windows.Forms.Panel pnlTest;
        private System.Windows.Forms.CheckBox ckxReverseOrder;
        private System.Windows.Forms.CheckBox ckxUseCaption;
        private System.Windows.Forms.ComboBox cbxBetweenElements;
    }
}