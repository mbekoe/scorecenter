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
            this.ckxReload = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnClearIcon = new System.Windows.Forms.Button();
            this.btnSetIcon = new System.Windows.Forms.Button();
            this.pbxIcon = new System.Windows.Forms.PictureBox();
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
            this.tsbMoveBack = new System.Windows.Forms.ToolStripButton();
            this.tsbMoveRight = new System.Windows.Forms.ToolStripButton();
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
            this.pnlTest = new System.Windows.Forms.Panel();
            this.ofdImport = new System.Windows.Forms.OpenFileDialog();
            this.pnlEditor = new System.Windows.Forms.Panel();
            this.tvwScores = new MediaPortal.Plugin.ScoreCenter.ThreeStateTreeView();
            ((System.ComponentModel.ISupportInitialize)(this.pbxIcon)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ckxReload
            // 
            this.ckxReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ckxReload.AutoSize = true;
            this.ckxReload.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ckxReload.Location = new System.Drawing.Point(384, 682);
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
            this.btnSave.Location = new System.Drawing.Point(222, 678);
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
            this.btnTest.Location = new System.Drawing.Point(303, 678);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 29;
            this.btnTest.Text = "&Test";
            this.toolTip1.SetToolTip(this.btnTest, "Test your settings");
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
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
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(858, 678);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOk.Location = new System.Drawing.Point(777, 678);
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
            this.tsbMoveBack,
            this.tsbMoveRight,
            this.tsbEditStyles,
            this.toolStripSeparator1,
            this.tsbExport,
            this.tsbImport,
            this.tsbOptions});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(945, 25);
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
            this.tsbNewLigue.Click += new System.EventHandler(this.tsbNewItem_Click);
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
            // tsbMoveBack
            // 
            this.tsbMoveBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMoveBack.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.BuilderDialog_moveback;
            this.tsbMoveBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMoveBack.Name = "tsbMoveBack";
            this.tsbMoveBack.Size = new System.Drawing.Size(23, 22);
            this.tsbMoveBack.Text = "Move Back";
            this.tsbMoveBack.Click += new System.EventHandler(this.tsbMoveBack_Click);
            // 
            // tsbMoveRight
            // 
            this.tsbMoveRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMoveRight.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.BuilderDialog_moveright;
            this.tsbMoveRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMoveRight.Name = "tsbMoveRight";
            this.tsbMoveRight.Size = new System.Drawing.Size(23, 22);
            this.tsbMoveRight.Text = "Move Right";
            this.tsbMoveRight.ToolTipText = "Move Right";
            this.tsbMoveRight.Click += new System.EventHandler(this.tsbMoveRight_Click);
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
            this.panel1.Location = new System.Drawing.Point(0, 632);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(212, 81);
            this.panel1.TabIndex = 4;
            // 
            // pnlTest
            // 
            this.pnlTest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTest.AutoScroll = true;
            this.pnlTest.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlTest.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlTest.Location = new System.Drawing.Point(218, 342);
            this.pnlTest.Name = "pnlTest";
            this.pnlTest.Size = new System.Drawing.Size(715, 330);
            this.pnlTest.TabIndex = 7;
            // 
            // ofdImport
            // 
            this.ofdImport.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            // 
            // pnlEditor
            // 
            this.pnlEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlEditor.Location = new System.Drawing.Point(218, 28);
            this.pnlEditor.Name = "pnlEditor";
            this.pnlEditor.Size = new System.Drawing.Size(715, 308);
            this.pnlEditor.TabIndex = 31;
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
            this.tvwScores.Size = new System.Drawing.Size(212, 598);
            this.tvwScores.TabIndex = 1;
            this.tvwScores.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvwScores_AfterCheck);
            this.tvwScores.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvwScores_AfterLabelEdit);
            this.tvwScores.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwScores_AfterSelect);
            // 
            // ScoreCenterConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 713);
            this.Controls.Add(this.pnlEditor);
            this.Controls.Add(this.pnlTest);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tvwScores);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.ckxReload);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ScoreCenterConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "My Score Center Configuration";
            this.Load += new System.EventHandler(this.ScoreCenterConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbxIcon)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ThreeStateTreeView tvwScores;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbNewLigue;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.ToolStripButton tsbAbout;
        private System.Windows.Forms.CheckBox ckxReload;
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
        private System.Windows.Forms.ToolStripButton tsbEditStyles;
        private System.Windows.Forms.OpenFileDialog ofdImport;
        private System.Windows.Forms.ToolStripButton tsbOptions;
        private System.Windows.Forms.ToolStripButton tsbMoveUp;
        private System.Windows.Forms.ToolStripButton tsbMoveDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem alignementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leftToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem centerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rightToolStripMenuItem;
        private System.Windows.Forms.Panel pnlTest;
        private System.Windows.Forms.ToolStripButton tsbMoveBack;
        private System.Windows.Forms.ToolStripButton tsbMoveRight;
        private System.Windows.Forms.Panel pnlEditor;
    }
}