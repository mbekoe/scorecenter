namespace MediaPortal.Plugin.ScoreCenter
{
    partial class StyleControl
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
            this.tbxStyleName = new System.Windows.Forms.TextBox();
            this.pnlStyle = new System.Windows.Forms.Panel();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.btnDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbxStyleName
            // 
            this.tbxStyleName.Location = new System.Drawing.Point(40, 2);
            this.tbxStyleName.MaxLength = 30;
            this.tbxStyleName.Name = "tbxStyleName";
            this.tbxStyleName.Size = new System.Drawing.Size(149, 20);
            this.tbxStyleName.TabIndex = 1;
            // 
            // pnlStyle
            // 
            this.pnlStyle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlStyle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlStyle.Location = new System.Drawing.Point(195, 2);
            this.pnlStyle.Name = "pnlStyle";
            this.pnlStyle.Size = new System.Drawing.Size(61, 20);
            this.pnlStyle.TabIndex = 3;
            this.pnlStyle.Click += new System.EventHandler(this.pnlStyle_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::MediaPortal.Plugin.ScoreCenter.Properties.Resources.DeleteHS;
            this.btnDelete.Location = new System.Drawing.Point(0, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(34, 23);
            this.btnDelete.TabIndex = 0;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // StyleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.pnlStyle);
            this.Controls.Add(this.tbxStyleName);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "StyleControl";
            this.Size = new System.Drawing.Size(259, 24);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxStyleName;
        private System.Windows.Forms.Panel pnlStyle;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button btnDelete;
    }
}
