namespace MediaPortal.Plugin.ScoreCenter
{
    partial class CreateScoreDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateScoreDlg));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxType = new System.Windows.Forms.ComboBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbnNone = new System.Windows.Forms.RadioButton();
            this.rbnParent = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.cbxType);
            this.groupBox1.Controls.Add(this.tbxName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbxName
            // 
            resources.ApplyResources(this.tbxName, "tbxName");
            this.tbxName.Name = "tbxName";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cbxType
            // 
            this.cbxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxType.FormattingEnabled = true;
            resources.ApplyResources(this.cbxType, "cbxType");
            this.cbxType.Name = "cbxType";
            // 
            // btnCreate
            // 
            resources.ApplyResources(this.btnCreate, "btnCreate");
            this.btnCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.rbnParent);
            this.groupBox2.Controls.Add(this.rbnNone);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // rbnNone
            // 
            resources.ApplyResources(this.rbnNone, "rbnNone");
            this.rbnNone.Name = "rbnNone";
            this.rbnNone.TabStop = true;
            this.rbnNone.UseVisualStyleBackColor = true;
            // 
            // rbnParent
            // 
            resources.ApplyResources(this.rbnParent, "rbnParent");
            this.rbnParent.Name = "rbnParent";
            this.rbnParent.TabStop = true;
            this.rbnParent.UseVisualStyleBackColor = true;
            // 
            // CreateScoreDlg
            // 
            this.AcceptButton = this.btnCreate;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.groupBox1);
            this.Name = "CreateScoreDlg";
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CreateScoreDlg_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbxName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxType;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbnParent;
        private System.Windows.Forms.RadioButton rbnNone;
    }
}