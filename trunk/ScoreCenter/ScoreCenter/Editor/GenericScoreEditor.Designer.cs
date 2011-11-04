﻿namespace MediaPortal.Plugin.ScoreCenter.Editor
{
    partial class GenericScoreEditor
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpgGeneral = new System.Windows.Forms.TabPage();
            this.lblLiveFormat = new System.Windows.Forms.Label();
            this.tbxLiveFormat = new System.Windows.Forms.TextBox();
            this.ckxLive = new System.Windows.Forms.CheckBox();
            this.lblId = new System.Windows.Forms.Label();
            this.tbxScoreId = new System.Windows.Forms.TextBox();
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
            this.lblName = new System.Windows.Forms.Label();
            this.ckxUseCaption = new System.Windows.Forms.CheckBox();
            this.ckxUseTheader = new System.Windows.Forms.CheckBox();
            this.lblMaxLines = new System.Windows.Forms.Label();
            this.lblSkip = new System.Windows.Forms.Label();
            this.tbxSkip = new System.Windows.Forms.TextBox();
            this.tbxMaxLines = new System.Windows.Forms.TextBox();
            this.tbxSizes = new System.Windows.Forms.TextBox();
            this.tbxHeaders = new System.Windows.Forms.TextBox();
            this.tbxXpath = new System.Windows.Forms.TextBox();
            this.tbxUrl = new System.Windows.Forms.TextBox();
            this.lblLive = new System.Windows.Forms.Label();
            this.lblSizes = new System.Windows.Forms.Label();
            this.lblHeaders = new System.Windows.Forms.Label();
            this.lblXpathElement = new System.Windows.Forms.Label();
            this.lblEncoding = new System.Windows.Forms.Label();
            this.lblXPath = new System.Windows.Forms.Label();
            this.lblUrl = new System.Windows.Forms.Label();
            this.tpgRules = new System.Windows.Forms.TabPage();
            this.grdRule = new System.Windows.Forms.DataGridView();
            this.colColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOperator = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAction = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colStyle = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblDictionary = new System.Windows.Forms.Label();
            this.tbxDictionary = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tpgGeneral.SuspendLayout();
            this.tpgRules.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRule)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.tpgGeneral);
            this.tabControl1.Controls.Add(this.tpgRules);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(707, 321);
            this.tabControl1.TabIndex = 0;
            // 
            // tpgGeneral
            // 
            this.tpgGeneral.Controls.Add(this.tbxDictionary);
            this.tpgGeneral.Controls.Add(this.lblDictionary);
            this.tpgGeneral.Controls.Add(this.lblLiveFormat);
            this.tpgGeneral.Controls.Add(this.tbxLiveFormat);
            this.tpgGeneral.Controls.Add(this.ckxLive);
            this.tpgGeneral.Controls.Add(this.lblId);
            this.tpgGeneral.Controls.Add(this.tbxScoreId);
            this.tpgGeneral.Controls.Add(this.cbxBetweenElements);
            this.tpgGeneral.Controls.Add(this.ckxReverseOrder);
            this.tpgGeneral.Controls.Add(this.ckxAllowWrapping);
            this.tpgGeneral.Controls.Add(this.ckxNewLine);
            this.tpgGeneral.Controls.Add(this.lblTotalSize);
            this.tpgGeneral.Controls.Add(this.tbxEncoding);
            this.tpgGeneral.Controls.Add(this.tbxElement);
            this.tpgGeneral.Controls.Add(this.btnOpenUrl);
            this.tpgGeneral.Controls.Add(this.btnAuto);
            this.tpgGeneral.Controls.Add(this.tbxScore);
            this.tpgGeneral.Controls.Add(this.lblName);
            this.tpgGeneral.Controls.Add(this.ckxUseCaption);
            this.tpgGeneral.Controls.Add(this.ckxUseTheader);
            this.tpgGeneral.Controls.Add(this.lblMaxLines);
            this.tpgGeneral.Controls.Add(this.lblSkip);
            this.tpgGeneral.Controls.Add(this.tbxSkip);
            this.tpgGeneral.Controls.Add(this.tbxMaxLines);
            this.tpgGeneral.Controls.Add(this.tbxSizes);
            this.tpgGeneral.Controls.Add(this.tbxHeaders);
            this.tpgGeneral.Controls.Add(this.tbxXpath);
            this.tpgGeneral.Controls.Add(this.tbxUrl);
            this.tpgGeneral.Controls.Add(this.lblLive);
            this.tpgGeneral.Controls.Add(this.lblSizes);
            this.tpgGeneral.Controls.Add(this.lblHeaders);
            this.tpgGeneral.Controls.Add(this.lblXpathElement);
            this.tpgGeneral.Controls.Add(this.lblEncoding);
            this.tpgGeneral.Controls.Add(this.lblXPath);
            this.tpgGeneral.Controls.Add(this.lblUrl);
            this.tpgGeneral.Location = new System.Drawing.Point(4, 4);
            this.tpgGeneral.Name = "tpgGeneral";
            this.tpgGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpgGeneral.Size = new System.Drawing.Size(699, 295);
            this.tpgGeneral.TabIndex = 0;
            this.tpgGeneral.Text = "General";
            // 
            // lblLiveFormat
            // 
            this.lblLiveFormat.AutoSize = true;
            this.lblLiveFormat.Location = new System.Drawing.Point(93, 232);
            this.lblLiveFormat.Name = "lblLiveFormat";
            this.lblLiveFormat.Size = new System.Drawing.Size(64, 13);
            this.lblLiveFormat.TabIndex = 27;
            this.lblLiveFormat.Text = "Live Display";
            // 
            // tbxLiveFormat
            // 
            this.tbxLiveFormat.Location = new System.Drawing.Point(163, 229);
            this.tbxLiveFormat.Name = "tbxLiveFormat";
            this.tbxLiveFormat.Size = new System.Drawing.Size(370, 20);
            this.tbxLiveFormat.TabIndex = 28;
            this.toolTip1.SetToolTip(this.tbxLiveFormat, "Configure the live display");
            // 
            // ckxLive
            // 
            this.ckxLive.AutoSize = true;
            this.ckxLive.Location = new System.Drawing.Point(72, 232);
            this.ckxLive.Name = "ckxLive";
            this.ckxLive.Size = new System.Drawing.Size(15, 14);
            this.ckxLive.TabIndex = 26;
            this.toolTip1.SetToolTip(this.ckxLive, "Set as live");
            this.ckxLive.UseVisualStyleBackColor = true;
            this.ckxLive.CheckedChanged += new System.EventHandler(this.ckxLive_CheckedChanged);
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblId.Location = new System.Drawing.Point(49, 8);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(16, 13);
            this.lblId.TabIndex = 0;
            this.lblId.Text = "Id";
            // 
            // tbxScoreId
            // 
            this.tbxScoreId.HideSelection = false;
            this.tbxScoreId.Location = new System.Drawing.Point(72, 5);
            this.tbxScoreId.Name = "tbxScoreId";
            this.tbxScoreId.ReadOnly = true;
            this.tbxScoreId.Size = new System.Drawing.Size(301, 20);
            this.tbxScoreId.TabIndex = 1;
            // 
            // cbxBetweenElements
            // 
            this.cbxBetweenElements.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBetweenElements.FormattingEnabled = true;
            this.cbxBetweenElements.Location = new System.Drawing.Point(391, 110);
            this.cbxBetweenElements.Name = "cbxBetweenElements";
            this.cbxBetweenElements.Size = new System.Drawing.Size(142, 21);
            this.cbxBetweenElements.TabIndex = 13;
            this.toolTip1.SetToolTip(this.cbxBetweenElements, "Action between each elements");
            // 
            // ckxReverseOrder
            // 
            this.ckxReverseOrder.AutoSize = true;
            this.ckxReverseOrder.Location = new System.Drawing.Point(372, 209);
            this.ckxReverseOrder.Name = "ckxReverseOrder";
            this.ckxReverseOrder.Size = new System.Drawing.Size(138, 17);
            this.ckxReverseOrder.TabIndex = 24;
            this.ckxReverseOrder.Text = "Reverse Columns Order";
            this.ckxReverseOrder.UseVisualStyleBackColor = true;
            // 
            // ckxAllowWrapping
            // 
            this.ckxAllowWrapping.AutoSize = true;
            this.ckxAllowWrapping.Location = new System.Drawing.Point(372, 186);
            this.ckxAllowWrapping.Name = "ckxAllowWrapping";
            this.ckxAllowWrapping.Size = new System.Drawing.Size(81, 17);
            this.ckxAllowWrapping.TabIndex = 22;
            this.ckxAllowWrapping.Text = "Word Wrap";
            this.ckxAllowWrapping.UseVisualStyleBackColor = true;
            // 
            // ckxNewLine
            // 
            this.ckxNewLine.AutoSize = true;
            this.ckxNewLine.Location = new System.Drawing.Point(258, 186);
            this.ckxNewLine.Name = "ckxNewLine";
            this.ckxNewLine.Size = new System.Drawing.Size(99, 17);
            this.ckxNewLine.TabIndex = 21;
            this.ckxNewLine.Text = "Allow New Line";
            this.ckxNewLine.UseVisualStyleBackColor = true;
            // 
            // lblTotalSize
            // 
            this.lblTotalSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblTotalSize.Location = new System.Drawing.Point(383, 187);
            this.lblTotalSize.Name = "lblTotalSize";
            this.lblTotalSize.Size = new System.Drawing.Size(151, 23);
            this.lblTotalSize.TabIndex = 23;
            this.lblTotalSize.Text = "Total";
            this.lblTotalSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbxEncoding
            // 
            this.tbxEncoding.Location = new System.Drawing.Point(72, 110);
            this.tbxEncoding.MaxLength = 20;
            this.tbxEncoding.Name = "tbxEncoding";
            this.tbxEncoding.Size = new System.Drawing.Size(119, 20);
            this.tbxEncoding.TabIndex = 10;
            this.toolTip1.SetToolTip(this.tbxEncoding, "Web page encoding");
            // 
            // tbxElement
            // 
            this.tbxElement.Location = new System.Drawing.Point(284, 110);
            this.tbxElement.Name = "tbxElement";
            this.tbxElement.Size = new System.Drawing.Size(100, 20);
            this.tbxElement.TabIndex = 12;
            this.toolTip1.SetToolTip(this.tbxElement, "XPath elements to keep (separated by \';\')");
            // 
            // btnOpenUrl
            // 
            this.btnOpenUrl.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.Web;
            this.btnOpenUrl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOpenUrl.Location = new System.Drawing.Point(533, 55);
            this.btnOpenUrl.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.btnOpenUrl.Name = "btnOpenUrl";
            this.btnOpenUrl.Size = new System.Drawing.Size(25, 25);
            this.btnOpenUrl.TabIndex = 6;
            this.toolTip1.SetToolTip(this.btnOpenUrl, "Open in a browser");
            this.btnOpenUrl.UseVisualStyleBackColor = true;
            this.btnOpenUrl.Click += new System.EventHandler(this.btnOpenUrl_Click);
            // 
            // btnAuto
            // 
            this.btnAuto.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAuto.Location = new System.Drawing.Point(540, 161);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(75, 23);
            this.btnAuto.TabIndex = 18;
            this.btnAuto.Text = "Auto";
            this.toolTip1.SetToolTip(this.btnAuto, "Auto calculate the columns width");
            this.btnAuto.UseVisualStyleBackColor = true;
            this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
            // 
            // tbxScore
            // 
            this.tbxScore.Location = new System.Drawing.Point(72, 31);
            this.tbxScore.MaxLength = 50;
            this.tbxScore.Name = "tbxScore";
            this.tbxScore.Size = new System.Drawing.Size(461, 20);
            this.tbxScore.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblName.Location = new System.Drawing.Point(30, 34);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name";
            // 
            // ckxUseCaption
            // 
            this.ckxUseCaption.AutoSize = true;
            this.ckxUseCaption.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ckxUseCaption.Location = new System.Drawing.Point(72, 209);
            this.ckxUseCaption.Name = "ckxUseCaption";
            this.ckxUseCaption.Size = new System.Drawing.Size(100, 17);
            this.ckxUseCaption.TabIndex = 20;
            this.ckxUseCaption.Text = "Include Caption";
            this.ckxUseCaption.UseVisualStyleBackColor = true;
            // 
            // ckxUseTheader
            // 
            this.ckxUseTheader.AutoSize = true;
            this.ckxUseTheader.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ckxUseTheader.Location = new System.Drawing.Point(72, 187);
            this.ckxUseTheader.Name = "ckxUseTheader";
            this.ckxUseTheader.Size = new System.Drawing.Size(164, 17);
            this.ckxUseTheader.TabIndex = 19;
            this.ckxUseTheader.Text = "Include Header/Footer Rows";
            this.ckxUseTheader.UseVisualStyleBackColor = true;
            // 
            // lblMaxLines
            // 
            this.lblMaxLines.AutoSize = true;
            this.lblMaxLines.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMaxLines.Location = new System.Drawing.Point(567, 86);
            this.lblMaxLines.Name = "lblMaxLines";
            this.lblMaxLines.Size = new System.Drawing.Size(55, 13);
            this.lblMaxLines.TabIndex = 33;
            this.lblMaxLines.Text = "Max Lines";
            // 
            // lblSkip
            // 
            this.lblSkip.AutoSize = true;
            this.lblSkip.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSkip.Location = new System.Drawing.Point(587, 60);
            this.lblSkip.Name = "lblSkip";
            this.lblSkip.Size = new System.Drawing.Size(28, 13);
            this.lblSkip.TabIndex = 31;
            this.lblSkip.Text = "Skip";
            // 
            // tbxSkip
            // 
            this.tbxSkip.Location = new System.Drawing.Point(628, 57);
            this.tbxSkip.MaxLength = 3;
            this.tbxSkip.Name = "tbxSkip";
            this.tbxSkip.Size = new System.Drawing.Size(45, 20);
            this.tbxSkip.TabIndex = 32;
            this.toolTip1.SetToolTip(this.tbxSkip, "Number of line to skip");
            // 
            // tbxMaxLines
            // 
            this.tbxMaxLines.Location = new System.Drawing.Point(628, 83);
            this.tbxMaxLines.MaxLength = 3;
            this.tbxMaxLines.Name = "tbxMaxLines";
            this.tbxMaxLines.Size = new System.Drawing.Size(45, 20);
            this.tbxMaxLines.TabIndex = 34;
            this.toolTip1.SetToolTip(this.tbxMaxLines, "Maximum number of lines to keep");
            // 
            // tbxSizes
            // 
            this.tbxSizes.Location = new System.Drawing.Point(72, 163);
            this.tbxSizes.MaxLength = 100;
            this.tbxSizes.Name = "tbxSizes";
            this.tbxSizes.Size = new System.Drawing.Size(462, 20);
            this.tbxSizes.TabIndex = 17;
            this.toolTip1.SetToolTip(this.tbxSizes, "Columns sizes");
            this.tbxSizes.TextChanged += new System.EventHandler(this.tbxSizes_TextChanged);
            // 
            // tbxHeaders
            // 
            this.tbxHeaders.Location = new System.Drawing.Point(72, 137);
            this.tbxHeaders.MaxLength = 200;
            this.tbxHeaders.Name = "tbxHeaders";
            this.tbxHeaders.Size = new System.Drawing.Size(462, 20);
            this.tbxHeaders.TabIndex = 15;
            this.toolTip1.SetToolTip(this.tbxHeaders, "Headers to display on first line");
            // 
            // tbxXpath
            // 
            this.tbxXpath.Location = new System.Drawing.Point(71, 83);
            this.tbxXpath.MaxLength = 200;
            this.tbxXpath.Name = "tbxXpath";
            this.tbxXpath.Size = new System.Drawing.Size(462, 20);
            this.tbxXpath.TabIndex = 8;
            this.toolTip1.SetToolTip(this.tbxXpath, "XPath expression to find the score");
            // 
            // tbxUrl
            // 
            this.tbxUrl.Location = new System.Drawing.Point(71, 57);
            this.tbxUrl.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.tbxUrl.MaxLength = 200;
            this.tbxUrl.Name = "tbxUrl";
            this.tbxUrl.Size = new System.Drawing.Size(462, 20);
            this.tbxUrl.TabIndex = 5;
            // 
            // lblLive
            // 
            this.lblLive.AutoSize = true;
            this.lblLive.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblLive.Location = new System.Drawing.Point(33, 232);
            this.lblLive.Name = "lblLive";
            this.lblLive.Size = new System.Drawing.Size(27, 13);
            this.lblLive.TabIndex = 25;
            this.lblLive.Text = "Live";
            // 
            // lblSizes
            // 
            this.lblSizes.AutoSize = true;
            this.lblSizes.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSizes.Location = new System.Drawing.Point(34, 166);
            this.lblSizes.Name = "lblSizes";
            this.lblSizes.Size = new System.Drawing.Size(32, 13);
            this.lblSizes.TabIndex = 16;
            this.lblSizes.Text = "Sizes";
            // 
            // lblHeaders
            // 
            this.lblHeaders.AutoSize = true;
            this.lblHeaders.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblHeaders.Location = new System.Drawing.Point(19, 140);
            this.lblHeaders.Name = "lblHeaders";
            this.lblHeaders.Size = new System.Drawing.Size(47, 13);
            this.lblHeaders.TabIndex = 14;
            this.lblHeaders.Text = "Headers";
            // 
            // lblXpathElement
            // 
            this.lblXpathElement.AutoSize = true;
            this.lblXpathElement.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblXpathElement.Location = new System.Drawing.Point(197, 113);
            this.lblXpathElement.Name = "lblXpathElement";
            this.lblXpathElement.Size = new System.Drawing.Size(82, 13);
            this.lblXpathElement.TabIndex = 11;
            this.lblXpathElement.Text = "XPath Elements";
            // 
            // lblEncoding
            // 
            this.lblEncoding.AutoSize = true;
            this.lblEncoding.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblEncoding.Location = new System.Drawing.Point(19, 113);
            this.lblEncoding.Name = "lblEncoding";
            this.lblEncoding.Size = new System.Drawing.Size(52, 13);
            this.lblEncoding.TabIndex = 9;
            this.lblEncoding.Text = "Encoding";
            // 
            // lblXPath
            // 
            this.lblXPath.AutoSize = true;
            this.lblXPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblXPath.Location = new System.Drawing.Point(30, 86);
            this.lblXPath.Name = "lblXPath";
            this.lblXPath.Size = new System.Drawing.Size(36, 13);
            this.lblXPath.TabIndex = 7;
            this.lblXPath.Text = "XPath";
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblUrl.Location = new System.Drawing.Point(37, 60);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(29, 13);
            this.lblUrl.TabIndex = 4;
            this.lblUrl.Text = "URL";
            // 
            // tpgRules
            // 
            this.tpgRules.Controls.Add(this.grdRule);
            this.tpgRules.Location = new System.Drawing.Point(4, 4);
            this.tpgRules.Name = "tpgRules";
            this.tpgRules.Padding = new System.Windows.Forms.Padding(3);
            this.tpgRules.Size = new System.Drawing.Size(699, 295);
            this.tpgRules.TabIndex = 1;
            this.tpgRules.Text = "Rules";
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
            this.grdRule.Size = new System.Drawing.Size(693, 289);
            this.grdRule.TabIndex = 1;
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
            // lblDictionary
            // 
            this.lblDictionary.AutoSize = true;
            this.lblDictionary.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDictionary.Location = new System.Drawing.Point(11, 258);
            this.lblDictionary.Name = "lblDictionary";
            this.lblDictionary.Size = new System.Drawing.Size(54, 13);
            this.lblDictionary.TabIndex = 29;
            this.lblDictionary.Text = "Dictionary";
            // 
            // tbxDictionary
            // 
            this.tbxDictionary.Location = new System.Drawing.Point(72, 255);
            this.tbxDictionary.Name = "tbxDictionary";
            this.tbxDictionary.Size = new System.Drawing.Size(119, 20);
            this.tbxDictionary.TabIndex = 30;
            this.toolTip1.SetToolTip(this.tbxDictionary, "Configure the live display");
            // 
            // GenericScoreEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "GenericScoreEditor";
            this.Size = new System.Drawing.Size(707, 321);
            this.Load += new System.EventHandler(this.GenericScoreEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tpgGeneral.ResumeLayout(false);
            this.tpgGeneral.PerformLayout();
            this.tpgRules.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdRule)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpgGeneral;
        private System.Windows.Forms.TabPage tpgRules;
        private System.Windows.Forms.ComboBox cbxBetweenElements;
        private System.Windows.Forms.CheckBox ckxReverseOrder;
        private System.Windows.Forms.CheckBox ckxAllowWrapping;
        private System.Windows.Forms.CheckBox ckxNewLine;
        private System.Windows.Forms.Label lblTotalSize;
        private System.Windows.Forms.TextBox tbxEncoding;
        private System.Windows.Forms.TextBox tbxElement;
        private System.Windows.Forms.Button btnOpenUrl;
        private System.Windows.Forms.Button btnAuto;
        private System.Windows.Forms.TextBox tbxScore;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox ckxUseCaption;
        private System.Windows.Forms.CheckBox ckxUseTheader;
        private System.Windows.Forms.Label lblMaxLines;
        private System.Windows.Forms.Label lblSkip;
        private System.Windows.Forms.TextBox tbxSkip;
        private System.Windows.Forms.TextBox tbxMaxLines;
        private System.Windows.Forms.TextBox tbxSizes;
        private System.Windows.Forms.TextBox tbxHeaders;
        private System.Windows.Forms.TextBox tbxXpath;
        private System.Windows.Forms.TextBox tbxUrl;
        private System.Windows.Forms.Label lblSizes;
        private System.Windows.Forms.Label lblHeaders;
        private System.Windows.Forms.Label lblXpathElement;
        private System.Windows.Forms.Label lblEncoding;
        private System.Windows.Forms.Label lblXPath;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.DataGridView grdRule;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn colOperator;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.DataGridViewComboBoxColumn colAction;
        private System.Windows.Forms.DataGridViewComboBoxColumn colStyle;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.TextBox tbxScoreId;
        private System.Windows.Forms.TextBox tbxLiveFormat;
        private System.Windows.Forms.CheckBox ckxLive;
        private System.Windows.Forms.Label lblLive;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblLiveFormat;
        private System.Windows.Forms.TextBox tbxDictionary;
        private System.Windows.Forms.Label lblDictionary;
    }
}