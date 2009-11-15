namespace MediaPortal.Plugin.ScoreCenter
{
    partial class StyleDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StyleDialog));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSkinColor = new System.Windows.Forms.Label();
            this.pnlSkinColor = new System.Windows.Forms.Panel();
            this.btnSetSkinColor = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.lblFontColor = new System.Windows.Forms.Label();
            this.pnlFontColor = new System.Windows.Forms.Panel();
            this.btnSetFontColor = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // lblSkinColor
            // 
            resources.ApplyResources(this.lblSkinColor, "lblSkinColor");
            this.lblSkinColor.Name = "lblSkinColor";
            // 
            // pnlSkinColor
            // 
            this.pnlSkinColor.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSkinColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pnlSkinColor, "pnlSkinColor");
            this.pnlSkinColor.Name = "pnlSkinColor";
            // 
            // btnSetSkinColor
            // 
            resources.ApplyResources(this.btnSetSkinColor, "btnSetSkinColor");
            this.btnSetSkinColor.Name = "btnSetSkinColor";
            this.toolTip1.SetToolTip(this.btnSetSkinColor, resources.GetString("btnSetSkinColor.ToolTip"));
            this.btnSetSkinColor.UseVisualStyleBackColor = true;
            this.btnSetSkinColor.Click += new System.EventHandler(this.btnSetSkinColor_Click);
            // 
            // lblFontColor
            // 
            resources.ApplyResources(this.lblFontColor, "lblFontColor");
            this.lblFontColor.Name = "lblFontColor";
            // 
            // pnlFontColor
            // 
            this.pnlFontColor.BackColor = System.Drawing.SystemColors.Window;
            this.pnlFontColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pnlFontColor, "pnlFontColor");
            this.pnlFontColor.Name = "pnlFontColor";
            // 
            // btnSetFontColor
            // 
            resources.ApplyResources(this.btnSetFontColor, "btnSetFontColor");
            this.btnSetFontColor.Name = "btnSetFontColor";
            this.toolTip1.SetToolTip(this.btnSetFontColor, resources.GetString("btnSetFontColor.ToolTip"));
            this.btnSetFontColor.UseVisualStyleBackColor = true;
            this.btnSetFontColor.Click += new System.EventHandler(this.btnSetSkinColor_Click);
            // 
            // StyleDialog
            // 
            this.AcceptButton = this.btnOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.btnSetFontColor);
            this.Controls.Add(this.pnlFontColor);
            this.Controls.Add(this.btnSetSkinColor);
            this.Controls.Add(this.lblFontColor);
            this.Controls.Add(this.pnlSkinColor);
            this.Controls.Add(this.lblSkinColor);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StyleDialog";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.StyleDialog_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StyleDialog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label lblSkinColor;
        private System.Windows.Forms.Panel pnlSkinColor;
        private System.Windows.Forms.Button btnSetSkinColor;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label lblFontColor;
        private System.Windows.Forms.Panel pnlFontColor;
        private System.Windows.Forms.Button btnSetFontColor;
    }
}